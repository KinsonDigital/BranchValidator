﻿// <copyright file="AssertExtensions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedMember.Global
namespace TestingShared
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Xunit;
    using Xunit.Sdk;

    /// <summary>
    /// Provides helper methods for the <see cref="Xunit"/>'s <see cref="Assert"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AssertExtensions : Assert
    {
        private const string TableFlip = "(╯'□')╯︵┻━┻  ";

        /// <summary>
        /// Fails the test with a message that the unit test is not implemented or needs rework.
        /// </summary>
        public static void TestNotImplemented() => Assert.True(false, "Test Not Implemented Or Needs Rework");

        /// <summary>
        /// Verifies that the exact exception type (not a derived exception type) is thrown and that
        /// the exception message matches the given <paramref name="expectedMessage"/>.
        /// </summary>
        /// <typeparam name="T">The type of exception that the test is verifying.</typeparam>
        /// <param name="testCode">The code that will be throwing the expected exception.</param>
        /// <param name="expectedMessage">The expected message of the exception.</param>
        public static void ThrowsWithMessage<T>(Action testCode, string expectedMessage)
            where T : Exception =>
            Equal(expectedMessage, Throws<T>(testCode).Message);

        /// <summary>
        /// Asserts that the given test code does not throw the exception of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of exception to check.</typeparam>
        /// <param name="testCode">The test code that should not throw the exception.</param>
        public static void DoesNotThrow<T>(Action testCode)
            where T : Exception
        {
            if (testCode is null)
            {
                Assert.True(false, $"{TableFlip}Cannot perform assertion with null '{testCode}' parameter.");
            }

            try
            {
                testCode();
            }
            catch (T)
            {
                Assert.True(false, $"{TableFlip}Did not expect the exception {typeof(T).Name} to be thrown.");
            }
        }

        /// <summary>
        /// Asserts that the given <paramref name="testCode"/> does not throw a null reference exception.
        /// </summary>
        /// <param name="testCode">The test that should not throw an exception.</param>
        public static void DoesNotThrowNullReference(Action testCode)
        {
            if (testCode is null)
            {
                Assert.True(false, $"{TableFlip}Cannot perform assertion with null '{testCode}' parameter.");
            }

            try
            {
                testCode();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(NullReferenceException))
                {
                    Assert.True(false, $"{TableFlip}Expected not to raise a {nameof(NullReferenceException)} exception.");
                }
                else
                {
                    Assert.True(true);
                }
            }
        }

        /// <summary>
        /// Asserts that all of the given <paramref name="items"/> are <c>true</c> which is dictated
        /// by the given <paramref name="arePredicate"/> predicate.
        /// </summary>
        /// <typeparam name="T">The type of item in the list of items.</typeparam>
        /// <param name="items">The list of items to assert.</param>
        /// <param name="arePredicate">Fails the assertion when returning <c>false</c>.</param>
        public static void AllItemsAre<T>(IEnumerable<T> items, Predicate<T> arePredicate)
        {
            if (arePredicate is null)
            {
                Assert.True(false, $"{TableFlip}Cannot perform assertion with null '{arePredicate}' parameter.");
            }

            var itemsToCheck = items.ToArray();

            for (var i = 0; i < itemsToCheck.Length; i++)
            {
                if (arePredicate(itemsToCheck[i]))
                {
                    continue;
                }

                Assert.True(false, $"{TableFlip}The item '{itemsToCheck[i]}' at index '{i}' returned false with the '{nameof(arePredicate)}'");
            }
        }

        /// <summary>
        /// Verifies that all of the given <paramref name="items"/> in the collection pass as long as the
        /// <paramref name="assertAction"/> does not contain an assertion failure.  If the <paramref name="includePredicate"/>
        /// returns true, the item is checked.
        /// </summary>
        /// <param name="items">The items to check.</param>
        /// <param name="includePredicate">Runs the <paramref name="assertAction"/> if the predicate returns true.</param>
        /// <param name="assertAction">
        ///     Contains the code to perform an assertion.  If the action code returns, then the assertion has passed.
        /// </param>
        /// <typeparam name="T">The data type of the items.</typeparam>
        public static void AllIncluded<T>(IEnumerable<T> items, Predicate<T> includePredicate, Action<T> assertAction)
        {
            if (includePredicate is null)
            {
                Assert.True(false, $"{TableFlip}Cannot perform assertion with null '{nameof(includePredicate)}' parameter.");
            }

            if (assertAction is null)
            {
                Assert.True(false, $"{TableFlip}Cannot perform assertion with null '{nameof(assertAction)}' parameter.");
            }

            var itemsToCheck = items.ToArray();

            foreach (var t in itemsToCheck)
            {
                if (includePredicate(t))
                {
                    assertAction(t);
                }
            }
        }

        /// <summary>
        /// Verifies that all items in the collection pass when executed against the given action.
        /// </summary>
        /// <typeparam name="T">The type of object to be verified.</typeparam>
        /// <param name="collection">The 2-dimensional collection.</param>
        /// <param name="width">The width of the first dimension.</param>
        /// <param name="height">The height of the second dimension.</param>
        /// <param name="action">The action to test each item against.</param>
        /// <remarks>
        ///     The last 2 <see langword="in"/> parameters T2 and T3 of type <see langword="int"/> of the <paramref name="action"/>
        ///     is the X and Y location within the <paramref name="collection"/> that failed the assertion.
        /// </remarks>
        public static void All<T>(T[,] collection, uint width, uint height, Action<T, int, int> action)
        {
            var actionInvoked = false;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    actionInvoked = true;
                    action(collection[x, y], x, y);
                }
            }

            var userMessage = TableFlip;
            userMessage += $"{TableFlip}No assertions were actually made in {nameof(AssertExtensions)}.{nameof(All)}<T>.";
            userMessage += "  Are there any items?";

            Assert.True(actionInvoked, userMessage);
        }

        /// <summary>
        /// Verifies that all items in the collection pass when executed against the given action.
        /// </summary>
        /// <typeparam name="T">The type of object to be verified.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action to test each item against.</param>
        public static void All<T>(T[] collection, Action<T, int> action)
        {
            var actionInvoked = false;

            for (var i = 0; i < collection.Length; i++)
            {
                actionInvoked = true;
                action(collection[i], i);
            }

            var userMessage = TableFlip;
            userMessage += $"No assertions were actually made in {nameof(AssertExtensions)}.{nameof(All)}<T>.";
            userMessage += "  Are there any items?";

            Assert.True(actionInvoked, userMessage);
        }

        /// <summary>
        /// Verifies that an object reference is not null and shows the given <paramref name="message"/> if the assertion fails.
        /// </summary>
        /// <param name="obj">The object to be validated.</param>
        /// <param name="message">The message to display if the assertion fails.</param>
        /// <exception cref="T:Xunit.Sdk.NotNullException">Thrown when the object is not null.</exception>
        public static void NotNullWithMessage(object obj, string message) => Assert.True(obj != null, message);

        /// <summary>
        /// Verifies that an expression is true for a member of a type.
        /// </summary>
        /// <param name="condition">The condition to be expected.</param>
        /// <param name="typeName">The name of the type that contains the member.</param>
        /// <param name="memberName">The name of the member that is not <c>true</c>.</param>
        public static void TypeMemberTrue(bool condition, string typeName, string memberName)
        {
            var message = $"{TableFlip}{typeName}.{memberName} not true.";
            Assert.True(condition, message);
        }

        /// <summary>
        /// Verifies that an expression is false for a member of a type.
        /// </summary>
        /// <param name="condition">The condition to be expected.</param>
        /// <param name="typeName">The name of the type that contains the member.</param>
        /// <param name="memberName">The name of the member that is not <c>false</c>.</param>
        public static void TypeMemberFalse(bool condition, string typeName, string memberName)
        {
            var message = $"{TableFlip}{typeName}.{memberName} not true.";
            Assert.False(condition, message);
        }

        /// <summary>
        /// Verifies that an event with the exact event args is not raised.
        /// </summary>
        /// <typeparam name="T">The type of the event arguments to expect.</typeparam>
        /// <param name="attach">Code to attach the event handler.</param>
        /// <param name="detach">Code to detach the event handler.</param>
        /// <param name="testCode">A delegate to the code to be tested.</param>
        public static void DoesNotRaise<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Action testCode)
            where T : EventArgs
        {
            try
            {
                Assert.Raises(attach, detach, testCode);
                Assert.Equal("No event was raised", "An event was raised.");
            }
            catch (Exception ex)
            {
                Assert.Equal("(No event was raised)\r\nEventArgs\r\n(No event was raised)", ex.Message);
            }
        }
    }
}
