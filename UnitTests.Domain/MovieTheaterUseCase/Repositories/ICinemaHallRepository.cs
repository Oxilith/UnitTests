using UnitTests.Domain.MovieTheaterUseCase.Entities;

namespace UnitTests.Domain.MovieTheaterUseCase.Repositories;

public interface ICinemaHallRepository
{
    Task<CinemaHall?> GetCinemaHall(Guid id);
    Task Save(CinemaHall theater);
}