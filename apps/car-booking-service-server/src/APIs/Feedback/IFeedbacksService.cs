using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;

namespace CarBookingService.APIs;

public interface IFeedbacksService
{
    /// <summary>
    /// Create one feedback
    /// </summary>
    public Task<Feedback> CreateFeedback(FeedbackCreateInput feedback);

    /// <summary>
    /// Delete one feedback
    /// </summary>
    public Task DeleteFeedback(FeedbackWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many feedbacks
    /// </summary>
    public Task<List<Feedback>> Feedbacks(FeedbackFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about feedback records
    /// </summary>
    public Task<MetadataDto> FeedbacksMeta(FeedbackFindManyArgs findManyArgs);

    /// <summary>
    /// Get one feedback
    /// </summary>
    public Task<Feedback> Feedback(FeedbackWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one feedback
    /// </summary>
    public Task UpdateFeedback(FeedbackWhereUniqueInput uniqueId, FeedbackUpdateInput updateDto);

    /// <summary>
    /// Get a customer record for feedback
    /// </summary>
    public Task<Customer> GetCustomer(FeedbackWhereUniqueInput uniqueId);
}
