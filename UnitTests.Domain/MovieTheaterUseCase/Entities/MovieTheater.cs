using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class MovieTheater
{
    public Guid Id { get; }
    private readonly List<Reservation> _reservations = new();

    public MovieTheater(Guid id) => Id = id;

    public void Reserve(Guid reservationId, IEnumerable<SeatPosition> seats)
    {
        // TODO: 
    }

    public IReadOnlyCollection<Reservation> Reservations => _reservations;
}