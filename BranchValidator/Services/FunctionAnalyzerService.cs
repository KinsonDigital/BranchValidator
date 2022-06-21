using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

public class FunctionAnalyzerService : IAnalyzerService
{
    private readonly IFunctionNamesExtractorService funNamesExtractService;
    private readonly ICSharpMethodNamesService csharpMethodNamesService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionAnalyzerService"/> class.
    /// </summary>
    /// <param name="funNamesExtractService"></param>
    /// <param name="csharpMethodNamesService"></param>
    public FunctionAnalyzerService(
        IFunctionNamesExtractorService funNamesExtractService,
        ICSharpMethodNamesService csharpMethodNamesService)
    {
        // TODO: Unit test ctor params for null
        this.funNamesExtractService = funNamesExtractService;
        this.csharpMethodNamesService = csharpMethodNamesService;
    }


    /* TODO: Need to analyze the parameters.
        1. This means getting method arg information to verify it matches.
        2. This also means verifying data type.
        3. This means that we check if no parameter exists as well.
    */

    public (bool valid, string msg) Analyze(string expression)
    {
        var methodNames = this.csharpMethodNamesService.GetMethodNames(nameof(FunctionDefinitions)).ToArray();

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

        var expressionFunNames = this.funNamesExtractService.ExtractNames(expression);

        var nonExistingFunctions = expressionFunNames.Where(f => methodNames.DoesNotContain(f)).ToArray();

        return nonExistingFunctions.Length == 0
            ? (true, "All Functions Found")
            : (false, $"The expression function '{nonExistingFunctions[0]}' is not a usable function.");
    }
}
