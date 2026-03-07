// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using System.Collections.Frozen;

namespace Cdes.Reference;

/// <summary>
/// Authoritative CDES cannabinoid reference data.
/// 11 standard cannabinoids with CDES v1.3 deep jewel tone palette,
/// accessibility support, therapeutic uses, and percentage thresholds.
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
            Hex = "#B71C1C",
            Rgb = new CannabinoidRgb(183, 28, 28),
            Therapeutic = "Psychoactive, Pain Relief, Nausea",
            ThresholdMin = 0,
            ThresholdMax = 35,
            Pattern = "diagonal-right",
            Shape = "\u25C6",
            LegacyHex = "#E74C3C",
        },
        new()
        {
            Name = "CBD",
            Hex = "#1565C0",
            Rgb = new CannabinoidRgb(21, 101, 192),
            Therapeutic = "Anti-inflammatory, Anxiety, Seizures",
            ThresholdMin = 0,
            ThresholdMax = 25,
            Pattern = "horizontal-lines",
            Shape = "\u25A0",
            LegacyHex = "#3498DB",
        },
        new()
        {
            Name = "CBN",
            Hex = "#A0522D",
            Rgb = new CannabinoidRgb(160, 82, 45),
            Therapeutic = "Sedation, Sleep, Anti-inflammatory",
            ThresholdMin = 0,
            ThresholdMax = 5,
            Pattern = "crosshatch",
            Shape = "\u25CF",
            LegacyHex = "#E67E22",
        },
        new()
        {
            Name = "CBG",
            Hex = "#6A1B9A",
            Rgb = new CannabinoidRgb(106, 27, 154),
            Therapeutic = "Uplift, Focus, Appetite Stimulant",
            ThresholdMin = 0,
            ThresholdMax = 5,
            Pattern = "dots-coarse",
            Shape = "\u25B2",
            LegacyHex = "#9B59B6",
        },
        new()
        {
            Name = "CBC",
            Hex = "#00695C",
            Rgb = new CannabinoidRgb(0, 105, 92),
            Therapeutic = "Anti-inflammatory, Mood Support",
            ThresholdMin = 0,
            ThresholdMax = 1,
            Pattern = "dots-fine",
            Shape = "\u25BC",
            LegacyHex = "#16A085",
        },
        new()
        {
            Name = "THCV",
            Hex = "#C77C02",
            Rgb = new CannabinoidRgb(199, 124, 2),
            Therapeutic = "Energy, Appetite Suppression, Focus",
            ThresholdMin = 0,
            ThresholdMax = 3,
            Pattern = "zigzag",
            Shape = "\u2B21",
            LegacyHex = "#F39C12",
        },
        new()
        {
            Name = "CBDV",
            Hex = "#00796B",
            Rgb = new CannabinoidRgb(0, 121, 107),
            Therapeutic = "Seizure Support, Nausea",
            ThresholdMin = 0,
            ThresholdMax = 1,
            Pattern = "waves",
            Shape = "\u2B20",
            LegacyHex = "#1ABC9C",
        },
        new()
        {
            Name = "CBDA",
            Hex = "#9E7C1F",
            Rgb = new CannabinoidRgb(158, 124, 31),
            Therapeutic = "Raw Form, Anti-inflammatory",
            ThresholdMin = 0,
            ThresholdMax = 5,
            Pattern = "vertical-lines",
            Shape = "\u25A1",
            LegacyHex = "#D4AF37",
        },
        new()
        {
            Name = "THCA",
            Hex = "#880E0E",
            Rgb = new CannabinoidRgb(136, 14, 14),
            Therapeutic = "Raw Psychoactive, Anti-inflammatory",
            ThresholdMin = 0,
            ThresholdMax = 35,
            Pattern = "diagonal-left",
            Shape = "\u25C7",
            LegacyHex = "#C0392B",
        },
        new()
        {
            Name = "CBGA",
            Hex = "#4A148C",
            Rgb = new CannabinoidRgb(74, 20, 140),
            Therapeutic = "Raw CBG Precursor, Mother of All Cannabinoids",
            ThresholdMin = 0,
            ThresholdMax = 5,
            Pattern = "checkerboard",
            Shape = "\u25B3",
            LegacyHex = "#8E44AD",
        },
        new()
        {
            Name = "Delta-8 THC",
            Hex = "#C62828",
            Rgb = new CannabinoidRgb(198, 40, 40),
            Therapeutic = "Mild Psychoactive, Anti-nausea, Appetite",
            ThresholdMin = 0,
            ThresholdMax = 15,
            Pattern = "diagonal-dense",
            Shape = "\u25C7",
            LegacyHex = "#E67E22",
        },
    ];

    /// <summary>
    /// All 11 CDES standard cannabinoid definitions, keyed by canonical name.
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
    public const int MaxCannabinoids = 11;
}
