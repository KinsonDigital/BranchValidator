// <copyright file="ParsingServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using FluentAssertions;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="ParsingService"/> class.
/// </summary>
public class ParsingServiceTests
{
    #region Method Tests
    [Theory]
    [InlineData("System.Boolean MyFunc1(System.String value)", "myFunc1(value: string): bool")]
    [InlineData("System.Boolean MyFunc2()", "myFunc2(): bool")]
    [InlineData("System.Boolean MyFunc3(System.String value1, System.String value2)", "myFunc3(value1: string, value2: string): bool")]
    [InlineData("System.Boolean MyFunc4(System.String value1, System.UInt32 value2)", "myFunc4(value1: string, value2: number): bool")]
    [InlineData("System.Boolean MyFunc5(System.UInt32 value1, System.UInt32 value2)", "myFunc5(value1: number, value2: number): bool")]
    public void ToExpressionFunctionSignature_WhenInvoked_ReturnsCorrectResult(
        string signature,
        string expected)
    {
        // Arrange
        var service = new ParsingService();

        // Act
        var actual = service.ToExpressionFunctionSignature(signature);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
