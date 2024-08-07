using CarBookingService.Infrastructure;

namespace CarBookingService.APIs;

public class FeedbacksService : FeedbacksServiceBase
{
    public FeedbacksService(CarBookingServiceDbContext context)
        : base(context) { }
}
