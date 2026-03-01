using DerivativesPricingApp.Api.Data;
using DerivativesPricingApp.Api.Data.Entities;
using DerivativesPricingApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DerivativesPricingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollateralController : ControllerBase
{
    private readonly AppDbContext _db;

    public CollateralController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CollateralPosition>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _db.CollateralPositions.Include(c => c.Trade).ToListAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CollateralPosition>> GetById([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.CollateralPositions.Include(c => c.Trade).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (entity == null)
            return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<CollateralPosition>> Create([FromBody] CollateralCreateRequest request, CancellationToken cancellationToken = default)
    {
        if (request.TradeId.HasValue)
        {
            var tradeExists = await _db.Trades.AnyAsync(t => t.Id == request.TradeId.Value, cancellationToken);
            if (!tradeExists)
                return BadRequest("Invalid TradeId");
        }
        var entity = new CollateralPosition
        {
            TradeId = request.TradeId,
            AssetId = request.AssetId,
            Quantity = request.Quantity,
            Value = request.Value,
            PostedDate = request.PostedDate ?? DateTime.UtcNow
        };
        _db.CollateralPositions.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _db.Entry(entity).Reference(c => c.Trade).LoadAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CollateralPosition>> Put([FromRoute] int id, [FromBody] CollateralUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _db.CollateralPositions.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
            return NotFound();
        if (request.TradeId.HasValue)
        {
            var tradeExists = await _db.Trades.AnyAsync(t => t.Id == request.TradeId.Value, cancellationToken);
            if (!tradeExists)
                return BadRequest("Invalid TradeId");
        }
        entity.TradeId = request.TradeId;
        entity.AssetId = request.AssetId;
        entity.Quantity = request.Quantity;
        entity.Value = request.Value;
        entity.PostedDate = request.PostedDate;
        await _db.SaveChangesAsync(cancellationToken);
        await _db.Entry(entity).Reference(c => c.Trade).LoadAsync(cancellationToken);
        return Ok(entity);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<CollateralPosition>> Patch([FromRoute] int id, [FromBody] CollateralPatchRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _db.CollateralPositions.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
            return NotFound();
        if (request.TradeId.HasValue)
        {
            var tradeExists = await _db.Trades.AnyAsync(t => t.Id == request.TradeId.Value, cancellationToken);
            if (!tradeExists)
                return BadRequest("Invalid TradeId");
        }
        if (request.TradeId.HasValue) entity.TradeId = request.TradeId;
        if (request.AssetId != null) entity.AssetId = request.AssetId;
        if (request.Quantity.HasValue) entity.Quantity = request.Quantity.Value;
        if (request.Value.HasValue) entity.Value = request.Value.Value;
        if (request.PostedDate.HasValue) entity.PostedDate = request.PostedDate.Value;
        await _db.SaveChangesAsync(cancellationToken);
        await _db.Entry(entity).Reference(c => c.Trade).LoadAsync(cancellationToken);
        return Ok(entity);
    }
}
