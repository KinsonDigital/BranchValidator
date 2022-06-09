// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BranchValidator;

/// <summary>
/// Provides extensions.
/// </summary>
public static class ExtensionMethods
{
    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };
    private static readonly char[] LowerCaseLetters =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
    };

    /// <summary>
    /// Converts the given array items of type <typeparamref name="T"/> to a
    /// <see cref="ReadOnlyCollection{T}"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="value">The list of items to convert.</param>
    /// <typeparam name="T">The type of items in the array and <see cref="ReadOnlyCollection{T}"/>.</typeparam>
    /// <returns>A <see cref="ReadOnlyCollection{T}"/> of the array items.</returns>
    public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> value) => new (value.ToArray());

    /// <summary>
    /// Converts the given <c>string</c> <paramref name="value"/> to pascal casing.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The <paramref name="value"/> in pascal case.</returns>
    /// <remarks>
    ///     A null or empty <paramref name="value"/> will return an empty string.
    ///     Only changes the first character to uppercase and assumes the rest of the value is correct.
    /// </remarks>
    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (LowerCaseLetters.Contains(value[0]))
        {
            value = $"{value[0].ToString().ToUpper()}{value.Substring(1, value.Length - 1)}";
        }

        return value;
    }

    /// <summary>
    /// Returns a result indicating whether or not the given <paramref name="item"/> is contained in this list of items.
    /// </summary>
    /// <param name="items">The items to check.</param>
    /// <param name="item">The item to check for.</param>
    /// <typeparam name="T">The type of item in the list.</typeparam>
    /// <returns><c>true</c> if the <paramref name="item"/> does not exist.</returns>
    public static bool DoesNotContain<T>(this IEnumerable<T> items, T item) => !items.Contains(item);

    /// <summary>
    /// Returns the number of times the <c>string</c> <paramref name="value"/> occurs in this <c>string</c>.
    /// </summary>
    /// <param name="thisStr">The <c>string</c> that might contain the <paramref name="value"/>.</param>
    /// <param name="value">The <c>string</c> to check for.</param>
    /// <returns>The number of times it exists.</returns>
    public static int Count(this string thisStr, string value)
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

    /// <summary>
    /// Returns a value indicating whether or not the given this <c>string</c> <paramref name="value"/> does not start with the given <paramref name="character"/>.
    /// </summary>
    /// <param name="value">The value that might start with the <paramref name="character"/>.</param>
    /// <param name="character">The character to check for.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> does not start with the <paramref name="character"/>.</returns>
    public static bool DoesNotStartWith(this string value, char character) => !value.StartsWith(character);

    /// <summary>
    /// Returns a value indicating whether or not the given this <c>string</c> <paramref name="value"/> ends with the given <paramref name="character"/>.
    /// </summary>
    /// <param name="value">The value that might end with the <paramref name="character"/>.</param>
    /// <param name="character">The character to check for.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> does not end with the <paramref name="character"/>.</returns>
    public static bool DoesNotEndWith(this string value, char character) => !value.EndsWith(character);

    /// <summary>
    /// Returns a value indicating if the given <paramref name="character"/> is contained in this <c>string</c> value.
    /// </summary>
    /// <param name="value">The <c>string</c> that might hold the <paramref name="character"/>.</param>
    /// <param name="character">The character to check for.</param>
    /// <returns><c>true</c> if the <paramref name="character"/> is contained.</returns>
    public static bool DoesNotContain(this string value, char character) => !string.IsNullOrEmpty(value) && !value.Contains(character);

    /// <summary>
    /// Returns a value indicating whether or not a specified substring does not occur within this string.
    /// </summary>
    /// <param name="thisStr">The value container.</param>
    /// <param name="value">The string to seek.</param>
    /// <returns><c>true</c> if the value parameter does not occur within this string.</returns>
    public static bool DoesNotContain(this string thisStr, string value)
    {
        if (string.IsNullOrEmpty(thisStr) && !string.IsNullOrEmpty(value))
        {
            return false;
        }

        if (string.IsNullOrEmpty(thisStr) && string.IsNullOrEmpty(value))
        {
            return true;
        }

        return !thisStr.Contains(value);
    }

    /// <summary>
    /// Returns the text data from this <c>string</c> up to the first occurrence of the given <paramref name="character"/>.
    /// </summary>
    /// <param name="value">The value that holds the <c>string</c> to return.</param>
    /// <param name="character">The character that is the end of the string to return.</param>
    /// <returns>All of the <c>string</c> data before the <paramref name="character"/>.</returns>
    public static string GetUpToChar(this string value, char character)
    {
        var result = string.Empty;

        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (DoesNotContain(value, character))
        {
            return value;
        }

        return value.TakeWhile(symbol => symbol != character)
            .Aggregate(result, (current, symbol) => current + symbol);
    }

    /// <summary>
    /// Gets all of the string data between the <paramref name="leftChar"/> and <paramref name="rightChar"/>
    /// for this string.
    /// </summary>
    /// <param name="value">The string to seek in.</param>
    /// <param name="leftChar">The <see cref="char"/> on the left of the string to get.</param>
    /// <param name="rightChar">The <see cref="char"/> on the right of the string to get.</param>
    /// <returns>
    ///     The string that exists between the <paramref name="leftChar"/> and <paramref name="rightChar"/>.
    /// </returns>
    /// <remarks>
    /// Consider Items Below:
    /// <list type="bullet">
    ///     <item>The <paramref name="leftChar"/> is searched for relative from the left end of the <c>this</c> string.</item>
    ///     <item>The <paramref name="rightChar"/> is searched for relative from the right end of the <c>this</c> string.</item>
    ///     <item>If <c>this</c> string is <c>null</c> or <c>empty</c>, then an empty string will be returned.</item>
    /// </list>
    /// </remarks>
    public static string GetBetween(this string value, char leftChar, char rightChar)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var leftIndex = value.IndexOf(leftChar);
        var rightIndex = value.LastIndexOf(rightChar);

        if (leftIndex > rightIndex)
        {
            return string.Empty;
        }

        if (leftIndex == -1 || rightIndex == -1)
        {
            return string.Empty;
        }

        return value.Substring(leftIndex + 1, rightIndex - leftIndex - 1);
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <c>string</c> <paramref name="value"/> is located
    /// between the given <paramref name="startPos"/> and <paramref name="endPos"/> within this <c>string</c>.
    /// </summary>
    /// <param name="thisStr">The <c>string</c> to check in.</param>
    /// <param name="value">The value possibly located between the positions.</param>
    /// <param name="startPos">The starting position.</param>
    /// <param name="endPos">The ending position.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> is in between the positions.</returns>
    /// <remarks>
    ///     An <paramref name="endPos"/> value that is larger than the length of this <c>string</c>
    ///     will use the last character position for the ending position.
    /// </remarks>
    public static bool IsBetween(this string thisStr, string value, uint startPos, uint endPos)
    {
        if (string.IsNullOrEmpty(thisStr) || string.IsNullOrEmpty(value) || thisStr.DoesNotContain(value))
        {
            return false;
        }

        var valuePos = thisStr.IndexOf(value, (int)startPos, StringComparison.Ordinal);

        return valuePos >= startPos && valuePos + value.Length <= endPos;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given <c>string</c> <paramref name="value"/> is located
    /// between the given <paramref name="startPos"/> and <paramref name="endPos"/> within this <c>string</c>.
    /// </summary>
    /// <param name="thisStr">The <c>string</c> to check in.</param>
    /// <param name="value">The value possibly located between the positions.</param>
    /// <param name="startPos">The starting position.</param>
    /// <param name="endPos">The ending position.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> is not in between the positions.</returns>
    /// <remarks>
    ///     An <paramref name="endPos"/> value that is larger than the length of this <c>string</c>
    ///     will use the last character position for the ending position.
    /// </remarks>
    public static bool IsNotBetween(this string thisStr, string value, uint startPos, uint endPos)
        => !IsBetween(thisStr, value, startPos, endPos);

    /// <summary>
    /// Returns a value indicating whether or not the given <c>string</c> <paramref name="value"/> is
    /// a negative or positive whole number.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> is a whole number.</returns>
    public static bool IsWholeNumber(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        if (value.StartsWith('-') && value.Length < 2)
        {
            return false;
        }

        if (value.Contains('-') && value.DoesNotStartWith('-'))
        {
            return false;
        }

        if (value.All(c => c == '-' || Numbers.Contains(c)))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns a result indicating whether or not this <c>object</c> contains a method that matches
    /// the given <paramref name="methodName"/> with the given <paramref name="returnType"/> with the given <paramref name="argValues"/>.
    /// </summary>
    /// <param name="obj">The <c>object</c> that might contain the method.</param>
    /// <param name="methodName">The name of the method to search for.</param>
    /// <param name="returnType">The type returned by the method.</param>
    /// <param name="argValues">The list of argument values.</param>
    /// <returns>
    ///     Tuple Result:
    /// <list type="bullet">
    ///     <item><c>exists:</c> <c>true</c> if the object contains the method.</item>
    ///     <item><c>msg:</c> Additional information about the result.</item>
    /// </list>
    /// </returns>
    public static (bool result, string msg, MethodInfo? method) GetMethod(this object? obj, string methodName, Type returnType, string[]? argValues)
    {
        var totalArgValues = argValues?.Length ?? 0;
        (bool, string, MethodInfo?) invalidResult = (false, $"No function with the name '{methodName}' with '{totalArgValues}' parameters found.", null);

        var possibleMethods = (from m in obj?.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
            where m.Name == methodName
            select m).ToArray();

        MethodInfo? foundMethod = null;
        var methodFound = false;
        var returnMsg = string.Empty;

        if (possibleMethods.Length <= 0)
        {
            return (false, $"A function with the name '{methodName}' does not exist.", null);
        }

        foreach (MethodInfo possibleMethod in possibleMethods)
        {
            var matchFound = true;
            var methodParams = possibleMethod.GetParameters();

            bool NullOrEmpty<T>(IEnumerable<T>? items) => items is null || !items.Any();

            if (NullOrEmpty(methodParams) || methodParams.Length != (argValues?.Length ?? 0))
            {
                return invalidResult;
            }

            bool IsNumberParam(string value)
            {
                return value.All(c => Numbers.Contains(c));
            }

            for (var i = 0; i < methodParams.Length; i++)
            {
                var methodParamType = methodParams[i].ParameterType == typeof(string)
                    ? "string"
                    : "number";
                var funcParamType = IsNumberParam(argValues?[i] ?? string.Empty)
                    ? "number"
                    : "string";

                if (methodParamType != funcParamType)
                {
                    matchFound = false;
                    returnMsg = $"No function with the parameter type of '{funcParamType}' found at parameter position '{i + 1}'.";
                }
            }

            methodFound = matchFound;

            if (!methodFound)
            {
                continue;
            }

            foundMethod = possibleMethod;

            // If this point is reached, then the method was found.  Reset the msg back to empty and leave loop.
            returnMsg = string.Empty;
            break;
        }

        return (methodFound, returnMsg, foundMethod);
    }
}
