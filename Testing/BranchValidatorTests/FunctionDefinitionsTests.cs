// <copyright file="FunctionDefinitionsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using FluentAssertions;

namespace BranchValidatorTests;

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
        var definitions = new FunctionDefinitions();

        // Act
        var actual = definitions.EqualTo(value, branchName);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(8, "", false)]
    [InlineData(4, "feature/123-my-branch", false)]
    [InlineData(400, "feature/123-my-branch", false)]
    [InlineData(8, "feature/123-my-branch", true)]
    public void IsCharNum_WhenInvoked_ReturnsCorrectResult(
        uint charPos,
        string branchName,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions();

        // Act
        var actual = definitions.IsCharNum(charPos, branchName);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, 0, 10, false)]
    [InlineData("", 0, 10, false)]
    [InlineData("feature/123-test-branch", 8, 12, false)]
    [InlineData("feature/test-branch", 12, 3000, false)]
    [InlineData("feature/123-test-branch", 8, 11, false)]
    [InlineData("feature/123-test-branch", 8, 10, true)]
    [InlineData("feature/0123456789", 12, 3000, true)]
    public void IsSectionNum_WhenInvokedWithStartAndEndPosParams_ReturnsCorrectResult(
        string branchName,
        uint startPos,
        uint endPos,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions();

        // Act
        var actual = definitions.IsSectionNum(startPos, endPos, branchName);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, 0, "-", false)]
    [InlineData("", 0, "-", false)]
    [InlineData("feature/123-test-branch", 8, null, false)]
    [InlineData("feature/123-test-branch", 8, "", false)]
    [InlineData("feature/123testbranch", 8, "-", false)]
    [InlineData("feature/123-testbranch", 11, "-", false)]
    [InlineData("feature/123test-branch", 8, "-", false)]
    [InlineData("feature/123-test-branch", 3000, "-", false)]
    [InlineData("feature/123-test-branch", 8, "-", true)]
    [InlineData("feature/123-test-branch", 8, "-other-characters", true)]
    [InlineData("0123456789-", 0, "-", true)]
    public void IsSectionNum_WhenInvokedWithStartPosAndUpToCharParams_ReturnsCorrectResult(
        string branchName,
        uint startPos,
        string upToChar,
        bool expected)
    {
        // Arrange
        var definitions = new FunctionDefinitions();

        // Act
        var actual = definitions.IsSectionNum(startPos, upToChar, branchName);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
