using DerivativesPricingApp.Api.Models;
using DerivativesPricingApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DerivativesPricingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    private readonly OptionPricingService _optionPricingService;
    private readonly BondPricingService _bondPricingService;

    public PricingController(OptionPricingService optionPricingService, BondPricingService bondPricingService)
    {
        _optionPricingService = optionPricingService;
        _bondPricingService = bondPricingService;
    }

    [HttpPost("options")]
    public ActionResult<OptionPricingResult> PriceOption([FromBody] OptionPricingRequest request)
    {
        var isCall = string.Equals(request.OptionType, "Call", StringComparison.OrdinalIgnoreCase);
        var price = _optionPricingService.PriceOption(
            request.Spot,
            request.Strike,
            request.TimeToMaturityYears,
            request.RiskFreeRate,
            request.Volatility,
            isCall);
        return Ok(new OptionPricingResult { Price = price });
    }

    [HttpPost("bonds")]
    public ActionResult<BondPricingResult> PriceBond([FromBody] BondPricingRequest request)
    {
        var price = _bondPricingService.PriceBond(
            request.FaceValue,
            request.CouponRate,
            request.Frequency,
            request.YearsToMaturity,
            request.YieldToMaturity);
        return Ok(new BondPricingResult { Price = price });
    }
}
