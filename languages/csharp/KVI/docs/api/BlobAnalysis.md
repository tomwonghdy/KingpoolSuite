---
title: "BLOB Analysis"
description: "Overview of BLOB concepts and the KBlob class in KVI."
keywords: ["KVI", "KBlob", "BLOB", "connected component"]
---

# BLOB Analysis

## Overview

A **BLOB** (Binary Large Object) is a set of connected pixels with value 255 in a binary image, typically representing a foreground object. Through connected-component analysis, separate foreground regions can be labeled as independent BLOBs.

The `KBlob` class wraps an unmanaged BLOB handle from `blob.dll` (debug: `blob_d.dll`).

## Key Concepts

### BLOB Encoding Types

| Type | Description |
| :--- | :--- |
| **Raw** | Stores the entire BLOB region as a binary image |
| **Contour** | Stores only contour points; smaller data size |
| **Cluster** | Stores all foreground pixel positions as a point set |

### Distance Measures

- **Centroid distance**: Euclidean distance between centroids
- **Bounding rect center distance**: Based on axis-aligned bounding box
- **Minimum box center distance**: Based on rotated rectangle
- **Contour shortest distance**: Shortest distance between boundaries (currently unsupported)

## Capabilities

`KBlob` supports BLOB creation, conversion, cloning, merging, distance calculation, property queries, and conversion to/from images and masks.

## Detailed Reference

For constructors, methods, properties, and examples, see:

- [KBlob Class Usage Guide](KBlobClassUsageGuide.md)

## Related Documentation

- [Digital Image Analysis](DigitalImageAnalysis.md) — whole-image and mask-based analysis
- [Mask Class](MaskClass.md) — converting BLOBs to masks