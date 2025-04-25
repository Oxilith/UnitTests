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

    public async Task Reserve(Guid theaterId, Reservation reservation)
    {
        var cinemaHall = await _repo.GetCinemaHall(theaterId);
        ArgumentNullException.ThrowIfNull(cinemaHall, nameof(cinemaHall));

        CheckIfReservationIsPossible(reservation, cinemaHall);
        cinemaHall.Reserve(reservation);

        CheckIfReservationWasSaved(reservation, cinemaHall);
        await _repo.Save(cinemaHall);
    }

    private static void CheckIfReservationWasSaved(Reservation reservation, CinemaHall theater)
    {
        if (!theater.DoesReservationExistInTheHall(reservation.Id)) return;

        Console.WriteLine("Reservation failed");
        throw new InvalidOperationException("Reservation failed");
    }

    private static void CheckIfReservationIsPossible(Reservation reservation, CinemaHall theater)
    {
        if (theater.DoesReservationExistInTheHall(reservation))
            throw new BusinessRuleViolationException("Reservation was already added.");

        foreach (var rowSeats in reservation.SeatsByRows)
        {
            if (!theater.DoesRowExistInTheHall(rowSeats.Key))
                throw new BusinessRuleViolationException($"Row {rowSeats.Key} does not exist in the theater.");

            if (!theater.DoesRowContainEnoughSeats(rowSeats.Key, rowSeats.Value.Count))
                throw new BusinessRuleViolationException($"Not enough seats available in the row {rowSeats.Key}.");
        }
    }
}