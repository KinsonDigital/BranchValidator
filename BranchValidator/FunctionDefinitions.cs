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
