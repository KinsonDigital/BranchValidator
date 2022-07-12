// <copyright file="FunctionDefinitionsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using FluentAssertions;

namespace BranchValidatorTests;

/// <summary>
/// Tests the <see cref="FunctionDefinitions"/> class.
/// </summary>
public class FunctionDefinitionsTests
{
    #region Method Tests
    [Theory]
    [InlineData("my-branch", "other-branch", false)]
    [InlineData(null, "my-branch", false)]
    [InlineData("", "my-branch", false)]
    [InlineData("my-branch", null, false)]
    [InlineData("my-branch", "", false)]
    [InlineData("my-branch", "my-branch", true)]
    [InlineData(null, "", true)]
    [InlineData("", null, true)]
    [InlineData("release/v#.#.#-preview.#", "release/v1.^.3-preview.4", false)]
    [InlineData("release/v1.2.3-*.4", "release/v1.2.3-.4", false)]
    [InlineData("release/v#.2.#-*.4", "release/vT.2.3-.4", false)]
    [InlineData("release/v#.#.#-preview.#", "release/v1.2.3-preview.4", true)]
    [InlineData("release/v1.2.3-*.4", "release/v1.2.3-preview.4", true)]
    [InlineData("release/v#.2.#-*.4", "release/v1.2.3-preview.4", true)]
    public void EqualTo_WhenInvoked_ReturnsCorrectResult(
        string value,
        string branchName,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.EqualTo(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"equalTo(string) -> {expected.ToString().ToLower()}");
    }

    [Fact]
    public void AllUpperCase_WhenAllUpperCase_ReturnsTrue()
    {
        // Arrange
        const string branchName = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.AllUpperCase();
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().BeTrue();
        actualFunctionResults.Should().Contain("allUpperCase() -> true");
    }

    [Fact]
    public void AllUpperCase_WhenSingleCharacterIsLowerCase_ReturnsFalse()
    {
        // Arrange
        const string letters = "abcdefghijklmnopqrstuvwxyz";
        Assert.All(letters, c =>
        {
            // Arrange
            var branchName = c.ToString();
            var definitions = new FunctionDefinitions(branchName);

            // Act
            var actual = definitions.AllUpperCase();
            var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

            // Assert
            actual.Should().BeFalse();
            actualFunctionResults.Should().Contain("allUpperCase() -> false");
        });
    }

    [Fact]
    public void AllLowerCase_WhenAllLowerCase_ReturnsTrue()
    {
        // Arrange
        const string branchName = "abcdefghijklmnopqrstuvwxyz";
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.AllLowerCase();
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().BeTrue();
        actualFunctionResults.Should().Contain("allLowerCase() -> true");
    }

    [Fact]
    public void AllLowerCase_WhenSingleCharacterIsUpperCase_ReturnsFalse()
    {
        // Arrange
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Assert.All(letters, c =>
        {
            // Arrange
            var branchName = c.ToString();
            var definitions = new FunctionDefinitions(branchName);

            // Act
            var actual = definitions.AllLowerCase();
            var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

            // Assert
            actual.Should().BeFalse();
            actualFunctionResults.Should().Contain("allLowerCase() -> false");
        });
    }
    #endregion
}
