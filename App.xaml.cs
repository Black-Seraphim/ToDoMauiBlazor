using System.Text;

namespace ToDoMauiBlazor;

public partial class App : Application
{
    public App()
    {
        CreateDatabase();

        InitializeComponent();

        MainPage = new MainPage();
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
