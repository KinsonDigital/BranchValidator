// <copyright file="IObservable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Observables.Core;

/// <summary>
/// Defines a provider for push-based notification.
/// </summary>
/// <typeparam name="T">The information sent with the push notification.</typeparam>
public interface IObservable<T> : IDisposable
{
    /// <summary>
    /// Subscribes the given <paramref name="observer"/> to receive notifications.
    /// </summary>
    /// <param name="observer">The object that receives notifications.</param>
    /// <returns>
    ///     A reference to an interface that allows <see cref="IObserver{T}"/> to stop receiving
    ///     notifications before the provider has finished sending them.
    /// </returns>
    IDisposable Subscribe(IObserver<T> observer);

    /// <summary>
    /// Pushes a single notification with the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data to send with the push notification.</param>
    void PushNotification(T data);

    /// <summary>
    /// Ends notifications by invoking <see cref="Observer{T}.OnCompleted"/> to all subscribed <see cref="IObserver{T}"/>s.
    /// </summary>
    /// <remarks>
    ///     Will not invoke the <see cref="IObserver{T}"/>.<see cref="IObserver{T}.OnCompleted"/> more than once.
    /// </remarks>
    void EndNotifications();

    /// <summary>
    /// Unsubscribes all of the currently subscribed <see cref="IObserver{T}"/>s.
    /// </summary>
    void UnsubscribeAll();
}
