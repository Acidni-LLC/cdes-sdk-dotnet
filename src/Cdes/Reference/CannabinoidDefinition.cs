// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

namespace Cdes.Reference;

/// <summary>
/// RGB color value for a cannabinoid.
/// </summary>
public readonly record struct CannabinoidRgb(byte R, byte G, byte B);

/// <summary>
/// Complete CDES cannabinoid definition with color, therapeutic use, and
/// expected percentage thresholds.
/// </summary>
public sealed record CannabinoidDefinition
{
    /// <summary>Canonical abbreviation (e.g. "THC", "CBD").</summary>
    public required string Name { get; init; }

    /// <summary>Hex color code.</summary>
    public required string Hex { get; init; }

    /// <summary>RGB color components.</summary>
    public required CannabinoidRgb Rgb { get; init; }

    /// <summary>Therapeutic description.</summary>
    public string? Therapeutic { get; init; }

    /// <summary>Minimum expected percentage (typical range).</summary>
    public double ThresholdMin { get; init; }

    /// <summary>Maximum expected percentage (typical range).</summary>
    public double ThresholdMax { get; init; }
}
