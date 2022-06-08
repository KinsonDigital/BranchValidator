// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace BranchValidatorTests;

/// <summary>
/// Provides extensions to help with unit testing.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Converts the given array items of type <typeparamref name="T"/> to a
    /// <see cref="ReadOnlyCollection{T}"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="value">The list of items to convert.</param>
    /// <typeparam name="T">The type of items in the array and <see cref="ReadOnlyCollection{T}"/>.</typeparam>
    /// <returns>A <see cref="ReadOnlyCollection{T}"/> of the array items.</returns>
    public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> value) => new (value.ToArray());
}
