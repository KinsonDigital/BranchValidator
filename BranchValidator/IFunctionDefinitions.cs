// <copyright file="IFunctionDefinitions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedMemberInSuper.Global
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
    /// Returns a value indicating whether or not a section of a branch name is a whole number.
    /// The section is determined by the text between the <paramref name="startPos"/> and <paramref name="endPos"/>.
    /// </summary>
    /// <param name="startPos">The starting position of the section to check.</param>
    /// <param name="endPos">The ending position of the section to check.</param>
    /// <returns><c>true</c> if the section is a whole number.</returns>
    /// <remarks>
    /// Things to consider:
    /// <list type="bullet">
    ///     <item>
    ///         If the <paramref name="startPos"/> is larger than the length of the branch name, then the length of the branch name is used.
    ///     </item>
    ///     <item>
    ///         If the <paramref name="endPos"/> is larger than the length of the branch name, then the length of the branch name is used.
    ///     </item>
    /// </list>
    /// </remarks>
    bool IsSectionNum(uint startPos, uint endPos);

    /// <summary>
    /// Returns a value indicating whether or not a section of a branch name is a whole number.
    /// The section is determined by the text between the <paramref name="startPos"/> and the position of the first
    /// occurrence of the given <paramref name="upToChar"/>.
    /// </summary>
    /// <param name="startPos">The starting position of the section to check.</param>
    /// <param name="upToChar">The character to signify the ending position of the section.</param>
    /// <returns><c>true</c> if the section is a whole number.</returns>
    /// <remarks>
    ///     Things to consider:
    ///     <list type="bullet">
    ///         <item>
    ///             If the <paramref name="startPos"/> is larger than the length of the branch name, then the length of the branch name is used.
    ///         </item>
    ///         <item>
    ///             If the <paramref name="upToChar"/> does not exist anywhere after the <paramref name="startPos"/>, then the rest of the <c>string</c>
    ///             after the <paramref name="startPos"/> will be considered.
    ///         </item>
    ///         <item>
    ///             The <paramref name="upToChar"/> is exclusive in the <c>string</c> section to be checked.
    ///         </item>
    ///         <item>
    ///             The match against the <paramref name="upToChar"/> is vase sensitive if it is a letter.
    ///         </item>
    ///     </list>
    ///
    ///     Examples:
    ///     <list type="bullet">
    ///         <item>
    ///             Example 1:
    ///             <code>
    ///             // This would return true if the branch name was 'feature/123-test-branch'
    ///             IsCheckNum(8, "-");
    ///             </code>
    ///         </item>
    ///         <item>
    ///             Example 2:
    ///             <code>
    ///             // This would return true if the branch name was 'feature/12345'
    ///             IsCheckNum(8, "-");
    ///             </code>
    ///         </item>
    ///         <item>
    ///             Example 3:
    ///             <code>
    ///             // This would return false if the branch name was 'feature/123testbranch'
    ///             IsCheckNum(8, "-");
    ///             </code>
    ///         </item>
    ///     </list>
    /// </remarks>
    bool IsSectionNum(uint startPos, string upToChar);

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> is contained within a branch name.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <returns><c>true</c> if a branch name contains the <paramref name="value"/>.</returns>
    /// <remarks>
    ///     The search is case sensitive.
    /// </remarks>
    bool Contains(string value);

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> is contained within a branch name.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <returns><c>true</c> if a branch name does not contain the <paramref name="value"/>.</returns>
    /// <remarks>
    ///     The search is case sensitive.
    /// </remarks>
    bool NotContains(string value);

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> occurs within
    /// a branch name the given <paramref name="total"/> amount of times.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <param name="total">The total number of times that the <paramref name="value"/> should exist.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> exists the exact specified number of times.</returns>
    /// <remarks>
    ///     The search is case sensitive.
    /// </remarks>
    bool ExistTotal(string value, uint total);

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/> exists
    /// less than the given <paramref name="total"/> amount of times.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <param name="total">The total number of times to check that the <paramref name="value"/> exists.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> exists greater than the specified number of times.</returns>
    bool ExistsLessThan(string value, uint total);

    /// <summary>
    /// Returns a value indicating whether or not the given <paramref name="value"/>
    /// exists greater than the given <paramref name="total"/> number of times.
    /// </summary>
    /// <param name="value">The value to check for.</param>
    /// <param name="total">The total number of times to check that the <paramref name="value"/> exists.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> exists greater than the specified number of times.</returns>
    bool ExistsGreaterThan(string value, uint total);

    /// <summary>
    /// Returns a value indicating whether or not a branch name starts with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly starts with.</param>
    /// <returns><c>true</c> if the branch starts with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    bool StartsWith(string value);

    /// <summary>
    /// Returns a value indicating whether or not a branch name starts with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly starts with.</param>
    /// <returns><c>true</c> if the branch does not start with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    bool NotStartsWith(string value);

    /// <summary>
    /// Returns a value indicating whether or not a branch name ends with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly ends with.</param>
    /// <returns><c>true</c> if the branch does ends with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    bool EndsWith(string value);

    /// <summary>
    /// Returns a value indicating whether or not a branch name ends with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value that the branch possibly ends with.</param>
    /// <returns><c>true</c> if the branch does not end with the given <paramref name="value."/>.</returns>
    /// <remarks>
    ///     The match for the <paramref name="value"/> is case sensitive.
    /// </remarks>
    bool NotEndsWith(string value);

    /// <summary>
    /// Returns a value indicating whether or not the branch name starts with a number.
    /// </summary>
    /// <returns><c>true</c> if the branch starts with a number.</returns>
    bool StartsWithNum();
}
