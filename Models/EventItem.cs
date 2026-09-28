using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public sealed class EventItem
{
    public int Id { get; init; }

    [Required, StringLength(80, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Choose an event date."), FutureEventDate]
    public DateTime? Date { get; set; }

    [Required, StringLength(100, MinimumLength = 3)]
    public string Location { get; set; } = string.Empty;

    public string Category { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
}

public sealed class FutureEventDateAttribute : ValidationAttribute
{
    public FutureEventDateAttribute()
    {
        ErrorMessage = "Choose today or a future date.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value is DateTime date && date.Date >= DateTime.Today
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage);
    }
}