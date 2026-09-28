## **Usage Instructions for Digital Image Processing Functions**

## 1. Binarization

### Adaptive

csharp

public static void Adaptive(IntPtr hImage, AdaptiveBinarize method, int blockSize, double extra1, double extra2)

**Description**: Adaptive binarization. Each pixel computes its threshold independently based on its neighborhood information; suitable for images with uneven illumination.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Input image handle; should be grayscale |
| method | AdaptiveBinarize | Adaptive method: Mean, Gaussian, LocalInteger, IsoData |
| blockSize | int | Neighborhood window size, typically an odd value larger than the target size |
| extra1 | double | Method-related additional parameter (meaning varies with method) |
| extra2 | double | Method-related additional parameter |

**Return Value**: None. Directly modifies hImage data.

### Canny

csharp

public static void Canny(IntPtr hImage, double lowThresh, double highThresh, bool bForceToBin, int aperture = 3)

**Description**: Canny edge detection; extracts edges via dual thresholds and gradient determination.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Input image handle; should be grayscale |
| lowThresh | double | Low threshold; gradients below this value are classified as non-edges |
| highThresh | double | High threshold; gradients above this value are directly retained as strong edges |
| bForceToBin | bool | When true, outputs a binary edge image (edges uniformly 255); when false, retains gradient magnitude |
| aperture | int | Gaussian smoothing kernel size, default 3 |

**Return Value**: None. Directly modifies hImage data.

### Dark / Light

csharp

public static void Dark(IntPtr hImage, byte level, byte newVal = 255, bool bBin = false)

public static void Light(IntPtr hImage, byte level, byte newVal = 255, bool bBin = false)

**Description**: Extracts dark regions (Dark) or bright regions (Light).

**Parameters**

| Parameter | Type   | Description                                             |
|-----------|--------|---------------------------------------------------------|
| hImage    | IntPtr | Input image handle                                      |
| level     | byte   | Threshold (0~255)                                       |
| newVal    | byte   | Value set for pixels meeting the condition, default 255 |
| bBin      | bool   | When true, outputs a binary image                       |

**Return Value**: None.

### Inner / Outer

csharp

public static void Inner(IntPtr hImage, byte low, byte high, byte newVal = 255, bool bBin = false)

public static void Outer(IntPtr hImage, byte low, byte high, byte newVal = 255, bool bBin = false)

**Description**: Extracts within the range (Inner) or outside the range (Outer).

**Parameters**

| Parameter | Type   | Description                                             |
|-----------|--------|---------------------------------------------------------|
| hImage    | IntPtr | Input image handle                                      |
| low       | byte   | Lower bound                                             |
| high      | byte   | Upper bound                                             |
| newVal    | byte   | Value set for pixels meeting the condition, default 255 |
| bBin      | bool   | When true, outputs a binary image                       |

**Return Value**: None.

### MaxEntropy

csharp

public static void MaxEntropy(IntPtr hImage)

**Description**: Maximum entropy method automatic binarization.

**Parameters**

| Parameter | Type   | Description                             |
|-----------|--------|-----------------------------------------|
| hImage    | IntPtr | Input image handle; should be grayscale |

**Return Value**: None.

### MinError / MinErrorE1

csharp

public static void MinError(IntPtr hImage, MinErrorMode mode)

public static void MinErrorE1(IntPtr hImage, IntPtr hMask, MinErrorMode mode)

**Description**: Minimum error method automatic binarization.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Input image handle |
| hMask | IntPtr | Mask handle; limits the computation region (MinErrorE1) |
| mode | MinErrorMode | Distribution model: Gaussion or Poinssen |

**Return Value**: None.

### Otsu / Majority

csharp

public static void Otsu(IntPtr hImage, IntPtr hMask)

public static void Majority(IntPtr hImage, IntPtr hMask)

**Description**: Otsu method (Otsu) or majority method (Majority) automatic binarization.

**Parameters**

| Parameter | Type   | Description                                       |
|-----------|--------|---------------------------------------------------|
| hImage    | IntPtr | Input image handle                                |
| hMask     | IntPtr | Mask handle; pass IntPtr.Zero to indicate no mask |

**Return Value**: None.

### Simple / SimpleE1

csharp

public static void Simple(IntPtr hImage, byte level, bool bReverse)

public static void SimpleE1(IntPtr hImage, byte level, bool bReverse)

**Description**: Simple threshold binarization. Pixels with grayscale greater than level are set to 255, otherwise to 0.

**Parameters**

| Parameter | Type   | Description                    |
|-----------|--------|--------------------------------|
| hImage    | IntPtr | Input image handle             |
| level     | byte   | Threshold (0~255)              |
| bReverse  | bool   | When true, reverses the result |

**Return Value**: None.

### Hysteresis

csharp

public static void Hysteresis(IntPtr hImage, int low, int high, int maxLength)

**Description**: Hysteresis threshold binarization. Grayscale above high is directly retained; below low is directly set to background; pixels between the two are retained only if connected to a strong target (connection span not exceeding maxLength).

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Input image handle |
| low | int | Low threshold |
| high | int | High threshold |
| maxLength | int | Maximum span allowed for a pending pixel to connect to a strong target |

**Return Value**: None.

### GetOptimum / GetOptimumE1

csharp

public static byte GetOptimum(IntPtr hImage, int method)

public static byte GetOptimumE1(IntPtr hImage, IntPtr hMask, int method)

**Description**: Computes and returns the automatic threshold.

**Parameters**

| Parameter | Type   | Description                                |
|-----------|--------|--------------------------------------------|
| hImage    | IntPtr | Input image handle                         |
| hMask     | IntPtr | Mask handle (GetOptimumE1)                 |
| method    | int    | Method constant, specified by RTA\_ series |

**Return Value**: byte — the computed threshold (0~255).

## 2. Pixel Operations

### PixelAdd / PixelSubtract / PixelDiff

csharp

public static IntPtr PixelAdd(IntPtr hImage0, IntPtr hImage1)

public static IntPtr PixelSubtract(IntPtr hImage0, IntPtr hImage1)

public static IntPtr PixelDiff(IntPtr hImage0, IntPtr hImage1)

**Description**: Pixel-by-pixel addition, subtraction, or absolute difference of two images.

**Parameters**

| Parameter | Type   | Description         |
|-----------|--------|---------------------|
| hImage0   | IntPtr | First image handle  |
| hImage1   | IntPtr | Second image handle |

**Return Value**: IntPtr — result image handle, must be managed by the caller.

### PixelLarge / PixelSmall

csharp

public static IntPtr PixelLarge(IntPtr hImage0, IntPtr hImage1)

public static IntPtr PixelSmall(IntPtr hImage0, IntPtr hImage1)

**Description**: Pixel-by-pixel takes the larger or smaller value.

**Parameters**

| Parameter | Type   | Description         |
|-----------|--------|---------------------|
| hImage0   | IntPtr | First image handle  |
| hImage1   | IntPtr | Second image handle |

**Return Value**: IntPtr — result image handle.

### PixelAnd / PixelOr / PixelXor

csharp

public static IntPtr PixelAnd(IntPtr hImage0, IntPtr hImage1)

public static IntPtr PixelOr(IntPtr hImage0, IntPtr hImage1)

public static IntPtr PixelXor(IntPtr hImage0, IntPtr hImage1)

**Description**: Pixel-by-pixel bitwise AND, OR, XOR.

**Parameters**

| Parameter | Type   | Description         |
|-----------|--------|---------------------|
| hImage0   | IntPtr | First image handle  |
| hImage1   | IntPtr | Second image handle |

**Return Value**: IntPtr — result image handle.

### PixelCopy

csharp

public static IntPtr PixelCopy(IntPtr hImage0, IntPtr hImage1)

**Description**: Copies pixels from hImage1 to hImage0.

**Parameters**

| Parameter | Type   | Description              |
|-----------|--------|--------------------------|
| hImage0   | IntPtr | Destination image handle |
| hImage1   | IntPtr | Source image handle      |

**Return Value**: IntPtr — result image handle.

### PixelMerge

csharp

public static IntPtr PixelMerge(IntPtr hImage0, IntPtr hImage1, PixelOperator type)

**Description**: Merges two images by the specified operation type; the operation form is hImage0 op hImage1.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage0 | IntPtr | First image handle |
| hImage1 | IntPtr | Second image handle |
| type | PixelOperator | Operation type: Add, Sub, Abs, Mul, Div, And, Or, Xor, Large, Small |

**Return Value**: IntPtr — result image handle.

## 3. Pixel Access

### GetPixel / GetPixelE1

csharp

public static uint GetPixel(IntPtr hImage, int x, int y)

public static uint GetPixelE1(IntPtr hImage, int x, int y, int aperture)

**Description**: Reads the color value of the pixel at the specified position.

**Parameters**

| Parameter | Type   | Description                              |
|-----------|--------|------------------------------------------|
| hImage    | IntPtr | Image handle                             |
| x         | int    | X coordinate                             |
| y         | int    | Y coordinate                             |
| aperture  | int    | Aperture parameter for sub-pixel reading |

**Return Value**: uint — 32-bit color value (0xAARRGGBB).

### SetPixel

csharp

public static void SetPixel(IntPtr hImage, int x, int y, uint color)

**Description**: Sets the pixel color at the specified position.

**Parameters**

| Parameter | Type   | Description        |
|-----------|--------|--------------------|
| hImage    | IntPtr | Image handle       |
| x         | int    | X coordinate       |
| y         | int    | Y coordinate       |
| color     | uint   | 32-bit color value |

**Return Value**: None.

### SetPixelE1 / SetPixelE2

csharp

public static void SetPixelE1(IntPtr hImage, RvPoint\[\] pPtArr, uint color)

public static void SetPixelE2(IntPtr hImage, int dx, int dy, RvPoint\[\] pPtArr, uint color)

**Description**: Batch sets multiple coordinate positions to the same color. The E2 version supports an additional offset.

**Parameters**

| Parameter | Type        | Description            |
|-----------|-------------|------------------------|
| hImage    | IntPtr      | Image handle           |
| pPtArr    | RvPoint\[\] | Coordinate array       |
| color     | uint        | 32-bit color value     |
| dx        | int         | Horizontal offset (E2) |
| dy        | int         | Vertical offset (E2)   |

**Return Value**: None.

### SetPixelEx

csharp

public static void SetPixelEx(IntPtr hImage, uint color, IntPtr hMask, bool bOutside)

**Description**: Batch pixel setting based on a mask.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Image handle |
| color | uint | 32-bit color value |
| hMask | IntPtr | Mask handle |
| bOutside | bool | false modifies pixels inside the mask; true modifies pixels outside the mask |

**Return Value**: None.

### ReplacePixels / ReplaceChannel

csharp

public static void ReplacePixels(IntPtr hImage, uint target, uint newColor, IntPtr hMask)

public static IntPtr ReplaceChannel(IntPtr hImage, IntPtr sub, int channel = 1)

**Description**: Replaces the specified color or channel.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Image handle |
| target | uint | Target color to be replaced |
| newColor | uint | New color |
| hMask | IntPtr | Mask handle |
| sub | IntPtr | Source image handle for replacement |
| channel | int | Target channel, specified by COLOR_CH1~COLOR_CH4, default 1 |

**Return Value**: ReplaceChannel returns IntPtr result image handle; ReplacePixels has no return value.

## 4. Filling

### Fill / FillE1 / FillEx

csharp

public static IntPtr Fill(IntPtr hImage, int x, int y, uint nNewValue)

public static IntPtr FillE1(IntPtr hImage, int x, int y, uint targetColor, uint newColor)

public static void FillEx(IntPtr hImage, int channel, byte nNewValue)

**Description**: Flood fill or channel fill.

**Parameters**

| Parameter   | Type   | Description                 |
|-------------|--------|-----------------------------|
| hImage      | IntPtr | Image handle                |
| x           | int    | Starting X coordinate       |
| y           | int    | Starting Y coordinate       |
| nNewValue   | uint   | Fill value                  |
| targetColor | uint   | Target color to be replaced |
| newColor    | uint   | New color                   |
| channel     | int    | Channel identifier          |
| nNewValue   | byte   | Fill value                  |

**Return Value**: Fill / FillE1 return IntPtr result image handle; FillEx has no return value.

### FillBorder

csharp

public static void FillBorder(IntPtr hImage, uint nNewValue)

**Description**: Fills the image's four borders with the specified value.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| hImage    | IntPtr | Image handle |
| nNewValue | uint   | Fill value   |

**Return Value**: None.

### FillEllipse / FillEllipseE1

csharp

public static void FillEllipse(IntPtr hImage, int left, int top, int right, int bottom, uint nNewValue)

public static void FillEllipseE1(IntPtr hImage, RvBox2D box, uint nNewValue)

**Description**: Fills an ellipse.

**Parameters**

| Parameter                | Type    | Description                   |
|--------------------------|---------|-------------------------------|
| hImage                   | IntPtr  | Image handle                  |
| left, top, right, bottom | int     | Bounding rectangle boundaries |
| box                      | RvBox2D | Rotated rectangle description |
| nNewValue                | uint    | Fill value                    |

**Return Value**: None.

### FillGradient

csharp

public static void FillGradient(IntPtr hImage, RvRgb colorStart, RvRgb colorEnd, GradientDirection direction)

**Description**: Fills the image with a linear gradient.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Image handle |
| colorStart | RvRgb | Start color |
| colorEnd | RvRgb | End color |
| direction | GradientDirection | Gradient direction: Horizontal, Vertical, Diagonal, DiagonalReverse |

**Return Value**: None.

### FillHole / FillHoleEx

csharp

public static void FillHole(IntPtr hImage)

public static void FillHoleEx(IntPtr hImage, IntPtr hMask)

**Description**: Fills holes inside the foreground.

**Parameters**

| Parameter | Type   | Description                    |
|-----------|--------|--------------------------------|
| hImage    | IntPtr | Image handle; should be binary |
| hMask     | IntPtr | Mask handle (FillHoleEx)       |

**Return Value**: None.

### FillPolygon / FillPolygonE1 / FillPolygonE2

csharp

public static void FillPolygon(IntPtr hImage, IntPtr pVertexArray, int nArraySize, uint nNewValue)

public static void FillPolygon(IntPtr hImage, RvPoint\[\] vertexArray, uint nNewValue)

public static void FillPolygonE1(IntPtr hImage, IntPtr pVertexArray, int nArraySize, IntPtr pHoleArray, int nHoleSize, uint nNewValue)

public static void FillPolygonE1(IntPtr hImage, RvPoint\[\] vertexArray, RvPoint\[\] holeArray, uint nNewValue)

public static void FillPolygonE2(IntPtr hImage, IntPtr pVertexArray, int nArraySize, uint nNewValue, bool bFill)

public static void FillPolygonE2(IntPtr hImage, RvPoint\[\] vertexArray, uint nNewValue, bool bFill)

**Description**: Fills or outlines a polygon. E1 supports holes; E2 supports fill/outline switching.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Image handle |
| pVertexArray / vertexArray | IntPtr / RvPoint\[\] | Polygon vertex array |
| nArraySize | int | Vertex count |
| pHoleArray / holeArray | IntPtr / RvPoint\[\] | Hole vertex array (E1) |
| nHoleSize | int | Hole vertex count (E1) |
| nNewValue | uint | Fill value |
| bFill | bool | true fills; false outlines (E2) |

**Return Value**: None.

### FillRect

csharp

public static void FillRect(IntPtr hImage, RvRect rect, uint nNewValue, bool bBorderOnly)

**Description**: Fills a rectangle or draws only the border.

**Parameters**

| Parameter   | Type   | Description                                  |
|-------------|--------|----------------------------------------------|
| hImage      | IntPtr | Image handle                                 |
| rect        | RvRect | Rectangle region                             |
| nNewValue   | uint   | Fill value                                   |
| bBorderOnly | bool   | true draws only the border; false solid fill |

**Return Value**: None.

### FillText / FillTextEx

csharp

public static void FillText(IntPtr hImage, string strText, int x, int y, int fontSize, uint textColor)

public static void FillTextEx(IntPtr hImage, string strText, int x, int y, string strFaceName, int fontSize, uint textColor, uint backColor, int flags)

**Description**: Draws text on the image.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Image handle |
| strText | string | Text content |
| x, y | int | Starting coordinates |
| fontSize | int | Font size |
| textColor | uint | Text color |
| strFaceName | string | Font name (FillTextEx) |
| backColor | uint | Background color (FillTextEx) |
| flags | int | Style flags, combined by FillTextStyle bits (FillTextEx) |

**Return Value**: None.

### FloodFill / FloodMask

csharp

public static void FloodFill(IntPtr hImage, RvPoint pos, RvRgb newColor, RvScalarF64 lodiff, RvScalarF64 updiff, int connectivity, int flags = 0)

public static IntPtr FloodMask(IntPtr hImage, RvPoint pos, RvScalarF64 lower, RvScalarF64 upper, int connectivity, int flags, IntPtr imMask)

**Description**: Flood fill based on color similarity; FloodMask outputs the result as a mask.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Image handle |
| pos | RvPoint | Seed point |
| newColor | RvRgb | New color (FloodFill) |
| lodiff, updiff | RvScalarF64 | Lower and upper tolerances per channel (FloodFill) |
| lower, upper | RvScalarF64 | Lower and upper bounds per channel (FloodMask) |
| connectivity | int | Connectivity; takes FF_CONN4 or FF_CONN8 |
| flags | int | Flags; FF_DEFAULT_RANGE or FF_FIXED_RANGE |
| imMask | IntPtr | Output mask handle (FloodMask) |

**Return Value**: FloodFill has no return value; FloodMask returns IntPtr result mask handle.

## 5. Geometric Drawing

### SetEllipse

csharp

public static void SetEllipse(IntPtr hImage, int cx, int cy, int rx, int ry, float angle, uint nNewValue)

**Description**: Draws a rotated ellipse outline.

**Parameters**

| Parameter | Type   | Description              |
|-----------|--------|--------------------------|
| hImage    | IntPtr | Image handle             |
| cx, cy    | int    | Center coordinates       |
| rx, ry    | int    | Semi-axis lengths        |
| angle     | float  | Rotation angle (degrees) |
| nNewValue | uint   | Drawing color            |

**Return Value**: None.

### SetLine

csharp

public static void SetLine(IntPtr hImage, RvPoint p0, RvPoint p1, uint nNewValue)

**Description**: Draws a single line.

**Parameters**

| Parameter | Type    | Description   |
|-----------|---------|---------------|
| hImage    | IntPtr  | Image handle  |
| p0        | RvPoint | Start point   |
| p1        | RvPoint | End point     |
| nNewValue | uint    | Drawing color |

**Return Value**: None.

### SetPolyline

csharp

public static void SetPolyline(IntPtr hImage, RvPoint\[\] vertexArray, uint nNewValue)

**Description**: Draws a polyline.

**Parameters**

| Parameter   | Type        | Description   |
|-------------|-------------|---------------|
| hImage      | IntPtr      | Image handle  |
| vertexArray | RvPoint\[\] | Vertex array  |
| nNewValue   | uint        | Drawing color |

**Return Value**: None.

## 6. Geometric Transforms

### Resize / ResizeE1

csharp

public static void Resize(IntPtr hImage, int width, int height, int fillColor)

public static void ResizeE1(IntPtr hImage, int width, int height, PixelFilterType flag = PixelFilterType.Bilinear)

**Description**: Resizes the image to the specified width and height.

**Parameters**

| Parameter     | Type            | Description                            |
|---------------|-----------------|----------------------------------------|
| hImage        | IntPtr          | Image handle                           |
| width, height | int             | Target width and height                |
| fillColor     | int             | Fill color                             |
| flag          | PixelFilterType | Interpolation method, default Bilinear |

**Return Value**: None.

### Rotate / RotateEx / RotateE2 / RotateE3

csharp

public static void Rotate(IntPtr hImage, int times)

public static void RotateEx(IntPtr hImage, double angle, bool bNearest, bool bReserveDim)

public static void RotateE2(IntPtr hImage, double angle, uint fillColor, bool bNearest, bool bReserveDim)

public static void RotateE3(IntPtr hImage, double cx, double cy, double angle, uint fillColor)

**Description**: Rotates the image. Rotate rotates by multiples of 90 degrees; RotateEx by an arbitrary angle; RotateE2 with fill color; RotateE3 around a specified center.

**Parameters**

| Parameter   | Type   | Description                                    |
|-------------|--------|------------------------------------------------|
| hImage      | IntPtr | Image handle                                   |
| times       | int    | Multiple of 90 degrees                         |
| angle       | double | Rotation angle (degrees); positive = clockwise |
| bNearest    | bool   | true uses nearest-neighbor; otherwise bilinear |
| bReserveDim | bool   | true keeps the original size                   |
| fillColor   | uint   | Fill color for blank areas                     |
| cx, cy      | double | Rotation center coordinates (RotateE3)         |

**Return Value**: None.

### Scale / ScaleE1 / ScaleEx

csharp

public static bool Scale(IntPtr hImage, float scale, PixelFilterType flag = PixelFilterType.Nearest)

public static void ScaleE1(IntPtr hImage, float scale, uint fillColor, PixelFilterType flag = PixelFilterType.Nearest, bool bReserveDim = false)

public static bool ScaleEx(IntPtr imgOri, IntPtr imgNew, PixelFilterType flag = PixelFilterType.Nearest)

**Description**: Scales the image.

**Parameters**

| Parameter   | Type            | Description                          |
|-------------|-----------------|--------------------------------------|
| hImage      | IntPtr          | Image handle                         |
| scale       | float           | Scaling factor                       |
| flag        | PixelFilterType | Interpolation method                 |
| fillColor   | uint            | Fill color (ScaleE1)                 |
| bReserveDim | bool            | true keeps the canvas size unchanged |
| imgOri      | IntPtr          | Source image handle (ScaleEx)        |
| imgNew      | IntPtr          | Destination image handle (ScaleEx)   |

**Return Value**: Scale and ScaleEx return bool indicating success; ScaleE1 has no return value.

### Flip

csharp

public static void Flip(IntPtr hImage, RvDirection direction)

**Description**: Flips the image.

**Parameters**

| Parameter | Type        | Description                                |
|-----------|-------------|--------------------------------------------|
| hImage    | IntPtr      | Image handle                               |
| direction | RvDirection | Flip direction: Horizontal, Vertical, Both |

**Return Value**: None.

### Translate / TranslateEx

csharp

public static void Translate(IntPtr hImage, float dx, float dy, bool bNearest)

public static void TranslateEx(IntPtr hImage, float dx, float dy, bool bNearest, uint fillColor)

**Description**: Translates the image.

**Parameters**

| Parameter | Type   | Description                              |
|-----------|--------|------------------------------------------|
| hImage    | IntPtr | Image handle                             |
| dx, dy    | float  | Translation amounts                      |
| bNearest  | bool   | true uses nearest-neighbor               |
| fillColor | uint   | Fill color for blank areas (TranslateEx) |

**Return Value**: None.

### Warp / WarpEx / Unwarp

csharp

public static void Warp(KImage src, RvPointF32\[\] pSrcControls, KImage dest)

public static void Warp(KImage src, RvPointF32\[\] pSrcControls, KImage dest, RvPointF32\[\] pDestControls, bool bKeepOutlier)

public static IntPtr Warp(IntPtr hImage, IntPtr pSrcControls, IntPtr pDestControls, IntPtr dest)

public static IntPtr WarpEx(IntPtr hImage, IntPtr pSrcControls, int nSrcControls, IntPtr pDestControls, int nDestControls, bool bKeepOutlier, IntPtr dest)

public static void Unwarp(KImage src, RvPointF32\[\] srcCtrls, KImage dest)

**Description**: Perspective transform.

**Parameters**

| Parameter                   | Type            | Description                    |
|-----------------------------|-----------------|--------------------------------|
| src / hImage                | KImage / IntPtr | Source image                   |
| dest                        | KImage / IntPtr | Destination image              |
| pSrcControls / pSrcCtrls    | RvPointF32\[\]  | Source control points (4)      |
| pDestControls               | RvPointF32\[\]  | Destination control points (4) |
| nSrcControls, nDestControls | int             | Control point counts (WarpEx)  |
| bKeepOutlier                | bool            | Whether to preserve outliers   |

**Return Value**: The KImage versions have no return value; the IntPtr versions return IntPtr result image handle.

### CopyImage / CopyPixels / CopyPixelsE2 / CopyPixelsE3

csharp

public static void CopyImage(KImage src, int sx, int sy, int sw, int sh, KImage dest, int dx = 0, int dy = 0, int dw = -1, int dh = -1)

public static void CopyPixels(IntPtr src, int sx, int sy, int sw, int sh, IntPtr dest, int dx = 0, int dy = 0, int dw = -1, int dh = -1)

public static void CopyPixelsE2(IntPtr src, IntPtr dest, IntPtr hMask)

public static void CopyPixelsE3(IntPtr src, IntPtr dest, RvPointF32\[\] vertex)

**Description**: Copies pixels from the source image to the destination image. CopyPixels by rectangular region; E2 by mask; E3 by quadrilateral control points.

**Parameters**

| Parameter | Type            | Description                                 |
|-----------|-----------------|---------------------------------------------|
| src       | KImage / IntPtr | Source image                                |
| dest      | KImage / IntPtr | Destination image                           |
| sx, sy    | int             | Source region origin                        |
| sw, sh    | int             | Source region width/height                  |
| dx, dy    | int             | Destination origin, default 0               |
| dw, dh    | int             | Destination width/height, default -1 (auto) |
| hMask     | IntPtr          | Mask handle                                 |
| vertex    | RvPointF32\[\]  | Quadrilateral vertices (4)                  |

**Return Value**: None.

### Clip

csharp

public static KImage Clip(KImage image, int x, int y, uint width, uint height)

**Description**: Clips the specified rectangular region from the image.

**Parameters**

| Parameter     | Type   | Description           |
|---------------|--------|-----------------------|
| image         | KImage | Source image          |
| x, y          | int    | Clipping origin       |
| width, height | uint   | Clipping width/height |

**Return Value**: KImage — the new clipped result image; returns null on failure.

### Pyramid

csharp

public static IntPtr Pyramid(IntPtr hImage, bool bUp, IntPtr dest)

**Description**: Image pyramid.

**Parameters**

| Parameter | Type   | Description                                             |
|-----------|--------|---------------------------------------------------------|
| hImage    | IntPtr | Source image handle                                     |
| bUp       | bool   | true upsamples (magnifies); false downsamples (shrinks) |
| dest      | IntPtr | Destination image handle; may be IntPtr.Zero            |

**Return Value**: IntPtr — result image handle.

## 7. Grayscale Adjustment

### AdjustBright

csharp

public static void AdjustBright(IntPtr hImage, bool bDarker, float percent)

**Description**: Brightens or darkens proportionally.

**Parameters**

| Parameter | Type   | Description                   |
|-----------|--------|-------------------------------|
| hImage    | IntPtr | Image handle                  |
| bDarker   | bool   | true darkens; false brightens |
| percent   | float  | Ratio (0~1)                   |

**Return Value**: None.

### Contrast

csharp

public static void Contrast(IntPtr hImage, float lPercent, float hPercent)

**Description**: Contrast adjustment.

**Parameters**

| Parameter | Type   | Description                   |
|-----------|--------|-------------------------------|
| hImage    | IntPtr | Image handle                  |
| lPercent  | float  | Low-end clipping ratio (0~1)  |
| hPercent  | float  | High-end clipping ratio (0~1) |

**Return Value**: None.

### Equalize / Expand

csharp

public static void Equalize(IntPtr hImage)

public static void Expand(IntPtr hImage)

**Description**: Histogram equalization (Equalize) or grayscale expansion (Expand).

**Parameters**

| Parameter | Type   | Description                       |
|-----------|--------|-----------------------------------|
| hImage    | IntPtr | Image handle; should be grayscale |

**Return Value**: None.

### IncreaseLum

csharp

public static IntPtr IncreaseLum(IntPtr hImage)

**Description**: Increases overall luminance.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| hImage    | IntPtr | Image handle |

**Return Value**: IntPtr — result image handle.

### Linear / LinearEx

csharp

public static void Linear(IntPtr hImage, float gain, float offset)

public static void LinearEx(IntPtr hImage, int channel, float gain, float offset)

**Description**: Linear transform output = gain $\times$ input + offset.

**Parameters**

| Parameter | Type   | Description               |
|-----------|--------|---------------------------|
| hImage    | IntPtr | Image handle              |
| gain      | float  | Gain                      |
| offset    | float  | Offset                    |
| channel   | int    | Target channel (LinearEx) |

**Return Value**: None.

### Normalize

csharp

public static void Normalize(IntPtr hImage, float avg, float var)

**Description**: Adjusts the grayscale mean and variance to target values.

**Parameters**

| Parameter | Type   | Description     |
|-----------|--------|-----------------|
| hImage    | IntPtr | Image handle    |
| avg       | float  | Target mean     |
| var       | float  | Target variance |

**Return Value**: None.

## 8. Convolution and Filtering

### Blur / SmoothGaussian / SmoothMedian / SmoothAvg

csharp

public static IntPtr Blur(IntPtr hImage, int kernWidth, int kernHeight)

public static IntPtr SmoothGaussian(IntPtr hImage, int kernWidth, int kernHeight)

public static IntPtr SmoothMedian(IntPtr hImage, int kernelSize)

public static IntPtr SmoothAvg(IntPtr hImage, int kernelSize)

**Description**: Blur and smoothing.

**Parameters**

| Parameter             | Type   | Description                                |
|-----------------------|--------|--------------------------------------------|
| hImage                | IntPtr | Image handle                               |
| kernWidth, kernHeight | int    | Kernel width/height (Blur, SmoothGaussian) |
| kernelSize            | int    | Kernel size (SmoothMedian, SmoothAvg)      |

**Return Value**: IntPtr — result image handle.

### Filter / Median

csharp

public static void Filter(IntPtr hImage, IntPtr kernel)

public static void Median(IntPtr hImage, int kernelSize)

**Description**: Custom kernel filtering (Filter) or median filtering (Median).

**Parameters**

| Parameter  | Type   | Description               |
|------------|--------|---------------------------|
| hImage     | IntPtr | Image handle              |
| kernel     | IntPtr | Convolution kernel handle |
| kernelSize | int    | Kernel size               |

**Return Value**: None.

### FuseGradients

csharp

public static void FuseGradients(IntPtr hImage, ColorFusionType mode, float threshold)

**Description**: Converts a color image to grayscale by gradient fusion methods and enhances edges.

**Parameters**

| Parameter | Type            | Description                                   |
|-----------|-----------------|-----------------------------------------------|
| hImage    | IntPtr          | Color image handle                            |
| mode      | ColorFusionType | Fusion method: Sallience, Voting, Direction   |
| threshold | float           | Threshold parameter; meaning varies with mode |

**Return Value**: None.

## 9. Edge Detection and Differential

### DiffFirst / DiffSecond

csharp

public static void DiffFirst(IntPtr hImage, RvDirection direction)

public static void DiffSecond(IntPtr hImage, RvDirection direction)

**Description**: First-order or second-order difference.

**Parameters**

| Parameter | Type        | Description                           |
|-----------|-------------|---------------------------------------|
| hImage    | IntPtr      | Image handle                          |
| direction | RvDirection | Direction: Horizontal, Vertical, Both |

**Return Value**: None.

### Gradient

csharp

public static void Gradient(IntPtr hImage, RvDirection direction, int gap)

**Description**: Gradient operation.

**Parameters**

| Parameter | Type        | Description         |
|-----------|-------------|---------------------|
| hImage    | IntPtr      | Image handle        |
| direction | RvDirection | Direction           |
| gap       | int         | Difference interval |

**Return Value**: None.

### Kirsch / Prewitt / Scharr / Sobel

csharp

public static void Kirsch(IntPtr hImage, RvDirection direction)

public static void Prewitt(IntPtr hImage, RvDirection direction)

public static void Scharr(IntPtr hImage, RvDirection direction)

public static void Sobel(IntPtr hImage, RvDirection direction)

**Description**: First-order differential operators for edge detection.

**Parameters**

| Parameter | Type        | Description  |
|-----------|-------------|--------------|
| hImage    | IntPtr      | Image handle |
| direction | RvDirection | Direction    |

**Return Value**: None.

### Laplacian / Roberts

csharp

public static void Laplacian(IntPtr hImage)

public static void Roberts(IntPtr hImage)

**Description**: Laplacian second-order differential or Roberts cross-difference.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| hImage    | IntPtr | Image handle |

**Return Value**: None.

### Sharp1541

csharp

public static IntPtr Sharp1541(IntPtr hImage)

**Description**: Sharpening convolution kernel for enhancing image details and edge contrast.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| hImage    | IntPtr | Image handle |

**Return Value**: IntPtr — result image handle.

### DetectEdge

csharp

public static IntPtr DetectEdge(IntPtr hImage, DetectEdgeType type, float thresVal)

**Description**: General edge detection.

**Parameters**

| Parameter | Type           | Description                |
|-----------|----------------|----------------------------|
| hImage    | IntPtr         | Image handle               |
| type      | DetectEdgeType | Operator: Sobel, ZeroCross |
| thresVal  | float          | Threshold                  |

**Return Value**: IntPtr — result image handle.

## 10. Morphological Processing

### Dilate / DilateE1 / Erode / ErodeBin

csharp

public static void Dilate(IntPtr hImage, IntPtr shape)

public static void DilateE1(IntPtr hImage, int radius)

public static void Erode(IntPtr hImage, IntPtr shape)

public static void ErodeBin(IntPtr hImage)

**Description**: Dilation and erosion.

**Parameters**

| Parameter | Type   | Description                                    |
|-----------|--------|------------------------------------------------|
| hImage    | IntPtr | Image handle                                   |
| shape     | IntPtr | Structuring element handle                     |
| radius    | int    | Circular structuring element radius (DilateE1) |

**Return Value**: None.

### Eclose

csharp

public static void Eclose(IntPtr hImage, int radius)

**Description**: Extended version of the closing operation, supporting a specified radius.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| hImage    | IntPtr | Image handle |
| radius    | int    | Radius       |

**Return Value**: None.

### Morphology / MorpOpen / MorpClose

csharp

public static IntPtr Morphology(IntPtr hImage, MorphologyType type, int kernelSize)

public static IntPtr MorpOpen(IntPtr hImage)

public static IntPtr MorpClose(IntPtr hImage)

**Description**: Morphological processing.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Image handle |
| type | MorphologyType | Type: Erode, Dilate, Open, Close, Gradient, TopHat, BlackHat |
| kernelSize | int | Kernel size |

**Return Value**: IntPtr — result image handle.

### RemoveBorder / RemoveBorderEx

csharp

public static void RemoveBorder(IntPtr hImage)

public static void RemoveBorderEx(IntPtr hImage, IntPtr hMask)

**Description**: Removes foreground pixels connected to the image border.

**Parameters**

| Parameter | Type   | Description                  |
|-----------|--------|------------------------------|
| hImage    | IntPtr | Image handle                 |
| hMask     | IntPtr | Mask handle (RemoveBorderEx) |

**Return Value**: None.

## 11. Skeletonization and Thinning

### Skeleton / Thinning

csharp

public static void Skeleton(IntPtr hImage, int method = 0)

public static void Thinning(IntPtr hImage)

**Description**: Skeletonization or thinning.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Image handle; should be binary |
| method | int | Skeletonization method: SM_DIST_TRANS or SM_MULT_FILTER, default 0 |

**Return Value**: None.

### DistTrans / DistTransE1

csharp

public static uint DistTrans(IntPtr hImage, IntPtr pArrResult, uint size)

public static IntPtr DistTransE1(IntPtr hImage, int thres)

**Description**: Distance transform.

**Parameters**

| Parameter  | Type   | Description                      |
|------------|--------|----------------------------------|
| hImage     | IntPtr | Binary image handle              |
| pArrResult | IntPtr | Result array pointer (DistTrans) |
| size       | uint   | Array size (DistTrans)           |
| thres      | int    | Distance threshold (DistTransE1) |

**Return Value**: DistTrans returns uint actual written count; DistTransE1 returns IntPtr result image handle.

### RetrieveEdge / RetrieveEdgeE1

csharp

public static IntPtr RetrieveEdge(IntPtr hImage, bool bDark2Light, IntPtr dest)

public static IntPtr RetrieveEdgeE1(IntPtr hImage, bool n8, IntPtr dest)

**Description**: Extracts edges from a grayscale image.

**Parameters**

| Parameter   | Type   | Description                                       |
|-------------|--------|---------------------------------------------------|
| hImage      | IntPtr | Image handle                                      |
| bDark2Light | bool   | Dark-to-light transition direction (RetrieveEdge) |
| n8          | bool   | Eight-neighborhood search (RetrieveEdgeE1)        |
| dest        | IntPtr | Destination image handle                          |

**Return Value**: IntPtr — result image handle.

## 12. Misc

### Differ

csharp

public static void Differ(KImage image, KImage background, int offset, int mode)

public static void Differ(IntPtr hImage, IntPtr background, int offset, int mode)

**Description**: Image-background difference.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Input image |
| background | KImage / IntPtr | Background image |
| offset | int | Offset |
| mode | int | Mode, specified by BD\_ series: BD_DARK, BD_LIGHT, BD_EQUAL, BD_NOTEQUAL |

**Return Value**: None.

### MakeSquarePixel

csharp

public static bool MakeSquarePixel(IntPtr hImage, int pixelSizeX, int pixelSizeY, int filterType, bool bShrink)

**Description**: Converts non-square pixels to square pixels.

**Parameters**

| Parameter              | Type   | Description       |
|------------------------|--------|-------------------|
| hImage                 | IntPtr | Image handle      |
| pixelSizeX, pixelSizeY | int    | Pixel size        |
| filterType             | int    | Filter type       |
| bShrink                | bool   | Whether to shrink |

**Return Value**: bool — whether successful.

### Outline / OutlineE1

csharp

public static void Outline(IntPtr hImage, IntPtr hMask, bool b8)

public static void OutlineE1(IntPtr hImage, IntPtr hMask, int dist, bool b8)

**Description**: Extracts the foreground outline.

**Parameters**

| Parameter | Type   | Description                 |
|-----------|--------|-----------------------------|
| hImage    | IntPtr | Image handle                |
| hMask     | IntPtr | Mask handle                 |
| dist      | int    | Search distance (OutlineE1) |
| b8        | bool   | Eight-neighborhood          |

**Return Value**: None.

### CutMargin

csharp

public static void CutMargin(IntPtr hImage, int thick, uint color)

**Description**: Generates a border of the specified width around the image, filled with the specified color.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| hImage    | IntPtr | Image handle |
| thick     | int    | Border width |
| color     | uint   | Fill color   |

**Return Value**: None.
