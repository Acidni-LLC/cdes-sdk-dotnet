using Cdes.Reference;

namespace Cdes.Tests.Reference;

public class TerpeneLibraryTests
{
    [Fact]
    public void StandardTerpenes_Contains30Entries()
    {
        Assert.Equal(30, TerpeneLibrary.StandardTerpenes.Count);
    }

    [Theory]
    [InlineData("β-Myrcene")]
    [InlineData("δ-Limonene")]
    [InlineData("β-Caryophyllene")]
    [InlineData("α-Pinene")]
    [InlineData("Linalool")]
    [InlineData("α-Humulene")]
    [InlineData("Terpinolene")]
    [InlineData("Ocimene")]
    public void StandardTerpenes_ContainsPrimaryEight(string name)
    {
        Assert.True(TerpeneLibrary.StandardTerpenes.ContainsKey(name));
    }

    [Fact]
    public void InDisplayOrder_IsSorted()
    {
        var ordered = TerpeneLibrary.InDisplayOrder;

        for (var i = 1; i < ordered.Count; i++)
        {
            Assert.True(
                ordered[i].DisplayOrder >= ordered[i - 1].DisplayOrder,
                $"DisplayOrder at index {i} ({ordered[i].DisplayOrder}) should be >= index {i - 1} ({ordered[i - 1].DisplayOrder})");
        }
    }

    [Fact]
    public void InDisplayOrder_HasSameCountAsStandard()
    {
        Assert.Equal(
            TerpeneLibrary.StandardTerpenes.Count,
            TerpeneLibrary.InDisplayOrder.Count);
    }

    [Fact]
    public void ColorPalette_HasSameCountAsStandard()
    {
        Assert.Equal(
            TerpeneLibrary.StandardTerpenes.Count,
            TerpeneLibrary.ColorPalette.Count);
    }

    [Fact]
    public void Myrcene_HasCorrectProperties()
    {
        var myrcene = TerpeneLibrary.StandardTerpenes["β-Myrcene"];

        Assert.Equal(1, myrcene.Id);
        Assert.Equal("β-Myrcene", myrcene.CanonicalName);
        Assert.Equal("Myrcene", myrcene.DisplayName);
        Assert.Equal(TerpeneCategory.Monoterpene, myrcene.Category);
        Assert.Equal(1, (int)myrcene.DisplayOrder);
        Assert.Equal("#FFFF00", myrcene.Hex);
        Assert.Equal(167, (int)(myrcene.BoilingPointC ?? 0));
    }

    [Fact]
    public void Limonene_HasCorrectHex()
    {
        var limonene = TerpeneLibrary.StandardTerpenes["δ-Limonene"];
        Assert.Equal("#66CCFF", limonene.Hex);
        Assert.Equal(2, (int)limonene.DisplayOrder);
    }

    [Fact]
    public void AllDefinitions_HaveUniqueIds()
    {
        var ids = TerpeneLibrary.StandardTerpenes.Values
            .Select(t => t.Id)
            .ToList();

        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void AllDefinitions_HaveNonEmptyAliases()
    {
        foreach (var (name, def) in TerpeneLibrary.StandardTerpenes)
        {
            Assert.NotNull(def.Aliases);
            Assert.True(def.Aliases.Count > 0, $"{name} should have at least one alias");
        }
    }

    [Fact]
    public void AllDefinitions_HaveBoilingPoints()
    {
        foreach (var (name, def) in TerpeneLibrary.StandardTerpenes)
        {
            Assert.True(def.BoilingPointC > 0, $"{name} should have a positive boiling point");
        }
    }

    [Fact]
    public void AllDefinitions_HaveValidHexColors()
    {
        foreach (var (name, def) in TerpeneLibrary.StandardTerpenes)
        {
            Assert.Matches(@"^#[0-9A-Fa-f]{6}$", def.Hex);
        }
    }
}
