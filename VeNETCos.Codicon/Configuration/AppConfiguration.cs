using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace VeNETCos.Codicon.Configuration;

public static class AppConfiguration
{
    private static string? userProfile;

    public static IConfigurationRoot Configuration { get; }
    public static AppConfigurationSettings Settings { get; }
    public static string UserProfile 
    {
        get => userProfile ?? throw new InvalidOperationException("The user profile has not been set"); 
        set
        {
            if (userProfile is not null)
                throw new InvalidOperationException("The user profile has already been set");
            userProfile = value;
            return;
        }
    }

    static AppConfiguration()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json", true);
#if DEBUG
        config.AddJsonFile("appsettings.Development.json");
#endif
        Configuration = config.Build();

        var lc = new LoggerConfiguration();

        //#error Bring over the log config shenanigans from SyncDaemon

        Settings = Configuration.GetRequiredSection("Settings").Get<AppConfigurationSettings>() ?? throw new ConfigurationErrorsException("Could not read Settings section");
    }
}
