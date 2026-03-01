using System.Net.Http.Json;
using System.Text.Json;
using DerivativesPricingApp.Models;
using DerivativesPricingApp.Models.Requests;

namespace DerivativesPricingApp.Services;

public class DataApiService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly HttpClient _http;

    public DataApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<Counterparty>> GetCounterpartiesAsync(CancellationToken ct = default)
    {
        var list = await GetAsync<List<Counterparty>>("api/counterparties", ct);
        return list ?? new List<Counterparty>();
    }

    public async Task<IReadOnlyList<Trade>> GetTradesAsync(CancellationToken ct = default)
    {
        var list = await GetAsync<List<Trade>>("api/trades", ct);
        return list ?? new List<Trade>();
    }

    public async Task<IReadOnlyList<HistoricalPrice>> GetHistoricalPricesAsync(CancellationToken ct = default)
    {
        var list = await GetAsync<List<HistoricalPrice>>("api/historicalprices", ct);
        return list ?? new List<HistoricalPrice>();
    }

    public async Task<IReadOnlyList<CollateralPosition>> GetCollateralAsync(CancellationToken ct = default)
    {
        var list = await GetAsync<List<CollateralPosition>>("api/collateral", ct);
        return list ?? new List<CollateralPosition>();
    }

    public async Task<CollateralPosition?> GetCollateralByIdAsync(int id, CancellationToken ct = default)
    {
        return await GetAsync<CollateralPosition>($"api/collateral/{id}", ct);
    }

    public async Task<CollateralPosition?> CreateCollateralAsync(CollateralCreateRequest request, CancellationToken ct = default)
    {
        return await PostAsync<CollateralCreateRequest, CollateralPosition>("api/collateral", request, ct);
    }

    public async Task<CollateralPosition?> UpdateCollateralAsync(int id, CollateralUpdateRequest request, CancellationToken ct = default)
    {
        return await PutAsync<CollateralUpdateRequest, CollateralPosition>($"api/collateral/{id}", request, ct);
    }

    public async Task<CollateralPosition?> PatchCollateralAsync(int id, CollateralPatchRequest request, CancellationToken ct = default)
    {
        return await PatchAsync<CollateralPatchRequest, CollateralPosition>($"api/collateral/{id}", request, ct);
    }

    public async Task<IReadOnlyList<MarginCall>> GetMarginCallsAsync(CancellationToken ct = default)
    {
        var list = await GetAsync<List<MarginCall>>("api/margincalls", ct);
        return list ?? new List<MarginCall>();
    }

    public async Task<MarginCall?> GetMarginCallByIdAsync(int id, CancellationToken ct = default)
    {
        return await GetAsync<MarginCall>($"api/margincalls/{id}", ct);
    }

    public async Task<MarginCall?> CreateMarginCallAsync(MarginCallCreateRequest request, CancellationToken ct = default)
    {
        return await PostAsync<MarginCallCreateRequest, MarginCall>("api/margincalls", request, ct);
    }

    public async Task<MarginCall?> UpdateMarginCallAsync(int id, MarginCallUpdateRequest request, CancellationToken ct = default)
    {
        return await PutAsync<MarginCallUpdateRequest, MarginCall>($"api/margincalls/{id}", request, ct);
    }

    public async Task<MarginCall?> PatchMarginCallAsync(int id, MarginCallPatchRequest request, CancellationToken ct = default)
    {
        return await PatchAsync<MarginCallPatchRequest, MarginCall>($"api/margincalls/{id}", request, ct);
    }

    private async Task<T?> GetAsync<T>(string requestUri, CancellationToken ct)
    {
        var response = await _http.GetAsync(requestUri, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
    }

    private async Task<TResponse?> PostAsync<TRequest, TResponse>(string requestUri, TRequest body, CancellationToken ct)
    {
        var response = await _http.PostAsJsonAsync(requestUri, body, JsonOptions, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct);
    }

    private async Task<TResponse?> PutAsync<TRequest, TResponse>(string requestUri, TRequest body, CancellationToken ct)
    {
        var response = await _http.PutAsJsonAsync(requestUri, body, JsonOptions, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct);
    }

    private async Task<TResponse?> PatchAsync<TRequest, TResponse>(string requestUri, TRequest body, CancellationToken ct)
    {
        var response = await _http.PatchAsJsonAsync(requestUri, body, JsonOptions, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct);
    }
}
