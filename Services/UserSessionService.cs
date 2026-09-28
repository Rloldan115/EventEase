using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using EventEase.Models;
using Microsoft.JSInterop;

namespace EventEase.Services;

public sealed class UserSessionService(IJSRuntime jsRuntime, EventCatalog catalog)
{
    private const string StorageKey = "eventease.session.v1";
    private readonly HashSet<int> _registeredEventIds = [];
    private readonly HashSet<int> _attendedEventIds = [];
    private bool _initialized;

    public string? AttendeeName { get; private set; }
    public string? AttendeeEmail { get; private set; }
    public bool IsInitialized => _initialized;
    public bool IsPersistenceAvailable { get; private set; } = true;
    public int RegisteredCount => _registeredEventIds.Count;
    public int AttendedCount => _attendedEventIds.Count;

    public bool HasRegistration(int eventId) => _registeredEventIds.Contains(eventId);
    public bool HasAttended(int eventId) => _attendedEventIds.Contains(eventId);

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        try
        {
            var json = await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", StorageKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var storedSession = JsonSerializer.Deserialize<StoredSession>(json);
            if (storedSession is null)
            {
                return;
            }

            var profile = new RegistrationFormModel
            {
                Name = storedSession.AttendeeName,
                Email = storedSession.AttendeeEmail
            };

            if (!IsValid(profile))
            {
                await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", StorageKey);
                IsPersistenceAvailable = false;
                return;
            }

            AttendeeName = profile.Name;
            AttendeeEmail = profile.Email;

            foreach (var eventId in storedSession.RegisteredEventIds.Distinct())
            {
                if (catalog.Find(eventId) is not null)
                {
                    _registeredEventIds.Add(eventId);
                }
            }

            foreach (var eventId in storedSession.AttendedEventIds.Distinct())
            {
                if (_registeredEventIds.Contains(eventId))
                {
                    _attendedEventIds.Add(eventId);
                }
            }
        }
        catch (JsonException)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", StorageKey);
            }
            catch (JSException)
            {
                IsPersistenceAvailable = false;
            }
        }
        catch (JSException)
        {
            IsPersistenceAvailable = false;
        }
    }

    public async Task<bool> RegisterAsync(int eventId, RegistrationFormModel form)
    {
        if (!_initialized || catalog.Find(eventId) is null || !IsValid(form))
        {
            return false;
        }

        AttendeeName = form.Name;
        AttendeeEmail = form.Email;
        _registeredEventIds.Add(eventId);
        await PersistAsync();
        return true;
    }

    public async Task<bool> SetAttendanceAsync(int eventId, bool attended)
    {
        if (!_initialized || !_registeredEventIds.Contains(eventId))
        {
            return false;
        }

        if (attended)
        {
            _attendedEventIds.Add(eventId);
        }
        else
        {
            _attendedEventIds.Remove(eventId);
        }

        await PersistAsync();
        return true;
    }

    private async Task PersistAsync()
    {
        try
        {
            var state = new StoredSession
            {
                AttendeeName = AttendeeName ?? string.Empty,
                AttendeeEmail = AttendeeEmail ?? string.Empty,
                RegisteredEventIds = _registeredEventIds.ToList(),
                AttendedEventIds = _attendedEventIds.ToList()
            };

            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", StorageKey, JsonSerializer.Serialize(state));
        }
        catch (JSException)
        {
            IsPersistenceAvailable = false;
        }
    }

    private static bool IsValid(RegistrationFormModel form) =>
        Validator.TryValidateObject(form, new ValidationContext(form), [], validateAllProperties: true);

    private sealed class StoredSession
    {
        public string AttendeeName { get; set; } = string.Empty;
        public string AttendeeEmail { get; set; } = string.Empty;
        public List<int> RegisteredEventIds { get; set; } = [];
        public List<int> AttendedEventIds { get; set; } = [];
    }
}