# Kingpool Vision Interface (KVI) Documentation

Welcome to the documentation for the **Kingpool Vision Interface (KVI)** — the foundational module of the Kingpool Suite. KVI provides core data structures, image handling, mask operations, BLOB analysis, image processing, image analysis, drawing, disk I/O, and mathematical utilities for C# developers.

This `docs/` directory contains the full documentation set, organized into two main sections: **API Reference** and **Examples**.

## API Reference

The [`api/`](api/index.md) section contains the complete reference for every public class and type in KVI. For convenience, all documents are listed below by category.

### Core Data Types

| Document | Description |
| :--- | :--- |
| [Basic Data Structures](api/BasicDataStructures.md) | Enums, geometric structures, color structures, scalar structures, and utility classes. |
| [Image and Related Types](api/ImageAndRelatedTypes.md) | Pixel formats, grayscale conversion methods, color space structures, and the KImage overview. |
| [Mask Class](api/MaskClass.md) | Overview of the KMask class: mask shapes, merge types, creation, and conversion. |
| [BLOB Analysis](api/BlobAnalysis.md) | Overview of BLOB concepts: encoding types, distance measures, and the KBlob class. |

### Utility Classes

| Document | Description |
| :--- | :--- |
| [CAP Class Usage Guide](api/CAPClassUsageGuide.md) | Global configuration, version numbers, and character set. |
| [Clock Class Usage Guide](api/ClockClassUsageGuide.md) | Lightweight timer based on `DateTime.Now`. |
| [FileOption Class Usage Guide](api/FileOptionClassUsageGuide.md) | INI configuration file read/write (Windows only). |
| [High-Precision Stopwatch](api/HighPrecisionStopwatch.md) | Overview of the StopWatch class. |
| [StopWatch Class Usage Guide](api/StopWatchClassUsageGuide.md) | Detailed reference for the high-precision stopwatch. |
| [KFdox Class Function Usage Guide](api/KFdoxClassFunctionUsageGuide.md) | Tree-structured data container for analysis results. |
| [NativeArg Class Usage Guide](api/NativeArgClassUsageGuide.md) | Marshalling helper for unmanaged buffers. |
| [Pool Class Usage Guide](api/PoolClassUsageGuide.md) | General-purpose static helper class. |

### Image Classes

| Document | Description |
| :--- | :--- |
| [KImage Class Function Usage Guide](api/KImageClassFunctionUsageGuide.md) | Complete guide to the KImage class. |
| [KMask Class Detailed Function Usage Guide](api/KMaskClassDetailedFunctionUsageGuide.md) | Detailed KMask reference. |

### Vision Analysis

| Document | Description |
| :--- | :--- |
| [Dia Class Function Usage Guide](api/DiaClassFunctionUsageGuide.md) | Pixel statistics, histogram, projection, geometric properties, threshold estimation. |
| [KBlob Class Usage Guide](api/KBlobClassUsageGuide.md) | BLOB constructors, properties, statistics, edge/cluster data, batch extraction. |

### Image Processing

| Document | Description |
| :--- | :--- |
| [Digital Image Processing](api/DigitalImageProcessing.md) | Overview of the Dip static class. |
| [Digital Image Processing Usage Guide](api/DigitalImageProcessingUsageGuide.md) | Detailed Dip reference organized by function. |
| [Digital Image Analysis](api/DigitalImageAnalysis.md) | Overview of the Dia static class. |

### Drawing

| Document | Description |
| :--- | :--- |
| [Graphics and Image Drawing](api/GraphicsAndImageDrawing.md) | Overview of the Render class and stroke model. |
| [Render Class Function Usage Guide](api/RenderClassFunctionUsageGuide.md) | Detailed Render reference. |

### Disk I/O

| Document | Description |
| :--- | :--- |
| [Disk File Read/Write](api/DiskFileReadWrite.md) | Overview of the DiskHelper class. |
| [DiskHelper Class Usage Guide](api/DiskHelperClassUsageGuide.md) | Detailed DiskHelper reference. |

### Math and Geometry

| Document | Description |
| :--- | :--- |
| [Utility Math and Geometry](api/UtilityMathAndGeometry.md) | Overview of the Smath static class. |
| [Utility Math and Geometry Usage Guide](api/UtilityMathAndGeometryUsageGuide.md) | Detailed Smath reference. |

→ **[Browse the full API Index](api/index.md)**

---

## Examples

The [`examples/`](examples/index.md) section provides practical, runnable examples that demonstrate common KVI tasks.

| Document | Description |
| :--- | :--- |
| [Image Basics](examples/ImageBasics.md) | Create, clone, resize, convert, load, and save images. |
| [Mask Basics](examples/MaskBasics.md) | Create, clone, resize, and merge masks; convert between images and masks. |
| [BLOB Basics](examples/BlobBasics.md) | Extract BLOBs, compute properties, measure distances and density. |
| [Digital Image Processing](examples/DigitalImageProcess.md) | Binarization, convolution, pixel filling, geometric transforms. |
| [Digital Image Analysis](examples/DigitalImageAnalyse.md) | Binary features, sharpness, histogram, projection, Canny contrast estimation. |
| [Graphics](examples/Graphics.md) | Drawing context, geometric shapes, text, images, strokes, canvas. |
| [Practical Mathematics](examples/PracticalMathematics.md) | Geometric calculations, rectangle operations, point-in-polygon, distance. |

→ **[Browse the full Examples Index](examples/index.md)**

---

## Getting Started

If you are new to KVI, we recommend the following reading order:

1. **Read the overview** — start with the [KVI module introduction](../README.md) (in the parent directory).
2. **Explore the examples** — begin with [Image Basics](examples/ImageBasics.md) to understand the core `KImage` class.
3. **Dive into the API** — refer to the [API Reference](api/index.md) for detailed class documentation.

---

## Version

This documentation corresponds to the current release of the Kingpool Suite C# SDK. For version history and compatibility information, refer to the repository root `CHANGELOG.md`.

---

## Contributing

If you find errors or want to improve the documentation, please open an issue or submit a pull request in the main repository.