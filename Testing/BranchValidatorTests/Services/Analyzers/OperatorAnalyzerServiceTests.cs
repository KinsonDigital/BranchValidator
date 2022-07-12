// <copyright file="OperatorAnalyzerServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Analyzers;
using FluentAssertions;

namespace BranchValidatorTests.Services.Analyzers;

/// <summary>
/// Tests the <see cref="OperatorAnalyzerService"/> class.
/// </summary>
public class OperatorAnalyzerServiceTests
{
    #region Method Tests
    [Theory]
    [InlineData("&&funA()()", false, "Cannot start or end an expression with an '&&' operator or '&' character.")]
    [InlineData("&funA()()", false, "Cannot start or end an expression with an '&&' operator or '&' character.")]
    [InlineData("funA()()&&", false, "Cannot start or end an expression with an '&&' operator or '&' character.")]
    [InlineData("funA()()&", false, "Cannot start or end an expression with an '&&' operator or '&' character.")]
    [InlineData(" &&funA()()", false, "Cannot start or end an expression with an '&&' operator or '&' character.")]
    [InlineData("funA()()&& ", false, "Cannot start or end an expression with an '&&' operator or '&' character.")]
    [InlineData("||funA()()", false, "Cannot start or end an expression with an '||' operator or '|' character.")]
    [InlineData("funA()()||", false, "Cannot start or end an expression with an '||' operator or '|' character.")]
    [InlineData("|funA()()", false, "Cannot start or end an expression with an '||' operator or '|' character.")]
    [InlineData("funA()()|", false, "Cannot start or end an expression with an '||' operator or '|' character.")]
    [InlineData(" ||funA()()", false, "Cannot start or end an expression with an '||' operator or '|' character.")]
    [InlineData("funA()()|| ", false, "Cannot start or end an expression with an '||' operator or '|' character.")]
    [InlineData("funA() || funB() | funC()", false, "Expression is missing an '|' operator.")]
    [InlineData("funA() && funB() & funC()", false, "Expression is missing an '&' operator.")]
    [InlineData("funA() | funB() | funC() | funD() | funE()", false, "OR operators must be 2 consecutive '|' symbols.")]
    [InlineData("funA() & funB() & funC() & funD() & funE()", false, "AND operators must be 2 consecutive '&' symbols.")]
    [InlineData("funA() && funB() funC()", false, "Expression functions must be separated by '&&' or '||' operators.")]
    [InlineData("funA() funB() && funC()", false, "Expression functions must be separated by '&&' or '||' operators.")]
    [InlineData("funA()funB()", false, "Expression functions must be separated by '&&' or '||' operators.")]
    [InlineData("funA() && funB() funC(", false, "Expression functions must be separated by '&&' or '||' operators.")]
    [InlineData("funA() || funB() funC()", false, "Expression functions must be separated by '&&' or '||' operators.")]
    [InlineData("funA() funB() || funC()", false, "Expression functions must be separated by '&&' or '||' operators.")]
    [InlineData("funA() || funB() funC(", false, "Expression functions must be separated by '&&' or '||' operators.")]
    [InlineData(null, true, "expression valid")]
    [InlineData("", true, "expression valid")]
    [InlineData("stuff", true, "expression valid")]
    [InlineData("funA() && funB() && funC()", true, "expression valid")]
    [InlineData("funA() || funB() || funC()", true, "expression valid")]
    public void Analyze_WhenInvoked_ReturnsCorrectResult(
        string expression,
        bool expectedValidResult,
        string expectedMsgResult)
    {
        // Arrange
        var service = new OperatorAnalyzerService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        var validBecauseClause = expectedValidResult
            ? "the expression is valid."
            : "the expression starts or ends with and AND or OR operator character.";
        actual.valid.Should().Be(expectedValidResult, validBecauseClause);
        actual.msg.Should().Be(expectedMsgResult);
    }
    #endregion
}
