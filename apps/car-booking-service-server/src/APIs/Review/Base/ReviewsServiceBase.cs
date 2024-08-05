using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class ReviewsServiceBase : IReviewsService
{
    protected readonly CarBookingServiceDbContext _context;

    public ReviewsServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Review
    /// </summary>
    public async Task<Review> CreateReview(ReviewCreateInput createDto)
    {
        var review = new ReviewDbModel
        {
            Comments = createDto.Comments,
            CreatedAt = createDto.CreatedAt,
            Date = createDto.Date,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            review.Id = createDto.Id;
        }
        if (createDto.Customer != null)
        {
            review.Customer = await _context
                .Customers.Where(customer => createDto.Customer.Id == customer.Id)
                .FirstOrDefaultAsync();
        }

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<ReviewDbModel>(review.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Review
    /// </summary>
    public async Task DeleteReview(ReviewWhereUniqueInput uniqueId)
    {
        var review = await _context.Reviews.FindAsync(uniqueId.Id);
        if (review == null)
        {
            throw new NotFoundException();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Reviews
    /// </summary>
    public async Task<List<Review>> Reviews(ReviewFindManyArgs findManyArgs)
    {
        var reviews = await _context
            .Reviews.Include(x => x.Customer)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return reviews.ConvertAll(review => review.ToDto());
    }

    /// <summary>
    /// Meta data about Review records
    /// </summary>
    public async Task<MetadataDto> ReviewsMeta(ReviewFindManyArgs findManyArgs)
    {
        var count = await _context.Reviews.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Review
    /// </summary>
    public async Task<Review> Review(ReviewWhereUniqueInput uniqueId)
    {
        var reviews = await this.Reviews(
            new ReviewFindManyArgs { Where = new ReviewWhereInput { Id = uniqueId.Id } }
        );
        var review = reviews.FirstOrDefault();
        if (review == null)
        {
            throw new NotFoundException();
        }

        return review;
    }

    /// <summary>
    /// Update one Review
    /// </summary>
    public async Task UpdateReview(ReviewWhereUniqueInput uniqueId, ReviewUpdateInput updateDto)
    {
        var review = updateDto.ToModel(uniqueId);

        _context.Entry(review).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Reviews.Any(e => e.Id == review.Id))
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
    /// Get a Customer record for Review
    /// </summary>
    public async Task<Customer> GetCustomer(ReviewWhereUniqueInput uniqueId)
    {
        var review = await _context
            .Reviews.Where(review => review.Id == uniqueId.Id)
            .Include(review => review.Customer)
            .FirstOrDefaultAsync();
        if (review == null)
        {
            throw new NotFoundException();
        }
        return review.Customer.ToDto();
    }
}
