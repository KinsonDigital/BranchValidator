// <copyright file="AnalyzerFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Factories;

/// <inheritdoc/>
public static class AnalyzerFactory
{
    /// <inheritdoc/>
    public static ReadOnlyCollection<IAnalyzerService> CreateAnalyzers()
    {
        var result = new IAnalyzerService[]
        {
            // Program.AppServiceProvider.GetRequiredService<ParenAnalyzerService>(),
            // Program.AppServiceProvider.GetRequiredService<QuoteAnalyzerService>(),
            // Program.AppServiceProvider.GetRequiredService<OperatorAnalyzerService>(),
            // Program.AppServiceProvider.GetRequiredService<FunctionAnalyzerService>(),
        };

        return result.ToReadOnlyCollection();
    }
}
