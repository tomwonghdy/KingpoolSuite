# KMask Class Detailed Function Usage Guide

## Overview

KMask is a class used to represent a binary mask. A mask is a binary template used to identify which elements in an image participate in analysis and processing. When processing large images, it can reduce computation and improve performance. The basic unit in mask data is called an **element**, and each element is either 0 or 1: 1 means valid, 0 means invalid.

Masks are divided into **area masks** and **line masks**. Area mask data is a 2D byte array, used to define the pixel processing region; line masks are a sequence of coordinate points, used to define sampling paths. In the current version, only area masks are valid.

The methods in KMask are divided by function into: construction and lifecycle, creation, cloning and copying, property queries, data access, logical operations, size and scaling, reshaping, and conversion to/from images.

## Nested Enum

### KMask.MergeType

Mask merge operation types, used by the KMask.Merge method.

| Member | Value | Description |
|----|----|----|
| NotA | 17 | Not A, i.e., keep the parts of B that do not belong to A |
| NotB | 18 | Not B, i.e., keep the parts of A that do not belong to B |
| Both | 19 | Intersection; result is 1 only when both masks have 1 at the corresponding element |
| AnyOf | 20 | Union; result is 1 if either mask has 1 at the corresponding element |

**Usage Example**

csharp

KMask m1 = new KMask();

m1.Create(MaskShape.HorizontalStrap, 100, 80);

KMask m2 = new KMask();

m2.Create(MaskShape.VerticalStrap, 100, 80);

KMask union = KMask.Merge(m1, m2, (int)KMask.MergeType.AnyOf); *// Union, forms a squared ring*

KMask inter = KMask.Merge(m1, m2, (int)KMask.MergeType.Both); *// Intersection*

## Constructors

### KMask()

csharp

public KMask()

**Description**: Creates a default mask with shape Rect and size 120$\times$80.

**Parameters**: None.

**Return Value**: None (constructor).

**Usage Example**

csharp

KMask mask = new KMask();

### KMask(KImage imBin)

csharp

public KMask(KImage imBin)

**Description**: Derives a mask from a binary image.

**Parameters**

| Parameter | Type   | Description                                       |
|-----------|--------|---------------------------------------------------|
| imBin     | KImage | Input binary image; foreground pixel value is 255 |

**Return Value**: None (constructor).

**Notes**

- The input image must be in PixelFormat.Bin format; otherwise a default rectangular mask is created.

- Elements with value 1 in the mask correspond to foreground regions with pixel value 255 in the binary image.

**Usage Example**

csharp

KImage im = new KImage("..\\samples\\triangle.png");

im.Cast(PixelFormat.Gray);

Dip.MinError(im.Handle, MinErrorMode.Poinssen);

im.Cast(PixelFormat.Bin);

KMask mask = new KMask(im);

### KMask(int width, int height)

csharp

public KMask(int width, int height)

**Description**: Creates a rectangular mask with the specified width and height.

**Parameters**

| Parameter | Type | Description               |
|-----------|------|---------------------------|
| width     | int  | Mask width (in elements)  |
| height    | int  | Mask height (in elements) |

**Return Value**: None (constructor).

**Usage Example**

csharp

KMask mask = new KMask(100, 80);

### KMask(IntPtr hMask, bool bAttached)

csharp

public KMask(IntPtr hMask, bool bAttached)

**Description**: Wraps an existing mask handle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hMask | IntPtr | Existing mask handle |
| bAttached | bool | true means the handle is externally owned and this instance will not release it; false means this instance takes ownership and will release it on destruction |

**Usage Example**

csharp

KMask mask = new KMask(handle, false);

**Notes**

- If hMask is IntPtr.Zero, throws ArgumentException.

### KMask(MaskShape shape, int width, int height)

csharp

public KMask(MaskShape shape, int width, int height)

**Description**: Creates a mask with the specified shape, width, and height.

**Parameters**

| Parameter | Type      | Description                                       |
|-----------|-----------|---------------------------------------------------|
| shape     | MaskShape | Mask shape, e.g., Rect, FilledEllipse, Ring, etc. |
| width     | int       | Width                                             |
| height    | int       | Height                                            |

**Usage Example**

csharp

KMask mask = new KMask(MaskShape.FilledEllipse, 160, 160);

### KMask(MaskShape shape, int para1, int para2, int para3)

csharp

public KMask(MaskShape shape, int para1, int para2, int para3)

**Description**: Creates a mask with the specified shape and three parameters. The meaning of the parameters varies with the shape.

**Parameters**

| Parameter           | Type      | Description              |
|---------------------|-----------|--------------------------|
| shape               | MaskShape | Mask shape               |
| para1, para2, para3 | int       | Shape-related parameters |

**Usage Example**

csharp

*// Elliptical ring: width 100, height 80, ring width 20*

KMask ring = new KMask(MaskShape.Ring, 100, 80, 20);

*// Banana (arc): first quadrant, radius 160, angle 4500 (i.e., 45 degrees), arc band width 60*

KMask banana = new KMask(MaskShape.BananaQ1, 160, 4500, 60);

**Notes**

- For the Ring shape, if width and height are unequal, the result is an elliptical ring.

- The angle parameter for the banana shape is in units of 1/100 degree.

## Creation

### Create(MaskShape shape, int width, int height)

csharp

public bool Create(MaskShape shape, int width, int height)

**Description**: Creates a mask with the specified shape, width, and height, releasing the old handle.

**Parameters**

| Parameter | Type      | Description |
|-----------|-----------|-------------|
| shape     | MaskShape | Mask shape  |
| width     | int       | Width       |
| height    | int       | Height      |

**Return Value**

| Type | Description                              |
|------|------------------------------------------|
| bool | Returns true on success; otherwise false |

### Create(MaskShape shape, int width, int height, int stride)

csharp

public bool Create(MaskShape shape, int width, int height, int stride)

**Description**: Creates a mask with the specified shape, width, height, and stride.

**Parameters**

| Parameter | Type      | Description |
|-----------|-----------|-------------|
| shape     | MaskShape | Mask shape  |
| width     | int       | Width       |
| height    | int       | Height      |
| stride    | int       | Row stride  |

**Return Value**

| Type | Description                |
|------|----------------------------|
| bool | Whether creation succeeded |

### Create(RvPoint\[\] polygon)

csharp

public bool Create(RvPoint\[\] polygon)

**Description**: Creates a mask from a polygon vertex array.

**Parameters**

| Parameter | Type        | Description                               |
|-----------|-------------|-------------------------------------------|
| polygon   | RvPoint\[\] | Polygon vertex array; at least 3 vertices |

**Return Value**

| Type | Description                |
|------|----------------------------|
| bool | Whether creation succeeded |

**Usage Example**

csharp

RvPoint\[\] pts = new RvPoint\[4\] {

new RvPoint(10, 10), new RvPoint(200, 10),

new RvPoint(200, 150), new RvPoint(10, 150)

};

KMask mask = new KMask();

mask.Create(pts);

### CreateBanana

csharp

public bool CreateBanana(MaskShape shape, int radius, int angle, int stride)

**Description**: Creates a banana (arc) mask.

**Parameters**

| Parameter | Type      | Description                        |
|-----------|-----------|------------------------------------|
| shape     | MaskShape | Must be one of BananaQ1 ~ BananaQ4 |
| radius    | int       | Radius                             |
| angle     | int       | Angle in units of 1/100 degree     |
| stride    | int       | Arc band width                     |

**Return Value**

| Type | Description                |
|------|----------------------------|
| bool | Whether creation succeeded |

**Usage Example**

csharp

KMask mask = new KMask();

mask.CreateBanana(MaskShape.BananaQ1, 160, 4500, 60);

## Cloning and Copying

### Clone

csharp

public KMask Clone()

**Description**: Deep copy; creates a fully independent mask copy.

**Return Value**

| Type  | Description                            |
|-------|----------------------------------------|
| KMask | New mask copy; returns null on failure |

**Usage Example**

csharp

KMask copy = mask.Clone();

### CopyTo

csharp

public void CopyTo(KMask twin)

**Description**: Copies the current mask to the target mask; the target's original handle is released.

**Parameters**

| Parameter | Type  | Description |
|-----------|-------|-------------|
| twin      | KMask | Target mask |

**Return Value**: None.

**Notes**

- Unlike Clone, CopyTo does not create a new object but writes content into an existing target. Suitable for scenarios that reuse the target instance.

## Property Queries

### GetWidth / GetHeight

csharp

public int GetWidth()

public int GetHeight()

**Description**: Gets the mask width or height (in elements).

**Return Value**

| Type | Description     |
|------|-----------------|
| int  | Width or height |

### GetPitch

csharp

public int GetPitch()

**Description**: Gets bytes per row (row stride).

**Return Value**

| Type | Description   |
|------|---------------|
| int  | Bytes per row |

### GetSize / GetArea

csharp

public UIntPtr GetSize()

public UIntPtr GetArea()

**Description**: Gets the total bytes of the mask data buffer or the area of the valid region.

- GetSize: total bytes of the mask data buffer.

- GetArea: area of the mask's valid region, i.e., the total number of elements with value 1.

**Return Value**

| Type    | Description              |
|---------|--------------------------|
| UIntPtr | Byte count or area value |

### IsEmpty

csharp

public bool IsEmpty()

**Description**: Determines whether the mask is empty (no valid elements).

**Return Value**

| Type | Description                            |
|------|----------------------------------------|
| bool | Returns true if empty; otherwise false |

### GetAnchor / SetAncor

csharp

public RvPoint GetAnchor()

public void SetAncor(int x, int y)

**Description**: Gets or sets the mask's anchor position. The anchor is used to define the mask's reference point.

**Parameters**

| Parameter | Type | Description                   |
|-----------|------|-------------------------------|
| x, y      | int  | Anchor coordinates (SetAncor) |

**Return Value** (only GetAnchor)

| Type    | Description        |
|---------|--------------------|
| RvPoint | Anchor coordinates |

**Usage Example**

csharp

mask.SetAncor(50, 50);

RvPoint anchor = mask.GetAnchor();

### GetOrigin / SetOrigin

csharp

public RvPoint GetOrigin()

public void SetOrigin(int x, int y)

**Description**: Gets or sets the mask's origin position, representing the offset of the mask coordinate system relative to the image coordinate system.

**Parameters**

| Parameter | Type | Description                    |
|-----------|------|--------------------------------|
| x, y      | int  | Origin coordinates (SetOrigin) |

**Return Value** (only GetOrigin)

| Type    | Description        |
|---------|--------------------|
| RvPoint | Origin coordinates |

## Data Access

### GetData

csharp

public byte\[\] GetData()

**Description**: Gets the contents of the mask data buffer.

**Return Value**

| Type     | Description                                   |
|----------|-----------------------------------------------|
| byte\[\] | Mask data byte array; returns null on failure |

### SetData

csharp

public void SetData(byte\[\] data)

**Description**: Writes a byte array to the mask data buffer.

**Parameters**

| Parameter | Type     | Description                                        |
|-----------|----------|----------------------------------------------------|
| data      | byte\[\] | Data to write; length should match the mask buffer |

**Return Value**: None.

## Logical Operations

### Toggle

csharp

public void Toggle()

**Description**: Flips the mask, swapping valid and invalid elements (1 becomes 0 and 0 becomes 1).

**Parameters**: None.

**Return Value**: None.

**Usage Example**

csharp

KMask mask = new KMask();

mask.Create(MaskShape.Ring, 100, 80, 20);

KMask inverse = mask.Clone();

inverse.Toggle(); *// Obtain a complementary mask*

### SetZero

csharp

public void SetZero()

**Description**: Sets all elements to 0.

**Parameters**: None.

**Return Value**: None.

### SetOne

csharp

public void SetOne()

**Description**: Sets all elements to 1.

**Parameters**: None.

**Return Value**: None.

### Merge

csharp

public static KMask Merge(KMask mask1, KMask mask2, int opType)

**Description**: Merges two masks, returning a new KMask object. The original masks are not modified.

**Parameters**

| Parameter | Type  | Description                                 |
|-----------|-------|---------------------------------------------|
| mask1     | KMask | First mask                                  |
| mask2     | KMask | Second mask                                 |
| opType    | int   | Merge type, specified by the MergeType enum |

**Return Value**

| Type  | Description                                     |
|-------|-------------------------------------------------|
| KMask | Merge result; returns null if either is invalid |

**Usage Example**

csharp

KMask union = KMask.Merge(m1, m2, (int)KMask.MergeType.AnyOf);

KMask inter = KMask.Merge(m1, m2, (int)KMask.MergeType.Both);

## Size and Scaling

### Resize(int width, int height)

csharp

public bool Resize(int width, int height)

**Description**: Adjusts the mask size.

**Parameters**

| Parameter | Type | Description   |
|-----------|------|---------------|
| width     | int  | Target width  |
| height    | int  | Target height |

**Return Value**

| Type | Description                      |
|------|----------------------------------|
| bool | Whether the adjustment succeeded |

### Resize(int width, int height, bool bKeepShape)

csharp

public void Resize(int width, int height, bool bKeepShape)

**Description**: Adjusts the mask size, optionally scaling the original content proportionally.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| width | int | Target width |
| height | int | Target height |
| bKeepShape | bool | When true, scales the original content proportionally to fit the new size |

**Return Value**: None.

### Scale

csharp

public bool Scale(float scale)

**Description**: Scales the entire mask by the specified factor. The mask size becomes scale times the original, and the internal valid region scales proportionally.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| scale | float | Scale factor; positive values magnify, values less than 1 shrink |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether scaling succeeded |

**Usage Example**

csharp

mask.Scale(1.2f); *// Magnify the mask by 1.2 times*

**Notes**

- Resize directly changes the mask size by specifying target width and height, with optional proportional content adaptation.

- Scale changes both mask size and content via a scaling factor, suitable for overall proportional magnification.

## Reshaping

### Reshape(KImage image)

csharp

public void Reshape(KImage image)

**Description**: Reshapes the mask from a binary image.

**Parameters**

| Parameter | Type   | Description                                     |
|-----------|--------|-------------------------------------------------|
| image     | KImage | Binary image; must be in PixelFormat.Bin format |

**Return Value**: None.

**Notes**

- If the input image is empty or not in binary format, throws an exception.

- Foreground regions with pixel value 255 in the image are mapped to mask elements with value 1.

### Reshape(RvPoint\[\] vertexArray)

csharp

public void Reshape(RvPoint\[\] vertexArray)

**Description**: Reshapes the mask into a polygon using an integer vertex array.

**Parameters**

| Parameter   | Type        | Description                               |
|-------------|-------------|-------------------------------------------|
| vertexArray | RvPoint\[\] | Polygon vertex array; at least 3 vertices |

**Return Value**: None.

### Reshape(RvPointF32\[\] vertexArray)

csharp

public void Reshape(RvPointF32\[\] vertexArray)

**Description**: Reshapes the mask into a polygon using a single-precision floating-point vertex array.

**Parameters**

| Parameter   | Type           | Description                               |
|-------------|----------------|-------------------------------------------|
| vertexArray | RvPointF32\[\] | Polygon vertex array; at least 3 vertices |

**Return Value**: None.

**Notes**

- Floating-point coordinates are truncated to integers internally, which may introduce precision loss.

### Reshape(RvPointF64\[\] vertexArray)

csharp

public void Reshape(RvPointF64\[\] vertexArray)

**Description**: Reshapes the mask into a polygon using a double-precision floating-point vertex array.

**Parameters**

| Parameter   | Type           | Description                               |
|-------------|----------------|-------------------------------------------|
| vertexArray | RvPointF64\[\] | Polygon vertex array; at least 3 vertices |

**Return Value**: None.

**Notes**

- Floating-point coordinates are truncated to integers internally, which may introduce precision loss.

## Conversion To/From Image

### Derive

csharp

public KImage Derive()

**Description**: Exports the mask as a binary image (PixelFormat.Bin).

**Return Value**

| Type | Description |
|----|----|
| KImage | Binary image; mask elements with value 1 are mapped to 255 (white), elements with value 0 to 0 (black); returns null on failure |

**Usage Example**

csharp

KMask m = new KMask();

m.Create(MaskShape.HorizontalStrap, 100, 80);

KImage im = m.Derive();

**Notes**

- This method is commonly used to visualize mask results or pass them to interfaces that only accept image input.

## Typical Usage

### Creating a Circle and an Elliptical Ring

csharp

KMask mask = new KMask();

mask.Create(MaskShape.FilledEllipse, 160, 160); *// Circle*

ShowMaskInPictureBox(mask, picPreview1);

mask.Create(MaskShape.Ring, 100, 80, 20); *// Elliptical ring*

ShowMaskInPictureBox(mask, picPreview2);

### Creating and Using a Complementary Mask

csharp

KMask mask = new KMask();

mask.Create(MaskShape.Ring, 100, 80, 20);

KMask newMask = mask.Clone();

newMask.Toggle(); *// Obtain a complementary mask*

ShowMaskInPictureBox(mask, picPreview1);

ShowMaskInPictureBox(newMask, picPreview2);

### Merging Masks

csharp

KMask m1 = new KMask();

m1.Create(MaskShape.HorizontalStrap, 100, 80);

KMask m2 = new KMask();

m2.Create(MaskShape.VerticalStrap, 100, 80);

KMask union = KMask.Merge(m1, m2, (int)KMask.MergeType.AnyOf); *// Union, forms a squared ring*

KMask inter = KMask.Merge(m1, m2, (int)KMask.MergeType.Both); *// Intersection*

### Mask Derived from a Binary Image

csharp

KImage im = new KImage();

bool ret = im.Load("..\\samples\\triangle.png");

Pool.Assert(ret);

im.Cast(PixelFormat.Bin);

KMask m = new KMask();

m.Reshape(im);

ShowMaskInPictureBox(m, picPreview2);

### Exporting a Mask as an Image

csharp

KMask m = new KMask();

m.Create(MaskShape.HorizontalStrap, 100, 80);

KImage im = m.Derive();

ShowImageInPictureBox(im, picPreview2);

### Outputting Properties

csharp

KMask mask = new KMask();

mask.Create(MaskShape.FullStrap, 120, 100);

string str = "Properties used frequently \r\n";

str += \$"Width: {mask.GetWidth()} \r\n";

str += \$"Height: {mask.GetHeight()} \r\n";

str += \$"Pitch: {mask.GetPitch()} \r\n";

str += \$"Size: {mask.GetSize()} \r\n";

str += \$"Anchor Pos: {mask.GetAnchor().ToString()} \r\n";

str += \$"Origin: {mask.GetOrigin().ToString()} \r\n";

str += \$"Area: {mask.GetArea()} \r\n";

MessageBox.Show(str, "Properties");
