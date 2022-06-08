// <copyright file="FunctionDefinitions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator;

/// <inheritdoc/>
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
}
