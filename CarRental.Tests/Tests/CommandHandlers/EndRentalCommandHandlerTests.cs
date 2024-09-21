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
        var startTime = new DateTime(2024, 09, 01);
        var endTime = new DateTime(2024, 08, 01); // Before start time = invalid
        const decimal startMileage = 10;
        const decimal endMileage = 20;

        var logger = Substitute.For<ILogger<EndRentalCommandHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<EndRentalCommand>>();
        var handler = new EndRentalCommandHandler(logger, repository, validator);

        repository.GetByRentalNumber(null!)
            .ReturnsForAnyArgs(new Rental("", "", "", CarCategory.Unknown, startTime, null, startMileage, null));
        validator.ValidateAsync(null!).ReturnsForAnyArgs(new ValidationResult());

        var result = await handler.Handle(new EndRentalCommand("", endTime, endMileage));

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Contain("date");
    }

    [Fact]
    public async void When_MileageValidationFails_Should_ReturnFailureWithErrors()
    {
        var startTime = new DateTime(2024, 09, 01);
        var endTime = new DateTime(2024, 09, 07);
        const decimal startMileage = 10;
        const decimal endMileage = 5; // Smaller than start mileage = invalid

        var logger = Substitute.For<ILogger<EndRentalCommandHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<EndRentalCommand>>();
        var handler = new EndRentalCommandHandler(logger, repository, validator);

        repository.GetByRentalNumber(null!)
            .ReturnsForAnyArgs(new Rental("", "", "", CarCategory.Unknown, startTime, null, startMileage, null));
        validator.ValidateAsync(null!).ReturnsForAnyArgs(new ValidationResult());

        var result = await handler.Handle(new EndRentalCommand("", endTime, endMileage));

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Contain("mileage");
    }

    [Fact]
    public async void When_RentalNotFound_Should_ReturnFailure()
    {
        const string rentalNumber = "rental";
        var endTime = new DateTime(2024, 09, 07);
        const decimal endMileage = 5;

        var logger = Substitute.For<ILogger<EndRentalCommandHandler>>();
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<EndRentalCommand>>();
        var handler = new EndRentalCommandHandler(logger, repository, validator);

        repository.GetByRentalNumber(null!).ReturnsForAnyArgs((Rental?)null);
        validator.ValidateAsync(null!).ReturnsForAnyArgs(new ValidationResult());

        var result = await handler.Handle(new EndRentalCommand(rentalNumber, endTime, endMileage));

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Contain("not found");
        result.Errors.Single().Should().Contain(rentalNumber);
    }
}