// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

namespace Cdes.Reference;

/// <summary>
/// RGB color value.
/// </summary>
public readonly record struct RgbColor(byte R, byte G, byte B);

/// <summary>
/// HSL color value.
/// </summary>
public readonly record struct HslColor(int H, int S, int L);

/// <summary>
/// Terpene chemical classification.
/// </summary>
public enum TerpeneCategory
{
    Monoterpene,
    Sesquiterpene,
    Monoterpenoid,
    Sesquiterpenoid,
    Ester
}

/// <summary>
/// Complete CDES terpene definition with color, aroma, therapeutic effects,
/// and physical properties. 30 standard terpenes are defined.
/// </summary>
public sealed record TerpeneDefinition
{
    /// <summary>Numeric identifier (1-30).</summary>
    public required int Id { get; init; }

    /// <summary>Canonical chemical name (e.g. "β-Myrcene").</summary>
    public required string CanonicalName { get; init; }

    /// <summary>Simplified display name (e.g. "Myrcene").</summary>
    public required string DisplayName { get; init; }

    /// <summary>Chemical category.</summary>
    public required TerpeneCategory Category { get; init; }

    /// <summary>Display order for consistent visual rendering.</summary>
    public required int DisplayOrder { get; init; }

    /// <summary>Hex color code (e.g. "#FFFF00").</summary>
    public required string Hex { get; init; }

    /// <summary>RGB color components.</summary>
    public required RgbColor Rgb { get; init; }

    /// <summary>HSL color components.</summary>
    public HslColor? Hsl { get; init; }

    /// <summary>Aroma description.</summary>
    public string? Aroma { get; init; }

    /// <summary>Known therapeutic effects.</summary>
    public IReadOnlyList<string>? Therapeutic { get; init; }

    /// <summary>Boiling point in degrees Celsius.</summary>
    public int? BoilingPointC { get; init; }

    /// <summary>Alternative names used across lab reports.</summary>
    public required IReadOnlyList<string> Aliases { get; init; }
}
