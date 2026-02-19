// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using System.Collections.Frozen;

namespace Cdes.Reference;

/// <summary>
/// Authoritative CDES terpene color palette and reference data.
/// 30 standard cannabis terpenes with colors, aromas, therapeutic effects,
/// and physical properties. Use <see cref="TerpeneUtilities"/> for lookups.
/// </summary>
public static class TerpeneLibrary
{
    /// <summary>
    /// Fallback hex color for unrecognized terpenes.
    /// </summary>
    public const string UnknownTerpeneColor = "#808080";

    // ═══════════════════════════════════════════════════════════════════
    // PRIMARY TERPENES (Top 8 — Most Common in Cannabis)
    // ═══════════════════════════════════════════════════════════════════

    private static readonly TerpeneDefinition[] s_definitions =
    [
        new()
        {
            Id = 1,
            CanonicalName = "β-Myrcene",
            DisplayName = "Myrcene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 1,
            Hex = "#FFFF00",
            Rgb = new RgbColor(255, 255, 0),
            Hsl = new HslColor(60, 100, 50),
            Aroma = "Earthy, musky, herbal, mango",
            Therapeutic = ["Relaxation", "Sedation", "Anti-inflammatory", "Pain Relief"],
            BoilingPointC = 167,
            Aliases = ["beta-myrcene", "b-myrcene", "myrcene"],
        },
        new()
        {
            Id = 2,
            CanonicalName = "δ-Limonene",
            DisplayName = "Limonene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 2,
            Hex = "#66CCFF",
            Rgb = new RgbColor(102, 204, 255),
            Hsl = new HslColor(200, 100, 70),
            Aroma = "Citrus, lemon, orange, bright",
            Therapeutic = ["Mood Elevation", "Stress Relief", "Energy", "Anti-anxiety"],
            BoilingPointC = 176,
            Aliases = ["d-limonene", "limonene", "delta-limonene", "+limonene", "r-limonene"],
        },
        new()
        {
            Id = 3,
            CanonicalName = "β-Caryophyllene",
            DisplayName = "Caryophyllene",
            Category = TerpeneCategory.Sesquiterpene,
            DisplayOrder = 3,
            Hex = "#92D050",
            Rgb = new RgbColor(146, 208, 80),
            Hsl = new HslColor(89, 56, 56),
            Aroma = "Spicy, peppery, warm, woody",
            Therapeutic = ["Pain Relief", "Anti-anxiety", "Anti-inflammatory", "CB2 Agonist"],
            BoilingPointC = 130,
            Aliases = ["caryophyllene", "beta-caryophyllene", "b-caryophyllene", "bcp"],
        },
        new()
        {
            Id = 4,
            CanonicalName = "α-Pinene",
            DisplayName = "Pinene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 4,
            Hex = "#00B0F0",
            Rgb = new RgbColor(0, 176, 240),
            Hsl = new HslColor(196, 100, 47),
            Aroma = "Pine, fresh, crisp, forest",
            Therapeutic = ["Memory", "Focus", "Energy", "Anti-inflammatory", "Bronchodilator"],
            BoilingPointC = 155,
            Aliases = ["alpha-pinene", "a-pinene", "pinene"],
        },
        new()
        {
            Id = 5,
            CanonicalName = "Linalool",
            DisplayName = "Linalool",
            Category = TerpeneCategory.Monoterpenoid,
            DisplayOrder = 5,
            Hex = "#FF7C80",
            Rgb = new RgbColor(255, 124, 128),
            Hsl = new HslColor(358, 100, 74),
            Aroma = "Floral, lavender, sweet, calming",
            Therapeutic = ["Relaxation", "Sleep", "Anti-anxiety", "Stress Relief"],
            BoilingPointC = 198,
            Aliases = ["d-linalool", "l-linalool"],
        },
        new()
        {
            Id = 6,
            CanonicalName = "α-Humulene",
            DisplayName = "Humulene",
            Category = TerpeneCategory.Sesquiterpene,
            DisplayOrder = 6,
            Hex = "#0070C0",
            Rgb = new RgbColor(0, 112, 192),
            Hsl = new HslColor(205, 100, 38),
            Aroma = "Hoppy, earthy, spicy, herbal",
            Therapeutic = ["Anti-inflammatory", "Appetite Suppression", "Antibacterial"],
            BoilingPointC = 106,
            Aliases = ["humulene", "alpha-humulene", "a-humulene"],
        },
        new()
        {
            Id = 7,
            CanonicalName = "Terpinolene",
            DisplayName = "Terpinolene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 7,
            Hex = "#0000CC",
            Rgb = new RgbColor(0, 0, 204),
            Hsl = new HslColor(240, 100, 40),
            Aroma = "Fresh, herbal, piney, floral",
            Therapeutic = ["Energy", "Alertness", "Uplifting", "Antioxidant"],
            BoilingPointC = 186,
            Aliases = ["alpha-terpinolene"],
        },
        new()
        {
            Id = 8,
            CanonicalName = "Ocimene",
            DisplayName = "Ocimene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 8,
            Hex = "#00CC00",
            Rgb = new RgbColor(0, 204, 0),
            Hsl = new HslColor(120, 100, 40),
            Aroma = "Fresh, minty, herbal, citrus",
            Therapeutic = ["Mood Elevation", "Uplifting", "Antimicrobial", "Decongestant"],
            BoilingPointC = 100,
            Aliases = ["beta-ocimene", "trans-ocimene", "b-ocimene"],
        },

        // ═══════════════════════════════════════════════════════════════
        // SECONDARY TERPENES (9-16)
        // ═══════════════════════════════════════════════════════════════

        new()
        {
            Id = 9,
            CanonicalName = "Camphene",
            DisplayName = "Camphene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 9,
            Hex = "#33CC33",
            Rgb = new RgbColor(51, 204, 51),
            Hsl = new HslColor(120, 60, 50),
            Aroma = "Woody, camphor, spicy, pungent",
            Therapeutic = ["Anti-inflammatory", "Antimicrobial", "Cardiovascular"],
            BoilingPointC = 159,
            Aliases = ["d-camphene", "alpha-camphene"],
        },
        new()
        {
            Id = 10,
            CanonicalName = "Caryophyllene Oxide",
            DisplayName = "Caryophyllene Oxide",
            Category = TerpeneCategory.Sesquiterpene,
            DisplayOrder = 10,
            Hex = "#66FF66",
            Rgb = new RgbColor(102, 255, 102),
            Hsl = new HslColor(120, 100, 70),
            Aroma = "Peppery, woody, warm, oxidized",
            Therapeutic = ["Stress Relief", "Anti-inflammatory", "Antifungal"],
            BoilingPointC = 257,
            Aliases = ["caryophyllene-oxide"],
        },
        new()
        {
            Id = 11,
            CanonicalName = "Eucalyptol",
            DisplayName = "Eucalyptol",
            Category = TerpeneCategory.Monoterpenoid,
            DisplayOrder = 11,
            Hex = "#CCFFCC",
            Rgb = new RgbColor(204, 255, 204),
            Hsl = new HslColor(120, 100, 90),
            Aroma = "Minty, cool, fresh, eucalyptus",
            Therapeutic = ["Respiratory Support", "Mental Clarity", "Pain Relief"],
            BoilingPointC = 176,
            Aliases = ["1,8-cineole", "cineole"],
        },
        new()
        {
            Id = 12,
            CanonicalName = "Fenchyl Alcohol",
            DisplayName = "Fenchol",
            Category = TerpeneCategory.Monoterpenoid,
            DisplayOrder = 12,
            Hex = "#FFCCFF",
            Rgb = new RgbColor(255, 204, 255),
            Hsl = new HslColor(300, 100, 90),
            Aroma = "Sweet, herbaceous, minty, piney",
            Therapeutic = ["Relaxation", "Anti-anxiety", "Antibacterial"],
            BoilingPointC = 201,
            Aliases = ["fenchol", "endo-fenchol"],
        },
        new()
        {
            Id = 13,
            CanonicalName = "Geraniol",
            DisplayName = "Geraniol",
            Category = TerpeneCategory.Monoterpenoid,
            DisplayOrder = 13,
            Hex = "#FF99FF",
            Rgb = new RgbColor(255, 153, 255),
            Hsl = new HslColor(300, 100, 80),
            Aroma = "Floral, rose, sweet, citrus",
            Therapeutic = ["Neuroprotection", "Antioxidant", "Cancer Research"],
            BoilingPointC = 230,
            Aliases = ["trans-geraniol"],
        },
        new()
        {
            Id = 14,
            CanonicalName = "Guaiol",
            DisplayName = "Guaiol",
            Category = TerpeneCategory.Sesquiterpenoid,
            DisplayOrder = 14,
            Hex = "#CC00CC",
            Rgb = new RgbColor(204, 0, 204),
            Hsl = new HslColor(300, 100, 40),
            Aroma = "Woody, smoky, warm, piney",
            Therapeutic = ["Anti-inflammatory", "Antifungal", "Antimicrobial"],
            BoilingPointC = 92,
            Aliases = ["alpha-guaiol"],
        },
        new()
        {
            Id = 15,
            CanonicalName = "Isopulegol",
            DisplayName = "Isopulegol",
            Category = TerpeneCategory.Monoterpenoid,
            DisplayOrder = 15,
            Hex = "#FFCCCC",
            Rgb = new RgbColor(255, 204, 204),
            Hsl = new HslColor(0, 100, 90),
            Aroma = "Minty, cooling, fresh, menthol-like",
            Therapeutic = ["Antimicrobial", "Anti-inflammatory", "Gastroprotective"],
            BoilingPointC = 212,
            Aliases = ["iso-pulegol", "l-isopulegol"],
        },
        new()
        {
            Id = 16,
            CanonicalName = "Menthol",
            DisplayName = "Menthol",
            Category = TerpeneCategory.Monoterpenoid,
            DisplayOrder = 16,
            Hex = "#CC0000",
            Rgb = new RgbColor(204, 0, 0),
            Hsl = new HslColor(0, 100, 40),
            Aroma = "Minty, cool, strong, medicinal",
            Therapeutic = ["Cooling Sensation", "Anti-inflammatory", "Pain Relief"],
            BoilingPointC = 212,
            Aliases = ["l-menthol", "dl-menthol"],
        },

        // ═══════════════════════════════════════════════════════════════
        // TERTIARY TERPENES (17-24)
        // ═══════════════════════════════════════════════════════════════

        new()
        {
            Id = 17,
            CanonicalName = "Terpineol",
            DisplayName = "Terpineol",
            Category = TerpeneCategory.Monoterpenoid,
            DisplayOrder = 17,
            Hex = "#0066FF",
            Rgb = new RgbColor(0, 102, 255),
            Hsl = new HslColor(216, 100, 50),
            Aroma = "Piney, floral, woody, lilac",
            Therapeutic = ["Mood Elevation", "Sedation", "Antibacterial"],
            BoilingPointC = 217,
            Aliases = ["alpha-terpineol", "total terpineol"],
        },
        new()
        {
            Id = 18,
            CanonicalName = "Valencene",
            DisplayName = "Valencene",
            Category = TerpeneCategory.Sesquiterpene,
            DisplayOrder = 18,
            Hex = "#006666",
            Rgb = new RgbColor(0, 102, 102),
            Hsl = new HslColor(180, 100, 20),
            Aroma = "Citrus, sweet orange, fresh",
            Therapeutic = ["Mood Elevation", "Anti-inflammatory", "Insect Repellent"],
            BoilingPointC = 123,
            Aliases = ["d-valencene"],
        },
        new()
        {
            Id = 19,
            CanonicalName = "cis-Nerolidol",
            DisplayName = "cis-Nerolidol",
            Category = TerpeneCategory.Sesquiterpenoid,
            DisplayOrder = 19,
            Hex = "#339966",
            Rgb = new RgbColor(51, 153, 102),
            Hsl = new HslColor(150, 50, 40),
            Aroma = "Woody, floral, earthy, fruity",
            Therapeutic = ["Sleep Induction", "Anti-anxiety", "Sedation"],
            BoilingPointC = 276,
            Aliases = ["z-nerolidol"],
        },
        new()
        {
            Id = 20,
            CanonicalName = "trans-Nerolidol",
            DisplayName = "trans-Nerolidol",
            Category = TerpeneCategory.Sesquiterpenoid,
            DisplayOrder = 20,
            Hex = "#CC3399",
            Rgb = new RgbColor(204, 51, 153),
            Hsl = new HslColor(320, 60, 50),
            Aroma = "Floral, woody, herbal, apple",
            Therapeutic = ["Sleep Support", "Neuroprotection", "Antimicrobial"],
            BoilingPointC = 276,
            Aliases = ["e-nerolidol", "t-nerolidol"],
        },
        new()
        {
            Id = 21,
            CanonicalName = "p-Cymene",
            DisplayName = "p-Cymene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 21,
            Hex = "#FF6699",
            Rgb = new RgbColor(255, 102, 153),
            Hsl = new HslColor(340, 100, 70),
            Aroma = "Herbal, thyme-like, warm, earthy",
            Therapeutic = ["Anti-inflammatory", "Antimicrobial", "Analgesic"],
            BoilingPointC = 177,
            Aliases = ["para-cymene"],
        },
        new()
        {
            Id = 22,
            CanonicalName = "trans-Caryophyllene",
            DisplayName = "trans-Caryophyllene",
            Category = TerpeneCategory.Sesquiterpene,
            DisplayOrder = 22,
            Hex = "#FF3399",
            Rgb = new RgbColor(255, 51, 153),
            Hsl = new HslColor(330, 100, 60),
            Aroma = "Spicy, peppery, hot, herbal",
            Therapeutic = ["Pain Relief", "CB2 Agonist", "Anti-inflammatory"],
            BoilingPointC = 130,
            Aliases = ["t-caryophyllene", "e-caryophyllene"],
        },
        new()
        {
            Id = 23,
            CanonicalName = "α-Bisabolol",
            DisplayName = "Bisabolol",
            Category = TerpeneCategory.Sesquiterpenoid,
            DisplayOrder = 23,
            Hex = "#002060",
            Rgb = new RgbColor(0, 32, 96),
            Hsl = new HslColor(220, 100, 19),
            Aroma = "Sweet, floral, warm, chamomile",
            Therapeutic = ["Anti-inflammatory", "Calming", "Skin Healing"],
            BoilingPointC = 153,
            Aliases = ["bisabolol", "alpha-bisabolol"],
        },
        new()
        {
            Id = 24,
            CanonicalName = "α-Terpinene",
            DisplayName = "α-Terpinene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 24,
            Hex = "#00B050",
            Rgb = new RgbColor(0, 176, 80),
            Hsl = new HslColor(147, 100, 35),
            Aroma = "Piney, herbal, fresh, lemony",
            Therapeutic = ["Energy", "Antimicrobial", "Antioxidant"],
            BoilingPointC = 174,
            Aliases = ["alpha-terpinene", "gamma-terpinene"],
        },

        // ═══════════════════════════════════════════════════════════════
        // MINOR TERPENES (25-30)
        // ═══════════════════════════════════════════════════════════════

        new()
        {
            Id = 25,
            CanonicalName = "β-Pinene",
            DisplayName = "β-Pinene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 25,
            Hex = "#FFC000",
            Rgb = new RgbColor(255, 192, 0),
            Hsl = new HslColor(45, 100, 50),
            Aroma = "Pine, woody, fresh, herbal",
            Therapeutic = ["Anti-inflammatory", "Memory Support", "Bronchodilator"],
            BoilingPointC = 166,
            Aliases = ["beta-pinene", "b-pinene"],
        },
        new()
        {
            Id = 26,
            CanonicalName = "δ-3-Carene",
            DisplayName = "Carene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 26,
            Hex = "#C00000",
            Rgb = new RgbColor(192, 0, 0),
            Hsl = new HslColor(0, 100, 38),
            Aroma = "Sweet, piney, woodsy, cypress",
            Therapeutic = ["Anti-inflammatory", "Bone Growth Research", "Drying"],
            BoilingPointC = 171,
            Aliases = ["3-carene", "delta-3-carene", "carene"],
        },
        new()
        {
            Id = 27,
            CanonicalName = "Borneol",
            DisplayName = "Borneol",
            Category = TerpeneCategory.Monoterpenoid,
            DisplayOrder = 27,
            Hex = "#99FF33",
            Rgb = new RgbColor(153, 255, 51),
            Hsl = new HslColor(90, 100, 60),
            Aroma = "Camphor, minty, woody, herbal",
            Therapeutic = ["Anti-inflammatory", "Analgesic", "Blood-Brain Barrier"],
            BoilingPointC = 210,
            Aliases = ["isoborneol", "d-borneol"],
        },
        new()
        {
            Id = 28,
            CanonicalName = "Sabinene",
            DisplayName = "Sabinene",
            Category = TerpeneCategory.Monoterpene,
            DisplayOrder = 28,
            Hex = "#C0C0C0",
            Rgb = new RgbColor(192, 192, 192),
            Hsl = new HslColor(0, 0, 75),
            Aroma = "Spicy, woody, warm, piney",
            Therapeutic = ["Anti-inflammatory", "Antimicrobial", "Antioxidant"],
            BoilingPointC = 163,
            Aliases = ["alpha-sabinene"],
        },
        new()
        {
            Id = 29,
            CanonicalName = "Farnesene",
            DisplayName = "Farnesene",
            Category = TerpeneCategory.Sesquiterpene,
            DisplayOrder = 29,
            Hex = "#00CC99",
            Rgb = new RgbColor(0, 204, 153),
            Hsl = new HslColor(165, 100, 40),
            Aroma = "Sweet, woody, apple-like, floral",
            Therapeutic = ["Anti-inflammatory", "Mood Elevation", "Antimicrobial"],
            BoilingPointC = 125,
            Aliases = ["alpha-farnesene", "beta-farnesene"],
        },
        new()
        {
            Id = 30,
            CanonicalName = "Geranyl Acetate",
            DisplayName = "Geranyl Acetate",
            Category = TerpeneCategory.Ester,
            DisplayOrder = 30,
            Hex = "#996633",
            Rgb = new RgbColor(153, 102, 51),
            Hsl = new HslColor(30, 50, 40),
            Aroma = "Floral, fruity, sweet, rose-like",
            Therapeutic = ["Uplifting", "Anti-inflammatory", "Antimicrobial"],
            BoilingPointC = 245,
            Aliases = ["geranylacetate"],
        },
    ];

    /// <summary>
    /// All 30 CDES standard terpene definitions, keyed by canonical name.
    /// Thread-safe, immutable, and optimized for lookup.
    /// </summary>
    public static FrozenDictionary<string, TerpeneDefinition> StandardTerpenes { get; } =
        s_definitions.ToFrozenDictionary(d => d.CanonicalName);

    /// <summary>
    /// All 30 terpene definitions sorted by <see cref="TerpeneDefinition.DisplayOrder"/>.
    /// </summary>
    public static IReadOnlyList<TerpeneDefinition> InDisplayOrder { get; } =
        s_definitions.OrderBy(d => d.DisplayOrder).ToArray();

    /// <summary>
    /// Hex color palette in display order (for Power BI, charts, etc.).
    /// </summary>
    public static IReadOnlyList<string> ColorPalette { get; } =
        InDisplayOrder.Select(d => d.Hex).ToArray();
}
