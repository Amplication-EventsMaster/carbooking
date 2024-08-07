using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class ReviewsControllerBase : ControllerBase
{
    protected readonly IReviewsService _service;

    public ReviewsControllerBase(IReviewsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Review
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<Review>> CreateReview(ReviewCreateInput input)
    {
        var review = await _service.CreateReview(input);

        return CreatedAtAction(nameof(Review), new { id = review.Id }, review);
    }

    /// <summary>
    /// Delete one Review
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteReview([FromRoute()] ReviewWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteReview(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Reviews
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<Review>>> Reviews([FromQuery()] ReviewFindManyArgs filter)
    {
        return Ok(await _service.Reviews(filter));
    }

    /// <summary>
    /// Meta data about Review records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> ReviewsMeta(
        [FromQuery()] ReviewFindManyArgs filter
    )
    {
        return Ok(await _service.ReviewsMeta(filter));
    }

    /// <summary>
    /// Get one Review
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<Review>> Review([FromRoute()] ReviewWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Review(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Review
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateReview(
        [FromRoute()] ReviewWhereUniqueInput uniqueId,
        [FromQuery()] ReviewUpdateInput reviewUpdateDto
    )
    {
        try
        {
            await _service.UpdateReview(uniqueId, reviewUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a Customer record for Review
    /// </summary>
    [HttpGet("{Id}/customers")]
    public async Task<ActionResult<List<Customer>>> GetCustomer(
        [FromRoute()] ReviewWhereUniqueInput uniqueId
    )
    {
        var customer = await _service.GetCustomer(uniqueId);
        return Ok(customer);
    }
}
