using CarRental.Domain.Configuration;
using CarRental.Domain.QueryResults;

namespace CarRental.Domain.Abstractions;

public interface IRentalPriceCalculator
{
    decimal Calculate(RentalForPricing rental, RentalPriceConfiguration configuration);
}