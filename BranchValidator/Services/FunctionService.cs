// <copyright file="FunctionService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
public class FunctionService : IFunctionService
{
    private const char FuncNameParamSeparator = ':';
    private const string FunctionDefFileName = "func-defs.json";
    private readonly Dictionary<string, DataTypes[]>? validFunctions;
    private readonly IMethodExecutor methodExecutor;
    private readonly IFunctionDefinitions functionDefinitions;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionService"/> class.
    /// </summary>
    /// <param name="jsonService">Serializes and deserializes JSON data.</param>
    /// <param name="resourceLoaderService">Loads resources.</param>
    /// <param name="methodExecutor">Executes methods on an object using reflection.</param>
    /// <param name="functionDefinitions">The available expression functions.</param>
    public FunctionService(
        IJSONService jsonService,
        IEmbeddedResourceLoaderService<string> resourceLoaderService,
        IMethodExecutor methodExecutor,
        IFunctionDefinitions functionDefinitions)
    {
        const char comma = ',';
        if (jsonService is null)
        {
            throw new ArgumentNullException(nameof(jsonService), "The parameter must not be null.");
        }

        if (resourceLoaderService is null)
        {
            throw new ArgumentNullException(nameof(resourceLoaderService), "The parameter must not be null.");
        }

        this.methodExecutor = methodExecutor ?? throw new ArgumentNullException(nameof(methodExecutor), "The parameter must not be null.");
        this.functionDefinitions = functionDefinitions ?? throw new ArgumentNullException(nameof(functionDefinitions), "The parameter must not be null.");

        var rawFuncDefData = resourceLoaderService.LoadResource(FunctionDefFileName);
        this.validFunctions = jsonService.Deserialize<Dictionary<string, DataTypes[]>>(rawFuncDefData);

        if (this.validFunctions is null)
        {
            throw new InvalidOperationException("Loading of the function definition data was unsuccessful.");
        }

        // Create the list of function names
        FunctionNames = this.validFunctions.Select(f =>
        {
            var sections = f.Key.Split(FuncNameParamSeparator, StringSplitOptions.RemoveEmptyEntries);

            return sections[0];
        }).ToReadOnlyCollection();

        // Create all of the function signatures
        FunctionSignatures = this.validFunctions.Select(f =>
        {
            var functionSections = f.Key.Split(FuncNameParamSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var funcName = functionSections[0];
            var paramNames = functionSections.Length <= 1
                ? Array.Empty<string>()
                : functionSections[1].Split(comma, StringSplitOptions.TrimEntries);

            if (paramNames.Length <= 0)
            {
                return $"{funcName}()";
            }

            var paramStr = string.Empty;

            for (var i = 0; i < paramNames.Length; i++)
            {
                var paramName = paramNames[i];

                var endSection = i < paramNames.Length - 1 ? $"{comma} " : string.Empty;

                paramStr += $"{paramName}: {f.Value[i].ToString().ToLower()}{endSection}";
            }

            return $"{funcName}({paramStr.TrimEnd(comma)})";
        }).ToReadOnlyCollection();
    }

    /// <inheritdoc/>
    public ReadOnlyCollection<string> FunctionNames { get; }

    /// <inheritdoc/>
    public ReadOnlyCollection<string> FunctionSignatures { get; }

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
    public (bool valid, string msg) Execute(string functionName, params string[]? argValues)
    {
        var methodResult = this.methodExecutor.ExecuteMethod(this.functionDefinitions, functionName.ToPascalCase(), argValues);
        return methodResult;
    }
}
