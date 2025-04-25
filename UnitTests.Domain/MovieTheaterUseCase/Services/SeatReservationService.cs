using UnitTests.Domain.MovieTheaterUseCase.Entities;
using UnitTests.Domain.MovieTheaterUseCase.Repositories;

namespace UnitTests.Domain.MovieTheaterUseCase.Services;

public class SeatReservationService
{
    private readonly IMovieTheaterRepository _repo;

    public SeatReservationService(IMovieTheaterRepository repo)
    {
        _repo = repo;
    }

    public async Task Reserve(Guid theaterId, Reservation reservation)
    {
        var theater = await _repo.Get(theaterId);
        if (theater == null)
        {
            Console.WriteLine("Theater not found");
            throw new InvalidOperationException("Theater not found");
        }

        Console.WriteLine("Trying to reserve seats");
        theater.Reserve(reservation);

        if (theater.Reservations.All(x => x.Id != reservation.Id))
        {
            Console.WriteLine("Reservation failed");
            throw new InvalidOperationException("Reservation failed");
        }

        Console.WriteLine("Reservation succeeded");
        await _repo.Save(theater);
    }
}