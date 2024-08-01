using CarBookingService.Infrastructure;

namespace CarBookingService.APIs;

public class CustomersService : CustomersServiceBase
{
    public CustomersService(CarBookingServiceDbContext context)
        : base(context) { }
}
