namespace DerivativesPricingApp.Models.Requests;

public class OptionPricingRequest
{
    public decimal Spot { get; set; }
    public decimal Strike { get; set; }
    public decimal TimeToMaturityYears { get; set; }
    public decimal RiskFreeRate { get; set; }
    public decimal Volatility { get; set; }
    public string OptionType { get; set; } = "Call";
}
