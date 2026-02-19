// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using System.Text.Json.Serialization;

namespace Cdes.Models;

/// <summary>
/// Parent/genetic strain information.
/// </summary>
public sealed record Genetics
{
    /// <summary>Mother strain name.</summary>
    public string? Mother { get; init; }

    /// <summary>Father strain name.</summary>
    public string? Father { get; init; }
}

/// <summary>
/// A cannabis strain definition.
/// </summary>
public sealed record Strain
{
    /// <summary>Strain name.</summary>
    public required string Name { get; init; }

    /// <summary>Classification (indica / sativa / hybrid / cbd).</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required StrainClassification Classification { get; init; }

    /// <summary>Parent/genetic lineage.</summary>
    public Genetics? Genetics { get; init; }

    /// <summary>Typical terpene profile for this strain.</summary>
    public IReadOnlyList<Terpene>? AverageTerpenes { get; init; }
}

/// <summary>
/// A CDES-compliant cannabis product.
/// </summary>
public sealed record Product
{
    /// <summary>Unique product identifier.</summary>
    public required string Id { get; init; }

    /// <summary>Product name.</summary>
    public required string Name { get; init; }

    /// <summary>Associated strain.</summary>
    public Strain? Strain { get; init; }

    /// <summary>Product category.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ProductCategory Category { get; init; }

    /// <summary>Product subcategory.</summary>
    public string? Subcategory { get; init; }

    /// <summary>THC percentage.</summary>
    public double? ThcContent { get; init; }

    /// <summary>CBD percentage.</summary>
    public double? CbdContent { get; init; }

    /// <summary>Terpene analysis results.</summary>
    public IReadOnlyList<Terpene>? TerpeneProfile { get; init; }

    /// <summary>Product weight.</summary>
    public Weight? Weight { get; init; }

    /// <summary>Retail price.</summary>
    public Price? Price { get; init; }

    /// <summary>Certificate of Analysis.</summary>
    public Coa? Coa { get; init; }
}
