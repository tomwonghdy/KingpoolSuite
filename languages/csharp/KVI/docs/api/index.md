---
title: "KVI API Reference"
description: "Complete API reference for the Kingpool Vision Interface (KVI) library, covering core data types, utility classes, image classes, vision analysis, image processing, drawing, and I/O."
keywords: ["Kingpool Suite", "KVI", "API", "C# SDK", "KImage", "KMask", "KBlob", "Dia", "Dip", "Render", "machine vision"]
---

# KVI API Reference

This section contains the complete API reference for the Kingpool Vision Interface (KVI) library. All documents are organized by functional area.

## Contents

- [Core Data Types](#core-data-types)
- [Utility Classes](#utility-classes)
- [Image Classes](#image-classes)
- [Vision Analysis](#vision-analysis)
- [Image Processing](#image-processing)
- [Drawing](#drawing)
- [Disk I/O](#disk-io)
- [Math and Geometry](#math-and-geometry)

---

## Core Data Types

Foundational data structures, geometric entities, color types, and image-related type definitions.

| Document | Description |
| :--- | :--- |
| [Basic Data Structures](BasicDataStructures.md) | Enums, geometric structures (point, line, rect, arc, box2d), color structures (RGB, RGBA), scalar structures, and utility classes (CAP, Pool, NativeArg). |
| [Image and Related Types](ImageAndRelatedTypes.md) | Pixel format enumeration, grayscale conversion methods, color space structures (HSV, YCbCr, YUV), and the core KImage class overview. |

## Utility Classes

Helper classes for configuration, timing, data containers, and native interop.

| Document | Description |
| :--- | :--- |
| [CAP Class Usage Guide](CAPClassUsageGuide.md) | Global configuration class: version numbers, lens distortion constants, and the global character set (CharSet). |
| [Clock Class Usage Guide](ClockClassUsageGuide.md) | Lightweight timer based on `DateTime.Now`, for measuring code segment execution time. |
| [FileOption Class Usage Guide](FileOptionClassUsageGuide.md) | INI-format configuration file read/write (Windows only), supporting int, float, bool, and string types. |
| [KFdox Class Function Usage Guide](KFdoxClassFunctionUsageGuide.md) | Tree-structured data container for organizing multi-dimensional, multi-level analysis results. Supports JSON and binary serialization. |
| [NativeArg Class Usage Guide](NativeArgClassUsageGuide.md) | Helper class for marshalling managed data (strings, scalars, arrays, raw pointers) into unmanaged buffers. |
| [Pool Class Usage Guide](PoolClassUsageGuide.md) | Static utility class aggregating constants, assertions, color operations, string encoding, math helpers, image helpers, license handling, and parsing helpers. |
| [StopWatch Class Usage Guide](StopWatchClassUsageGuide.md) | High-precision stopwatch with higher resolution than `DateTime`, supporting continuous multi-stage measurement. |

## Image Classes

Classes representing images, masks, and BLOBs.

| Document | Description |
| :--- | :--- |
| [KImage Class Function Usage Guide](KImageClassFunctionUsageGuide.md) | Complete guide to the KImage class: constructors, pixel filling, size queries, loading/saving, cloning, format conversion, and channel splitting/merging. |
| [Mask Class](MaskClass.md) | Overview of the KMask class: mask shapes, merge types, creation, cloning, resizing, and image conversion. |
| [KMask Class Detailed Function Usage Guide](KMaskClassDetailedFunctionUsageGuide.md) | Detailed KMask reference: constructors, creation methods, property queries, logical operations, reshaping, and conversion to/from images. |

## Vision Analysis

Classes for extracting structured information from binary images and analyzing image features.

| Document | Description |
| :--- | :--- |
| [BLOB Analysis](BlobAnalysis.md) | Overview of BLOB concepts: encoding types (Raw, Contour, Cluster), distance measures, and the KBlob class. |
| [KBlob Class Usage Guide](KBlobClassUsageGuide.md) | Detailed KBlob reference: constructors, state queries, geometric properties, pixel statistics, edge/cluster data, cloning/merging, and batch extraction. |
| [Dia Class Function Usage Guide](DiaClassFunctionUsageGuide.md) | Detailed Dia reference: pixel statistics, histogram, projection, geometric properties, bounding geometry, region analysis, and threshold estimation. |

## Image Processing

Static classes for image preprocessing, binarization, convolution, morphology, and geometric transforms.

| Document | Description |
| :--- | :--- |
| [Digital Image Processing](DigitalImageProcessing.md) | Overview of the Dip static class: binarization, pixel operations, pixel filling, geometric drawing, geometric transforms, grayscale adjustment, convolution, edge detection, morphology, and skeletonization. |
| [Digital Image Processing Usage Guide](DigitalImageProcessingUsageGuide.md) | Detailed Dip reference organized by function: binarization, pixel operations, pixel access, filling, geometric drawing, geometric transforms, grayscale adjustment, convolution, edge detection, morphology, skeletonization, and misc. |
| [Digital Image Analysis](DigitalImageAnalysis.md) | Overview of the Dia static class: pixel statistics, histogram and projection, geometric properties, bounding geometry, region analysis, and threshold estimation. |

## Drawing

OpenGL-accelerated graphics and image drawing classes.

| Document | Description |
| :--- | :--- |
| [Graphics and Image Drawing](GraphicsAndImageDrawing.md) | Overview of the Render class and stroke model: context management, geometric shapes, text, images, strokes, canvas, and export. |
| [Render Class Function Usage Guide](RenderClassFunctionUsageGuide.md) | Detailed Render reference: constants, context management, canvas settings, view control, coordinate conversion, drawing resources, primitive drawing, stroke management, clearing, export, and logo appearance. |

## Disk I/O

Classes for file read/write and structured data persistence.

| Document | Description |
| :--- | :--- |
| [Disk File Read/Write](DiskFileReadWrite.md) | Overview of the DiskHelper class: file sessions, section operations, object encoding/decoding, and data read/write. |
| [DiskHelper Class Usage Guide](DiskHelperClassUsageGuide.md) | Detailed DiskHelper reference: lifecycle, file operations, static save methods, file sessions, section operations, object encoding/decoding, data writing, and data reading. |

## Math and Geometry

Static classes for geometric calculations and pixel-related helpers.

| Document | Description |
| :--- | :--- |
| [Utility Math and Geometry](UtilityMathAndGeometry.md) | Overview of the Smath static class: arc/ellipse fitting, point-line relationships, polygon/polyline operations, rectangle operations, and pixel helpers. |
| [Utility Math and Geometry Usage Guide](UtilityMathAndGeometryUsageGuide.md) | Detailed Smath reference: angle normalization, arc and ellipse, rectangle and box, point and line, polygon and polyline, vertex operations, pixel helpers, MIP scaling, and other utilities. |

---

## Related Documentation

- [KVI Documentation](../index.md)
- [KVI Examples](../examples/index.md)
- [KVI Tutorials](../../tutorials/README.md)