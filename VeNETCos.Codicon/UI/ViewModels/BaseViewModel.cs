﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VeNETCos.Codicon.UI.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    private string? lastError;
    private readonly List<string> errors = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Called right before a property's change is notified
    /// </summary>
    /// <param name="property">The property that changed</param>
    /// <returns>true if the change should be notified, false otherwise</returns>
    protected virtual bool PropertyHasChanged(string property) => true;

    protected void NotifyPropertyChanged([CallerMemberName] string? caller = null)
    {
        Debug.Assert(caller is not null);
        if (PropertyHasChanged(caller))
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
    }

    protected T NotifyPropertyChanged<T>(T field, T value, IEqualityComparer<T>? comparer = null, [CallerMemberName] string? caller = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (comparer.Equals(field, value))
            return field;

        NotifyPropertyChanged(caller);
        return value;
    }

    protected void NotifyPropertyChanged<T>(ref T field, T value, IEqualityComparer<T>? comparer = null, [CallerMemberName] string? caller = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (comparer.Equals(field, value))
            field = value;

        NotifyPropertyChanged(caller);
    }

    public void AddModelError(string error)
    {
        ArgumentException.ThrowIfNullOrEmpty(error);
        errors.Add(error);
        LastError = error;
    }

    public void ClearModelErrors()
    {
        errors.Clear();
        LastError = null;
    }

    public void ClearLastError()
    {
        LastError = null;
    }

    public string? LastError 
    { 
        get => lastError; 
        private set => NotifyPropertyChanged(ref lastError, value); 
    }
}
