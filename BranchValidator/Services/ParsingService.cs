using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class ParsingService : IParsingService
{
    private const char LeftParen = '(';
    private const char RightParen = ')';
    private const char Comma = ',';
    private const char Space = ' ';

    /// <inheritdoc/>
    public string ToExpressionFunctionSignature(string methodSignature)
    {
        methodSignature = methodSignature.Replace("System.Boolean", "bool");
        methodSignature = methodSignature.Replace("System.String", "string");
        methodSignature = methodSignature.Replace("System.UInt32", "number");

        const StringSplitOptions splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

        var signatureSections = methodSignature.Split(Space, splitOptions);
        var methodReturnType = signatureSections[0];
        var methodName = signatureSections[1].GetUpToChar(LeftParen);
        methodName = $"{methodName[0].ToLower()}{methodName[1..]}";

        var methodParamInfo = methodSignature.GetBetween(LeftParen, RightParen);
        var methodParamSections = methodParamInfo.Split(Comma, splitOptions);

        var funcParams = new List<string>();

        foreach (var paramSection in methodParamSections)
        {
            var sections = paramSection.Split(' ', splitOptions);

            var funcParamInfo = $"{sections[1]}: {sections[0]}";

            funcParams.Add(funcParamInfo);
        }

        var funcParamSignature = string.Join(',', funcParams).Replace(",", ", ");

        return $"{methodName}({funcParamSignature}): {methodReturnType}";
    }
}
