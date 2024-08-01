using CarBookingService.APIs;

namespace CarBookingService;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IBookingsService, BookingsService>();
        services.AddScoped<ICarsService, CarsService>();
        services.AddScoped<ICustomersService, CustomersService>();
        services.AddScoped<IPaymentsService, PaymentsService>();
    }
}
