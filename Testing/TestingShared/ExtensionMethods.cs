// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Reflection;
using CommandLine;
using Xunit.Sdk;

namespace TestingShared;

/// <summary>
/// Various helper methods to be used across the solution.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Gets an attribute of type <typeparamref name="T"/> on a property of the object
    /// that matches with the name <paramref name="propName"/>.
    /// </summary>
    /// <param name="value">The object that contains the property.</param>
    /// <param name="propName">The name of the property on the object.</param>
    /// <typeparam name="T">The type of attribute on the property.</typeparam>
    /// <returns>The existing attribute.</returns>
    /// <exception cref="AssertActualExpectedException">
    ///     Thrown if the property or attribute does not exist.
    /// </exception>
    public static T GetAttrFromProp<T>(this object value, string propName)
        where T : Attribute
    {
        var props = value.GetType().GetProperties();
        var noPropsAssertMsg = string.IsNullOrEmpty(propName)
            ? $"Cannot get an attribute on a property when the '{nameof(propName)}' parameter is null or empty."
            : $"Cannot get an attribute on a property when no property with the name '{propName}' exists.";

        if (props.Length <= 0)
        {
            throw new AssertActualExpectedException(
                "at least 1 item.",
                "was 0 items.",
                noPropsAssertMsg);
        }

        var propNotFoundAssertMsg = $"Cannot get an attribute on the property '{propName}' if the property does not exist.";
        var foundProp = (from p in props
            where p.Name == propName
            select p).FirstOrDefault();

        if (foundProp is null)
        {
            throw new AssertActualExpectedException(
                "not to be null.",
                "was null",
                propNotFoundAssertMsg);
        }

        var noAttrsAssertMsg = $"Cannot get an attribute when the property '{propName}' does not have any attributes.";
        var attrs = foundProp.GetCustomAttributes<T>().ToArray();

        if (attrs.Length <= 0)
        {
            throw new AssertActualExpectedException(
                "at least 1 item.",
                "was 0 items.",
                noAttrsAssertMsg);
        }

        return attrs[0];
    }

    /// <summary>
    /// Asserts the properties below.
    /// <list type="bullet">
    ///     <item><see cref="OptionAttribute"/>.<see cref="OptionAttribute.ShortName"/></item>
    ///     <item><see cref="OptionAttribute"/>.<see cref="OptionAttribute.LongName"/></item>
    ///     <item><see cref="OptionAttribute"/>.<see cref="BaseAttribute.Required"/></item>
    ///     <item><see cref="OptionAttribute"/>.<see cref="BaseAttribute.Default"/></item>
    ///     <item><see cref="OptionAttribute"/>.<see cref="BaseAttribute.HelpText"/></item>
    /// </list>
    /// </summary>
    /// <param name="value">The attribute to assert.</param>
    /// <param name="shortNameExpected">The expected value of the <see cref="OptionAttribute.ShortName"/> property.</param>
    /// <param name="longNameExpected">The expected value of the <see cref="OptionAttribute.LongName"/> property.</param>
    /// <param name="requiredExpected">The expected value of the <see cref="OptionAttribute.Required"/> property.</param>
    /// <param name="defaultExpected">The expected value of the <see cref="BaseAttribute.Default"/> property.</param>
    /// <param name="helpTextExpected">The expected value of the <see cref="OptionAttribute.HelpText"/> property.</param>
    /// <exception cref="AssertActualExpectedException">
    ///     Thrown if the properties do not have the correct values.
    /// </exception>
    public static void AssertOptionAttrProps(this OptionAttribute value,
        string shortNameExpected,
        string longNameExpected,
        bool requiredExpected,
        object defaultExpected,
        string helpTextExpected)
    {
        if (value.ShortName != shortNameExpected)
        {
            throw new AssertActualExpectedException(
                shortNameExpected,
                value.ShortName,
                $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.ShortName)}' property value is not correct for option '{shortNameExpected}'.");
        }

        if (value.LongName != longNameExpected)
        {
            throw new AssertActualExpectedException(
                longNameExpected,
                value.LongName,
                $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.LongName)}' property value is not correct for option '{longNameExpected}'.");
        }

        if (value.Required != requiredExpected)
        {
            throw new AssertActualExpectedException(
                requiredExpected,
                value.Required,
                $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.Required)}' property value is not correct for option '{longNameExpected}'.");
        }

        if (value.Default.ToString() != defaultExpected.ToString())
        {
            throw new AssertActualExpectedException(
                defaultExpected,
                value.Default,
                $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.Default)}' property value is not correct for option '{defaultExpected}'.");
        }

        if (value.HelpText != helpTextExpected)
        {
            throw new AssertActualExpectedException(
                helpTextExpected,
                value.HelpText,
                $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.HelpText)}' property value is not correct for option '{longNameExpected}'.");
        }
    }
}
