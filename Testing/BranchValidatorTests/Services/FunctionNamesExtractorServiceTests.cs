// <copyright file="FunctionNamesExtractorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using FluentAssertions;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="FunctionNamesExtractorService"/> class.
/// </summary>
public class FunctionNamesExtractorServiceTests
{
    #region Method Tests
    [Theory]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    [InlineData("funA()", new[] { "funA" })]
    [InlineData("funA() && funB()", new[] { "funA", "funB" })]
    [InlineData("funA() || funB() && funC()", new[] { "funA", "funB", "funC" })]
    [InlineData("funA() && funB() || funC() || funD() && funE()", new[] { "funA", "funB", "funC", "funD", "funE" })]
    public void Analyze_WhenInvoked_ReturnsCorrectResult(
        string expression,
        string[] expected)
    {
        // Arrange
        var service = new FunctionNamesExtractorService();

        // Act
        var actual = service.ExtractNames(expression);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    #endregion
}
