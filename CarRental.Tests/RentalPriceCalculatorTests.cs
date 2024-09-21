namespace CarRental.Tests;

public class RentalPriceCalculatorTests
{
    [Theory]
    [InlineData(2, 3, 6)]
    public void SmallCar(decimal baseDailyRate, int days, decimal expected)
    {
    }

    [Theory]
    [InlineData(2, 3, 4, 5, 1000)]
    public void StationWagon(decimal baseDailyRate, int days, decimal baseKmRate, decimal km, decimal expected)
    {
    }

    [Theory]
    [InlineData(2, 3, 4, 5, 1000)]
    public void Truck(decimal baseDailyRate, int days, decimal baseKmRate, decimal km, decimal expected)
    {
    }
}