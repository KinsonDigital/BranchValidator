// <copyright file="StaticMethodMutationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using ScriptGenerator.Services;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="StaticMethodMutation"/> class.
/// </summary>
public class StaticMethodMutationTests
{
    #region Method Tests
    [Fact]
    public void Mutate_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        const string expected = "public static bool GetValue()";
        const string value = "public bool GetValue()";

        var mutation = new StaticMethodMutation();

        // Act
        var actual = mutation.Mutate(value);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
