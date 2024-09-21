using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;

namespace CarRental.Domain.CommandHandlers;

public class EndRentalCommandHandler(IRentalRepository repository)
{
    public async Task Handle(EndRentalCommand command)
    {
    }
}