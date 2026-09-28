# FileOption Class Detailed Usage Guide

## Overview

FileOption is a utility class for reading and writing INI-format configuration files. It wraps the Windows-platform INI file APIs (such as GetPrivateProfileString, WritePrivateProfileString, etc.) and provides read/write capabilities for configuration items organized by "Section" and "Field", supporting four data types: integer, float, boolean, and string.

This class is suitable for persisting small configuration data in Windows applications, such as window positions, user preferences, device parameters, etc. All read/write operations target the same configuration file path, and the target file can be dynamically switched via the Redir method.

**Note**: FileOption depends on Windows INI file APIs at the underlying level, so it **can only run on Windows platforms**. Using it on Linux, macOS, or non-Windows .NET Core environments will throw DllNotFoundException. Cross-platform projects should use JSON, XML, or other configuration schemes instead.

## Constructor

csharp

public FileOption(string strFilePath)

**Description**: Creates a FileOption instance and specifies the INI file path to operate on.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| strFilePath | string | Full or relative path to the INI file. If the file does not exist, subsequent write operations will create it automatically; if the path is invalid, reads and writes will fail |

**Return Value**: None (constructor).

**Usage Example**

csharp

FileOption opt = new FileOption("config.ini");

**Notes**

- The constructor only saves the path; it does not immediately read or write the file.

- If null or an empty string is passed, subsequent operations may fail or throw exceptions; it is recommended to pass a valid path.

## Methods

### GetInt

csharp

public int GetInt(string strSection, string strField, int defval = 0)

**Description**: Reads an integer value from the specified section and field in the configuration file.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| strSection | string | Section name; case-insensitive |
| strField | string | Field name; case-insensitive |
| defval | int | Default value. Returned if the field does not exist or reading fails. Default 0 |

**Return Value**

| Type | Description |
|----|----|
| int | The integer value read; if the field does not exist or cannot be parsed, returns defval |

**Usage Example**

csharp

int width = opt.GetInt("Camera", "Width", 640);

### GetFloat

csharp

public double GetFloat(string strSection, string strField, double defval = 0)

**Description**: Reads a floating-point value from the specified section and field in the configuration file.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| strSection | string | Section name |
| strField | string | Field name |
| defval | double | Default value. Returned if the field does not exist or parsing fails. Default 0 |

**Return Value**

| Type | Description |
|----|----|
| double | The floating-point value read; if the field does not exist or cannot be parsed, returns defval |

**Usage Example**

csharp

double exposure = opt.GetFloat("Camera", "Exposure", 10.0);

**Notes**

- Internally, the value is read as a string and then converted to double. If the string format is invalid (e.g., contains non-numeric characters), defval is returned.

- To distinguish between "field does not exist" and "value is 0", use GetText first to check existence.

### GetBool

csharp

public bool GetBool(string strSection, string strField, bool defval = false)

**Description**: Reads a boolean value from the specified section and field in the configuration file.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| strSection | string | Section name |
| strField | string | Field name |
| defval | bool | Default value. Returned if the field does not exist. Default false |

**Return Value**

| Type | Description                                                         |
|------|---------------------------------------------------------------------|
| bool | The boolean value read; if the field does not exist, returns defval |

**Behavior**

- Internally, the value is read as an integer: 0 means false, non-zero means true.

- When writing with SetBool, true is stored as 1 and false as 0.

**Usage Example**

csharp

bool autoFocus = opt.GetBool("Camera", "AutoFocus", true);

### GetText

csharp

public string GetText(string strSection, string strField)

**Description**: Reads a string value from the specified section and field in the configuration file.

**Parameters**

| Parameter  | Type   | Description  |
|------------|--------|--------------|
| strSection | string | Section name |
| strField   | string | Field name   |

**Return Value**

| Type | Description |
|----|----|
| string | The string read; if the field does not exist, returns an empty string "" |

**Usage Example**

csharp

string name = opt.GetText("Device", "Name");

**Notes**

- The returned string length is limited by the underlying buffer, typically within 2408 bytes. Very long strings may be truncated; in such cases, use another configuration scheme.

- To distinguish between "field does not exist" and "value is an empty string", combine with other methods (e.g., GetInt returning a default value) for indirect judgment.

### SetInt

csharp

public void SetInt(string strSection, string strField, int val)

**Description**: Writes an integer value to the specified section and field in the configuration file.

**Parameters**

| Parameter  | Type   | Description            |
|------------|--------|------------------------|
| strSection | string | Section name           |
| strField   | string | Field name             |
| val        | int    | Integer value to write |

**Return Value**: None (void).

**Usage Example**

csharp

opt.SetInt("Camera", "Width", 1280);

**Notes**

- If the specified section or field does not exist, it is created automatically.

- After writing, the underlying layer may cache the result, and actual disk persistence may be delayed. To persist immediately, call WritePrivateProfileString(null, null, null, path) to force a flush (however, FileOption does not expose this interface, so it can usually be ignored).

### SetFloat

csharp

public void SetFloat(string strSection, string strField, double val)

**Description**: Writes a floating-point value to the specified section and field in the configuration file.

**Parameters**

| Parameter  | Type   | Description                   |
|------------|--------|-------------------------------|
| strSection | string | Section name                  |
| strField   | string | Field name                    |
| val        | double | Floating-point value to write |

**Return Value**: None (void).

**Usage Example**

csharp

opt.SetFloat("Camera", "Exposure", 12.5);

**Notes**

- Internally, the floating-point value is formatted as a string before writing. It is parsed back to double via GetFloat when reading.

- Since floating-point-to-string conversion may cause precision loss, use with caution in extreme precision scenarios.

### SetBool

csharp

public void SetBool(string strSection, string strField, bool val)

**Description**: Writes a boolean value to the specified section and field in the configuration file.

**Parameters**

| Parameter  | Type   | Description            |
|------------|--------|------------------------|
| strSection | string | Section name           |
| strField   | string | Field name             |
| val        | bool   | Boolean value to write |

**Return Value**: None (void).

**Usage Example**

csharp

opt.SetBool("Camera", "AutoFocus", false);

**Notes**

- Internally, true is written as 1 and false as 0, corresponding to the reading logic of GetBool.

### SetText

csharp

public void SetText(string strSection, string strField, string strVal)

**Description**: Writes a string to the specified section and field in the configuration file.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| strSection | string | Section name |
| strField | string | Field name |
| strVal | string | String to write. If null, may throw an exception or write an empty string; it is recommended to pass a non-null string |

**Return Value**: None (void).

**Usage Example**

csharp

opt.SetText("Device", "Name", "Camera-01");

**Notes**

- The written string length should not exceed the single-line limit of an INI file (typically 32767 characters), but is actually affected by the underlying buffer.

- If the string contains line breaks, it may corrupt the INI format and should be avoided.

### Redir

csharp

public void Redir(string strFilePath)

**Description**: Redirects the current instance to a new INI file path. All subsequent read/write operations target the new path.

**Parameters**

| Parameter   | Type   | Description       |
|-------------|--------|-------------------|
| strFilePath | string | New INI file path |

**Return Value**: None (void).

**Usage Example**

csharp

opt.Redir("config_backup.ini");

**Notes**

- After calling Redir, the previously bound file path is replaced. If operations on the original file need to be preserved, create a new FileOption instance.

- If the new path does not exist, subsequent writes will create it automatically.

## Typical Usage

### Reading and Writing

csharp

FileOption opt = new FileOption("config.ini");

*// Read (with defaults)*

int width = opt.GetInt("Camera", "Width", 640);

double exposure = opt.GetFloat("Camera", "Exposure", 10.0);

bool autoFocus = opt.GetBool("Camera", "AutoFocus", true);

string name = opt.GetText("Device", "Name");

*// Write*

opt.SetInt("Camera", "Width", 1280);

opt.SetFloat("Camera", "Exposure", 12.5);

opt.SetBool("Camera", "AutoFocus", false);

opt.SetText("Device", "Name", "Camera-01");

### Switching Files

csharp

FileOption opt = new FileOption("config.ini");

opt.SetInt("App", "Version", 2);

opt.Redir("config_backup.ini");

opt.SetInt("App", "Version", 3); *// Write to the backup file*
