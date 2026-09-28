---
title: "Basic Data Structures"
description: "Overview of foundational data structures in Kingpool.Core."
keywords: ["KVI", "Kingpool.Core", "RvPoint", "RvRect", "RvRgb"]
---

# Basic Data Structures

## Overview

The `Kingpool.Core` namespace provides fundamental data structures, geometric entities, color types, and utility functions for the entire Kingpool Suite. Most types use `[StructLayout(LayoutKind.Sequential)]` for native code compatibility.

Naming convention: `Rv` prefix for base types, `F32` / `F64` suffixes for floating-point variants.

## Main Type Categories

### Geometric Structures

- `RvPoint` / `RvPointF32` / `RvPointF64` — 2D coordinate points
- `RvSize` / `RvOffset` — size and offset
- `RvRect` / `RvRectF32` / `RvRectF64` — axis-aligned rectangles
- `RvBox2D` — rotated rectangle
- `RvLine` / `RvLineF32` / `RvLineF64` — line segments
- `RvEllipticArcF32` / `RvCircularArcF32` — arcs
- `RvScalar` / `RvScalarF32` / `RvScalarF64` — length-4 scalar arrays

### Color Structures

- `RvRgb` — three-channel RGB color
- `RvRgba` — four-channel RGBA color

### Enums

- `CharEncoding` — UTF-8 or local ANSI
- `RvBool` — three-state boolean (False / True / Fuzzy)
- `RvDirection` — Horizontal / Vertical / Both

### Utility Classes

- `CAP` — global configuration and character set
- `Pool` — general-purpose helper (color, math, file, image, license, error)
- `NativeArg` — marshalling helper for unmanaged buffers

## Detailed References

For detailed API of individual classes, see:

- [CAP Class Usage Guide](CAPClassUsageGuide.md)
- [Pool Class Usage Guide](PoolClassUsageGuide.md)
- [NativeArg Class Usage Guide](NativeArgClassUsageGuide.md)
- [Clock Class Usage Guide](ClockClassUsageGuide.md)
- [FileOption Class Usage Guide](FileOptionClassUsageGuide.md)
- [KFdox Class Function Usage Guide](KFdoxClassFunctionUsageGuide.md)