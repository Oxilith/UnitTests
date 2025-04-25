using UnitTests.Domain.MovieTheaterUseCase.Entities;

namespace UnitTests.Domain.MovieTheaterUseCase.Repositories;

public class InMemoryCinemaHallRepository : ICinemaHallRepository
{
    private readonly Dictionary<Guid, CinemaHall> _store = new();

    public Task<CinemaHall> GetCinemaHall(Guid id)
    {
        return Task.FromResult(_store[id]);
    }

    public Task Save(CinemaHall theater)
    {
        _store[theater.Id] = theater;
        return Task.CompletedTask;
    }
}