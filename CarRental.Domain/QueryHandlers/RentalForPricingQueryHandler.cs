using CarRental.Domain.Persistence;
using CarRental.Domain.Queries;
using CarRental.Domain.SQueryResults;

namespace CarRental.Domain.QueryHandlers;

public class RentalForPricingQueryHandler(IRentalRepository repository)
{
    public async Task<RentalForPricing> Handle(RentalForPricingQuery query)
    {
        return new RentalForPricing();
    }
}