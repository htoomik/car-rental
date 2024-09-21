namespace CarRental.Domain.Persistence.Models;

public class Rental
{
    public Guid Id { get; set; }
    public string RentalNumber { get; }
    public string RegistrationNumber { get; }
    public string ClientIdentifier { get; }
    public CarCategory Category { get; }
    public DateTime StartTime { get; }
    public DateTime? EndTime { get; set; }
    public decimal StartMileage { get; }
    public decimal? EndMileage { get; set; }

    public Rental(
        string rentalNumber,
        string registrationNumber,
        string clientIdentifier,
        CarCategory category,
        DateTime startTime,
        decimal startMileage)
    {
        RentalNumber = rentalNumber;
        RegistrationNumber = registrationNumber;
        ClientIdentifier = clientIdentifier;
        Category = category;
        StartTime = startTime;
        StartMileage = startMileage;
    }

    public Rental(
        string rentalNumber,
        string registrationNumber,
        string clientIdentifier,
        CarCategory category,
        DateTime startTime,
        DateTime? endTime,
        decimal startMileage,
        decimal? endMileage)
    {
        RentalNumber = rentalNumber;
        RegistrationNumber = registrationNumber;
        ClientIdentifier = clientIdentifier;
        Category = category;
        StartTime = startTime;
        EndTime = endTime;
        StartMileage = startMileage;
        EndMileage = endMileage;
    }
}