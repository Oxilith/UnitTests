using UnitTests.Domain.MovieTheaterUseCase.Entities;

namespace UnitTests.Domain.MovieTheaterUseCase.Repositories;

public interface IMovieTheaterRepository
{
    Task<MovieTheater?> Get(Guid id);
    Task Save(MovieTheater theater);
}