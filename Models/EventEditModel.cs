using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public sealed class EventEditModel
{
    private string _name = string.Empty;
    private string _location = string.Empty;

    [Required, StringLength(80, MinimumLength = 3)]
    public string Name
    {
        get => _name;
        set => _name = value?.Trim() ?? string.Empty;
    }

    [Required(ErrorMessage = "Choose an event date."), FutureEventDate]
    public DateTime? Date { get; set; }

    [Required, StringLength(100, MinimumLength = 3)]
    public string Location
    {
        get => _location;
        set => _location = value?.Trim() ?? string.Empty;
    }

    public static EventEditModel From(EventItem eventItem) => new()
    {
        Name = eventItem.Name,
        Date = eventItem.Date,
        Location = eventItem.Location
    };
}