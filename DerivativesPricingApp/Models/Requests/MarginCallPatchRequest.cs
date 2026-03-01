namespace DerivativesPricingApp.Models.Requests;

public class MarginCallPatchRequest
{
    public string? Status { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
}
