namespace DerivativesPricingApp.Api.Services;

public class OptionPricingService
{
    /// <summary>
    /// European option price under Black-Scholes. Call: C = S*N(d1) - K*e^(-rT)*N(d2). Put: P = K*e^(-rT)*N(-d2) - S*N(-d1).
    /// </summary>
    public decimal PriceOption(decimal spot, decimal strike, decimal timeToMaturityYears, decimal riskFreeRate, decimal volatility, bool isCall)
    {
        if (timeToMaturityYears <= 0 || volatility <= 0)
            return 0;

        var (d1, d2) = ComputeD1D2(spot, strike, timeToMaturityYears, riskFreeRate, volatility);
        var discountFactor = (decimal)Math.Exp(-(double)riskFreeRate * (double)timeToMaturityYears);

        if (isCall)
            return spot * Cdf(d1) - strike * discountFactor * Cdf(d2);
        return strike * discountFactor * Cdf(-d2) - spot * Cdf(-d1);
    }

    /// <summary>
    /// d1 and d2 from Black-Scholes: d1 = (ln(S/K) + (r + sigma^2/2)*T) / (sigma*sqrt(T)), d2 = d1 - sigma*sqrt(T).
    /// </summary>
    private static (decimal d1, decimal d2) ComputeD1D2(decimal spot, decimal strike, decimal t, decimal r, decimal sigma)
    {
        var sigmaD = (double)sigma;
        var tD = (double)t;
        var lnSk = Math.Log((double)spot / (double)strike);
        var sigmaSqrtT = sigmaD * Math.Sqrt(tD);
        var d1 = (lnSk + ((double)r + 0.5 * sigmaD * sigmaD) * tD) / sigmaSqrtT;
        var d2 = d1 - sigmaSqrtT;
        return ((decimal)d1, (decimal)d2);
    }

    /// <summary>
    /// Standard normal CDF N(x). Uses Abramowitz & Stegun 26.2.17 approximation (polynomial in t = 1/(1+p*|x|)).
    /// </summary>
    private static decimal Cdf(decimal x)
    {
        var xD = (double)x;
        if (xD < -7.5) return 0;
        if (xD > 7.5) return 1;
        var t = 1 / (1 + 0.2316419 * Math.Abs(xD));
        var d = 0.3989423 * Math.Exp(-xD * xD / 2) * t * (0.3193815 + t * (-0.3565638 + t * (1.781478 + t * (-1.821256 + t * 1.330274))));
        var p = 1 - d;
        return (decimal)(xD < 0 ? 1 - p : p);
    }
}
