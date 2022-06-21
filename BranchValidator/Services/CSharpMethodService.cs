// <copyright file="CSharpMethodService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class CSharpMethodService : ICSharpMethodService
{
    /// <inheritdoc/>
    public IEnumerable<string> GetMethodNames(string className)
    {
        if (string.IsNullOrEmpty(className))
        {
            throw new ArgumentNullException(nameof(className), "The parameter must not be null or empty.");
        }

        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.FullName != null && a.FullName.StartsWith($"{nameof(BranchValidator)}, Version"));

        if (assembly is null)
        {
            throw new InvalidOperationException($"The assembly '{nameof(BranchValidator)}' was not found.");
        }

        var functionsClass = assembly.GetTypes().FirstOrDefault(t => t.Name == className);

        if (functionsClass is null)
        {
            throw new InvalidOperationException($"The '{nameof(CSharpMethodService)}' could not find the class name '{className}'.");
        }

        bool ContainExpressionFunctionAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes()
                .Where(a => a.GetType() == typeof(ExpressionFunctionAttribute)).ToArray().Length > 0;
        }

        var functionNames = functionsClass.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(ContainExpressionFunctionAttribute)
            .Select(m => m.Name).ToArray();

        return functionNames;
    }
}
