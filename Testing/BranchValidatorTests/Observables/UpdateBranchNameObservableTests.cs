// <copyright file="UpdateBranchNameObservableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Observables;
using Moq;

namespace BranchValidatorTests.Observables;

/// <summary>
/// Tests the <see cref="UpdateBranchNameObservable"/> class.
/// </summary>
public class UpdateBranchNameObservableTests
{
    #region Method Tests
    [Fact]
    public void PushNotification_WhenInvoked_SendsPushNotification()
    {
        // Arrange
        // var observer = new Mock<CoreObserver>();
        var observer = new Mock<IStringObserver>();

        var observable = new UpdateBranchNameObservable();
        observable.Subscribe(observer.Object);

        var data = "test-data";

        // Act
        observable.PushNotification(data);

        // Assert
        observer.Verify(m => m.OnNext(data), Times.Once());
    }
    #endregion
}
