// <copyright file="EnsureThatTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;

namespace BranchValidatorSharedTests;

using System;
using Xunit;
using BranchValidatorShared;

/// <summary>
/// Tests the <see cref="EnsureThat"/> class.
/// </summary>
public class EnsureThatTests
{
    #region Method Tests
    [Fact]
    public void ParamIsNotNull_WithNullValue_ThrowsException()
    {
        // Arrange
        object? nullObj = null;

        // Act
        var act = () => EnsureThat.ParamIsNotNull(nullObj);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'nullObj')");
    }

    [Fact]
    public void ParamIsNotNull_WithNonNullValue_DoesNotThrowException()
    {
        // Arrange
        object nonNullObj = "non-null-obj";

        // Act
        var act = () => EnsureThat.ParamIsNotNull(nonNullObj);

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void StringParamIsNotNullOrEmpty_WhenInvoked_ThrowsException(string value)
    {
        // Act
        var act = () => EnsureThat.StringParamIsNotNullOrEmpty(value);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"The string parameter must not be null or empty. (Parameter '{nameof(value)}')");
    }
    #endregion
}
