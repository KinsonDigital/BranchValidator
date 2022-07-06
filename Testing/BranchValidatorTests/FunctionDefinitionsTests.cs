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
    [InlineData("my-branch", "my-branch", true)]
    [InlineData(null, "", true)]
    [InlineData("", null, true)]
    [InlineData("my-branch", "other-branch", false)]
    [InlineData(null, "my-branch", false)]
    [InlineData("", "my-branch", false)]
    [InlineData("my-branch", null, false)]
    [InlineData("my-branch", "", false)]
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
    [InlineData(null, 0, 10, false)]
    [InlineData("", 0, 10, false)]
    [InlineData("feature/123-test-branch", 8, 12, false)]
    [InlineData("feature/test-branch", 12, 3000, false)]
    [InlineData("feature/123-test-branch", 8, 11, false)]
    [InlineData("feature/123-test-branch", 8, 10, true)]
    [InlineData("feature/0123456789", 12, 3000, true)]
    [InlineData("feature/123-test-branch", 8, 10, true)]
    [InlineData("release/v1.2.3-preview.4", 9, 9, true)]
    public void SectionIsNum_WhenInvokedWithStartAndEndPosParams_ReturnsCorrectResult(
        string branchName,
        uint startPos,
        uint endPos,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.SectionIsNum(startPos, endPos);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"sectionIsNum(number, number) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, 0, "-", false)]
    [InlineData(null, 1000, "-", false)]
    [InlineData("", 0, "-", false)]
    [InlineData("", 2000, "-", false)]
    [InlineData("feature/123-test-branch", 8, null, false)]
    [InlineData("feature/123-test-branch", 8, "", false)]
    [InlineData("feature/123testbranch", 8, "-", false)]
    [InlineData("feature/123-testbranch", 11, "-", false)]
    [InlineData("feature/123test-branch", 8, "-", false)]
    [InlineData("feature/123-test-branch", 3000, "-", false)]
    [InlineData("feature/123-test-branch", 8, "-", true)]
    [InlineData("feature/123-test-branch", 8, "-other-characters", true)]
    [InlineData("0123456789-", 0, "-", true)]
    public void SectionIsNum_WhenInvokedWithStartPosAndUpToCharParams_ReturnsCorrectResult(
        string branchName,
        uint startPos,
        string upToChar,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.SectionIsNum(startPos, upToChar);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"sectionIsNum(number, string) -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, "123-test", false)]
    [InlineData("", "123-test", false)]
    [InlineData("feature/123-test-branch", "is-not-contained", false)]
    [InlineData("feature/123-test-branch", "123-test", true)]
    [InlineData("release/v1.2.3-preview.4", "#.#.#", true)]
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
    [InlineData(null, "123-test", false)]
    [InlineData("", "123-test", false)]
    [InlineData("feature/123-test-branch", "123-test", false)]
    [InlineData("feature/123-test-branch", "is-not-contained", true)]
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
    [InlineData("feature/123-test-123-branch-123", "feature/123", true)]
    [InlineData("feature/123-test-branch", "123", false)]
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
    [InlineData(null, "test", false)]
    [InlineData("", "test", false)]
    [InlineData("feature/123-test-branch", "123", true)]
    [InlineData("feature/123-test-123-branch-123", "feature/123", false)]
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
    [InlineData("feature/123-test-branch", "branch", true)]
    [InlineData("feature/123-test-branch", "123", false)]
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
    [InlineData(null, "test", false)]
    [InlineData("", "test", false)]
    [InlineData("feature/123-test-branch", "123", true)]
    [InlineData("feature/123-test-branch", "branch", false)]
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
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("feature/123-test-branch", false)]
    [InlineData("123-test-branch", true)]
    public void StartsWithNum_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.StartsWithNum();
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"startsWithNum() -> {expected.ToString().ToLower()}");
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("feature/123-test-branch", false)]
    [InlineData("feature/123-test-branch-456", true)]
    public void EndsWithNum_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.EndsWithNum();
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"endsWithNum() -> {expected.ToString().ToLower()}");
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

    [Theory]
    [InlineData(null, "123", "branch", false)]
    [InlineData("", "123", "branch", false)]
    [InlineData("feature/123-test-branch", "123", "branch", false)]
    [InlineData("feature/123-test-branch", "branch", "123", true)]
    public void IsAfter_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        string after,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions(branchName);

        // Act
        var actual = definitions.IsAfter(value, after);
        var actualFunctionResults = FunctionDefinitions.GetFunctionResults();

        // Assert
        actual.Should().Be(expected);
        actualFunctionResults.Should().Contain($"isAfter(string, string) -> {expected.ToString().ToLower()}");
    }
    #endregion
}
