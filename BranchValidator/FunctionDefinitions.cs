// <copyright file="FunctionDefinitions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using BranchValidatorShared;

namespace BranchValidator;

/// <summary>
/// Represents the type of matching that should be performed.
/// </summary>
public enum MatchType
{
    /// <summary>
    /// Match against the entire string.
    /// </summary>
    All,

    /// <summary>
    /// Only match against the beginning of the string.
    /// </summary>
    Start,

    /// <summary>
    /// Only match against the end of the string.
    /// </summary>
    End,
}

/// <summary>
/// Holds all of the functions that can be used in an expression and
/// acts like a container to build the C# script for execution during runtime.
/// </summary>
[SuppressMessage("Requirements", "CA1822:Mark members as static", Justification = "Methods cannot be static due to reflection requirements and invocation.")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global", Justification = "Methods cannot be static due to reflection requirements and invocation.")]
public class FunctionDefinitions
{
    private const char MatchNumbers = '#';
    private const char MatchAnything = '*';
    private static readonly List<string> FunctionResults = new ();
    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };
    private static readonly char[] LowerCaseLetters =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
    };
    private static readonly char[] UpperCaseLetters =
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
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
    /// Gets the results of all the functions.
    /// </summary>
    /// <returns>The result of all the functions.</returns>
    public static string[] GetFunctionResults()
    {
        return FunctionResults.ToArray();
    }

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
        var branch = string.IsNullOrEmpty(this.branchName) ? string.Empty : this.branchName;
        value = string.IsNullOrEmpty(value) ? string.Empty : value;

        var hasGlobbingSyntax = value.Contains(MatchNumbers) || value.Contains(MatchAnything);
        var isEqual = hasGlobbingSyntax
            ? Match(branch, value, MatchType.All)
            : (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(branch)) || value == branch;

        RegisterFunctionResult($"{nameof(EqualTo)}({typeof(string)})", isEqual);

        return isEqual;
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
    [ExpressionFunction(nameof(CharIsNum))]
    public bool CharIsNum(uint charPos)
    {
        var notNullOrEmpty = !string.IsNullOrEmpty(this.branchName);

        var branch = notNullOrEmpty ? this.branchName : string.Empty;

        var charIsNum = charPos <= branch.Length - 1 && MemoryExtensions.Contains(Numbers, branch[(int)charPos]);

        var result = notNullOrEmpty && charIsNum;
        RegisterFunctionResult($"{nameof(CharIsNum)}({typeof(uint)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> is contained within a branch name.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <returns><c>true</c> if a branch name contains the <paramref name="value"/>.</returns>
    /// <remarks>
    ///     The search is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(Contains))]
    public bool Contains(string value)
    {
        var branchNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var branch = branchNotNullOrEmpty ? this.branchName : string.Empty;
        var hasGlobbingSyntax = value.Contains(MatchNumbers) || value.Contains(MatchAnything);
        var contains = hasGlobbingSyntax
            ? branchNotNullOrEmpty && Match(branch, value, MatchType.All)
            : branchNotNullOrEmpty && branch.Contains(value);

        RegisterFunctionResult($"{nameof(Contains)}({typeof(string)})", contains);

        return contains;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> is contained within a branch name.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <returns><c>true</c> if a branch name does not contain the <paramref name="value"/>.</returns>
    /// <remarks>
    ///     The search is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(NotContains))]
    public bool NotContains(string value)
    {
        var branchNullOrEmpty = string.IsNullOrEmpty(this.branchName);
        var branch = branchNullOrEmpty ? string.Empty : this.branchName;
        var hasGlobbingSyntax = value.Contains(MatchNumbers) || value.Contains(MatchAnything);
        var doesNotContain = hasGlobbingSyntax
            ? branchNullOrEmpty || !Match(branch, value, MatchType.All)
            : branchNullOrEmpty || !branch.Contains(value);

        RegisterFunctionResult($"{nameof(NotContains)}({typeof(string)})", doesNotContain);

        return doesNotContain;
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
    [ExpressionFunction(nameof(ExistsTotal))]
    public bool ExistsTotal(string value, uint total)
    {
        var branchIsNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var totalExists = Count(this.branchName, value) == total;

        var result = branchIsNotNullOrEmpty && totalExists;

        RegisterFunctionResult($"{nameof(ExistsTotal)}({typeof(string)}, {typeof(uint)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> exists
    /// less than the given <paramref name="total"/> amount of times.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <param name="total">The total number of times to check that the <paramref name="value"/> exists.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> exists greater than the specified number of times.</returns>
    [ExpressionFunction(nameof(ExistsLessThan))]
    public bool ExistsLessThan(string value, uint total)
    {
        var branchIsNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var isLessThan = Count(this.branchName, value) < total;

        var result = branchIsNotNullOrEmpty && isLessThan;

        RegisterFunctionResult($"{nameof(ExistsLessThan)}({typeof(string)}, {typeof(uint)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/>
    /// exists greater than the given <paramref name="total"/> number of times.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <param name="total">The total number of times to check that the <paramref name="value"/> exists.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> exists greater than the specified number of times.</returns>
    [ExpressionFunction(nameof(ExistsGreaterThan))]
    public bool ExistsGreaterThan(string value, uint total)
    {
        var branchIsNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var isGreaterThan = Count(this.branchName, value) > total;

        var result = branchIsNotNullOrEmpty && isGreaterThan;

        RegisterFunctionResult($"{nameof(ExistsGreaterThan)}({typeof(string)}, {typeof(uint)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch name starts with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly starts with.</param>
    /// <returns><c>true</c> if the branch starts with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(StartsWith))]
    public bool StartsWith(string value)
    {
        var branchIsNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var branch = branchIsNotNullOrEmpty ? this.branchName : string.Empty;
        var hasGlobbingSyntax = value.Contains(MatchNumbers) || value.Contains(MatchAnything);
        var startsWith = hasGlobbingSyntax
            ? Match(branch, value, MatchType.Start)
            : branch.StartsWith(value);
        var result = branchIsNotNullOrEmpty && startsWith;

        RegisterFunctionResult($"{nameof(StartsWith)}({typeof(string)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch name starts with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly starts with.</param>
    /// <returns><c>true</c> if the branch does not start with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(NotStartsWith))]
    public bool NotStartsWith(string value)
    {
        var branchNullOrEmpty = string.IsNullOrEmpty(this.branchName);

        var hasGlobbingSyntax = value.Contains(MatchNumbers) || value.Contains(MatchAnything);
        var startsWith = hasGlobbingSyntax
            ? branchNullOrEmpty || !Match(this.branchName, value, MatchType.Start)
            : branchNullOrEmpty || !this.branchName.StartsWith(value);

        RegisterFunctionResult($"{nameof(NotStartsWith)}({typeof(string)})", startsWith);

        return startsWith;
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch name ends with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly ends with.</param>
    /// <returns><c>true</c> if the branch does ends with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(EndsWith))]
    public bool EndsWith(string value)
    {
        var branchIsNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var branch = branchIsNotNullOrEmpty ? this.branchName : string.Empty;

        var hasGlobbingSyntax = value.Contains(MatchNumbers) || value.Contains(MatchAnything);

        var endsWith = hasGlobbingSyntax
            ? Match(branch, value, MatchType.End)
            : branch.EndsWith(value);
        var result = branchIsNotNullOrEmpty && endsWith;

        RegisterFunctionResult($"{nameof(endsWith)}({typeof(string)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not a branch name ends with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly ends with.</param>
    /// <returns><c>true</c> if the branch does not end with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    [ExpressionFunction(nameof(NotEndsWith))]
    public bool NotEndsWith(string value)
    {
        var branchNullOrEmpty = string.IsNullOrEmpty(this.branchName);

        var hasGlobbingSyntax = value.Contains(MatchNumbers) || value.Contains(MatchAnything);
        var endsWith = hasGlobbingSyntax
            ? branchNullOrEmpty || !Match(this.branchName, value, MatchType.End)
            : branchNullOrEmpty || !this.branchName.EndsWith(value);

        RegisterFunctionResult($"{nameof(NotEndsWith)}({typeof(string)})", endsWith);

        return endsWith;
    }

    /// <summary>
    /// Returns a value indicating whether or not the length of the branch name is less than the given <paramref name="length"/>.
    /// </summary>
    /// <param name="length">The length to compare to the length of the branch.</param>
    /// <returns><c>true</c> if the length of the branch is less than the given <paramref name="length"/>.</returns>
    /// <remarks>
    ///     If the <paramref name="length"/> value is less than 0, then 0 will be used.
    /// </remarks>
    [ExpressionFunction(nameof(LenLessThan))]
    public bool LenLessThan(uint length)
    {
        var branchIsNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var branch = branchIsNotNullOrEmpty ? this.branchName : string.Empty;
        var lengthIsLessThan = branch.Length < length;

        var result = branchIsNotNullOrEmpty && lengthIsLessThan;

        RegisterFunctionResult($"{nameof(LenLessThan)}({typeof(uint)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not the length of the branch name is greater than the given <paramref name="length"/>.
    /// </summary>
    /// <param name="length">The length to compare to the length of the branch.</param>
    /// <returns><c>true</c> if the length of the branch is greater than the given <paramref name="length"/>.</returns>
    [ExpressionFunction(nameof(LenGreaterThan))]
    public bool LenGreaterThan(uint length)
    {
        var branchIsNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var branch = branchIsNotNullOrEmpty ? this.branchName : string.Empty;
        var lengthIsGreaterThan = branch.Length > length;

        var result = branchIsNotNullOrEmpty && lengthIsGreaterThan;

        RegisterFunctionResult($"{nameof(LenGreaterThan)}({typeof(uint)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> <c>string</c> is located
    /// before the given <paramref name="after"/> <c>string</c>.
    /// </summary>
    /// <param name="value">The <c>string</c> located before <paramref name="after"/> <c>string</c>.</param>
    /// <param name="after">The <c>string</c> located after the <paramref name="value"/> <c>string</c>.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> <c>string</c> is located before the <paramref name="after"/> <c>string</c>.</returns>
    [ExpressionFunction(nameof(IsBefore))]
    public bool IsBefore(string value, string after)
    {
        var branchIsNotNullOrEmpty = !string.IsNullOrEmpty(this.branchName);
        var branch = branchIsNotNullOrEmpty ? this.branchName : string.Empty;
        var valueIndex = branch.IndexOf(value, StringComparison.Ordinal);
        var afterIndex = branch.IndexOf(after, StringComparison.Ordinal);
        var isBefore = valueIndex < afterIndex;

        var result = branchIsNotNullOrEmpty && isBefore;

        RegisterFunctionResult($"{nameof(IsBefore)}({typeof(string)}, {typeof(string)})", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not the branch name has all upper case letters.
    /// </summary>
    /// <returns><c>true</c> if all of the letters are uppercase.</returns>
    [ExpressionFunction(nameof(AllUpperCase))]
    public bool AllUpperCase()
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        var result = true;

        foreach (var c in this.branchName)
        {
            var isLetter = Contains(LowerCaseLetters, c) || Contains(UpperCaseLetters, c);

            // If the character is a symbol, ignore and move on to the next
            if (isLetter && Contains(UpperCaseLetters, c) is false)
            {
                result = false;
                break;
            }
        }

        RegisterFunctionResult($"{nameof(AllUpperCase)}()", result);

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether or not the branch name has all lower case letters.
    /// </summary>
    /// <returns><c>true</c> if all of the letters are lowercase.</returns>
    [ExpressionFunction(nameof(AllLowerCase))]
    public bool AllLowerCase()
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        var result = true;

        foreach (var c in this.branchName)
        {
            var isLetter = Contains(LowerCaseLetters, c) || Contains(UpperCaseLetters, c);

            // If the character is a symbol, ignore and move on to the next
            if (isLetter && Contains(LowerCaseLetters, c) is false)
            {
                result = false;
                break;
            }
        }

        RegisterFunctionResult($"{nameof(AllLowerCase)}()", result);

        return result;
    }

    /// <summary>
    /// Registers the given function <paramref name="name"/> and its associated result.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="result">The result of the function.</param>
    private static void RegisterFunctionResult(string name, bool result)
    {
        var newName = $"{name[0].ToString().ToLower()}{name[1..]}";

        // Replace types if they exist
        newName = newName.Replace($"{typeof(int)}", "number");
        newName = newName.Replace($"{typeof(uint)}", "number");
        newName = newName.Replace($"{typeof(string)}", "string");

        FunctionResults.Add($"{newName} -> {result.ToString().ToLower()}");
    }

    /// <summary>
    /// Counts how many times the given <paramref name="value"/> exists in the this <c>string</c>.
    /// </summary>
    /// <param name="thisStr">The string that might contain the <paramref name="value"/>.</param>
    /// <param name="value">The value to count.</param>
    /// <returns>The number of times the <paramref name="value"/> exists.</returns>
    private static int Count(string thisStr, string value)
    {
        // NOTE: Refer to this website for more regex information -> https://regex101.com/
        if (string.IsNullOrEmpty(thisStr))
        {
            return 0;
        }

        // Go through each character and escape it if needed
        // This is due to some characters having a regex specific meaning
        for (var i = 0; i < value.Length; i++)
        {
            var character = value[i];

            if (character == '|')
            {
                value = value.Insert(i, @"\");
                i++;
                continue;
            }

            if (character == '.')
            {
                value = value.Insert(i, @"\");
                i++;
            }
        }

        return Regex.Matches(thisStr, value, RegexOptions.IgnoreCase).Count;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="character"/>
    /// is contained in the given <c>string</c> <paramref name="characters"/>.
    /// </summary>
    /// <param name="characters">The <c>string</c> that might contain the character.</param>
    /// <param name="character">The character to check for.</param>
    /// <returns>
    ///     <c>true</c> if the given <paramref name="character"/> is
    ///     contained in the <c>string</c> <paramref name="characters"/>.
    /// </returns>
    private static bool Contains(char[] characters, char character)
    {
        foreach (var c in characters)
        {
            if (c == character)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a value indicating if the given <paramref name="globbingPattern"/> contains a match
    /// to the given <c>string</c> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <c>string</c> to match against.</param>
    /// <param name="globbingPattern">The globbing pattern and text to search for.</param>
    /// <param name="matchType">The type of matching that should be performed.</param>
    /// <returns>
    ///     <c>true</c> if the globbing pattern finds a match in the given <c>string</c> <paramref name="value"/>.
    /// </returns>
    private static bool Match(string value, string globbingPattern, MatchType matchType)
    {
        // NOTE: Refer to this website for more regex information -> https://regex101.com/
        const char matchNumbers = '#';
        const char matchAnything = '*';
        const char regexMatchStart = '^';
        const char regexMatchEnd = '$';
        const string regexMatchNumbers = @"\d+";
        const string regexMatchAnything = ".+";

        // Remove any consecutive '#' and '*' symbols until no more consecutive symbols exists anymore
        globbingPattern = RemoveConsecutiveCharacters(new[] { matchNumbers, matchAnything }, globbingPattern);

        // Replace the '#' symbol with
        globbingPattern = globbingPattern.Replace(matchNumbers.ToString(), regexMatchNumbers);

        // Prefix all '.' symbols with '\' to match the '.' literally in regex
        globbingPattern = globbingPattern.Replace(".", @"\.");

        // Replace all '*' character with '.+'
        globbingPattern = globbingPattern.Replace(matchAnything.ToString(), regexMatchAnything);

        if (matchType == MatchType.Start)
        {
            globbingPattern = $"{regexMatchStart}{globbingPattern}";
        }
        else if (matchType == MatchType.End)
        {
            globbingPattern = $"{globbingPattern}{regexMatchEnd}";
        }

        return Regex.Matches(value, globbingPattern, RegexOptions.IgnoreCase).Count > 0;
    }

    /// <summary>
    /// Removes any consecutive occurrences of the given <paramref name="characters"/> from the given <c>string</c> <paramref name="value"/>.
    /// </summary>
    /// <param name="characters">The <c>char</c> to check for.</param>
    /// <param name="value">The value to remove the consecutive characters from.</param>
    /// <returns>The original <c>string</c> value with the consecutive characters removed.</returns>
    private static string RemoveConsecutiveCharacters(IEnumerable<char> characters, string value)
    {
        foreach (var c in characters)
        {
            while (value.Contains($"{c}{c}"))
            {
                value = value.Replace($"{c}{c}", c.ToString());
            }
        }

        return value;
    }
    //</script-function>

// ReSharper restore ArrangeMethodOrOperatorBody
#pragma warning restore SA1005
#pragma warning restore SA1515
#pragma warning restore SA1514
#pragma warning restore SA1513
}
