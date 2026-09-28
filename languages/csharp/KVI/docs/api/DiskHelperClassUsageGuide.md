# DiskHelper Class Detailed Usage Guide

## Overview

DiskHelper is a utility class for disk file read/write operations, providing creation, opening, and closing of binary files, as well as serialization and deserialization of structured data. Its design goal is to support persistent saving and loading of complex objects such as images, masks, and BLOBs, while also providing item-by-item read/write interfaces for basic data types and structs, suitable for implementing custom file formats.

DiskHelper adopts a **file header + sections** organization: the file is opened with BeginFileOut / BeginFileIn, and may contain multiple named sections internally, each opened and closed with BeginSectionOut / BeginSectionIn. This structure facilitates saving multiple types of objects in the same file, and also facilitates locating the required content by tag when reading.

The methods are divided by function into: lifecycle, file operations, static image/mask/BLOB saving, file sessions, section operations, object encoding/decoding, data writing, and data reading.

## Lifecycle

### DiskHelper()

csharp

public DiskHelper()

**Description**: Creates an instance not associated with a file handle. CreateFile or OpenFile must be called before read/write operations.

**Parameters**: None.

**Return Value**: None (constructor).

**Usage Example**

csharp

DiskHelper helper = new DiskHelper();

### DiskHelper(IntPtr existingHandle, bool attach)

csharp

public DiskHelper(IntPtr existingHandle, bool attach)

**Description**: Wraps an existing file handle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| existingHandle | IntPtr | Existing file handle |
| attach | bool | true means the handle is externally owned and this instance will not close it; false means this instance takes ownership and will close it on destruction |

**Return Value**: None (constructor).

**Usage Example**

csharp

DiskHelper helper = new DiskHelper(handle, false);

**Notes**

- If existingHandle is IntPtr.Zero, throws ArgumentException.

### CloseFile

csharp

public void CloseFile()

**Description**: Closes the current file and releases the handle. Safe to call repeatedly.

**Parameters**: None.

**Return Value**: None (void).

**Usage Example**

csharp

helper.CloseFile();

## File Operations

### CreateFile

csharp

public bool CreateFile(string strFilePath)

**Description**: Creates a new file for writing. Releases any existing old handle before execution, so repeated calls are safe.

**Parameters**

| Parameter   | Type   | Description      |
|-------------|--------|------------------|
| strFilePath | string | Target file path |

**Return Value**

| Type | Description                                          |
|------|------------------------------------------------------|
| bool | Returns true on successful creation; otherwise false |

**Usage Example**

csharp

bool ok = helper.CreateFile("data.bin");

**Workflow**

CreateFile → BeginFileOut → write data → EndFileOut → CloseFile (or Dispose).

### OpenFile

csharp

public bool OpenFile(string strFilePath)

**Description**: Opens an existing file for reading. Also releases the old handle first.

**Parameters**

| Parameter   | Type   | Description      |
|-------------|--------|------------------|
| strFilePath | string | Target file path |

**Return Value**

| Type | Description                                         |
|------|-----------------------------------------------------|
| bool | Returns true on successful opening; otherwise false |

**Usage Example**

csharp

bool ok = helper.OpenFile("data.bin");

**Workflow**

OpenFile → BeginFileIn → read data → EndFileIn → CloseFile (or Dispose).

### OpenFileInMemory

csharp

public bool OpenFileInMemory(IntPtr pFileData, uint nSize, ref int errCode)

**Description**: Opens a file from an in-memory buffer, for handling file data already loaded into memory (such as network transmission or embedded resources).

**Parameters**

| Parameter | Type    | Description                    |
|-----------|---------|--------------------------------|
| pFileData | IntPtr  | Pointer to the file data       |
| nSize     | uint    | Number of bytes                |
| errCode   | ref int | Error code returned on failure |

**Return Value**

| Type | Description                                         |
|------|-----------------------------------------------------|
| bool | Returns true on successful opening; otherwise false |

**Usage Example**

csharp

int err = 0;

bool ok = helper.OpenFileInMemory(dataPtr, (uint)length, ref err);

## Static File Operations

The following methods are static, requiring no DiskHelper instance creation; they directly operate on image, mask, and BLOB handles.

### LoadBitmap

csharp

public static bool LoadBitmap(IntPtr image, string strFileName)

**Description**: Loads an image from file into the specified handle.

**Parameters**

| Parameter   | Type   | Description         |
|-------------|--------|---------------------|
| image       | IntPtr | Target image handle |
| strFileName | string | Image file path     |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether loading succeeded |

### SaveBitmap

csharp

public static bool SaveBitmap(IntPtr image, string strFileName)

**Description**: Saves an image to file.

**Parameters**

| Parameter   | Type   | Description          |
|-------------|--------|----------------------|
| image       | IntPtr | Image handle to save |
| strFileName | string | Target file path     |

**Return Value**

| Type | Description              |
|------|--------------------------|
| bool | Whether saving succeeded |

### SaveMask

csharp

public static bool SaveMask(IntPtr mask, string strFileName)

**Description**: Saves a mask to file.

**Parameters**

| Parameter   | Type   | Description         |
|-------------|--------|---------------------|
| mask        | IntPtr | Mask handle to save |
| strFileName | string | Target file path    |

**Return Value**

| Type | Description              |
|------|--------------------------|
| bool | Whether saving succeeded |

### SaveBlob

csharp

public static bool SaveBlob(IntPtr blob, string strFileName)

**Description**: Saves a BLOB to file.

**Parameters**

| Parameter   | Type   | Description         |
|-------------|--------|---------------------|
| blob        | IntPtr | BLOB handle to save |
| strFileName | string | Target file path    |

**Return Value**

| Type | Description              |
|------|--------------------------|
| bool | Whether saving succeeded |

**Usage Example**

csharp

DiskHelper.SaveBitmap(img.Handle, "out.bmp");

## File Sessions

### BeginFileOut(int version, string strTag)

csharp

public bool BeginFileOut(int version, string strTag)

**Description**: Opens a write session with the specified version number and file tag.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| version | int | Version number, used for compatibility determination when reading |
| strTag | string | File tag, identifying the file's purpose |

**Return Value**

| Type | Description                             |
|------|-----------------------------------------|
| bool | Whether the session opened successfully |

### BeginFileOut()

csharp

public bool BeginFileOut()

**Description**: Opens a write session with default parameters.

**Parameters**: None.

**Return Value**

| Type | Description                             |
|------|-----------------------------------------|
| bool | Whether the session opened successfully |

### EndFileOut

csharp

public bool EndFileOut()

**Description**: Ends the write session, flushing buffered content to disk.

**Parameters**: None.

**Return Value**

| Type | Description                            |
|------|----------------------------------------|
| bool | Whether the session ended successfully |

### BeginFileIn(out int pVersion, ref string strFileTag)

csharp

public bool BeginFileIn(out int pVersion, ref string strFileTag)

**Description**: Reads the version number and tag from the file header, opening a read session.

**Parameters**

| Parameter  | Type       | Description                |
|------------|------------|----------------------------|
| pVersion   | out int    | Output file version number |
| strFileTag | ref string | Output file tag            |

**Return Value**

| Type | Description                             |
|------|-----------------------------------------|
| bool | Whether the session opened successfully |

**Usage Example**

csharp

int version;

string tag = "";

helper.BeginFileIn(out version, ref tag);

### BeginFileIn()

csharp

public bool BeginFileIn()

**Description**: Enters read state directly without reading the file header.

**Parameters**: None.

**Return Value**

| Type | Description                             |
|------|-----------------------------------------|
| bool | Whether the session opened successfully |

### EndFileIn

csharp

public bool EndFileIn()

**Description**: Ends the read session.

**Parameters**: None.

**Return Value**

| Type | Description                            |
|------|----------------------------------------|
| bool | Whether the session ended successfully |

## Section Operations

### BeginSectionOut

csharp

public bool BeginSectionOut(uint tag, int version, out uint pCurPos)

**Description**: Begins a write section.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| tag | uint | Section identifier |
| version | int | Section version |
| pCurPos | out uint | Output current file position for backfilling section info at end |

**Return Value**

| Type | Description                            |
|------|----------------------------------------|
| bool | Whether the section began successfully |

### EndSectionOut

csharp

public bool EndSectionOut(uint tag, uint nPrevPos)

**Description**: Ends a write section.

**Parameters**

| Parameter | Type | Description                                |
|-----------|------|--------------------------------------------|
| tag       | uint | Section identifier                         |
| nPrevPos  | uint | Position value returned by BeginSectionOut |

**Return Value**

| Type | Description                            |
|------|----------------------------------------|
| bool | Whether the section ended successfully |

### BeginSectionIn

csharp

public bool BeginSectionIn(uint tag, out int pVersion, out uint pObjSize)

**Description**: Reads the section with the specified tag.

**Parameters**

| Parameter | Type     | Description              |
|-----------|----------|--------------------------|
| tag       | uint     | Section identifier       |
| pVersion  | out int  | Output section version   |
| pObjSize  | out uint | Output section data size |

**Return Value**

| Type | Description                            |
|------|----------------------------------------|
| bool | Whether the section began successfully |

### EndSectionIn

csharp

public bool EndSectionIn(uint tag)

**Description**: Ends the read section, moving the read position to the section end.

**Parameters**

| Parameter | Type | Description        |
|-----------|------|--------------------|
| tag       | uint | Section identifier |

**Return Value**

| Type | Description                            |
|------|----------------------------------------|
| bool | Whether the section ended successfully |

### PickSectionTag

csharp

public bool PickSectionTag(ref uint pTag, ref int pVersion, ref uint pObjSize)

**Description**: Scans forward to the next section's tag, version, and size without entering the section content. Used to traverse all sections in a file.

**Parameters**

| Parameter | Type     | Description                     |
|-----------|----------|---------------------------------|
| pTag      | ref uint | Input/output section identifier |
| pVersion  | ref int  | Input/output section version    |
| pObjSize  | ref uint | Input/output section size       |

**Return Value**

| Type | Description                                       |
|------|---------------------------------------------------|
| bool | Whether the next section was scanned successfully |

**Usage Example**

csharp

uint tag = 0; int ver = 0; uint size = 0;

while (helper.PickSectionTag(ref tag, ref ver, ref size))

{

helper.BeginSectionIn(tag, out int v, out uint s);

*// Read content by tag*

helper.EndSectionIn(tag);

}

## Object Encoding and Decoding

### EncodeImage

csharp

public bool EncodeImage(IntPtr image, int nFormat)

public bool EncodeImage(KImage image, int nFormat)

**Description**: Encodes an image and writes it to file.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| image | IntPtr / KImage | Image to encode |
| nFormat | int | Encoding format, specified by KImage.IMF\_ series constants |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### DecodeImage

csharp

public bool DecodeImage(IntPtr image)

public bool DecodeImage(KImage image)

public KImage DecodeImage()

**Description**: Decodes an image from file.

**Parameters**

| Parameter | Type            | Description  |
|-----------|-----------------|--------------|
| image     | IntPtr / KImage | Target image |

**Return Value**

| Method | Return Type | Description |
|----|----|----|
| DecodeImage(IntPtr) | bool | Whether successful |
| DecodeImage(KImage) | bool | Whether successful |
| DecodeImage() | KImage | Returns a newly created image object; returns null on failure |

**Usage Example**

csharp

KImage restored = helper.DecodeImage();

**Notes**

- The parameterless overload returns a new KImage object; the handle is released by KImage taking ownership, and the caller need not manage the handle manually. The return value may be null; check before use.

### EncodeMask

csharp

public bool EncodeMask(IntPtr mask)

public bool EncodeMask(KMask mask)

**Description**: Encodes a mask and writes it to file.

**Parameters**

| Parameter | Type           | Description    |
|-----------|----------------|----------------|
| mask      | IntPtr / KMask | Mask to encode |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### DecodeMask

csharp

public bool DecodeMask(IntPtr mask, bool bRestrictSize)

public bool DecodeMask(KMask mask, bool bRestrictSize)

**Description**: Decodes a mask from file.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| mask | IntPtr / KMask | Target mask |
| bRestrictSize | bool | When true, restricts the read size to not exceed the target object's existing size |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### EncodeBlob / DecodeBlob

csharp

public bool EncodeBlob(IntPtr blob)

public bool EncodeBlob(KBlob blob)

public bool DecodeBlob(IntPtr blob)

public bool DecodeBlob(KBlob blob)

**Description**: Encodes or decodes a BLOB object.

**Parameters**

| Parameter | Type           | Description              |
|-----------|----------------|--------------------------|
| blob      | IntPtr / KBlob | BLOB to encode or decode |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

## Data Writing

### Write(string strVal) / WriteE1(string strVal)

csharp

public bool Write(string strVal)

public bool WriteE1(string strVal)

**Description**: Writes a string.

- Write: \0-terminated; null treated as empty string.

- WriteE1: length-prefixed; supports content containing \0 or special characters; must be read with ReadE1.

**Parameters**

| Parameter | Type   | Description     |
|-----------|--------|-----------------|
| strVal    | string | String to write |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

**Notes**

- Long strings (over 2400 characters) should preferably use WriteE1.

### Write(char/short/ushort/bool/int/uint/long/ulong/float/double)

csharp

public bool Write(char val)

public bool Write(short val)

public bool Write(ushort val)

public bool Write(bool val)

public bool Write(int val)

public bool Write(uint val)

public bool Write(long val)

public bool Write(ulong val)

public bool Write(float val)

public bool Write(double val)

**Description**: Writes the corresponding basic type value.

**Parameters**

| Parameter | Type               | Description    |
|-----------|--------------------|----------------|
| val       | Corresponding type | Value to write |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Write(RvPoint / RvPointF32 / RvPointF64)

csharp

public bool Write(RvPoint val)

public bool Write(RvPointF32 val)

public bool Write(RvPointF64 val)

**Description**: Writes a point structure.

**Parameters**

| Parameter | Type                     | Description    |
|-----------|--------------------------|----------------|
| val       | Corresponding point type | Point to write |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Write(RvRect / RvRectF32 / RvRectF64)

csharp

public bool Write(RvRect rect)

public bool Write(RvRectF32 rect)

public bool Write(RvRectF64 rect)

**Description**: Writes a rectangle structure (field-by-field, four boundary values).

**Parameters**

| Parameter | Type                         | Description        |
|-----------|------------------------------|--------------------|
| rect      | Corresponding rectangle type | Rectangle to write |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Write(int left, int top, int right, int bottom)

csharp

public bool Write(int left, int top, int right, int bottom)

**Description**: Directly writes four integer boundary values.

**Parameters**

| Parameter                | Type | Description          |
|--------------------------|------|----------------------|
| left, top, right, bottom | int  | Four boundary values |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Write(IntPtr pData, int size)

csharp

public bool Write(IntPtr pData, int size)

**Description**: Writes size bytes from an unmanaged buffer to the file.

**Parameters**

| Parameter | Type   | Description         |
|-----------|--------|---------------------|
| pData     | IntPtr | Pointer to the data |
| size      | int    | Number of bytes     |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Write(object obj)

csharp

public bool Write(object obj)

**Description**: Writes a boxed value-type struct field by field in public instance field order.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| obj       | object | Struct object to write |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

**Supported Field Types**

Enum, bool, char, int, uint, short, ulong, float, double, string, RvPoint, RvPointF32, RvPointF64, RvRect, RvRectF32, RvRectF64.

**Limitations**

- Only supports value types (struct); reference types return false.

- Only serializes public instance fields; properties, private fields, and static fields are not included.

- Does not support DateTime, DateTimeOffset, TimeSpan, Guid, decimal, and other special types.

- Nested structs, arrays, and collections are not automatically recursed; handle manually.

- Field order depends on reflection return order, which may differ across platforms or compiler versions; **not recommended for persisting critical data**.

## Data Reading

### Read(ref int val) / Read(ref char val) / Read(ref short val) / Read(ref ushort val) / Read(ref bool val) / Read(ref float val) / Read(ref double val) / Read(ref uint val) / Read(ref long val) / Read(ref ulong val)

csharp

public bool Read(ref int val)

public bool Read(ref char val)

public bool Read(ref short val)

public bool Read(ref ushort val)

public bool Read(ref bool val)

public bool Read(ref float val)

public bool Read(ref double val)

public bool Read(ref uint val)

public bool Read(ref long val)

public bool Read(ref ulong val)

**Description**: Reads the corresponding basic type value from file.

**Parameters**

| Parameter | Type                   | Description              |
|-----------|------------------------|--------------------------|
| val       | ref corresponding type | Output of the read value |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Read(ref string val) / ReadE1(ref string val)

csharp

public bool Read(ref string val)

public bool ReadE1(ref string val)

**Description**: Reads a string.

- Read: reads a \0-terminated string; buffer cap 2408 bytes.

- ReadE1: reads length-prefixed + string, corresponding to WriteE1, supports null.

**Parameters**

| Parameter | Type       | Description               |
|-----------|------------|---------------------------|
| val       | ref string | Output of the read string |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

**Notes**

- If the string may exceed 2400 characters, use ReadE1.

### Read(ref RvPoint val) / Read(ref RvPointF32 val) / Read(ref RvPointF64 val)

csharp

public bool Read(ref RvPoint val)

public bool Read(ref RvPointF32 val)

public bool Read(ref RvPointF64 val)

**Description**: Reads a point structure.

**Parameters**

| Parameter | Type                         | Description              |
|-----------|------------------------------|--------------------------|
| val       | ref corresponding point type | Output of the read point |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Read(ref RvRect rect) / Read(ref RvRectF32 rect) / Read(ref RvRectF64 rect)

csharp

public bool Read(ref RvRect rect)

public bool Read(ref RvRectF32 rect)

public bool Read(ref RvRectF64 rect)

**Description**: Reads a rectangle structure.

**Parameters**

| Parameter | Type                             | Description                  |
|-----------|----------------------------------|------------------------------|
| rect      | ref corresponding rectangle type | Output of the read rectangle |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Read(ref int left, ref int top, ref int right, ref int bottom)

csharp

public bool Read(ref int left, ref int top, ref int right, ref int bottom)

**Description**: Reads four integer boundary values at once.

**Parameters**

| Parameter                | Type    | Description                        |
|--------------------------|---------|------------------------------------|
| left, top, right, bottom | ref int | Output of the four boundary values |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Read(IntPtr pData, uint size) / Read(IntPtr pData, ref uint size)

csharp

public bool Read(IntPtr pData, uint size)

public bool Read(IntPtr pData, ref uint size)

**Description**: Reads raw data from file into an unmanaged buffer.

- The first version reads a fixed size bytes.

- The second version passes the buffer size via size; after reading, size returns the actual bytes read.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| pData | IntPtr | Target buffer pointer |
| size | uint / ref uint | Buffer size; the second version writes back the actual bytes read |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

### Read(ref object obj)

csharp

public bool Read(ref object obj)

**Description**: Reads fields and fills the boxed value-type struct.

**Parameters**

| Parameter | Type       | Description           |
|-----------|------------|-----------------------|
| obj       | ref object | Struct object to fill |

**Return Value**

| Type | Description        |
|------|--------------------|
| bool | Whether successful |

**Supported Field Types**: Same as Write(object).

**Usage Example**

csharp

object obj = new MyParams();

helper.Read(ref obj);

MyParams q = (MyParams)obj;

## Typical Usage

### Custom File Writing

csharp

using (DiskHelper helper = new DiskHelper())

{

if (helper.CreateFile("data.bin"))

{

helper.BeginFileOut(1, "MyData");

helper.BeginSectionOut(0x1001, 1, out uint pos);

helper.Write(640);

helper.Write(480);

helper.Write("Camera-01");

helper.EndSectionOut(0x1001, pos);

helper.BeginSectionOut(0x1002, 1, out pos);

helper.Write(new RvPoint(100, 200));

helper.EndSectionOut(0x1002, pos);

helper.EndFileOut();

}

}

### Custom File Reading

csharp

using (DiskHelper helper = new DiskHelper())

{

if (helper.OpenFile("data.bin"))

{

int version;

string tag = "";

helper.BeginFileIn(out version, ref tag);

uint t = 0; int v = 0; uint size = 0;

while (helper.PickSectionTag(ref t, ref v, ref size))

{

helper.BeginSectionIn(t, out int secVer, out uint secSize);

if (t == 0x1001)

{

int w = 0, h = 0; string name = "";

helper.Read(ref w);

helper.Read(ref h);

helper.Read(ref name);

}

else if (t == 0x1002)

{

RvPoint pt = new RvPoint();

helper.Read(ref pt);

}

helper.EndSectionIn(t);

}

helper.EndFileIn();

}

}

### Image Writing and Reading

csharp

*// Write*

using (DiskHelper helper = new DiskHelper())

{

helper.CreateFile("img.dat");

helper.BeginFileOut(1, "Image");

helper.EncodeImage(img, KImage.IMF_JPG);

helper.EndFileOut();

}

*// Read (auto-creates KImage)*

using (DiskHelper helper = new DiskHelper())

{

helper.OpenFile("img.dat");

helper.BeginFileIn();

KImage restored = helper.DecodeImage();

helper.EndFileIn();

}

### Static Image Save

csharp

DiskHelper.SaveBitmap(img.Handle, "out.bmp");
