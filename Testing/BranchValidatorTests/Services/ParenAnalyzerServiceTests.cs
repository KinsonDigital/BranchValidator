// <copyright file="ParenAnalyzerServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using FluentAssertions;

namespace BranchValidatorTests.Services;

public class ParenAnalyzerServiceTests
{
    #region Method Tests
    [Theory]
    [InlineData("(start-with-left-paren", false, "The expression cannot start with a '(' or ')' parenthesis.")]
    [InlineData(")start-with-left-paren", false, "The expression cannot start with a '(' or ')' parenthesis.")]
    [InlineData("ends-with-left-paren(", false, "The expression cannot end with a '('.")]
    [InlineData("no-functions-exist", false, "The expression must have at least one function.")]
    [InlineData("myFunc)", false, "The expression is missing a '('.")]
    [InlineData("func1( && func2()", false, "The expression is missing a ')'.")]
    [InlineData("func3)( && func4()", false, "A function parameter list cannot start with a ')'.")]
    [InlineData(null, false, "The expression must have at least one function.")]
    [InlineData("", false, "The expression must have at least one function.")]
    [InlineData("even-(number)-of-parens", true, "")]
    public void Analyze_WhenInvoked_ReturnsCorrectResult(
        string expression,
        bool expectedValidResult,
        string expectedMsgResult)
    {
        // Arrange
        var service = new ParenAnalyzerService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        actual.valid.Should().Be(expectedValidResult);
        actual.msg.Should().Be(expectedMsgResult);
    }
    #endregion
}
