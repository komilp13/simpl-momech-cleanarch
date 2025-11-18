namespace Simpl.Mobile.Mechanic.Core.Domain;

public class Customer
{
  public Ulid Id { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public Address[]? Addresses { get; set; }
  public string? Phone1 { get; set; }
  public string? Phone2 { get; set; }
  public string? Email { get; set; }
}
