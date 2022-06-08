// <copyright file="AssertExtensions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;
// ReSharper disable ClassNeverInstantiated.Global
namespace BranchValidatorTests.Helpers;

/// <summary>
/// Provides helper methods for the <see cref="Xunit"/>'s <see cref="Assert"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
public class AssertExtensions : Assert
{
    private const string TableFlip = "(╯'□')╯︵┻━┻  ";

    /// <summary>
    /// Verifies if the <paramref name="expected"/> and <paramref name="actual"/> arguments are equal.
    /// </summary>
    /// <typeparam name="T">The <see cref="IEquatable{T}"/> type of the <paramref name="expected"/> and <paramref name="actual"/>.</typeparam>
    /// <param name="expected">The expected <see langword="int"/> value.</param>
    /// <param name="actual">The actual <see langword="int"/> value.</param>
    /// <param name="message">The message to be shown about the failed assertion.</param>
    public static void EqualWithMessage<T>(T? expected, T? actual, string message)
        where T : IEquatable<T>
    {
        try
        {
            Assert.True(expected.Equals(actual), string.IsNullOrEmpty(message) ? string.Empty : message);
        }
        catch (Exception)
        {
            var expectedStr = expected is null ? "NULL" : expected.ToString();
            var actualStr = actual is null ? "NULL" : actual.ToString();

            throw new AssertActualExpectedException(expectedStr, actualStr, $"{TableFlip}{message}");
        }
    }
}
