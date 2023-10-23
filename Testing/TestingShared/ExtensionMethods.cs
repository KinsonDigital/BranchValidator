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
    /// <exception cref="XunitException">
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
            throw new XunitException(noPropsAssertMsg);
        }

        var propNotFoundAssertMsg = $"Cannot get an attribute on the property '{propName}' if the property does not exist.";
        var foundProp = (from p in props
            where p.Name == propName
            select p).FirstOrDefault();

        if (foundProp is null)
        {
            throw new XunitException(propNotFoundAssertMsg);
        }

        var noAttrsAssertMsg = $"Cannot get an attribute when the property '{propName}' does not have any attributes.";
        var attrs = foundProp.GetCustomAttributes<T>().ToArray();

        if (attrs.Length <= 0)
        {
            throw new XunitException(noAttrsAssertMsg);
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
    /// <exception cref="XunitException">
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
            var exMsg = $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.ShortName)}' property";
            exMsg += " value is not correct for option '{shortNameExpected}'.";
            exMsg += $"\nExpected: {shortNameExpected}";
            exMsg += $"\nActual: {value.ShortName}";

            throw new XunitException(exMsg);
        }

        if (value.LongName != longNameExpected)
        {
            var exMsg = $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.LongName)}' property";
            exMsg += " value is not correct for option '{longNameExpected}'.";
            exMsg += $"\nExpected: {longNameExpected}";
            exMsg += $"\nActual: {value.LongName}";

            throw new XunitException(exMsg);
        }

        if (value.Required != requiredExpected)
        {
            var exMsg = $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.Required)}' property";
            exMsg += " value is not correct for option '{longNameExpected}'.";
            exMsg += $"\nExpected: {requiredExpected}";
            exMsg += $"\nActual: {value.Required}";

            throw new XunitException(exMsg);
        }

        if (value.Default.ToString() != defaultExpected.ToString())
        {
            var exMsg = $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.Default)}' property";
            exMsg += " value is not correct for option '{defaultExpected}'.";
            exMsg += $"\nExpected: {defaultExpected}";
            exMsg += $"\nActual: {value.Default}";

            throw new XunitException(exMsg);
        }

        if (value.HelpText != helpTextExpected)
        {
            var exMsg = $"The '{nameof(OptionAttribute)}.{nameof(OptionAttribute.HelpText)}' property";
            exMsg += " value is not correct for option '{longNameExpected}'.";
            exMsg += $"\nExpected: {helpTextExpected}";
            exMsg += $"\nActual: {value.HelpText}";

            throw new XunitException(exMsg);
        }
    }
}
