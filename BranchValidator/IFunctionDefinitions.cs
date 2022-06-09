// <copyright file="IFunctionDefinitions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator;

/// <summary>
/// The available functions that can be used in the GitHub action validation logic expression.
/// </summary>
public interface IFunctionDefinitions : IDisposable
{
    /// <summary>
    /// Returns a value indicating whether or not a branch with the given branch name
    /// matches the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to check against the branch name.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> is equal to the branch name.</returns>
    /// <remarks>
    ///     The comparison is case sensitive.
    /// </remarks>
    bool EqualTo(string value);

    /// <summary>
    /// Returns a value indicating whether or not the <c>char</c> at the given <paramref name="charPos"/>
    /// is a number in a branch <c>string</c> that matches the given branch name.
    /// </summary>
    /// <param name="charPos">The position of the character.</param>
    /// <returns><c>true</c> if the character is a number.</returns>
    /// <remarks>
    /// Things to consider:
    /// <list type="bullet">
    ///     <item>
    ///         If the character position is larger than the length of the branch name,
    ///         then the result will automatically be <c>false</c>.
    ///     </item>
    ///     <item>
    ///         A null or empty branch name will return <c>false.</c>
    ///     </item>
    /// </list>
    /// </remarks>
    bool IsCharNum(uint charPos);

    /// <summary>
    ///
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    /// <remarks>
    /// TODO: Add info that if the endPos is greater then the len of the branch name,
    /// TODO: ↪ then the length of the branch name will be used
    /// </remarks>
    bool IsSectionNum(uint startPos, uint endPos);

    /// <summary>
    ///
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="upToChar"></param>
    /// <returns></returns>
    /// <remarks>
    /// TODO: Add info that if the startPos is greater then the branch name len, then its false
    /// TODO: Add info that only the first character of the upToChar string is used and the rest is ignored
    /// </remarks>
    bool IsSectionNum(uint startPos, string upToChar);
}
