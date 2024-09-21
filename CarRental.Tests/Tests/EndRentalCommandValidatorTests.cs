using CarRental.Domain.Commands;
using CarRental.Domain.CommandValidators;
using FluentAssertions;
using Xunit.Abstractions;

namespace CarRental.Tests.Tests;

public class EndRentalCommandValidatorTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public EndRentalCommandValidatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [ClassData(typeof(ValidTestData))]
    public void When_Valid_Should_ReturnTrue(
        string rentalNumber,
        DateTime timeAtEnd,
        decimal mileageAtEnd)
    {
        var command = new EndRentalCommand(rentalNumber, timeAtEnd, mileageAtEnd);
        var validator = new EndRentalCommandValidator(TimeProvider.System);
        var result = validator.Validate(command);

        // Debug help
        if (!result.IsValid)
        {
            _testOutputHelper.WriteLine(string.Join("\r\n", result.Errors.Select(e => e.ErrorMessage)));
        }

        result.IsValid.Should().BeTrue();
    }

    private class ValidTestData : TheoryData<string, DateTime, decimal>
    {
        public ValidTestData()
        {
            Add("rental", DateTime.Today, 1);
        }
    }

    [Theory]
    [ClassData(typeof(InvalidTestData))]
    public void When_Invalid_Should_ReturnFalse(
        string rentalNumber,
        DateTime timeAtEnd,
        decimal mileageAtEnd,
        string description)
    {
        var command = new EndRentalCommand(rentalNumber, timeAtEnd, mileageAtEnd);
        var validator = new EndRentalCommandValidator(TimeProvider.System);
        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse(because: description);
    }

    private class InvalidTestData : TheoryData<string, DateTime, decimal, string>
    {
        public InvalidTestData()
        {
            var today = DateTime.Today;

            Add("", today, 1, "rental number empty");
            Add("rental", today.AddYears(-1), 1, "timestamp too small");
            Add("rental", today.AddYears(1), 1, "timestamp too large");
            Add("rental", today, 0, "mileage too small");
        }
    }
}