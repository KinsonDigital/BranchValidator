// <copyright file="IFunctionService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace BranchValidator.Services;

/// <summary>
/// Executes specialized functions from an expression.
/// </summary>
public interface IFunctionService
{
    /// <summary>
    /// Gets the list of available functions for use.
    /// </summary>
    ReadOnlyCollection<string> AvailableFunctions { get; }

    /// <summary>
    /// Gets the total number of parameters for a function that matches the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <returns>The number of parameters of the function.</returns>
    uint GetTotalFunctionParams(string name);

    /// <summary>
    /// Gets the data type for a parameter in the given <paramref name="paramPos"/> for a function with the given <paramref name="functionName"/>.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="paramPos">The parameter position.</param>
    /// <returns>The data type of the parameter.</returns>
    DataTypes GetFunctionParamDataType(string functionName, uint paramPos);

    /// <summary>
    /// Executes a function that matches the given <paramref name="functionName"/> and uses the given <paramref name="args"/>.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="args">The list of argument values to send into the function.</param>
    /// <returns>The function execution result.</returns>
    (bool valid, string msg) Execute(string functionName, params string[] args);

    /// <summary>
    /// Returns a value indicating whether or not a branch with the given <paramref name="branchName"/>
    /// matches the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to check against the <paramref name="branchName"/>.</param>
    /// <param name="branchName">The name of the branch.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> is equal to the <paramref name="branchName"/>.</returns>
    /// <remarks>
    ///     The comparison is case sensitive.
    /// </remarks>
    bool EqualTo(string value, string branchName);

    /// <summary>
    /// Returns a value indicating whether or not the <c>char</c> at the given <paramref name="charPos"/>
    /// is a number in a branch <c>string</c> that matches the given <paramref name="branchName"/>.
    /// </summary>
    /// <param name="charPos">The position of the character.</param>
    /// <param name="branchName">The name of the branch.</param>
    /// <returns><c>true</c> if the character is a number.</returns>
    /// <remarks>
    /// Things to consider:
    /// <list type="bullet">
    ///     <item>
    ///         If the character position is larger than the length of the <paramref name="branchName"/>,
    ///         then the result will automatically be <c>false</c>.
    ///     </item>
    ///     <item>
    ///         A null or empty branch name will return <c>false.</c>
    ///     </item>
    /// </list>
    /// </remarks>
    bool IsCharNum(uint charPos, string branchName);
}
