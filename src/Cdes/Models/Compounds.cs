// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using System.Text.Json.Serialization;

namespace Cdes.Models;

/// <summary>
/// A cannabinoid compound with its measured value.
/// Used in COA/LabResult arrays to represent analyzed cannabinoid compounds.
/// </summary>
public sealed record Cannabinoid
{
    /// <summary>Canonical compound name (e.g. "THC", "CBD").</summary>
    public required string Name { get; init; }

    /// <summary>Human-readable display name.</summary>
    public string? DisplayName { get; init; }

    /// <summary>Measured percentage / quantity.</summary>
    public required double Percentage { get; init; }

    /// <summary>Measurement unit. Defaults to <see cref="CompoundUnit.Percent"/>.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CompoundUnit Unit { get; init; } = CompoundUnit.Percent;
}

/// <summary>
/// A terpene compound with its measured value.
/// </summary>
public sealed record Terpene
{
    /// <summary>Canonical compound name (e.g. "β-Myrcene").</summary>
    public required string Name { get; init; }

    /// <summary>Human-readable display name (e.g. "Myrcene").</summary>
    public string? DisplayName { get; init; }

    /// <summary>Measured percentage / quantity.</summary>
    public required double Percentage { get; init; }

    /// <summary>Measurement unit. Defaults to <see cref="CompoundUnit.Percent"/>.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CompoundUnit Unit { get; init; } = CompoundUnit.Percent;
}

/// <summary>
/// Weight with unit.
/// </summary>
public sealed record Weight
{
    /// <summary>Numeric value.</summary>
    public required double Value { get; init; }

    /// <summary>Unit string (e.g. "g", "oz", "mg").</summary>
    public required string Unit { get; init; }
}

/// <summary>
/// Price with currency.
/// </summary>
public sealed record Price
{
    /// <summary>Monetary amount.</summary>
    public required decimal Amount { get; init; }

    /// <summary>ISO 4217 currency code (e.g. "USD").</summary>
    public string Currency { get; init; } = "USD";
}
