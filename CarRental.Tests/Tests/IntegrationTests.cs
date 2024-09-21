using CarRental.Domain;
using CarRental.Domain.CommandHandlers;
using CarRental.Domain.Commands;
using CarRental.Domain.Configuration;
using CarRental.Domain.Queries;
using CarRental.Domain.QueryHandlers;
using CarRental.Domain.Services;
using CarRental.Tests.Helpers;
using FluentAssertions;

namespace CarRental.Tests.Tests;

public class IntegrationTests
{
    [Fact]
    public async Task Start_End_Calculate()
    {
        const string rentalNumber = "rental number";
        const string rentalStart = "2024-09-01";
        const string rentalEnd = "2024-09-02";

        var repository = new InMemoryRentalRepository();

        var startCommand = CreateStartRentalCommand(rentalNumber, rentalStart);
        var startHandler = new StartRentalCommandHandler(repository);
        await startHandler.Handle(startCommand);

        var endCommand = CreateEndRentalCommand(rentalNumber, rentalEnd);
        var endHandler = new EndRentalCommandHandler(repository);
        await endHandler.Handle(endCommand);

        var query = new RentalForPricingQuery(rentalNumber);
        var queryHandler = new RentalForPricingQueryHandler(repository);
        var rentalForPricing = await queryHandler.Handle(query);

        var calculator = new RentalPriceCalculator();
        var priceConfiguration = new RentalPriceConfiguration();
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