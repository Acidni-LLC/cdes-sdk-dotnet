using Cdes.Models;
using Cdes.Serialization;
using System.Text.Json;

namespace Cdes.Tests.Serialization;

public class SerializationTests
{
    [Fact]
    public void Cannabinoid_RoundTrip()
    {
        var original = new Cannabinoid
        {
            Name = "THC",
            DisplayName = "Δ9-THC",
            Percentage = 25.5,
            Unit = CompoundUnit.Percent,
        };

        var json = CdesJsonOptions.Serialize(original);
        var deserialized = CdesJsonOptions.Deserialize<Cannabinoid>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(original.Name, deserialized!.Name);
        Assert.Equal(original.Percentage, deserialized.Percentage);
    }

    [Fact]
    public void Terpene_RoundTrip()
    {
        var original = new Terpene
        {
            Name = "Myrcene",
            DisplayName = "β-Myrcene",
            Percentage = 1.2,
            Unit = CompoundUnit.Percent,
        };

        var json = CdesJsonOptions.Serialize(original);
        var deserialized = CdesJsonOptions.Deserialize<Terpene>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(original.Name, deserialized!.Name);
        Assert.Equal(original.Percentage, deserialized.Percentage);
    }

    [Fact]
    public void Product_RoundTrip()
    {
        var original = new Product
        {
            Id = "prod-001",
            Name = "Blue Dream",
            Category = ProductCategory.Flower,
            ThcContent = 22.5,
            CbdContent = 0.1,
            Strain = new Strain
            {
                Name = "Blue Dream",
                Classification = StrainClassification.Hybrid,
            },
            Weight = new Weight { Value = 3.5, Unit = "g" },
            Price = new Price { Amount = 45.00m, Currency = "USD" },
        };

        var json = CdesJsonOptions.Serialize(original);
        var deserialized = CdesJsonOptions.Deserialize<Product>(json);

        Assert.NotNull(deserialized);
        Assert.Equal("prod-001", deserialized!.Id);
        Assert.Equal("Blue Dream", deserialized.Name);
        Assert.Equal(ProductCategory.Flower, deserialized.Category);
        Assert.Equal(22.5, deserialized.ThcContent);
    }

    [Fact]
    public void Serialize_UsesCamelCase()
    {
        var product = new Product
        {
            Id = "test",
            Name = "Test",
            Category = ProductCategory.Flower,
            ThcContent = 10.0,
        };

        var json = CdesJsonOptions.Serialize(product);

        Assert.Contains("\"thcContent\"", json);
        Assert.DoesNotContain("\"ThcContent\"", json);
    }

    [Fact]
    public void Serialize_OmitsNulls()
    {
        var product = new Product
        {
            Id = "test",
            Name = "Test",
            Category = ProductCategory.Flower,
        };

        var json = CdesJsonOptions.Serialize(product);

        // CbdContent is null and should be omitted
        Assert.DoesNotContain("\"cbdContent\"", json);
    }

    [Fact]
    public void SerializeIndented_ProducesFormattedOutput()
    {
        var terpene = new Terpene { Name = "Myrcene", Percentage = 1.0 };
        var json = CdesJsonOptions.SerializeIndented(terpene);

        Assert.Contains(Environment.NewLine, json);
    }

    [Fact]
    public void Deserialize_CaseInsensitivePropertyNames()
    {
        const string json = """{"Name":"THC","PERCENTAGE":25.0}""";

        var result = CdesJsonOptions.Deserialize<Cannabinoid>(json);

        Assert.NotNull(result);
        Assert.Equal("THC", result!.Name);
        Assert.Equal(25.0, result.Percentage);
    }

    [Fact]
    public void Deserialize_HandlesStringNumbers()
    {
        const string json = """{"name":"THC","percentage":"25.5"}""";

        var result = CdesJsonOptions.Deserialize<Cannabinoid>(json);

        Assert.NotNull(result);
        Assert.Equal(25.5, result!.Percentage);
    }

    [Fact]
    public void Enum_SerializesAsString()
    {
        var product = new Product
        {
            Id = "test",
            Name = "Test",
            Category = ProductCategory.Flower,
        };

        var json = CdesJsonOptions.Serialize(product);

        Assert.Contains("\"Flower\"", json);
        Assert.DoesNotContain("\"0\"", json);
    }

    [Fact]
    public void Batch_RoundTrip()
    {
        var original = new Batch
        {
            Id = "batch-001",
            Name = "OG Kush Lot 42",
            Strain = "OG Kush",
            DispensaryId = "disp-001",
            DispensaryName = "Cookies",
            ProcessedDate = DateTimeOffset.Parse("2026-01-15T00:00:00Z"),
            LabResult = new LabResult
            {
                LabName = "ACS Lab",
                TestDate = DateTimeOffset.Parse("2026-01-15T00:00:00Z"),
                Cannabinoids = new Cannabinoid[]
                {
                    new() { Name = "THC", Percentage = 28.5 },
                    new() { Name = "CBD", Percentage = 0.1 },
                },
                Terpenes = new Terpene[]
                {
                    new() { Name = "Myrcene", Percentage = 1.5 },
                    new() { Name = "Limonene", Percentage = 0.8 },
                },
            },
        };

        var json = CdesJsonOptions.Serialize(original);
        var deserialized = CdesJsonOptions.Deserialize<Batch>(json);

        Assert.NotNull(deserialized);
        Assert.Equal("batch-001", deserialized!.Id);
        Assert.Equal("OG Kush", deserialized.Strain);
        Assert.Equal(28.5, deserialized.LabResult?.Cannabinoids?.First(c => c.Name == "THC").Percentage);
    }
}
