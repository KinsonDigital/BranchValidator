// <copyright file="IAnalyzerFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Factories;

/// <summary>
/// Creates analyzers.
/// </summary>
public interface IAnalyzerFactory
{
    /// <summary>
    /// Creates a list of analyzers to analyze expressions.
    /// </summary>
    /// <returns>The list of analyzers.</returns>
    ReadOnlyCollection<IAnalyzerService> CreateAnalyzers();
}
