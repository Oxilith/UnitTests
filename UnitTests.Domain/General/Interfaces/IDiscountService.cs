using UnitTests.Domain.MeetingRoomReservationUseCase.Enums;

namespace UnitTests.Domain.General.Interfaces;

public interface IDiscountService
{
    decimal CalculateDiscount(decimal amount, string customerType);
    (decimal, int) Example(decimal number, CustomerType customerType);
}