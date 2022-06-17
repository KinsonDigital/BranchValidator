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
#pragma warning disable SA1513
// ReSharper disable ArrangeMethodOrOperatorBody

    //<script-function>
    /// <summary>
    /// Returns a value indicating whether or not a branch with the given branch name
    /// matches the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to check against the branch name.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> is equal to the branch name.</returns>
    /// <remarks>
    ///     The comparison is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool EqualTo(string value)
    {
        if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(this.branchName))
        {
            return true;
        }

        return value == this.branchName;
    }

    /// <summary>
    /// Returns a value indicating whether or not the <c>char</c> at the given <paramref name="charPos"/>
    /// is a number in a branch <c>string</c> that matches the given branch name.
    /// </summary>
    /// <param name="charPos">The position of the character.</param>
    /// <returns><c>true</c> if the character is a number.</returns>
    /// <remarks>
    /// Things to consider:
    /// <list type="bullet">
    ///     <item>
    ///         If the character position is larger than the length of the branch name,
    ///         then the result will automatically be <c>false</c>.
    ///     </item>
    ///     <item>
    ///         A null or empty branch name will return <c>false.</c>
    ///     </item>
    /// </list>
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool IsCharNum(uint charPos)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        return charPos <= this.branchName.Length - 1 && Numbers.Contains(this.branchName[(int)charPos]);
    }

    /// <summary>
    /// Returns a value indicating whether or not a section of a branch name is a whole number.
    /// The section is determined by the text between the <paramref name="startPos"/> and <paramref name="endPos"/>.
    /// </summary>
    /// <param name="startPos">The starting position of the section to check.</param>
    /// <param name="endPos">The ending position of the section to check.</param>
    /// <returns><c>true</c> if the section is a whole number.</returns>
    /// <remarks>
    /// Things to consider:
    /// <list type="bullet">
    ///     <item>
    ///         If the <paramref name="startPos"/> is larger than the length of the branch name, then the length of the branch name is used.
    ///     </item>
    ///     <item>
    ///         If the <paramref name="endPos"/> is larger than the length of the branch name, then the length of the branch name is used.
    ///     </item>
    /// </list>
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
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

    /// <summary>
    /// Returns a value indicating whether or not a section of a branch name is a whole number.
    /// The section is determined by the text between the <paramref name="startPos"/> and the position of the first
    /// occurrence of the given <paramref name="upToChar"/>.
    /// </summary>
    /// <param name="startPos">The starting position of the section to check.</param>
    /// <param name="upToChar">The character to signify the ending position of the section.</param>
    /// <returns><c>true</c> if the section is a whole number.</returns>
    /// <remarks>
    ///     Things to consider:
    ///     <list type="bullet">
    ///         <item>
    ///             If the <paramref name="startPos"/> is larger than the length of the branch name, then the length of the branch name is used.
    ///         </item>
    ///         <item>
    ///             If the <paramref name="upToChar"/> does not exist anywhere after the <paramref name="startPos"/>, then the rest of the <c>string</c>
    ///             after the <paramref name="startPos"/> will be considered.
    ///         </item>
    ///         <item>
    ///             The <paramref name="upToChar"/> is exclusive in the <c>string</c> section to be checked.
    ///         </item>
    ///         <item>
    ///             The match against the <paramref name="upToChar"/> is vase sensitive if it is a letter.
    ///         </item>
    ///     </list>
    ///
    ///     Examples:
    ///     <list type="bullet">
    ///         <item>
    ///             Example 1:
    ///             <code>
    ///             // This would return true if the branch name was 'feature/123-test-branch'
    ///             IsCheckNum(8, "-");
    ///             </code>
    ///         </item>
    ///         <item>
    ///             Example 2:
    ///             <code>
    ///             // This would return true if the branch name was 'feature/12345'
    ///             IsCheckNum(8, "-");
    ///             </code>
    ///         </item>
    ///         <item>
    ///             Example 3:
    ///             <code>
    ///             // This would return false if the branch name was 'feature/123testbranch'
    ///             IsCheckNum(8, "-");
    ///             </code>
    ///         </item>
    ///     </list>
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
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

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> is contained within a branch name.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <returns><c>true</c> if a branch name contains the <paramref name="value"/>.</returns>
    /// <remarks>
    ///     The search is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool Contains(string value)
    {
        return this.branchName.Contains(value);
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> is contained within a branch name.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <returns><c>true</c> if a branch name does not contain the <paramref name="value"/>.</returns>
    /// <remarks>
    ///     The search is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool NotContains(string value)
    {
        return this.branchName.Contains(value) is false;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> occurs within
    /// a branch name the given <paramref name="total"/> amount of times.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <param name="total">The total number of times that the <paramref name="value"/> should exist.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> exists the exact specified number of times.</returns>
    /// <remarks>
    ///     The search is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool ExistTotal(string value, uint total)
    {
        return Count(this.branchName, value) == total;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> exists
    /// less than the given <paramref name="total"/> amount of times.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <param name="total">The total number of times to check that the <paramref name="value"/> exists.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> exists greater than the specified number of times.</returns>
    [ExpressionFunction(nameof(EqualTo))]
    public bool ExistsLessThan(string value, uint total)
    {
        return Count(this.branchName, value) < total;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/>
    /// exists greater than the given <paramref name="total"/> number of times.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <param name="total">The total number of times to check that the <paramref name="value"/> exists.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> exists greater than the specified number of times.</returns>
    [ExpressionFunction(nameof(EqualTo))]
    public bool ExistsGreaterThan(string value, uint total)
    {
        return Count(this.branchName, value) > total;
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch name starts with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly starts with.</param>
    /// <returns><c>true</c> if the branch starts with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool StartsWith(string value)
    {
        return this.branchName.StartsWith(value);
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch name starts with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly starts with.</param>
    /// <returns><c>true</c> if the branch does not start with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool NotStartsWith(string value)
    {
        return !this.branchName.StartsWith(value);
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch name ends with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly ends with.</param>
    /// <returns><c>true</c> if the branch does ends with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool EndsWith(string value)
    {
        return this.branchName.EndsWith(value);
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch name ends with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly ends with.</param>
    /// <returns><c>true</c> if the branch does not end with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool NotEndsWith(string value)
    {
        return !this.branchName.EndsWith(value);
    }

    /// <summary>
    /// Returns a value indicating whether or not the branch name starts with a number.
    /// </summary>
    /// <returns><c>true</c> if the branch starts with a number.</returns>
    [ExpressionFunction(nameof(EqualTo))]
    public bool StartsWithNum()
    {
        return !string.IsNullOrEmpty(this.branchName) && Numbers.Contains(this.branchName[0]);
    }

    /// <summary>
    /// Returns a value indicating whether or not the branch name ends with a number.
    /// </summary>
    /// <returns><c>true</c> if the branch ends with a number.</returns>
    [ExpressionFunction(nameof(EqualTo))]
    public bool EndsWithNum()
    {
        return !string.IsNullOrEmpty(this.branchName) && Numbers.Contains(this.branchName[^1]);
    }

    /// <summary>
    /// Returns a value indicating whether or not the length of the branch name is less than the given <paramref name="length"/>.
    /// </summary>
    /// <param name="length">The length to compare to the length of the branch.</param>
    /// <returns><c>true</c> if the length of the branch is less than the given <paramref name="length"/>.</returns>
    /// <remarks>
    ///     If the <paramref name="length"/> value is less than 0, then 0 will be used.
    /// </remarks>
    [ExpressionFunction(nameof(EqualTo))]
    public bool LenLessThan(uint length)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        return this.branchName.Length < length;
    }

    /// <summary>
    /// Returns a value indicating whether or not the length of the branch name is greater than the given <paramref name="length"/>.
    /// </summary>
    /// <param name="length">The length to compare to the length of the branch.</param>
    /// <returns><c>true</c> if the length of the branch is greater than the given <paramref name="length"/>.</returns>
    [ExpressionFunction(nameof(EqualTo))]
    public bool LenGreaterThan(uint length)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return length > 0;
        }

        return this.branchName.Length > length;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> <c>string</c> is located
    /// before the given <paramref name="after"/> <c>string</c>.
    /// </summary>
    /// <param name="value">The <c>string</c> located before <paramref name="after"/> <c>string</c>.</param>
    /// <param name="after">The <c>string</c> located after the <paramref name="value"/> <c>string</c>.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> <c>string</c> is located before the <paramref name="after"/> <c>string</c>.</returns>
    [ExpressionFunction(nameof(EqualTo))]
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

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> <c>string</c> is located
    /// after the given <paramref name="before"/> <c>string</c>.
    /// </summary>
    /// <param name="value">The <c>string</c> located after <paramref name="before"/> <c>string</c>.</param>
    /// <param name="before">The <c>string</c> located before the <paramref name="value"/> <c>string</c>.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> <c>string</c> is located after the <paramref name="before"/> <c>string</c>.</returns>
    [ExpressionFunction(nameof(EqualTo))]
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

    /// <summary>
    /// Counts how many times the given <paramref name="value"/> exists in the this <c>string</c>.
    /// </summary>
    /// <param name="thisStr">The string that might contain the <paramref name="value"/>.</param>
    /// <param name="value">The value to count.</param>
    /// <returns>The number of times the <paramref name="value"/> exists.</returns>
    private static int Count(string thisStr, string value)
    {
        // Go through each character and escape it if needed
        for (var i = 0; i < value.Length; i++)
        {
            var character = value[i];

            if (character != '|')
            {
                continue;
            }

            value = value.Insert(i, @"\");
            i++;
        }

        return Regex.Matches(thisStr, value, RegexOptions.IgnoreCase).Count;
    }
    //</script-function>

// ReSharper restore ArrangeMethodOrOperatorBody
#pragma warning restore SA1005
#pragma warning restore SA1515
#pragma warning restore SA1514
#pragma warning restore SA1513
}
