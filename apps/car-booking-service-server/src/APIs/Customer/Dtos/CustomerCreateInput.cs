namespace CarBookingService.APIs.Dtos;

public class CustomerCreateInput
{
    public List<Booking>? Bookings { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Email { get; set; }

    public List<Feedback>? Feedbacks { get; set; }

    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Phone_2 { get; set; }

    public List<Review>? Reviews { get; set; }

    public DateTime UpdatedAt { get; set; }
}
