namespace Simpl.Mobile.Mechanic.Core.Domain;

public class Image
{
    public Ulid Id { get; set; }
    public Ulid WorkRequestId { get; set; }
    public string? ImageName { get; set; }
}