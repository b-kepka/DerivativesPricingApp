using DerivativesPricingApp.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        if (db.Counterparties.Any())
            return;

        var counterparties = new[]
        {
            new Counterparty { Name = "Bank Alpha SA" },
            new Counterparty { Name = "Fund Beta LLC" },
            new Counterparty { Name = "Corp Gamma Inc" },
            new Counterparty { Name = "Deutsche Kapital AG" },
            new Counterparty { Name = "Nordic Asset Management" },
            new Counterparty { Name = "Pacific Equity Partners" },
            new Counterparty { Name = "London Clearing House Ltd" },
            new Counterparty { Name = "Tokyo Securities Co" },
            new Counterparty { Name = "Swiss Private Bank SA" },
            new Counterparty { Name = "Hedge Fund Delta" },
            new Counterparty { Name = "Pension Fund Europa" },
            new Counterparty { Name = "Energy Corp International" },
            new Counterparty { Name = "Tech Holdings PLC" },
            new Counterparty { Name = "Municipal Finance Authority" },
            new Counterparty { Name = "Insurance Group North" },
            new Counterparty { Name = "Sovereign Wealth Fund" },
            new Counterparty { Name = "Commodity Trading Desk" }
        };
        db.Counterparties.AddRange(counterparties);
        db.SaveChanges();

        var trades = new List<Trade>();
        var baseDate = new DateTime(2024, 1, 1);
        var types = new[] { "Option", "Bond", "Option", "Swap", "Option", "Bond", "Option", "Swap" };
        var notionals = new[] { 500_000m, 1_000_000m, 2_000_000m, 5_000_000m, 10_000_000m, 500_000m, 3_000_000m, 750_000m };
        for (var i = 0; i < 48; i++)
        {
            trades.Add(new Trade
            {
                InstrumentType = types[i % types.Length],
                Notional = notionals[i % notionals.Length],
                TradeDate = baseDate.AddDays(15 * (i % 20)).AddMonths(i / 10),
                CounterpartyId = counterparties[i % counterparties.Length].Id
            });
        }
        db.Trades.AddRange(trades);
        db.SaveChanges();

        var instruments = new[] { "OPT-001", "OPT-002", "OPT-003", "BOND-X", "BOND-Y", "BOND-Z", "SWAP-A", "SWAP-B", "OPT-004", "OPT-005" };
        var historicalPrices = new List<HistoricalPrice>();
        var priceBase = new decimal[] { 12.50m, 8.20m, 15.00m, 98.75m, 102.30m, 95.00m, 100m, 100m, 22.10m, 5.50m };
        for (var i = 0; i < instruments.Length; i++)
        {
            for (var d = 0; d < 12; d++)
            {
                var date = new DateTime(2024, 1, 1).AddMonths(d).AddDays(d * 2);
                var variation = (decimal)((d % 5 - 2) * 0.15);
                historicalPrices.Add(new HistoricalPrice
                {
                    InstrumentId = instruments[i],
                    AsOfDate = date,
                    Price = priceBase[i] + variation * (i + 1),
                    Currency = i % 3 == 0 ? "EUR" : (i % 3 == 1 ? "USD" : "GBP")
                });
            }
        }
        db.HistoricalPrices.AddRange(historicalPrices);

        var collateralPositions = new List<CollateralPosition>();
        var assets = new[] { "CASH-EUR", "CASH-USD", "GOV-BOND", "CORP-BOND", "GOV-BOND", "EQUITY-BASKET", "CASH-GBP", "GOV-BOND" };
        var quantities = new[] { 50_000m, 100_000m, 200m, 500m, 75_000m, 1000m, 30_000m, 150m };
        var values = new[] { 50_000m, 100_000m, 10_200m, 52_000m, 15_300m, 45_000m, 30_000m, 15_000m };
        for (var i = 0; i < 24; i++)
        {
            collateralPositions.Add(new CollateralPosition
            {
                TradeId = trades[i].Id,
                AssetId = assets[i % assets.Length],
                Quantity = quantities[i % quantities.Length],
                Value = values[i % values.Length],
                PostedDate = trades[i].TradeDate.AddDays(1 + (i % 5))
            });
        }
        db.CollateralPositions.AddRange(collateralPositions);

        var statuses = new[] { "Pending", "Met", "Met", "Disputed", "Pending", "Met" };
        var marginCalls = new List<MarginCall>();
        var amounts = new[] { 25_000m, 100_000m, 50_000m, 200_000m, 75_000m, 30_000m, 150_000m, 40_000m };
        var currencies = new[] { "EUR", "USD", "GBP", "USD", "EUR", "USD", "EUR", "GBP" };
        for (var i = 0; i < 22; i++)
        {
            marginCalls.Add(new MarginCall
            {
                CounterpartyId = counterparties[i % counterparties.Length].Id,
                CallDate = new DateTime(2024, 4, 1).AddDays(10 * i),
                Amount = amounts[i % amounts.Length],
                Currency = currencies[i % currencies.Length],
                Status = statuses[i % statuses.Length]
            });
        }
        db.MarginCalls.AddRange(marginCalls);

        db.SaveChanges();
    }
}
