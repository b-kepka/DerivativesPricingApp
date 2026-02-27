namespace DerivativesPricingApp.Api.Data.Entities;

public class Counterparty
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Trade> Trades { get; set; } = new List<Trade>();
    public ICollection<MarginCall> MarginCalls { get; set; } = new List<MarginCall>();
}
