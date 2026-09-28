using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public sealed class RegistrationFormModel
{
    private string _name = string.Empty;
    private string _email = string.Empty;

    [Required, StringLength(80, MinimumLength = 2)]
    public string Name
    {
        get => _name;
        set => _name = value?.Trim() ?? string.Empty;
    }

    [Required, EmailAddress, StringLength(254)]
    public string Email
    {
        get => _email;
        set => _email = value?.Trim() ?? string.Empty;
    }
}