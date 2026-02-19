// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// =============================================================================

using System.Text.Json.Serialization;

namespace Cdes.Models;

/// <summary>
/// Testing laboratory information.
/// </summary>
public sealed record LabInfo
{
    /// <summary>Laboratory name.</summary>
    public required string Name { get; init; }

    /// <summary>State license number.</summary>
    public string? LicenseNumber { get; init; }

    /// <summary>Contact phone.</summary>
    public string? Phone { get; init; }

    /// <summary>Lab website URL.</summary>
    public string? Website { get; init; }

    /// <summary>Accreditations (e.g. ISO 17025).</summary>
    public IReadOnlyList<string>? Accreditations { get; init; }
}

/// <summary>
/// Sample information on a COA.
/// </summary>
public sealed record SampleInfo
{
    /// <summary>Batch number / lot number.</summary>
    public required string BatchNumber { get; init; }

    /// <summary>Product name as submitted.</summary>
    public required string ProductName { get; init; }

    /// <summary>Lab-assigned sample identifier.</summary>
    public string? SampleId { get; init; }

    /// <summary>Strain name.</summary>
    public string? StrainName { get; init; }

    /// <summary>Product type.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProductCategory? ProductType { get; init; }

    /// <summary>Date sample was received by lab.</summary>
    public DateOnly? ReceivedDate { get; init; }

    /// <summary>Date sample was tested.</summary>
    public DateOnly? TestedDate { get; init; }

    /// <summary>Harvest date.</summary>
    public DateOnly? HarvestDate { get; init; }

    /// <summary>Producer / cultivator name.</summary>
    public string? ProducerName { get; init; }

    /// <summary>Producer license number.</summary>
    public string? ProducerLicense { get; init; }
}

/// <summary>
/// A single safety analyte test result.
/// </summary>
public sealed record SafetyTestResult
{
    /// <summary>Analyte name.</summary>
    public required string Analyte { get; init; }

    /// <summary>Numeric result value.</summary>
    public double? Result { get; init; }

    /// <summary>Text representation of the result.</summary>
    public string? ResultText { get; init; }

    /// <summary>Action limit.</summary>
    public double? Limit { get; init; }

    /// <summary>Unit of measure.</summary>
    public string? Unit { get; init; }

    /// <summary>Pass/fail status.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required TestStatus Status { get; init; }
}

/// <summary>
/// A category of safety tests (e.g. pesticides, heavy metals).
/// </summary>
public sealed record SafetyTestCategory
{
    /// <summary>Overall category status.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required TestStatus Status { get; init; }

    /// <summary>Individual analyte results.</summary>
    public IReadOnlyList<SafetyTestResult>? Analytes { get; init; }
}

/// <summary>
/// All safety and compliance test results.
/// </summary>
public sealed record SafetyTests
{
    public SafetyTestCategory? Microbials { get; init; }
    public SafetyTestCategory? Pesticides { get; init; }
    public SafetyTestCategory? HeavyMetals { get; init; }
    public SafetyTestCategory? ResidualSolvents { get; init; }
    public SafetyTestCategory? Mycotoxins { get; init; }
    public SafetyTestCategory? Moisture { get; init; }
    public SafetyTestCategory? WaterActivity { get; init; }
    public SafetyTestCategory? ForeignMatter { get; init; }
}

/// <summary>
/// Potency test summary.
/// </summary>
public sealed record PotencyResults
{
    /// <summary>Potency test status.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required TestStatus Status { get; init; }

    /// <summary>Total THC (all forms combined) %.</summary>
    public double? TotalThc { get; init; }

    /// <summary>Total CBD (all forms combined) %.</summary>
    public double? TotalCbd { get; init; }

    /// <summary>Total cannabinoid content %.</summary>
    public double? TotalCannabinoids { get; init; }
}

/// <summary>
/// Certificate of Analysis (COA) — Full lab report (CDES v1.0).
/// A comprehensive lab test report for a cannabis sample, including
/// cannabinoid and terpene profiles, potency, and safety test results.
/// </summary>
public sealed record Coa
{
    /// <summary>Unique COA / report identifier.</summary>
    public required string Id { get; init; }

    /// <summary>Testing laboratory information.</summary>
    public required LabInfo Lab { get; init; }

    /// <summary>Sample / product information.</summary>
    public required SampleInfo Sample { get; init; }

    /// <summary>Overall pass/fail status.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required CoaStatus OverallStatus { get; init; }

    /// <summary>Lab-assigned COA number.</summary>
    public string? CoaNumber { get; init; }

    /// <summary>Cannabinoid analysis results.</summary>
    public IReadOnlyList<Cannabinoid>? Cannabinoids { get; init; }

    /// <summary>Terpene analysis results.</summary>
    public IReadOnlyList<Terpene>? Terpenes { get; init; }

    /// <summary>Potency summary.</summary>
    public PotencyResults? PotencyResults { get; init; }

    /// <summary>Safety / compliance tests.</summary>
    public SafetyTests? SafetyTests { get; init; }

    /// <summary>Report issue date.</summary>
    public DateOnly? IssuedDate { get; init; }

    /// <summary>Report expiration date.</summary>
    public DateOnly? ExpirationDate { get; init; }

    /// <summary>URL to the PDF report.</summary>
    public string? PdfUrl { get; init; }

    /// <summary>QR code value or URL.</summary>
    public string? QrCode { get; init; }

    /// <summary>Free-text notes.</summary>
    public string? Notes { get; init; }

    /// <summary>Analytical method used.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TestMethod? TestMethod { get; init; }

    /// <summary>Additional metadata.</summary>
    public IDictionary<string, object>? Metadata { get; init; }
}
