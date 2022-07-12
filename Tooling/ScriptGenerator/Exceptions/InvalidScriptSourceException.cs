// <copyright file="InvalidScriptSourceException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScriptGenerator.Exceptions;

/// <summary>
/// Occurs when a file does not contain any data.
/// </summary>
public class InvalidScriptSourceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidScriptSourceException"/> class.
    /// </summary>
    public InvalidScriptSourceException()
        : base("Script Source Invalid.") => HResult = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidScriptSourceException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidScriptSourceException(string message)
        : base(message) => HResult = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidScriptSourceException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public InvalidScriptSourceException(string message, Exception innerException)
        : base(message, innerException) => HResult = 10;
}
