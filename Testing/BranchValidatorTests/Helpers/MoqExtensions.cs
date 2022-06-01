// <copyright file="MoqExtensions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Moq;

namespace BranchValidatorTests.Helpers;

/// <summary>
/// Provides extensions to the <see cref="Moq"/> library for ease of use and readability purposes.
/// </summary>
[ExcludeFromCodeCoverage]
public static class MoqExtensions
{
    /// <summary>
    /// Verifies that a specific invocation matching the given expression was performed on the mock exactly one time.
    /// Use in conjunction with the default <see cref="MockBehavior.Loose"/>.
    /// </summary>
    /// <param name="mock">The mock object to extend.</param>
    /// <param name="expression">Expression to verify.</param>
    /// <typeparam name="T">Type to mock, which can be an interface, a class, or a delegate.</typeparam>
    /// <exception cref="MockException">
    ///   The invocation was called when it was not expected to be called.
    /// </exception>
    public static void VerifyOnce<T>(this Mock<T> mock, Expression<Action<T>> expression)
        where T : class
        => mock.Verify(expression, Times.Once);

    /// <summary>
    ///   Verifies that a specific invocation, matching the given expression, was performed on the mock.
    ///   Use in conjunction with the default <see cref="MockBehavior.Loose"/>.
    /// </summary>
    /// <param name="mock">The mock object to extend.</param>
    /// <param name="expression">Expression to verify.</param>
    /// <param name="times">The number of times a method is expected to be called.</param>
    /// <typeparam name="T">Type to mock, which can be an interface, a class, or a delegate.</typeparam>
    /// <exception cref="MockException">
    ///   The invocation was not called the number of times specified by <paramref name="times"/>.
    /// </exception>
    public static void VerifyOnce<T>(this Mock<T> mock, Expression<Action<T>> expression, Func<Times> times)
        where T : class
        => mock.Verify(expression, times);

    /// <summary>
    ///   Verifies that a property was set on the mock exactly one time.
    /// </summary>
    /// <param name="mock">The mock object to extend.</param>
    /// <param name="setterExpression">Expression to verify.</param>
    /// <typeparam name="T">Type to mock, which can be an interface, a class, or a delegate.</typeparam>
    /// <exception cref="MockException">
    ///   The invocation was not called exactly one time.
    /// </exception>
    public static void VerifySetOnce<T>(this Mock<T> mock, Action<T> setterExpression)
        where T : class
        => mock.VerifySet(setterExpression, Times.Once);

    /// <summary>
    /// Verifies that a specific invocation matching the given expression was never performed on the mock.
    /// Use in conjunction with the default <see cref="MockBehavior.Loose"/>.
    /// </summary>
    /// <param name="mock">The mock object to extend.</param>
    /// <param name="expression">Expression to verify.</param>
    /// <typeparam name="T">Type to mock, which can be an interface, a class, or a delegate.</typeparam>
    /// <exception cref="MockException">
    ///   The invocation was called when it was not expected to be called.
    /// </exception>
    public static void VerifyNever<T>(this Mock<T> mock, Expression<Action<T>> expression)
        where T : class
        => mock.Verify(expression, Times.Never);
}
