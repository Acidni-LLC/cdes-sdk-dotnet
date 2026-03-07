# CDES .NET SDK

> **Cannabis Data Exchange Standard** — .NET 8+ implementation

[![NuGet](https://img.shields.io/nuget/v/Cdes.svg)](https://www.nuget.org/packages/Cdes/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

The official .NET SDK for the **Cannabis Data Exchange Standard (CDES)** — an open-source data interchange specification for the cannabis industry. This package provides strongly-typed models, reference data for 30 terpenes and 11 cannabinoids, analysis utilities, and JSON serialization helpers.

## Installation

```bash
dotnet add package Cdes
```

Or via the NuGet Package Manager:

```powershell
Install-Package Cdes
```

## Quick Start

```csharp
using Cdes.Models;
using Cdes.Reference;
using Cdes.Analysis;
using Cdes.Serialization;

// Create a batch with lab results
var batch = new Batch
{
    Id = "batch-001",
    Name = "Blue Dream Lot 42",
    Strain = new Strain
    {
        Name = "Blue Dream",
        Classification = StrainClassification.Hybrid,
    },
    LabResult = new LabResult
    {
        LabName = "ACS Laboratory",
        TestDate = "2026-01-15",
        Cannabinoids = new Dictionary<string, double>
        {
            ["THC"] = 24.5,
            ["CBD"] = 0.8,
            ["CBG"] = 0.3,
        },
        Terpenes = new Dictionary<string, double>
        {
            ["Myrcene"] = 1.2,
            ["Limonene"] = 0.9,
            ["Caryophyllene"] = 0.4,
        },
    },
};

// Classify the strain
var classification = CdesAnalyzer.ClassifyStrain(batch);
Console.WriteLine($"{classification.Classification} — THC Tier: {classification.ThcTier}");
// Output: THC-Dominant — THC Tier: High

// Score profile completeness
var completeness = CdesAnalyzer.ScoreCompleteness(batch);
Console.WriteLine($"Completeness: {completeness.Score:F1}% ({completeness.CannabinoidCount} cannabinoids, {completeness.TerpeneCount} terpenes)");

// Serialize to CDES JSON
var json = CdesJsonOptions.SerializeIndented(batch);
```

## Features

### Models

Strongly-typed C# records for all CDES data structures:

| Model | Description |
|-------|-------------|
| `Batch` | A batch of cannabis product with lab results |
| `Product` | A cannabis product listing |
| `Coa` | Certificate of Analysis with full test results |
| `Strain` | Strain information with genetics |
| `Cannabinoid` / `Terpene` | Compound measurement records |
| `LabResult` | Lab test results with compound profiles |

### Reference Data

Complete definitions for all CDES-standard compounds:

```csharp
// 30 standard terpenes with colors, aromas, therapeutic effects, boiling points
var myrcene = TerpeneLibrary.StandardTerpenes["β-Myrcene"];
Console.WriteLine($"{myrcene.DisplayName}: {myrcene.Hex}, {myrcene.BoilingPointC}°C");
Console.WriteLine($"Aroma: {string.Join(", ", myrcene.Aroma)}");
Console.WriteLine($"Effects: {string.Join(", ", myrcene.Therapeutic)}");

// 11 standard cannabinoids with thresholds and accessibility metadata
var thc = CannabinoidLibrary.StandardCannabinoids["THC"];
console.WriteLine($"{thc.Name}: {thc.Hex}, range {thc.ThresholdMin}-{thc.ThresholdMax}%");
```

### Terpene Utilities

```csharp
// Normalize common aliases to canonical CDES names
TerpeneUtilities.NormalizeName("myrcene");      // → "β-Myrcene"
TerpeneUtilities.NormalizeName("b-myrcene");     // → "β-Myrcene"
TerpeneUtilities.NormalizeName("beta-myrcene");  // → "β-Myrcene"

// Get colors for charting
var color = TerpeneUtilities.GetColorSafe("β-Myrcene");  // → "#FFFF00"
var palette = TerpeneUtilities.GetPalette(8);             // Top 8 terpene colors

// Search by therapeutic effect
var sedatives = TerpeneUtilities.FindByEffect("sedation");

// Filter by category
var monoterpenes = TerpeneUtilities.GetByCategory(TerpeneCategory.Monoterpene);
```

### Cannabinoid Utilities

```csharp
// Categorize potency
CannabinoidUtilities.GetCategory("THC", 28.0);  // → "very-high"
CannabinoidUtilities.GetCategory("THC", 15.0);  // → "medium"

// Calculate Euclidean distance between profiles
var distance = CannabinoidUtilities.CalculateEuclideanDistance(profileA, profileB);
```

### Analysis

```csharp
// Compare two batches
var comparison = CdesAnalyzer.CompareProfiles(batchA, batchB);
Console.WriteLine($"Similarity: {comparison.OverallSimilarity:F1}%");

// Find similar batches
var similar = CdesAnalyzer.FindSimilarProfiles(target, allBatches, minSimilarity: 70.0, limit: 10);

// Score profile completeness (out of 100)
var score = CdesAnalyzer.ScoreCompleteness(batch);

// Classify strain type
var classification = CdesAnalyzer.ClassifyStrain(batch);
```

### Serialization

Pre-configured `System.Text.Json` options for CDES-compliant JSON:

```csharp
// Serialize with camelCase, string enums, null omission
var json = CdesJsonOptions.Serialize(batch);

// Deserialize (case-insensitive, handles string numbers)
var batch = CdesJsonOptions.Deserialize<Batch>(json);

// Pretty-print
var indented = CdesJsonOptions.SerializeIndented(batch);
```

## API Reference

### Namespaces

| Namespace | Contents |
|-----------|----------|
| `Cdes.Models` | Core data models (Batch, Product, Coa, etc.) |
| `Cdes.Reference` | Terpene/Cannabinoid definitions and libraries |
| `Cdes.Analysis` | Profile comparison, similarity, completeness scoring |
| `Cdes.Serialization` | JSON serialization helpers |

### Key Types

| Type | Kind | Description |
|------|------|-------------|
| `TerpeneLibrary` | static class | 30 standard terpene definitions |
| `CannabinoidLibrary` | static class | 11 standard cannabinoid definitions |
| `TerpeneUtilities` | static class | Name normalization, color lookup, search |
| `CannabinoidUtilities` | static class | Normalization, categorization, distance |
| `CdesAnalyzer` | static class | Profile comparison and classification |
| `CdesJsonOptions` | static class | Pre-configured JSON serialization |

## Related SDKs

| Language | Package | Repository |
|----------|---------|------------|
| TypeScript | `@cdes/sdk` | [cdes-sdk-typescript](https://github.com/Acidni-LLC/cdes-sdk-typescript) |
| Python | `cdes` | [cdes-python](https://github.com/Acidni-LLC/cdes-python) |
| **.NET** | **`Cdes`** | **This repository** |

## License

MIT — see [LICENSE](LICENSE) for details.

---

*Built by [Acidni LLC](https://www.acidni.com) — Advancing cannabis data standards.*
