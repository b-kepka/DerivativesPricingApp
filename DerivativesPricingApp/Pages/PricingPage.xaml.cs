using DerivativesPricingApp.Models.Requests;
using DerivativesPricingApp.Services;

namespace DerivativesPricingApp.Pages;

public partial class PricingPage : ContentPage
{
    private PricingApiService? _pricingApi;

    public PricingPage()
    {
        InitializeComponent();
        OptionTypePicker.ItemsSource = new[] { "Call", "Put" };
        OptionTypePicker.SelectedIndex = 0;
    }

    private PricingApiService GetPricingApi() => _pricingApi ??= Application.Current?.Handler?.MauiContext?.Services?.GetService<PricingApiService>() ?? throw new InvalidOperationException("PricingApiService not found");

    private async void OnOptionCalculateClicked(object? sender, EventArgs e)
    {
        OptionResultLabel.Text = "";
        if (!decimal.TryParse(OptionSpot.Text, out var spot) || !decimal.TryParse(OptionStrike.Text, out var strike) ||
            !decimal.TryParse(OptionTime.Text, out var time) || !decimal.TryParse(OptionRate.Text, out var rate) ||
            !decimal.TryParse(OptionVol.Text, out var vol))
        {
            OptionResultLabel.Text = "Enter valid numbers.";
            return;
        }
        var optionType = OptionTypePicker.SelectedItem?.ToString() ?? "Call";
        try
        {
            var request = new OptionPricingRequest { Spot = spot, Strike = strike, TimeToMaturityYears = time, RiskFreeRate = rate, Volatility = vol, OptionType = optionType };
            var price = await GetPricingApi().PriceOptionAsync(request);
            OptionResultLabel.Text = $"Price: {price:N4}";
        }
        catch (Exception ex)
        {
            OptionResultLabel.Text = $"Error: {ex.Message}";
        }
    }

    private async void OnBondCalculateClicked(object? sender, EventArgs e)
    {
        BondResultLabel.Text = "";
        if (!decimal.TryParse(BondFaceValue.Text, out var face) || !decimal.TryParse(BondCoupon.Text, out var coupon) ||
            !int.TryParse(BondFrequency.Text, out var freq) || !decimal.TryParse(BondYears.Text, out var years) ||
            !decimal.TryParse(BondYtm.Text, out var ytm))
        {
            BondResultLabel.Text = "Enter valid numbers.";
            return;
        }
        try
        {
            var request = new BondPricingRequest { FaceValue = face, CouponRate = coupon, Frequency = freq, YearsToMaturity = years, YieldToMaturity = ytm };
            var price = await GetPricingApi().PriceBondAsync(request);
            BondResultLabel.Text = $"Price: {price:N4}";
        }
        catch (Exception ex)
        {
            BondResultLabel.Text = $"Error: {ex.Message}";
        }
    }
}
