using Cdes.Reference;

namespace Cdes.Tests.Reference;

public class CannabinoidLibraryTests
{
    [Fact]
    public void StandardCannabinoids_ContainsElevenEntries()
    {
        Assert.Equal(11, CannabinoidLibrary.StandardCannabinoids.Count);
    }

    [Fact]
    public void MaxCannabinoids_IsEleven()
    {
        Assert.Equal(11, CannabinoidLibrary.MaxCannabinoids);
    }

    [Theory]
    [InlineData("THC")]
    [InlineData("CBD")]
    [InlineData("CBN")]
    [InlineData("CBG")]
    [InlineData("CBC")]
    [InlineData("THCV")]
    [InlineData("CBDV")]
    [InlineData("CBDA")]
    [InlineData("THCA")]
    [InlineData("CBGA")]
    [InlineData("Delta-8 THC")]
    public void StandardCannabinoids_ContainsAllExpected(string name)
    {
        Assert.True(CannabinoidLibrary.StandardCannabinoids.ContainsKey(name));
    }

    [Fact]
    public void THC_HasCorrectProperties()
    {
        var thc = CannabinoidLibrary.StandardCannabinoids["THC"];

        Assert.Equal("THC", thc.Name);
        Assert.Equal("#B71C1C", thc.Hex);
        Assert.Equal(0, thc.ThresholdMin);
        Assert.Equal(35, thc.ThresholdMax);
    }

    [Fact]
    public void CBD_HasCorrectProperties()
    {
        var cbd = CannabinoidLibrary.StandardCannabinoids["CBD"];

        Assert.Equal("CBD", cbd.Name);
        Assert.Equal("#1565C0", cbd.Hex);
        Assert.Equal(0, cbd.ThresholdMin);
        Assert.Equal(25, cbd.ThresholdMax);
    }

    [Fact]
    public void AllNames_HasElevenEntries()
    {
        Assert.Equal(11, CannabinoidLibrary.AllNames.Count);
    }

    [Fact]
    public void ColorPalette_HasElevenEntries()
    {
        Assert.Equal(11, CannabinoidLibrary.ColorPalette.Count);
    }

    [Fact]
    public void AllDefinitions_HaveValidHexColors()
    {
        foreach (var (name, def) in CannabinoidLibrary.StandardCannabinoids)
        {
            Assert.Matches(@"^#[0-9A-Fa-f]{6}$", def.Hex);
        }
    }

    [Fact]
    public void AllDefinitions_HaveValidThresholds()
    {
        foreach (var (name, def) in CannabinoidLibrary.StandardCannabinoids)
        {
            Assert.True(def.ThresholdMin >= 0, $"{name} min threshold should be >= 0");
            Assert.True(def.ThresholdMax > def.ThresholdMin, $"{name} max should be > min");
        }
    }
}
