// <copyright file="ThisRefToStaticRefMutation.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Services;

/// <summary>
/// Replaces all instances of 'this.branchName' with 'BranchName' in a <c>string</c> value.
/// </summary>
public class ThisRefToStaticRefMutation : IStringMutation
{
    /// <inheritdoc/>
    public string Mutate(string value) => value.Replace("this.branchName", "BranchName");
}
