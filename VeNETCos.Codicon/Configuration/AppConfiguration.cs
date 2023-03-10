using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace VeNETCos.Codicon.Configuration;

public static class AppConfiguration
{
    public static IConfigurationRoot Configuration { get; }
    public static AppConfigurationSettings Settings { get; }

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
