# KImage Class Detailed Function Usage Guide

## Overview

KImage is an image object class used to represent digital images and support multiple pixel formats. It provides image creation, loading, saving, format conversion, channel splitting and merging, pixel filling, cloning, memory export, and other functions. It is the data carrier for image processing and analysis.

KImage is divided by function into: construction and lifecycle, image creation and destruction, pixel filling, size and property queries, loading and saving, cloning and copying, format conversion, and channel splitting and merging.

## Constants

### Binary Image Values

| Name | Type | Value | Description |
|----|----|----|----|
| BIN_ONE | int | 0 | Storage value for "1" in binary images (displays as black) |
| BIN_ZERO | int | 255 | Storage value for "0" in binary images (displays as white) |

**Notes**

The "1" and "0" of binary images are mapped to 0 and 255 respectively for storage, opposite to common intuition. When using, note: BIN_ONE corresponds to foreground displaying as black, BIN_ZERO corresponds to background displaying as white.

### Image File Formats (IFF\_ Series)

File format extensions defined as strings, used by methods such as Save and LoadEx.

| Constant | Value | Constant | Value |
|----|----|----|----|
| IFF_DEFAULT | "bmp" | IFF_JPG / IFF_JPEG | "jpg" / "jpeg" |
| IFF_PNG | "png" | IFF_TIFF / IFF_TIF | "tiff" / "tif" |
| IFF_GIF | "gif" | IFF_TGA / IFF_TARGA | "tga" / "targa" |
| IFF_PCX | "pcx" | IFF_PSD | "psd" |
| IFF_PBM / IFF_PGM / IFF_PPM | "pbm" / "pgm" / "ppm" | IFF_HDR | "hdr" |
| IFF_EXR | "exr" | IFF_J2K / IFF_JP2 | "j2k" / "jp2" |
| IFF_PFM | "pfm" | IFF_ICO | "ico" |

Also includes IFF_JNG, IFF_KOALA, IFF_LBM, IFF_IFF, IFF_MNG, IFF_PCD, IFF_RAS, IFF_WBMP, IFF_CUT, IFF_XBM, IFF_XPM, IFF_DDS, IFF_FAXG3, IFF_SGI, IFF_PICT, IFF_RAW, etc.

### Image Memory Formats (IMF\_ Series)

Integer constants defining encoding formats, used by methods such as ExportImageToMemory and LoadImageInMemory.

| Constant    | Value | Constant            | Value |
|-------------|-------|---------------------|-------|
| IMF_DEFAULT | -1    | IMF_BMP             | 0     |
| IMF_ICO     | 1     | IMF_JPEG / IMF_JPG  | 2     |
| IMF_PNG     | 13    | IMF_TIFF / IMF_TIF  | 18    |
| IMF_GIF     | 25    | IMF_TGA / IMF_TARGA | 17    |
| IMF_J2K     | 30    | IMF_JP2             | 31    |

**Notes**

The IMF\_ series is used for in-memory image encoding; the IFF\_ series for disk file extensions. They correspond one-to-one, but have different uses: the former for ExportImageToMemory and LoadImageInMemory, the latter for Save and Load.

### Fill Data Types (RDT\_ Series)

Array element type identifiers for the Flood method family.

| Name       | Value | Description                  |
|------------|-------|------------------------------|
| RDT_BYTE   | 1     | Byte array                   |
| RDT_INT    | 4     | Integer array                |
| RDT_FLOAT  | 5     | Single-precision float array |
| RDT_DOUBLE | 6     | Double-precision float array |

## Constructors

### KImage()

csharp

public KImage()

**Description**: Creates a default image, format BGR, size 120$\times$80.

**Parameters**: None.

**Return Value**: None (constructor).

**Usage Example**

csharp

KImage img = new KImage();

### KImage(PixelFormat pixfmt)

csharp

public KImage(PixelFormat pixfmt)

**Description**: Creates an image with the specified pixel format, default size 120$\times$80.

**Parameters**

| Parameter | Type        | Description  |
|-----------|-------------|--------------|
| pixfmt    | PixelFormat | Pixel format |

**Usage Example**

csharp

KImage img = new KImage(PixelFormat.Gray);

### KImage(string strFilePath)

csharp

public KImage(string strFilePath)

**Description**: Creates a default image then immediately loads from the specified file.

**Parameters**

| Parameter   | Type   | Description     |
|-------------|--------|-----------------|
| strFilePath | string | Image file path |

**Usage Example**

csharp

KImage img = new KImage("..\\samples\\lenna.png");

**Notes**

- Internally first creates a 120$\times$80 BGR image, then calls Load to load the specified file. If loading fails, the object retains an empty image.

### KImage(IntPtr hImage, bool bAttach = true)

csharp

public KImage(IntPtr hImage, bool bAttach = true)

**Description**: Wraps an existing image handle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| hImage | IntPtr | Existing image handle |
| bAttach | bool | true means the handle is externally owned and this instance will not release it; false means this instance takes ownership and will release it on destruction |

**Usage Example**

csharp

KImage img = new KImage(handle, false);

**Notes**

- Clarify handle ownership to avoid double release or memory leak.

### KImage(PixelFormat pixfmt, int width, int height)

csharp

public KImage(PixelFormat pixfmt, int width, int height)

**Description**: Creates an image with the specified pixel format, width, and height.

**Parameters**

| Parameter | Type        | Description           |
|-----------|-------------|-----------------------|
| pixfmt    | PixelFormat | Pixel format          |
| width     | int         | Image width (pixels)  |
| height    | int         | Image height (pixels) |

**Usage Example**

csharp

KImage img = new KImage(PixelFormat.BGR, 640, 480);

## Image Creation and Destruction

### Create

csharp

public bool Create(PixelFormat pixfmt, int width, int height)

**Description**: Creates a new image with specified format and size, releasing the old handle.

**Parameters**

| Parameter | Type        | Description  |
|-----------|-------------|--------------|
| pixfmt    | PixelFormat | Pixel format |
| width     | int         | Image width  |
| height    | int         | Image height |

**Return Value**

| Type | Description                              |
|------|------------------------------------------|
| bool | Returns true on success; otherwise false |

### Destroy

csharp

public static void Destroy(ref IntPtr hImage)

**Description**: Destroys the specified image handle and sets the reference to IntPtr.Zero.

**Parameters**

| Parameter | Type       | Description             |
|-----------|------------|-------------------------|
| hImage    | ref IntPtr | Image handle to destroy |

**Return Value**: None.

## Pixel Filling

### Flood(byte level)

csharp

public void Flood(byte level)

**Description**: Fills the entire image with the specified grayscale value. Suitable for grayscale and binary images.

**Parameters**

| Parameter | Type | Description             |
|-----------|------|-------------------------|
| level     | byte | Grayscale value (0~255) |

**Return Value**: None.

**Usage Example**

csharp

img.Flood(255); *// Fill with white*

### Flood(uint color)

csharp

public void Flood(uint color)

**Description**: Fills the entire image with the specified 32-bit color value. Suitable for color images.

**Parameters**

| Parameter | Type | Description                     |
|-----------|------|---------------------------------|
| color     | uint | 32-bit color value (0xAARRGGBB) |

**Return Value**: None.

**Usage Example**

csharp

uint color = Pool.RGB(255, 0, 0);

img.Flood(color);

### Flood(byte\[\] data)

csharp

public void Flood(byte\[\] data)

**Description**: Fills the image with a byte array; length should match the image buffer.

**Parameters**

| Parameter | Type     | Description |
|-----------|----------|-------------|
| data      | byte\[\] | Fill data   |

**Return Value**: None.

### Flood(int\[\] data)

csharp

public void Flood(int\[\] data)

**Description**: Fills the image with an integer array.

**Parameters**

| Parameter | Type    | Description |
|-----------|---------|-------------|
| data      | int\[\] | Fill data   |

**Return Value**: None.

### Flood(float\[\] data)

csharp

public void Flood(float\[\] data)

**Description**: Fills the image with a single-precision float array.

**Parameters**

| Parameter | Type      | Description |
|-----------|-----------|-------------|
| data      | float\[\] | Fill data   |

**Return Value**: None.

### Flood(double\[\] data)

csharp

public void Flood(double\[\] data)

**Description**: Fills the image with a double-precision float array.

**Parameters**

| Parameter | Type       | Description |
|-----------|------------|-------------|
| data      | double\[\] | Fill data   |

**Return Value**: None.

### Flood(byte\[\] data, double minValue, double maxValue)

csharp

public void Flood(byte\[\] data, double minValue, double maxValue)

**Description**: Byte array fill with value range. Array values are mapped within \[minValue, maxValue\] to the image's effective grayscale range.

**Parameters**

| Parameter | Type     | Description |
|-----------|----------|-------------|
| data      | byte\[\] | Fill data   |
| minValue  | double   | Lower bound |
| maxValue  | double   | Upper bound |

**Return Value**: None.

### Flood(int\[\] data, double minValue, double maxValue)

csharp

public void Flood(int\[\] data, double minValue, double maxValue)

**Description**: Integer array fill with value range.

**Parameters**

| Parameter | Type    | Description |
|-----------|---------|-------------|
| data      | int\[\] | Fill data   |
| minValue  | double  | Lower bound |
| maxValue  | double  | Upper bound |

**Return Value**: None.

### Flood(float\[\] data, double minValue, double maxValue)

csharp

public void Flood(float\[\] data, double minValue, double maxValue)

**Description**: Single-precision float array fill with value range.

**Parameters**

| Parameter | Type      | Description |
|-----------|-----------|-------------|
| data      | float\[\] | Fill data   |
| minValue  | double    | Lower bound |
| maxValue  | double    | Upper bound |

**Return Value**: None.

### Flood(double\[\] data, double minValue, double maxValue)

csharp

public void Flood(double\[\] data, double minValue, double maxValue)

**Description**: Double-precision float array fill with value range.

**Parameters**

| Parameter | Type       | Description |
|-----------|------------|-------------|
| data      | double\[\] | Fill data   |
| minValue  | double     | Lower bound |
| maxValue  | double     | Upper bound |

**Return Value**: None.

### FloodEx(byte\[\] data)

csharp

public void FloodEx(byte\[\] data)

**Description**: Writes the byte array directly to the image buffer without type conversion.

**Parameters**

| Parameter | Type     | Description                                     |
|-----------|----------|-------------------------------------------------|
| data      | byte\[\] | Fill data; length should match the image buffer |

**Return Value**: None.

## Size and Property Queries

### SetSize

csharp

public void SetSize(int width, int height)

**Description**: Modifies the image size; original data is reset.

**Parameters**

| Parameter | Type | Description |
|-----------|------|-------------|
| width     | int  | New width   |
| height    | int  | New height  |

**Return Value**: None.

### GetSize

csharp

public UIntPtr GetSize()

**Description**: Gets the total bytes of the image data buffer.

**Return Value**

| Type    | Description       |
|---------|-------------------|
| UIntPtr | Buffer byte count |

### GetPixelFormat

csharp

public PixelFormat GetPixelFormat()

**Description**: Gets the image pixel format.

**Return Value**

| Type        | Description  |
|-------------|--------------|
| PixelFormat | Pixel format |

### GetData

csharp

public IntPtr GetData()

**Description**: Gets a pointer to the image data buffer.

**Return Value**

| Type   | Description         |
|--------|---------------------|
| IntPtr | Data buffer pointer |

### GetPitch

csharp

public int GetPitch()

**Description**: Gets bytes per row (row stride).

**Return Value**

| Type | Description   |
|------|---------------|
| int  | Bytes per row |

### GetWidth / GetHeight

csharp

public int GetWidth()

public int GetHeight()

**Description**: Gets the image width or height (pixels).

**Return Value**

| Type | Description     |
|------|-----------------|
| int  | Width or height |

### GetBytesPerPixel

csharp

public int GetBytesPerPixel()

**Description**: Gets bytes per pixel.

**Return Value**

| Type | Description     |
|------|-----------------|
| int  | Bytes per pixel |

### GetDepth

csharp

public int GetDepth()

**Description**: Gets bits per channel (typically 8).

**Return Value**

| Type | Description |
|------|-------------|
| int  | Bit depth   |

### GetChannels

csharp

public int GetChannels()

**Description**: Gets the image channel count.

**Return Value**

| Type | Description   |
|------|---------------|
| int  | Channel count |

## Loading and Saving

### Load

csharp

public bool Load(string strFileName)

**Description**: Loads an image from file; format inferred from extension. Releases the existing handle before loading.

**Parameters**

| Parameter   | Type   | Description     |
|-------------|--------|-----------------|
| strFileName | string | Image file path |

**Return Value**

| Type | Description                              |
|------|------------------------------------------|
| bool | Returns true on success; otherwise false |

**Usage Example**

csharp

KImage img = new KImage();

if (img.Load("..\\samples\\lenna.png"))

{

*// Use img*

}

### LoadEx

csharp

public bool LoadEx(string strFileName, string strFormat, int flag = 0)

**Description**: Loads an image from file with an explicit format string.

**Parameters**

| Parameter   | Type   | Description               |
|-------------|--------|---------------------------|
| strFileName | string | Image file path           |
| strFormat   | string | Format string             |
| flag        | int    | Flag parameter, default 0 |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether loading succeeded |

### LoadImageInMemory

csharp

public bool LoadImageInMemory(byte\[\] data)

**Description**: Decodes and loads an image from an in-memory byte array.

**Parameters**

| Parameter | Type     | Description              |
|-----------|----------|--------------------------|
| data      | byte\[\] | Byte array of image data |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether loading succeeded |

**Usage Example**

csharp

byte\[\] fileData = File.ReadAllBytes("..\\samples\\lenna.png");

KImage img = new KImage();

img.LoadImageInMemory(fileData);

### Save

csharp

public bool Save(string strFileName, string strFormat = "bmp", int flag = 0)

**Description**: Saves the image to file; format specified by strFormat, default bmp.

**Parameters**

| Parameter   | Type   | Description                  |
|-------------|--------|------------------------------|
| strFileName | string | Target file path             |
| strFormat   | string | Format string, default "bmp" |
| flag        | int    | Flag parameter, default 0    |

**Return Value**

| Type | Description              |
|------|--------------------------|
| bool | Whether saving succeeded |

**Usage Example**

csharp

img.Save("output.png", "png");

### ExportImageToMemory

csharp

public uint ExportImageToMemory(int nFormat, int flag, ref byte\[\] data)

**Description**: Encodes the image and exports it to an in-memory buffer, returning the actual bytes written.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| nFormat | int | Encoding format, specified by IMF\_ series constants |
| flag | int | Flag parameter |
| data | ref byte\[\] | Pre-allocated output buffer |

**Return Value**

| Type | Description          |
|------|----------------------|
| uint | Actual bytes written |

**Usage Example**

csharp

byte\[\] data = new byte\[1024 \* 1024\];

uint size = img.ExportImageToMemory(KImage.IMF_JPG, 0, ref data);

**Notes**

- The caller must pre-allocate a sufficiently large data; otherwise writing may fail.

- The returned byte count indicates the length of valid data.

## Cloning and Copying

### Clone()

csharp

public KImage Clone()

**Description**: Deep copy; creates a fully independent image copy unaffected by the original.

**Return Value**

| Type   | Description                             |
|--------|-----------------------------------------|
| KImage | New image copy; returns null on failure |

**Usage Example**

csharp

KImage copy = img.Clone();

### Clone(bool bDummy)

csharp

public KImage Clone(bool bDummy)

**Description**: Shadow clone. When bDummy is true, shares the data buffer with the original image.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| bDummy | bool | When true, shares data with the original image; when false, behaves the same as Clone() |

**Return Value**

| Type   | Description                             |
|--------|-----------------------------------------|
| KImage | New image copy; returns null on failure |

**Notes**

- When using shadow cloning, all copies must be destroyed **before** the original image; otherwise the copies accessing the released buffer will cause a crash.

### Clone(out IntPtr hImage)

csharp

public bool Clone(out IntPtr hImage)

**Description**: Clones and returns a handle for the caller to manage.

**Parameters**

| Parameter | Type       | Description                       |
|-----------|------------|-----------------------------------|
| hImage    | out IntPtr | Output handle of the clone result |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether cloning succeeded |

### Copy

csharp

public static void Copy(IntPtr src, IntPtr dest)

**Description**: Copies data from the src image to dest.

**Parameters**

| Parameter | Type   | Description              |
|-----------|--------|--------------------------|
| src       | IntPtr | Source image handle      |
| dest      | IntPtr | Destination image handle |

**Return Value**: None.

### CopyFrom

csharp

public void CopyFrom(IntPtr hImage)

**Description**: Copies data from the specified handle image to the current image.

**Parameters**

| Parameter | Type   | Description         |
|-----------|--------|---------------------|
| hImage    | IntPtr | Source image handle |

**Return Value**: None.

## Format Conversion

### Cast

csharp

public void Cast(PixelFormat pixelFormat)

**Description**: Converts the image to the specified pixel format; size unchanged, data reallocated.

**Parameters**

| Parameter   | Type        | Description         |
|-------------|-------------|---------------------|
| pixelFormat | PixelFormat | Target pixel format |

**Return Value**: None.

**Usage Example**

csharp

img.Cast(PixelFormat.Gray);

### Convert24To8

csharp

public void Convert24To8(RgbToGray method)

**Description**: Converts a 24-bit color image to an 8-bit grayscale image by the specified method.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| method | RgbToGray | Grayscale conversion method, e.g., Default, ValueOfHSV, MaxOfRgb, MinOfRgb, AvgOfRgb |

**Return Value**: None.

**Usage Example**

csharp

img.Convert24To8(RgbToGray.Default);

## Channel Splitting and Merging

### Split(KImage red, KImage green, KImage blue)

csharp

public void Split(KImage red, KImage green, KImage blue)

**Description**: Splits a BGR or BGRA image into red, green, blue channels, output to three target grayscale images.

**Parameters**

| Parameter | Type   | Description                 |
|-----------|--------|-----------------------------|
| red       | KImage | Red channel output target   |
| green     | KImage | Green channel output target |
| blue      | KImage | Blue channel output target  |

**Return Value**: None.

**Usage Example**

csharp

KImage imgray = new KImage(PixelFormat.Gray, img.GetWidth(), img.GetHeight());

img.Split(imgray, null, null); *// Extract red channel*

**Notes**

- The source image must be in BGR or BGRA format.

- A target may be null to skip that channel.

### Split(KImage red, KImage green, KImage blue, KImage alpha)

csharp

public void Split(KImage red, KImage green, KImage blue, KImage alpha)

**Description**: Splits a BGRA image into four channels.

**Parameters**

| Parameter        | Type   | Description                 |
|------------------|--------|-----------------------------|
| red, green, blue | KImage | RGB channel output targets  |
| alpha            | KImage | Alpha channel output target |

**Return Value**: None.

**Notes**

- The source image must be in BGRA format.

### Merge

csharp

public void Merge(KImage red, KImage green, KImage blue)

**Description**: Merges three grayscale images into the current color image by red, green, blue channels.

**Parameters**

| Parameter | Type   | Description                   |
|-----------|--------|-------------------------------|
| red       | KImage | Red channel grayscale image   |
| green     | KImage | Green channel grayscale image |
| blue      | KImage | Blue channel grayscale image  |

**Return Value**: None.

**Usage Example**

csharp

KImage imRed = new KImage(PixelFormat.Gray, 120, 100);

KImage imGreen = new KImage(PixelFormat.Gray, 120, 100);

KImage imBlue = new KImage(PixelFormat.Gray, 120, 100);

KImage img = new KImage(PixelFormat.BGR, 120, 100);

FillRandom(imRed);

img.Merge(imRed, imGreen, imBlue);

## Static Helper Methods

### rvCreateImage

csharp

public static IntPtr rvCreateImage(int type, int width, int height)

**Description**: Creates an image with an integer format identifier and returns the image handle. Suitable for scenarios requiring underlying handles.

**Parameters**

| Parameter | Type | Description             |
|-----------|------|-------------------------|
| type      | int  | Pixel format identifier |
| width     | int  | Width                   |
| height    | int  | Height                  |

**Return Value**

| Type   | Description             |
|--------|-------------------------|
| IntPtr | Handle of the new image |

### rvDestroyImage

csharp

public static void rvDestroyImage(IntPtr image)

**Description**: Destroys the specified image handle.

**Parameters**

| Parameter | Type   | Description             |
|-----------|--------|-------------------------|
| image     | IntPtr | Image handle to destroy |

**Return Value**: None.

### rviFloodE1

csharp

public static void rviFloodE1(IntPtr image, uint color)

**Description**: Fills the image with a handle and color value.

**Parameters**

| Parameter | Type   | Description        |
|-----------|--------|--------------------|
| image     | IntPtr | Image handle       |
| color     | uint   | 32-bit color value |

**Return Value**: None.

### rviSetSize

csharp

public static bool rviSetSize(IntPtr image, int width, int height)

**Description**: Modifies the image size via a handle.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| image     | IntPtr | Image handle |
| width     | int    | New width    |
| height    | int    | New height   |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |
