// <copyright file="FunctionDefinitions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace BranchValidator;

/// <inheritdoc/>
[SuppressMessage("Requirements", "CA1822:Mark members as static", Justification = "Methods cannot be static due to reflection requirements and invocation.")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global", Justification = "Methods cannot be static due to reflection requirements and invocation.")]
public class FunctionDefinitions : IFunctionDefinitions
{
    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };

    /// <inheritdoc/>
    public bool EqualTo(string value, string branchName)
    {
        if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(branchName))
        {
            return true;
        }

        return value == branchName;
    }

    /// <inheritdoc/>
    public bool IsCharNum(uint charPos, string branchName)
    {
        if (string.IsNullOrEmpty(branchName))
        {
            return false;
        }

        return charPos <= branchName.Length - 1 && Numbers.Contains(branchName[(int)charPos]);
    }

    /// <inheritdoc/>
    public bool IsSectionNum(uint startPos, uint endPos, string branchName)
    {
        if (string.IsNullOrEmpty(branchName))
        {
            return false;
        }

        endPos = endPos > branchName.Length - 1 ? (uint)branchName.Length - 1u : endPos;

        for (var i = startPos; i <= endPos; i++)
        {
            if (Numbers.DoesNotContain(branchName[(int)i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public bool IsSectionNum(uint startPos, string upToChar, string branchName)
    {
        if (string.IsNullOrEmpty(branchName) || string.IsNullOrEmpty(upToChar))
        {
            return false;
        }

        if (startPos > branchName.Length - 1)
        {
            return false;
        }

        var upToCharIndex = branchName.IndexOf(upToChar[0], (int)startPos);

        if (upToCharIndex == -1)
        {
            return false;
        }

        var section = branchName.Substring((int)startPos, upToCharIndex - (int)startPos);

        return !string.IsNullOrEmpty(section) && section.All(c => Numbers.Contains(c));
    }
}
