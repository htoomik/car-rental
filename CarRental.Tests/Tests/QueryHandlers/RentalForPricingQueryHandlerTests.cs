using CarRental.Domain;
using CarRental.Domain.Persistence;
using CarRental.Domain.Persistence.Models;
using CarRental.Domain.Queries;
using CarRental.Domain.QueryHandlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CarRental.Tests.Tests.QueryHandlers;

public class RentalForPricingQueryHandlerTests
{
    [Fact]
    public async Task When_RentalNotFound_Should_ReturnFailure()
    {
        const string rentalNumber = "rental number";

        var logger = Substitute.For<ILogger<RentalForPricingQueryHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        repository.GetByRentalNumber(null!).ReturnsForAnyArgs((Rental?)null);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(logger, repository);

        var result = await queryHandler.Handle(query);

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Contain("not found");
        result.Errors.Single().Should().Contain(rentalNumber);
    }

    [Fact]
    public async Task When_EndTimeMissing_Should_ReturnFailure()
    {
        const string rentalNumber = "rental number";
        const decimal startMileage = 1.1m;
        const decimal endMileage = 10.3m;

        var logger = Substitute.For<ILogger<RentalForPricingQueryHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var rental = new Rental(rentalNumber, "reg", "client", CarCategory.Unknown, DateTime.MinValue,
            null, startMileage, endMileage);
        repository.GetByRentalNumber(null!).ReturnsForAnyArgs(rental);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(logger, repository);

        var result = await queryHandler.Handle(query);

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Contain("end time");
    }

    [Fact]
    public async Task When_EndMileageMissing_Should_ReturnFailure()
    {
        const string rentalNumber = "rental number";
        const decimal startMileage = 1.1m;

        var logger = Substitute.For<ILogger<RentalForPricingQueryHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var rental = new Rental(rentalNumber, "reg", "client", CarCategory.Unknown, DateTime.MinValue,
            DateTime.MinValue, startMileage, null);
        repository.GetByRentalNumber(null!).ReturnsForAnyArgs(rental);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(logger,repository);

        var result = await queryHandler.Handle(query);

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Contain("end mileage");
    }

    [Fact]
    public async Task Should_CalculateMileage_WithNoRounding()
    {
        const string rentalNumber = "rental number";
        const decimal startMileage = 1.1m;
        const decimal endMileage = 10.3m;

        var logger = Substitute.For<ILogger<RentalForPricingQueryHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var rental = new Rental(rentalNumber, "reg", "client", CarCategory.Unknown, DateTime.MinValue,
            DateTime.MinValue, startMileage, endMileage);
        repository.GetByRentalNumber(null!).ReturnsForAnyArgs(rental);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(logger, repository);

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

        var logger = Substitute.For<ILogger<RentalForPricingQueryHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var rental = new Rental(rentalNumber, "reg", "client", CarCategory.Unknown, startDate, endDate, 0, 0);
        repository.GetByRentalNumber(null!).ReturnsForAnyArgs(rental);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(logger, repository);

        var result = await queryHandler.Handle(query);

        result.Success.Should().BeTrue();
        result.Result!.Days.Should().Be(expected);
    }
}