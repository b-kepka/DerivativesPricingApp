using DerivativesPricingApp.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace DerivativesPricingApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton(_ =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(Constants.ApiBaseUrl)
                };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            });

            builder.Services.AddSingleton<DataApiService>();
            builder.Services.AddSingleton<PricingApiService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
