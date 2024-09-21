using CarRental.Domain;
using CarRental.Domain.CommandHandlers;
using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace CarRental.Tests.Tests;

public class StartRentalCommandHandlerTests
{
    [Fact]
    public async void When_CommandIsValid_Should_ReturnSuccess()
    {
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<StartRentalCommand>>();
        var handler = new StartRentalCommandHandler(repository, validator);

        validator.ValidateAsync(null!).ReturnsForAnyArgs(new ValidationResult());

        var result = await handler.Handle(new StartRentalCommand("", "", "", CarCategory.Unknown, DateTime.MinValue, 0));

        result.Success.Should().BeTrue();
    }

    [Fact]
    public async void When_CommandNotValid_Should_ReturnFailureWithErrors()
    {
        var repository = Substitute.For<IRentalRepository>();
        var validator = Substitute.For<IValidator<StartRentalCommand>>();
        var handler = new StartRentalCommandHandler(repository, validator);

        validator.ValidateAsync(null!)
            .ReturnsForAnyArgs(new ValidationResult(new List<ValidationFailure> { new("prop", "message") }));

        var result = await handler.Handle(new StartRentalCommand("", "", "", CarCategory.Unknown, DateTime.MinValue, 0));

        result.Success.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Single().Should().Be("message");
    }
}