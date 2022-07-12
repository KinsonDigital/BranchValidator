// <copyright file="RemoveExpressionAttributeMutationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using ScriptGenerator.Services;
using ScriptGeneratorTests.Helpers;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="RemoveExpressionAttributeMutation"/> class.
/// </summary>
public class RemoveExpressionAttributeMutationTests
{
    #region Method Tests
    [Fact]
    public void Mutate_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        const string expected =
@"public class FunctionDefinitions
{
    public bool FuncA(string value)
    {
        return true;
    }

    public bool FuncB(string value)
    {
        return true;
    }
}";
        var testData = TestDataLoader.LoadFileData("FunctionDefinitions.txt");
        var mutation = new RemoveExpressionAttributeMutation();

        // Act
        var actual = mutation.Mutate(testData);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
