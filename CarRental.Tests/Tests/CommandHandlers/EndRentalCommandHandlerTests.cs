using CarRental.Domain;
using CarRental.Domain.CommandHandlers;
using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;
using CarRental.Domain.Persistence.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CarRental.Tests.Tests.CommandHandlers;

public class EndRentalCommandHandlerTests
{
    [Fact]
    public async void When_CommandIsValid_And_RentalIsFound_Should_ReturnSuccess()
    {
        var logger = Substitute.For<ILogger<EndRentalCommandHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<EndRentalCommand>>();
        var handler = new EndRentalCommandHandler(logger, repository, validator);

        repository.GetByRentalNumber(null!)
            .ReturnsForAnyArgs(new Rental("", "", "", CarCategory.Unknown, DateTime.MinValue, DateTime.MinValue, 0, 0));
        validator.ValidateAsync(null!).ReturnsForAnyArgs(new ValidationResult());

        var result = await handler.Handle(new EndRentalCommand("", DateTime.MinValue, 0));

        result.Success.Should().BeTrue();
    }

    [Fact]
    public async void When_CommandNotValid_Should_ReturnFailureWithErrors()
    {
        var logger = Substitute.For<ILogger<EndRentalCommandHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<EndRentalCommand>>();
        var handler = new EndRentalCommandHandler(logger, repository, validator);

        validator.ValidateAsync(null!)
            .ReturnsForAnyArgs(new ValidationResult(new List<ValidationFailure> { new("prop", "message") }));

        var result = await handler.Handle(new EndRentalCommand("", DateTime.MinValue, 0));

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Be("message");
    }

    [Fact]
    public async void When_DateValidationFails_Should_ReturnFailureWithErrors()
    {
        var timeAtStart = new DateTime(2024, 09, 01);
        var timeAtEnd = new DateTime(2024, 08, 01); // Before timeAtStart = invalid
        const decimal mileageAtStart = 10;
        const decimal mileageAtEnd = 20;

        var logger = Substitute.For<ILogger<EndRentalCommandHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<EndRentalCommand>>();
        var handler = new EndRentalCommandHandler(logger, repository, validator);

        repository.GetByRentalNumber(null!)
            .ReturnsForAnyArgs(new Rental("", "", "", CarCategory.Unknown, timeAtStart, null, mileageAtStart, null));
        validator.ValidateAsync(null!).ReturnsForAnyArgs(new ValidationResult());

        var result = await handler.Handle(new EndRentalCommand("", timeAtEnd, mileageAtEnd));

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Contain("date");
    }

    [Fact]
    public async void When_MileageValidationFails_Should_ReturnFailureWithErrors()
    {
        var timeAtStart = new DateTime(2024, 09, 01);
        var timeAtEnd = new DateTime(2024, 09, 07);
        const decimal mileageAtStart = 10;
        const decimal mileageAtEnd = 5; // Smaller than mileage at start = invalid

        var logger = Substitute.For<ILogger<EndRentalCommandHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<EndRentalCommand>>();
        var handler = new EndRentalCommandHandler(logger, repository, validator);

        repository.GetByRentalNumber(null!)
            .ReturnsForAnyArgs(new Rental("", "", "", CarCategory.Unknown, timeAtStart, null, mileageAtStart, null));
        validator.ValidateAsync(null!).ReturnsForAnyArgs(new ValidationResult());

        var result = await handler.Handle(new EndRentalCommand("", timeAtEnd, mileageAtEnd));

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Contain("mileage");
    }
}