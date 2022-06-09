// <copyright file="FunctionDefinitions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using BranchValidator.Observables.Core;

namespace BranchValidator;

/// <inheritdoc/>
[SuppressMessage("Requirements", "CA1822:Mark members as static", Justification = "Methods cannot be static due to reflection requirements and invocation.")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global", Justification = "Methods cannot be static due to reflection requirements and invocation.")]
public class FunctionDefinitions : IFunctionDefinitions
{
    private static readonly char[] Numbers =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    };
    private IDisposable? branchNameUnsubscriber;
    private string branchName = string.Empty;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionDefinitions"/> class.
    /// </summary>
    /// <param name="branchNameObservable">Receives push notifications of branch name updates.</param>
    public FunctionDefinitions(IBranchNameObservable branchNameObservable) =>
        this.branchNameUnsubscriber = branchNameObservable.Subscribe(new Observer<string>(
            onNext: branchNameValue =>
            {
                this.branchName = branchNameValue;
            }, onCompleted: () =>
            {
                this.branchNameUnsubscriber?.Dispose();
            }, onError: _ =>
            {
                this.branchNameUnsubscriber?.Dispose();
                this.branchNameUnsubscriber = null;
            }));

    /// <inheritdoc/>
    public bool EqualTo(string value)
    {
        if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(this.branchName))
        {
            return true;
        }

        return value == this.branchName;
    }

    /// <inheritdoc/>
    public bool IsCharNum(uint charPos)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        return charPos <= this.branchName.Length - 1 && Numbers.Contains(this.branchName[(int)charPos]);
    }

    /// <inheritdoc/>
    public bool IsSectionNum(uint startPos, uint endPos)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        endPos = endPos > this.branchName.Length - 1 ? (uint)this.branchName.Length - 1u : endPos;

        for (var i = startPos; i <= endPos; i++)
        {
            if (Numbers.DoesNotContain(this.branchName[(int)i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public bool IsSectionNum(uint startPos, string upToChar)
    {
        if (string.IsNullOrEmpty(this.branchName) || string.IsNullOrEmpty(upToChar))
        {
            return false;
        }

        if (startPos > this.branchName.Length - 1)
        {
            return false;
        }

        var upToCharIndex = this.branchName.IndexOf(upToChar[0], (int)startPos);

        if (upToCharIndex == -1)
        {
            return false;
        }

        var section = this.branchName.Substring((int)startPos, upToCharIndex - (int)startPos);

        return !string.IsNullOrEmpty(section) && section.All(c => Numbers.Contains(c));
    }

    /// <inheritdoc/>
    public bool Contains(string value) => this.branchName.Contains(value);

    /// <inheritdoc/>
    public bool NotContains(string value) => this.branchName.DoesNotContain(value);

    /// <inheritdoc/>
    public bool ExistTotal(string value, uint total) => this.branchName.Count(value) == total;

    /// <inheritdoc/>
    public bool ExistsLessThan(string value, uint total) => this.branchName.Count(value) < total;

    /// <inheritdoc/>
    public bool ExistsGreaterThan(string value, uint total) => this.branchName.Count(value) > total;

    /// <inheritdoc/>
    public bool StartsWith(string value) => this.branchName.StartsWith(value);

    /// <inheritdoc/>
    public bool NotStartsWith(string value) => !this.branchName.StartsWith(value);

    /// <inheritdoc/>
    public bool EndsWith(string value) => this.branchName.EndsWith(value);

    /// <inheritdoc/>
    public bool NotEndsWith(string value) => !this.branchName.EndsWith(value);

    /// <inheritdoc/>
    public bool StartsWithNum() => !string.IsNullOrEmpty(this.branchName) && Numbers.Contains(this.branchName[0]);

    /// <inheritdoc/>
    public bool EndsWithNum() => !string.IsNullOrEmpty(this.branchName) && Numbers.Contains(this.branchName[^1]);

    /// <inheritdoc/>
    public bool LenLessThan(uint length)
    {
        if (string.IsNullOrEmpty(this.branchName))
        {
            return false;
        }

        return this.branchName.Length < length;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        this.branchNameUnsubscriber?.Dispose();

        this.isDisposed = true;
    }
}
