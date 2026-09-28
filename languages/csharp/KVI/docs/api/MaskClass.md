---
title: "Mask Class"
description: "Overview of the KMask class and mask concepts in KVI."
keywords: ["KVI", "KMask", "mask", "region of interest"]
---

# Mask Class

## Overview

A **mask** is a binary template used to identify which elements in an image participate in analysis and processing. The basic unit is called an **element**, with each element being 0 or 1: 1 means valid, 0 means invalid.

Masks are divided into **area masks** and **line masks**. In the current version, only area masks are valid.

The `KMask` class wraps an unmanaged mask handle from `msk.dll` (debug: `msk_d.dll`).

## Key Concepts

### Area Masks

| Shape | Description |
| :--- | :--- |
| `Rect` | Rectangular area |
| `FilledEllipse` | Filled ellipse |
| `FullStrap` | Full strap |
| `Ring` | Ring or elliptical ring |

### Line Masks (Under Development)

`Line`, `Arc`, `Circle`, `Ellipse` — defined by coordinate point sequences.

### Merge Types

| Type | Description |
| :--- | :--- |
| `NotA` | Keep parts of B not in A |
| `NotB` | Keep parts of A not in B |
| `Both` | Intersection |
| `AnyOf` | Union |

## Capabilities

`KMask` supports mask creation, cloning, merging, scaling, logical operations, and conversion to/from images.

## Detailed Reference

For constructors, methods, properties, and examples, see:

- [KMask Class Detailed Function Usage Guide](KMaskClassDetailedFunctionUsageGuide.md)

## Related Documentation

- [BLOB Analysis](BlobAnalysis.md) — converting BLOBs to masks