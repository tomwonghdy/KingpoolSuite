# Dia Class Detailed Function Usage Guide

## Overview

Dia is a static image analysis utility class used to extract structured information from images, converting pixel data into quantifiable, comparable, and decision-ready attributes. Its analysis capabilities cover:

- **Pixel statistics**: counting, summation, mean, variance, luminance total

- **Histogram and projection**: grayscale distribution, horizontal/vertical projection

- **Geometric properties**: area, perimeter, centroid, circularity, slope, density

- **Bounding geometry**: bounding rect, minimum bounding box, rotated rect vertices

- **Region analysis**: directional density, extremum localization, clearness evaluation

- **Threshold estimation**: automatic binarization threshold, Canny contrast, noise estimation

Dia is declared static, cannot be instantiated, and all members are static. Most methods provide two overloads: one accepting KImage and KMask objects, and another accepting IntPtr handles, for use in different scenarios. To limit the analysis range, pass a mask; passing null (or IntPtr.Zero) means computing on the entire image.

## Constants

### Bounding Box Calculation Order (GBB Series)

Used by the order parameter of GetBoundBoxE1.

| Name      | Type | Value | Description                   |
|-----------|------|-------|-------------------------------|
| GBB_AREA  | int  | 1     | Computed by minimum area      |
| GBB_PERIM | int  | 2     | Computed by minimum perimeter |

### Density Parts (BSP Series)

Used by integer-parameter interfaces to identify foreground object sub-regions.

| Name      | Type | Value | Description         |
|-----------|------|-------|---------------------|
| BSP_WHOLE | int  | 255   | Entire region       |
| BSP_NORTH | int  | 1     | North side (top)    |
| BSP_EAST  | int  | 2     | East side (right)   |
| BSP_SOUTH | int  | 4     | South side (bottom) |
| BSP_WEST  | int  | 8     | West side (left)    |
| BSP_NW    | int  | 9     | Northwest corner    |
| BSP_NE    | int  | 3     | Northeast corner    |
| BSP_SW    | int  | 12    | Southwest corner    |
| BSP_SE    | int  | 6     | Southeast corner    |

### Automatic Binarization Methods (AB Series)

Used by GetBinarizationOptimum.

| Name        | Type | Value | Description                         |
|-------------|------|-------|-------------------------------------|
| AB_GAUSSIAN | int  | 0     | Gaussian model minimum error method |
| AB_POINSON  | int  | 1     | Poisson model minimum error method  |
| AB_ENTROPY  | int  | 2     | Maximum entropy method              |
| AB_MAJORITY | int  | 3     | Majority method                     |
| AB_OTSU     | int  | 4     | Otsu method                         |

### Canny Contrast Estimation Methods (ECC Series)

Used by EstimateCannyContrast.

| Name | Type | Value | Description |
|----|----|----|----|
| ECC_MEDINA | int | 0 | Median method, suitable for images with uniform grayscale distribution |
| ECC_ADAPTIVE | int | 1 | Adaptive method, suitable for uneven illumination scenarios |
| ECC_HALIKE | int | 2 | Histogram similarity method, suitable for images with distinct peaks and valleys in the histogram |

## Pixel Statistics

### CountPixels

csharp

public static long CountPixels(KImage image, KMask mask = null)

public static long CountPixels(IntPtr hImage, IntPtr hMask)

**Description**: Counts the number of non-zero pixels in the image. In a binary image, this is the total number of foreground pixels, reflecting the target region's area.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| mask / hMask | KMask / IntPtr | Mask limiting the analysis region; may be null or IntPtr.Zero |

**Return Value**

| Type | Description                                |
|------|--------------------------------------------|
| long | Total number of qualifying non-zero pixels |

**Usage Example**

csharp

long n = Dia.CountPixels(img, mask);

**Notes**

- Input should be a binary or grayscale image; color images must first be converted to grayscale and binarized.

### CountPixelsEx

csharp

public static KFdox CountPixelsEx(KImage image, KMask mask = null, KFdox reading = null)

public static IntPtr CountPixelsEx(IntPtr hImage, IntPtr hMask, IntPtr reading)

**Description**: Extended version of pixel counting, returning a structured result.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| mask / hMask | KMask / IntPtr | Mask limiting the analysis region |
| reading | KFdox / IntPtr | Optional result container; if an existing instance is passed, it is reused; if null or IntPtr.Zero, created internally |

**Return Value**

| Overload       | Return Type | Description                 |
|----------------|-------------|-----------------------------|
| KImage version | KFdox       | Structured result container |
| IntPtr version | IntPtr      | Result handle               |

### Summary

csharp

public static double Summary(KImage image, KMask mask = null)

public static double Summary(IntPtr hImage, IntPtr hMask)

**Description**: Computes the total grayscale within the specified region.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description     |
|--------|-----------------|
| double | Total grayscale |

### SummaryE1

csharp

public static double SummaryE1(KImage image, RvRect rect)

public static double SummaryE1(IntPtr hImage, RvRect rect)

**Description**: Computes the total grayscale limited to a rectangular region.

**Parameters**

| Parameter      | Type            | Description                 |
|----------------|-----------------|-----------------------------|
| image / hImage | KImage / IntPtr | Image to analyze            |
| rect           | RvRect          | Limiting rectangular region |

**Return Value**

| Type   | Description     |
|--------|-----------------|
| double | Total grayscale |

### SummaryEx

csharp

public static KFdox SummaryEx(KImage image, KMask mask = null, KFdox reading = null)

public static IntPtr SummaryEx(IntPtr hImage, IntPtr hMask, IntPtr reading)

**Description**: Extended version of grayscale total, returning a structured result.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |
| reading        | KFdox / IntPtr  | Optional result container         |

**Return Value**

| Overload       | Return Type | Description                 |
|----------------|-------------|-----------------------------|
| KImage version | KFdox       | Structured result container |
| IntPtr version | IntPtr      | Result handle               |

### Average

csharp

public static double Average(KImage image, KMask mask = null)

public static double Average(IntPtr hImage, IntPtr hMask)

**Description**: Computes the grayscale mean within the specified region, reflecting the overall brightness level.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description    |
|--------|----------------|
| double | Grayscale mean |

### AverageEx

csharp

public static KFdox AverageEx(KImage image, KMask mask = null, KFdox reading = null)

public static IntPtr AverageEx(IntPtr hImage, IntPtr hMask, IntPtr reading)

**Description**: Extended version of grayscale mean, returning a structured result.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |
| reading        | KFdox / IntPtr  | Optional result container         |

**Return Value**

| Overload       | Return Type | Description                 |
|----------------|-------------|-----------------------------|
| KImage version | KFdox       | Structured result container |
| IntPtr version | IntPtr      | Result handle               |

### Variance

csharp

public static double Variance(KImage image, KMask mask = null)

public static double Variance(IntPtr hImage, IntPtr hMask)

**Description**: Computes the grayscale variance within the specified region, reflecting the dispersion of the grayscale distribution. Larger variance means higher contrast.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description        |
|--------|--------------------|
| double | Grayscale variance |

### VarianceEx

csharp

public static KFdox VarianceEx(KImage image, KMask mask = null, KFdox reading = null)

public static IntPtr VarianceEx(IntPtr hImage, IntPtr hMask, IntPtr reading)

**Description**: Extended version of grayscale variance, returning a structured result.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |
| reading        | KFdox / IntPtr  | Optional result container         |

**Return Value**

| Overload       | Return Type | Description                 |
|----------------|-------------|-----------------------------|
| KImage version | KFdox       | Structured result container |
| IntPtr version | IntPtr      | Result handle               |

### GetLum

csharp

public static double GetLum(KImage image, KMask mask = null)

public static double GetLum(IntPtr hImage, IntPtr hMask)

**Description**: Computes the luminance value of the specified region.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description     |
|--------|-----------------|
| double | Luminance value |

### GetPixelSum

csharp

public static uint GetPixelSum(KImage image, int x, int y, int radius)

public static uint GetPixelSum(IntPtr hImage, int x, int y, int radius)

**Description**: Computes the total grayscale within a circular neighborhood centered at (x, y) with radius radius.

**Parameters**

| Parameter      | Type            | Description         |
|----------------|-----------------|---------------------|
| image / hImage | KImage / IntPtr | Image to analyze    |
| x              | int             | Center X coordinate |
| y              | int             | Center Y coordinate |
| radius         | int             | Neighborhood radius |

**Return Value**

| Type | Description                             |
|------|-----------------------------------------|
| uint | Total grayscale within the neighborhood |

### GetPixelSumEx

csharp

public static double GetPixelSumEx(KImage image, int x, int y, int radius, KMask mask = null)

public static double GetPixelSumEx(IntPtr hImage, int x, int y, int radius, IntPtr hMask)

**Description**: Mask-limited local grayscale total, returning a double-precision result.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| x              | int             | Center X coordinate               |
| y              | int             | Center Y coordinate               |
| radius         | int             | Neighborhood radius               |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description                             |
|--------|-----------------------------------------|
| double | Total grayscale within the neighborhood |

### CalcGrayStats

csharp

public static int CalcGrayStats(KImage image, KMask mask, double\[\] readingArray)

public static int CalcGrayStats(KImage image, KMask mask, IntPtr pReadingArray, int nArraySize)

public static int CalcGrayStats(IntPtr hImage, IntPtr hMask, IntPtr pReadingArray, int nArraySize)

**Description**: Computes grayscale image statistics; result is written to an array.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| mask / hMask | KMask / IntPtr | Mask limiting the analysis region |
| readingArray / pReadingArray | double\[\] / IntPtr | Result array or pointer |
| nArraySize | int | Array size |

**Return Value**

| Type | Description                       |
|------|-----------------------------------|
| int  | Actual number of elements written |

**Notes**

- Array items 0, 1, 2 correspond to grayscale total, mean, and variance respectively.

### CalcGrayStatsEx

csharp

public static KFdox CalcGrayStatsEx(KImage image, KMask mask = null, KFdox reading = null)

public static IntPtr CalcGrayStatsEx(IntPtr hImage, IntPtr hMask, IntPtr reading)

**Description**: Extended version of grayscale statistics, returning a structured result.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |
| reading        | KFdox / IntPtr  | Optional result container         |

**Return Value**

| Overload       | Return Type | Description                 |
|----------------|-------------|-----------------------------|
| KImage version | KFdox       | Structured result container |
| IntPtr version | IntPtr      | Result handle               |

## Histogram and Projection

### Histogram

csharp

public static int Histogram(KImage image, long\[\] readingArray)

**Description**: Computes the image grayscale histogram.

**Parameters**

| Parameter    | Type     | Description                        |
|--------------|----------|------------------------------------|
| image        | KImage   | Image to analyze                   |
| readingArray | long\[\] | Result array, typically length 256 |

**Return Value**

| Type | Description                       |
|------|-----------------------------------|
| int  | Actual number of elements written |

### HistogramEx

csharp

public static KFdox HistogramEx(KImage image, KFdox reading = null)

**Description**: Extended version of the histogram, returning a structured result organized by channel.

**Parameters**

| Parameter | Type   | Description               |
|-----------|--------|---------------------------|
| image     | KImage | Image to analyze          |
| reading   | KFdox  | Optional result container |

**Return Value**

| Type | Description |
|----|----|
| KFdox | Structured result; each child of the root node corresponds to a color channel |

**Usage Example**

csharp

KFdox dox = Dia.HistogramEx(img);

if (dox != null)

{

int chns = img.GetChannels();

for (int c = 0; c \< chns; c++)

{

KFdox sub = dox.GetChild(c);

for (int i = 0; i \< sub.GetCount(); i++)

{

KFdox child = sub.GetChild(i);

long n;

if (child.GetInt64(out n)) { */\* pixel count at gray level i for this channel \*/* }

}

}

}

### rvHistogram

csharp

public static int rvHistogram(IntPtr hImage, IntPtr pReadingArray, int nArraySize)

**Description**: Computes the grayscale histogram using handles and pointers.

**Parameters**

| Parameter     | Type   | Description             |
|---------------|--------|-------------------------|
| hImage        | IntPtr | Image handle to analyze |
| pReadingArray | IntPtr | Result array pointer    |
| nArraySize    | int    | Array size              |

**Return Value**

| Type | Description                       |
|------|-----------------------------------|
| int  | Actual number of elements written |

### rvHistogramEx

csharp

public static IntPtr rvHistogramEx(IntPtr hImage, IntPtr reading)

**Description**: Extended version of the histogram using handles and pointers, returning a result handle.

**Parameters**

| Parameter | Type   | Description               |
|-----------|--------|---------------------------|
| hImage    | IntPtr | Image handle to analyze   |
| reading   | IntPtr | Optional result container |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Result handle |

### Project

csharp

public static int Project(KImage image, int dir, long\[\] readingArray)

public static int Project(KImage image, int dir, IntPtr pReadingArray, int nArraySize)

public static int Project(IntPtr hImage, int dir, IntPtr pReadingArray, int nArraySize)

**Description**: Computes the image projection in the specified direction.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| dir | int | Projection direction, specified by the RvDirection enum (Horizontal, Vertical, Both) |
| readingArray / pReadingArray | long\[\] / IntPtr | Result array or pointer |
| nArraySize | int | Array size |

**Return Value**

| Type | Description                       |
|------|-----------------------------------|
| int  | Actual number of elements written |

**Notes**

- Horizontal projection sums pixels in each row; result array length equals image height. Vertical projection sums pixels in each column; result array length equals image width.

### ProjectEx

csharp

public static KFdox ProjectEx(KImage image, int dir, KFdox reading = null)

public static IntPtr ProjectEx(IntPtr hImage, int dir, IntPtr reading)

**Description**: Extended version of projection, returning a structured result organized by channel.

**Parameters**

| Parameter      | Type            | Description               |
|----------------|-----------------|---------------------------|
| image / hImage | KImage / IntPtr | Image to analyze          |
| dir            | int             | Projection direction      |
| reading        | KFdox / IntPtr  | Optional result container |

**Return Value**

| Overload       | Return Type | Description                 |
|----------------|-------------|-----------------------------|
| KImage version | KFdox       | Structured result container |
| IntPtr version | IntPtr      | Result handle               |

## Geometric Properties

### Area

csharp

public static uint Area(KImage image, KMask mask = null)

public static uint Area(IntPtr hImage, IntPtr hMask)

**Description**: Computes the area of the foreground region, i.e., the total number of foreground pixels.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Binary image to analyze           |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type | Description |
|------|-------------|
| uint | Area value  |

### Perimeter

csharp

public static uint Perimeter(KImage image, KMask mask = null)

public static uint Perimeter(IntPtr hImage, IntPtr hMask)

**Description**: Computes the perimeter of the foreground region, i.e., the boundary pixel length.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Binary image to analyze           |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type | Description     |
|------|-----------------|
| uint | Perimeter value |

### Centroid

csharp

public static RvPointF32 Centroid(KImage image, KMask mask = null)

public static RvPointF32 Centroid(IntPtr hImage, IntPtr hMask)

**Description**: Computes the centroid coordinates of the foreground region, i.e., the geometric center of all foreground pixels.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type       | Description                                            |
|------------|--------------------------------------------------------|
| RvPointF32 | Centroid coordinates (single-precision floating point) |

### Circular

csharp

public static double Circular(KImage image, KMask mask = null)

public static double Circular(IntPtr hImage, IntPtr hMask)

**Description**: Computes circularity, reflecting how close the object's shape is to a circle. An ideal circle has circularity 1; the more irregular the shape, the smaller the circularity.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description       |
|--------|-------------------|
| double | Circularity value |

### Slope

csharp

public static double Slope(KImage image, KMask mask = null)

public static double Slope(IntPtr hImage, IntPtr hMask)

**Description**: Computes the slope, representing the inclination angle of the object's major axis.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description |
|--------|-------------|
| double | Slope value |

**Notes**

- For elongated targets, the slope is the angle between the major axis and horizontal; for nearly symmetric targets, the slope may have no clear physical meaning.

### Density

csharp

public static double Density(KImage image, BlobPart subRegion)

public static double Density(IntPtr hImage, int subRegion)

**Description**: Computes the density (fill ratio) of the foreground object within the specified sub-region.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| subRegion | BlobPart / int | Sub-region identifier; the integer version uses BSP\_ series constants |

**Return Value**

| Type   | Description         |
|--------|---------------------|
| double | Density value (0~1) |

**Usage Example**

csharp

double whole = Dia.Density(img, BlobPart.Whole);

double east = Dia.Density(img, BlobPart.East);

double west = Dia.Density(img, BlobPart.West);

*// If east is significantly greater than west, the target's center of mass is to the right*

### GetBoundRect

csharp

public static RvRect GetBoundRect(KImage image, KMask mask = null)

public static RvRect GetBoundRect(IntPtr hImage, IntPtr hMask)

**Description**: Computes the axis-aligned bounding rectangle of the foreground region; sides parallel to the image coordinate axes.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description                                   |
|--------|-----------------------------------------------|
| RvRect | Bounding rectangle (left, top, right, bottom) |

### GetBoundBox

csharp

public static RvBox2D GetBoundBox(KImage image, KMask mask = null)

public static RvBox2D GetBoundBox(IntPtr hImage, IntPtr hMask)

**Description**: Computes the minimum bounding box (rotated rectangle), which rotates with the object's direction and more tightly encloses the object.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type    | Description                                      |
|---------|--------------------------------------------------|
| RvBox2D | Rotated rectangle (center, width, height, angle) |

### GetBoundBoxE1

csharp

public static RvBox2D GetBoundBoxE1(KImage image, int order, bool bIncludeBorder)

public static RvBox2D GetBoundBoxE1(IntPtr hImage, int order, bool bIncludeBorder)

**Description**: Computes the minimum bounding box with the specified order.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| order | int | Calculation order: GBB_AREA (minimum area) or GBB_PERIM (minimum perimeter) |
| bIncludeBorder | bool | Whether to include boundary pixels |

**Return Value**

| Type    | Description       |
|---------|-------------------|
| RvBox2D | Rotated rectangle |

### FindBoundRect

csharp

public static bool FindBoundRect(KImage image, ref RvRect seed, bool bInward)

public static bool FindBoundRect(IntPtr hImage, ref RvRect seed, bool bInward)

**Description**: Searches for the actual foreground boundary rectangle starting from the given initial rectangle, inward or outward.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| seed | ref RvRect | Input initial rectangle; output actual boundary rectangle |
| bInward | bool | true shrinks inward; false expands outward |

**Return Value**

| Type | Description                  |
|------|------------------------------|
| bool | Whether the search succeeded |

### BoxVertex

csharp

public static void BoxVertex(ref RvBox2D pBox, RvPointF32\[\] arrPts4)

**Description**: Computes the four vertex coordinates of the rotated rectangle and writes them to the array.

**Parameters**

| Parameter | Type           | Description                           |
|-----------|----------------|---------------------------------------|
| pBox      | ref RvBox2D    | Rotated rectangle                     |
| arrPts4   | RvPointF32\[\] | Output vertex array; length must be 4 |

**Return Value**: None.

**Notes**

- Vertex order is top-left, top-right, bottom-right, bottom-left.

**Usage Example**

csharp

RvBox2D box = Dia.GetBoundBox(img, null);

RvPointF32\[\] pts = new RvPointF32\[4\];

Dia.BoxVertex(ref box, pts);

*// pts\[0\]~pts\[3\] are the four vertices of the rotated rectangle*

## Region Analysis

### FindPolar

csharp

public static int FindPolar(KImage image, KMask mask, out RvPoint pos, bool bFindMinimum)

public static int FindPolar(KImage image, KMask mask, IntPtr pPosOut, bool bFindMinimum)

public static int FindPolar(IntPtr hImage, IntPtr hMask, IntPtr pPosOut, bool bFindMinimum)

**Description**: Finds the position of the extremum point within the specified region.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| mask / hMask | KMask / IntPtr | Mask limiting the analysis region |
| pos / pPosOut | out RvPoint / IntPtr | Output extremum point position |
| bFindMinimum | bool | true finds minimum; false finds maximum |

**Return Value**

| Type | Description                                                   |
|------|---------------------------------------------------------------|
| int  | Whether found (return value greater than 0 indicates success) |

### FindPolarEx

csharp

public static KFdox FindPolarEx(KImage image, KMask mask = null, KFdox reading = null)

public static IntPtr FindPolarEx(IntPtr hImage, IntPtr hMask, IntPtr reading)

**Description**: Extended version of extremum point finding, returning a structured result.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |
| reading        | KFdox / IntPtr  | Optional result container         |

**Return Value**

| Overload       | Return Type | Description                 |
|----------------|-------------|-----------------------------|
| KImage version | KFdox       | Structured result container |
| IntPtr version | IntPtr      | Result handle               |

### GetClearness

csharp

public static double GetClearness(KImage image, KMask mask = null)

public static double GetClearness(IntPtr hImage, IntPtr hMask)

**Description**: Computes the image clearness. A larger return value means the image is clearer with richer details.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type   | Description     |
|--------|-----------------|
| double | Clearness value |

**Usage Example**

csharp

KImage im = new KImage("blur.jpg");

im.Cast(PixelFormat.Gray);

KMask mask = new KMask(MaskShape.FilledEllipse, im.GetWidth(), im.GetHeight());

double sharp = Dia.GetClearness(im, mask);

### RetrievePosArray

csharp

public static int RetrievePosArray(IntPtr hImage, IntPtr pPosArray, int arrSize)

**Description**: Retrieves an array of positions of non-zero pixels in the image.

**Parameters**

| Parameter | Type   | Description                   |
|-----------|--------|-------------------------------|
| hImage    | IntPtr | Image handle to process       |
| pPosArray | IntPtr | Output position array pointer |
| arrSize   | int    | Array size                    |

**Return Value**

| Type | Description                        |
|------|------------------------------------|
| int  | Actual number of positions written |

## Threshold Estimation

### GetBinarizationOptimum

csharp

public static byte GetBinarizationOptimum(KImage image, KMask mask, int method)

public static byte GetBinarizationOptimum(IntPtr hImage, IntPtr hMask, int method)

**Description**: Estimates the optimal binarization threshold using the specified method.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image / hImage | KImage / IntPtr | Image to analyze |
| mask / hMask | KMask / IntPtr | Mask limiting the analysis region |
| method | int | Method constant, specified by the AB\_ series |

**Return Value**

| Type | Description                |
|------|----------------------------|
| byte | Computed threshold (0~255) |

### EstimateCannyContrast

csharp

public static bool EstimateCannyContrast(KImage image, KMask mask, int method, float extra, ref float minContrast, ref float maxContrast)

public static bool EstimateCannyContrast(IntPtr hImage, IntPtr hMask, int method, float extra, ref float minContrast, ref float maxContrast)

**Description**: Estimates the minimum and maximum contrast thresholds for the Canny operator.

**Parameters**

| Parameter      | Type            | Description                           |
|----------------|-----------------|---------------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                      |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region     |
| method         | int             | Method, specified by the ECC\_ series |
| extra          | float           | Method-specific additional parameter  |
| minContrast    | ref float       | Output minimum contrast threshold     |
| maxContrast    | ref float       | Output maximum contrast threshold     |

**Return Value**

| Type | Description                                             |
|------|---------------------------------------------------------|
| bool | Returns true on successful estimation; false on failure |

**Usage Example**

csharp

float minC = 0, maxC = 0;

if (Dia.EstimateCannyContrast(img, null, Dia.ECC_ADAPTIVE, 0.15f, ref minC, ref maxC))

{

*// Use minC, maxC as Canny dual thresholds*

}

**Notes**

- The meaning of extra varies with method: for ECC_ADAPTIVE it is the sensitivity coefficient; for ECC_HALIKE it is the histogram binning parameter; for ECC_MEDINA typically pass 0.

### EstimateImmerkaerNoise

csharp

public static float EstimateImmerkaerNoise(KImage image, KMask mask = null)

public static float EstimateImmerkaerNoise(IntPtr hImage, IntPtr hMask)

**Description**: Estimates the image noise level (Immerkær algorithm). Larger return values indicate stronger noise.

**Parameters**

| Parameter      | Type            | Description                       |
|----------------|-----------------|-----------------------------------|
| image / hImage | KImage / IntPtr | Image to analyze                  |
| mask / hMask   | KMask / IntPtr  | Mask limiting the analysis region |

**Return Value**

| Type  | Description          |
|-------|----------------------|
| float | Noise level estimate |

## KFdox Structured Result Description

Several analysis functions return KFdox objects, a tree-like data container for organizing multidimensional results. Common structures:

- **HistogramEx**: Root node → each channel → each gray level (stores Int64 pixel count)

- **ProjectEx**: Root node → each channel → each projection data point (stores Int64 accumulated value)

- **CalcGrayStatsEx**: Root node → each statistic item (stores Double value); indices 0, 1, 2 corresponding to total, mean, variance

- **CountPixelsEx / SummaryEx / VarianceEx / AverageEx / FindPolarEx**: Flat structure or single-level child nodes

**Common Access Methods**

| Method | Description |
|----|----|
| int GetCount() | Number of child nodes |
| KFdox GetChild(int index) | Gets a child node by index |
| bool GetInt64(out long v) | Reads the current node as a long integer |
| bool GetDouble(out double v) | Reads the current node as a double-precision float |
| double GetDoubleAt(int index, out double v) | Reads a float value at the specified index |

**Key Points**

- Objects returning KFdox should be released when no longer needed to avoid resource accumulation.

- If the provided parameter is null, the function internally creates and returns a new KFdox; if an existing instance is passed, the result is written to that instance and the same handle is returned.

- Check whether the returned object is null before traversal.
