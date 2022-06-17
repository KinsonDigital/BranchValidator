using System.Reflection;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

public class FunctionAnalyzerService : IAnalyzerService
{
    private readonly IFunctionNamesExtractorService funNamesExtractService;
    private readonly IMethodNamesService methodNamesService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionAnalyzerService"/> class.
    /// </summary>
    /// <param name="funNamesExtractService"></param>
    /// <param name="methodNamesService"></param>
    public FunctionAnalyzerService(
        IFunctionNamesExtractorService funNamesExtractService,
        IMethodNamesService methodNamesService)
    {
        // TODO: Unit test ctor params for null
        this.funNamesExtractService = funNamesExtractService;
        this.methodNamesService = methodNamesService;
    }


    /* TODO: Need to analyze the parameters.
        1. This means getting method param information to verify it matches.
        2. This also means verifying data type.
        3. This means that we check if no parameter exists as well.
    */

    public (bool valid, string msg) Analyze(string expression)
    {
        var methodNames = this.methodNamesService.GetMethodNames(nameof(FunctionDefinitions)).ToArray();

        // Lower case the first character of each method name to matching the casing of the expression function names before validation
        methodNames = methodNames.Select(name =>
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            var result = string.Empty;

            for (var i = 0; i < name.Length; i++)
            {
                result += i == 0 ? name[i].ToString().ToLower() : name[i];
            }

            return result;
        }).ToArray();

        var functionNames = this.funNamesExtractService.ExtractNames(expression);

        return (true, "result");
    }
}
