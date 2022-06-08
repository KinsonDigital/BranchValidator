// <copyright file="QuoteAnalyzerServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using FluentAssertions;

namespace BranchValidatorTests.Services;

public class QuoteAnalyzerServiceTests
{
    #region Method Tests
    [Theory]
    [InlineData("contains-\"both\"-'quote'-types", false, "Cannot use both single and double quotes in an expression.")]
    [InlineData("this-is-'sample-expression", false, "Expression missing a single quote.")]
    [InlineData("this-is-\"sample-expression", false, "Expression missing a double quote.")]
    [InlineData(null, true, "")]
    [InlineData("", true, "")]
    [InlineData("contains-no-quotes", true, "")]
    [InlineData("even-'number'-of-single-quotes", true, "")]
    [InlineData("even-\"number\"-of-double-quotes", true, "")]
    public void Analyze_WhenInvoked_ReturnsCorrectResult(
        string expression,
        bool expectedValidResult,
        string expectedMsgResult)
    {
        // Arrange
        var service = new QuoteAnalyzerService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        var validBecauseClause = expectedValidResult
            ? "the expression is valid."
            : "the expression must have an even number of single or double quotes.";
        actual.valid.Should().Be(expectedValidResult, validBecauseClause);
        actual.msg.Should().Be(expectedMsgResult);
    }
    #endregion
}
