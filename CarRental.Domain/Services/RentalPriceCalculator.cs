using CarRental.Domain.Configuration;
using CarRental.Domain.QueryResults;

namespace CarRental.Domain.Services;

public class RentalPriceCalculator
{
    private readonly Dictionary<CarCategory, Func<RentalForPricing, RentalPriceConfiguration, decimal>> _mapping = new()
    {
        { CarCategory.Small, ForSmall },
        { CarCategory.StationWagon, ForStationWagon },
        { CarCategory.Truck, ForTruck }
    };

    public decimal Calculate(RentalForPricing rental, RentalPriceConfiguration configuration)
    {
        var category = rental.Category;

        if (!_mapping.TryGetValue(category, out var pricingFunction))
        {
            throw new Exception("Unknown car category " + category);
        }

        return pricingFunction(rental, configuration);
    }

    private static decimal ForSmall(RentalForPricing rental, RentalPriceConfiguration configuration)
    {
        return configuration.BaseDailyRate * rental.Days;
    }

    private static decimal ForStationWagon(RentalForPricing rental, RentalPriceConfiguration configuration)
    {
        return configuration.BaseDailyRate * rental.Days * 1.3m +
               configuration.BaseMileageRate * rental.Mileage;
    }

    private static decimal ForTruck(RentalForPricing rental, RentalPriceConfiguration configuration)
    {
        return configuration.BaseDailyRate * rental.Days * 1.5m +
               configuration.BaseMileageRate * rental.Mileage * 1.5m;
    }
}