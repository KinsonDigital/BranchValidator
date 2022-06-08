// <copyright file="ExpressionValidatorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using BranchValidator.Factories;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class ExpressionValidatorService : IExpressionValidatorService
{
    private readonly IAnalyzerFactory analyzerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionValidatorService"/> class.
    /// </summary>
    /// <param name="analyzerFactory">Creates analyzers.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="analyzerFactory"/> parameter is null.</exception>
    [ExcludeFromCodeCoverage]
    public ExpressionValidatorService(IAnalyzerFactory analyzerFactory)
        => this.analyzerFactory =
            analyzerFactory ?? throw new ArgumentNullException(nameof(analyzerFactory), "The parameter must not be null.");

    /// <inheritdoc/>
    public (bool isValid, string msg) Validate(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return (false, "The expression must not be null or empty.");
        }

        expression = expression.Trim();

        var analyzers = this.analyzerFactory.CreateAnalyzers();

        foreach (IAnalyzerService analyzer in analyzers)
        {
            var result = analyzer.Analyze(expression);

            if (result.valid is false)
            {
                return result;
            }
        }

        return (true, "expression valid");
    }
}
