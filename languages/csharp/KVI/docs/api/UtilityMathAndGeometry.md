---
title: "Utility Math and Geometry"
description: "Overview of the Smath static class for geometric calculations in KVI."
keywords: ["KVI", "Smath", "geometry", "polygon", "rectangle"]
---

# Utility Math and Geometry

## Overview

The `Smath` static class in the `Kingpool.Utility` namespace provides math and geometry computation functions, wrapping the native library `smath.dll` (debug: `smath_d.dll`).

It is a foundational tool for geometric measurement, shape analysis, and coordinate processing.

## Capabilities

- **Arc and ellipse**: Arc center from three points, ellipse fitting, point generation
- **Point and line**: Rotation, perpendicular foot, intersection, angle, distance, parallel/perpendicular derivation, extension/trimming
- **Polygon and polyline**: Area, length, center, offset, point-in-polygon, sub-polygon extraction
- **Rectangle and box**: Conversion, intersection, union, scaling, rotation, containment
- **Pixel helpers**: Row stride, depth, brightness adjustment, Otsu threshold
- **MIP scaling**: Up/down scale ratios by level

## General Conventions

- Most methods provide int, float (F32), and double (F64) overloads.
- Methods returning `bool` must have their return value checked before using output parameters.
- `RvBool` is a three-state boolean (True / False / Fuzzy).

## Detailed Reference

For method signatures, parameters, return values, and examples, see:

- [Utility Math and Geometry Usage Guide](UtilityMathAndGeometryUsageGuide.md)

## Related Documentation

- [Basic Data Structures](BasicDataStructures.md) — `RvPoint`, `RvLine`, `RvRect`, `RvBox2D`