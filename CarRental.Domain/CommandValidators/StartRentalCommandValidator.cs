using CarRental.Domain.Commands;
using FluentValidation;

namespace CarRental.Domain.CommandValidators;

public class StartRentalCommandValidator : AbstractValidator<StartRentalCommand>
{
    public StartRentalCommandValidator(TimeProvider timeProvider)
    {
        var today = timeProvider.GetUtcNow().Date;

        RuleFor(c => c.RentalNumber).NotEmpty();
        RuleFor(c => c.ClientIdentifier).NotEmpty();
        RuleFor(c => c.RegistrationNumber).NotEmpty();

        RuleFor(c => c.Category).NotEqual(CarCategory.Unknown);

        // Past dates make sense to some extent in case of late registration but there should be a limit
        RuleFor(c => c.Timestamp).GreaterThan(today.AddDays(-1));

        // Future dates make sense to some extent in case of system time diffs but there should be a limit
        RuleFor(c => c.Timestamp).LessThan(today.AddDays(1));

        RuleFor(c => c.Mileage).GreaterThan(0);
    }
}