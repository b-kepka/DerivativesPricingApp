namespace DerivativesPricingApp.Models;

public class CollateralPosition
{
    public int Id { get; set; }
    public int? TradeId { get; set; }
    public string? AssetId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Value { get; set; }
    public DateTime PostedDate { get; set; }
    public Trade? Trade { get; set; }
}
