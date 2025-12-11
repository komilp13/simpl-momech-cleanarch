namespace Simpl.Mobile.Mechanic.Core.Domain;

public class Mechanic
{
    public Ulid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Address? Address { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Email { get; set; }
}
