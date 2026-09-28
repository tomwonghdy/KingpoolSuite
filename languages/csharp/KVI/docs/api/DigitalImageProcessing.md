---
title: "Digital Image Processing"
description: "Overview of the Dip static class and the image processing capabilities of KVI."
keywords: ["KVI", "Dip", "image processing", "binarization", "convolution"]
---

# Digital Image Processing

## Overview

The `Dip` static class in the `Kingpool.Vision` namespace provides core APIs for digital image processing, wrapping the native library `pprs.dll` (debug: `pprs_d.dll`). All methods operate on image handles (`IntPtr`) and directly modify image data.

Its purpose is to extract useful information from images and enhance features, providing stable, quantifiable input for subsequent measurement, positioning, recognition, and classification.

## Capabilities

`Dip` covers the following processing categories:

- **Binarization**: Simple threshold, dark/light region extraction, inner/outer range, Otsu, maximum entropy, minimum error, adaptive, hysteresis
- **Pixel operations**: Addition, subtraction, difference, bitwise operations, merging
- **Pixel access**: Read/write individual pixels, batch setting by coordinate or mask
- **Pixel filling**: Flood fill, rectangles, ellipses, polygons, gradients, text, channel fill
- **Geometric drawing**: Lines, polylines, ellipses
- **Geometric transforms**: Scaling, rotation, translation, flipping, perspective, pyramid, clipping
- **Grayscale adjustment**: Normalization, equalization, expansion, contrast, linear adjustment
- **Convolution and filtering**: Blur, Gaussian, median, mean, custom kernels
- **Edge detection and differential**: Gradient, first/second-order difference, Sobel, Scharr, Canny
- **Morphological processing**: Erosion, dilation, opening, closing, top-hat, black-hat, hole filling
- **Skeletonization and thinning**: Skeleton, thinning, distance transform

## General Conventions

- All methods directly modify the passed image; invalid handles cause a silent return.
- Methods returning `IntPtr` produce a new image handle that must be wrapped with `KImage(IntPtr, false)` and released by the caller.
- The library name is selected by the conditional compilation constant `DEBUGGING_KINGPOOL_SUITE`.

## Detailed Reference

For method signatures, parameters, return values, and examples, see:

- [Digital Image Processing Usage Guide](DigitalImageProcessingUsageGuide.md)

## Related Documentation

- [Digital Image Analysis](DigitalImageAnalysis.md) — analysis capabilities of the `Dia` class
- [KImage Class Function Usage Guide](KImageClassFunctionUsageGuide.md) — the image object that `Dip` operates on