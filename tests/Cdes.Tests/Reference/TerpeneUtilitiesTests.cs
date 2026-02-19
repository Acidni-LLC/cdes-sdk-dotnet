using Cdes.Reference;

namespace Cdes.Tests.Reference;

public class TerpeneUtilitiesTests
{
    [Theory]
    [InlineData("b-myrcene", "β-Myrcene")]
    [InlineData("beta-myrcene", "β-Myrcene")]
    [InlineData("myrcene", "β-Myrcene")]
    [InlineData("d-limonene", "δ-Limonene")]
    [InlineData("delta-limonene", "δ-Limonene")]
    [InlineData("limonene", "δ-Limonene")]
    [InlineData("b-caryophyllene", "β-Caryophyllene")]
    [InlineData("caryophyllene", "β-Caryophyllene")]
    [InlineData("a-pinene", "α-Pinene")]
    [InlineData("alpha-pinene", "α-Pinene")]
    public void NormalizeName_ResolvesCommonAliases(string alias, string expected)
    {
        Assert.Equal(expected, TerpeneUtilities.NormalizeName(alias));
    }

    [Fact]
    public void NormalizeName_ReturnsOriginalForUnknown()
    {
        Assert.Equal("UnknownTerpene", TerpeneUtilities.NormalizeName("UnknownTerpene"));
    }

    [Fact]
    public void NormalizeName_IsCaseInsensitive()
    {
        Assert.Equal("β-Myrcene", TerpeneUtilities.NormalizeName("MYRCENE"));
        Assert.Equal("β-Myrcene", TerpeneUtilities.NormalizeName("Myrcene"));
        Assert.Equal("β-Myrcene", TerpeneUtilities.NormalizeName("myrcene"));
    }

    [Theory]
    [InlineData("β-Myrcene", "#FFFF00")]
    [InlineData("δ-Limonene", "#66CCFF")]
    [InlineData("β-Caryophyllene", "#92D050")]
    [InlineData("Linalool", "#FF7C80")]
    public void GetColor_ReturnsCorrectHex(string name, string expectedHex)
    {
        var color = TerpeneUtilities.GetColor(name);
        Assert.Equal(expectedHex, color);
    }

    [Fact]
    public void GetColor_ReturnsNullForUnknown()
    {
        Assert.Null(TerpeneUtilities.GetColor("FakeTerpene"));
    }

    [Fact]
    public void GetColorSafe_ReturnsFallbackForUnknown()
    {
        Assert.Equal(TerpeneLibrary.UnknownTerpeneColor, TerpeneUtilities.GetColorSafe("FakeTerpene"));
    }

    [Fact]
    public void GetDefinition_ReturnsDefinitionForKnown()
    {
        var def = TerpeneUtilities.GetDefinition("β-Myrcene");
        Assert.NotNull(def);
        Assert.Equal(1, def!.Id);
    }

    [Fact]
    public void GetDefinition_ReturnsNullForUnknown()
    {
        Assert.Null(TerpeneUtilities.GetDefinition("Unobtanium"));
    }

    [Theory]
    [InlineData("β-Myrcene", true)]
    [InlineData("myrcene", true)]
    [InlineData("Linalool", true)]
    [InlineData("FakeTerpene", false)]
    public void IsRecognized_IdentifiesKnownTerpenes(string name, bool expected)
    {
        Assert.Equal(expected, TerpeneUtilities.IsRecognized(name));
    }

    [Fact]
    public void GetAllNames_ReturnsAllCanonicalNames()
    {
        var names = TerpeneUtilities.GetAllNames();
        Assert.Equal(30, names.Count);
        Assert.Contains("β-Myrcene", names);
        Assert.Contains("Geranyl Acetate", names);
    }

    [Fact]
    public void GetByCategory_ReturnsCorrectSubset()
    {
        var monoterpenes = TerpeneUtilities.GetByCategory(TerpeneCategory.Monoterpene);
        Assert.All(monoterpenes, t => Assert.Equal(TerpeneCategory.Monoterpene, t.Category));
        Assert.True(monoterpenes.Count > 0);
    }

    [Fact]
    public void FindByEffect_ReturnsMatchingTerpenes()
    {
        var sedatives = TerpeneUtilities.FindByEffect("sedation");
        Assert.NotEmpty(sedatives);
        Assert.All(sedatives, t =>
            Assert.Contains("Sedation", t.Therapeutic, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetRgb_ReturnsCorrectValues()
    {
        var rgb = TerpeneUtilities.GetRgb("β-Myrcene");
        Assert.NotNull(rgb);
        Assert.Equal(255, rgb!.Value.R);
        Assert.Equal(255, rgb.Value.G);
        Assert.Equal(0, rgb.Value.B);
    }

    [Fact]
    public void GetPalette_ReturnsColorsInOrder()
    {
        var palette = TerpeneUtilities.GetPalette(3);
        Assert.Equal(3, palette.Count);
        // First three in display order: Myrcene, Limonene, Caryophyllene
        Assert.Equal("#FFFF00", palette[0]);
        Assert.Equal("#66CCFF", palette[1]);
        Assert.Equal("#92D050", palette[2]);
    }
}
