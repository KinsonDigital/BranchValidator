// <copyright file="MutationFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using ScriptGenerator.Services;
using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Factories;

/// <summary>
/// Creates a list of various mutations.
/// </summary>
public static class MutationFactory
{
    private static IStringMutation? staticMethodMutation;
    private static IStringMutation? thisRefToStaticRefMutation;
    private static IStringMutation? removeInheritCodeDocMutation;
    private static IStringMutation? removeExpressionAttributeMutation;

    /// <summary>
    /// Creates a list of mutations that can be performed on a string.
    /// </summary>
    /// <returns>The list of string mutations.</returns>
    /// <remarks>
    /// List of mutations returned:
    ///     <list type="bullet">
    ///         <item><see cref="StaticMethodMutation"/></item>
    ///         <item><see cref="ThisRefToStaticRefMutation"/></item>
    ///         <item><see cref="RemoveInheritCodeDocsMutation"/></item>
    ///         <item><see cref="RemoveExpressionAttributeMutation"/></item>
    ///     </list>
    /// </remarks>
    public static IStringMutation[] CreateMutations()
    {
        var result = new List<IStringMutation>();

        staticMethodMutation ??= new StaticMethodMutation();
        thisRefToStaticRefMutation ??= new ThisRefToStaticRefMutation();
        removeInheritCodeDocMutation ??= new RemoveInheritCodeDocsMutation();
        removeExpressionAttributeMutation ??= new RemoveExpressionAttributeMutation();

        result.Add(staticMethodMutation);
        result.Add(thisRefToStaticRefMutation);
        result.Add(removeInheritCodeDocMutation);
        result.Add(removeExpressionAttributeMutation);

        return result.ToArray();
    }
}
