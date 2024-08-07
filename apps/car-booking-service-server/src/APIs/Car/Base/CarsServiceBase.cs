using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class CarsServiceBase : ICarsService
{
    protected readonly CarBookingServiceDbContext _context;

    public CarsServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Car
    /// </summary>
    public async Task<Car> CreateCar(CarCreateInput createDto)
    {
        var car = new CarDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Make = createDto.Make,
            Model = createDto.Model,
            UpdatedAt = createDto.UpdatedAt,
            Year = createDto.Year
        };

        if (createDto.Id != null)
        {
            car.Id = createDto.Id;
        }
        if (createDto.Bookings != null)
        {
            car.Bookings = await _context
                .Bookings.Where(booking =>
                    createDto.Bookings.Select(t => t.Id).Contains(booking.Id)
                )
                .ToListAsync();
        }

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<CarDbModel>(car.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Car
    /// </summary>
    public async Task DeleteCar(CarWhereUniqueInput uniqueId)
    {
        var car = await _context.Cars.FindAsync(uniqueId.Id);
        if (car == null)
        {
            throw new NotFoundException();
        }

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Cars
    /// </summary>
    public async Task<List<Car>> Cars(CarFindManyArgs findManyArgs)
    {
        var cars = await _context
            .Cars.Include(x => x.Bookings)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return cars.ConvertAll(car => car.ToDto());
    }

    /// <summary>
    /// Meta data about Car records
    /// </summary>
    public async Task<MetadataDto> CarsMeta(CarFindManyArgs findManyArgs)
    {
        var count = await _context.Cars.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Car
    /// </summary>
    public async Task<Car> Car(CarWhereUniqueInput uniqueId)
    {
        var cars = await this.Cars(
            new CarFindManyArgs { Where = new CarWhereInput { Id = uniqueId.Id } }
        );
        var car = cars.FirstOrDefault();
        if (car == null)
        {
            throw new NotFoundException();
        }

        return car;
    }

    /// <summary>
    /// Update one Car
    /// </summary>
    public async Task UpdateCar(CarWhereUniqueInput uniqueId, CarUpdateInput updateDto)
    {
        var car = updateDto.ToModel(uniqueId);

        if (updateDto.Bookings != null)
        {
            car.Bookings = await _context
                .Bookings.Where(booking => updateDto.Bookings.Select(t => t).Contains(booking.Id))
                .ToListAsync();
        }

        _context.Entry(car).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Cars.Any(e => e.Id == car.Id))
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
    /// Connect multiple Bookings records to Car
    /// </summary>
    public async Task ConnectBookings(
        CarWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    )
    {
        var parent = await _context
            .Cars.Include(x => x.Bookings)
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
    /// Disconnect multiple Bookings records from Car
    /// </summary>
    public async Task DisconnectBookings(
        CarWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    )
    {
        var parent = await _context
            .Cars.Include(x => x.Bookings)
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
    /// Find multiple Bookings records for Car
    /// </summary>
    public async Task<List<Booking>> FindBookings(
        CarWhereUniqueInput uniqueId,
        BookingFindManyArgs carFindManyArgs
    )
    {
        var bookings = await _context
            .Bookings.Where(m => m.CarId == uniqueId.Id)
            .ApplyWhere(carFindManyArgs.Where)
            .ApplySkip(carFindManyArgs.Skip)
            .ApplyTake(carFindManyArgs.Take)
            .ApplyOrderBy(carFindManyArgs.SortBy)
            .ToListAsync();

        return bookings.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Bookings records for Car
    /// </summary>
    public async Task UpdateBookings(
        CarWhereUniqueInput uniqueId,
        BookingWhereUniqueInput[] bookingsId
    )
    {
        var car = await _context
            .Cars.Include(t => t.Bookings)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (car == null)
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

        car.Bookings = bookings;
        await _context.SaveChangesAsync();
    }
}
