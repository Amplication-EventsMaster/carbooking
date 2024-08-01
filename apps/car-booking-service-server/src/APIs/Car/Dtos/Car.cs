namespace CarBookingService.APIs.Dtos;

public class Car
{
    public List<string>? Bookings { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Id { get; set; }

    public string? Make { get; set; }

    public string? Model { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? Year { get; set; }
}
