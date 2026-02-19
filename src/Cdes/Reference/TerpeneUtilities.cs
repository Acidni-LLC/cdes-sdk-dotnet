// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using System.Collections.Frozen;

namespace Cdes.Reference;

/// <summary>
/// Terpene name normalization, color lookup, alias matching, and query helpers.
/// All lookups are case-insensitive and alias-aware.
/// </summary>
public static class TerpeneUtilities
{
    /// <summary>
    /// Alias → canonical name map (case-insensitive).
    /// Built once from <see cref="TerpeneLibrary.StandardTerpenes"/>.
    /// </summary>
    private static readonly FrozenDictionary<string, string> s_aliasMap = BuildAliasMap();

    private static FrozenDictionary<string, string> BuildAliasMap()
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var (canonical, def) in TerpeneLibrary.StandardTerpenes)
        {
            map.TryAdd(canonical, canonical);
            map.TryAdd(def.DisplayName, canonical);

            foreach (var alias in def.Aliases)
            {
                map.TryAdd(alias, canonical);
            }
        }

        return map.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Normalize a raw terpene name to its CDES canonical form.
    /// Handles aliases, casing, and common naming variations.
    /// </summary>
    /// <param name="rawName">Input terpene name in any format.</param>
    /// <returns>Canonical name if recognized; otherwise the trimmed original.</returns>
    public static string NormalizeName(string rawName)
    {
        ArgumentNullException.ThrowIfNull(rawName);
        var cleaned = rawName.Trim();
        return s_aliasMap.TryGetValue(cleaned, out var canonical) ? canonical : cleaned;
    }

    /// <summary>
    /// Get a terpene definition by name (supports aliases).
    /// </summary>
    /// <param name="name">Terpene name (canonical or alias).</param>
    /// <returns>Definition if found; otherwise <c>null</c>.</returns>
    public static TerpeneDefinition? GetDefinition(string name)
    {
        var canonical = NormalizeName(name);
        return TerpeneLibrary.StandardTerpenes.TryGetValue(canonical, out var def) ? def : null;
    }

    /// <summary>
    /// Get the hex color for a terpene (supports aliases).
    /// </summary>
    /// <param name="name">Terpene name.</param>
    /// <returns>Hex color string or <c>null</c> if not found.</returns>
    public static string? GetColor(string name) =>
        GetDefinition(name)?.Hex;

    /// <summary>
    /// Get the hex color for a terpene with a fallback for unknown names.
    /// </summary>
    /// <param name="name">Terpene name.</param>
    /// <returns>Hex color string (never <c>null</c>).</returns>
    public static string GetColorSafe(string name) =>
        GetColor(name) ?? TerpeneLibrary.UnknownTerpeneColor;

    /// <summary>
    /// Get the RGB color for a terpene (supports aliases).
    /// </summary>
    /// <param name="name">Terpene name.</param>
    /// <returns>RGB color or <c>null</c> if not found.</returns>
    public static RgbColor? GetRgb(string name) =>
        GetDefinition(name)?.Rgb;

    /// <summary>
    /// Check whether a terpene name is recognized in the CDES standard.
    /// </summary>
    /// <param name="name">Terpene name to check.</param>
    /// <returns><c>true</c> if recognized.</returns>
    public static bool IsRecognized(string name) =>
        GetDefinition(name) is not null;

    /// <summary>
    /// Get all 30 canonical terpene names.
    /// </summary>
    public static IReadOnlyList<string> GetAllNames() =>
        TerpeneLibrary.StandardTerpenes.Keys.ToArray();

    /// <summary>
    /// Filter terpenes by chemical category.
    /// </summary>
    /// <param name="category">Category to filter by.</param>
    /// <returns>Definitions matching the category.</returns>
    public static IReadOnlyList<TerpeneDefinition> GetByCategory(TerpeneCategory category) =>
        TerpeneLibrary.InDisplayOrder.Where(d => d.Category == category).ToArray();

    /// <summary>
    /// Find terpenes that include a therapeutic effect (partial match, case-insensitive).
    /// </summary>
    /// <param name="effect">Effect keyword to search for.</param>
    /// <returns>Matching definitions.</returns>
    public static IReadOnlyList<TerpeneDefinition> FindByEffect(string effect)
    {
        ArgumentNullException.ThrowIfNull(effect);
        return TerpeneLibrary.InDisplayOrder
            .Where(d => d.Therapeutic?.Any(t => t.Contains(effect, StringComparison.OrdinalIgnoreCase)) == true)
            .ToArray();
    }

    /// <summary>
    /// Get a subset of the color palette (for charting).
    /// </summary>
    /// <param name="count">Number of colors to return (max 30).</param>
    /// <returns>Hex color strings in display order.</returns>
    public static IReadOnlyList<string> GetPalette(int count = 30) =>
        TerpeneLibrary.ColorPalette.Take(Math.Min(count, 30)).ToArray();
}
