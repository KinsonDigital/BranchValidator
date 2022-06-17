// <copyright file="ScriptTemplateServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Text;
using FluentAssertions;
using ScriptGenerator.Services;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="ScriptTemplateService"/> class.
/// </summary>
public class ScriptTemplateServiceTests
{
    #region Method Tests
    [Fact]
    public void CreateTemplate_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
#pragma warning disable SA1137
        var expectedStrBuilder = new StringBuilder();
        expectedStrBuilder.AppendLine("// <copyright file=\"ExpressionFunctions.cs\" company=\"KinsonDigital\">");
        expectedStrBuilder.AppendLine("// Copyright (c) KinsonDigital. All rights reserved.");
        expectedStrBuilder.AppendLine("// </copyright>");
        expectedStrBuilder.AppendLine();
        expectedStrBuilder.AppendLine("#pragma warning disable SA1137");
        expectedStrBuilder.AppendLine("#pragma warning disable SA1633");
        expectedStrBuilder.AppendLine("#pragma warning disable SA1600");
        expectedStrBuilder.AppendLine("#pragma warning disable SA1027");
        expectedStrBuilder.AppendLine("#pragma warning disable SA1515");
        expectedStrBuilder.AppendLine("#pragma warning disable CA1050");
        expectedStrBuilder.AppendLine("#pragma warning disable SA1005");
        expectedStrBuilder.AppendLine("// ReSharper disable ArrangeMethodOrOperatorBody");
        expectedStrBuilder.AppendLine("// ReSharper disable CheckNamespace");
        expectedStrBuilder.AppendLine("// ReSharper disable UnusedType.Global");
        expectedStrBuilder.AppendLine("// ReSharper disable UnusedMember.Global");
        expectedStrBuilder.AppendLine("// ReSharper disable UnusedMember.Global");
        expectedStrBuilder.AppendLine();
        expectedStrBuilder.AppendLine("/*THIS SCRIPT IS AUTO-GENERATED AND SHOULD NOT BE CHANGED MANUALLY*/");
        expectedStrBuilder.AppendLine();
        expectedStrBuilder.AppendLine("using System.Text.RegularExpressions;");
        expectedStrBuilder.AppendLine();
        expectedStrBuilder.AppendLine("public static class ExpressionFunctions");
        expectedStrBuilder.AppendLine("{");
        expectedStrBuilder.AppendLine("\tprivate const string BranchName = \"//<branch-name/>\";");
            expectedStrBuilder.AppendLine("\tprivate static readonly char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', };");
            expectedStrBuilder.AppendLine();
            expectedStrBuilder.AppendLine("//<function-code/>");
        expectedStrBuilder.AppendLine("}");
        expectedStrBuilder.AppendLine();
        expectedStrBuilder.AppendLine("//<expression/>");
#pragma warning restore SA1137

        var expected = expectedStrBuilder.ToString();
        var service = new ScriptTemplateService();

        // Act
        var actual = service.CreateTemplate();

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
