using CarRental.Domain.Persistence;
using CarRental.Domain.Queries;
using CarRental.Domain.QueryResults;

namespace CarRental.Domain.QueryHandlers;

public class RentalForPricingQueryHandler(IRentalRepository repository)
{
    public async Task<RentalForPricing> Handle(RentalForPricingQuery query)
    {
        var rental = await repository.GetByRentalNumber(query.RentalNumber);

        var mileage = rental.MileageAtEnd - rental.MileageAtStart;
        var days = rental.TimeAtEnd.Date.Subtract(rental.TimeAtStart.Date).Days + 1;

        return new RentalForPricing(rental.Category, mileage, days);
    }
}