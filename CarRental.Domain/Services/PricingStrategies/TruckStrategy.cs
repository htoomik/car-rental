using CarRental.Domain.Configuration;
using CarRental.Domain.QueryResults;

namespace CarRental.Domain.Services.PricingStrategies;

public class TruckStrategy : IPricingStrategy
{
    public bool IsApplicable(CarCategory category)
    {
        return category == CarCategory.Truck;
    }

    public decimal Calculate(RentalForPricing rental, RentalPriceConfiguration configuration)
    {
        return configuration.BaseDailyRate * rental.Days * 1.5m +
               configuration.BaseMileageRate * rental.Mileage * 1.5m;
    }
}