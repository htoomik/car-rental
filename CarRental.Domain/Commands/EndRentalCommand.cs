namespace CarRental.Domain.Commands;

public record EndRentalCommand(
    string RentalNumber,
    DateTime Timestamp,
    decimal Mileage
    );