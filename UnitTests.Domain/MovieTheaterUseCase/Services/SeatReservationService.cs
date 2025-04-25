using UnitTests.Domain.MovieTheaterUseCase.Entities;
using UnitTests.Domain.MovieTheaterUseCase.Exceptions;
using UnitTests.Domain.MovieTheaterUseCase.Repositories;

namespace UnitTests.Domain.MovieTheaterUseCase.Services;

public class SeatReservationService
{
    private readonly ICinemaHallRepository _repo;

    public SeatReservationService(ICinemaHallRepository repo)
    {
        _repo = repo;
    }

    public async Task Reserve(Guid theaterId, HallReservation hallReservation)
    {
        var cinemaHall = await _repo.GetCinemaHall(theaterId);
        ArgumentNullException.ThrowIfNull(cinemaHall, nameof(cinemaHall));

        CheckIfReservationIsPossible(hallReservation, cinemaHall);
        cinemaHall.Reserve(hallReservation);

        CheckIfReservationWasSaved(hallReservation, cinemaHall);
        await _repo.Save(cinemaHall);
    }

    private static void CheckIfReservationWasSaved(HallReservation hallReservation, CinemaHall theater)
    {
        if (!theater.DoesReservationExistInTheHall(hallReservation.Id)) return;

        Console.WriteLine("HallReservation failed");
        throw new InvalidOperationException("HallReservation failed");
    }

    private static void CheckIfReservationIsPossible(HallReservation hallReservation, CinemaHall theater)
    {
        if (theater.DoesReservationExistInTheHall(hallReservation))
            throw new BusinessRuleViolationException("HallReservation was already added.");

        foreach (var rowSeats in hallReservation.SeatsByRows)
        {
            if (!theater.DoesRowExistInTheHall(rowSeats.Key))
                throw new BusinessRuleViolationException($"HallRow {rowSeats.Key} does not exist in the theater.");

            if (!theater.DoesRowContainEnoughSeats(rowSeats.Key, rowSeats.Value.Count))
                throw new BusinessRuleViolationException($"Not enough seats available in the row {rowSeats.Key}.");
        }
    }
}