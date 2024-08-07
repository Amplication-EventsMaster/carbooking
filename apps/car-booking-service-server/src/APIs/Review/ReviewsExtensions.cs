using CarBookingService.APIs.Dtos;
using CarBookingService.Infrastructure.Models;

namespace CarBookingService.APIs.Extensions;

public static class ReviewsExtensions
{
    public static Review ToDto(this ReviewDbModel model)
    {
        return new Review
        {
            Comments = model.Comments,
            CreatedAt = model.CreatedAt,
            Customer = model.CustomerId,
            Date = model.Date,
            Id = model.Id,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static ReviewDbModel ToModel(
        this ReviewUpdateInput updateDto,
        ReviewWhereUniqueInput uniqueId
    )
    {
        var review = new ReviewDbModel
        {
            Id = uniqueId.Id,
            Comments = updateDto.Comments,
            Date = updateDto.Date
        };

        if (updateDto.CreatedAt != null)
        {
            review.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Customer != null)
        {
            review.CustomerId = updateDto.Customer;
        }
        if (updateDto.UpdatedAt != null)
        {
            review.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return review;
    }
}
