// <copyright file="FileDataDoesNotExistException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using ScriptGenerator.Exceptions;

namespace ScriptGeneratorTests.Exceptions;

/// <summary>
/// Tests the <see cref="FileDataDoesNotExistException"/> class.
/// </summary>
public class FileDataDoesNotExistExceptionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNoParam_CorrectlySetsExceptionMessage()
    {
        // Act
        var exception = new FileDataDoesNotExistException();

        // Assert
        exception.Message.Should().Be("No file data exists.");
        exception.HResult.Should().Be(10);
    }

    [Fact]
    public void Ctor_WhenInvokedWithSingleMessageParam_CorrectlySetsMessage()
    {
        // Act
        var exception = new FileDataDoesNotExistException("test-message");

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
        var deviceException = new FileDataDoesNotExistException("test-exception", innerException);

        // Assert
        deviceException.InnerException.Message.Should().Be("inner-exception");
        deviceException.Message.Should().Be("test-exception");
        deviceException.HResult.Should().Be(10);
    }
    #endregion
}
