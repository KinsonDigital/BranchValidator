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
}
