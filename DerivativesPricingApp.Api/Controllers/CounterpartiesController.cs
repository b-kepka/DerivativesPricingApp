using DerivativesPricingApp.Api.Data;
using DerivativesPricingApp.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CounterpartiesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CounterpartiesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Counterparty>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _db.Counterparties.ToListAsync(cancellationToken));
    }
}
