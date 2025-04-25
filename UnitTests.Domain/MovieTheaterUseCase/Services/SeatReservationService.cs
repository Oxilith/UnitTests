using UnitTests.Domain.Extensions;
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

    private void CheckIfReservationWasSaved(HallReservation hallReservation, CinemaHall cinemaHall)
    {
        if (!cinemaHall.DoesReservationExistInTheHall(hallReservation.Id))
            throw new InvalidOperationException("HallReservation failed");
    }

    private static void CheckIfReservationIsPossible(HallReservation hallReservation, CinemaHall theater)
    {
        if (theater.DoesReservationExistInTheHall(hallReservation))
            throw new BusinessRuleViolationException("HallReservation was already added.");

        var allReservations = theater.GetReservedSeatsByRow();

        foreach (var seatsByRowForReservation in allReservations)
        foreach (var pair in hallReservation.SeatsByRows)
            if (seatsByRowForReservation[pair.Key].Intersect(hallReservation.SeatsByRows[pair.Key]).Any())
                throw new BusinessRuleViolationException("Seats are already reserved.");

        foreach (var rowSeats in hallReservation.SeatsByRows)
        {
            if (!theater.DoesRowExistInTheHall(rowSeats.Key))
                throw new BusinessRuleViolationException($"HallRow {rowSeats.Key} does not exist in the theater.");

            if (!theater.DoesRowContainEnoughSeats(rowSeats.Key, rowSeats.Value.Count))
                throw new BusinessRuleViolationException($"Not enough seats available in the row {rowSeats.Key}.");
        }

        foreach (var seatsByRowForReservation in allReservations)
        foreach (var pair in hallReservation.SeatsByRows)
        {
            var lists = seatsByRowForReservation[pair.Key].SplitByGaps();

            foreach (var list in lists) CheckForDistance(hallReservation, list, pair);
        }
    }

    private static void CheckForDistance(HallReservation hallReservation, List<int> lists,
        KeyValuePair<Guid, List<int>> pair)
    {
        var min = lists.Min();
        var max = lists.Max();

        var reservationMin = hallReservation.SeatsByRows[pair.Key].Min();
        var reservationMax = hallReservation.SeatsByRows[pair.Key].Max();

        if (Math.Abs(min - reservationMax) <= CinemaHall.SocialDistance ||
            Math.Abs(max - reservationMin) <= CinemaHall.SocialDistance ||
            Math.Abs(min - reservationMin) <= CinemaHall.SocialDistance ||
            Math.Abs(max - reservationMax) <= CinemaHall.SocialDistance)
            throw new BusinessRuleViolationException(
                $"Cannot reserve seats without social distance. Minimum distance required {CinemaHall.SocialDistance} seats.");
    }
}