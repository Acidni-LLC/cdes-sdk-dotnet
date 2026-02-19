using Cdes.Models;

namespace Cdes.Tests.Models;

public class CompoundsTests
{
    [Fact]
    public void Cannabinoid_RecordCreation()
    {
        var cannabinoid = new Cannabinoid
        {
            Name = "THC",
            DisplayName = "Δ9-THC",
            Percentage = 25.5,
            Unit = CompoundUnit.Percent,
        };

        Assert.Equal("THC", cannabinoid.Name);
        Assert.Equal("Δ9-THC", cannabinoid.DisplayName);
        Assert.Equal(25.5, cannabinoid.Percentage);
        Assert.Equal(CompoundUnit.Percent, cannabinoid.Unit);
    }

    [Fact]
    public void Terpene_RecordCreation()
    {
        var terpene = new Terpene
        {
            Name = "Myrcene",
            DisplayName = "β-Myrcene",
            Percentage = 1.2,
            Unit = CompoundUnit.Percent,
        };

        Assert.Equal("Myrcene", terpene.Name);
        Assert.Equal("β-Myrcene", terpene.DisplayName);
        Assert.Equal(1.2, terpene.Percentage);
    }

    [Fact]
    public void Weight_RecordCreation()
    {
        var weight = new Weight { Value = 3.5, Unit = "g" };
        Assert.Equal(3.5, weight.Value);
        Assert.Equal("g", weight.Unit);
    }

    [Fact]
    public void Price_RecordCreation()
    {
        var price = new Price { Amount = 45.00m, Currency = "USD" };
        Assert.Equal(45.00m, price.Amount);
        Assert.Equal("USD", price.Currency);
    }

    [Fact]
    public void Cannabinoid_RecordEquality()
    {
        var a = new Cannabinoid { Name = "THC", Percentage = 25.0 };
        var b = new Cannabinoid { Name = "THC", Percentage = 25.0 };
        Assert.Equal(a, b);
    }

    [Fact]
    public void Terpene_RecordInequality()
    {
        var a = new Terpene { Name = "Myrcene", Percentage = 1.0 };
        var b = new Terpene { Name = "Myrcene", Percentage = 2.0 };
        Assert.NotEqual(a, b);
    }
}
