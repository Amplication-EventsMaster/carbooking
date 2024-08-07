using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class FeedbacksServiceBase : IFeedbacksService
{
    protected readonly CarBookingServiceDbContext _context;

    public FeedbacksServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one feedback
    /// </summary>
    public async Task<Feedback> CreateFeedback(FeedbackCreateInput createDto)
    {
        var feedback = new FeedbackDbModel
        {
            Comments = createDto.Comments,
            CreatedAt = createDto.CreatedAt,
            Date = createDto.Date,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            feedback.Id = createDto.Id;
        }
        if (createDto.Customer != null)
        {
            feedback.Customer = await _context
                .Customers.Where(customer => createDto.Customer.Id == customer.Id)
                .FirstOrDefaultAsync();
        }

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<FeedbackDbModel>(feedback.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one feedback
    /// </summary>
    public async Task DeleteFeedback(FeedbackWhereUniqueInput uniqueId)
    {
        var feedback = await _context.Feedbacks.FindAsync(uniqueId.Id);
        if (feedback == null)
        {
            throw new NotFoundException();
        }

        _context.Feedbacks.Remove(feedback);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many feedbacks
    /// </summary>
    public async Task<List<Feedback>> Feedbacks(FeedbackFindManyArgs findManyArgs)
    {
        var feedbacks = await _context
            .Feedbacks.Include(x => x.Customer)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return feedbacks.ConvertAll(feedback => feedback.ToDto());
    }

    /// <summary>
    /// Meta data about feedback records
    /// </summary>
    public async Task<MetadataDto> FeedbacksMeta(FeedbackFindManyArgs findManyArgs)
    {
        var count = await _context.Feedbacks.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one feedback
    /// </summary>
    public async Task<Feedback> Feedback(FeedbackWhereUniqueInput uniqueId)
    {
        var feedbacks = await this.Feedbacks(
            new FeedbackFindManyArgs { Where = new FeedbackWhereInput { Id = uniqueId.Id } }
        );
        var feedback = feedbacks.FirstOrDefault();
        if (feedback == null)
        {
            throw new NotFoundException();
        }

        return feedback;
    }

    /// <summary>
    /// Update one feedback
    /// </summary>
    public async Task UpdateFeedback(
        FeedbackWhereUniqueInput uniqueId,
        FeedbackUpdateInput updateDto
    )
    {
        var feedback = updateDto.ToModel(uniqueId);

        _context.Entry(feedback).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Feedbacks.Any(e => e.Id == feedback.Id))
            {
                throw new NotFoundException();
            }
            else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Get a customer record for feedback
    /// </summary>
    public async Task<Customer> GetCustomer(FeedbackWhereUniqueInput uniqueId)
    {
        var feedback = await _context
            .Feedbacks.Where(feedback => feedback.Id == uniqueId.Id)
            .Include(feedback => feedback.Customer)
            .FirstOrDefaultAsync();
        if (feedback == null)
        {
            throw new NotFoundException();
        }
        return feedback.Customer.ToDto();
    }
}
