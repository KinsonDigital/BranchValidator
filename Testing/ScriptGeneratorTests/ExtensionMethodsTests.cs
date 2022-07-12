// <copyright file="ExtensionMethodsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using ScriptGenerator;

namespace ScriptGeneratorTests;

/// <summary>
/// Tests the <see cref="ExtensionMethods"/> class.
/// </summary>
public class ExtensionMethodsTests
{
    #region Method Tests
    [Theory]
    [InlineData(null, new[] { 'k', 'd' }, false)]
    [InlineData("", new[] { 'k', 'd' }, false)]
    [InlineData("test-value", new[] { 'z', }, false)]
    [InlineData("test\tvalue", new[] { '\t', }, false)]
    [InlineData("\t", new[] { '\t', }, true)]
    [InlineData("\n", new[] { '\n', }, true)]
    [InlineData("\r", new[] { '\r', }, true)]
    [InlineData("\r\n", new[] { '\r', '\n', }, true)]
    [InlineData("\r \n", new[] { ' ', '\r', '\n', }, true)]
    [InlineData("test-value", new[] { 't', 'e', 's', '-', 'v', 'a', 'l', 'u', 'e', }, true)]
    public void OnlyContains_WhenInvoked_ReturnsCorrectResult(string value, char[] characters, bool expected)
    {
        // Act
        var actual = value.OnlyContains(characters);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("---b---a---", 'b', 'a', true)]
    [InlineData("---b------", 'b', 'a', true)]
    [InlineData(null, 'b', 'a', false)]
    [InlineData("", 'b', 'a', false)]
    [InlineData("------a---", 'b', 'a', false)]
    [InlineData("---a---b---", 'b', 'a', false)]
    public void IsBefore_WithCharacterBeforeCharacter_ReturnsCorrectResult(
        string value,
        char beforeChar,
        char afterChar,
        bool expected)
    {
        // Act
        var actual = value.IsBefore(beforeChar, afterChar);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("---str---a---", "str", 'a', true)]
    [InlineData("---str------", "str", 'a', true)]
    [InlineData(null, "", 'a', false)]
    [InlineData("", "", 'a', false)]
    [InlineData("---a---str---", "str", 'a', false)]
    [InlineData("------a---", "str", 'a', false)]
    public void IsBefore_WithStringBeforeCharacter_ReturnsCorrectResult(
        string value,
        string beforeStr,
        char afterChar,
        bool expected)
    {
        // Act
        var actual = value.IsBefore(beforeStr, afterChar);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("---a---str---", "str", 'a', true)]
    [InlineData("------str---", "str", 'a', true)]
    [InlineData(null, "", 'a', false)]
    [InlineData("", "", 'a', false)]
    [InlineData("---str---a---", "str", 'a', false)]
    [InlineData("---a------", "str", 'a', false)]
    public void IsAfter_WithStringAfterCharacter_ReturnsCorrectResult(
        string value,
        string afterStr,
        char beforeChar,
        bool expected)
    {
        // Act
        var actual = value.IsAfter(afterStr, beforeChar);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("---b---a---", 'a', 'b', true)]
    [InlineData("------a---", 'a', 'b', true)]
    [InlineData("---b------", 'a', 'b', false)]
    [InlineData(null, 'a', 'b', false)]
    [InlineData("", 'a', 'b', false)]
    [InlineData("---a---b---", 'a', 'b', false)]
    public void IsAfter_WithCharacterAfterCharacter_ReturnsCorrectResult(
        string value,
        char afterChar,
        char beforeChar,
        bool expected)
    {
        // Act
        var actual = value.IsAfter(afterChar, beforeChar);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
