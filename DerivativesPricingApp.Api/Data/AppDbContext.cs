using DerivativesPricingApp.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<HistoricalPrice> HistoricalPrices => Set<HistoricalPrice>();
    public DbSet<Trade> Trades => Set<Trade>();
    public DbSet<Counterparty> Counterparties => Set<Counterparty>();
    public DbSet<CollateralPosition> CollateralPositions => Set<CollateralPosition>();
    public DbSet<MarginCall> MarginCalls => Set<MarginCall>();
}
