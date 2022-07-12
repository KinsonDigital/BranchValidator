// <copyright file="NullOrEmptyStringArgumentExceptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Exceptions;
using FluentAssertions;

namespace BranchValidatorTests.Exceptions;

/// <summary>
/// Tests the <see cref="NullOrEmptyStringArgumentException"/> class.
/// </summary>
public class NullOrEmptyStringArgumentExceptionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNoParam_CorrectlySetsExceptionMessage()
    {
        // Act
        var exception = new NullOrEmptyStringArgumentException();

        // Assert
        exception.Message.Should().Be("The string argument must not be null or empty.");
        exception.HResult.Should().Be(50);
    }

    [Fact]
    public void Ctor_WhenInvokedWithSingleMessageParam_CorrectlySetsMessage()
    {
        // Act
        var exception = new NullOrEmptyStringArgumentException("test-message");

        // Assert
        exception.Message.Should().Be("test-message");
        exception.HResult.Should().Be(50);
    }

    [Fact]
    public void Ctor_WhenInvokedWithParamNameAndMessageParams_CorrectlySetsMessage()
    {
        // Act
        var exception = new NullOrEmptyStringArgumentException("test-param", "test-message");

        // Assert
        exception.Message.Should().Be("test-message (Parameter 'test-param')");
        exception.HResult.Should().Be(50);
    }

    [Fact]
    public void Ctor_WhenInvokedWithMessageAndInnerException_ThrowsException()
    {
        // Arrange
        var innerException = new Exception("inner-exception");

        // Act
        var deviceException = new NullOrEmptyStringArgumentException("test-exception", innerException);

        // Assert
        deviceException.InnerException.Message.Should().Be("inner-exception");
        deviceException.Message.Should().Be("test-exception");
        deviceException.HResult.Should().Be(50);
    }
    #endregion
}
