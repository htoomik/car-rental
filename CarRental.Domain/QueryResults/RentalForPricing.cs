namespace CarRental.Domain.QueryResults;

public record RentalForPricing(
    CarCategory Category,
    decimal Mileage,
    int Days
    );