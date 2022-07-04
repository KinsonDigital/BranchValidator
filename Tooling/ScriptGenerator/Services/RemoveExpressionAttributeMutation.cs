// <copyright file="RemoveExpressionAttributeMutation.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidatorShared;
using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Services;

/// <summary>
/// Removes all lines of text from a <c>string</c> that contain the <see cref="ExpressionFunctionAttribute"/>.
/// </summary>
public class RemoveExpressionAttributeMutation : IStringMutation
{
    /// <inheritdoc/>
    public string Mutate(string value)
    {
        value = value.Replace("\r\n", "\n");
        value = value.Replace("\n\r", "\n");
        value = value.Replace("\r", "\n");

        var lines = value.Split('\n');

        var resultLines = new List<string>();

        for (var i = 0; i < lines.Length; i++)
        {
            if (i == lines.Length - 1 && string.IsNullOrEmpty(lines[i]))
            {
                continue;
            }

            var trimmedLine = lines[i].Trim();
            var attrName = nameof(ExpressionFunctionAttribute).Replace(nameof(Attribute), string.Empty);
            var containsAttrName = trimmedLine.Contains(attrName);
            var startsWithBracket = trimmedLine.StartsWith('[');
            var endsWithBracket = trimmedLine.EndsWith(']');
            var atLeastOneLeftParen = trimmedLine.Count(c => c == '(') >= 1;
            var atLeastOneRightParen = trimmedLine.Count(c => c == ')') >= 1;
            var leftSqrBracketBeforeLeftParen = trimmedLine.IsBefore('[', '(');
            var rightParenAfterLeftSqrBracket = trimmedLine.IsAfter(')', '[');
            var leftParenBeforeRightSqrBracket = trimmedLine.IsBefore('(', ']');
            var rightParenBeforeRightSqrBracket = trimmedLine.IsBefore(')', ']');
            var leftParenBeforeRightParen = trimmedLine.IsBefore('(', ')');
            var attrNameAfterLeftSqrBracket = trimmedLine.IsAfter(attrName, '[');
            var attrNameBeforeLeftParenBracket = trimmedLine.IsBefore(attrName, '(');

            if (containsAttrName &&
                startsWithBracket &&
                endsWithBracket &&
                atLeastOneLeftParen &&
                atLeastOneRightParen &&
                leftSqrBracketBeforeLeftParen &&
                rightParenAfterLeftSqrBracket &&
                leftParenBeforeRightSqrBracket &&
                rightParenBeforeRightSqrBracket &&
                leftParenBeforeRightParen &&
                attrNameAfterLeftSqrBracket &&
                attrNameBeforeLeftParenBracket)
            {
                continue;
            }

            resultLines.Add(lines[i]);
        }

        var result = string.Join(Environment.NewLine, resultLines);

        return result;
    }
}
