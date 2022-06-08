// <copyright file="InvalidActionInputTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Exceptions;
using FluentAssertions;

namespace BranchValidatorTests.Exceptions;

/// <summary>
/// Tests the <see cref="InvalidActionInput"/> class.
/// </summary>
public class InvalidActionInputTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNoParam_CorrectlySetsExceptionMessage()
    {
        // Act
        var exception = new InvalidActionInput();

        // Assert
        exception.Message.Should().Be("The action input value is invalid.");
        exception.HResult.Should().Be(10);
    }

    [Fact]
    public void Ctor_WhenInvokedWithSingleMessageParam_CorrectlySetsMessage()
    {
        // Act
        var exception = new InvalidActionInput("test-message");

        // Assert
        exception.Message.Should().Be("test-message");
        exception.HResult.Should().Be(10);
    }

    [Fact]
    public void Ctor_WhenInvokedWithMessageAndInnerException_ThrowsException()
    {
        // Arrange
        var innerException = new Exception("inner-exception");

        // Act
        var deviceException = new InvalidActionInput("test-exception", innerException);

        // Assert
        deviceException.InnerException.Message.Should().Be("inner-exception");
        deviceException.Message.Should().Be("test-exception");
        deviceException.HResult.Should().Be(10);
    }
    #endregion
}
