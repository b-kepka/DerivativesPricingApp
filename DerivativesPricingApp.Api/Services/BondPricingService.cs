namespace DerivativesPricingApp.Api.Services;

public class BondPricingService
{
    /// <summary>
    /// Coupon bond price: P = sum of C/(1+y)^t for t=1..n plus F/(1+y)^n. C = F*c/m per period, y = YTM/m.
    /// </summary>
    public decimal PriceBond(decimal faceValue, decimal couponRate, int frequency, decimal yearsToMaturity, decimal yieldToMaturity)
    {
        if (frequency <= 0 || yearsToMaturity <= 0)
            return 0;

        var n = (int)(yearsToMaturity * frequency);
        if (n <= 0)
            return 0;

        var y = (double)(yieldToMaturity / frequency);
        var c = (double)(faceValue * couponRate / frequency);
        var f = (double)faceValue;

        var pv = 0.0;
        for (var t = 1; t <= n; t++)
            pv += c / Math.Pow(1 + y, t);
        pv += f / Math.Pow(1 + y, n);

        return (decimal)pv;
    }
}
