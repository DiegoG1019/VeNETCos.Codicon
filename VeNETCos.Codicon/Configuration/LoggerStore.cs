using System;
using Serilog.Events;
using Serilog;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;

namespace VeNETCos.Codicon.Configuration;

/// <summary>
/// Represents a store location for <see cref="ILogger"/> objects relating to a specific object
/// </summary>
public static class LoggerStore
{
    /// <summary>
    /// Encapsulates a given logger, keeping track of its last use time for cleanup purposes
    /// </summary>
    /// <param name="Logger"></param>
    private record class LoggerHandle(ILogger Logger)
    {
        private readonly ILogger logger = Logger;

        public ILogger Logger
        {
            get
            {
                LastUsed = DateTime.Now;
                return logger;
            }
        }

        public DateTime LastUsed { get; private set; } = DateTime.Now;
    }

    private static readonly Task CleanUpTask; // The task that will do the sweeps
    private static readonly ConcurrentDictionary<string, LoggerHandle> Loggers = new(); // The dictionary where we store loggers
    private static readonly CancellationTokenSource Cancellation = new(); // Cancellation source for the task
    private static readonly TimeSpan DeletionThreshold = TimeSpan.FromMinutes(10); // The amount of time a given logger must go unused to be considered for deletion
    private static readonly TimeSpan CleanUpInterval = TimeSpan.FromMinutes(5); // The amount of time to wait between sweeps
    private static readonly LogConfig Config; // The configuration object for loggers

    static LoggerStore()
    {
        var lconf = new LogConfig(AppConfiguration.Configuration.GetRequiredSection("Logging:System"));
        Log.Logger = ConfigureDefaultLogger(
                        lconf,
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] ({Owner})]{NewLine} > {Message:lj}{NewLine}{Exception}",
                        ("Owner", "Ve.NETcos App", false)
                    ).CreateLogger(); // System Logger
        Config = lconf;

        var ct = Cancellation.Token;
        CleanUpTask = Task.Run(async () => // We setup our cleanup task
        {
            await Task.Yield();
            DateTime lastCleanup = DateTime.Now;
            while (ct.IsCancellationRequested is false) // If it's cancelled, we should stop
            {
                await Task.Delay(10_000); // So that it doesn't have to wait the entirety of the CleanUpInterval if a cancellation is requested
                if (DateTime.Now - lastCleanup > CleanUpInterval) // If it's time to sweep, let's-a-sweep
                {
                    lastCleanup = DateTime.Now; // We log our last cleanup time

                    foreach (var item in Loggers.Where(x => DateTime.Now - x.Value.LastUsed > DeletionThreshold).ToArray()) // Fetch all stale loggers (We can't modify a collection while it's being enumerated)
                        Loggers.TryRemove(item); // To heck with 'em
                }
            }
        }, Cancellation.Token); // We pass the cancellation token to the Task.Run call
    }

    public static ILogger GetLogger(string? loggerName)
        => loggerName is null ? Log.Logger : Loggers.GetOrAdd(loggerName, LogFactory).Logger;

    public static ILogger GetLogger(object? obj)
        => GetLogger(obj?.GetType().Name);

    private static LoggerHandle LogFactory(string? owner)
        => new(
            ConfigureDefaultLogger(
                Config,
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] ({Owner})]{NewLine} > {Message:lj}{NewLine}{Exception}",
                ("Owner", owner, false)
            )
            .CreateLogger()
        );

    public static LoggerConfiguration ConfigureDefaultLogger(LogConfig config, string template, params (string Name, object? Value, bool Destructure)[] properties)
    {
        var lc = new LoggerConfiguration().MinimumLevel.Is(config.Minimum);

        if (config.File is LogEventLevel lelf)
            lc.WriteTo.File(
                Path.Combine(config.FileDirectory, ".log"),
                restrictedToMinimumLevel: lelf,
                outputTemplate: template,
                rollingInterval: RollingInterval.Day,
                shared: false,
                flushToDiskInterval: TimeSpan.FromSeconds(5)
            );

        if (config.Console is LogEventLevel lelc)
            lc.WriteTo.Console(
                restrictedToMinimumLevel: lelc,
                outputTemplate: template
            );

        foreach (var (Name, Value, Destructure) in properties)
            lc.Enrich.WithProperty(Name, Value!, Destructure);

        return lc;
    }
}