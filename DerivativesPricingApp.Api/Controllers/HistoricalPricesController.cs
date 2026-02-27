using DerivativesPricingApp.Api.Data;
using DerivativesPricingApp.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoricalPricesController : ControllerBase
{
    private readonly AppDbContext _db;

    public HistoricalPricesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HistoricalPrice>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _db.HistoricalPrices.ToListAsync(cancellationToken));
    }
}
