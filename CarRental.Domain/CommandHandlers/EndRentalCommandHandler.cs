using CarRental.Domain.Commands;
using CarRental.Domain.CommandValidators;
using CarRental.Domain.Persistence;

namespace CarRental.Domain.CommandHandlers;

public class EndRentalCommandHandler(IRentalRepository repository, EndRentalCommandValidator validator)
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

        // NEXT: validate

        await repository.Update(rental);

        return ExecutionResult.ForSuccess();
    }
}