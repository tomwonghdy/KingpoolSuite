# KBlob Class Detailed Usage Guide

## Overview

KBlob is a class used to represent connected components in a binary image (Binary Large Object, abbreviated as BLOB). A BLOB refers to a set of mutually connected pixels with a value of 255 in a binary image, typically representing a foreground object.

BLOB analysis is a traditional and simple image object analysis method. Its algorithm principles are intuitive and its computational complexity is low, so it has low hardware performance requirements and is suitable for resource-constrained environments. Each BLOB can have various geometric and morphological features computed, including position (centroid or bounding rectangle), area, orientation, perimeter, circularity, density, etc. These are commonly used for target counting, shape recognition, size measurement, and position localization.

The methods in KBlob are divided by function into: construction and lifecycle, state queries, geometric properties, pixel statistics, edge and cluster data, distance calculation, cloning and merging, conversion to/from images and masks, and static factory methods and batch extraction.

## Constructors

### KBlob()

csharp

public KBlob()

**Description**: Creates an empty BLOB object.

**Parameters**: None.

**Return Value**: None (constructor).

**Usage Example**

csharp

KBlob blob = new KBlob();

**Notes**

- After creation, the BLOB content is empty and can be filled by subsequent methods.

### KBlob(BlobEncoder encoder, IntPtr baseImage = default)

csharp

public KBlob(BlobEncoder encoder, IntPtr baseImage = default)

**Description**: Creates a BLOB with the specified encoding type and base image.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| encoder | BlobEncoder | BLOB encoding type: Raw (raw), Cluster (point cluster), Contour (contour), RLE (not currently supported) |
| baseImage | IntPtr | Base image handle, defaults to IntPtr.Zero |

**Return Value**: None (constructor).

**Usage Example**

csharp

KBlob blob = new KBlob(BlobEncoder.Raw, imageHandle);

### KBlob(IntPtr existingBlob, bool attach)

csharp

public KBlob(IntPtr existingBlob, bool attach)

**Description**: Wraps an existing BLOB handle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| existingBlob | IntPtr | Existing BLOB handle |
| attach | bool | true means the handle is externally owned and this instance will not release it; false means this instance takes ownership and will release it on destruction |

**Return Value**: None (constructor).

**Usage Example**

csharp

*// Retrieve a BLOB handle from a sequence; release is managed uniformly by the sequence*

KBlob blob = new KBlob(seq.GetAt(i), true);

**Notes**

- Mainly used to wrap handles returned by static factory methods or Extract\*List, typically passing attach = true, with release managed uniformly by an external container (such as KSequence).

## State Queries

### IsEmpty

csharp

public bool IsEmpty()

**Description**: Determines whether the BLOB is empty (contains no foreground pixels).

**Return Value**

| Type | Description                            |
|------|----------------------------------------|
| bool | Returns true if empty; otherwise false |

**Usage Example**

csharp

if (!blob.IsEmpty()) { */\* process non-empty BLOB \*/* }

### IsNormal

csharp

public bool IsNormal()

**Description**: Determines whether the BLOB is in a normal state.

**Return Value**

| Type | Description                             |
|------|-----------------------------------------|
| bool | Returns true if normal; otherwise false |

### GetEncoder

csharp

public BlobEncoder GetEncoder()

**Description**: Gets the BLOB encoding type.

**Return Value**

| Type        | Description                                           |
|-------------|-------------------------------------------------------|
| BlobEncoder | Encoding type: Raw, Cluster, Contour, RLE, or Unknown |

**Usage Example**

csharp

BlobEncoder enc = blob.GetEncoder();

### GetWidth / GetHeight

csharp

public int GetWidth()

public int GetHeight()

**Description**: Gets the BLOB width or height (in elements).

**Return Value**

| Type | Description     |
|------|-----------------|
| int  | Width or height |

### GetTag / SetTag

csharp

public int GetTag()

public void SetTag(int tag)

**Description**: Gets or sets the BLOB tag value.

**Parameters**

| Parameter | Type | Description        |
|-----------|------|--------------------|
| tag       | int  | Tag value (SetTag) |

**Return Value** (only GetTag)

| Type | Description       |
|------|-------------------|
| int  | Current tag value |

**Usage Example**

csharp

blob.SetTag(1001);

int tag = blob.GetTag();

**Notes**

- The tag is used to attach custom identification to a BLOB, facilitating distinction or association of different objects in batch processing.

### HitTest

csharp

public bool HitTest(int x, int y)

**Description**: Determines whether the specified coordinate is inside the BLOB.

**Parameters**

| Parameter | Type | Description  |
|-----------|------|--------------|
| x         | int  | X coordinate |
| y         | int  | Y coordinate |

**Return Value**

| Type | Description                             |
|------|-----------------------------------------|
| bool | Returns true if inside; otherwise false |

**Usage Example**

csharp

if (blob.HitTest(mouseX, mouseY)) { */\* hit \*/* }

## Geometric Properties

### GetArea

csharp

public uint GetArea()

**Description**: Gets the BLOB area, i.e., the total number of foreground pixels.

**Return Value**

| Type | Description |
|------|-------------|
| uint | Area value  |

### GetCentroid

csharp

public RvPointF32 GetCentroid()

**Description**: Gets the BLOB centroid coordinates, i.e., the geometric center of all foreground pixels.

**Return Value**

| Type       | Description                                            |
|------------|--------------------------------------------------------|
| RvPointF32 | Centroid coordinates (single-precision floating point) |

**Usage Example**

csharp

RvPointF32 c = blob.GetCentroid();

Console.WriteLine(\$"Centroid: ({c.x}, {c.y})");

### GetRect / GetBoundBox

csharp

public RvRect GetRect()

public RvBox2D GetBoundBox()

**Description**: Gets the BLOB bounding geometry.

- GetRect: Axis-aligned bounding rectangle; sides parallel to the coordinate axes; simple computation but includes more blank area.

- GetBoundBox: Minimum bounding box (rotated rectangle); rotates with the object's direction and more tightly encloses the object.

**Return Value**

| Method | Return Type | Description |
|----|----|----|
| GetRect | RvRect | Axis-aligned bounding rectangle (left, top, right, bottom) |
| GetBoundBox | RvBox2D | Rotated rectangle (center, width, height, angle) |

**Usage Example**

csharp

RvRect rect = blob.GetRect();

RvBox2D box = blob.GetBoundBox();

Console.WriteLine(\$"Rect: {rect}, Box angle: {box.angle}");

### GetOffset

csharp

public RvPoint GetOffset()

**Description**: Gets the BLOB position offset in the image.

**Return Value**

| Type    | Description     |
|---------|-----------------|
| RvPoint | Position offset |

### GetPerimeter

csharp

public double GetPerimeter()

**Description**: Gets the BLOB perimeter.

**Return Value**

| Type   | Description     |
|--------|-----------------|
| double | Perimeter value |

### GetCircular

csharp

public double GetCircular()

**Description**: Gets the BLOB circularity, reflecting how close the shape is to a circle. An ideal circle has circularity 1; the more irregular the shape, the smaller the circularity.

**Return Value**

| Type   | Description       |
|--------|-------------------|
| double | Circularity value |

### GetSlope

csharp

public double GetSlope()

**Description**: Gets the BLOB major axis slope, reflecting the object's overall orientation.

**Return Value**

| Type   | Description |
|--------|-------------|
| double | Slope value |

**Notes**

- For elongated targets, the slope is the angle between the major axis and horizontal; for nearly symmetric targets, the slope may have no clear physical meaning.

### GetDensity

csharp

public double GetDensity(BlobPart part)

**Description**: Gets the BLOB density (fill ratio) within the specified sub-region, i.e., the ratio of foreground elements to the sub-region's area.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| part | BlobPart | Sub-region identifier: Whole, North, East, South, West, or combined directions |

**Return Value**

| Type   | Description         |
|--------|---------------------|
| double | Density value (0~1) |

**Usage Example**

csharp

double whole = blob.GetDensity(BlobPart.Whole);

double east = blob.GetDensity(BlobPart.East);

double west = blob.GetDensity(BlobPart.West);

*// If east is significantly greater than west, the target's center of mass is to the right*

## Pixel Statistics

The following methods require a reference original grayscale image to compute grayscale features of the corresponding pixels within the BLOB region.

### GetSummary

csharp

public bool GetSummary(KImage image, out double avg, out double var)

**Description**: Simultaneously gets the mean and variance of grayscale pixels within the BLOB region.

**Parameters**

| Parameter | Type       | Description               |
|-----------|------------|---------------------------|
| image     | KImage     | Reference grayscale image |
| avg       | out double | Output grayscale mean     |
| var       | out double | Output grayscale variance |

**Return Value**

| Type | Description |
|----|----|
| bool | Returns true on success; returns false on failure, with output parameters set to 0 |

### GetAverage

csharp

public double GetAverage(KImage image)

**Description**: Computes the average grayscale value within the BLOB region.

**Parameters**

| Parameter | Type   | Description               |
|-----------|--------|---------------------------|
| image     | KImage | Reference grayscale image |

**Return Value**

| Type   | Description                          |
|--------|--------------------------------------|
| double | Grayscale mean; returns 0 on failure |

### GetVariance

csharp

public double GetVariance(KImage image)

**Description**: Computes the grayscale variance within the BLOB region.

**Parameters**

| Parameter | Type   | Description               |
|-----------|--------|---------------------------|
| image     | KImage | Reference grayscale image |

**Return Value**

| Type   | Description                              |
|--------|------------------------------------------|
| double | Grayscale variance; returns 0 on failure |

### GetStrength

csharp

public double GetStrength(KImage image)

**Description**: Computes the total grayscale intensity within the BLOB region.

**Parameters**

| Parameter | Type   | Description               |
|-----------|--------|---------------------------|
| image     | KImage | Reference grayscale image |

**Return Value**

| Type   | Description                           |
|--------|---------------------------------------|
| double | Total intensity; returns 0 on failure |

**Notes**

- Before statistics, save a copy of the original grayscale image, because BLOB extraction typically involves binarization, which modifies image data. A typical workflow: grayscale → Clone to save imGray → binarize → extract BLOB → use imGray for statistics.

## Edge and Cluster Data

### GetEdges

csharp

public RvPoint\[\] GetEdges()

**Description**: Gets the BLOB edge point set, returning the point sequence on the contour.

**Return Value**

| Type        | Description                                              |
|-------------|----------------------------------------------------------|
| RvPoint\[\] | Edge point array; returns null if no edges or on failure |

**Usage Example**

csharp

RvPoint\[\] edges = blob.GetEdges();

if (edges != null)

{

Console.WriteLine(\$"Edge point count: {edges.Length}");

}

**Notes**

- Edge points can be used to draw contours, compute bounding polygons, fit ellipses, or as input to geometric algorithms.

### GetClusterPoints / GetClusterSize

csharp

public RvPoint\[\] GetClusterPoints()

public int GetClusterSize()

**Description**: Gets all foreground pixel position points or the point count of a cluster BLOB.

**Return Value**

| Method | Return Type | Description |
|----|----|----|
| GetClusterPoints | RvPoint\[\] | All foreground pixel position points; returns null on failure or if empty |
| GetClusterSize | int | Point count |

**Notes**

- A cluster BLOB stores all foreground pixel positions as a point set. If only the count is needed, GetClusterSize is more efficient.

## Cloning and Merging

### Clone

csharp

public KBlob Clone()

**Description**: Deep copy; creates a fully independent BLOB copy.

**Return Value**

| Type  | Description                            |
|-------|----------------------------------------|
| KBlob | New BLOB copy; returns null on failure |

**Usage Example**

csharp

KBlob copy = blob.Clone();

### Merge

csharp

public KBlob Merge(KBlob other)

**Description**: Merges the current BLOB with another BLOB.

**Parameters**

| Parameter | Type  | Description             |
|-----------|-------|-------------------------|
| other     | KBlob | The other BLOB to merge |

**Return Value**

| Type  | Description                                        |
|-------|----------------------------------------------------|
| KBlob | New merged BLOB; returns null if either is invalid |

**Notes**

- The merge operation does not modify the original objects; it returns a new KBlob instance.

### Clear

csharp

public void Clear()

**Description**: Clears the BLOB content.

**Return Value**: None.

## Conversion To/From Image/Mask

### ToImage

csharp

public KImage ToImage()

**Description**: Converts the BLOB to a binary image (PixelFormat.Bin).

**Return Value**

| Type | Description |
|----|----|
| KImage | Binary image; BLOB foreground is mapped to 255 (white), background to 0 (black); returns null on failure |

### ToMask

csharp

public KMask ToMask()

**Description**: Converts the BLOB to a mask object.

**Return Value**

| Type | Description |
|----|----|
| KMask | Mask object; BLOB valid region is mapped to element 1; returns null on failure |

**Usage Example**

csharp

KImage imBin = blob.ToImage();

KMask mask = blob.ToMask();

**Notes**

- The converted mask can be used for subsequent mask operations, region filling, image cropping, etc.

## Static Factory Methods

### FromBinaryImage

csharp

public static KBlob FromBinaryImage(KImage imBin, BlobEncoder encoder = BlobEncoder.Raw)

**Description**: Creates a BLOB from a binary image with the specified encoding.

**Parameters**

| Parameter | Type        | Description                                       |
|-----------|-------------|---------------------------------------------------|
| imBin     | KImage      | Input binary image; foreground pixel value is 255 |
| encoder   | BlobEncoder | Encoding type; default Raw                        |

**Return Value**

| Type  | Description                                      |
|-------|--------------------------------------------------|
| KBlob | The created BLOB object; returns null on failure |

**Notes**

- This method treats the entire binary image as a **single** BLOB object, suitable for images containing only a single foreground object.

- If the image contains multiple independent foreground objects, use the Extract\*List methods to separate them into multiple BLOBs.

### FromBinaryImageRaw / FromBinaryImageCluster / FromBinaryImageContour

csharp

public static KBlob FromBinaryImageRaw(KImage imBin)

public static KBlob FromBinaryImageCluster(KImage imBin)

public static KBlob FromBinaryImageContour(KImage imBin)

**Description**: Creates a BLOB from a binary image with a specific encoding type.

**Parameters**

| Parameter | Type   | Description        |
|-----------|--------|--------------------|
| imBin     | KImage | Input binary image |

**Return Value**

| Type  | Description                                      |
|-------|--------------------------------------------------|
| KBlob | The created BLOB object; returns null on failure |

**Notes**

- FromBinaryImageRaw: Raw encoding; fully preserves the BLOB region.

- FromBinaryImageCluster: Cluster encoding; stores all foreground pixels as a point set.

- FromBinaryImageContour: Contour encoding; stores only contour points.

### ExtractRawList / ExtractClusterList / ExtractContourList

csharp

public static KSequence ExtractRawList(KImage imBin, bool bConn8 = false, int minSize = 4)

public static KSequence ExtractClusterList(KImage imBin, bool bConn8, int minSize)

public static KSequence ExtractContourList(KImage imBin, int chainCoder = 0, int filter = 0, int minSize = 4)

**Description**: Separates multiple BLOB objects from a binary image, returning a KSequence container.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| imBin | KImage | Input binary image; foreground pixel value is 255 |
| bConn8 | bool | Connectivity determination. true = eight-connectivity (including diagonals); false = four-connectivity |
| minSize | int | Minimum BLOB size. Both length and width of the BLOB must reach this value to be retained; used to filter noise |
| chainCoder | int | Contour encoding method; 0 means default encoding (only ExtractContourList) |
| filter | int | Contour filtering condition; 0 means no filtering (only ExtractContourList) |

**Return Value**

| Type      | Description                                      |
|-----------|--------------------------------------------------|
| KSequence | The extracted BLOB list; returns null on failure |

**Usage Example**

csharp

KSequence seq = KBlob.ExtractClusterList(imBin, false, 4);

if (seq != null)

{

for (int i = 0; i \< seq.Count; i++)

{

using (KBlob blob = new KBlob(seq.GetAt(i), true))

{

Console.WriteLine(\$"BLOB {i + 1}: Area={blob.GetArea()}");

}

}

KBlob.ReleaseBlobList(seq);

}

**Notes**

- Each element returned is a BLOB handle, obtainable via seq.GetAt(i) and wrapped as a KBlob with attach = true.

- After use, ReleaseBlobList must be called to release the list.

### ReleaseBlobList

csharp

public static void ReleaseBlobList(KSequence seq, bool alsoDestroySequence = true)

**Description**: Releases the BLOB list returned by the Extract\*List methods.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| seq | KSequence | The BLOB list to release |
| alsoDestroySequence | bool | When true, also destroys the KSequence object itself; default true |

**Return Value**: None.

**Notes**

- The extracted BLOB list occupies resources and must be released via this method after use; otherwise memory leaks occur.

- Release order: first process each BLOB by iteration, then release the list as a whole.

### Distance

csharp

public static float Distance(KBlob blob0, KBlob blob1, BlobDistance type = BlobDistance.Default)

**Description**: Computes the distance between two BLOBs.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| blob0 | KBlob | First BLOB |
| blob1 | KBlob | Second BLOB |
| type | BlobDistance | Distance measurement type; default Default (centroid distance) |

**Return Value**

| Type  | Description                                                      |
|-------|------------------------------------------------------------------|
| float | Distance value; returns float.MaxValue if either BLOB is invalid |

**Distance Measurement Types**

| Type    | Description                                                 |
|---------|-------------------------------------------------------------|
| Default | Centroid distance, reflecting overall positional difference |
| Rect    | Bounding rectangle center distance, stable computation      |
| MinBox  | Minimum box center distance, sensitive to inclination       |
| Contour | Contour shortest distance (not currently supported)         |

**Usage Example**

csharp

double d1 = KBlob.Distance(blob1, blob2);

double d2 = KBlob.Distance(blob1, blob2, BlobDistance.Rect);

double d3 = KBlob.Distance(blob1, blob2, BlobDistance.MinBox);

## Typical Usage

### Extracting Properties from a Single BLOB

csharp

KImage im = new KImage("..\\samples\\triangle.png");

im.Cast(PixelFormat.Gray);

Dip.MinError(im.Handle, MinErrorMode.Poinssen);

KBlob blob = KBlob.FromBinaryImageRaw(im);

if (blob != null)

{

Console.WriteLine(\$"Area: {blob.GetArea()}");

Console.WriteLine(\$"Centroid: {blob.GetCentroid()}");

Console.WriteLine(\$"Circularity: {blob.GetCircular()}");

Console.WriteLine(\$"Perimeter: {blob.GetPerimeter()}");

}

### Multi-BLOB Extraction and Iteration

csharp

KImage im = new KImage("..\\samples\\cell.jpg");

im.Cast(PixelFormat.Gray);

KImage imGray = im.Clone();

Dip.MinError(im.Handle, MinErrorMode.Poinssen);

KSequence seq = KBlob.ExtractClusterList(im, false, 4);

if (seq != null)

{

for (int i = 0; i \< seq.Count; i++)

{

using (KBlob blob = new KBlob(seq.GetAt(i), true))

{

double avg = blob.GetAverage(imGray);

Console.WriteLine(\$"BLOB {i + 1}: Area={blob.GetArea()}, Avg={avg}");

}

}

KBlob.ReleaseBlobList(seq);

}

### Pairwise Distances Between BLOBs

csharp

for (int i = 0; i \< seq.Count - 1; i++)

{

using (KBlob blob1 = new KBlob(seq.GetAt(i), true))

{

for (int j = i + 1; j \< seq.Count; j++)

{

using (KBlob blob2 = new KBlob(seq.GetAt(j), true))

{

double d = KBlob.Distance(blob1, blob2);

Console.WriteLine(\$"Distance between BLOB {i + 1} and BLOB {j + 1}: {d}");

}

}

}

}

### BLOB-Mask/Image Conversion

csharp

KBlob blob = KBlob.FromBinaryImageCluster(im);

KMask mask = blob.ToMask();

KImage imBin = blob.ToImage();
