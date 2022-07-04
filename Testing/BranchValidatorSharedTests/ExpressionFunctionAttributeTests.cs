// <copyright file="ExpressionFunctionAttributeTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidatorShared;
using FluentAssertions;

namespace BranchValidatorSharedTests;

/// <summary>
/// Tests the <see cref="ExpressionFunctionAttribute"/> class.
/// </summary>
public class ExpressionFunctionAttributeTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_CorrectlySetsProperty()
    {
        // Arrange
        const string expected = "test-func-name";

        // Act
        var attribute = new ExpressionFunctionAttribute(expected);
        var actual = attribute.FunctionName;

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
