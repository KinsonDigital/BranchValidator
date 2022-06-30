// <copyright file="CSharpMethodService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class CSharpMethodService : ICSharpMethodService
{
    private Assembly? cachedAssembly;

    /// <inheritdoc/>
    public IEnumerable<string> GetMethodNames(string className)
    {
        if (string.IsNullOrEmpty(className))
        {
            throw new ArgumentNullException(nameof(className), "The parameter must not be null or empty.");
        }

        return GetMethods(className).Select(m => m.Name).ToArray();
    }

    public Dictionary<string, Type[]> GetMethodParamTypes(string className, string methodName)
    {
        if (string.IsNullOrEmpty(className))
        {
            throw new ArgumentNullException(nameof(className), "The parameter must not be null or empty.");
        }

        if (string.IsNullOrEmpty(methodName))
        {
            throw new ArgumentNullException(nameof(methodName), "The parameter must not be null or empty.");
        }

        var possibleMethods = GetMethods(className).ToArray();

        if (possibleMethods is null)
        {
            throw new InvalidOperationException($"The method '{methodName}' for class '{className}' does not exist.");
        }

        var result = new Dictionary<string, Type[]>();

        for (var i = 0; i < possibleMethods.Length; i++)
        {
            var method = possibleMethods[i];
            var parameters = method.GetParameters();

            // The ':i + 1' section of the key is there to keep the dictionary keys unique for overloaded methods/expression functions
            result.Add($"{method.Name}:{i + 1}", parameters.Select(parameter => parameter.ParameterType).ToArray());
        }

        return result;
    }

    public IEnumerable<string> GetMethodSignatures(string className)
    {
        var result = new List<string>();
        var methods = GetMethods(className).ToArray();

        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            var paramSignatureBuilder = new StringBuilder();

            foreach (var paramInfo in parameters)
            {
                paramSignatureBuilder.Append($"{paramInfo.ParameterType} {paramInfo.Name}, ");
            }

            var paramSignature = paramSignatureBuilder.ToString().TrimEnd().TrimEnd(',');

            result.Add($"{method.ReturnType} {method.Name}({paramSignature})");
        }

        return result;
    }

    private IEnumerable<MethodInfo> GetMethods(string className)
    {
        var assembly = GetAssembly();

        var methodClass = assembly.GetTypes().FirstOrDefault(t => t.Name == className);

        if (methodClass is null)
        {
            throw new InvalidOperationException($"The '{nameof(CSharpMethodService)}' could not find the class name '{className}'.");
        }

        bool ContainExpressionMethodAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes()
                .Where(a => a.GetType() == typeof(ExpressionFunctionAttribute)).ToArray().Length > 0;
        }

        return methodClass.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(ContainExpressionMethodAttribute)
            .Select(m => m).ToArray();
    }

    // TODO: Create code docs for rest of this class
    private Assembly GetAssembly()
    {
        if (this.cachedAssembly is null)
        {
            var foundAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.FullName != null && a.FullName.StartsWith($"{nameof(BranchValidator)}, Version"));

            this.cachedAssembly = foundAssembly ?? throw new InvalidOperationException($"The assembly '{nameof(BranchValidator)}' was not found.");
        }

        return this.cachedAssembly;
    }
}
