using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;
using CarRental.Domain.Persistence.Models;
using FluentValidation;

namespace CarRental.Domain.CommandHandlers;

public class EndRentalCommandHandler(IRentalRepository repository, IValidator<EndRentalCommand> validator)
{
    public async Task<ExecutionResult> Handle(EndRentalCommand command)
    {
        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            return ExecutionResult.ForFailure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var rental = await repository.GetByRentalNumber(command.RentalNumber);

        rental.TimeAtEnd = command.Timestamp;
        rental.MileageAtEnd = command.Mileage;

        var isValid = Validate(command, rental, out var errors);
        if (!isValid)
        {
            return ExecutionResult.ForFailure(errors);
        }

        await repository.Update(rental);

        return ExecutionResult.ForSuccess();
    }

    private static bool Validate(EndRentalCommand command, Rental rental, out List<string> errors)
    {
        var isValid = true;
        errors = [];

        if (command.Timestamp < rental.TimeAtStart)
        {
            isValid = false;
            errors.Add("Rental end date/time must be after rental start");
        }

        if (command.Mileage < rental.MileageAtStart)
        {
            isValid = false;
            errors.Add("Rental end mileage must be greater than mileage at start");
        }

        return isValid;
    }
}