using CarRental.Domain.Persistence;
using CarRental.Domain.Persistence.Models;

namespace CarRental.Tests.Tests.Integration.Helpers;

public class InMemoryRentalRepository : IRentalRepository
{
    private readonly Dictionary<string, Rental> _storage = new();

    public async Task Add(Rental rental)
    {
        rental.Id = Guid.NewGuid();
        _storage[rental.RentalNumber] = rental;
    }

    public async Task Update(Rental rental)
    {
        _storage[rental.RentalNumber] = rental;
    }

    public async Task<Rental> GetByRentalNumber(string rentalNumber)
    {
        return _storage[rentalNumber];
    }
}