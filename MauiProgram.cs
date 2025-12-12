using IPR1App.Services;
using Microsoft.Extensions.Http;

namespace IPR1App;

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

        // Регистрация HttpClient и IRateService
        builder.Services.AddHttpClient<IRateService, RateService>();

        // Регистрация страниц (обязательно для DI через конструктор)
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<IntegralPage>();
        builder.Services.AddTransient<CurrencyConverterPage>();

        return builder.Build();
    }
}