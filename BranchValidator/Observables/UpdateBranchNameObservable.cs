// <copyright file="UpdateBranchNameObservable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Observables.Core;

namespace BranchValidator.Observables;

/// <summary>
/// Creates a <see cref="Core.IObservable{T}"/> to send push notifications to signal that the position of the mouse has changed.
/// </summary>
public class UpdateBranchNameObservable : Observable<string>
{
    /// <summary>
    /// Sends a push notification to update the name of a branch string.
    /// </summary>
    /// <param name="data">The data to send with the push notification.</param>
    public override void PushNotification(string data)
    {
        for (var i = Observers.Count - 1; i >= 0; i--)
        {
            Observers[i].OnNext(data);
        }
    }
}
