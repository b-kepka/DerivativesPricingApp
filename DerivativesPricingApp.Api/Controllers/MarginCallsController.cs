using DerivativesPricingApp.Api.Data;
using DerivativesPricingApp.Api.Data.Entities;
using DerivativesPricingApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarginCallsController : ControllerBase
{
    private static readonly string[] AllowedStatuses = { "Pending", "Met", "Disputed" };

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

    [HttpGet("{id}")]
    public async Task<ActionResult<MarginCall>> GetById([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var marginCall = await _db.MarginCalls.Include(m => m.Counterparty).FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        if (marginCall == null)
            return NotFound();
        return Ok(marginCall);
    }

    [HttpPost]
    public async Task<ActionResult<MarginCall>> Create([FromBody] MarginCallCreateRequest request, CancellationToken cancellationToken = default)
    {
        if (!AllowedStatuses.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
            return BadRequest($"Status must be one of: {string.Join(", ", AllowedStatuses)}");
        var counterparty = await _db.Counterparties.FindAsync(new object[] { request.CounterpartyId }, cancellationToken);
        if (counterparty == null)
            return BadRequest("Invalid CounterpartyId");
        var marginCall = new MarginCall
        {
            CounterpartyId = request.CounterpartyId,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = request.Status,
            CallDate = DateTime.UtcNow
        };
        _db.MarginCalls.Add(marginCall);
        await _db.SaveChangesAsync(cancellationToken);
        await _db.Entry(marginCall).Reference(m => m.Counterparty).LoadAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = marginCall.Id }, marginCall);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MarginCall>> Put([FromRoute] int id, [FromBody] MarginCallUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _db.MarginCalls.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
            return NotFound();
        if (!AllowedStatuses.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
            return BadRequest($"Status must be one of: {string.Join(", ", AllowedStatuses)}");
        var counterparty = await _db.Counterparties.FindAsync(new object[] { request.CounterpartyId }, cancellationToken);
        if (counterparty == null)
            return BadRequest("Invalid CounterpartyId");
        entity.CounterpartyId = request.CounterpartyId;
        entity.CallDate = request.CallDate;
        entity.Amount = request.Amount;
        entity.Currency = request.Currency;
        entity.Status = request.Status;
        await _db.SaveChangesAsync(cancellationToken);
        await _db.Entry(entity).Reference(m => m.Counterparty).LoadAsync(cancellationToken);
        return Ok(entity);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<MarginCall>> Patch(int id, [FromBody] MarginCallPatchRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _db.MarginCalls.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
            return NotFound();

        if (request.Status != null)
        {
            if (!AllowedStatuses.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
                return BadRequest($"Status must be one of: {string.Join(", ", AllowedStatuses)}");
            entity.Status = request.Status;
        }
        if (request.Amount.HasValue)
            entity.Amount = request.Amount.Value;
        if (request.Currency != null)
            entity.Currency = request.Currency;

        await _db.SaveChangesAsync(cancellationToken);
        await _db.Entry(entity).Reference(m => m.Counterparty).LoadAsync(cancellationToken);
        return Ok(entity);
    }
}
