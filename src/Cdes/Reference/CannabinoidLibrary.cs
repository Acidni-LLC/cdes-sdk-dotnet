// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using System.Collections.Frozen;

namespace Cdes.Reference;

/// <summary>
/// Authoritative CDES cannabinoid reference data.
/// 9 standard cannabinoids with colors, therapeutic uses, and percentage thresholds.
/// </summary>
public static class CannabinoidLibrary
{
    /// <summary>
    /// Fallback hex color for unrecognized cannabinoids.
    /// </summary>
    public const string UnknownCannabinoidColor = "#808080";

    private static readonly CannabinoidDefinition[] s_definitions =
    [
        new()
        {
            Name = "THC",
            Hex = "#E74C3C",
            Rgb = new CannabinoidRgb(231, 76, 60),
            Therapeutic = "Psychoactive, Pain Relief, Nausea",
            ThresholdMin = 0,
            ThresholdMax = 35,
        },
        new()
        {
            Name = "CBD",
            Hex = "#3498DB",
            Rgb = new CannabinoidRgb(52, 152, 219),
            Therapeutic = "Anti-inflammatory, Anxiety, Seizures",
            ThresholdMin = 0,
            ThresholdMax = 25,
        },
        new()
        {
            Name = "CBN",
            Hex = "#E67E22",
            Rgb = new CannabinoidRgb(230, 126, 34),
            Therapeutic = "Sedation, Sleep, Anti-inflammatory",
            ThresholdMin = 0,
            ThresholdMax = 5,
        },
        new()
        {
            Name = "CBG",
            Hex = "#9B59B6",
            Rgb = new CannabinoidRgb(155, 89, 182),
            Therapeutic = "Uplift, Focus, Appetite Stimulant",
            ThresholdMin = 0,
            ThresholdMax = 5,
        },
        new()
        {
            Name = "CBC",
            Hex = "#16A085",
            Rgb = new CannabinoidRgb(22, 160, 133),
            Therapeutic = "Anti-inflammatory, Mood Support",
            ThresholdMin = 0,
            ThresholdMax = 1,
        },
        new()
        {
            Name = "THCV",
            Hex = "#F39C12",
            Rgb = new CannabinoidRgb(243, 156, 18),
            Therapeutic = "Energy, Appetite Suppression, Focus",
            ThresholdMin = 0,
            ThresholdMax = 3,
        },
        new()
        {
            Name = "CBDV",
            Hex = "#1ABC9C",
            Rgb = new CannabinoidRgb(26, 188, 156),
            Therapeutic = "Seizure Support, Nausea",
            ThresholdMin = 0,
            ThresholdMax = 1,
        },
        new()
        {
            Name = "CBDA",
            Hex = "#D4AF37",
            Rgb = new CannabinoidRgb(212, 175, 55),
            Therapeutic = "Raw Form, Anti-inflammatory",
            ThresholdMin = 0,
            ThresholdMax = 5,
        },
        new()
        {
            Name = "THCA",
            Hex = "#C0392B",
            Rgb = new CannabinoidRgb(192, 57, 43),
            Therapeutic = "Raw Psychoactive, Anti-inflammatory",
            ThresholdMin = 0,
            ThresholdMax = 35,
        },
    ];

    /// <summary>
    /// All 9 CDES standard cannabinoid definitions, keyed by canonical name.
    /// Thread-safe and optimized for lookup.
    /// </summary>
    public static FrozenDictionary<string, CannabinoidDefinition> StandardCannabinoids { get; } =
        s_definitions.ToFrozenDictionary(d => d.Name);

    /// <summary>
    /// All canonical cannabinoid names.
    /// </summary>
    public static IReadOnlyList<string> AllNames { get; } =
        s_definitions.Select(d => d.Name).ToArray();

    /// <summary>
    /// Hex color palette for all standard cannabinoids.
    /// </summary>
    public static IReadOnlyList<string> ColorPalette { get; } =
        s_definitions.Select(d => d.Hex).ToArray();

    /// <summary>
    /// Maximum number of recognized cannabinoids (for completeness scoring).
    /// </summary>
    public const int MaxCannabinoids = 9;
}
