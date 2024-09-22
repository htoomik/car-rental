using CarRental.Domain.Abstractions;
using CarRental.Domain.Configuration;
using CarRental.Domain.QueryResults;

namespace CarRental.Domain.Services.PricingStrategies;

public class StationWagonStrategy : IPricingStrategy
{
    public bool IsApplicable(CarCategory category)
    {
        return category == CarCategory.StationWagon;
    }

    public decimal Calculate(RentalForPricing rental, RentalPriceConfiguration configuration)
    {
        return configuration.BaseDailyRate * rental.Days * 1.3m +
               configuration.BaseMileageRate * rental.Mileage;
    }
}