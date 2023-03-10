using System.Runtime.InteropServices;
using System.Windows;
using Serilog;

namespace VeNETCos.Codicon;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static bool UseDebugConsole { get; }

    static App()
    {
#if DEBUG
        UseDebugConsole = true;
#else
        UseDebugConsole = Environment.GetCommandLineArgs().Contains("open-console", StringInvariantIgnoreCaseComparison.Instance);
#endif
    }

    protected ILogger Log { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (UseDebugConsole)
            AllocConsole();

        Log = LoggerStore.GetLogger(this);
        Log.Information("Started app");
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (UseDebugConsole)
            FreeConsole();

        base.OnExit(e);
    }

    [LibraryImport("Kernel32")]
    private static partial void AllocConsole();

    [LibraryImport("Kernel32", SetLastError = true)]
    private static partial void FreeConsole();
}
