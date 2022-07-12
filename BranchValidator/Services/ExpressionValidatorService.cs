// <copyright file="ExpressionValidatorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class ExpressionValidatorService : IExpressionValidatorService
{
    private readonly Dictionary<string, (bool isValid, string msg)> validationResults = new ();
    private readonly ReadOnlyCollection<IAnalyzerService> analyzers;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionValidatorService"/> class.
    /// </summary>
    /// <param name="analyzers">The analyzers to execute.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="analyzers"/> parameter is null.</exception>
    public ExpressionValidatorService(ReadOnlyCollection<IAnalyzerService> analyzers)
        => this.analyzers = analyzers ?? throw new ArgumentNullException(nameof(analyzers), "The parameter must not be null.");

    /// <inheritdoc/>
    public (bool isValid, string msg) Validate(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return (false, "The expression must not be null or empty.");
        }

        if (this.validationResults.ContainsKey(expression))
        {
            return this.validationResults[expression];
        }

        expression = expression.Trim();

        foreach (IAnalyzerService analyzer in this.analyzers)
        {
            var result = analyzer.Analyze(expression);

            if (result.valid)
            {
                continue;
            }

            this.validationResults.Add(expression, result);
            return result;
        }

        return (true, "expression valid");
    }
}
