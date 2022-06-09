// <copyright file="ObservableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Observables.Core;
using BranchValidatorTests.Fakes;
using BranchValidatorTests.Helpers;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Observables.Core;

/// <summary>
/// Tests the <see cref="Observable{TData}"/> class.
/// </summary>
public class ObservableTests
{
    #region Method Tests
    [Fact]
    public void Subscribe_WithNullObserverParam_ThrowsException()
    {
        // Arrange
        var observable = CreateObservable<bool>();

        // Act
        var act = () => observable.Subscribe(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'observer')");
    }

    [Fact]
    public void Subscribe_WhenAddingNewObserver_ObserverAddedToObservable()
    {
        // Arrange
        var observable = CreateObservable<bool>();

        // Act
        observable.Subscribe(new Observer<bool>());

        // Assert
        Assert.Single(observable.Observers);
    }

    [Fact]
    public void Subscribe_WhenAddingNewObserver_ReturnsUnsubscriber()
    {
        // Arrange
        var observable = CreateObservable<bool>();
        var observer = new Observer<bool>();

        // Act
        var actual = (ObservableUnsubscriber<bool>)observable.Subscribe(observer);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ObservableUnsubscriber<bool>>(actual);
        Assert.Same(observer, actual.Observer);
    }

    [Fact]
    public void EndNotifications_WhenInvoked_CompletesAllObservers()
    {
        // Arrange
        var observable = CreateObservable<bool>();
        var mockObserverA = new Mock<BranchValidator.Observables.Core.IObserver<bool>>();
        var mockObserverB = new Mock<BranchValidator.Observables.Core.IObserver<bool>>();

        observable.Subscribe(mockObserverA.Object);
        observable.Subscribe(mockObserverB.Object);

        // Act
        observable.EndNotifications();
        observable.EndNotifications();

        // Assert
        mockObserverA.VerifyOnce(m => m.OnCompleted());
    }

    [Fact]
    public void Dispose_WhenInvokedWithObservers_RemovesObservers()
    {
        // Arrange
        var observable = CreateObservable<bool>();

        observable.Subscribe(new Observer<bool>());

        // Act
        observable.Dispose();
        observable.Dispose();

        // Assert
        Assert.Empty(observable.Observers);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of the abstract <see cref="Observable{TData}"/> for the purpose of testing.
    /// </summary>
    /// <typeparam name="T">The type of data that the <see cref="BranchValidator.Observables.Core.IObservable{T}"/> will deal with.</typeparam>
    /// <returns>The instance to test.</returns>
    private static ObservableFake<T> CreateObservable<T>()
        => new ();
}
