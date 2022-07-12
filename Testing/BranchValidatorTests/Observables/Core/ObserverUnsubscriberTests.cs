// <copyright file="ObserverUnsubscriberTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Observables.Core;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Observables.Core;

/// <summary>
/// Tests the <see cref="ObservableUnsubscriber{T}"/> class.
/// </summary>
public class ObserverUnsubscriberTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullObserversParam_ThrowsException()
    {
        // Arrange & Act
        var act = () => _ = new ObservableUnsubscriber<bool>(null, new Observer<bool>());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'observers')");
    }

    [Fact]
    public void Ctor_WithNullObserverParam_ThrowsException()
    {
        // Arrange & Act
        var act = () => _ = new ObservableUnsubscriber<bool>(new List<BranchValidator.Observables.Core.IObserver<bool>>(), null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'observer')");
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsObserverProperty()
    {
        // Arrange
        var expected = new Mock<BranchValidator.Observables.Core.IObserver<It.IsAnyType>>();

        var unsubscriber = new ObservableUnsubscriber<It.IsAnyType>(
            new List<BranchValidator.Observables.Core.IObserver<It.IsAnyType>>(),
            expected.Object);

        // Act
        var actual = unsubscriber.Observer;

        // Assert
        Assert.Same(expected.Object, actual);
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsObservers()
    {
        // Arrange
        var observers = new Mock<List<BranchValidator.Observables.Core.IObserver<It.IsAnyType>>>();
        observers.Object.Add(It.IsAny<BranchValidator.Observables.Core.IObserver<It.IsAnyType>>());
        observers.Object.Add(It.IsAny<BranchValidator.Observables.Core.IObserver<It.IsAnyType>>());
        observers.Object.Add(It.IsAny<BranchValidator.Observables.Core.IObserver<It.IsAnyType>>());

        var unsubscriber = new ObservableUnsubscriber<It.IsAnyType>(
            observers.Object,
            new Mock<BranchValidator.Observables.Core.IObserver<It.IsAnyType>>().Object);

        // Act
        var actual = unsubscriber.TotalObservers;

        // Assert
        Assert.Equal(3, actual);
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Dispose_WhenInvoked_RemovesObserverFromObserversList()
    {
        // Arrange
        var observerA = new Observer<int>();
        var observerB = new Observer<int>();
        var observers = new List<BranchValidator.Observables.Core.IObserver<int>> { observerA, observerB };

        /* NOTE: The observer is added to the list of observers
         * via the Observer<T> class then both the list and the single
         * observer are passed to the ObserverUnsubscriber.  The single
         * observer is added to the observers list above to simulate
         * this process for the purpose of testing.
         */
        var unsubscriber = new ObservableUnsubscriber<int>(
            observers,
            observerB);

        // Act
        unsubscriber.Dispose();
        unsubscriber.Dispose();

        // Assert
        Assert.Equal(1, unsubscriber.TotalObservers);
    }
    #endregion
}
