// <copyright file="NullOrEmptyStringArgumentException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Exceptions;

/// <summary>
/// Occurs when a<c>string</c>is null or empty.
/// </summary>
public class NullOrEmptyStringArgumentException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NullOrEmptyStringArgumentException"/> class.
    /// </summary>
    public NullOrEmptyStringArgumentException()
        : base("The string argument must not be null or empty.") => HResult = 50;

    /// <summary>
    /// Initializes a new instance of the <see cref="NullOrEmptyStringArgumentException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NullOrEmptyStringArgumentException(string message)
        : base(message) => HResult = 50;

    /// <summary>
    /// Initializes a new instance of the <see cref="NullOrEmptyStringArgumentException"/> class.
    /// </summary>
    /// <param name="paramName">The name of the parameter.</param>
    /// <param name="message">The message that describes the error.</param>
    public NullOrEmptyStringArgumentException(string paramName, string message)
        : base($"{message} (Parameter '{paramName}')") => HResult = 50;

    /// <summary>
    /// Initializes a new instance of the <see cref="NullOrEmptyStringArgumentException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public NullOrEmptyStringArgumentException(string message, Exception innerException)
        : base(message, innerException) => HResult = 50;
}
