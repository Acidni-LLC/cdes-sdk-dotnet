// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cdes.Serialization;

/// <summary>
/// Pre-configured <see cref="JsonSerializerOptions"/> for CDES-compliant JSON.
/// Uses camelCase naming, string enum values, and relaxed number handling.
/// </summary>
public static class CdesJsonOptions
{
    /// <summary>
    /// Default CDES serialization options.
    /// </summary>
    public static JsonSerializerOptions Default { get; } = CreateDefault();

    /// <summary>
    /// CDES serialization options with indentation for human-readable output.
    /// </summary>
    public static JsonSerializerOptions Indented { get; } = CreateIndented();

    private static JsonSerializerOptions CreateDefault()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            WriteIndented = false,
        };

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        return options;
    }

    private static JsonSerializerOptions CreateIndented()
    {
        var options = CreateDefault();
        options.WriteIndented = true;
        return options;
    }

    /// <summary>
    /// Serialize an object to CDES-format JSON.
    /// </summary>
    public static string Serialize<T>(T value) =>
        JsonSerializer.Serialize(value, Default);

    /// <summary>
    /// Serialize an object to indented CDES-format JSON.
    /// </summary>
    public static string SerializeIndented<T>(T value) =>
        JsonSerializer.Serialize(value, Indented);

    /// <summary>
    /// Deserialize CDES-format JSON to an object.
    /// </summary>
    public static T? Deserialize<T>(string json) =>
        JsonSerializer.Deserialize<T>(json, Default);

    /// <summary>
    /// Deserialize CDES-format JSON from a stream.
    /// </summary>
    public static ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken ct = default) =>
        JsonSerializer.DeserializeAsync<T>(stream, Default, ct);
}
