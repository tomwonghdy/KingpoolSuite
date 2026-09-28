---
title: "Disk File Read/Write"
description: "Overview of the DiskHelper class for file read/write and structured data persistence in KVI."
keywords: ["KVI", "DiskHelper", "file I/O", "serialization", "binary file"]
---

# Disk File Read/Write

## Overview

The `DiskHelper` class wraps the disk file read/write functions in the native library `UniSupport.dll` (debug: `UniSupport_d.dll`). It provides binary file creation, opening, and closing, as well as serialization and deserialization of structured data.

Its design goal is to support persistence of complex objects such as images, masks, and BLOBs, while also providing item-by-item read/write interfaces for basic data types and structs, suitable for implementing custom file formats.

## Key Concepts

### File Header + Sections

The file is opened with `BeginFileOut` / `BeginFileIn`, and may contain multiple named **sections** internally. Each section is opened and closed with `BeginSectionOut` / `BeginSectionIn`.

This structure facilitates saving multiple types of objects in the same file, and also facilitates locating the required content by tag when reading.

### Object Encoding

The class supports encoding and decoding of:

- **Images**: via `EncodeImage` / `DecodeImage`, using formats defined by `KImage.IMF_` constants
- **Masks**: via `EncodeMask` / `DecodeMask`
- **BLOBs**: via `EncodeBlob` / `DecodeBlob`

### Data Read/Write

- **Write**: strings, basic types (char, short, int, long, float, double), geometric structures (`RvPoint`, `RvRect`), raw data, and boxed structs
- **Read**: corresponding read methods for each write type

## Capabilities

`DiskHelper` covers:

- **Lifecycle**: construction, close file, resource release
- **File operations**: create, open, open from memory
- **Static save**: `LoadBitmap`, `SaveBitmap`, `SaveMask`, `SaveBlob`
- **File sessions**: `BeginFileOut` / `EndFileOut`, `BeginFileIn` / `EndFileIn`
- **Section operations**: `BeginSectionOut` / `EndSectionOut`, `BeginSectionIn` / `EndSectionIn`, `PickSectionTag`
- **Object encoding/decoding**: images, masks, BLOBs
- **Data writing**: strings, basic types, geometric structures, raw data, boxed structs
- **Data reading**: matching read methods for each write type

## Workflow

### Writing a File
CreateFile → BeginFileOut → write data → EndFileOut → CloseFile (or Dispose)

text

### Reading a File
OpenFile → BeginFileIn → read data → EndFileIn → CloseFile (or Dispose)

text

## Detailed Reference

For constructors, methods, parameters, return values, and examples, see:

- [DiskHelper Class Usage Guide](DiskHelperClassUsageGuide.md)

## Related Documentation

- [KImage Class Function Usage Guide](KImageClassFunctionUsageGuide.md) — image object used by image encoding
- [KMask Class Detailed Function Usage Guide](KMaskClassDetailedFunctionUsageGuide.md) — mask object used by mask encoding
- [KBlob Class Usage Guide](KBlobClassUsageGuide.md) — BLOB object used by BLOB encoding