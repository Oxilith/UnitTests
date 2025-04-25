using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class Reservation
{
    public Guid Id { get; }
    public IEnumerable<SeatPosition> Seats { get; }
    
    public Reservation(IEnumerable<SeatPosition> seats)
    {
        Id = Guid.NewGuid();
        // TODO: 
    }
}