using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace VeNETCos.Codicon.Services.Containers;

public abstract class ServicesContainer : IDisposable
{
    private IServiceScope scope;

    internal IServiceScope Scope
    {
        get => scope;
        init => scope = value ?? throw new ArgumentNullException(nameof(scope));
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal ServicesContainer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public T GetService<T>() where T : class
    {
        Debug.Assert(scope is not null);
        return Scope.ServiceProvider.GetRequiredService<T>();
    }

    public void Dispose()
    {
        Debug.Assert(scope is not null);
        Scope.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class ServicesContainer<T> : ServicesContainer
    where T : class
{
    public ServicesContainer<T> Get(out T service)
    {
        service = GetService<T>();
        return this;
    }
}

public class ServicesContainer<T1, T2> : ServicesContainer
    where T1 : class
    where T2 : class
{
    public ServicesContainer<T1, T2> Get(out T1 s1, out T2 s2)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        return this;
    }
}

public class ServicesContainer<T1, T2, T3> : ServicesContainer
    where T1 : class
    where T2 : class
    where T3 : class
{
    public ServicesContainer<T1, T2, T3> Get(out T1 s1, out T2 s2, out T3 s3)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        s3 = GetService<T3>();
        return this;
    }
}

public class ServicesContainer<T1, T2, T3, T4> : ServicesContainer
    where T1 : class
    where T2 : class
    where T3 : class
    where T4 : class
{
    public ServicesContainer<T1, T2, T3, T4> Get(out T1 s1, out T2 s2, out T3 s3, out T4 s4)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        s3 = GetService<T3>();
        s4 = GetService<T4>();
        return this;
    }
}

public class ServicesContainer<T1, T2, T3, T4, T5> : ServicesContainer
    where T1 : class
    where T2 : class
    where T3 : class
    where T4 : class
    where T5 : class
{
    public ServicesContainer<T1, T2, T3, T4, T5> Get(out T1 s1, out T2 s2, out T3 s3, out T4 s4, out T5 s5)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        s3 = GetService<T3>();
        s4 = GetService<T4>();
        s5 = GetService<T5>();
        return this;
    }
}

public class ServicesContainer<T1, T2, T3, T4, T5, T6> : ServicesContainer
    where T1 : class
    where T2 : class
    where T3 : class
    where T4 : class
    where T5 : class
    where T6 : class
{
    public ServicesContainer<T1, T2, T3, T4, T5, T6> Get(out T1 s1, out T2 s2, out T3 s3, out T4 s4, out T5 s5, out T6 s6)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        s3 = GetService<T3>();
        s4 = GetService<T4>();
        s5 = GetService<T5>();
        s6 = GetService<T6>();
        return this;
    }
}

public class ServicesContainer<T1, T2, T3, T4, T5, T6, T7> : ServicesContainer
    where T1 : class
    where T2 : class
    where T3 : class
    where T4 : class
    where T5 : class
    where T6 : class
    where T7 : class
{
    public ServicesContainer<T1, T2, T3, T4, T5, T6, T7> Get(out T1 s1, out T2 s2, out T3 s3, out T4 s4, out T5 s5, out T6 s6, out T7 s7)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        s3 = GetService<T3>();
        s4 = GetService<T4>();
        s5 = GetService<T5>();
        s6 = GetService<T6>();
        s7 = GetService<T7>();
        return this;
    }
}

public class ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8> : ServicesContainer
    where T1 : class
    where T2 : class
    where T3 : class
    where T4 : class
    where T5 : class
    where T6 : class
    where T7 : class
    where T8 : class
{
    public ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8> Get(out T1 s1, out T2 s2, out T3 s3, out T4 s4, out T5 s5, out T6 s6, out T7 s7, out T8 s8)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        s3 = GetService<T3>();
        s4 = GetService<T4>();
        s5 = GetService<T5>();
        s6 = GetService<T6>();
        s7 = GetService<T7>();
        s8 = GetService<T8>();
        return this;
    }
}

public class ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ServicesContainer
    where T1 : class
    where T2 : class
    where T3 : class
    where T4 : class
    where T5 : class
    where T6 : class
    where T7 : class
    where T8 : class
    where T9 : class
{
    public ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8, T9> Get(out T1 s1, out T2 s2, out T3 s3, out T4 s4, out T5 s5, out T6 s6, out T7 s7, out T8 s8, out T9 s9)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        s3 = GetService<T3>();
        s4 = GetService<T4>();
        s5 = GetService<T5>();
        s6 = GetService<T6>();
        s7 = GetService<T7>();
        s8 = GetService<T8>();
        s9 = GetService<T9>();
        return this;
    }
}

public class ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ServicesContainer
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
{
    public ServicesContainer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Get(out T1 s1, out T2 s2, out T3 s3, out T4 s4, out T5 s5, out T6 s6, out T7 s7, out T8 s8, out T9 s9, out T10 s10)
    {
        s1 = GetService<T1>();
        s2 = GetService<T2>();
        s3 = GetService<T3>();
        s4 = GetService<T4>();
        s5 = GetService<T5>();
        s6 = GetService<T6>();
        s7 = GetService<T7>();
        s8 = GetService<T8>();
        s9 = GetService<T9>();
        s10 = GetService<T10>();
        return this;
    }
}