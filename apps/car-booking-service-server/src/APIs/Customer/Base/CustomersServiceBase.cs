using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class CustomersServiceBase : ICustomersService
{
    protected readonly CarBookingServiceDbContext _context;

    public CustomersServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Customer
    /// </summary>
    public async Task<Customer> CreateCustomer(CustomerCreateInput createDto)
    {
        var customer = new CustomerDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Email = createDto.Email,
            Name = createDto.Name,
            Phone = createDto.Phone,
            Phone_2 = createDto.Phone_2,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            customer.Id = createDto.Id;
        }
        if (createDto.Bookings != null)
        {
            customer.Bookings = await _context
                .Bookings.Where(booking =>
                    createDto.Bookings.Select(t => t.Id).Contains(booking.Id)
                )
                .ToListAsync();
        }

        if (createDto.Feedbacks != null)
        {
            customer.Feedbacks = await _context
                .Feedbacks.Where(feedback =>
                    createDto.Feedbacks.Select(t => t.Id).Contains(feedback.Id)
                )
                .ToListAsync();
        }

        if (createDto.Reviews != null)
        {
            customer.Reviews = await _context
                .Reviews.Where(review => createDto.Reviews.Select(t => t.Id).Contains(review.Id))
                .ToListAsync();
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<CustomerDbModel>(customer.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Customer
    /// </summary>
    public async Task DeleteCustomer(CustomerWhereUniqueInput uniqueId)
    {
        var customer = await _context.Customers.FindAsync(uniqueId.Id);
        if (customer == null)
        {
            throw new NotFoundException();
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Customers
    /// </summary>
    public async Task<List<Customer>> Customers(CustomerFindManyArgs findManyArgs)
    {
        var customers = await _context
            .Customers.Include(x => x.Bookings)
            .Include(x => x.Feedbacks)
            .Include(x => x.Reviews)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return customers.ConvertAll(customer => customer.ToDto());
    }

    /// <summary>
    /// Meta data about Customer records
    /// </summary>
    public async Task<MetadataDto> CustomersMeta(CustomerFindManyArgs findManyArgs)
    {
        var count = await _context.Customers.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Customer
    /// </summary>
    public async Task<Customer> Customer(CustomerWhereUniqueInput uniqueId)
    {
        var customers = await this.Customers(
            new CustomerFindManyArgs { Where = new CustomerWhereInput { Id = uniqueId.Id } }
        );
        var customer = customers.FirstOrDefault();
        if (customer == null)
        {
            throw new NotFoundException();
        }

        return customer;
    }

    /// <summary>
    /// Update one Customer
    /// </summary>
    public async Task UpdateCustomer(
        CustomerWhereUniqueInput uniqueId,
        CustomerUpdateInput updateDto
    )
    {
        var customer = updateDto.ToModel(uniqueId);

        if (updateDto.Bookings != null)
        {
            customer.Bookings = await _context
                .Bookings.Where(booking => updateDto.Bookings.Select(t => t).Contains(booking.Id))
                .ToListAsync();
        }

        if (updateDto.Feedbacks != null)
        {
            customer.Feedbacks = await _context
                .Feedbacks.Where(feedback =>
                    updateDto.Feedbacks.Select(t => t).Contains(feedback.Id)
                )
                .ToListAsync();
        }

        if (updateDto.Reviews != null)
        {
            customer.Reviews = await _context
                .Reviews.Where(review => updateDto.Reviews.Select(t => t).Contains(review.Id))
                .ToListAsync();
        }

        _context.Entry(customer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Customers.Any(e => e.Id == customer.Id))
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
    /// Connect multiple Bookings records to Customer
    /// </summary>
    public async Task ConnectBookings(
        CustomerWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Bookings)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var bookings = await _context
            .Bookings.Where(t => bookingsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (bookings.Count == 0)
        {
            throw new NotFoundException();
        }

        var bookingsToConnect = bookings.Except(parent.Bookings);

        foreach (var booking in bookingsToConnect)
        {
            parent.Bookings.Add(booking);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Bookings records from Customer
    /// </summary>
    public async Task DisconnectBookings(
        CustomerWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Bookings)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var bookings = await _context
            .Bookings.Where(t => bookingsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var booking in bookings)
        {
            parent.Bookings?.Remove(booking);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Bookings records for Customer
    /// </summary>
    public async Task<List<Booking>> FindBookings(
        CustomerWhereUniqueInput uniqueId,
        BookingFindManyArgs customerFindManyArgs
    )
    {
        var bookings = await _context
            .Bookings.Where(m => m.CustomerId == uniqueId.Id)
            .ApplyWhere(customerFindManyArgs.Where)
            .ApplySkip(customerFindManyArgs.Skip)
            .ApplyTake(customerFindManyArgs.Take)
            .ApplyOrderBy(customerFindManyArgs.SortBy)
            .ToListAsync();

        return bookings.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Bookings records for Customer
    /// </summary>
    public async Task UpdateBookings(
        CustomerWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    )
    {
        var customer = await _context
            .Customers.Include(t => t.Bookings)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (customer == null)
        {
            throw new NotFoundException();
        }

        var bookings = await _context
            .Bookings.Where(a => bookingsId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (bookings.Count == 0)
        {
            throw new NotFoundException();
        }

        customer.Bookings = bookings;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Connect multiple feedbacks records to Customer
    /// </summary>
    public async Task ConnectFeedbacks(
        CustomerWhereUniqueInput uniqueId,
        FeedbackWhereUniqueInput[] feedbacksId
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Feedbacks)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var feedbacks = await _context
            .Feedbacks.Where(t => feedbacksId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (feedbacks.Count == 0)
        {
            throw new NotFoundException();
        }

        var feedbacksToConnect = feedbacks.Except(parent.Feedbacks);

        foreach (var feedback in feedbacksToConnect)
        {
            parent.Feedbacks.Add(feedback);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple feedbacks records from Customer
    /// </summary>
    public async Task DisconnectFeedbacks(
        CustomerWhereUniqueInput uniqueId,
        FeedbackWhereUniqueInput[] feedbacksId
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Feedbacks)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var feedbacks = await _context
            .Feedbacks.Where(t => feedbacksId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var feedback in feedbacks)
        {
            parent.Feedbacks?.Remove(feedback);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple feedbacks records for Customer
    /// </summary>
    public async Task<List<Feedback>> FindFeedbacks(
        CustomerWhereUniqueInput uniqueId,
        FeedbackFindManyArgs customerFindManyArgs
    )
    {
        var feedbacks = await _context
            .Feedbacks.Where(m => m.CustomerId == uniqueId.Id)
            .ApplyWhere(customerFindManyArgs.Where)
            .ApplySkip(customerFindManyArgs.Skip)
            .ApplyTake(customerFindManyArgs.Take)
            .ApplyOrderBy(customerFindManyArgs.SortBy)
            .ToListAsync();

        return feedbacks.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple feedbacks records for Customer
    /// </summary>
    public async Task UpdateFeedbacks(
        CustomerWhereUniqueInput uniqueId,
        FeedbackWhereUniqueInput[] feedbacksId
    )
    {
        var customer = await _context
            .Customers.Include(t => t.Feedbacks)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (customer == null)
        {
            throw new NotFoundException();
        }

        var feedbacks = await _context
            .Feedbacks.Where(a => feedbacksId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (feedbacks.Count == 0)
        {
            throw new NotFoundException();
        }

        customer.Feedbacks = feedbacks;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Connect multiple Reviews records to Customer
    /// </summary>
    public async Task ConnectReviews(
        CustomerWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var reviews = await _context
            .Reviews.Where(t => reviewsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (reviews.Count == 0)
        {
            throw new NotFoundException();
        }

        var reviewsToConnect = reviews.Except(parent.Reviews);

        foreach (var review in reviewsToConnect)
        {
            parent.Reviews.Add(review);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Reviews records from Customer
    /// </summary>
    public async Task DisconnectReviews(
        CustomerWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var reviews = await _context
            .Reviews.Where(t => reviewsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var review in reviews)
        {
            parent.Reviews?.Remove(review);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Reviews records for Customer
    /// </summary>
    public async Task<List<Review>> FindReviews(
        CustomerWhereUniqueInput uniqueId,
        ReviewFindManyArgs customerFindManyArgs
    )
    {
        var reviews = await _context
            .Reviews.Where(m => m.CustomerId == uniqueId.Id)
            .ApplyWhere(customerFindManyArgs.Where)
            .ApplySkip(customerFindManyArgs.Skip)
            .ApplyTake(customerFindManyArgs.Take)
            .ApplyOrderBy(customerFindManyArgs.SortBy)
            .ToListAsync();

        return reviews.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Reviews records for Customer
    /// </summary>
    public async Task UpdateReviews(
        CustomerWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var customer = await _context
            .Customers.Include(t => t.Reviews)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (customer == null)
        {
            throw new NotFoundException();
        }

        var reviews = await _context
            .Reviews.Where(a => reviewsId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (reviews.Count == 0)
        {
            throw new NotFoundException();
        }

        customer.Reviews = reviews;
        await _context.SaveChangesAsync();
    }
}
