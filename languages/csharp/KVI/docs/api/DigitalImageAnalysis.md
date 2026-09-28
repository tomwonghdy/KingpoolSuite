---
title: "Digital Image Analysis"
description: "Overview of the Dia static class and the image analysis capabilities of KVI."
keywords: ["KVI", "Dia", "image analysis", "histogram", "threshold"]
---

# Digital Image Analysis

## Overview

The `Dia` static class in the `Kingpool.Vision` namespace provides core APIs for digital image analysis, wrapping the native library `anlz.dll` (debug: `anlz_d.dll`).

Its purpose is to extract structured information from images, converting pixel data into quantifiable, comparable, and decision-ready attributes.

## Capabilities

`Dia` covers the following analysis categories:

- **Pixel statistics**: Counting, summation, mean, variance, luminance
- **Histogram and projection**: Grayscale distribution, horizontal/vertical projection
- **Geometric properties**: Area, perimeter, centroid, circularity, slope, density
- **Bounding geometry**: Bounding rect, minimum bounding box, rotated rect vertices
- **Region analysis**: Directional density, extremum localization, clearness evaluation
- **Threshold estimation**: Automatic binarization threshold, Canny contrast, noise estimation

## General Conventions

- Most methods accept an optional `KMask` parameter; `null` means computing on the entire image.
- Methods returning `KFdox` support passing an existing container for reuse.
- The library name is selected by the conditional compilation constant `DEBUGGING_KINGPOOL_SUITE`.

## Detailed Reference

For method signatures, parameters, return values, and examples, see:

- [Dia Class Function Usage Guide](DiaClassFunctionUsageGuide.md)

## Related Documentation

- [Digital Image Processing](DigitalImageProcessing.md) — processing capabilities of the `Dip` class
- [BLOB Analysis](BlobAnalysis.md) — per-object property queries via `KBlob`