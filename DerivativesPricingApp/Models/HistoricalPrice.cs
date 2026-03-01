namespace DerivativesPricingApp.Models;

public class HistoricalPrice
{
    public int Id { get; set; }
    public string InstrumentId { get; set; } = string.Empty;
    public DateTime AsOfDate { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
}
