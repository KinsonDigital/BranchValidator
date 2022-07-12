// <copyright file="RemoveInheritCodeDocsMutationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using ScriptGenerator.Services;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="RemoveInheritCodeDocsMutation"/> class.
/// </summary>
public class RemoveInheritCodeDocsMutationTests
{
    #region Method Tests
    [Fact]
    public void Mutate_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var expected = "public string MyMethod()";
        expected += Environment.NewLine;
        expected += "{";
        expected += Environment.NewLine;
        expected += "\tvar number = 1234;";
        expected += Environment.NewLine;
        expected += "}";

        const string value = "/// <inheritdoc/>\r\npublic string MyMethod()\n\r{\r\tvar number = 1234;\r}";

        var mutation = new RemoveInheritCodeDocsMutation();

        // Act
        var actual = mutation.Mutate(value);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
