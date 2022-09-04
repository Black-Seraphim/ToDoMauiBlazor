using Microsoft.AspNetCore.Components.WebView.Maui;
using ToDoMauiBlazor.Data;
using Microsoft.Extensions.DependencyInjection;
using ToDoMauiBlazor.Services;

namespace ToDoMauiBlazor;

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
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddDbContext<ToDoContext>();

        builder.Services.AddScoped<ToDoListService>();
        builder.Services.AddScoped<ToDoTaskService>();

        return builder.Build();
    }
}
