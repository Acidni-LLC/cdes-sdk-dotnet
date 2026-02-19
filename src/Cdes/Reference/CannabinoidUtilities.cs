// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

namespace Cdes.Reference;

/// <summary>
/// Cannabinoid name normalization, color lookup, and categorization helpers.
/// </summary>
public static class CannabinoidUtilities
{
    /// <summary>
    /// Normalize a cannabinoid name for matching.
    /// Strips whitespace, hyphens, underscores, and percentage characters;
    /// returns upper-cased result capped at 5 characters.
    /// </summary>
    /// <param name="name">Raw cannabinoid name.</param>
    /// <returns>Normalized name suitable for matching.</returns>
    public static string NormalizeName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var cleaned = name
            .Trim()
            .ToUpperInvariant()
            .Replace(" ", "", StringComparison.Ordinal)
            .Replace("-", "", StringComparison.Ordinal)
            .Replace("_", "", StringComparison.Ordinal)
            .Replace("%", "", StringComparison.Ordinal);

        return cleaned.Length > 5 ? cleaned[..5] : cleaned;
    }

    /// <summary>
    /// Get the definition for a cannabinoid (fuzzy matching by normalized name).
    /// </summary>
    /// <param name="name">Raw cannabinoid name.</param>
    /// <returns>Definition if matched; otherwise <c>null</c>.</returns>
    public static CannabinoidDefinition? GetDefinition(string name)
    {
        var normalized = NormalizeName(name);

        foreach (var (key, def) in CannabinoidLibrary.StandardCannabinoids)
        {
            if (key.Contains(normalized, StringComparison.OrdinalIgnoreCase) ||
                normalized.Contains(key, StringComparison.OrdinalIgnoreCase))
            {
                return def;
            }
        }

        return null;
    }

    /// <summary>
    /// Get the hex color for a cannabinoid.
    /// </summary>
    /// <param name="name">Cannabinoid name.</param>
    /// <returns>Hex color or <c>null</c>.</returns>
    public static string? GetColor(string name) =>
        GetDefinition(name)?.Hex;

    /// <summary>
    /// Get the hex color for a cannabinoid with a fallback.
    /// </summary>
    /// <param name="name">Cannabinoid name.</param>
    /// <returns>Hex color (never <c>null</c>).</returns>
    public static string GetColorSafe(string name) =>
        GetColor(name) ?? CannabinoidLibrary.UnknownCannabinoidColor;

    /// <summary>
    /// Get the canonical display name for a cannabinoid.
    /// </summary>
    /// <param name="name">Raw name.</param>
    /// <returns>Display name if found; otherwise the original.</returns>
    public static string GetDisplayName(string name) =>
        GetDefinition(name)?.Name ?? name;

    /// <summary>
    /// Categorize a cannabinoid percentage level.
    /// </summary>
    /// <param name="name">Cannabinoid name.</param>
    /// <param name="percentage">Measured percentage.</param>
    /// <returns>Category label.</returns>
    public static string GetCategory(string name, double percentage)
    {
        var def = GetDefinition(name);
        if (def is null) return "low";

        var range = def.ThresholdMax - def.ThresholdMin;
        if (range <= 0) return "low";

        return percentage switch
        {
            _ when percentage <= def.ThresholdMin + range * 0.25 => "low",
            _ when percentage <= def.ThresholdMin + range * 0.50 => "medium",
            _ when percentage <= def.ThresholdMin + range * 0.75 => "high",
            _ => "very-high",
        };
    }

    /// <summary>
    /// Format a cannabinoid value for display.
    /// </summary>
    /// <param name="value">Percentage value.</param>
    /// <param name="decimals">Decimal places (default 2).</param>
    /// <returns>Formatted string (e.g. "23.45%").</returns>
    public static string FormatValue(double value, int decimals = 2) =>
        $"{value.ToString($"F{decimals}")}%";

    /// <summary>
    /// Calculate total cannabinoid percentage from a profile dictionary.
    /// </summary>
    /// <param name="profile">Cannabinoid name → percentage map.</param>
    /// <returns>Sum of all percentages.</returns>
    public static double CalculateTotal(IReadOnlyDictionary<string, double> profile) =>
        profile.Values.Sum();

    /// <summary>
    /// Calculate Euclidean distance between two cannabinoid profiles.
    /// Used for similarity comparisons.
    /// </summary>
    /// <param name="profile1">First profile.</param>
    /// <param name="profile2">Second profile.</param>
    /// <returns>Euclidean distance (lower = more similar).</returns>
    public static double CalculateEuclideanDistance(
        IReadOnlyDictionary<string, double> profile1,
        IReadOnlyDictionary<string, double> profile2)
    {
        var allKeys = new HashSet<string>(profile1.Keys);
        allKeys.UnionWith(profile2.Keys);

        double sumSquares = 0;
        foreach (var key in allKeys)
        {
            profile1.TryGetValue(key, out var val1);
            profile2.TryGetValue(key, out var val2);
            sumSquares += (val1 - val2) * (val1 - val2);
        }

        return Math.Sqrt(sumSquares);
    }
}
