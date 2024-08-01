namespace CarBookingService.APIs.Dtos;

public class BookingUpdateInput
{
    public string? Car { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Customer { get; set; }

    public DateTime? Date { get; set; }

    public string? Id { get; set; }

    public List<string>? Payments { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
