// <copyright file="ExtensionMethodsForTesting.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Reflection;
using CommandLine;
using Xunit.Sdk;

namespace BranchValidatorTests.Helpers;

/// <summary>
/// Provides extension/helper methods to assist in unit testing.
/// </summary>
public static class ExtensionMethodsForTesting
{


    /// <summary>
    /// Asserts the properties below.
    /// <list type="bullet">
    ///     <item><see cref="OptionAttribute"/>.<see cref="OptionAttribute.LongName"/></item>
    ///     <item><see cref="OptionAttribute"/>.<see cref="BaseAttribute.Required"/></item>
    ///     <item><see cref="OptionAttribute"/>.<see cref="BaseAttribute.HelpText"/></item>
    /// </list>
    /// </summary>
    /// <param name="value">The attribute to assert.</param>
    /// <param name="longNameExpected">The expected value of the <see cref="OptionAttribute.LongName"/> property.</param>
    /// <param name="requiredExpected">The expected value of the <see cref="OptionAttribute.Required"/> property.</param>
    /// <param name="helpTextExpected">The expected value of the <see cref="OptionAttribute.HelpText"/> property.</param>
    /// <exception cref="AssertActualExpectedException">
    ///     Thrown if the properties do not have the correct values.
    /// </exception>
    public static void AssertOptionAttrProps(this OptionAttribute value,
        string longNameExpected,
        bool requiredExpected,
        string helpTextExpected)
    {
        if (value.LongName != longNameExpected)
        {
            throw new AssertActualExpectedException(
                longNameExpected,
                value.LongName,
                $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.LongName)}' property value is not correct.");
        }

        if (value.Required != requiredExpected)
        {
            throw new AssertActualExpectedException(
                requiredExpected,
                value.Required,
                $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.Required)}' property value is not correct for option '{longNameExpected}'.");
        }

        if (value.HelpText != helpTextExpected)
        {
            throw new AssertActualExpectedException(
                helpTextExpected,
                value.HelpText,
                $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.HelpText)}' property value is not correct.");
        }
    }
}
