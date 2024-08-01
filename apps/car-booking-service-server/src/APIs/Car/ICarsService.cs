using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;

namespace CarBookingService.APIs;

public interface ICarsService
{
    /// <summary>
    /// Create one Car
    /// </summary>
    public Task<Car> CreateCar(CarCreateInput car);

    /// <summary>
    /// Delete one Car
    /// </summary>
    public Task DeleteCar(CarWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Cars
    /// </summary>
    public Task<List<Car>> Cars(CarFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Car records
    /// </summary>
    public Task<MetadataDto> CarsMeta(CarFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Car
    /// </summary>
    public Task<Car> Car(CarWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Car
    /// </summary>
    public Task UpdateCar(CarWhereUniqueInput uniqueId, CarUpdateInput updateDto);

    /// <summary>
    /// Connect multiple Bookings records to Car
    /// </summary>
    public Task ConnectBookings(CarWhereUniqueInput uniqueId, BookingWhereUniqueInput[] bookingsId);

    /// <summary>
    /// Disconnect multiple Bookings records from Car
    /// </summary>
    public Task DisconnectBookings(
        CarWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    );

    /// <summary>
    /// Find multiple Bookings records for Car
    /// </summary>
    public Task<List<Booking>> FindBookings(
        CarWhereUniqueInput uniqueId,
        BookingFindManyArgs BookingFindManyArgs
    );

    /// <summary>
    /// Update multiple Bookings records for Car
    /// </summary>
    public Task UpdateBookings(CarWhereUniqueInput uniqueId, BookingWhereUniqueInput[] bookingsId);
}
