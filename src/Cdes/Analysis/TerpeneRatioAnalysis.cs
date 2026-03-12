// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================
// CDES v1.5 Terpene Ratio Analysis
// Provides compositional data analysis metrics for terpene profiles:
//   - Dominance Index + qualitative level
//   - Balance Score (Shannon Entropy, normalised)
//   - HHI (Herfindahl-Hirschman Index)
//   - CLR (Centered Log-Ratio) Coordinates
//   - Ratio Signatures
//   - Archetype Classification via Aitchison distance
// =============================================================================

using System.Collections.Frozen;

namespace Cdes.Analysis;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ENUMERATIONS
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

/// <summary>
/// CDES v1.5: Terpene profile archetypes based on clustering analysis.
/// </summary>
public enum TerpeneArchetype
{
    /// <summary>Myrcene Dominant</summary>
    ArchMyr,
    /// <summary>Limonene Forward</summary>
    ArchLim,
    /// <summary>Balanced Trio</summary>
    ArchBal,
    /// <summary>Pinene Sharp</summary>
    ArchPin,
    /// <summary>Caryophyllene Spicy</summary>
    ArchCar,
    /// <summary>Terpinolene Rare</summary>
    ArchTer,
    /// <summary>Linalool Floral</summary>
    ArchLin,
    /// <summary>Ocimene Sweet</summary>
    ArchOci,
    /// <summary>Unknown / Unclassified</summary>
    ArchUnk,
}

/// <summary>
/// CDES v1.5: Qualitative dominance level based on Dominance Index.
/// </summary>
public enum DominanceLevel
{
    /// <summary>DI &lt; 0.25</summary>
    Balanced,
    /// <summary>DI 0.25 – 0.50</summary>
    Slight,
    /// <summary>DI 0.50 – 0.75</summary>
    Moderate,
    /// <summary>DI &gt; 0.75</summary>
    Strong,
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// RESULT RECORDS
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

/// <summary>CDES v1.5: A terpene with its rank position in the profile.</summary>
public sealed record RankedTerpene
{
    public required int Rank { get; init; }
    public required string TerpeneId { get; init; }
    public required string Name { get; init; }
    public required double Concentration { get; init; }
    public required string Unit { get; init; }
    public required double PercentOfTotal { get; init; }
}

/// <summary>CDES v1.5: Compact ratio representation for similarity matching.</summary>
public sealed record RatioSignature
{
    /// <summary>Encoded string, e.g. "MYR:LIM:CAR|0.57:0.38:0.24".</summary>
    public required string Encoded { get; init; }
    /// <summary>T1/T2 ratio.</summary>
    public required double T1T2Ratio { get; init; }
    /// <summary>T1/T3 ratio.</summary>
    public required double T1T3Ratio { get; init; }
    /// <summary>T2/T3 ratio.</summary>
    public required double T2T3Ratio { get; init; }
}

/// <summary>CDES v1.5: Archetype classification results.</summary>
public sealed record ArchetypeClassification
{
    public required TerpeneArchetype PrimaryArchetype { get; init; }
    public required double ArchetypeConfidence { get; init; }
    public required IReadOnlyDictionary<string, double> ArchetypeDistances { get; init; }
}

/// <summary>CDES v1.5: Computed ratio analysis metrics for terpene profiles.</summary>
public sealed record TerpeneRatioMetrics
{
    public required double DominanceIndex { get; init; }
    public required DominanceLevel DominanceLevel { get; init; }
    public required double BalanceScore { get; init; }
    public required double BalanceScoreNormalized { get; init; }
    public required double Hhi { get; init; }
    public required int TopTerpeneCount { get; init; }
    public required IReadOnlyList<RankedTerpene> RankedTerpenes { get; init; }
    public required RatioSignature? RatioSignature { get; init; }
    public required IReadOnlyDictionary<string, double> ClrCoordinates { get; init; }
    public required IReadOnlyDictionary<string, double> KeyRatios { get; init; }
    public required ArchetypeClassification? ArchetypeClassification { get; init; }
}

/// <summary>Human-readable archetype description.</summary>
public sealed record ArchetypeDescription
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyList<string> TypicalEffects { get; init; }
    public required string SignaturePattern { get; init; }
}

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// STATIC ANALYSER
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

/// <summary>
/// CDES v1.5 terpene ratio analysis engine.
/// Computes dominance, balance, CLR coordinates, ratio signatures, and archetype
/// classification for terpene profiles.
/// </summary>
public static class TerpeneRatioAnalyzer
{
    // ── Reference data ───────────────────────────────────────────────────

    /// <summary>Short codes for the 9 CDES standard terpenes.</summary>
    public static readonly FrozenDictionary<string, string> TerpeneCodes =
        new Dictionary<string, string>
        {
            ["myrcene"] = "MYR",
            ["limonene"] = "LIM",
            ["caryophyllene"] = "CAR",
            ["pinene"] = "PIN",
            ["linalool"] = "LIN",
            ["humulene"] = "HUM",
            ["terpinolene"] = "TER",
            ["ocimene"] = "OCI",
            ["bisabolol"] = "BIS",
        }.ToFrozenDictionary();

    /// <summary>Canonical display names (Greek-letter prefix).</summary>
    public static readonly FrozenDictionary<string, string> TerpeneNames =
        new Dictionary<string, string>
        {
            ["myrcene"] = "\u03B2-Myrcene",
            ["limonene"] = "\u03B4-Limonene",
            ["caryophyllene"] = "\u03B2-Caryophyllene",
            ["pinene"] = "\u03B1-Pinene",
            ["linalool"] = "Linalool",
            ["humulene"] = "\u03B1-Humulene",
            ["terpinolene"] = "Terpinolene",
            ["ocimene"] = "Ocimene",
            ["bisabolol"] = "\u03B1-Bisabolol",
        }.ToFrozenDictionary();

    /// <summary>Fixed analysis order for the 9 CDES standard terpenes.</summary>
    public static readonly IReadOnlyList<string> FixedTerpeneOrder =
    [
        "myrcene", "limonene", "caryophyllene", "pinene", "linalool",
        "humulene", "terpinolene", "ocimene", "bisabolol",
    ];

    /// <summary>Archetype centroids (CLR coordinates) trained from Terprint data.</summary>
    private static readonly FrozenDictionary<TerpeneArchetype, FrozenDictionary<string, double>> s_archetypeCentroids =
        new Dictionary<TerpeneArchetype, FrozenDictionary<string, double>>
        {
            [TerpeneArchetype.ArchMyr] = Centroid(1.2, -0.3, -0.4, -0.5, -0.2, -0.6, -0.8, -0.7, -0.5),
            [TerpeneArchetype.ArchLim] = Centroid(-0.2, 1.1, -0.2, 0.3, -0.4, -0.5, 0.2, -0.3, -0.4),
            [TerpeneArchetype.ArchBal] = Centroid(0.3, 0.2, 0.2, 0.1, 0.1, -0.1, -0.2, -0.2, -0.1),
            [TerpeneArchetype.ArchPin] = Centroid(-0.4, 0.2, -0.3, 1.0, -0.3, -0.4, 0.1, -0.2, -0.3),
            [TerpeneArchetype.ArchCar] = Centroid(-0.3, -0.4, 1.0, -0.2, 0.2, 0.6, -0.5, -0.4, -0.3),
            [TerpeneArchetype.ArchTer] = Centroid(-0.5, -0.3, -0.4, 0.2, -0.3, -0.5, 1.3, 0.1, -0.4),
            [TerpeneArchetype.ArchLin] = Centroid(0.2, -0.2, 0.1, -0.3, 1.0, -0.2, -0.4, -0.3, 0.3),
            [TerpeneArchetype.ArchOci] = Centroid(-0.3, -0.2, -0.3, -0.2, -0.2, -0.4, 0.2, 1.1, -0.2),
        }.ToFrozenDictionary();

    // ── Public API ───────────────────────────────────────────────────────

    /// <summary>
    /// Compute CLR coordinates for a terpene profile.
    /// </summary>
    /// <param name="terpeneValues">Map of terpene name to concentration (%).</param>
    /// <returns>Map of terpene name to CLR coordinate.</returns>
    public static IReadOnlyDictionary<string, double> ComputeClrCoordinates(
        IReadOnlyDictionary<string, double> terpeneValues)
    {
        var values = new double[FixedTerpeneOrder.Count];
        for (var i = 0; i < FixedTerpeneOrder.Count; i++)
        {
            terpeneValues.TryGetValue(FixedTerpeneOrder[i], out values[i]);
        }

        PrepareForClr(values);
        var clr = ComputeClr(values);

        var result = new Dictionary<string, double>(FixedTerpeneOrder.Count);
        for (var i = 0; i < FixedTerpeneOrder.Count; i++)
        {
            result[FixedTerpeneOrder[i]] = clr[i];
        }
        return result;
    }

    /// <summary>
    /// Compute all CDES v1.5 ratio metrics for a terpene profile.
    /// </summary>
    /// <param name="terpeneValues">Map of terpene name to concentration (%).</param>
    /// <returns><see cref="TerpeneRatioMetrics"/> with all computed values.</returns>
    public static TerpeneRatioMetrics ComputeRatioMetrics(
        IReadOnlyDictionary<string, double> terpeneValues)
    {
        // Get values in fixed order
        var entries = new (string Terpene, double Conc)[FixedTerpeneOrder.Count];
        for (var i = 0; i < FixedTerpeneOrder.Count; i++)
        {
            var t = FixedTerpeneOrder[i];
            terpeneValues.TryGetValue(t, out var conc);
            entries[i] = (t, conc);
        }

        // Sort by concentration descending
        var sorted = entries.OrderByDescending(e => e.Conc).ToArray();

        // Total
        var total = entries.Sum(e => e.Conc);
        if (total == 0) total = 1.0;

        // Ranked terpenes
        var ranked = new RankedTerpene[sorted.Length];
        for (var i = 0; i < sorted.Length; i++)
        {
            ranked[i] = new RankedTerpene
            {
                Rank = i + 1,
                TerpeneId = $"terpene:{sorted[i].Terpene}",
                Name = TerpeneNames.GetValueOrDefault(sorted[i].Terpene, sorted[i].Terpene),
                Concentration = sorted[i].Conc,
                Unit = "percent",
                PercentOfTotal = total > 0 ? sorted[i].Conc / total * 100 : 0,
            };
        }

        // T1..T4
        var t1 = sorted.Length > 0 ? sorted[0].Conc : 0;
        var t2 = sorted.Length > 1 ? sorted[1].Conc : 0;
        var t3 = sorted.Length > 2 ? sorted[2].Conc : 0;
        var t4 = sorted.Length > 3 ? sorted[3].Conc : 0;

        // Dominance Index
        var dominanceIndex = t1 + t2 > 0 ? (t1 - t2) / (t1 + t2) : 0;

        var dominanceLevel = dominanceIndex switch
        {
            < 0.25 => DominanceLevel.Balanced,
            < 0.50 => DominanceLevel.Slight,
            < 0.75 => DominanceLevel.Moderate,
            _ => DominanceLevel.Strong,
        };

        // Balance Score (Shannon Entropy)
        var balanceScore = 0.0;
        foreach (var (_, conc) in entries)
        {
            if (conc > 0 && total > 0)
            {
                var p = conc / total;
                balanceScore -= p * Math.Log(p);
            }
        }

        // Normalised balance score (0-1)
        var nonZeroCount = entries.Count(e => e.Conc > 0);
        var balanceScoreNormalized = 0.0;
        if (nonZeroCount > 1)
        {
            var maxEntropy = Math.Log(nonZeroCount);
            balanceScoreNormalized = maxEntropy > 0 ? balanceScore / maxEntropy : 0;
        }

        // HHI
        var hhi = total > 0
            ? entries.Sum(e => Math.Pow(e.Conc / total, 2))
            : 0;

        // Top terpene count (> 5% of total)
        var topTerpeneCount = entries.Count(e => e.Conc / total * 100 > 5);

        // Ratio Signature
        var t1Code = TerpeneCodes.GetValueOrDefault(sorted[0].Terpene, "UNK");
        var t2Code = sorted.Length > 1 ? TerpeneCodes.GetValueOrDefault(sorted[1].Terpene, "UNK") : "UNK";
        var t3Code = sorted.Length > 2 ? TerpeneCodes.GetValueOrDefault(sorted[2].Terpene, "UNK") : "UNK";

        var t1T2Ratio = t2 > 0 ? Math.Min(t1 / t2, 99.99) : 99.99;
        var t1T3Ratio = t3 > 0 ? Math.Min(t1 / t3, 99.99) : 99.99;
        var t2T3Ratio = t3 > 0 ? Math.Min(t2 / t3, 99.99) : 99.99;

        var r2 = t1 > 0 ? t2 / t1 : 0;
        var r3 = t1 > 0 ? t3 / t1 : 0;
        var r4 = t1 > 0 ? t4 / t1 : 0;

        var ratioSignature = new RatioSignature
        {
            Encoded = $"{t1Code}:{t2Code}:{t3Code}|{r2:F2}:{r3:F2}:{r4:F2}",
            T1T2Ratio = t1T2Ratio,
            T1T3Ratio = t1T3Ratio,
            T2T3Ratio = t2T3Ratio,
        };

        // CLR Coordinates
        var clrCoords = ComputeClrCoordinates(terpeneValues);
        var clrRounded = clrCoords.ToDictionary(
            kv => kv.Key,
            kv => Round(kv.Value, 4));

        // Key Ratios (pairwise log-ratios)
        var myrcene = Math.Max(GetValue(terpeneValues, "myrcene"), 0.001);
        var limonene = Math.Max(GetValue(terpeneValues, "limonene"), 0.001);
        var pinene = Math.Max(GetValue(terpeneValues, "pinene"), 0.001);
        var caryophyllene = Math.Max(GetValue(terpeneValues, "caryophyllene"), 0.001);
        var humulene = Math.Max(GetValue(terpeneValues, "humulene"), 0.001);
        var terpinolene = Math.Max(GetValue(terpeneValues, "terpinolene"), 0.001);

        var keyRatios = new Dictionary<string, double>
        {
            ["myrceneLimoneneLr"] = Round(Math.Log(myrcene / limonene), 3),
            ["myrcenePineneLr"] = Round(Math.Log(myrcene / pinene), 3),
            ["caryophylleneHumuleneLr"] = Round(Math.Log(caryophyllene / humulene), 3),
            ["limoneneTerpinoleneLr"] = Round(Math.Log(limonene / terpinolene), 3),
        };

        // Classify archetype
        var archetypeClassification = ClassifyArchetype(clrCoords);

        return new TerpeneRatioMetrics
        {
            DominanceIndex = Round(dominanceIndex, 4),
            DominanceLevel = dominanceLevel,
            BalanceScore = Round(balanceScore, 4),
            BalanceScoreNormalized = Round(balanceScoreNormalized, 4),
            Hhi = Round(hhi, 4),
            TopTerpeneCount = topTerpeneCount,
            RankedTerpenes = ranked,
            RatioSignature = ratioSignature,
            ClrCoordinates = clrRounded,
            KeyRatios = keyRatios,
            ArchetypeClassification = archetypeClassification,
        };
    }

    /// <summary>
    /// Classify a terpene profile into an archetype based on CLR coordinates.
    /// </summary>
    public static ArchetypeClassification ClassifyArchetype(
        IReadOnlyDictionary<string, double> clrCoordinates)
    {
        var distances = new Dictionary<string, double>();

        foreach (var (archetype, centroid) in s_archetypeCentroids)
        {
            distances[archetype.ToString()] = Round(AitchisonDistance(clrCoordinates, centroid), 4);
        }

        var sortedDistances = distances
            .OrderBy(kv => kv.Value)
            .ToArray();

        var primaryArchetype = TerpeneArchetype.ArchUnk;
        var confidence = 0.0;

        if (sortedDistances.Length >= 2)
        {
            primaryArchetype = Enum.Parse<TerpeneArchetype>(sortedDistances[0].Key);
            var primaryDist = sortedDistances[0].Value;
            var secondDist = sortedDistances[1].Value;

            if (secondDist > 0 && primaryDist < secondDist)
                confidence = 1 - primaryDist / secondDist;
            else
                confidence = 0.5;

            confidence = Math.Clamp(confidence, 0, 1);
        }

        return new ArchetypeClassification
        {
            PrimaryArchetype = primaryArchetype,
            ArchetypeConfidence = Round(confidence, 4),
            ArchetypeDistances = distances,
        };
    }

    /// <summary>
    /// Get human-readable description of an archetype.
    /// </summary>
    public static ArchetypeDescription GetArchetypeDescription(TerpeneArchetype archetype)
    {
        return s_archetypeDescriptions.GetValueOrDefault(archetype, s_archetypeDescriptions[TerpeneArchetype.ArchUnk]);
    }

    // ── Private helpers ──────────────────────────────────────────────────

    private static readonly FrozenDictionary<TerpeneArchetype, ArchetypeDescription> s_archetypeDescriptions =
        new Dictionary<TerpeneArchetype, ArchetypeDescription>
        {
            [TerpeneArchetype.ArchMyr] = new()
            {
                Name = "Myrcene Dominant",
                Description = "High myrcene content (>40%), classic indica-leaning profile",
                TypicalEffects = ["sedating", "body-focused", "relaxing"],
                SignaturePattern = "MYR > 40%, others < 15% each",
            },
            [TerpeneArchetype.ArchLim] = new()
            {
                Name = "Limonene Forward",
                Description = "Limonene-led profile with citrus notes",
                TypicalEffects = ["uplifting", "energizing", "mood-enhancing"],
                SignaturePattern = "LIM > 25%, MYR secondary",
            },
            [TerpeneArchetype.ArchBal] = new()
            {
                Name = "Balanced Trio",
                Description = "Top 3 terpenes within 80-100% of each other",
                TypicalEffects = ["complex", "nuanced", "full-spectrum"],
                SignaturePattern = "Top 3 close in concentration",
            },
            [TerpeneArchetype.ArchPin] = new()
            {
                Name = "Pinene Sharp",
                Description = "Pinene-dominant with cognitive clarity effects",
                TypicalEffects = ["alert", "focused", "clear-headed"],
                SignaturePattern = "PIN > 15%, low MYR",
            },
            [TerpeneArchetype.ArchCar] = new()
            {
                Name = "Caryophyllene Spicy",
                Description = "Caryophyllene-led with humulene support",
                TypicalEffects = ["peppery", "therapeutic", "anti-inflammatory"],
                SignaturePattern = "CAR > 20%, HUM present",
            },
            [TerpeneArchetype.ArchTer] = new()
            {
                Name = "Terpinolene Rare",
                Description = "Unusual terpinolene-dominant profile",
                TypicalEffects = ["unique", "energizing", "creative"],
                SignaturePattern = "TER dominant (rare)",
            },
            [TerpeneArchetype.ArchLin] = new()
            {
                Name = "Linalool Floral",
                Description = "Linalool-forward with floral/lavender notes",
                TypicalEffects = ["calming", "floral", "anxiety-reducing"],
                SignaturePattern = "LIN > 15%",
            },
            [TerpeneArchetype.ArchOci] = new()
            {
                Name = "Ocimene Sweet",
                Description = "Ocimene-led sweet and herbaceous profile",
                TypicalEffects = ["sweet", "herbaceous", "uplifting"],
                SignaturePattern = "OCI > 10%",
            },
            [TerpeneArchetype.ArchUnk] = new()
            {
                Name = "Unknown",
                Description = "Profile does not match known archetypes",
                TypicalEffects = ["variable"],
                SignaturePattern = "N/A",
            },
        }.ToFrozenDictionary();

    /// <summary>Build a centroid dictionary from values in FixedTerpeneOrder sequence.</summary>
    private static FrozenDictionary<string, double> Centroid(
        double myr, double lim, double car, double pin, double lin,
        double hum, double ter, double oci, double bis) =>
        new Dictionary<string, double>
        {
            ["myrcene"] = myr, ["limonene"] = lim, ["caryophyllene"] = car,
            ["pinene"] = pin, ["linalool"] = lin, ["humulene"] = hum,
            ["terpinolene"] = ter, ["ocimene"] = oci, ["bisabolol"] = bis,
        }.ToFrozenDictionary();

    /// <summary>Multiplicative replacement of zeros before CLR transform.</summary>
    private static void PrepareForClr(double[] values, double delta = 0.001)
    {
        var zeroCount = 0;
        var nonZeroSum = 0.0;

        for (var i = 0; i < values.Length; i++)
        {
            if (values[i] <= 0)
                zeroCount++;
            else
                nonZeroSum += values[i];
        }

        if (zeroCount == 0) return;

        var totalDelta = delta * zeroCount;

        if (nonZeroSum > 0)
        {
            var scale = (1 - totalDelta) / nonZeroSum;
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = values[i] > 0 ? values[i] * scale : delta;
            }
        }
        else
        {
            Array.Fill(values, delta);
        }
    }

    /// <summary>Centered log-ratio transformation.</summary>
    private static double[] ComputeClr(double[] values)
    {
        var logValues = new double[values.Length];
        var geoMeanLog = 0.0;

        for (var i = 0; i < values.Length; i++)
        {
            logValues[i] = Math.Log(values[i]);
            geoMeanLog += logValues[i];
        }
        geoMeanLog /= values.Length;

        var clr = new double[values.Length];
        for (var i = 0; i < values.Length; i++)
        {
            clr[i] = logValues[i] - geoMeanLog;
        }
        return clr;
    }

    /// <summary>Aitchison distance between two CLR coordinate sets.</summary>
    private static double AitchisonDistance(
        IReadOnlyDictionary<string, double> clr1,
        IReadOnlyDictionary<string, double> clr2)
    {
        var sum = 0.0;
        foreach (var key in FixedTerpeneOrder)
        {
            clr1.TryGetValue(key, out var v1);
            clr2.TryGetValue(key, out var v2);
            var diff = v1 - v2;
            sum += diff * diff;
        }
        return Math.Sqrt(sum);
    }

    private static double Round(double value, int decimals)
    {
        return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
    }

    private static double GetValue(IReadOnlyDictionary<string, double> dict, string key)
    {
        dict.TryGetValue(key, out var v);
        return v;
    }
}
