namespace DerivativesPricingApp.Api.Data.Entities;

public class Trade
{
    public int Id { get; set; }
    public string InstrumentType { get; set; } = string.Empty;
    public decimal Notional { get; set; }
    public DateTime TradeDate { get; set; }
    public int? CounterpartyId { get; set; }

    public Counterparty? Counterparty { get; set; }
    public ICollection<CollateralPosition> CollateralPositions { get; set; } = new List<CollateralPosition>();
}
