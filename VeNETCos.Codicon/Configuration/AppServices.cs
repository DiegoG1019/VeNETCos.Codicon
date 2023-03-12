using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VeNETCos.Codicon.Configuration;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Services.Containers;

namespace VeNETCos.Codicon;

public static partial class AppServices
{
    #region Public Use

    private static readonly IServiceProvider serviceProvider;

    static AppServices()
    {
        var s = new ServiceCollection();
        BuildServices(s, AppConfiguration.Configuration);
        serviceProvider = s.BuildServiceProvider();
    }

    private static T Create<T>() where T : ServicesContainer, new()
        => new() { Scope = serviceProvider.CreateScope() };

    public static ServicesContainer<AppDbContext> GetDbContext(out AppDbContext context)
        => Create<ServicesContainer<AppDbContext>>().Get(out context);

    public static ServicesContainer<T> GetServices<T>()
        where T : class
        => Create<ServicesContainer<T>>();

    public static ServicesContainer<T1, T2> GetServices<T1, T2>()
        where T1 : class
        where T2 : class
        => Create<ServicesContainer<T1, T2>>();

    public static ServicesContainer<T1, T2, T3> GetServices<T1, T2, T3>()
        where T1 : class
        where T2 : class
        where T3 : class
        => Create<ServicesContainer<T1, T2, T3>>();

    public static ServicesContainer<T1, T2, T3, T4> GetServices<T1, T2, T3, T4>()
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        => Create<ServicesContainer<T1, T2, T3, T4>>();

    public static ServicesContainer<T1, T2, T3, T4, T5> GetServices<T1, T2, T3, T4, T5>()
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        => Create<ServicesContainer<T1, T2, T3, T4, T5>>();

    public static ServicesContainer<T1, T2, T3, T4, T5, T6> GetServices<T1, T2, T3, T4, T5, T6>()
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        => Create<ServicesContainer<T1, T2, T3, T4, T5, T6>>();

    public static ServicesContainer<T1, T2, T3, T4, T5, T6, T7> GetServices<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        => Create<ServicesContainer<T1, T2, T3, T4, T5, T6, T7>>();

    public static ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8> GetServices<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
        => Create<ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8>>();

    public static ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8, T9> GetServices<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
        where T9 : class
        => Create<ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();

    public static ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> GetServices<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
        where T9 : class
        where T10 : class
        => Create<ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();

    #endregion
}
