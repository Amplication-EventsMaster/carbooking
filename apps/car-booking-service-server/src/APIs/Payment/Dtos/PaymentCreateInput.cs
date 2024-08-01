namespace CarBookingService.APIs.Dtos;

public class PaymentCreateInput
{
    public double? Amount { get; set; }

    public Booking? Booking { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? Date { get; set; }

    public string? Id { get; set; }

    public DateTime UpdatedAt { get; set; }
}
