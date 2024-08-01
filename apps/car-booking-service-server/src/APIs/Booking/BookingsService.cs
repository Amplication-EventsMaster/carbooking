using CarBookingService.Infrastructure;

namespace CarBookingService.APIs;

public class BookingsService : BookingsServiceBase
{
    public BookingsService(CarBookingServiceDbContext context)
        : base(context) { }
}
