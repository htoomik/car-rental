using CarRental.Domain.Abstractions;
using CarRental.Domain.Persistence;
using CarRental.Domain.Queries;
using CarRental.Domain.QueryResults;
using Microsoft.Extensions.Logging;

namespace CarRental.Domain.QueryHandlers;

public class RentalForPricingQueryHandler(ILogger<RentalForPricingQueryHandler> logger, IRentalRepository repository)
    : IQueryHandler
{
    public async Task<ExecutionResult<RentalForPricing>> Handle(RentalForPricingQuery query)
    {
        try
        {
            var rental = await repository.GetByRentalNumber(query.RentalNumber);

            if (rental == null)
            {
                return ExecutionResult<RentalForPricing>.ForFailure(
                    [$"Rental with rental number {query.RentalNumber} not found"]);
            }

            if (rental.EndTime == null)
            {
                return ExecutionResult<RentalForPricing>.ForFailure(
                    ["Rental end time must be set before calculating price"]);
            }

            if (rental.EndMileage == null)
            {
                return ExecutionResult<RentalForPricing>.ForFailure(
                    ["Rental end mileage must be set before calculating price"]);
            }

            var mileage = rental.EndMileage.Value - rental.StartMileage;
            var days = rental.EndTime.Value.Date.Subtract(rental.StartTime.Date).Days + 1;

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