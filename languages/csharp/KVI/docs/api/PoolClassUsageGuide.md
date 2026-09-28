# Pool Class Detailed Usage Guide

## Overview

Pool is a static utility class that aggregates commonly used constants, assertions, boolean conversions, color operations, string encoding, mathematical computations, file path handling, image loading and saving, license authorization, error handling, parsing helpers, and Windows message parameter handling for the Kingpool Suite. It is the most frequently used helper class in the entire suite.

Pool itself is declared static, cannot be instantiated, and all members are static. It does not hold any unmanaged resources and requires no manual release.

## Constants

| Name | Type | Value | Description |
|----|----|----|----|
| MAX_DESC_TEXT_LEN | int | 64 | Maximum description text length (including terminator) |
| TRUE | int | 1 | Boolean true |
| FALSE | int | 0 | Boolean false |
| INVALID_ID | uint | 0x7FFFFFFF | Invalid identifier |
| WAIT_INFINITE | int | 0x7FFFFFFF | Infinite wait |
| WAIT_NONE | int | 0 | No wait |

**Notes**

These constants are mainly used for interfacing with native code to represent boolean values and special numeric values. In pure managed code, simply use bool and int.

## Assertions and Boolean Conversion

### Assert

csharp

public static void Assert(bool b, string s = null)

**Description**: Debug assertion. Triggers Debug.Assert when the condition b is false.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| b | bool | Assertion condition |
| s | string | Optional assertion message. When null, uses a message-less assertion |

**Return Value**: None.

**Usage Example**

csharp

Pool.Assert(b != null);

Pool.Assert(size \> 0, "Size must be positive");

**Notes**

- Effective only in **Debug builds**; Debug.Assert is compiled out in Release builds.

- Mainly used for parameter validation and logic self-checks during development; should not be used as error handling for business logic.

### BOOL(bool)

csharp

public static int BOOL(bool b)

**Description**: Converts a boolean value to an integer.

**Parameters**

| Parameter | Type | Description                  |
|-----------|------|------------------------------|
| b         | bool | The boolean value to convert |

**Return Value**

| Type | Description                                    |
|------|------------------------------------------------|
| int  | true returns TRUE (1); false returns FALSE (0) |

**Usage Example**

csharp

int n = Pool.BOOL(true); *// n = 1*

### BOOL(int)

csharp

public static bool BOOL(int b)

**Description**: Converts an integer to a boolean value.

**Parameters**

| Parameter | Type | Description            |
|-----------|------|------------------------|
| b         | int  | The integer to convert |

**Return Value**

| Type | Description                                          |
|------|------------------------------------------------------|
| bool | Returns true when equal to TRUE (1); otherwise false |

**Usage Example**

csharp

bool b = Pool.BOOL(1); *// b = true*

bool c = Pool.BOOL(2); *// c = false (only 1 is treated as true)*

## Color Operations

### RGB

csharp

public static UInt32 RGB(byte red, byte green, byte blue)

**Description**: Combines RGB components into a 32-bit color value with Alpha fixed at 255.

**Parameters**

| Parameter | Type | Description           |
|-----------|------|-----------------------|
| red       | byte | Red component (0~255) |
| green     | byte | Green component       |
| blue      | byte | Blue component        |

**Return Value**

| Type   | Description                             |
|--------|-----------------------------------------|
| UInt32 | 32-bit color value in 0xFFRRGGBB format |

**Usage Example**

csharp

uint color = Pool.RGB(255, 0, 0); *// Red*

### RGBA

csharp

public static UInt32 RGBA(byte red, byte green, byte blue, byte alpha)

**Description**: Combines RGBA components into a 32-bit color value.

**Parameters**

| Parameter        | Type | Description                    |
|------------------|------|--------------------------------|
| red, green, blue | byte | RGB components                 |
| alpha            | byte | Alpha (transparency) component |

**Return Value**

| Type   | Description                             |
|--------|-----------------------------------------|
| UInt32 | 32-bit color value in 0xAARRGGBB format |

**Usage Example**

csharp

uint color = Pool.RGBA(255, 0, 0, 128); *// Semi-transparent red*

### GET_COLOR_RED / GET_COLOR_GREEN / GET_COLOR_BLUE / GET_COLOR_ALPHA

csharp

public static byte GET_COLOR_RED(UInt32 color)

public static byte GET_COLOR_GREEN(UInt32 color)

public static byte GET_COLOR_BLUE(UInt32 color)

public static byte GET_COLOR_ALPHA(UInt32 color)

**Description**: Extracts the specified component from a 32-bit color value.

**Parameters**

| Parameter | Type   | Description        |
|-----------|--------|--------------------|
| color     | UInt32 | 32-bit color value |

**Return Value**

| Type | Description                               |
|------|-------------------------------------------|
| byte | The corresponding component value (0~255) |

**Usage Example**

csharp

uint color = Pool.RGB(123, 45, 67);

byte r = Pool.GET_COLOR_RED(color); *// 123*

byte g = Pool.GET_COLOR_GREEN(color); *// 45*

byte b = Pool.GET_COLOR_BLUE(color); *// 67*

### COLOR_TO_RGB / COLOR_TO_RGBA

csharp

public static RvRgb COLOR_TO_RGB(UInt32 color)

public static RvRgba COLOR_TO_RGBA(UInt32 color)

**Description**: Converts a 32-bit color value to an RvRgb or RvRgba structure.

**Parameters**

| Parameter | Type   | Description        |
|-----------|--------|--------------------|
| color     | UInt32 | 32-bit color value |

**Return Value**

| Type           | Description                       |
|----------------|-----------------------------------|
| RvRgb / RvRgba | The corresponding color structure |

**Usage Example**

csharp

uint color = Pool.RGB(255, 128, 64);

RvRgb rgb = Pool.COLOR_TO_RGB(color);

Console.WriteLine(rgb.ToString()); *// "255,128,64"*

### RGB_TO_COLOR / RGBA_TO_COLOR

csharp

public static uint RGB_TO_COLOR(RvRgb rgb)

public static uint RGBA_TO_COLOR(RvRgba rgba)

**Description**: Converts an RvRgb or RvRgba structure to a 32-bit color value.

**Parameters**

| Parameter  | Type           | Description     |
|------------|----------------|-----------------|
| rgb / rgba | RvRgb / RvRgba | Color structure |

**Return Value**

| Type | Description        |
|------|--------------------|
| uint | 32-bit color value |

**Usage Example**

csharp

RvRgb rgb = new RvRgb(255, 128, 64);

uint color = Pool.RGB_TO_COLOR(rgb);

## String and Encoding

### SetCharEncoding

csharp

public static void SetCharEncoding(CharEncoding encoding)

**Description**: Sets the global character set. Equivalent to CAP.CharSet = encoding.

**Parameters**

| Parameter | Type         | Description                         |
|-----------|--------------|-------------------------------------|
| encoding  | CharEncoding | Target character set: Utf8 or local |

**Return Value**: None.

**Usage Example**

csharp

Pool.SetCharEncoding(CharEncoding.Utf8);

**Notes**

- This setting affects the marshalling encoding of all subsequent strings between managed and native code.

- After switching, ensure both native and managed sides use consistent encoding conventions.

### StringToCharBytes

csharp

public static byte\[\] StringToCharBytes(string s)

**Description**: Converts a string to a \0-terminated byte array; encoding is determined by CAP.CharSet.

**Parameters**

| Parameter | Type   | Description           |
|-----------|--------|-----------------------|
| s         | string | The string to convert |

**Return Value**

| Type     | Description                           |
|----------|---------------------------------------|
| byte\[\] | Encoded byte array with a trailing \0 |

**Usage Example**

csharp

byte\[\] bytes = Pool.StringToCharBytes("Hello");

**Notes**

- When CAP.CharSet == Utf8, uses Encoding.UTF8; otherwise uses Encoding.Default.

- The returned array always ends with \0, conforming to C-style string conventions.

## Math and Numeric

### Limit

csharp

public static int Limit(int lower, int upper, int pos)

**Description**: Clamps pos to the range \[lower, upper\].

**Parameters**

| Parameter | Type | Description        |
|-----------|------|--------------------|
| lower     | int  | Lower bound        |
| upper     | int  | Upper bound        |
| pos       | int  | The value to clamp |

**Return Value**

| Type | Description       |
|------|-------------------|
| int  | The clamped value |

**Usage Example**

csharp

int n = Pool.Limit(0, 100, 150); *// n = 100*

### Max / Min

csharp

public static double Max(double n1, double n2)

public static double Min(double n1, double n2)

public static float Max(float n1, float n2)

public static float Min(float n1, float n2)

public static int Max(int n1, int n2)

public static int Min(int n1, int n2)

public static short Max(short n1, short n2)

public static short Min(short n1, short n2)

**Description**: Returns the larger or smaller of two numbers. Provides double, float, int, and short overloads.

**Parameters**

| Parameter | Type                       | Description               |
|-----------|----------------------------|---------------------------|
| n1, n2    | Corresponding numeric type | The two values to compare |

**Return Value**

| Type               | Description                                   |
|--------------------|-----------------------------------------------|
| Corresponding type | The larger value (Max) or smaller value (Min) |

**Usage Example**

csharp

double a = Pool.Max(3.14, 2.71); *// a = 3.14*

int b = Pool.Min(10, 20); *// b = 10*

### Dist

csharp

public static double Dist(RvPointF64 p0, RvPointF64 p1)

public static double Dist(RvPointF32 p0, RvPointF32 p1)

public static double Dist(float x0, float y0, float x1, float y1)

**Description**: Computes the Euclidean distance between two points.

**Parameters**

| Parameter      | Type                    | Description                   |
|----------------|-------------------------|-------------------------------|
| p0, p1         | RvPointF64 / RvPointF32 | The two points                |
| x0, y0, x1, y1 | float                   | Coordinates of the two points |

**Return Value**

| Type   | Description        |
|--------|--------------------|
| double | The distance value |

**Usage Example**

csharp

RvPointF64 p0 = new RvPointF64(0, 0);

RvPointF64 p1 = new RvPointF64(3, 4);

double d = Pool.Dist(p0, p1); *// d = 5*

## File Paths

### GetFileName

csharp

public static string GetFileName(string strFilePath)

**Description**: Extracts the filename without extension from a full path.

**Parameters**

| Parameter   | Type   | Description |
|-------------|--------|-------------|
| strFilePath | string | Full path   |

**Return Value**

| Type   | Description                |
|--------|----------------------------|
| string | Filename without extension |

**Usage Example**

csharp

string name = Pool.GetFileName(@"C:\images\photo.jpg"); *// "photo"*

### GetFullFileName

csharp

public static string GetFullFileName(string strFilePath)

**Description**: Extracts the filename with extension from a full path.

**Parameters**

| Parameter   | Type   | Description |
|-------------|--------|-------------|
| strFilePath | string | Full path   |

**Return Value**

| Type   | Description             |
|--------|-------------------------|
| string | Filename with extension |

**Usage Example**

csharp

string name = Pool.GetFullFileName(@"C:\images\photo.jpg"); *// "photo.jpg"*

### GetPath

csharp

public static string GetPath(string strFilePath)

**Description**: Extracts the directory portion from a full path.

**Parameters**

| Parameter   | Type   | Description |
|-------------|--------|-------------|
| strFilePath | string | Full path   |

**Return Value**

| Type   | Description                                    |
|--------|------------------------------------------------|
| string | Directory portion (without trailing separator) |

**Usage Example**

csharp

string dir = Pool.GetPath(@"C:\images\photo.jpg"); *// "C:\images"*

## Image Helpers

### LoadImage

csharp

public static KImage LoadImage(string strFilePath)

**Description**: Loads an image from a file. Returns null if loading fails or the path is empty.

**Parameters**

| Parameter   | Type   | Description     |
|-------------|--------|-----------------|
| strFilePath | string | Image file path |

**Return Value**

| Type   | Description                                         |
|--------|-----------------------------------------------------|
| KImage | The loaded image object on success; null on failure |

**Usage Example**

csharp

KImage img = Pool.LoadImage("..\\samples\\lenna.png");

if (img != null)

{

*// Use img*

}

**Notes**

- Internally creates a KImage and calls Load; does not throw on failure but returns null.

- The returned KImage must be released by the caller.

### SaveImage

csharp

public static bool SaveImage(KImage image, string strFilePath)

**Description**: Saves an image to a file; format is determined by the file extension, default bmp.

**Parameters**

| Parameter   | Type   | Description       |
|-------------|--------|-------------------|
| image       | KImage | The image to save |
| strFilePath | string | Target file path  |

**Return Value**

| Type | Description                |
|------|----------------------------|
| bool | Whether the save succeeded |

**Usage Example**

csharp

bool ok = Pool.SaveImage(img, "output.png");

**Notes**

- Returns false when the image is null or the path is empty.

- If the path has no extension, the default format bmp is used.

## Version and License

### GetVersionDescription

csharp

public static string GetVersionDescription()

**Description**: Returns a version description string containing major, sub, and revision numbers, as well as whether it is a debug build.

**Return Value**

| Type   | Description                                                |
|--------|------------------------------------------------------------|
| string | Version description, e.g., "Version: 2.5.904, Debug: true" |

**Usage Example**

csharp

string ver = Pool.GetVersionDescription();

### InitTrial / InitSubscribe / InitLifetime

csharp

public static extern bool InitTrial(string strLicenceFile, int verifyCode)

public static extern bool InitSubscribe(string strLicenceFile, int verifyCode)

public static extern bool InitLifetime(string strFrontGateFile, int verifyCode)

**Description**: Initializes the corresponding license (trial, subscription, or lifetime).

**Parameters**

| Parameter                         | Type   | Description       |
|-----------------------------------|--------|-------------------|
| strLicenceFile / strFrontGateFile | string | License file path |
| verifyCode                        | int    | Verification code |

**Return Value**

| Type | Description                      |
|------|----------------------------------|
| bool | Whether initialization succeeded |

**Usage Example**

csharp

bool ok = Pool.InitTrial("licence.dvl", 0);

**Notes**

- All imported from UniSupport.dll (debug: UniSupport_d.dll).

- During deployment, ensure the corresponding DLL is accessible and the license file is valid.

### Uninitialize

csharp

public static extern void Uninitialize()

**Description**: Uninitializes and releases license resources.

**Return Value**: None.

**Usage Example**

csharp

Pool.Uninitialize();

### GetLicenceType / GetComputerId / GetDiskCode / GetFrontUserData / GetRemainedDays

csharp

public static extern int GetLicenceType(string strFilePath)

public static extern int GetComputerId(StringBuilder strOut, int size)

public static extern int GetDiskCode(StringBuilder strOut, int size)

public static extern bool GetFrontUserData(string strFilePath, StringBuilder strOut, int size)

public static extern int GetRemainedDays(string strFilePath)

**Description**: Queries license type, computer ID, disk code, front-gate user data, and remaining days.

**Parameters**

| Parameter   | Type          | Description       |
|-------------|---------------|-------------------|
| strFilePath | string        | License file path |
| strOut      | StringBuilder | Output buffer     |
| size        | int           | Buffer size       |

**Return Value**

| Method           | Return Value | Description                                  |
|------------------|--------------|----------------------------------------------|
| GetLicenceType   | int          | License type                                 |
| GetComputerId    | int          | Status code; result written to strOut        |
| GetDiskCode      | int          | Status code; result written to strOut        |
| GetFrontUserData | bool         | Whether successful; result written to strOut |
| GetRemainedDays  | int          | Remaining days                               |

### GetComputerId / GetDiskCode / GetFrontUserData (String Wrappers)

csharp

public static string GetComputerId()

public static string GetDiskCode()

public static string GetFrontUserData(string strFilePath)

**Description**: String wrapper versions of the above native functions, internally using a 1204-length StringBuilder.

**Parameters**

| Parameter   | Type   | Description                          |
|-------------|--------|--------------------------------------|
| strFilePath | string | License file path (GetFrontUserData) |

**Return Value**

| Type | Description |
|----|----|
| string | The corresponding query result; GetFrontUserData returns null on failure |

**Usage Example**

csharp

string computerId = Pool.GetComputerId();

string diskCode = Pool.GetDiskCode();

string userData = Pool.GetFrontUserData("front.dat");

## Error Handling

### GetErrorText

csharp

public static string GetErrorText(int errCode)

**Description**: Retrieves the error description text by error code.

**Parameters**

| Parameter | Type | Description |
|-----------|------|-------------|
| errCode   | int  | Error code  |

**Return Value**

| Type   | Description                                                   |
|--------|---------------------------------------------------------------|
| string | Error description text; empty string if no corresponding text |

**Usage Example**

csharp

string msg = Pool.GetErrorText(errCode);

Console.WriteLine(msg);

**Notes**

- Internally calls the native uniGetErrorText and converts the returned ANSI string pointer to a managed string.

- The returned string is managed by the native side; callers need not release it.

## Parsing Helpers

### ParseInt / ParseUshort / ParseBool / ParseDouble

csharp

public static int ParseInt(string str)

public static ushort ParseUshort(string str)

public static bool ParseBool(string str)

public static double ParseDouble(string str)

**Description**: Parses a string into the corresponding type. Returns 0 (or false) on parsing failure.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| str | string | The string to parse. Returns the default when null or empty |

**Return Value**

| Method | Return Value | Description |
|----|----|----|
| ParseInt | int | Parsing result; 0 on failure |
| ParseUshort | ushort | Parsing result; 0 on failure |
| ParseBool | bool | Returns true when the string is "true" (case-insensitive); otherwise false |
| ParseDouble | double | Parsing result; 0 on failure |

**Usage Example**

csharp

int n = Pool.ParseInt("123"); *// n = 123*

bool b = Pool.ParseBool("TRUE"); *// b = true*

double d = Pool.ParseDouble("3.14"); *// d = 3.14*

**Notes**

- This method does not throw exceptions; suitable for safe parsing of UI input.

- ParseBool recognizes only "true"; any other string (including "1", "yes") returns false.

## Windows Message Parameters

### HIWORD / LOWORD

csharp

public static short HIWORD(IntPtr p)

public static short LOWORD(IntPtr p)

**Description**: Gets the high 16 bits or low 16 bits of a 32-bit parameter.

**Parameters**

| Parameter | Type   | Description                               |
|-----------|--------|-------------------------------------------|
| p         | IntPtr | 32-bit parameter (such as WPARAM, LPARAM) |

**Return Value**

| Type  | Description                 |
|-------|-----------------------------|
| short | High 16 bits or low 16 bits |

**Usage Example**

csharp

IntPtr lParam = */\* mouse message parameter \*/*;

short x = Pool.LOWORD(lParam);

short y = Pool.HIWORD(lParam);

**Notes**

- On 64-bit platforms, internally converts IntPtr to a 32-bit integer before extracting the bit segment.

- Typically used to parse coordinates from mouse, keyboard, and other messages.

### MAKE_PARAM

csharp

public static IntPtr MAKE_PARAM(int low, int high)

**Description**: Combines two 16-bit values into a 32-bit parameter pointer.

**Parameters**

| Parameter | Type | Description  |
|-----------|------|--------------|
| low       | int  | Low 16 bits  |
| high      | int  | High 16 bits |

**Return Value**

| Type   | Description                   |
|--------|-------------------------------|
| IntPtr | The combined 32-bit parameter |

**Usage Example**

csharp

IntPtr lParam = Pool.MAKE_PARAM(x, y); *// Commonly used to construct mouse message parameters*

### GET_PARAM_X / GET_PARAM_Y

csharp

public static short GET_PARAM_X(IntPtr lParam)

public static short GET_PARAM_Y(IntPtr lParam)

**Description**: Extracts the X or Y coordinate from a mouse message parameter.

**Parameters**

| Parameter | Type   | Description          |
|-----------|--------|----------------------|
| lParam    | IntPtr | The message's LPARAM |

**Return Value**

| Type  | Description                                               |
|-------|-----------------------------------------------------------|
| short | X coordinate (low 16 bits) or Y coordinate (high 16 bits) |

**Usage Example**

csharp

private void OnMouseClick(IntPtr lParam)

{

short x = Pool.GET_PARAM_X(lParam);

short y = Pool.GET_PARAM_Y(lParam);

}
