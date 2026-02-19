using Cdes.Reference;

namespace Cdes.Tests.Reference;

public class CannabinoidUtilitiesTests
{
    [Theory]
    [InlineData("thc", "THC")]
    [InlineData("cbd", "CBD")]
    [InlineData("delta-9-thc", "DELTA")]
    [InlineData("Δ9-THC", "Δ9THC")]
    public void NormalizeName_NormalizesCorrectly(string input, string expected)
    {
        Assert.Equal(expected, CannabinoidUtilities.NormalizeName(input));
    }

    [Theory]
    [InlineData("THC", "#E74C3C")]
    [InlineData("CBD", "#3498DB")]
    [InlineData("CBN", "#E67E22")]
    public void GetColor_ReturnsCorrectHex(string name, string expectedHex)
    {
        Assert.Equal(expectedHex, CannabinoidUtilities.GetColor(name));
    }

    [Fact]
    public void GetColor_ReturnsNullForUnknown()
    {
        Assert.Null(CannabinoidUtilities.GetColor("FAKE"));
    }

    [Fact]
    public void GetColorSafe_ReturnsFallbackForUnknown()
    {
        Assert.Equal(CannabinoidLibrary.UnknownCannabinoidColor,
            CannabinoidUtilities.GetColorSafe("FAKE"));
    }

    [Fact]
    public void GetDefinition_FindsByExactMatch()
    {
        var def = CannabinoidUtilities.GetDefinition("THC");
        Assert.NotNull(def);
        Assert.Equal("THC", def!.Name);
    }

    [Fact]
    public void GetDefinition_FindsByPartialMatch()
    {
        var def = CannabinoidUtilities.GetDefinition("Δ9-THC");
        Assert.NotNull(def);
        Assert.Equal("THC", def!.Name);
    }

    [Fact]
    public void GetDefinition_ReturnsNullForUnknown()
    {
        Assert.Null(CannabinoidUtilities.GetDefinition("ZZZZZ"));
    }

    [Theory]
    [InlineData("THC", 0.0, "low")]
    [InlineData("THC", 5.0, "low")]
    [InlineData("THC", 12.0, "medium")]
    [InlineData("THC", 20.0, "high")]
    [InlineData("THC", 30.0, "very-high")]
    public void GetCategory_ClassifiesCorrectly(string name, double percentage, string expected)
    {
        Assert.Equal(expected, CannabinoidUtilities.GetCategory(name, percentage));
    }

    [Theory]
    [InlineData(0.0, "0.00%")]
    [InlineData(25.5, "25.50%")]
    [InlineData(100.0, "100.00%")]
    public void FormatValue_FormatsPercentage(double value, string expected)
    {
        Assert.Equal(expected, CannabinoidUtilities.FormatValue(value));
    }

    [Fact]
    public void CalculateTotal_SumsAllValues()
    {
        var profile = new Dictionary<string, double>
        {
            ["THC"] = 25.0,
            ["CBD"] = 5.0,
            ["CBN"] = 1.0,
        };

        Assert.Equal(31.0, CannabinoidUtilities.CalculateTotal(profile));
    }

    [Fact]
    public void CalculateTotal_ReturnsZeroForEmpty()
    {
        Assert.Equal(0.0, CannabinoidUtilities.CalculateTotal(new Dictionary<string, double>()));
    }

    [Fact]
    public void CalculateEuclideanDistance_IdenticalProfiles_ReturnsZero()
    {
        var a = new Dictionary<string, double> { ["THC"] = 25.0, ["CBD"] = 5.0 };
        var b = new Dictionary<string, double> { ["THC"] = 25.0, ["CBD"] = 5.0 };

        Assert.Equal(0.0, CannabinoidUtilities.CalculateEuclideanDistance(a, b), 4);
    }

    [Fact]
    public void CalculateEuclideanDistance_DifferentProfiles_ReturnsPositive()
    {
        var a = new Dictionary<string, double> { ["THC"] = 25.0, ["CBD"] = 5.0 };
        var b = new Dictionary<string, double> { ["THC"] = 20.0, ["CBD"] = 10.0 };

        var distance = CannabinoidUtilities.CalculateEuclideanDistance(a, b);
        Assert.True(distance > 0);
        // Expected: sqrt((25-20)^2 + (5-10)^2) = sqrt(25+25) = sqrt(50) ≈ 7.071
        Assert.Equal(Math.Sqrt(50), distance, 4);
    }

    [Fact]
    public void CalculateEuclideanDistance_MissingKeys_TreatedAsZero()
    {
        var a = new Dictionary<string, double> { ["THC"] = 10.0 };
        var b = new Dictionary<string, double> { ["CBD"] = 10.0 };

        // sqrt((10-0)^2 + (0-10)^2) = sqrt(200) ≈ 14.142
        var distance = CannabinoidUtilities.CalculateEuclideanDistance(a, b);
        Assert.Equal(Math.Sqrt(200), distance, 4);
    }
}
