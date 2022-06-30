// <copyright file="NegativeNumberAnalyzerTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Analyzers;
using FluentAssertions;

namespace BranchValidatorTests.Services.Analyzers;

/// <summary>
/// Tests the <see cref="NegativeNumberAnalyzer"/> class.
/// </summary>
public class NegativeNumberAnalyzerTests
{
    #region Method Tests
    [Theory]
    [InlineData("funA(10)", true, "")]
    [InlineData("funA('-30')", true, "")]
    [InlineData("funA('value1')", true, "")]
    [InlineData("funA(\"value2\")", true, "")]
    [InlineData("funA('str-value')", true, "")]
    [InlineData("funA(40, 50)", true, "")]
    [InlineData("funA()", true, "")]
    [InlineData("funA(10-0)", false, "Negative number argument values not aloud.")]
    [InlineData("funA(-60, 70)", false, "Negative number argument values not aloud.")]
    [InlineData("funA(80, -90)", false, "Negative number argument values not aloud.")]
    [InlineData("funA(110,)", true, "")]
    [InlineData("funA(-20)", false, "Negative number argument values not aloud.")]
    public void Analyze_WhenInvoked_ReturnsCorrectResult(
        string function,
        bool expectedValid,
        string expectedMsg)
    {
        // Arrange
        var service = new NegativeNumberAnalyzer();

        // Act
        var actual = service.Analyze(function);

        // Assert
        actual.valid.Should().Be(expectedValid);
        actual.msg.Should().Be(expectedMsg);
    }
    #endregion
}
