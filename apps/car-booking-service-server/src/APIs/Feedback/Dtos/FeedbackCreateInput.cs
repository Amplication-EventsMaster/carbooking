namespace CarBookingService.APIs.Dtos;

public class FeedbackCreateInput
{
    public string? Comments { get; set; }

    public DateTime CreatedAt { get; set; }

    public Customer? Customer { get; set; }

    public DateTime? Date { get; set; }

    public string? Id { get; set; }

    public DateTime UpdatedAt { get; set; }
}
