using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;
using CarRental.Domain.Persistence.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CarRental.Domain.CommandHandlers;

public class StartRentalCommandHandler(
    ILogger<StartRentalCommandHandler> logger,
    IRentalRepository repository,
    IValidator<StartRentalCommand> validator)
{
    public async Task<ExecutionResult> Handle(StartRentalCommand command)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return ExecutionResult.ForFailure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var model = new Rental(
                rentalNumber: command.RentalNumber,
                registrationNumber: command.RegistrationNumber,
                clientIdentifier: command.ClientIdentifier,
                category: command.Category,
                startTime: command.Timestamp,
                startMileage: command.Mileage);
            await repository.Add(model);

            return ExecutionResult.ForSuccess();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling start of rental");
            return ExecutionResult.ForFailure([ex.Message]);
        }
    }
}