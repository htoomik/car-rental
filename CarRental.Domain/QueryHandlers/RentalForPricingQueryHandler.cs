using CarRental.Domain.Persistence;
using CarRental.Domain.Queries;
using CarRental.Domain.QueryResults;
using Microsoft.Extensions.Logging;

namespace CarRental.Domain.QueryHandlers;

public class RentalForPricingQueryHandler(ILogger<RentalForPricingQueryHandler> logger, IRentalRepository repository)
{
    public async Task<ExecutionResult<RentalForPricing>> Handle(RentalForPricingQuery query)
    {
        try
        {
            var rental = await repository.GetByRentalNumber(query.RentalNumber);

            if (rental.TimeAtEnd == null)
            {
                return ExecutionResult<RentalForPricing>.ForFailure(
                    ["Rental end time must be set before calculating price"]);
            }

            if (rental.MileageAtEnd == null)
            {
                return ExecutionResult<RentalForPricing>.ForFailure(
                    ["Rental end mileage must be set before calculating price"]);
            }

            var mileage = rental.MileageAtEnd.Value - rental.MileageAtStart;
            var days = rental.TimeAtEnd.Value.Date.Subtract(rental.TimeAtStart.Date).Days + 1;

            var result = new RentalForPricing(rental.Category, mileage, days);

            return ExecutionResult<RentalForPricing>.ForSuccess(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rental data for pricing");
            return ExecutionResult<RentalForPricing>.ForFailure([ex.Message]);
        }
    }
}