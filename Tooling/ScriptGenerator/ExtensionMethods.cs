// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace ScriptGenerator;

/// <summary>
/// Provides extensions to improve code.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Returns a value indicating whether or not the given <c>string</c> <paramref name="value"/>
    /// only contains the given characters.
    /// </summary>
    /// <param name="value">The <c>string</c> to check.</param>
    /// <param name="characters">The list of characters that are allowed in the given <paramref name="value"/>.</param>
    /// <returns><c>true</c> if the given <paramref name="value"/> only contains the given <paramref name="characters"/>.</returns>
    public static bool OnlyContains(this string value, IEnumerable<char> characters)
        => !string.IsNullOrEmpty(value) && value.All(characters.Contains);

    [ExcludeFromCodeCoverage]
    public static T GetRequiredServiceAndHandleError<T>(this IServiceProvider provider)
        where T : notnull
    {
        try
        {
            return provider.GetRequiredService<T>();
        }
        catch (InvalidOperationException e) when (e.Message.StartsWith("Unable to resolve service for type"))
        {
            var currentClr = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR > {e.Message}");
            Console.ForegroundColor = currentClr;
            Environment.Exit(e.HResult);
        }

        return default!;
    }

    public static bool IsBefore(this string value, char beforeChar, char afterChar)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        var beforeCharIndex = value.IndexOf(beforeChar);
        var afterCharIndex = value.IndexOf(afterChar);

        if (beforeCharIndex == -1)
        {
            return false;
        }

        if (afterCharIndex == -1)
        {
            return true;
        }

        return beforeCharIndex < afterCharIndex;
    }

    public static bool IsBefore(this string value, string beforeStr, char afterChar)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        var beforeStrIndex = value.IndexOf(beforeStr, StringComparison.Ordinal);

        if (beforeStrIndex == -1)
        {
            return false;
        }

        var afterCharIndex = value.IndexOf(afterChar);

        if (afterCharIndex == -1)
        {
            return true;
        }

        return beforeStrIndex < afterCharIndex;
    }

    public static bool IsAfter(this string value, char afterChar, char beforeChar)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        var beforeCharIndex = value.IndexOf(beforeChar);
        var afterCharIndex = value.IndexOf(afterChar);

        if (afterCharIndex == -1)
        {
            return false;
        }

        if (beforeCharIndex == -1)
        {
            return true;
        }

        return beforeCharIndex < afterCharIndex;
    }

    public static bool IsAfter(this string value, string afterStr, char beforeChar)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        var afterStrIndex = value.IndexOf(afterStr, StringComparison.Ordinal);

        if (afterStrIndex == -1)
        {
            return false;
        }

        var beforeCharIndex = value.IndexOf(beforeChar);

        if (beforeCharIndex == -1)
        {
            return true;
        }

        return beforeCharIndex < afterStrIndex;
    }
}
