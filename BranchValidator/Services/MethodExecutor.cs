// <copyright file="MethodExecutor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class MethodExecutor : IMethodExecutor
{
    private const char SingleQuote = '\'';
    private const char DoubleQuote = '"';
    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };

    /// <inheritdoc/>
    public (bool result, string msg) ExecuteMethod(object obj, string name, string[] argValues)
    {
        name = name.ToPascalCase();
        var methodExistsResult = obj.ContainsMethod(name, typeof(bool));

        if (methodExistsResult.exists is false)
        {
            return methodExistsResult;
        }

        var methodContainsParams = obj.MethodContainsParams(name, typeof(bool), argValues);

        if (methodContainsParams.result is false)
        {
            return methodContainsParams;
        }

        bool IsStringParam(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var hasSingleQuotes = value.StartsWith(SingleQuote) && value.EndsWith(SingleQuote) && value.DoesNotContain(DoubleQuote);
            var hasDoubleQuotes = value.StartsWith(DoubleQuote) && value.EndsWith(DoubleQuote) && value.DoesNotContain(SingleQuote);

            return hasSingleQuotes || hasDoubleQuotes;
        }

        bool IsNumParam(string value)
        {
            return value.DoesNotContain(SingleQuote) &&
                   value.DoesNotContain(DoubleQuote) &&
                   value.All(c => Numbers.Contains(c));
        }

        var foundMethod = (from m in obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
            where m.Name == name && m.ReturnType == typeof(bool) && m.GetParameters().Length == argValues.Length
            select m).ToArray()[0];

        var methodParams = foundMethod.GetParameters();

        var methodArgValues = new List<object>();

        for (var i = 0; i < argValues.Length; i++)
        {
            var parameter = argValues[i];

            // If the param is a number
            if (!IsStringParam(parameter) && IsNumParam(parameter))
            {
                bool parseSuccess;
                var paramType = methodParams[i].ParameterType;

                var intValue = -1;
                uint uintValue = 0;

                if (paramType == typeof(int))
                {
                    parseSuccess = int.TryParse(parameter, out intValue);
                }
                else if (paramType == typeof(uint))
                {
                    parseSuccess = uint.TryParse(parameter, out uintValue);
                }
                else
                {
                    var paramName = methodParams[i].Name;

                    var exceptionMsg = $"Could not convert value to '{paramType}' for parameter '{paramName}'.";
                    exceptionMsg += $"{Environment.NewLine}Can only use 'int' or 'uint' for function '{foundMethod.Name}'.";

                    throw new Exception(exceptionMsg);
                }

                if (parseSuccess is false)
                {
                    throw new Exception($"Issues parsing the '{i + 1}' parameter for the '{name}' method..");
                }

                methodArgValues.Add(paramType == typeof(int) ? intValue : uintValue);
            }
            else
            {
                // Is a string parameter
                methodArgValues.Add(parameter.Trim('\'').Trim('"'));
            }
        }

        var methodResult = (bool)(foundMethod.Invoke(obj, methodArgValues.ToArray()) ?? false);

        return (methodResult, string.Empty);
    }
}
