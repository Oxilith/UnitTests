using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class Reservation
{
    public Guid Id { get; }
    public IEnumerable<SeatPosition> Seats { get; }
    
    public Reservation(Guid id, IEnumerable<SeatPosition> seats)
    {
        // TODO: 
    }
}