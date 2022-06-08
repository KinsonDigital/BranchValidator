// <copyright file="IFunctionDefinitions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator;

/// <summary>
/// The available functions that can be used in the GitHub action validation logic expression.
/// </summary>
public interface IFunctionDefinitions
{
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
