using UnitTests.Domain.MovieTheaterUseCase.Entities;

namespace UnitTests.Domain.MovieTheaterUseCase.Repositories;

public class InMemoryMovieTheaterRepository : IMovieTheaterRepository
{
    private readonly Dictionary<Guid, MovieTheater> _store = new();

    public Task<MovieTheater> Get(Guid id)
    {
        return Task.FromResult(_store[id]);
    }

    public Task Save(MovieTheater theater)
    {
        _store[theater.Id] = theater;
        return Task.CompletedTask;
    }
}