using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;

namespace CarBookingService.APIs;

public interface ICustomersService
{
    /// <summary>
    /// Create one Customer
    /// </summary>
    public Task<Customer> CreateCustomer(CustomerCreateInput customer);

    /// <summary>
    /// Delete one Customer
    /// </summary>
    public Task DeleteCustomer(CustomerWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Customers
    /// </summary>
    public Task<List<Customer>> Customers(CustomerFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Customer records
    /// </summary>
    public Task<MetadataDto> CustomersMeta(CustomerFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Customer
    /// </summary>
    public Task<Customer> Customer(CustomerWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Customer
    /// </summary>
    public Task UpdateCustomer(CustomerWhereUniqueInput uniqueId, CustomerUpdateInput updateDto);

    /// <summary>
    /// Connect multiple Bookings records to Customer
    /// </summary>
    public Task ConnectBookings(
        CustomerWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    );

    /// <summary>
    /// Disconnect multiple Bookings records from Customer
    /// </summary>
    public Task DisconnectBookings(
        CustomerWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    );

    /// <summary>
    /// Find multiple Bookings records for Customer
    /// </summary>
    public Task<List<Booking>> FindBookings(
        CustomerWhereUniqueInput uniqueId,
        BookingFindManyArgs BookingFindManyArgs
    );

    /// <summary>
    /// Update multiple Bookings records for Customer
    /// </summary>
    public Task UpdateBookings(
        CustomerWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    );

    /// <summary>
    /// Connect multiple feedbacks records to Customer
    /// </summary>
    public Task ConnectFeedbacks(
        CustomerWhereUniqueInput uniqueId,
        FeedbackWhereUniqueInput[] feedbacksId
    );

    /// <summary>
    /// Disconnect multiple feedbacks records from Customer
    /// </summary>
    public Task DisconnectFeedbacks(
        CustomerWhereUniqueInput uniqueId,
        FeedbackWhereUniqueInput[] feedbacksId
    );

    /// <summary>
    /// Find multiple feedbacks records for Customer
    /// </summary>
    public Task<List<Feedback>> FindFeedbacks(
        CustomerWhereUniqueInput uniqueId,
        FeedbackFindManyArgs FeedbackFindManyArgs
    );

    /// <summary>
    /// Update multiple feedbacks records for Customer
    /// </summary>
    public Task UpdateFeedbacks(
        CustomerWhereUniqueInput uniqueId,
        FeedbackWhereUniqueInput[] feedbacksId
    );

    /// <summary>
    /// Connect multiple Reviews records to Customer
    /// </summary>
    public Task ConnectReviews(
        CustomerWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    );

    /// <summary>
    /// Disconnect multiple Reviews records from Customer
    /// </summary>
    public Task DisconnectReviews(
        CustomerWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    );

    /// <summary>
    /// Find multiple Reviews records for Customer
    /// </summary>
    public Task<List<Review>> FindReviews(
        CustomerWhereUniqueInput uniqueId,
        ReviewFindManyArgs ReviewFindManyArgs
    );

    /// <summary>
    /// Update multiple Reviews records for Customer
    /// </summary>
    public Task UpdateReviews(
        CustomerWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    );
}
