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
    #endregion
}
