using CarRental.Domain;
using CarRental.Domain.Abstractions;
using CarRental.Domain.Configuration;
using CarRental.Domain.QueryResults;
using CarRental.Domain.Services;
using CarRental.Domain.Services.PricingStrategies;
using FluentAssertions;

namespace CarRental.Tests.Tests.Services;

public class RentalPriceCalculatorTests
{
    private static readonly List<IPricingStrategy> Strategies =
    [
        new SmallStrategy(),
        new StationWagonStrategy(),
        new TruckStrategy()
    ];

    [Theory]
    [InlineData(2, 3, 6)]
    public void SmallCar(decimal baseDailyRate, int days, decimal expected)
    {
        var rental = new RentalForPricing(CarCategory.Small, 0, days);
        var config = new RentalPriceConfiguration(baseDailyRate, 0);
        var calculator = new RentalPriceCalculator(Strategies);

        var result = calculator.Calculate(rental, config);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(2, 3, 4, 5, 27.8)] // 2 * 3 * 1.3 + 4 * 5 = 27.8
    public void StationWagon(decimal baseDailyRate, int days, decimal baseMileageRate, decimal mileage, decimal expected)
    {
        var rental = new RentalForPricing(CarCategory.StationWagon, mileage, days);
        var config = new RentalPriceConfiguration(baseDailyRate, baseMileageRate);
        var calculator = new RentalPriceCalculator(Strategies);

        var result = calculator.Calculate(rental, config);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(2, 3, 4, 5, 39)] // 2 * 3 * 1.5 + 4 * 5 * 1.5 = 39
    public void Truck(decimal baseDailyRate, int days, decimal baseMileageRate, decimal mileage, decimal expected)
    {
        var rental = new RentalForPricing(CarCategory.Truck, mileage, days);
        var config = new RentalPriceConfiguration(baseDailyRate, baseMileageRate);
        var calculator = new RentalPriceCalculator(Strategies);

        var result = calculator.Calculate(rental, config);

        result.Should().Be(expected);
    }
}