using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.Infrastructure;

public class CarBookingServiceDbContext : DbContext
{
    public CarBookingServiceDbContext(DbContextOptions<CarBookingServiceDbContext> options)
        : base(options) { }

    public DbSet<CustomerDbModel> Customers { get; set; }

    public DbSet<CarDbModel> Cars { get; set; }

    public DbSet<PaymentDbModel> Payments { get; set; }

    public DbSet<BookingDbModel> Bookings { get; set; }

    public DbSet<FeedbackDbModel> Feedbacks { get; set; }
}
