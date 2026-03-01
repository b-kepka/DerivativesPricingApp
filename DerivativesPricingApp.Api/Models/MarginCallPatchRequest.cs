namespace DerivativesPricingApp.Api.Models;

public class MarginCallPatchRequest
{
    public string? Status { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
}
