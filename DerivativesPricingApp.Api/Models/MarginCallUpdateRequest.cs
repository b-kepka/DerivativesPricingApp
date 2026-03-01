namespace DerivativesPricingApp.Api.Models;

public class MarginCallUpdateRequest
{
    public int CounterpartyId { get; set; }
    public DateTime CallDate { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string Status { get; set; } = null!;
}
