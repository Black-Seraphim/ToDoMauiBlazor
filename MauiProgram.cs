using ToDoMauiBlazor.Data;
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

        builder.Services.AddScoped<ToDoService>();

        return builder.Build();
    }
}
