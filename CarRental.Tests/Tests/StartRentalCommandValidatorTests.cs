using CarRental.Domain;
using CarRental.Domain.Commands;
using CarRental.Domain.CommandValidators;
using FluentAssertions;
using Xunit.Abstractions;

namespace CarRental.Tests.Tests;

public class StartRentalCommandValidatorTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public StartRentalCommandValidatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [ClassData(typeof(ValidTestData))]
    public void When_Valid_Should_ReturnTrue(
        string rentalNumber,
        string registrationNumber,
        string clientIdentifier,
        CarCategory category,
        DateTime timestamp,
        decimal mileage)
    {
        var command = new StartRentalCommand(
            rentalNumber, registrationNumber, clientIdentifier, category, timestamp, mileage);
        var validator = new StartRentalCommandValidator(TimeProvider.System);
        var result = validator.Validate(command);

        // Debug help
        if (!result.IsValid)
        {
            _testOutputHelper.WriteLine(string.Join("\r\n", result.Errors.Select(e => e.ErrorMessage)));
        }

        result.IsValid.Should().BeTrue();
    }

    private class ValidTestData : TheoryData<string, string, string, CarCategory, DateTime, decimal>
    {
        public ValidTestData()
        {
            Add("rental", "reg", "client", CarCategory.Small, DateTime.UtcNow, 1);
        }
    }

    [Theory]
    [ClassData(typeof(InvalidTestData))]
    public void When_Invalid_Should_ReturnFalse(
        string rentalNumber,
        string registrationNumber,
        string clientIdentifier,
        CarCategory category,
        DateTime timestamp,
        decimal mileage,
        string description)
    {
        var command = new StartRentalCommand(
            rentalNumber, registrationNumber, clientIdentifier, category, timestamp, mileage);
        var validator = new StartRentalCommandValidator(TimeProvider.System);
        var result = validator.Validate(command);
        result.IsValid.Should().BeFalse(because: description);
    }

    private class InvalidTestData : TheoryData<string, string, string, CarCategory, DateTime, decimal, string>
    {
        public InvalidTestData()
        {
            Add("", "reg", "client", CarCategory.Small, DateTime.UtcNow, 1, "rental number empty");
            Add("rental", "", "client", CarCategory.Small, DateTime.UtcNow, 1, "registration number empty");
            Add("rental", "reg", "", CarCategory.Small, DateTime.UtcNow, 1, "client id empty");
            Add("rental", "reg", "client", CarCategory.Unknown, DateTime.UtcNow, 1, "category unknown");
            Add("rental", "reg", "client", CarCategory.Small, DateTime.MinValue, 1, "timestamp too small");
            Add("rental", "reg", "client", CarCategory.Small, DateTime.MaxValue, 1, "timestamp too large");
            Add("rental", "reg", "client", CarCategory.Small, DateTime.UtcNow, 0, "mileage zero");
        }
    }
}