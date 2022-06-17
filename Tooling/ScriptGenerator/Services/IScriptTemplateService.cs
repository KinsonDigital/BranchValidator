// <copyright file="IScriptTemplateService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScriptGenerator.Services;

/// <summary>
/// Creates a script template for the purpose of execution.
/// </summary>
public interface IScriptTemplateService
{
    /// <summary>
    /// Creates a new script template.
    /// </summary>
    /// <returns>The template.</returns>
    string CreateTemplate();
}
