// <copyright file="FunctionExtractorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using FluentAssertions;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="FunctionExtractorService"/> class.
/// </summary>
public class FunctionExtractorServiceTests
{
    #region Method Tests
    [Theory]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    [InlineData("funA()", new[] { "funA" })]
    [InlineData("funA() && funB()", new[] { "funA", "funB" })]
    [InlineData("funA() || funB() && funC()", new[] { "funA", "funB", "funC" })]
    [InlineData("funA() && funB() || funC() || funD() && funE()", new[] { "funA", "funB", "funC", "funD", "funE" })]
    public void ExtractNames_WhenInvoked_ReturnsCorrectResult(
        string expression,
        string[] expected)
    {
        // Arrange
        var service = new FunctionExtractorService();

        // Act
        var actual = service.ExtractNames(expression);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    [InlineData("funA()", new[] { "funA()" })]
    [InlineData("funA('str-value')", new[] { "funA('str-value')" })]
    [InlineData("funA(123)", new[] { "funA(123)" })]
    [InlineData("funA() && funB()", new[] { "funA()", "funB()" })]
    [InlineData("funA() || funB()", new[] { "funA()", "funB()" })]
    [InlineData("funA() && funB() || funC()", new[] { "funA()", "funB()", "funC()" })]
    [InlineData("funA() || funB() && funC()", new[] { "funA()", "funB()", "funC()" })]
    [InlineData("funA() && funB() || funC() || funD() && funE()", new[] { "funA()", "funB()", "funC()", "funD()", "funE()" })]
    [InlineData("funA('str-value') && funB()", new[] { "funA('str-value')", "funB()" })]
    [InlineData("funA() && funB(123)", new[] { "funA()", "funB(123)" })]
    [InlineData("funA('str-value') && funB(123)", new[] { "funA('str-value')", "funB(123)" })]
    [InlineData("funA(123) && funB('str-value')", new[] { "funA(123)", "funB('str-value')" })]
    public void ExtractFunctions_WhenInvoked_ReturnsCorrectResult(
        string expression,
        string[] expected)
    {
        // Arrange
        var service = new FunctionExtractorService();

        // Act
        var actual = service.ExtractFunctions(expression);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    [InlineData("funA()&&", new string[0])]
    [InlineData("funA()||", new string[0])]
    [InlineData("funA", new string[0])]
    [InlineData("funA(", new string[0])]
    [InlineData("funA)", new string[0])]
    [InlineData("funA(()", new string[0])]
    [InlineData("funA())", new string[0])]
    [InlineData("funA()", new string[0])]
    [InlineData("funA('string-value')", new[] { "'string-value'" })]
    [InlineData("funA(\"string-value\")", new[] { "\"string-value\"" })]
    [InlineData("funA('string-value', 123)", new[] { "'string-value'", "123" })]
    [InlineData("funA('string-value' ,123)", new[] { "'string-value'", "123" })]
    [InlineData("funA('string-value',123)", new[] { "'string-value'", "123" })]
    [InlineData("funA( 'string-value',123)", new[] { "'string-value'", "123" })]
    [InlineData("funA('string-value',123 )", new[] { "'string-value'", "123" })]
    public void ExtractArgValues_WhenInvoked_ReturnsCorrectResult(
        string functionSignature,
        string[] expected)
    {
        // Arrange
        var service = new FunctionExtractorService();

        // Act
        var actual = service.ExtractArgValues(functionSignature);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("", new Type[0])]
    [InlineData("funA('valueA')", new[] { typeof(string) })]
    [InlineData("funA(\"valueB\")", new[] { typeof(string) })]
    [InlineData("funA(123)", new[] { typeof(uint) })]
    [InlineData("funA('valueC', \"valueD\", 456)", new[] { typeof(string), typeof(string), typeof(uint) })]
    public void ExtractArgDataTypes_WhenInvoked_ReturnsCorrectResult(
        string functionSignature,
        Type[] expected)
    {
        // Arrange
        var service = new FunctionExtractorService();

        // Act
        var actual = service.ExtractArgDataTypes(functionSignature);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    #endregion
}
