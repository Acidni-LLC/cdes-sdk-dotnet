// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

namespace Cdes.Models;

/// <summary>
/// A batch represents a single production lot or dispensary menu entry.
/// </summary>
public sealed record Batch
{
    /// <summary>Unique batch identifier.</summary>
    public required string Id { get; init; }

    /// <summary>Product / batch name.</summary>
    public required string Name { get; init; }

    /// <summary>Strain name.</summary>
    public required string Strain { get; init; }

    /// <summary>Source dispensary identifier.</summary>
    public required string DispensaryId { get; init; }

    /// <summary>Source dispensary name.</summary>
    public string? DispensaryName { get; init; }

    /// <summary>Date the batch was processed / ingested.</summary>
    public required DateTimeOffset ProcessedDate { get; init; }

    /// <summary>Lab test results for this batch.</summary>
    public LabResult? LabResult { get; init; }

    /// <summary>Additional metadata.</summary>
    public IDictionary<string, object>? Metadata { get; init; }
}

/// <summary>
/// Lab result summary attached to a batch (simplified view of a COA).
/// </summary>
public sealed record LabResult
{
    /// <summary>Cannabinoid analysis.</summary>
    public required IReadOnlyList<Cannabinoid> Cannabinoids { get; init; }

    /// <summary>Terpene analysis.</summary>
    public required IReadOnlyList<Terpene> Terpenes { get; init; }

    /// <summary>Date of analysis.</summary>
    public required DateTimeOffset TestDate { get; init; }

    /// <summary>Expiration date of the lab certificate.</summary>
    public DateTimeOffset? ExpirationDate { get; init; }

    /// <summary>Lab identifier.</summary>
    public string? LabId { get; init; }

    /// <summary>Lab name.</summary>
    public string? LabName { get; init; }

    /// <summary>Test method used.</summary>
    public TestMethod? TestMethod { get; init; }
}

/// <summary>
/// A cannabinoid profile comparison snapshot.
/// </summary>
public sealed record CannabinoidProfile
{
    /// <summary>Source batch identifier.</summary>
    public required string BatchId { get; init; }

    /// <summary>Source batch name.</summary>
    public required string BatchName { get; init; }

    /// <summary>Cannabinoid name → percentage map.</summary>
    public required IReadOnlyDictionary<string, double> Cannabinoids { get; init; }

    /// <summary>Sum of all cannabinoid percentages.</summary>
    public double TotalCannabinoids { get; init; }
}

/// <summary>
/// A terpene profile comparison snapshot.
/// </summary>
public sealed record TerpeneProfile
{
    /// <summary>Source batch identifier.</summary>
    public required string BatchId { get; init; }

    /// <summary>Source batch name.</summary>
    public required string BatchName { get; init; }

    /// <summary>Terpene name → percentage map.</summary>
    public required IReadOnlyDictionary<string, double> Terpenes { get; init; }

    /// <summary>Sum of all terpene percentages.</summary>
    public double TotalTerpenes { get; init; }
}
