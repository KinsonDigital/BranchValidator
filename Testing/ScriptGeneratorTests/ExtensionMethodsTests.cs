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
    #endregion
}
