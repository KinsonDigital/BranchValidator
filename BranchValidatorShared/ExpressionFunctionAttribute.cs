// <copyright file="ExpressionFunctionAttribute.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidatorShared;

/// <summary>
/// Marks a method as an expression function that can be executed in an expression.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ExpressionFunctionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionFunctionAttribute"/> class.
    /// </summary>
    /// <param name="functionName">The name of the expression function.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="functionName"/> is null or empty.</exception>
    public ExpressionFunctionAttribute(string functionName)
    {
        if (string.IsNullOrEmpty(functionName))
        {
            throw new ArgumentNullException(nameof(functionName), "The parameter must not be null or empty.");
        }

        FunctionName = functionName;
    }

    /// <summary>
    /// Gets the name of the expression function that the method is for.
    /// </summary>
    public string FunctionName { get; }
}
