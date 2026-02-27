using DerivativesPricingApp.Api.Data;
using DerivativesPricingApp.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TradesController : ControllerBase
{
    private readonly AppDbContext _db;

    public TradesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trade>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _db.Trades.Include(t => t.Counterparty).ToListAsync(cancellationToken));
    }
}
