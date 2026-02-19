// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using Cdes.Models;
using Cdes.Reference;

namespace Cdes.Analysis;

/// <summary>
/// Result of comparing two cannabinoid profiles.
/// </summary>
public sealed record ComparisonResult
{
    /// <summary>First profile.</summary>
    public required CannabinoidProfile Profile1 { get; init; }

    /// <summary>Second profile.</summary>
    public required CannabinoidProfile Profile2 { get; init; }

    /// <summary>Euclidean distance between profiles (lower = more similar).</summary>
    public required double Distance { get; init; }

    /// <summary>Similarity score 0-100 (higher = more similar).</summary>
    public required double Similarity { get; init; }

    /// <summary>Per-compound differences.</summary>
    public required IReadOnlyDictionary<string, CompoundDelta> Differences { get; init; }
}

/// <summary>
/// Delta between two values for a single compound.
/// </summary>
public sealed record CompoundDelta(double Value1, double Value2, double Delta);

/// <summary>
/// A profile ranked by similarity to a target.
/// </summary>
public sealed record SimilarityResult
{
    /// <summary>The compared profile.</summary>
    public required CannabinoidProfile Profile { get; init; }

    /// <summary>Euclidean distance.</summary>
    public required double Distance { get; init; }

    /// <summary>Similarity score 0-100.</summary>
    public required double Similarity { get; init; }

    /// <summary>1-based rank.</summary>
    public required int Rank { get; init; }
}

/// <summary>
/// Profile completeness assessment.
/// </summary>
public sealed record CompletenessScore
{
    /// <summary>Score 0-100.</summary>
    public required double Score { get; init; }

    /// <summary>Coverage percentage (detected / max cannabinoids).</summary>
    public required double Coverage { get; init; }

    /// <summary>Human-readable assessment.</summary>
    public required string Message { get; init; }
}

/// <summary>
/// Strain classification based on cannabinoid ratios.
/// </summary>
public sealed record StrainClassificationResult
{
    /// <summary>Dominance type.</summary>
    public required string Type { get; init; }

    /// <summary>THC content tier.</summary>
    public required string ThcContent { get; init; }

    /// <summary>CBD content tier.</summary>
    public required string CbdContent { get; init; }

    /// <summary>Human-readable ratio.</summary>
    public required string Ratio { get; init; }
}

/// <summary>
/// CDES analysis engine for comparing, scoring, and classifying cannabinoid profiles.
/// Ports the TypeScript CDESAnalyzer to .NET.
/// </summary>
public static class CdesAnalyzer
{
    /// <summary>
    /// Compare two cannabinoid profiles and return distance, similarity, and deltas.
    /// </summary>
    public static ComparisonResult CompareProfiles(
        CannabinoidProfile profile1,
        CannabinoidProfile profile2)
    {
        var distance = CannabinoidUtilities.CalculateEuclideanDistance(
            profile1.Cannabinoids,
            profile2.Cannabinoids);

        // Inverse exponential decay: similarity = 100 × e^(−distance/10)
        var similarity = Math.Max(0, Math.Min(100, 100 * Math.Exp(-distance / 10)));

        var allKeys = new HashSet<string>(profile1.Cannabinoids.Keys);
        allKeys.UnionWith(profile2.Cannabinoids.Keys);

        var differences = new Dictionary<string, CompoundDelta>();
        foreach (var key in allKeys)
        {
            profile1.Cannabinoids.TryGetValue(key, out var v1);
            profile2.Cannabinoids.TryGetValue(key, out var v2);

            if (Math.Abs(v1 - v2) > 1e-10)
            {
                differences[key] = new CompoundDelta(v1, v2, v2 - v1);
            }
        }

        return new ComparisonResult
        {
            Profile1 = profile1,
            Profile2 = profile2,
            Distance = distance,
            Similarity = similarity,
            Differences = differences,
        };
    }

    /// <summary>
    /// Find the most similar profiles from a list.
    /// </summary>
    /// <param name="target">Target profile to compare against.</param>
    /// <param name="candidates">Pool of candidate profiles.</param>
    /// <param name="limit">Maximum results to return.</param>
    /// <param name="minSimilarity">Minimum similarity score to include.</param>
    /// <returns>Ranked similarity results.</returns>
    public static IReadOnlyList<SimilarityResult> FindSimilarProfiles(
        CannabinoidProfile target,
        IEnumerable<CannabinoidProfile> candidates,
        int limit = 5,
        double minSimilarity = 50)
    {
        return candidates
            .Where(c => c.BatchId != target.BatchId) // exclude self
            .Select(c =>
            {
                var distance = CannabinoidUtilities.CalculateEuclideanDistance(
                    target.Cannabinoids, c.Cannabinoids);
                var similarity = Math.Max(0, Math.Min(100, 100 * Math.Exp(-distance / 10)));
                return (Profile: c, Distance: distance, Similarity: similarity);
            })
            .Where(r => r.Similarity >= minSimilarity)
            .OrderByDescending(r => r.Similarity)
            .Take(limit)
            .Select((r, i) => new SimilarityResult
            {
                Profile = r.Profile,
                Distance = r.Distance,
                Similarity = r.Similarity,
                Rank = i + 1,
            })
            .ToArray();
    }

    /// <summary>
    /// Score the completeness of a cannabinoid profile.
    /// </summary>
    public static CompletenessScore ScoreCompleteness(CannabinoidProfile profile)
    {
        var detected = profile.Cannabinoids.Count;
        var coverage = (double)detected / CannabinoidLibrary.MaxCannabinoids * 100;

        var score = coverage;
        if (detected >= 7) score += 20;
        if (profile.TotalCannabinoids > 10) score += 10;
        score = Math.Min(100, score);

        var message = coverage switch
        {
            < 30 => "Incomplete profile - limited cannabinoid data",
            < 60 => "Partial profile - some cannabinoid data",
            < 90 => "Good profile - most cannabinoids detected",
            _ => "Complete profile - all major cannabinoids detected",
        };

        return new CompletenessScore
        {
            Score = score,
            Coverage = coverage,
            Message = message,
        };
    }

    /// <summary>
    /// Classify a strain by its cannabinoid profile (THC-dominant, CBD-dominant, balanced).
    /// </summary>
    public static StrainClassificationResult ClassifyStrain(CannabinoidProfile profile)
    {
        profile.Cannabinoids.TryGetValue("THC", out var thc);
        profile.Cannabinoids.TryGetValue("CBD", out var cbd);

        var type = (thc, cbd) switch
        {
            _ when thc > cbd * 2 => "THC-Dominant",
            _ when cbd > thc * 2 => "CBD-Dominant",
            _ when thc > 0 || cbd > 0 => "Balanced",
            _ => "Unknown",
        };

        var thcContent = thc switch
        {
            > 20 => "High",
            > 10 => "Moderate",
            > 0.5 => "Low",
            _ => "Trace",
        };

        var cbdContent = cbd switch
        {
            > 15 => "High",
            > 7 => "Moderate",
            > 0.5 => "Low",
            _ => "Trace",
        };

        string ratio;
        if (thc > 0 && cbd > 0)
        {
            var ratioParts = Math.Max(thc, cbd) / Math.Min(thc, cbd);
            ratio = thc > cbd
                ? $"{ratioParts:F1}:1 THC:CBD"
                : $"{ratioParts:F1}:1 CBD:THC";
        }
        else if (thc > 0)
        {
            ratio = $"{thc:F1}% THC";
        }
        else if (cbd > 0)
        {
            ratio = $"{cbd:F1}% CBD";
        }
        else
        {
            ratio = "N/A";
        }

        return new StrainClassificationResult
        {
            Type = type,
            ThcContent = thcContent,
            CbdContent = cbdContent,
            Ratio = ratio,
        };
    }

    /// <summary>
    /// Convert a <see cref="CannabinoidProfile"/> into a <see cref="Batch"/> (compatibility helper).
    /// </summary>
    public static Batch ProfileToBatch(CannabinoidProfile profile)
    {
        var cannabinoids = profile.Cannabinoids
            .Select(kv => new Cannabinoid
            {
                Name = kv.Key,
                Percentage = kv.Value,
                Unit = CompoundUnit.Percent,
            })
            .ToArray();

        var labResult = new LabResult
        {
            Cannabinoids = cannabinoids,
            Terpenes = Array.Empty<Terpene>(),
            TestDate = DateTimeOffset.UtcNow,
        };

        return new Batch
        {
            Id = profile.BatchId,
            Name = profile.BatchName,
            Strain = profile.BatchName,
            DispensaryId = "unknown",
            ProcessedDate = DateTimeOffset.UtcNow,
            LabResult = labResult,
        };
    }
}
