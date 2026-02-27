namespace DerivativesPricingApp.Api.Data.Entities;

public class MarginCall
{
    public int Id { get; set; }
    public int CounterpartyId { get; set; }
    public DateTime CallDate { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Met, Disputed

    public Counterparty Counterparty { get; set; } = null!;
}
