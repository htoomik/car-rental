using CarRental.Domain.CommandHandlers;
using CarRental.Domain.Commands;
using CarRental.Domain.Configuration;
using CarRental.Domain.Queries;
using CarRental.Domain.QueryHandlers;
using CarRental.Domain.Services;
using CarRental.Tests.Helpers;
using FluentAssertions;

namespace CarRental.Tests;

public class IntegrationTests
{
    [Fact]
    public async Task Start_End_Calculate()
    {
        var repository = new InMemoryRentalRepository();

        var startCommand = new StartRentalCommand();
        var startHandler = new StartRentalCommandHandler(repository);
        var id = await startHandler.Handle(startCommand);

        var endCommand = new EndRentalCommand();
        var endHandler = new EndRentalCommandHandler(repository);
        await endHandler.Handle(endCommand);

        var query = new RentalForPricingQuery(id);
        var queryHandler = new RentalForPricingQueryHandler(repository);
        var rentalForPricing = await queryHandler.Handle(query);

        var calculator = new RentalPriceCalculator();
        var priceConfiguration = new RentalPriceConfiguration();
        var price = calculator.Calculate(rentalForPricing, priceConfiguration);

        // Calculation details are tested separately
        price.Should().NotBe(0);
    }
}