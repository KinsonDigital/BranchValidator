// <copyright file="FunctionService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class FunctionService : IFunctionService
{
    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };

    // TODO: Possibly load this data as JSON data from an embedded JSON data file
    private readonly Dictionary<string, DataTypes> validFunctions = new ()
    {
        { "equalTo:1", DataTypes.String },
        { "isCharNum:1", DataTypes.Number },
    };
    private readonly IMethodExecutor methodExecutor;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionService"/> class.
    /// </summary>
    /// <param name="methodExecutor">Executes methods on an object using reflection.</param>
    public FunctionService(IMethodExecutor methodExecutor)
    {
        this.methodExecutor = methodExecutor;

        // No empty or null keys aloud
        if (this.validFunctions.Keys.Any(string.IsNullOrEmpty))
        {
            throw new Exception("Valid function data set key cannot be null or empty.");
        }

        // Check that a colon exists in every single key of the func list
        if (this.validFunctions.Keys.Any(k => k.DoesNotContain(':')))
        {
            throw new Exception("Valid function data set key missing a ':' character.");
        }

        if (this.validFunctions.Keys.Any(k =>
            {
                var sections = k.Split(':', StringSplitOptions.RemoveEmptyEntries);

                if (sections.Length <= 1 || sections[1].Any(c => Numbers.DoesNotContain(c)))
                {
                    return true;
                }

                return false;
            }))
        {
            throw new Exception("Valid function data set key does not end with a number.");
        }

        AvailableFunctions = this.validFunctions.Select(f =>
        {
            var sections = f.Key.Split(':');

            return sections[0];
        }).ToReadOnlyCollection();
    }

    /// <inheritdoc/>
    public ReadOnlyCollection<string> AvailableFunctions { get; }

    /// <inheritdoc/>
    public uint GetTotalFunctionParams(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return 0;
        }

        return (uint)(from f in this.validFunctions
            where f.Key.StartsWith(name)
            select f).Count();
    }

    /// <inheritdoc/>
    public DataTypes GetFunctionParamDataType(string functionName, uint paramPos)
    {
        if (string.IsNullOrEmpty(functionName))
        {
            throw new ArgumentNullException(nameof(functionName), "The parameter must not be null or empty.");
        }

        var fullFuncName = $"{functionName}:{paramPos}";

        var foundFunction = (from f in this.validFunctions
            where f.Key == fullFuncName
            select f).ToArray();

        if (foundFunction is null || foundFunction.Length <= 0)
        {
            throw new Exception($"The function '{functionName}' was not found.");
        }

        return foundFunction[0].Value;
    }

    /// <inheritdoc/>
    public (bool valid, string msg) Execute(string functionName, params string[] argValues)
    {
        var methodResult = this.methodExecutor.ExecuteMethod(this, functionName.ToPascalCase(), argValues);
        return methodResult;
    }

    /// <inheritdoc/>
    public bool EqualTo(string value, string branchName)
    {
        if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(branchName))
        {
            return true;
        }

        return value == branchName;
    }

    /// <inheritdoc/>
    public bool IsCharNum(uint charPos, string branchName)
    {
        if (string.IsNullOrEmpty(branchName))
        {
            return false;
        }

        return charPos <= branchName.Length - 1 && Numbers.Contains(branchName[(int)charPos]);
    }
}
