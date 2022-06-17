// <copyright file="FunctionNamesExtractorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="FunctionNamesExtractorService"/> class.
/// </summary>
public class FunctionNamesExtractorServiceTests
{
    private readonly Mock<IExpressionValidatorService> mockExpressionValidatorService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionNamesExtractorServiceTests"/> class.
    /// </summary>
    public FunctionNamesExtractorServiceTests() => this.mockExpressionValidatorService = new Mock<IExpressionValidatorService>();

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
        this.mockExpressionValidatorService.Setup(m => m.Validate(expression))
            .Returns((true, string.Empty));
        var service = new FunctionNamesExtractorService(this.mockExpressionValidatorService.Object);

        // Act
        var actual = service.ExtractNames(expression);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    #endregion
}
