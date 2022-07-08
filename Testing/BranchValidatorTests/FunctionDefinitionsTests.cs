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

    [Theory]
    [InlineData(null, "test", 1, false)]
    [InlineData("", "test", 1, false)]
    [InlineData("feature/123-branch", "test", 1, false)]
    [InlineData("feature/123-branch", "a", 2, true)]
    [InlineData("feature/123-test-branch-test", "test", 4, false)]
    [InlineData("feature/123-test-branch", "test", 1, true)]
    [InlineData("feature/123-te|st-branch", "te|st", 1, true)]
    [InlineData("feature/123-test-branch-test", "test", 2, true)]
    [InlineData("release/v1.2.3-preview.4", ".", 3, true)]
    public void ExistsTotal_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        uint total,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.ExistsTotal(value, total);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"existsTotal(string, number) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, "test", 1, false)]
    [InlineData("", "test", 1, false)]
    [InlineData("feature/123-test-branch", "123", 2, true)]
    [InlineData("feature/123-branch", "test", 200, true)]
    [InlineData("feature/123-test-branch", "123", 1, false)]
    public void ExistsLessThan_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        uint total,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.ExistsLessThan(value, total);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"existsLessThan(string, number) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, "test", 1, false)]
    [InlineData("", "test", 1, false)]
    [InlineData("feature/123-test-123-branch-123", "123", 2, true)]
    [InlineData("feature/123-test-branch", "123", 1, false)]
    public void ExistsGreaterThan_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        uint total,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.ExistsGreaterThan(value, total);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"existsGreaterThan(string, number) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, "test", false)]
    [InlineData("", "test", false)]
    [InlineData("feature/123-test-branch", "123", false)]
    [InlineData("feature/123-test-branch", "feature/#-test-branch", true)]
    [InlineData("feature/123-test-branch", "feature/##-test-branch", true)]
    [InlineData("123-my-test-branch", "#-my-test-branch", true)]
    [InlineData("letters-numbers-numbers-test", "*-*-numbers", true)]
    [InlineData("123-my-test-branch", "#-my", true)]
    [InlineData("123-my-test-branch", "#*-test", true)]
    [InlineData("feature/123-test-123-branch-123", "feature/123", true)]
    public void StartsWith_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.StartsWith(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"startsWith(string) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData("feature/123-test-branch", "feature/#-test-branch", false)]
    [InlineData("feature/123-test-branch", "feature/##-test-branch", false)]
    [InlineData("123-my-test-branch", "#-my-test-branch", false)]
    [InlineData("letters-numbers-numbers-test", "*-*-numbers", false)]
    [InlineData("123-my-test-branch", "#-my", false)]
    [InlineData("123-my-test-branch", "#*-test", false)]
    [InlineData("feature/123-test-123-branch-123", "feature/123", false)]
    [InlineData(null, "test", true)]
    [InlineData("", "test", true)]
    [InlineData("feature/123-test-branch", "123", true)]
    public void NotStartsWith_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.NotStartsWith(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"notStartsWith(string) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, "test", false)]
    [InlineData("", "test", false)]
    [InlineData("feature/123-test-branch", "123", false)]
    [InlineData("release/v.1.2.3-preview.4-hello", "release/v.1.2.3-preview.#", false)]
    [InlineData("feature/123-test-branch", "branch", true)]
    [InlineData("release/v.1.2.3-preview.4", "-*.4", true)]
    [InlineData("release/v.1.2.3-beta.4", "-*.4", true)]
    [InlineData("release/v.1.2.3", "v.#.#.#", true)]
    [InlineData("release/v.1.2.3-preview.4", "#", true)]
    [InlineData("release/v.1.2.3-preview.123456789", "#", true)]
    [InlineData("release/v.1.2.3-preview.4", "3-*.#", true)]
    public void EndsWith_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.EndsWith(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"endsWith(string) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData("feature/123-test-branch", "branch", false)]
    [InlineData("release/v.1.2.3-preview.4", "-*.4", false)]
    [InlineData("release/v.1.2.3-beta.4", "-*.4", false)]
    [InlineData("release/v.1.2.3", "v.#.#.#", false)]
    [InlineData("release/v.1.2.3-preview.4", "#", false)]
    [InlineData("release/v.1.2.3-preview.123456789", "#", false)]
    [InlineData("release/v.1.2.3-preview.4", "3-*.#", false)]
    [InlineData(null, "test", true)]
    [InlineData("", "test", true)]
    [InlineData("feature/123-test-branch", "123", true)]
    [InlineData("release/v.1.2.3-preview.4-hello", "release/v.1.2.3-preview.#", true)]
    public void NotEndsWith_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.NotEndsWith(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"notEndsWith(string) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, 0, false)]
    [InlineData("", 0, false)]
    [InlineData("feature/123-test-branch", 10, false)]
    [InlineData("feature/123-test-branch", 23, false)]
    [InlineData("feature/123-test-branch", 200, true)]
    public void LenLessThan_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        uint value,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.LenLessThan(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"lenLessThan(number) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, 0, false)]
    [InlineData("", 0, false)]
    [InlineData("feature/123-test-branch", 100, false)]
    [InlineData("feature/123-test-branch", 10, true)]
    public void LenGreaterThan_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        uint value,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.LenGreaterThan(value);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"lenGreaterThan(number) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, "123", "branch", false)]
    [InlineData("", "123", "branch", false)]
    [InlineData("feature/123-test-branch", "branch", "123", false)]
    [InlineData("feature/123-test-branch", "123", "branch", true)]
    public void IsBefore_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        string after,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.IsBefore(value, after);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"isBefore(string, string) -> {expected.ToString().ToLower()}");
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
