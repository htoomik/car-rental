using CarRental.Domain;
using CarRental.Domain.Persistence.Models;
using CarRental.Domain.Queries;
using CarRental.Domain.QueryHandlers;
using CarRental.Tests.Helpers;
using FluentAssertions;

namespace CarRental.Tests.Tests;

public class RentalForPricingQueryHandlerTests
{
    [Fact]
    public async Task When_EndTimeMissing_Should_ReturnFailure()
    {
        const string rentalNumber = "rental number";
        const decimal mileageAtStart = 1.1m;
        const decimal mileageAtEnd = 10.3m;

        var repository = new InMemoryRentalRepository();
        var rental = new Rental(rentalNumber, "reg", "client", CarCategory.Unknown, DateTime.MinValue,
            null, mileageAtStart, mileageAtEnd);
        await repository.Add(rental);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(repository);

        var result = await queryHandler.Handle(query);

        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task When_EndMileageMissing_Should_ReturnFailure()
    {
        const string rentalNumber = "rental number";
        const decimal mileageAtStart = 1.1m;

        var repository = new InMemoryRentalRepository();
        var rental = new Rental(rentalNumber, "reg", "client", CarCategory.Unknown, DateTime.MinValue,
            DateTime.MinValue, mileageAtStart, null);
        await repository.Add(rental);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(repository);

        var result = await queryHandler.Handle(query);

        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Should_CalculateMileage_WithNoRounding()
    {
        const string rentalNumber = "rental number";
        const decimal mileageAtStart = 1.1m;
        const decimal mileageAtEnd = 10.3m;

        var repository = new InMemoryRentalRepository();
        var rental = new Rental(rentalNumber, "reg", "client", CarCategory.Unknown, DateTime.MinValue,
            DateTime.MinValue, mileageAtStart, mileageAtEnd);
        await repository.Add(rental);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(repository);

        var result = await queryHandler.Handle(query);

        result.Success.Should().BeTrue();
        result.Result!.Mileage.Should().Be(9.2m);
    }

    [Theory]
    [InlineData("2024-09-21", "2024-09-21", 1)]
    [InlineData("2024-09-21", "2024-09-23", 3)]
    public async Task Should_CalculateDays_IncludingBothStartAndEndDates(string start, string end, int expected)
    {
        const string rentalNumber = "rental number";

        var startDate = DateTime.Parse(start);
        var endDate = DateTime.Parse(end);

        var repository = new InMemoryRentalRepository();
        var rental = new Rental(rentalNumber, "reg", "client", CarCategory.Unknown, startDate, endDate, 0, 0);
        await repository.Add(rental);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(repository);

        var result = await queryHandler.Handle(query);

        result.Success.Should().BeTrue();
        result.Result!.Days.Should().Be(expected);
    }
}