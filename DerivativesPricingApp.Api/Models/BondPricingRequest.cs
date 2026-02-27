namespace DerivativesPricingApp.Api.Models;

public class BondPricingRequest
{
    public decimal FaceValue { get; set; }
    public decimal CouponRate { get; set; }
    public int Frequency { get; set; } = 2;
    public decimal YearsToMaturity { get; set; }
    public decimal YieldToMaturity { get; set; }
}
