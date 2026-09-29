# CAP Class Detailed Usage Guide

## Overview

CAP is a static class that manages global configuration and foundational capabilities for the Kingpool Suite. It centrally defines the software version numbers, constants related to lens distortion calibration, the maximum length of description text, and provides interfaces for setting and querying the global character set.

The class is declared `public static`, cannot be instantiated, and all members are static. The character set setting in CAP affects the string encoding used throughout all managed code and native library (`eeh.dll` / `eeh_d.dll`) interactions, and is a fundamental item that must be correctly configured before using native interfaces.

## Constants

| Name | Type | Value | Description |
|------|------|-------|-------------|
| `MAIN_VERSION` | int | 2 | Major version number |
| `SUB_VERSION` | int | 5 | Sub version number |
| `MINOR_VERSION` | int | 904 | Revision number |
| `MAX_DESC_TEXT_LEN` | int | 64 | Maximum description text length (63 + 1, including terminator) |
| `LR_RVEC_SIZE` | int | 3 | Lens distortion calibration rotation vector length |
| `LR_TVEC_SIZE` | int | 3 | Lens distortion calibration translation vector length |

**Notes**

- `MAIN_VERSION`, `SUB_VERSION`, and `MINOR_VERSION` together form the version number, e.g., `2.5.904`. The concatenated version description string can be obtained via `Pool.GetVersionDescription()`.
- `MAX_DESC_TEXT_LEN` is 64, meaning the description text buffer can hold up to 63 characters plus a `\0` terminator.
- `LR_RVEC_SIZE` and `LR_TVEC_SIZE` are used by the lens distortion calibration module, representing the sizes of the rotation vector and translation vector respectively.

## Character Set Property

### CharSet

```csharp
public static CharEncoding CharSet { get; set; }
```

**Description**: Gets or sets the global character set. When set, it synchronously calls the native interface `rvSetStringEncoding` to update the encoding, ensuring that the managed and native sides use consistent character encoding.

**Type**: `CharEncoding`

**Values**

| Member | Value | Description |
|--------|-------|-------------|
| `CharEncoding.Utf8` | 0 | UTF-8 encoding (default) |
| `CharEncoding.local` | 1 | Local ANSI encoding |

**get Behavior**

Returns the current global character set. The default value is `CharEncoding.Utf8`. The read operation does not trigger any native call; it simply returns the current value of the internal field `_charSet`.

**set Behavior**

Writing performs two steps:

1. Calls the native function `rvSetStringEncoding((int)value)` to notify the native library to switch string encoding.
2. Updates the internal field `_charSet` to the new value.

If the native library fails to load (e.g., `eeh.dll` is missing), the set call will throw `DllNotFoundException` or `EntryPointNotFoundException`.

**Usage Example**

```csharp
// Set to UTF-8 encoding
CAP.CharSet = CharEncoding.Utf8;

// Set to local ANSI encoding
CAP.CharSet = CharEncoding.local;

// Read current character set
CharEncoding current = CAP.CharSet;
Console.WriteLine(current); // Outputs Utf8 or local
```

**Notes**

- This property affects the encoding of all strings marshalled through `Pool.StringToCharBytes`, `NativeArg`, etc.
- Before calling any native interface that depends on string encoding, `CharSet` should be set to the correct value.
- After switching encoding, ensure both the native and managed sides use consistent encoding conventions; otherwise garbled text or parsing errors may occur.
- The default value is `CharEncoding.Utf8`, which is suitable for cross-platform and internationalization scenarios. If local ANSI encoding is required when interacting with the native library, explicitly set it to `CharEncoding.local`.

## Native Dependency

### rvSetStringEncoding

```csharp
[DllImport(LIB_NAME)]
private static extern void rvSetStringEncoding(int encoding);
```

**Description**: Notifies the native library to switch string encoding. This method is private and not directly exposed; it is called only by the `CharSet` property setter.

**Parameters**

| Parameter | Type | Description |
|-----------|------|-------------|
| `encoding` | int | Encoding identifier: 0 = UTF-8, 1 = local ANSI |

**Return Value**: None.

**Library Name Selection**

Determined by the conditional compilation constant `DEBUGGING_KINGPOOL_SUITE`:

| Condition | Library Name |
|-----------|--------------|
| `DEBUGGING_KINGPOOL_SUITE` defined | `eeh_d.dll` (debug) |
| Not defined | `eeh.dll` (release) |

**Notes**

- During deployment, ensure the corresponding DLL is accessible; otherwise an exception will be thrown when setting `CharSet`.
- This function is implemented by the native library and is responsible for updating its internal encoding context.

## Typical Usage

### Setting the Global Character Set

```csharp
// Set at application startup
CAP.CharSet = CharEncoding.Utf8;

// Or choose based on the runtime environment
if (useLocalAnsi)
{
    CAP.CharSet = CharEncoding.local;
}
else
{
    CAP.CharSet = CharEncoding.Utf8;
}
```

### Getting the Version Number

```csharp
string version = $"{CAP.MAIN_VERSION}.{CAP.SUB_VERSION}.{CAP.MINOR_VERSION}";
Console.WriteLine(version); // e.g., "2.5.904"
```

### Using the Description Text Length Constant

```csharp
byte[] descBuffer = new byte[CAP.MAX_DESC_TEXT_LEN];
// Used as a description text buffer when interacting with native interfaces
```

### Using the Lens Distortion Vector Lengths

```csharp
double[] rvec = new double[CAP.LR_RVEC_SIZE];
double[] tvec = new double[CAP.LR_TVEC_SIZE];
// Pass to the lens distortion calibration interface
```
