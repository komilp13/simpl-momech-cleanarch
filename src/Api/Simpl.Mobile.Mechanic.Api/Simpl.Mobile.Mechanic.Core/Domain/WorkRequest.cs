namespace Simpl.Mobile.Mechanic.Core.Domain;

public class WorkRequest
{
  public Ulid Id { get; set; }
  public Ulid CustomerId { get; set; }
  public Ulid AddressId { get; set; }
  public Int16 Year { get; set; }
  public string? Make { get; set; }
  public string? Model { get; set; }
  public string? Vin { get; set; }
  public string? LicensePlate { get; set; }
  public string? IssueDescription { get; set; }
  public Image[]? Images { get; set; }
}
