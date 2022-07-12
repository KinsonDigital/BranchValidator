// <copyright file="IStringMutation.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScriptGenerator.Services.Interfaces;

/// <summary>
/// Mutates a <c>string</c> by performing a removal or addition to the <c>string</c> and returns the result.
/// </summary>
public interface IStringMutation
{
    /// <summary>
    /// Mutates the given <c>string</c> <paramref name="value"/> and returns the result.
    /// </summary>
    /// <param name="value">The <c>string</c> to mutate.</param>
    /// <returns>The mutated <c>string</c> <paramref name="value"/>.</returns>
    string Mutate(string value);
}
