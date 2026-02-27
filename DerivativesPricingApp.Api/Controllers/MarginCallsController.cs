using DerivativesPricingApp.Api.Data;
using DerivativesPricingApp.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarginCallsController : ControllerBase
{
    private readonly AppDbContext _db;

    public MarginCallsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarginCall>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _db.MarginCalls.Include(m => m.Counterparty).ToListAsync(cancellationToken));
    }
}
