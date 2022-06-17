namespace BranchValidator;

public class ExpressionFunctionAttribute : Attribute
{
    public ExpressionFunctionAttribute(string functionName)
    {
        if (string.IsNullOrEmpty(functionName))
        {
            throw new ArgumentNullException(nameof(functionName), "The parameter must not be null or empty.");
        }

        FunctionName = functionName;
    }

    public string FunctionName { get; } = string.Empty;
}
