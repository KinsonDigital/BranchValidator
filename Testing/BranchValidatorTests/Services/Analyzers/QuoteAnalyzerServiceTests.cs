// <copyright file="QuoteAnalyzerServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Analyzers;
using FluentAssertions;

namespace BranchValidatorTests.Services.Analyzers;

public class QuoteAnalyzerServiceTests
{
    #region Method Tests
    [Theory]
    [InlineData("funA(\"'both'\")", false, "Cannot use both single and double quotes in an expression.")]
    [InlineData("funA('value)", false, "Expression missing a single quote.")]
    [InlineData("funA(\"value)", false, "Expression missing a double quote.")]
    [InlineData("fun'A(value')", false, "Single and double quotes must only exist inside of a function argument list.")]
    [InlineData("fun\"A(value\")", false, "Single and double quotes must only exist inside of a function argument list.")]
    [InlineData(null, true, "")]
    [InlineData("", true, "")]
    [InlineData("funA()", true, "")]
    [InlineData("funA(123)", true, "")]
    [InlineData("funA('value')", true, "")]
    [InlineData("funB(\"value\")", true, "")]
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
