using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class CustomersControllerBase : ControllerBase
{
    protected readonly ICustomersService _service;

    public CustomersControllerBase(ICustomersService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Customer
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<Customer>> CreateCustomer(CustomerCreateInput input)
    {
        var customer = await _service.CreateCustomer(input);

        return CreatedAtAction(nameof(Customer), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Delete one Customer
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteCustomer([FromRoute()] CustomerWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteCustomer(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Customers
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<Customer>>> Customers(
        [FromQuery()] CustomerFindManyArgs filter
    )
    {
        return Ok(await _service.Customers(filter));
    }

    /// <summary>
    /// Meta data about Customer records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> CustomersMeta(
        [FromQuery()] CustomerFindManyArgs filter
    )
    {
        return Ok(await _service.CustomersMeta(filter));
    }

    /// <summary>
    /// Get one Customer
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<Customer>> Customer(
        [FromRoute()] CustomerWhereUniqueInput uniqueId
    )
    {
        try
        {
            return await _service.Customer(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Customer
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateCustomer(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromQuery()] CustomerUpdateInput customerUpdateDto
    )
    {
        try
        {
            await _service.UpdateCustomer(uniqueId, customerUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Bookings records to Customer
    /// </summary>
    [HttpPost("{Id}/bookings")]
    public async Task<ActionResult> ConnectBookings(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromQuery()] BookingWhereUniqueInput[] bookingsId
    )
    {
        try
        {
            await _service.ConnectBookings(uniqueId, bookingsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Bookings records from Customer
    /// </summary>
    [HttpDelete("{Id}/bookings")]
    public async Task<ActionResult> DisconnectBookings(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromBody()] BookingWhereUniqueInput[] bookingsId
    )
    {
        try
        {
            await _service.DisconnectBookings(uniqueId, bookingsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Bookings records for Customer
    /// </summary>
    [HttpGet("{Id}/bookings")]
    public async Task<ActionResult<List<Booking>>> FindBookings(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromQuery()] BookingFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindBookings(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Bookings records for Customer
    /// </summary>
    [HttpPatch("{Id}/bookings")]
    public async Task<ActionResult> UpdateBookings(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromBody()] BookingWhereUniqueInput[] bookingsId
    )
    {
        try
        {
            await _service.UpdateBookings(uniqueId, bookingsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple feedbacks records to Customer
    /// </summary>
    [HttpPost("{Id}/feedbacks")]
    public async Task<ActionResult> ConnectFeedbacks(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromQuery()] FeedbackWhereUniqueInput[] feedbacksId
    )
    {
        try
        {
            await _service.ConnectFeedbacks(uniqueId, feedbacksId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple feedbacks records from Customer
    /// </summary>
    [HttpDelete("{Id}/feedbacks")]
    public async Task<ActionResult> DisconnectFeedbacks(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromBody()] FeedbackWhereUniqueInput[] feedbacksId
    )
    {
        try
        {
            await _service.DisconnectFeedbacks(uniqueId, feedbacksId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple feedbacks records for Customer
    /// </summary>
    [HttpGet("{Id}/feedbacks")]
    public async Task<ActionResult<List<Feedback>>> FindFeedbacks(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromQuery()] FeedbackFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindFeedbacks(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple feedbacks records for Customer
    /// </summary>
    [HttpPatch("{Id}/feedbacks")]
    public async Task<ActionResult> UpdateFeedbacks(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromBody()] FeedbackWhereUniqueInput[] feedbacksId
    )
    {
        try
        {
            await _service.UpdateFeedbacks(uniqueId, feedbacksId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Reviews records to Customer
    /// </summary>
    [HttpPost("{Id}/reviews")]
    public async Task<ActionResult> ConnectReviews(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromQuery()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.ConnectReviews(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Reviews records from Customer
    /// </summary>
    [HttpDelete("{Id}/reviews")]
    public async Task<ActionResult> DisconnectReviews(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromBody()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.DisconnectReviews(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Reviews records for Customer
    /// </summary>
    [HttpGet("{Id}/reviews")]
    public async Task<ActionResult<List<Review>>> FindReviews(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromQuery()] ReviewFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindReviews(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Reviews records for Customer
    /// </summary>
    [HttpPatch("{Id}/reviews")]
    public async Task<ActionResult> UpdateReviews(
        [FromRoute()] CustomerWhereUniqueInput uniqueId,
        [FromBody()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.UpdateReviews(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
