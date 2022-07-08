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

    [Theory]
    [InlineData(8, null, false)]
    [InlineData(8, "", false)]
    [InlineData(4, "feature/123-my-branch", false)]
    [InlineData(400, "feature/123-my-branch", false)]
    [InlineData(8, "feature/123-my-branch", true)]
    public void CharIsNum_WhenInvoked_ReturnsCorrectResult(
        uint charPos,
        string branchName,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.CharIsNum(charPos);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"charIsNum(number) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, "123-test", false)]
    [InlineData("", "123-test", false)]
    [InlineData("feature/123-test-branch", "is-not-contained", false)]
    [InlineData("feature/123-test-branch", "123-test", true)]
    [InlineData("release/v1.2.3-preview.4", "#.#.#", true)]
    [InlineData("start2end", "start##end", true)]
    [InlineData("start2end", "start**end", true)]
    [InlineData("release/v1.2.3-ANYTHING.4", "release/v1.2.3-*.4", true)]
    [InlineData("release/v1.2.3-preview.4", "-preview.#", true)]
    [InlineData("release/v1.2.3-preview.4", "release/v#.#.#-preview.#", true)]
    [InlineData("release/v1.20.300-preview.4000", "release/v#.#.#-preview.#", true)]
    [InlineData("ANYTHING1/v1.20.300-ANYTHING2.4000", "*/v#.#.#-*.#", true)]
    public void Contains_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.Contains(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"contains(string) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData("feature/123-test-branch", "123-test", false)]
    [InlineData("feature/123-456-branch", "123-#-branch", false)]
    [InlineData(null, "123-test", true)]
    [InlineData("", "123-test", true)]
    [InlineData("feature/123-test-branch", "is-not-contained", true)]
    [InlineData("feature/123-test-branch", "123-#-branch", true)]
    [InlineData("feature/123-test-branch", "#-*-#", true)]
    public void NotContains_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.NotContains(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"notContains(string) -> {expected.ToString().ToLower()}");
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
