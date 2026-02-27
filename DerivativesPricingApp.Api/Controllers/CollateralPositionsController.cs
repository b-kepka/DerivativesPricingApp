using DerivativesPricingApp.Api.Data;
using DerivativesPricingApp.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollateralPositionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public CollateralPositionsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CollateralPosition>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _db.CollateralPositions.Include(c => c.Trade).ToListAsync(cancellationToken));
    }
}
