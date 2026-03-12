using Cdes.Analysis;
using Cdes.Models;

namespace Cdes.Tests.Analysis;

public class CdesAnalyzerTests
{
    private static CannabinoidProfile CreateProfile(
        string id,
        Dictionary<string, double> cannabinoids) =>
        new()
        {
            BatchId = id,
            BatchName = $"Batch {id}",
            Cannabinoids = cannabinoids,
            TotalCannabinoids = cannabinoids.Values.Sum(),
        };

    // ── CompareProfiles ──────────────────────────────────────────────────

    [Fact]
    public void CompareProfiles_IdenticalProfiles_Returns100()
    {
        var a = CreateProfile("A", new() { ["THC"] = 25.0, ["CBD"] = 1.0 });
        var b = CreateProfile("B", new() { ["THC"] = 25.0, ["CBD"] = 1.0 });

        var result = CdesAnalyzer.CompareProfiles(a, b);

        Assert.Equal(100.0, result.Similarity, 1);
        Assert.Empty(result.Differences);
    }

    [Fact]
    public void CompareProfiles_CompletelyDifferent_ReturnsLowScore()
    {
        var a = CreateProfile("A", new() { ["THC"] = 30.0 });
        var b = CreateProfile("B", new() { ["CBD"] = 20.0 });

        var result = CdesAnalyzer.CompareProfiles(a, b);

        Assert.True(result.Similarity < 50.0);
    }

    [Fact]
    public void CompareProfiles_ReturnsDeltas()
    {
        var a = CreateProfile("A", new() { ["THC"] = 25.0 });
        var b = CreateProfile("B", new() { ["THC"] = 20.0 });

        var result = CdesAnalyzer.CompareProfiles(a, b);

        Assert.Single(result.Differences);
        Assert.True(result.Differences.ContainsKey("THC"));
        Assert.Equal(25.0, result.Differences["THC"].Value1);
        Assert.Equal(20.0, result.Differences["THC"].Value2);
        Assert.Equal(-5.0, result.Differences["THC"].Delta);
    }

    [Fact]
    public void CompareProfiles_EmptyProfiles_Returns100()
    {
        var a = CreateProfile("A", new());
        var b = CreateProfile("B", new());

        var result = CdesAnalyzer.CompareProfiles(a, b);
        Assert.Equal(100.0, result.Similarity, 1);
    }

    // ── FindSimilarProfiles ──────────────────────────────────────────────

    [Fact]
    public void FindSimilarProfiles_ExcludesSelf()
    {
        var target = CreateProfile("target", new() { ["THC"] = 25.0 });

        var others = new[]
        {
            target, // Same BatchId
            CreateProfile("other", new() { ["THC"] = 24.0 }),
        };

        var results = CdesAnalyzer.FindSimilarProfiles(target, others);
        Assert.All(results, r => Assert.NotEqual("target", r.Profile.BatchId));
    }

    [Fact]
    public void FindSimilarProfiles_RespectsMinSimilarity()
    {
        var target = CreateProfile("target", new() { ["THC"] = 25.0 });

        var others = new[]
        {
            CreateProfile("similar", new() { ["THC"] = 24.0 }),
            CreateProfile("different", new() { ["CBD"] = 20.0 }),
        };

        var results = CdesAnalyzer.FindSimilarProfiles(target, others, minSimilarity: 80.0);
        Assert.All(results, r => Assert.True(r.Similarity >= 80.0));
    }

    [Fact]
    public void FindSimilarProfiles_RespectsLimit()
    {
        var target = CreateProfile("target", new() { ["THC"] = 25.0 });

        var others = Enumerable.Range(1, 20)
            .Select(i => CreateProfile($"batch-{i}",
                new() { ["THC"] = 25.0 + i * 0.1 }))
            .ToList();

        var results = CdesAnalyzer.FindSimilarProfiles(target, others, limit: 5);
        Assert.True(results.Count <= 5);
    }

    [Fact]
    public void FindSimilarProfiles_ResultsAreSortedDescending()
    {
        var target = CreateProfile("target", new() { ["THC"] = 25.0 });

        var others = new[]
        {
            CreateProfile("a", new() { ["THC"] = 20.0 }),
            CreateProfile("b", new() { ["THC"] = 24.5 }),
            CreateProfile("c", new() { ["THC"] = 10.0 }),
        };

        var results = CdesAnalyzer.FindSimilarProfiles(target, others, minSimilarity: 0);

        for (var i = 1; i < results.Count; i++)
        {
            Assert.True(results[i].Similarity <= results[i - 1].Similarity);
        }
    }

    [Fact]
    public void FindSimilarProfiles_ResultsHaveRanks()
    {
        var target = CreateProfile("target", new() { ["THC"] = 25.0 });

        var others = new[]
        {
            CreateProfile("a", new() { ["THC"] = 24.0 }),
            CreateProfile("b", new() { ["THC"] = 23.0 }),
        };

        var results = CdesAnalyzer.FindSimilarProfiles(target, others, minSimilarity: 0);

        for (var i = 0; i < results.Count; i++)
        {
            Assert.Equal(i + 1, results[i].Rank);
        }
    }

    // ── ScoreCompleteness ────────────────────────────────────────────────

    [Fact]
    public void ScoreCompleteness_FullProfile_HighScore()
    {
        var profile = CreateProfile("full", new()
        {
            ["THC"] = 25.0, ["CBD"] = 1.0, ["CBN"] = 0.5,
            ["CBG"] = 0.3, ["CBC"] = 0.1, ["THCV"] = 0.2,
            ["CBDV"] = 0.1, ["CBDA"] = 0.5, ["THCA"] = 28.0,
            ["CBGA"] = 0.2, ["Delta-8 THC"] = 0.1,
        });

        var score = CdesAnalyzer.ScoreCompleteness(profile);

        Assert.True(score.Score > 70.0);
        Assert.True(score.Coverage > 90.0);
        Assert.Contains("complete", score.Message.ToLowerInvariant());
    }

    [Fact]
    public void ScoreCompleteness_EmptyProfile_ZeroScore()
    {
        var profile = CreateProfile("empty", new());

        var score = CdesAnalyzer.ScoreCompleteness(profile);

        Assert.Equal(0.0, score.Score);
        Assert.Equal(0.0, score.Coverage);
    }

    [Fact]
    public void ScoreCompleteness_SevenPlusCannabinoidsBonusApplied()
    {
        var withBonus = CreateProfile("bonus", new()
        {
            ["THC"] = 25.0, ["CBD"] = 1.0, ["CBN"] = 0.5,
            ["CBG"] = 0.3, ["CBC"] = 0.1, ["THCV"] = 0.2,
            ["CBDV"] = 0.1,
        });

        var withoutBonus = CreateProfile("no-bonus", new()
        {
            ["THC"] = 25.0, ["CBD"] = 1.0, ["CBN"] = 0.5,
            ["CBG"] = 0.3, ["CBC"] = 0.1,
        });

        var scoreBonus = CdesAnalyzer.ScoreCompleteness(withBonus);
        var scoreNoBonus = CdesAnalyzer.ScoreCompleteness(withoutBonus);

        Assert.True(scoreBonus.Score > scoreNoBonus.Score);
    }

    // ── ClassifyStrain ───────────────────────────────────────────────────

    [Fact]
    public void ClassifyStrain_HighThcLowCbd_ThcDominant()
    {
        var profile = CreateProfile("thc", new() { ["THC"] = 25.0, ["CBD"] = 0.5 });

        var result = CdesAnalyzer.ClassifyStrain(profile);

        Assert.Equal("THC-Dominant", result.Type);
        Assert.Equal("High", result.ThcContent);
        Assert.Equal("Trace", result.CbdContent);
    }

    [Fact]
    public void ClassifyStrain_HighCbdLowThc_CbdDominant()
    {
        var profile = CreateProfile("cbd", new() { ["THC"] = 0.5, ["CBD"] = 18.0 });

        var result = CdesAnalyzer.ClassifyStrain(profile);

        Assert.Equal("CBD-Dominant", result.Type);
    }

    [Fact]
    public void ClassifyStrain_SimilarLevels_Balanced()
    {
        var profile = CreateProfile("balanced", new() { ["THC"] = 10.0, ["CBD"] = 10.0 });

        var result = CdesAnalyzer.ClassifyStrain(profile);

        Assert.Equal("Balanced", result.Type);
    }

    [Fact]
    public void ClassifyStrain_NoData_Unknown()
    {
        var profile = CreateProfile("unknown", new());

        var result = CdesAnalyzer.ClassifyStrain(profile);

        Assert.Equal("Unknown", result.Type);
    }

    [Theory]
    [InlineData(25.0, "High")]
    [InlineData(15.0, "Moderate")]
    [InlineData(5.0, "Low")]
    [InlineData(0.1, "Trace")]
    public void ClassifyStrain_ThcContentTier_IsCorrect(double thc, string expectedTier)
    {
        var profile = CreateProfile("tier", new() { ["THC"] = thc, ["CBD"] = 0.1 });

        var result = CdesAnalyzer.ClassifyStrain(profile);

        Assert.Equal(expectedTier, result.ThcContent);
    }

    [Fact]
    public void ClassifyStrain_ReturnsRatioString()
    {
        var profile = CreateProfile("ratio", new() { ["THC"] = 20.0, ["CBD"] = 1.0 });

        var result = CdesAnalyzer.ClassifyStrain(profile);

        Assert.Contains("THC:CBD", result.Ratio);
    }

    // ── ProfileToBatch ───────────────────────────────────────────────────

    [Fact]
    public void ProfileToBatch_CreatesValidBatch()
    {
        var profile = CreateProfile("test-batch", new() { ["THC"] = 25.0, ["CBD"] = 1.0 });

        var batch = CdesAnalyzer.ProfileToBatch(profile);

        Assert.Equal("test-batch", batch.Id);
        Assert.Equal("Batch test-batch", batch.Name);
        Assert.NotNull(batch.LabResult);
        Assert.Contains(batch.LabResult!.Cannabinoids, c => c.Name == "THC" && c.Percentage == 25.0);
        Assert.Contains(batch.LabResult!.Cannabinoids, c => c.Name == "CBD" && c.Percentage == 1.0);
    }

    [Fact]
    public void ProfileToBatch_EmptyProfile_CreatesEmptyBatch()
    {
        var profile = CreateProfile("empty", new());

        var batch = CdesAnalyzer.ProfileToBatch(profile);

        Assert.NotNull(batch.LabResult);
        Assert.Empty(batch.LabResult!.Cannabinoids);
        Assert.Empty(batch.LabResult.Terpenes);
    }
}
