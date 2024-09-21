using CarRental.Domain.Persistence.Models;

namespace CarRental.Domain.Persistence;

public interface IRentalRepository
{
    Task Add(Rental rental);
    Task Update(Rental rental);
    Task<Rental> GetByRentalNumber(string rentalNumber);
}