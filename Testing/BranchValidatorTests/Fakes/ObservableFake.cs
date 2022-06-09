using BranchValidator.Observables.Core;

namespace BranchValidatorTests.Fakes;

/// <summary>
/// Used for testing the abstract <see cref="Observable{TData}"/> class.
/// </summary>
/// <typeparam name="T">The type of notification to set.</typeparam>
public class ObservableFake<T> : Observable<T>
{
    public override void PushNotification(T data) => throw new NotImplementedException();
}
