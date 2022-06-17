// <copyright file="JSONService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BranchValidator.Services.Interfaces;

namespace BranchValidator.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class JSONService : IJSONService
{
    private readonly JsonSerializerOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="JSONService"/> class.
    /// </summary>
    public JSONService()
    {
        this.options = new JsonSerializerOptions { WriteIndented = true };
        this.options.Converters.Add(new JsonStringEnumConverter());
    }

    /// <inheritdoc/>
    public string Serialize(object? value) => JsonSerializer.Serialize(value, this.options);

    /// <inheritdoc/>
    public T? Deserialize<T>(string value)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        return JsonSerializer.Deserialize<T>(stream, this.options);
    }
}
