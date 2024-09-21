namespace CarRental.Domain.Commands;

public record StartRentalCommand(
    string RentalNumber,
    string RegistrationNumber,
    string ClientIdentifier,
    CarCategory Category,
    DateTime Timestamp,
    decimal Mileage
    );