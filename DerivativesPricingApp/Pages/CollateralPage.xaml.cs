using System.Linq;
using DerivativesPricingApp.Models;
using DerivativesPricingApp.Models.Requests;
using DerivativesPricingApp.Services;

namespace DerivativesPricingApp.Pages;

public partial class CollateralPage : ContentPage
{
    private DataApiService? _dataApi;
    private List<CollateralPosition> _items = new();
    private int? _editingId;

    public CollateralPage()
    {
        InitializeComponent();
    }

    private DataApiService GetDataApi() => _dataApi ??= Application.Current?.Handler?.MauiContext?.Services?.GetService<DataApiService>() ?? throw new InvalidOperationException("DataApiService not found");

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadList();
    }

    private async Task LoadList()
    {
        try
        {
            _items = (await GetDataApi().GetCollateralAsync()).ToList();
            CollateralList.ItemsSource = null;
            CollateralList.ItemsSource = _items;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnAddClicked(object? sender, EventArgs e)
    {
        _editingId = null;
        FormTradeId.Text = "";
        FormAssetId.Text = "";
        FormQuantity.Text = "";
        FormValue.Text = "";
        FormPostedDate.Text = "";
        CollateralFormBorder.IsVisible = true;
    }

    private void OnFormCancelClicked(object? sender, EventArgs e)
    {
        CollateralFormBorder.IsVisible = false;
        _editingId = null;
    }

    private async void OnFormSaveClicked(object? sender, EventArgs e)
    {
        if (!decimal.TryParse(FormQuantity.Text, out var qty) || !decimal.TryParse(FormValue.Text, out var value))
        {
            await DisplayAlert("Error", "Enter valid Quantity and Value.", "OK");
            return;
        }
        int? tradeId = string.IsNullOrWhiteSpace(FormTradeId.Text) ? null : int.TryParse(FormTradeId.Text, out var t) ? t : null;
        DateTime? postedDate = string.IsNullOrWhiteSpace(FormPostedDate.Text) ? null : DateTime.TryParse(FormPostedDate.Text, out var d) ? d : null;
        try
        {
            if (_editingId.HasValue)
            {
                await GetDataApi().UpdateCollateralAsync(_editingId.Value, new CollateralUpdateRequest { TradeId = tradeId, AssetId = FormAssetId.Text?.Trim(), Quantity = qty, Value = value, PostedDate = postedDate ?? DateTime.UtcNow });
            }
            else
            {
                await GetDataApi().CreateCollateralAsync(new CollateralCreateRequest { TradeId = tradeId, AssetId = FormAssetId.Text?.Trim(), Quantity = qty, Value = value, PostedDate = postedDate });
            }
            CollateralFormBorder.IsVisible = false;
            _editingId = null;
            await LoadList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnCollateralSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is CollateralPosition pos)
        {
            _editingId = pos.Id;
            FormTradeId.Text = pos.TradeId?.ToString() ?? "";
            FormAssetId.Text = pos.AssetId ?? "";
            FormQuantity.Text = pos.Quantity.ToString();
            FormValue.Text = pos.Value.ToString();
            FormPostedDate.Text = pos.PostedDate.ToString("yyyy-MM-dd");
            CollateralFormBorder.IsVisible = true;
        }
    }

    private async void OnRefreshClicked(object? sender, EventArgs e) => await LoadList();
}
