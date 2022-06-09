// <copyright file="Observable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace BranchValidator.Observables.Core;

/// <summary>
/// Defines a provider for push-based notifications.
/// </summary>
/// <typeparam name="TData">The data to send with the notification.</typeparam>
public abstract class Observable<TData> : IObservable<TData>
{
    private readonly List<IObserver<TData>> observers = new ();
    private bool isDisposed;
    private bool notificationsEnded;

    /// <summary>
    /// Gets the list of <see cref="System.IObserver{T}"/> that are subscribed to this <see cref="Observable{TData}"/>.
    /// </summary>
    public ReadOnlyCollection<IObserver<TData>> Observers => new (this.observers);

    /// <inheritdoc/>
    public virtual IDisposable Subscribe(IObserver<TData> observer)
    {
        if (observer is null)
        {
            throw new ArgumentNullException(nameof(observer), "The parameter must not be null.");
        }

        if (!this.observers.Contains(observer))
        {
            this.observers.Add(observer);
        }

        return new ObservableUnsubscriber<TData>(this.observers, observer);
    }

    /// <inheritdoc/>
    public abstract void PushNotification(TData data);

    /// <inheritdoc/>
    public void EndNotifications()
    {
        if (this.notificationsEnded)
        {
            return;
        }

        // ReSharper disable ForCanBeConvertedToForeach
        /* Keep this loop as a for-loop.  Do not convert to for-each.
         * This is due to the Dispose() method possibly being called during
         * iteration of the observers list which will cause an exception.
        */
        for (var i = 0; i < this.observers.Count; i++)
        {
            this.observers[i].OnCompleted();
        }

        // ReSharper restore ForCanBeConvertedToForeach
        this.notificationsEnded = true;
    }

    /// <inheritdoc/>
    public void UnsubscribeAll() => this.observers.Clear();

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    /// <param name="disposing">Disposes managed resources when <c>true</c>.</param>
    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.observers.Clear();
        }

        this.isDisposed = true;
    }
}
