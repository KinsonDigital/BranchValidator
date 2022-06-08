// <copyright file="FunctionService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class FunctionService : IFunctionService
{
    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };
    private static readonly char[] LowerCaseLetters =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
    };

    // TODO: Possibly load this data as JSON data from an embedded JSON data file
    private readonly Dictionary<string, DataTypes[]> validFunctions = new ()
    {
        // NOTE: For no parameters, just leave the right side of the ':' empty.  Example: equalTo:
        { "equalTo:value", new[] { DataTypes.String } },
        { "isCharNum:charPos", new[] { DataTypes.Number } },
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

        // Check that the param names section after the function name is valid
        if (this.validFunctions.Keys.Any(k =>
            {
                var sections = k.Split(':', StringSplitOptions.RemoveEmptyEntries);

                // Commas aloud but param names only allowed to have lower and upper case letters
                return sections.Length <= 1 || sections[1].Any(c => c != ',' && LowerCaseLetters.DoesNotContain(c.ToString().ToLower()[0]));
            }))
        {
            throw new Exception("Valid function data set key does not end with a number.");
        }

        // Check if the param name count is equal to the param data list count
        foreach (KeyValuePair<string, DataTypes[]> validFunction in this.validFunctions)
        {
            var functionSections = validFunction.Key.Split(':', StringSplitOptions.RemoveEmptyEntries);
            var paramNames = functionSections.Length >= 2
                ? functionSections[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                : Array.Empty<string>();

            // If no parameters exist, move on
            if (paramNames.Length <= 0 || paramNames.Length == validFunction.Value.Length)
            {
                continue;
            }

            var exceptionMsg = $"The total number of parameter names vs the total number of parameter data types do not match for function '{functionSections[0]}'.";
            throw new Exception(exceptionMsg);
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

        var foundFunction = (from f in this.validFunctions
            where f.Key.StartsWith(functionName)
            select f).ToArray();

        if (foundFunction is null || foundFunction.Length <= 0)
        {
            throw new Exception($"The function '{functionName}' was not found.");
        }

        return foundFunction[0].Value[paramPos - 1];
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
