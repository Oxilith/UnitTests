﻿namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class HallRow
{
    public HallRow(int number, int seats)
    {
        Number = number;
        Seats = seats;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public int Number { get; }
    public int Seats { get; }
}