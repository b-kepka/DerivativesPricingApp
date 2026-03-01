namespace DerivativesPricingApp.Models.Requests;

public class MarginCallCreateRequest
{
    public int CounterpartyId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string Status { get; set; } = "Pending";
}
