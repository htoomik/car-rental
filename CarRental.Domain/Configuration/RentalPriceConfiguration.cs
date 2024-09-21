namespace CarRental.Domain.Configuration;

public record RentalPriceConfiguration(
    decimal BaseDailyRate,
    decimal BaseMileageRate);