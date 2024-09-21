namespace CarRental.Domain.Persistence.Models;

public class Rental
{
    public Guid Id { get; set; }
    public string RentalNumber { get; }
    public string RegistrationNumber { get; }
    public string ClientIdentifier { get; }
    public CarCategory Category { get; }
    public DateTime TimeAtStart { get; }
    public DateTime TimeAtEnd { get; set; }
    public decimal MileageAtStart { get; }
    public decimal MileageAtEnd { get; set; }

    public Rental(
        string rentalNumber,
        string registrationNumber,
        string clientIdentifier,
        CarCategory category,
        DateTime timeAtStart,
        decimal mileageAtStart)
    {
        RentalNumber = rentalNumber;
        RegistrationNumber = registrationNumber;
        ClientIdentifier = clientIdentifier;
        Category = category;
        TimeAtStart = timeAtStart;
        MileageAtStart = mileageAtStart;
    }

    public Rental(
        string rentalNumber,
        string registrationNumber,
        string clientIdentifier,
        CarCategory category,
        DateTime timeAtStart,
        DateTime timeAtEnd,
        decimal mileageAtStart,
        decimal mileageAtEnd)
    {
        RentalNumber = rentalNumber;
        RegistrationNumber = registrationNumber;
        ClientIdentifier = clientIdentifier;
        Category = category;
        TimeAtStart = timeAtStart;
        TimeAtEnd = timeAtEnd;
        MileageAtStart = mileageAtStart;
        MileageAtEnd = mileageAtEnd;
    }
}