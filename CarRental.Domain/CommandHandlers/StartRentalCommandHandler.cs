using CarRental.Domain.Commands;
using CarRental.Domain.Persistence;

namespace CarRental.Domain.CommandHandlers;

public class StartRentalCommandHandler(IRentalRepository repository)
{
    public async Task<Guid> Handle(StartRentalCommand command)
    {
        return Guid.NewGuid();
    }
}