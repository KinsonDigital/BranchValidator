// <copyright file="ThisRefToStaticRefMutationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using ScriptGenerator.Services;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="ThisRefToStaticRefMutation"/> class.
/// </summary>
public class ThisRefToStaticRefMutationTests
{
    #region Method Tests
    [Fact]
    public void Mutate_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        const string expected = "public string TheBranch => BranchName;";
        const string value = "public string TheBranch => this.branchName;";

        var mutation = new ThisRefToStaticRefMutation();

        // Act
        var actual = mutation.Mutate(value);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
