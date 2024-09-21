using CarRental.Domain.Commands;
using CarRental.Domain.CommandValidators;
using CarRental.Domain.Persistence;
using CarRental.Domain.Persistence.Models;

namespace CarRental.Domain.CommandHandlers;

public class StartRentalCommandHandler(IRentalRepository repository, StartRentalCommandValidator validator)
{
    public async Task<ExecutionResult> Handle(StartRentalCommand command)
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
            timeAtStart: command.Timestamp,
            mileageAtStart: command.Mileage);
        await repository.Add(model);

        return ExecutionResult.ForSuccess();
    }
}