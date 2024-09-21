using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;
using CarRental.Domain.Persistence.Models;
using CarRental.Domain.QueryResults;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CarRental.Domain.CommandHandlers;

public class EndRentalCommandHandler(
    ILogger<EndRentalCommandHandler> logger,
    IRentalRepository repository,
    IValidator<EndRentalCommand> validator)
{
    public async Task<ExecutionResult> Handle(EndRentalCommand command)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return ExecutionResult.ForFailure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var rental = await repository.GetByRentalNumber(command.RentalNumber);

            if (rental == null)
            {
                return ExecutionResult.ForFailure(
                    [$"Rental with rental number {command.RentalNumber} not found"]);
            }

            var isValid = Validate(command, rental, out var errors);
            if (!isValid)
            {
                return ExecutionResult.ForFailure(errors);
            }

            rental.EndTime = command.Timestamp;
            rental.EndMileage = command.Mileage;

            await repository.Update(rental);

            return ExecutionResult.ForSuccess();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling end of rental");
            return ExecutionResult.ForFailure([ex.Message]);
        }
    }

    private static bool Validate(EndRentalCommand command, Rental rental, out List<string> errors)
    {
        var isValid = true;
        errors = [];

        if (command.Timestamp < rental.StartTime)
        {
            isValid = false;
            errors.Add("Rental end date/time must be after rental start");
        }

        if (command.Mileage < rental.StartMileage)
        {
            isValid = false;
            errors.Add("Rental end mileage must be greater than mileage at start");
        }

        return isValid;
    }
}