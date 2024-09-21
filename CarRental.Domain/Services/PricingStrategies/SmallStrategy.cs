using CarRental.Domain.Configuration;
using CarRental.Domain.QueryResults;

namespace CarRental.Domain.Services.PricingStrategies;

public class SmallStrategy : IPricingStrategy
{
    public bool IsApplicable(CarCategory category)
    {
        return category == CarCategory.Small;
    }

    public decimal Calculate(RentalForPricing rental, RentalPriceConfiguration configuration)
    {
        return configuration.BaseDailyRate * rental.Days;
    }
}