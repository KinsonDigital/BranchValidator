// <copyright file="StaticMethodMutation.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Services;

/// <summary>
/// Changes all lines of text in a <c>string</c> where the line is a public boolean method signature
/// to a public static bool method signature.
/// </summary>
public class StaticMethodMutation : IStringMutation
{
    /// <inheritdoc/>
    public string Mutate(string value) => value.Replace("public bool", "public static bool");
}
