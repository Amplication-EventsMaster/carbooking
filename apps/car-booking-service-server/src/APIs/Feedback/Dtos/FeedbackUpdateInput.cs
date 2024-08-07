namespace CarBookingService.APIs.Dtos;

public class FeedbackUpdateInput
{
    public string? Comments { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Customer { get; set; }

    public DateTime? Date { get; set; }

    public string? Id { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
