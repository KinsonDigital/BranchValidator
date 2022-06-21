// <copyright file="FunctionExtractorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class FunctionExtractorService : IFunctionExtractorService
{
    private const string AndOperator = "&&";
    private const string OrOperator = "||";
    private const char LeftParen = '(';

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionExtractorService"/> class.
    /// </summary>
    /// <param name="expressionValidatorService"></param>
    public FunctionExtractorService()
    {

    }

    /// <inheritdoc/>
    public IEnumerable<string> ExtractNames(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Array.Empty<string>();
        }

        const StringSplitOptions splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        var doesNotContainAnyOperators = expression.Contains(AndOperator) is false && expression.Contains(OrOperator) is false;

        // If no operators exist, then it is a single function expression
        if (doesNotContainAnyOperators)
        {
            return new[] { expression.Split(LeftParen, splitOptions)[0] };
        }

        var funcNames = new List<string>();

        var andSections = expression.Split(AndOperator, splitOptions);

        foreach (var section in andSections)
        {
            if (section.Contains(OrOperator))
            {
                var orSections = section.Split(OrOperator, splitOptions);

                foreach (var orSection in orSections)
                {
                    var sectionSplit = orSection.Split(LeftParen, splitOptions);

                    funcNames.Add(sectionSplit[0]);
                }
            }
            else
            {
                var sectionSplit = section.Split(LeftParen, splitOptions);

                funcNames.Add(sectionSplit[0]);
            }
        }

        return funcNames.ToArray();
    }
}
