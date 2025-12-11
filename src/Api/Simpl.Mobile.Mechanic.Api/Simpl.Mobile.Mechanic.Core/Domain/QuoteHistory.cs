namespace Simpl.Mobile.Mechanic.Core.Domain;

public class QuoteHistory
{
    public Ulid Id { get; set; }
    public Ulid QuoteId { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Description { get; set; } = string.Empty;
}