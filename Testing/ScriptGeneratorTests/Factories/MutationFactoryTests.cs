// <copyright file="MutationFactoryTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using ScriptGenerator.Factories;
using ScriptGenerator.Services;

namespace ScriptGeneratorTests.Factories;

/// <summary>
/// Tests the <see cref="MutationFactory"/> class.
/// </summary>
public class MutationFactoryTests
{
    #region Method Tests
    [Fact]
    public void CreateMutations_WhenInvoked_ReturnsCorrectResult()
    {
        // Act
        var expected = new[]
        {
            typeof(StaticMethodMutation),
            typeof(ThisRefToStaticRefMutation),
            typeof(RemoveInheritCodeDocsMutation),
            typeof(RemoveExpressionAttributeMutation),
        };
        var mutations = MutationFactory.CreateMutations();
        var actual = mutations.Select(m => m.GetType()).ToArray();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    #endregion
}
