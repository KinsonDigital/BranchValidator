// <copyright file="ExpressionValidatorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/* TODO: Do the following
    1. Look at all of the other services that could use this service for validation first before processing and inject it

    Need to cache the validation result every time and expression is analyzed.
    This way the expression validation is more performant when used multiple times.

    This will allow this service to be used in all of the other services before doing expression
    work.  Since the expression validation result is cached, it will just returned the cached result
    and the main validation work will simply only work hard the first time.
 */

/// <inheritdoc/>
public class ExpressionValidatorService : IExpressionValidatorService
{
    // private readonly IAnalyzerFactory analyzerFactory;
    private readonly Dictionary<string, (bool isValid, string msg)> validationResults = new ();
    private readonly ReadOnlyCollection<IAnalyzerService> analyzers;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionValidatorService"/> class.
    /// </summary>
    /// <param name="analyzers">The analyzers to execute.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="analyzers"/> parameter is null.</exception>
    [ExcludeFromCodeCoverage]
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
