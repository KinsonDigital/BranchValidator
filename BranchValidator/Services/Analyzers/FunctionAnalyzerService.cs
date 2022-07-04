// <copyright file="FunctionAnalyzerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services.Interfaces;
using BranchValidatorShared;

namespace BranchValidator.Services.Analyzers;

/// <inheritdoc/>
public class FunctionAnalyzerService : IAnalyzerService
{
    private const char LeftParen = '(';
    private readonly ICSharpMethodService csharpMethodService;
    private readonly IFunctionExtractorService functionExtractorService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionAnalyzerService"/> class.
    /// </summary>
    /// <param name="functionExtractorService">Extracts functions from expressions.</param>
    /// <param name="csharpMethodService">Provides data about <c>C#</c> methods.</param>
    public FunctionAnalyzerService(
        IFunctionExtractorService functionExtractorService,
        ICSharpMethodService csharpMethodService)
    {
        EnsureThat.ParamIsNotNull(functionExtractorService);
        EnsureThat.ParamIsNotNull(csharpMethodService);

        this.functionExtractorService = functionExtractorService;
        this.csharpMethodService = csharpMethodService;
    }

    /// <inheritdoc/>
    public (bool valid, string msg) Analyze(string expression)
    {
        var allFunctionsExistResult = AllMethodsExist(expression);

        if (allFunctionsExistResult.valid is false)
        {
            return allFunctionsExistResult;
        }

        var functionSignatures = this.functionExtractorService.ExtractFunctions(expression);

        foreach (var functionSignature in functionSignatures)
        {
            var expressionFuncName = functionSignature.GetUpToChar(LeftParen);
            var csharpMethodName = $"{expressionFuncName[0].ToUpper()}{expressionFuncName[1..]}";
            var expressionFuncArgDataTypes
                = this.functionExtractorService.ExtractArgDataTypes(functionSignature).ToArray();

            // This dictionary can hold more then one method with some param data types.
            // This is due to the possibility of the CSharp method being overloaded which
            // means more than one method with different signatures could be found
            var methodsWithParamsDataTypes
                = this.csharpMethodService.GetMethodParamTypes(nameof(FunctionDefinitions), csharpMethodName).ToArray();

            var methodWithMatchingParamCount = methodsWithParamsDataTypes.Any(m =>
            {
                var methodName = m.Key.GetUpToChar(':');
                methodName = $"{methodName[0].ToLower()}{methodName[1..]}";

                return methodName == expressionFuncName && m.Value.Length == expressionFuncArgDataTypes.Length;
            });
            if (methodWithMatchingParamCount is false)
            {
                return (false, "The expression function is missing an argument.");
            }

            for (var i = 0; i < expressionFuncArgDataTypes.Length; i++)
            {
                var methodMatchFound = methodsWithParamsDataTypes.Any(m =>
                {
                    var matchFound = expressionFuncArgDataTypes.SequenceEqual(m.Value);

                    return matchFound;
                });

                if (methodMatchFound)
                {
                    continue;
                }

                return (false,
                    $"The value at argument position '{i + 1}' for the expression function '{expressionFuncName}' has an incorrect data type.");
            }
        }

        return (true, string.Empty);
    }

    /// <summary>
    /// Returns a result indicating if all of the <c>C#</c> equivalent methods for the expression
    /// function methods in the given <paramref name="expression"/> exist.
    /// </summary>
    /// <param name="expression">The expression to analyze.</param>
    /// <returns>
    ///     A result from analyzing the expression.
    ///     <para>
    ///         <c>Tuple.valid</c> = <c>true</c> if the methods exist in the <paramref name="expression"/>.
    ///     </para>
    ///     <para>
    ///         <c>Tuple.msg</c> = A message about the pass or failure.
    ///     </para>
    /// </returns>
    private (bool valid, string msg) AllMethodsExist(string expression)
    {
        var methodNames = this.csharpMethodService.GetMethodNames(nameof(FunctionDefinitions)).ToArray();

        // Lower case the first character of each method name to matching the casing of the expression function names before validation
        methodNames = methodNames.Select(name => $"{name[0].ToLower()}{name[1..]}").ToArray();

        var expressionFunNames = this.functionExtractorService.ExtractNames(expression);

        var nonExistingFunctions = expressionFunNames.Where(f => methodNames.DoesNotContain(f)).ToArray();

        return nonExistingFunctions.Length == 0
            ? (true, string.Empty)
            : (false, $"The expression function '{nonExistingFunctions[0]}' is not a usable function.");
    }
}
