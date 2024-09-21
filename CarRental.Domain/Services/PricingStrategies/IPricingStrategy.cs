using CarRental.Domain.Configuration;
using CarRental.Domain.QueryResults;

namespace CarRental.Domain.Services.PricingStrategies;

public interface IPricingStrategy
{
    public bool IsApplicable(CarCategory category);
    public decimal Calculate(RentalForPricing rental, RentalPriceConfiguration configuration);
}