namespace Simpl.Mobile.Mechanic.Core.Domain;

public class Vehicle
{
  public Ulid Id { get; set; }
  public Ulid CustomerId { get; set; }
  public Int16 Year { get; set; }
  public string? Make { get; set; }
  public string? Model { get; set; }
  public string? Vin { get; set; }
  public string? LicensePlate { get; set; }
}