using CarRental.Domain.Commands;
using FluentValidation;

namespace CarRental.Domain.CommandValidators;

public class EndRentalCommandValidator : AbstractValidator<EndRentalCommand>
{
    public EndRentalCommandValidator(TimeProvider timeProvider)
    {
        var today = timeProvider.GetUtcNow().Date;

        RuleFor(c => c.RentalNumber).NotEmpty();

        // Past dates make sense to some extent in case of late registration but there should be a limit
        RuleFor(c => c.Timestamp).GreaterThan(today.AddDays(-1));

        // Future dates make sense to some extent in case of system time diffs but there should be a limit
        RuleFor(c => c.Timestamp).LessThan(today.AddDays(1));

        RuleFor(c => c.Mileage).GreaterThan(0);
    }
}