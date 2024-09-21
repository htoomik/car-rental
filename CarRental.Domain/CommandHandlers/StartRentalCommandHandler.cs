using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;
using CarRental.Domain.Persistence.Models;

namespace CarRental.Domain.CommandHandlers;

public class StartRentalCommandHandler(IRentalRepository repository)
{
    public async Task Handle(StartRentalCommand command)
    {
        var model = new Rental(
            rentalNumber: command.RentalNumber,
            registrationNumber: command.RegistrationNumber,
            clientIdentifier: command.ClientIdentifier,
            category: command.Category,
            timeAtStart: command.Timestamp,
            mileageAtStart: command.Mileage);
        await repository.Add(model);
    }
}