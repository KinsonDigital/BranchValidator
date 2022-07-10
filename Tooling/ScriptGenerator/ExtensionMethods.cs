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

    /// <summary>
    /// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/> and
    /// handles an error when no service of type <typeparamref name="T"/> exists.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service object from.</param>
    /// <returns>A service object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="T"/>.</exception>
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

    /// <summary>
    /// Returns a value indicating whether or not the first occurence of the given <paramref name="beforeChar"/>
    /// is located positionally before the given <paramref name="afterChar"/>.
    /// </summary>
    /// <param name="value">This <c>string</c> to search for the <paramref name="beforeChar"/> and <paramref name="afterChar"/>.</param>
    /// <param name="beforeChar">The <c>char</c> that may or may not be located before the <paramref name="afterChar"/>.</param>
    /// <param name="afterChar">The <c>char</c> that may or may not be located after the <paramref name="beforeChar"/>.</param>
    /// <returns><c>true</c> if the <paramref name="beforeChar"/> is located before the <paramref name="afterChar"/>.</returns>
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

    /// <summary>
    /// Returns a value indicating whether or not the first occurence of the given <paramref name="beforeStr"/>
    /// is located positionally before the given <paramref name="afterChar"/>.
    /// </summary>
    /// <param name="value">This <c>string</c> to search for the <paramref name="beforeStr"/> and <paramref name="afterChar"/>.</param>
    /// <param name="beforeStr">The <c>char</c> that may or may not be located before the <paramref name="afterChar"/>.</param>
    /// <param name="afterChar">The <c>char</c> that may or may not be located after the <paramref name="beforeStr"/>.</param>
    /// <returns><c>true</c> if the <paramref name="beforeStr"/> is located before the <paramref name="afterChar"/>.</returns>
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

    /// <summary>
    /// Returns a value indicating whether or not the first occurence of the given <paramref name="afterChar"/>
    /// is located positionally after the given <paramref name="beforeChar"/>.
    /// </summary>
    /// <param name="value">This <c>string</c> to search for the <paramref name="afterChar"/> and <paramref name="beforeChar"/>.</param>
    /// <param name="afterChar">The <c>char</c> that may or may not be located after the <paramref name="beforeChar"/>.</param>
    /// <param name="beforeChar">The <c>char</c> that may or may not be located before the <paramref name="afterChar"/>.</param>
    /// <returns><c>true</c> if the <paramref name="afterChar"/> is located after the <paramref name="beforeChar"/>.</returns>
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

    /// <summary>
    /// Returns a value indicating whether or not the first occurence of the given <paramref name="afterStr"/>
    /// is located positionally after the given <paramref name="beforeChar"/>.
    /// </summary>
    /// <param name="value">This <c>string</c> to search for the <paramref name="afterStr"/> and <paramref name="beforeChar"/>.</param>
    /// <param name="afterStr">The <c>char</c> that may or may not be located after the <paramref name="beforeChar"/>.</param>
    /// <param name="beforeChar">The <c>char</c> that may or may not be located before the <paramref name="afterStr"/>.</param>
    /// <returns><c>true</c> if the <paramref name="afterStr"/> is located after the <paramref name="beforeChar"/>.</returns>
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
