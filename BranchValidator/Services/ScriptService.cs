// <copyright file="ScriptService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using BranchValidator.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class ScriptService<T> : IScriptService<T>
{
    /// <inheritdoc/>
    public T Execute(string scriptSrc) => CSharpScript.EvaluateAsync<T>(scriptSrc).Result;
}
