using CarBookingService.APIs.Dtos;
using CarBookingService.Infrastructure.Models;

namespace CarBookingService.APIs.Extensions;

public static class CarsExtensions
{
    public static Car ToDto(this CarDbModel model)
    {
        return new Car
        {
            Bookings = model.Bookings?.Select(x => x.Id).ToList(),
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Make = model.Make,
            Model = model.Model,
            UpdatedAt = model.UpdatedAt,
            Year = model.Year,
        };
    }

    public static CarDbModel ToModel(this CarUpdateInput updateDto, CarWhereUniqueInput uniqueId)
    {
        var car = new CarDbModel
        {
            Id = uniqueId.Id,
            Make = updateDto.Make,
            Model = updateDto.Model,
            Year = updateDto.Year
        };

        if (updateDto.CreatedAt != null)
        {
            car.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            car.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return car;
    }
}
