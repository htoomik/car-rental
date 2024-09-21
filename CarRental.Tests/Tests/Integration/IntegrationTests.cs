using CarRental.Domain;
using CarRental.Domain.CommandHandlers;
using CarRental.Domain.Commands;
using CarRental.Domain.CommandValidators;
using CarRental.Domain.Configuration;
using CarRental.Domain.Queries;
using CarRental.Domain.QueryHandlers;
using CarRental.Domain.Services;
using CarRental.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace CarRental.Tests.Tests.Integration;

public class IntegrationTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Start_End_Calculate()
    {
        const string rentalNumber = "rental number";
        const string rentalStart = "2024-09-01 09:00";
        const string rentalEnd = "2024-09-02 11:00";

        var timeProvider = new FakeTimeProvider();
        timeProvider.SetUtcNow(new DateTimeOffset(2024, 09, 02, 13, 0, 0, TimeSpan.Zero));

        var repository = new InMemoryRentalRepository();

        var startCommand = CreateStartRentalCommand(rentalNumber, rentalStart);
        var startValidator = new StartRentalCommandValidator(timeProvider);
        var startHandler = new StartRentalCommandHandler(repository, startValidator);
        var result = await startHandler.Handle(startCommand);

        if (!result.Success)
        {
            _testOutputHelper.WriteLine(string.Join("\r\n", result.Errors));
        }
        result.Success.Should().BeTrue();

        var endCommand = CreateEndRentalCommand(rentalNumber, rentalEnd);
        var endValidator = new EndRentalCommandValidator(timeProvider);
        var endHandler = new EndRentalCommandHandler(repository, endValidator);
        var result2 = await endHandler.Handle(endCommand);

        if (!result2.Success)
        {
            _testOutputHelper.WriteLine(string.Join("\r\n", result2.Errors));
        }
        result2.Success.Should().BeTrue();

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(repository);
        var queryResult = await queryHandler.Handle(query);
        queryResult.Success.Should().BeTrue();
        var rentalForPricing = queryResult.Result!;

        var calculator = new RentalPriceCalculator();
        var priceConfiguration = new RentalPriceConfiguration(2, 3);
        var price = calculator.Calculate(rentalForPricing, priceConfiguration);

        // Calculation details are tested separately
        price.Should().NotBe(0);
    }

    private static StartRentalCommand CreateStartRentalCommand(string rentalNumber, string rentalStart)
    {
        var timestamp = DateTime.Parse(rentalStart);
        return new StartRentalCommand(rentalNumber, "reg number", "client id", CarCategory.Small, timestamp, 1);
    }

    private static EndRentalCommand CreateEndRentalCommand(string rentalNumber, string rentalEnd)
    {
        var timestamp = DateTime.Parse(rentalEnd);
        return new EndRentalCommand(rentalNumber, timestamp, 2);
    }
}