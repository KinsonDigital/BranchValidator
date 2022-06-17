// <copyright file="FunctionDefinitions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace BranchValidator;

/// <summary>
/// Holds all of the functions that can be used in an expression and
/// acts like a container to build the C# script for execution during runtime.
/// </summary>
[SuppressMessage("Requirements", "CA1822:Mark members as static", Justification = "Methods cannot be static due to reflection requirements and invocation.")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global", Justification = "Methods cannot be static due to reflection requirements and invocation.")]
public class FunctionDefinitions
{
    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };
    private readonly string branchName;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionDefinitions"/> class.
    /// </summary>
    /// <param name="branchName">The name of the branch.</param>
    public FunctionDefinitions(string branchName) => this.branchName = branchName;

#pragma warning disable SA1005
#pragma warning disable SA1515
#pragma warning disable SA1514
// ReSharper disable ArrangeMethodOrOperatorBody

    //<script-function>
    /// <inheritdoc/>
    public bool EqualTo(string value)
    {
        if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(this.branchName))
        {
            return true;
        }

        return value == this.branchName;
    }

    /// <inheritdoc/>
    public bool IsCharNum(uint charPos)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        return charPos <= this.branchName.Length - 1 && Numbers.Contains(this.branchName[(int)charPos]);
    }

    /// <inheritdoc/>
    public bool IsSectionNum(uint startPos, uint endPos)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        endPos = endPos > this.branchName.Length - 1 ? (uint)this.branchName.Length - 1u : endPos;

        for (var i = startPos; i <= endPos; i++)
        {
            if (Numbers.Contains(this.branchName[(int)i]) is false)
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public bool IsSectionNum(uint startPos, string upToChar)
    {
        if (string.IsNullOrEmpty(this.branchName) || string.IsNullOrEmpty(upToChar))
        {
            return false;
        }

        if (startPos > this.branchName.Length - 1)
        {
            return false;
        }

        var upToCharIndex = this.branchName.IndexOf(upToChar[0], (int)startPos);

        if (upToCharIndex == -1)
        {
            return false;
        }

        var section = this.branchName.Substring((int)startPos, upToCharIndex - (int)startPos);

        return !string.IsNullOrEmpty(section) && section.All(c => Numbers.Contains(c));
    }

    /// <inheritdoc/>
    public bool Contains(string value)
    {
        return this.branchName.Contains(value);
    }

    /// <inheritdoc/>
    public bool NotContains(string value)
    {
        return this.branchName.Contains(value) is false;
    }

    /// <inheritdoc/>
    public bool ExistTotal(string value, uint total)
    {
        return Count(this.branchName, value) == total;
    }

    /// <inheritdoc/>
    public bool ExistsLessThan(string value, uint total)
    {
        return Count(this.branchName, value) < total;
    }

    /// <inheritdoc/>
    public bool ExistsGreaterThan(string value, uint total)
    {
        return Count(this.branchName, value) > total;
    }

    /// <inheritdoc/>
    public bool StartsWith(string value)
    {
        return this.branchName.StartsWith(value);
    }

    /// <inheritdoc/>
    public bool NotStartsWith(string value)
    {
        return !this.branchName.StartsWith(value);
    }

    /// <inheritdoc/>
    public bool EndsWith(string value)
    {
        return this.branchName.EndsWith(value);
    }

    /// <inheritdoc/>
    public bool NotEndsWith(string value)
    {
        return !this.branchName.EndsWith(value);
    }

    /// <inheritdoc/>
    public bool StartsWithNum()
    {
        return !string.IsNullOrEmpty(this.branchName) && Numbers.Contains(this.branchName[0]);
    }

    /// <inheritdoc/>
    public bool EndsWithNum()
    {
        return !string.IsNullOrEmpty(this.branchName) && Numbers.Contains(this.branchName[^1]);
    }

    /// <inheritdoc/>
    public bool LenLessThan(uint length)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        return this.branchName.Length < length;
    }

    /// <inheritdoc/>
    public bool LenGreaterThan(uint length)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return length > 0;
        }

        return this.branchName.Length > length;
    }

    /// <inheritdoc/>
    public bool IsBefore(string value, string after)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        var valueIndex = this.branchName.IndexOf(value, StringComparison.Ordinal);
        var afterIndex = this.branchName.IndexOf(after, StringComparison.Ordinal);

        return valueIndex < afterIndex;
    }

    /// <inheritdoc/>
    public bool IsAfter(string value, string before)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        var valueIndex = this.branchName.IndexOf(value, StringComparison.Ordinal);
        var afterIndex = this.branchName.IndexOf(before, StringComparison.Ordinal);

        return valueIndex > afterIndex;
    }

    private static int Count(string thisStr, string value)
    {
        // Go through each character and escape it if needed
        for (var i = 0; i < value.Length; i++)
        {
            var character = value[i];

            if (character == '|')
            {
                value = value.Insert(i, @"\");
                i++;
            }
        }

        return Regex.Matches(thisStr, value, RegexOptions.IgnoreCase).Count;
    }
    //</script-function>

// ReSharper restore ArrangeMethodOrOperatorBody
#pragma warning restore SA1005
#pragma warning restore SA1515
#pragma warning restore SA1514
}
