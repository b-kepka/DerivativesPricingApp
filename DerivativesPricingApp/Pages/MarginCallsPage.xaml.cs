using System.Collections.ObjectModel;
using System.Linq;
using DerivativesPricingApp.Models;
using DerivativesPricingApp.Models.Requests;
using DerivativesPricingApp.Services;

namespace DerivativesPricingApp.Pages;

public partial class MarginCallsPage : ContentPage
{
    private DataApiService? _dataApi;
    private ObservableCollection<MarginCall> _items = new();
    private List<Counterparty> _counterparties = new();

    public MarginCallsPage()
    {
        InitializeComponent();
        FormStatusPicker.ItemsSource = new[] { "Pending", "Met", "Disputed" };
        FormStatusPicker.SelectedIndex = 0;
        MarginCallsList.ItemsSource = _items;
    }

    private DataApiService GetDataApi() => _dataApi ??= Application.Current?.Handler?.MauiContext?.Services?.GetService<DataApiService>() ?? throw new InvalidOperationException("DataApiService not found");

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCounterparties();
        await LoadList();
    }

    private async Task LoadCounterparties()
    {
        try
        {
            _counterparties = (await GetDataApi().GetCounterpartiesAsync()).ToList();
            FormCounterpartyPicker.ItemsSource = _counterparties;
            FormCounterpartyPicker.ItemDisplayBinding = new Binding("Name");
            if (_counterparties.Count > 0)
                FormCounterpartyPicker.SelectedIndex = 0;
        }
        catch { }
    }

    private async Task LoadList()
    {
        try
        {
            var list = await GetDataApi().GetMarginCallsAsync();
            _items.Clear();
            foreach (var item in list)
                _items.Add(item);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnAddClicked(object? sender, EventArgs e)
    {
        FormAmount.Text = "";
        FormCurrency.Text = "EUR";
        FormStatusPicker.SelectedIndex = 0;
        if (_counterparties.Count > 0)
            FormCounterpartyPicker.SelectedIndex = 0;
        MarginCallFormBorder.IsVisible = true;
    }

    private void OnFormCancelClicked(object? sender, EventArgs e)
    {
        MarginCallFormBorder.IsVisible = false;
    }

    private async void OnFormSaveClicked(object? sender, EventArgs e)
    {
        if (FormCounterpartyPicker.SelectedItem is not Counterparty cp)
        {
            await DisplayAlert("Error", "Select a counterparty.", "OK");
            return;
        }
        if (!decimal.TryParse(FormAmount.Text, out var amount))
        {
            await DisplayAlert("Error", "Enter valid amount.", "OK");
            return;
        }
        var currency = FormCurrency.Text?.Trim() ?? "EUR";
        var status = FormStatusPicker.SelectedItem?.ToString() ?? "Pending";
        try
        {
            await GetDataApi().CreateMarginCallAsync(new MarginCallCreateRequest { CounterpartyId = cp.Id, Amount = amount, Currency = currency, Status = status });
            MarginCallFormBorder.IsVisible = false;
            await LoadList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnStatusButtonClicked(object? sender, EventArgs e)
    {
        if (sender is not Button btn || btn.BindingContext is not MarginCall call)
            return;
        var newStatus = btn.Text;
        try
        {
            await GetDataApi().PatchMarginCallAsync(call.Id, new MarginCallPatchRequest { Status = newStatus });
            await LoadList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnRefreshClicked(object? sender, EventArgs e) => await LoadList();
}
