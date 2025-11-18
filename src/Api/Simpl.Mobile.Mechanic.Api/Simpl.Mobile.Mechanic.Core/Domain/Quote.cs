namespace Simpl.Mobile.Mechanic.Core.Domain;

public class Quote
{
  public Ulid Id { get; set; }
  public Ulid WorkRequestId { get; set; }
  public Ulid? MechanicId { get; set; }
}
