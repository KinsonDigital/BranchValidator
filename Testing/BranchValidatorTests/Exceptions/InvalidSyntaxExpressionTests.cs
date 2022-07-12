// <copyright file="InvalidSyntaxExpressionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidatorTests.Exceptions;

using BranchValidator.Exceptions;
using FluentAssertions;

/// <summary>
/// Tests the <see cref="InvalidSyntaxExpression"/> class.
/// </summary>
public class InvalidSyntaxExpressionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNoParam_CorrectlySetsExceptionMessage()
    {
        // Act
        var exception = new InvalidSyntaxExpression();

        // Assert
        exception.Message.Should().Be("The expression syntax is invalid.");
        exception.HResult.Should().Be(500);
    }

    [Fact]
    public void Ctor_WhenInvokedWithSingleMessageParam_CorrectlySetsMessage()
    {
        // Act
        var exception = new InvalidSyntaxExpression("test-message");

        // Assert
        exception.Message.Should().Be("test-message");
        exception.HResult.Should().Be(500);
    }

    [Fact]
    public void Ctor_WhenInvokedWithMessageAndInnerException_ThrowsException()
    {
        // Arrange
        var innerException = new Exception("inner-exception");

        // Act
        var deviceException = new InvalidSyntaxExpression("test-exception", innerException);

        // Assert
        deviceException.InnerException.Message.Should().Be("inner-exception");
        deviceException.Message.Should().Be("test-exception");
        deviceException.HResult.Should().Be(500);
    }
    #endregion
}
