namespace DerivativesPricingApp.Models.Requests;

public class CollateralPatchRequest
{
    public int? TradeId { get; set; }
    public string? AssetId { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? Value { get; set; }
    public DateTime? PostedDate { get; set; }
}
