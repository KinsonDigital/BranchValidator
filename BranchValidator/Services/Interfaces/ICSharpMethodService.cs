// <copyright file="ICSharpMethodService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidatorShared;

namespace BranchValidator.Services.Interfaces;

/// <summary>
/// Gets a list of method names from a class that are marked as valid expression functions.
/// </summary>
public interface ICSharpMethodService
{
    /// <summary>
    /// Returns a list of method names from a class that matches the given <paramref name="className"/>
    /// in the assembly that contains the <see cref="ExpressionFunctionAttribute"/> attribute.
    /// </summary>
    /// <param name="className">The name of the class.</param>
    /// <returns>The list of method names.</returns>
    IEnumerable<string> GetMethodNames(string className);

    /// <summary>
    /// Returns a <see cref="IDictionary{TKey,TValue}"/> of overloaded methods that match the given <paramref name="methodName"/>
    /// and the data types of each parameter of the method.
    /// </summary>
    /// <param name="className">The name of the <c>C#</c> class that contains the methods.</param>
    /// <param name="methodName">The name of the <c>C#</c> method.</param>
    /// <returns>
    ///     A list of overloaded methods with each method's list of parameter data types.
    /// </returns>
    /// <remarks>
    /// Things to note:
    ///     <list type="bullet">
    ///         <item>The <see cref="KeyValuePair{TKey,TValue}"/> key represents the name of an overloaded method.</item>
    ///         <item>The <see cref="KeyValuePair{TKey,TValue}"/> value is a list data types.  Each list item is a parameter.</item>
    ///         <item>The list of data types are in sequential order from left to right as they are laid out in the method signature.</item>
    ///     </list>
    /// </remarks>
    Dictionary<string, Type[]> GetMethodParamTypes(string className, string methodName);

    /// <summary>
    /// Returns a list of <c>string</c> values that represent all of the method signatures.
    /// </summary>
    /// <param name="className">The name of the class.</param>
    /// <returns>The list of method signatures.</returns>
    /// <remarks>
    ///     Each list item should be just like normal <c>C#</c> method signature line of code.
    /// </remarks>
    IEnumerable<string> GetMethodSignatures(string className);
}
