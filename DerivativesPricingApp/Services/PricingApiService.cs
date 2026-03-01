using System.Net.Http.Json;
using System.Text.Json;
using DerivativesPricingApp.Models.Requests;
using DerivativesPricingApp.Models.Results;

namespace DerivativesPricingApp.Services;

public class PricingApiService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly HttpClient _http;

    public PricingApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<decimal> PriceOptionAsync(OptionPricingRequest request, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/pricing/options", request, JsonOptions, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OptionPricingResult>(JsonOptions, ct);
        return result?.Price ?? 0;
    }

    public async Task<decimal> PriceBondAsync(BondPricingRequest request, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/pricing/bonds", request, JsonOptions, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<BondPricingResult>(JsonOptions, ct);
        return result?.Price ?? 0;
    }
}
