// <copyright file="ExpressionFunctions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable SA1137
#pragma warning disable SA1633
#pragma warning disable SA1600
#pragma warning disable SA1027
#pragma warning disable SA1515
#pragma warning disable CA1050
#pragma warning disable SA1005
// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Global

/*THIS SCRIPT IS AUTO-GENERATED AND SHOULD NOT BE CHANGED MANUALLY*/

using System.Text.RegularExpressions;

public static class ExpressionFunctions
{
	private const string BranchName = "//<branch-name/>";
	private static readonly char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', };

    public static bool EqualTo(string value)
    {
        if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(BranchName))
        {
            return true;
        }

        return value == BranchName;
    }

    public static bool IsCharNum(uint charPos)
    {
        if (string.IsNullOrEmpty(BranchName))
        {
            return false;
        }

        return charPos <= BranchName.Length - 1 && Numbers.Contains(BranchName[(int)charPos]);
    }

    public static bool IsSectionNum(uint startPos, uint endPos)
    {
        if (string.IsNullOrEmpty(BranchName))
        {
            return false;
        }

        endPos = endPos > BranchName.Length - 1 ? (uint)BranchName.Length - 1u : endPos;

        for (var i = startPos; i <= endPos; i++)
        {
            if (Numbers.Contains(BranchName[(int)i]) is false)
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsSectionNum(uint startPos, string upToChar)
    {
        if (string.IsNullOrEmpty(BranchName) || string.IsNullOrEmpty(upToChar))
        {
            return false;
        }

        if (startPos > BranchName.Length - 1)
        {
            return false;
        }

        var upToCharIndex = BranchName.IndexOf(upToChar[0], (int)startPos);

        if (upToCharIndex == -1)
        {
            return false;
        }

        var section = BranchName.Substring((int)startPos, upToCharIndex - (int)startPos);

        return !string.IsNullOrEmpty(section) && section.All(c => Numbers.Contains(c));
    }

    public static bool Contains(string value)
    {
        return BranchName.Contains(value);
    }

    public static bool NotContains(string value)
    {
        return BranchName.Contains(value) is false;
    }

    public static bool ExistTotal(string value, uint total)
    {
        return Count(BranchName, value) == total;
    }

    public static bool ExistsLessThan(string value, uint total)
    {
        return Count(BranchName, value) < total;
    }

    public static bool ExistsGreaterThan(string value, uint total)
    {
        return Count(BranchName, value) > total;
    }

    public static bool StartsWith(string value)
    {
        return BranchName.StartsWith(value);
    }

    public static bool NotStartsWith(string value)
    {
        return !BranchName.StartsWith(value);
    }

    public static bool EndsWith(string value)
    {
        return BranchName.EndsWith(value);
    }

    public static bool NotEndsWith(string value)
    {
        return !BranchName.EndsWith(value);
    }

    public static bool StartsWithNum()
    {
        return !string.IsNullOrEmpty(BranchName) && Numbers.Contains(BranchName[0]);
    }

    public static bool EndsWithNum()
    {
        return !string.IsNullOrEmpty(BranchName) && Numbers.Contains(BranchName[^1]);
    }

    public static bool LenLessThan(uint length)
    {
        if (string.IsNullOrEmpty(BranchName))
        {
            return false;
        }

        return BranchName.Length < length;
    }

    public static bool LenGreaterThan(uint length)
    {
        if (string.IsNullOrEmpty(BranchName))
        {
            return length > 0;
        }

        return BranchName.Length > length;
    }

    public static bool IsBefore(string value, string after)
    {
        if (string.IsNullOrEmpty(BranchName))
        {
            return false;
        }

        var valueIndex = BranchName.IndexOf(value, StringComparison.Ordinal);
        var afterIndex = BranchName.IndexOf(after, StringComparison.Ordinal);

        return valueIndex < afterIndex;
    }

    public static bool IsAfter(string value, string before)
    {
        if (string.IsNullOrEmpty(BranchName))
        {
            return false;
        }

        var valueIndex = BranchName.IndexOf(value, StringComparison.Ordinal);
        var afterIndex = BranchName.IndexOf(before, StringComparison.Ordinal);

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
}

//<expression/>
