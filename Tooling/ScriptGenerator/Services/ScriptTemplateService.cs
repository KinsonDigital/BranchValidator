// <copyright file="ScriptTemplateService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Text;

namespace ScriptGenerator.Services;

/// <inheritdoc/>
public class ScriptTemplateService : IScriptTemplateService
{
    private const string FunctionCode = "//<function-code/>";
    private const string ExpressionCode = "//<expression/>";

    /// <inheritdoc/>
    public string CreateTemplate()
    {
        var script = new StringBuilder();

#pragma warning disable SA1137
        script.AppendLine("// <copyright file=\"ExpressionFunctions.cs\" company=\"KinsonDigital\">");
        script.AppendLine("// Copyright (c) KinsonDigital. All rights reserved.");
        script.AppendLine("// </copyright>");
        script.AppendLine();
        script.AppendLine("#pragma warning disable SA1137");
        script.AppendLine("#pragma warning disable SA1633");
        script.AppendLine("#pragma warning disable SA1600");
        script.AppendLine("#pragma warning disable SA1027");
        script.AppendLine("#pragma warning disable SA1515");
        script.AppendLine("#pragma warning disable CA1050");
        script.AppendLine("#pragma warning disable SA1005");
        script.AppendLine("// ReSharper disable ArrangeMethodOrOperatorBody");
        script.AppendLine("// ReSharper disable CheckNamespace");
        script.AppendLine("// ReSharper disable UnusedType.Global");
        script.AppendLine("// ReSharper disable UnusedMember.Global");
        script.AppendLine("// ReSharper disable UnusedMember.Global");
        script.AppendLine();
        script.AppendLine("/*THIS SCRIPT IS AUTO-GENERATED AND SHOULD NOT BE CHANGED MANUALLY*/");
        script.AppendLine();
        script.AppendLine("using System.Text.RegularExpressions;");
        script.AppendLine();
        script.AppendLine("public static class ExpressionFunctions");
        script.AppendLine("{");
            script.AppendLine("\tprivate const string BranchName = \"//<branch-name/>\";");
            script.AppendLine("\tprivate static readonly char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', };");
            script.AppendLine();
            script.AppendLine(FunctionCode);
            script.AppendLine("}");
            script.AppendLine();
        script.AppendLine(ExpressionCode);
#pragma warning restore SA1137

        return script.ToString();
    }
}
