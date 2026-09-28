---
title: "KImage and Related Types"
description: "Overview of the KImage class, pixel formats, and color space structures in KVI."
keywords: ["KVI", "KImage", "PixelFormat", "color space"]
---

# KImage and Related Types

## Overview

The `KImage` class represents a digital image and is the fundamental carrier for all image processing in the Kingpool Suite. It wraps an unmanaged image handle from a native DLL.

## Key Concepts

### Pixel Formats

| Format | Description |
| :--- | :--- |
| `Bin` | Binary image, 1 bit per pixel |
| `Gray` | Grayscale image, 8 bits per pixel |
| `BGR` | Three-channel color image, order blue, green, red |
| `RGB` | Three-channel color image, order red, green, blue |
| `BGRA` | Four-channel image with Alpha transparency |
| `RGBA` | Four-channel image |
| `HSV` / `YUV` / `YCBCR` | Color space conversions |

### Grayscale Conversion Methods

| Method | Formula |
| :--- | :--- |
| `Default` | `0.299R + 0.587G + 0.114B` |
| `ValueOfHSV` | `max(R, G, B)` |
| `MaxOfRgb` | `MAX(R, G, B)` |
| `MinOfRgb` | `MIN(R, G, B)` |
| `AvgOfRgb` | `(R + G + B) / 3` |

### Color Space Structures

- `RvHsv`: Hue (0~360), Saturation (0~1), Value (0~1)
- `RvYCbCr`: Y, Cb, Cr (0~255)
- `RvYuv`: Y, U, V

## Capabilities

`KImage` supports image creation, loading, saving, format conversion, channel splitting/merging, pixel filling, cloning, and memory export.

## Detailed Reference

For constructors, methods, properties, and examples, see:

- [KImage Class Function Usage Guide](KImageClassFunctionUsageGuide.md)

## Related Documentation

- [Basic Data Structures](BasicDataStructures.md) — `RvPoint`, `RvRect`, `RvRgb`, `RvScalar`