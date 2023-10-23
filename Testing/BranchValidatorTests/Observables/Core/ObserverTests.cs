// <copyright file="ObserverTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Observables.Core;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Observables.Core;

/// <summary>
/// Tests the <see cref="Observer{T}"/> class.
/// </summary>
public class ObserverTests
{
    #region Method Tests
    [Fact]
    public void OnNext_WithNullOnNextDelegate_DoesNotThrowException()
    {
        // Arrange
        var observer = new Observer<bool>();

        // Act
        var act = () => observer.OnNext(It.IsAny<bool>());

        // Assert
        act.Should().NotThrow<NullReferenceException>();
    }

    [Fact]
    public void OnNext_WhenInvoked_ExecutesOnNext()
    {
        // Arrange
        var onNextInvoked = false;

        var observer = new Observer<bool>(onNext: _ => onNextInvoked = true);

        // Act
        observer.OnNext(It.IsAny<bool>());

        // Assert
        Assert.True(onNextInvoked);
    }

    [Fact]
    public void OnNext_WhenCompleted_DoesNotExecuteOnNext()
    {
        // Arrange
        var onNextInvoked = false;

        var observer = new Observer<bool>(onNext: _ => onNextInvoked = true);

        // Act
        observer.OnCompleted();
        observer.OnNext(It.IsAny<bool>());

        // Assert
        Assert.False(onNextInvoked);
    }

    [Fact]
    public void OnCompleted_WithNullOnCompletedDelegate_DoesNotThrowException()
    {
        // Arrange
        var observer = new Observer<bool>();

        // Act
        var act = () => observer.OnCompleted();

        // Assert
        act.Should().NotThrow<NullReferenceException>();
    }

    [Fact]
    public void OnCompleted_WhenInvokedAfterCompletion_DoesNotExecuteOnCompleted()
    {
        // Arrange
        var onCompleteInvokedCount = 0;

        var observer = new Observer<bool>(onCompleted: () => onCompleteInvokedCount += 1);

        // Act
        observer.OnCompleted();
        observer.OnCompleted();

        // Assert
        Assert.Equal(1, onCompleteInvokedCount);
    }

    [Fact]
    public void OnCompleted_WhenInvoked_ExecutesOnCompleted()
    {
        // Arrange
        var onNextInvoked = false;
        var onCompletedInvoked = false;

        var observer = new Observer<bool>(onNext: _ => onNextInvoked = true,
            onCompleted: () => onCompletedInvoked = true);

        // Act
        observer.OnCompleted();

        // Assert
        Assert.True(onCompletedInvoked);
        Assert.False(onNextInvoked);
    }

    [Fact]
    public void OnError_WithNullOnErrorDelegate_DoesNotThrowException()
    {
        // Arrange
        var observer = new Observer<bool>();

        // Act
        var act = () => observer.OnError(It.IsAny<Exception>());

        // Assert
        act.Should().NotThrow<NullReferenceException>();
    }

    [Fact]
    public void OnError_WhenInvoked_ExecutesOnError()
    {
        // Arrange
        var onErrorInvoked = false;

        var observer = new Observer<bool>(onError: _ => onErrorInvoked = true);

        // Act
        observer.OnError(It.IsAny<Exception>());

        // Assert
        Assert.True(onErrorInvoked);
    }
    #endregion
}
