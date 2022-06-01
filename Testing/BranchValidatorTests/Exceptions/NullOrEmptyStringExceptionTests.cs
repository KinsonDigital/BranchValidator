// <copyright file="NullOrEmptyStringExceptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Exceptions;
using FluentAssertions;

namespace BranchValidatorTests.Exceptions;

/// <summary>
/// Tests the <see cref="NullOrEmptyStringException"/> class.
/// </summary>
public class NullOrEmptyStringExceptionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNoParam_CorrectlySetsExceptionMessage()
    {
        // Act
        var exception = new NullOrEmptyStringException();

        // Assert
        exception.Message.Should().Be("The string must not be null or empty.");
        exception.HResult.Should().Be(50);
    }

    [Fact]
    public void Ctor_WhenInvokedWithSingleMessageParam_CorrectlySetsMessage()
    {
        // Act
        var exception = new NullOrEmptyStringException("test-message");

        // Assert
        exception.Message.Should().Be("test-message");
        exception.HResult.Should().Be(50);
    }

    [Fact]
    public void Ctor_WhenInvokedWithMessageAndInnerException_ThrowsException()
    {
        // Arrange
        var innerException = new Exception("inner-exception");

        // Act
        var deviceException = new NullOrEmptyStringException("test-exception", innerException);

        // Assert
        deviceException.InnerException.Message.Should().Be("inner-exception");
        deviceException.Message.Should().Be("test-exception");
        deviceException.HResult.Should().Be(50);
    }
    #endregion
}
