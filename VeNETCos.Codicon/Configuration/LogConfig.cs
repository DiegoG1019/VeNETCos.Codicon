using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace VeNETCos.Codicon.Configuration;

public readonly struct LogConfig
{
    public string FileDirectory { get; }

    public LogEventLevel? File { get; }
    public LogEventLevel? Console { get; }

    public LogEventLevel Minimum
        => (LogEventLevel)Math.Min((int)(File ?? LogEventLevel.Fatal), (int)(Console ?? LogEventLevel.Fatal));

    public LogConfig(IConfigurationSection section)
    {
        var bsec = section.GetRequiredSection("LogLevel");

        var str = bsec.GetValue<string>("File")!;
        File = str is not null ? ParseLevel(str, bsec) : null;

        str = bsec.GetValue<string>("Console")!;
        Console = str is not null ? ParseLevel(str, bsec) : null;

        FileDirectory = section.GetValue<string>("LogFileDirectory") ?? @"C:\WarlockCode\WardianDesktop\SyncDaemon\Logs";
        Directory.CreateDirectory(FileDirectory);
    }

    private static LogEventLevel ParseLevel(string str, IConfigurationSection section)
        => str switch
        {
            "Critical" => LogEventLevel.Fatal,
            "Debug" => LogEventLevel.Debug,
            "Error" => LogEventLevel.Error,
            "Information" => LogEventLevel.Information,
            "None" => (LogEventLevel)6,
            "Trace" => LogEventLevel.Verbose,
            "Warning" => LogEventLevel.Warning,
            _ => throw new ConfigurationErrorsException($"Unknown Log Event Level: \"{str}\"", section.Path, -1)
        };
}