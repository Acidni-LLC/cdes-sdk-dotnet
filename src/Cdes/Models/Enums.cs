// =============================================================================
// CDES .NET SDK — Cannabis Data Exchange Standard
// Copyright (c) 2026 Acidni LLC. Licensed under MIT.
// https://github.com/Acidni-LLC/cdes-sdk-dotnet
// =============================================================================

namespace Cdes.Models;

/// <summary>
/// Product category in the cannabis industry.
/// </summary>
public enum ProductCategory
{
    Flower,
    Concentrate,
    Edible,
    Topical,
    PreRoll,
    Vape,
    Tincture,
    Capsule,
    Other
}

/// <summary>
/// Strain classification.
/// </summary>
public enum StrainClassification
{
    Indica,
    Sativa,
    Hybrid,
    Cbd
}

/// <summary>
/// Measurement unit for compound quantities.
/// </summary>
public enum CompoundUnit
{
    /// <summary>Percentage (0-100).</summary>
    Percent,
    /// <summary>Milligrams per gram.</summary>
    MgPerG,
    /// <summary>Milligrams.</summary>
    Mg
}

/// <summary>
/// Safety test result status.
/// </summary>
public enum TestStatus
{
    Pass,
    Fail,
    Pending,
    NotTested,
    NotApplicable,
    Detected,
    NotDetected
}

/// <summary>
/// Overall COA status.
/// </summary>
public enum CoaStatus
{
    Pass,
    Fail,
    Pending,
    Partial
}

/// <summary>
/// Analytical test method used by a laboratory.
/// </summary>
public enum TestMethod
{
    HPLC,
    GC_MS,
    LC_MS,
    GC_FID,
    ICP_MS,
    PCR
}
