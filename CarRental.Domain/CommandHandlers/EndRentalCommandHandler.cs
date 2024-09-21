using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;

namespace CarRental.Domain.CommandHandlers;

public class EndRentalCommandHandler(IRentalRepository repository)
{
    public async Task Handle(EndRentalCommand command)
    {
        var rental = await repository.GetByRentalNumber(command.RentalNumber);
        rental.TimeAtEnd = command.Timestamp;
        rental.MileageAtEnd = command.Mileage;
        await repository.Update(rental);
    }
}