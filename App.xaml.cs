#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace ToDoMauiBlazor;

public partial class App : Application
{
    

    public App()
    {
        CreateDatabase();

        InitializeComponent();

        MainPage = new MainPage();

        int width = (int)MainPage.WidthRequest + 16;
        int height = (int)MainPage.HeightRequest + 39;

        SetAppWindowSize(width, height);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        // set title
        if (window != null)
        {
            window.Title = "ToDo App";
        }

        return window!;
    }

    private static void SetAppWindowSize(int width, int height)
    {
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
#if WINDOWS
            var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(width, height));
#endif
        });
    }


    /// <summary>
    /// Checks if AppDataDirectory and Database exist. If not, copies a new empty database to the app directory.
    /// </summary>
    private static void CreateDatabase()
    {
        string appDirectory = FileSystem.AppDataDirectory;
        if (!Directory.Exists(appDirectory))
        {
            Directory.CreateDirectory(appDirectory);
        }
        DirectoryInfo directoryInfo = new(appDirectory);
        FileInfo[] files = directoryInfo.GetFiles();

        if (!files.Any(f => f.Name == "todo.db"))
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("todo.db").Result;
            using var mstream = new MemoryStream();
            stream.CopyTo(mstream);
            using var output = File.Create($"{appDirectory}\\todo.db");
            output.Write(mstream.ToArray());
        }
    }
}
