using Cdes.Models;

namespace Cdes.Tests.Models;

public class EnumsTests
{
    [Theory]
    [InlineData(ProductCategory.Flower, "Flower")]
    [InlineData(ProductCategory.Concentrate, "Concentrate")]
    [InlineData(ProductCategory.Edible, "Edible")]
    [InlineData(ProductCategory.Topical, "Topical")]
    [InlineData(ProductCategory.Tincture, "Tincture")]
    [InlineData(ProductCategory.Vape, "Vape")]
    [InlineData(ProductCategory.PreRoll, "PreRoll")]
    [InlineData(ProductCategory.Capsule, "Capsule")]
    [InlineData(ProductCategory.Other, "Other")]
    public void ProductCategory_HasExpectedName(ProductCategory category, string name)
    {
        Assert.Equal(name, category.ToString());
    }

    [Fact]
    public void ProductCategory_HasNineValues()
    {
        var values = Enum.GetValues<ProductCategory>();
        Assert.Equal(9, values.Length);
    }

    [Theory]
    [InlineData(StrainClassification.Sativa)]
    [InlineData(StrainClassification.Indica)]
    [InlineData(StrainClassification.Hybrid)]
    [InlineData(StrainClassification.Cbd)]
    public void StrainClassification_ContainsExpectedValues(StrainClassification classification)
    {
        Assert.True(Enum.IsDefined(classification));
    }

    [Theory]
    [InlineData(CompoundUnit.Percent)]
    [InlineData(CompoundUnit.MgPerG)]
    [InlineData(CompoundUnit.Mg)]
    public void CompoundUnit_ContainsExpectedValues(CompoundUnit unit)
    {
        Assert.True(Enum.IsDefined(unit));
    }

    [Fact]
    public void TestStatus_HasSevenValues()
    {
        Assert.Equal(7, Enum.GetValues<TestStatus>().Length);
    }

    [Fact]
    public void CoaStatus_HasFourValues()
    {
        Assert.Equal(4, Enum.GetValues<CoaStatus>().Length);
    }

    [Fact]
    public void TestMethod_HasSixValues()
    {
        Assert.Equal(6, Enum.GetValues<TestMethod>().Length);
    }
}
