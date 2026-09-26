---
title: "KVI Examples"
description: "Practical examples for the Kingpool Vision Interface (KVI) library, covering image basics, masks, BLOBs, image processing, image analysis, graphics drawing, and practical mathematics."
keywords: ["Kingpool Suite", "KVI", "examples", "C# SDK", "image processing", "machine vision", "KImage", "KMask", "KBlob"]
---

# KVI Examples

This section contains practical examples for the Kingpool Vision Interface (KVI) library. Each page explains a specific topic with annotated C# code, screenshots, and parameter descriptions.

## Contents

| Example | Description |
| :--- | :--- |
| [Image Basics](ImageBasics.md) | Create, clone, resize, and convert images; load from memory; export encoded data. |
| [Mask Basics](MaskBasics.md) | Create, clone, resize, and merge masks; convert between images and masks. |
| [BLOB Basics](BlobBasics.md) | Extract BLOBs; compute geometric properties; measure distance and density. |
| [Digital Image Processing](DigitalImageProcess.md) | Binarization, convolution, pixel filling, and geometric transformation. |
| [Digital Image Analysis](DigitalImageAnalyse.md) | Binary image features, sharpness, histogram, projection, Canny contrast estimation. |
| [Graphics](Graphics.md) | Drawing context, geometric shapes, text, images, strokes, and canvas operations. |
| [Practical Mathematics](PracticalMathematics.md) | Geometric calculations, rectangle operations, point-in-polygon, distance calculations. |

## Image Basics

**File:** [`ImageBasics.md`](ImageBasics.md)

Introduces the `KImage` class and the four supported pixel formats (BGR, BGRA, GRAY, BIN). Covers:

- Image data format and memory layout
- Creating a digital image
- Cloning an image
- Changing image dimensions
- Changing pixel format
- Viewing basic image properties
- Cloning a shadow image
- Extracting a single channel image
- Merging single-channel images
- Loading an image from memory
- Exporting an image to memory as encoded data

**Related APIs:** `KImage`, `PixelFormat`, `Flood`, `FloodEx`, `Clone`, `SetSize`, `Cast`, `Load`, `LoadImageInMemory`, `ExportImageToMemory`, `Split`, `Merge`

## Mask Basics

**File:** [`MaskBasics.md`](MaskBasics.md)

Introduces the `KMask` class for defining regions of interest. Covers:

- Creating a mask (filled ellipse, banana, ring, strap)
- Cloning a mask
- Changing the mask size
- Viewing basic mask properties
- Mask merging (union and intersection)
- Converting an image to a mask
- Deriving an image from a mask

**Related APIs:** `KMask`, `MaskShape`, `Create`, `CreateBanana`, `Clone`, `Toggle`, `Resize`, `Scale`, `GetWidth`, `GetHeight`, `GetPitch`, `GetSize`, `GetAnchor`, `GetOrigin`, `GetArea`, `Merge`, `MergeType`, `Reshape`, `Derive`

## BLOB Basics

**File:** [`BlobBasics.md`](BlobBasics.md)

Introduces the `KBlob` class for connected-component analysis. Covers:

- BLOB extraction
- Displaying BLOB properties
- Displaying pixel grayscale information of the original image
- Calculating the distance between BLOBs
- Obtaining the density of a BLOB in different directions
- Deriving an image from a BLOB

**Related APIs:** `KBlob`, `ExtractRawList`, `ExtractClusterList`, `ReleaseBlobList`, `GetCentroid`, `GetCircular`, `GetOffset`, `GetPerimeter`, `GetRect`, `GetArea`, `GetStrength`, `GetAverage`, `GetVariance`, `Distance`, `BlobDistance`, `GetDensity`, `BlobPart`, `FromBinaryImageRaw`, `ToMask`

## Digital Image Processing

**File:** [`DigitalImageProcess.md`](DigitalImageProcess.md)

Covers the pre-processing and feature extraction stage of machine vision. Includes:

- Grayscale image binarization (manual and automatic)
- Adaptive binarization
- Removing border objects
- Filling holes
- Linearization (skeleton and thinning)
- Canny edge detection
- Hysteresis threshold binarization
- Distance transform
- Convolution (edge sharpening, denoising, difference operations, morphology)
- Pixel filling (lines, arcs, rectangles, polygons, gradient, channel, flood fill, borders, text, mask-based)
- Pixel operations (normalization, linear processing, pixel copying, pixel-level operations, position-based operations, inversion, color fusion, cropping)
- Geometric transformation (scaling, rotation, perspective, pyramid, translation, flipping)

**Related APIs:** `Dip`, `MinErrorMode`, `AdaptiveBinarize`, `MorphologyType`, `PixelOperator`, `PixelFilterType`, `ColorFusionType`, `GradientDirection`, `RvDirection`, `RvPoint`, `RvRect`, `RvRgb`, `RvScalarF64`

## Digital Image Analysis

**File:** [`DigitalImageAnalyse.md`](DigitalImageAnalyse.md)

Covers extracting structured information from images. Includes:

- Binary image features (density, circularity, slope, centroid, bounding box, bounding rectangle)
- Image sharpness
- Histogram
- Counting non-zero pixels
- Projection image
- Pixel intensity statistics
- Estimating Canny operator contrast

**Related APIs:** `Dia`, `Density`, `Circular`, `Slope`, `Centroid`, `GetBoundBox`, `GetBoundRect`, `GetClearness`, `HistogramEx`, `CountPixels`, `ProjectEx`, `CalcGrayStatsEx`, `EstimateCannyContrast`, `KFdox`, `BlobPart`, `RvBox2D`

## Graphics

**File:** [`Graphics.md`](Graphics.md)

Covers the OpenGL-based stroke model for high-performance drawing. Includes:

- Creating and releasing the drawing context
- Geometric shape drawing (lines, rectangles, arcs, polygons, composite graphics)
- Text drawing
- Digital image drawing
- Stroke-related operations (dummy stroke, modify, reuse, delete, hide, combine, split)
- Canvas-related operations (image frame, text on frame, image on frame, clearing, exporting, zoom display)
- Other related operations (LOGO display, default canvas style, OpenGL version query, coordinate conversion)
- Other RVB object drawing (KMask, KBlob, KContour)

**Related APIs:** `Render`, `XguiPanel`, `ContextType`, `CanvasLayer`, `ViewMode`, `LogoPlacement`, `MoldType`, `TileType`, `FlipType`, `EraseType`, `ModifyType`, `LinePattern`, `BrushPattern`, `FontFlags`, `TextAlign`, `GRgb`, `GRect`, `GPoint`, `RvBox2D`, `RvPoint`, `NativeArg`

## Practical Mathematics

**File:** [`PracticalMathematics.md`](PracticalMathematics.md)

Covers geometric calculations and pixel-related utilities. Includes:

- Determining circle center and radius from three points
- Polyline scaling
- Bounding regular rectangle and minimum rectangle
- Determining whether a point is inside a polygon
- Polygon expansion or shrinking copy
- Calculating area, angle, or length
- Extracting geometric points
- Adapting minimum rectangle
- Polyline rotation
- Line extension and trimming
- Regular rectangle operations
- Deriving perpendicular line and parallel line
- Determining the relative position of a point and other geometric entities
- Calculating the distance between a point and a line
- Pixel-related calculations

**Related APIs:** `Smath`, `GetArcCenter`, `ScaleVertex`, `CalcBoundRect`, `ApproxEllipse`, `IsPointInPolygon`, `PolygonOffset`, `CalcPolygonArea`, `CalcPolylineLength`, `Calc3PAngle`, `CalcPerpendicularPoint`, `CalcPolygonCenter`, `DeriveMidPoint`, `AdaptRect`, `RotatePoint`, `RotateVertex`, `ExtendLine`, `TrimLine`, `RotateRect`, `MoveRect`, `ScaleRect`, `RectUnion`, `RectIntersect`, `DeriveParallel`, `DerivePerpend`, `IsClockWise`, `IsRectSurround`, `IsPointInsideRect`, `IsRightOfLine`, `IsPointInsideCircle`, `DistPointToPoint`, `DistPointToLine`, `DistLineToLine`, `CalcPitch`, `CalcDepth`, `GetBrighterPixel`, `CalcPixelStrength`, `RvPoint`, `RvPointF32`, `RvPointF64`, `RvLine`, `RvLineF64`, `RvRect`, `RvBox2D`, `RvRgb`, `RvBool`, `ColorSpace`

## Related Documentation

- [KVI Documentation](../index.md)
- [KVI API Reference](../api/index.md)
- [KVI Tutorials](../../tutorials/README.md)

## See Also

- [Kingpool Suite Overview](../../../../README.md)
- [C# SDK Overview](../../README.md)