using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[ApiController()]
public class BookingsController : BookingsControllerBase
{
    public BookingsController(IBookingsService service)
        : base(service) { }
}
