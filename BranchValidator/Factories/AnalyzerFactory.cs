// <copyright file="AnalyzerFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BranchValidator.Services;

namespace BranchValidator.Factories;

/// <inheritdoc/>
public class AnalyzerFactory : IAnalyzerFactory
{
    /// <inheritdoc/>
    public ReadOnlyCollection<IAnalyzerService> CreateAnalyzers()
    {
        var result = new IAnalyzerService[]
        {
            new ParenAnalyzerService(),
            new QuoteAnalyzerService(),
            new OperatorAnalyzerService(),
        };

        return result.ToReadOnlyCollection();
    }
}
