using CarRental.Domain.Abstractions;
using CarRental.Domain.Configuration;
using CarRental.Domain.QueryResults;
using CarRental.Domain.Services.PricingStrategies;

namespace CarRental.Domain.Services;

public class RentalPriceCalculator(IEnumerable<IPricingStrategy> strategies) : IRentalPriceCalculator
{
    public decimal Calculate(RentalForPricing rental, RentalPriceConfiguration configuration)
    {
        var category = rental.Category;

        var applicableStrategies = strategies.Where(s => s.IsApplicable(category)).ToList();

        if (!applicableStrategies.Any())
        {
            throw new Exception($"No applicable pricing strategy found for {category}");
        }

        if (applicableStrategies.Count> 1)
        {
            throw new Exception($"Multiple applicable pricing strategies found for {category}");
        }

        return applicableStrategies.Single().Calculate(rental, configuration);
    }
}