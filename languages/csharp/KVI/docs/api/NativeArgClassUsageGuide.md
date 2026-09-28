# NativeArg Class Detailed Usage Guide

## Overview

NativeArg is a helper class for marshalling managed data (strings, scalars, arrays, raw pointers) into unmanaged buffers. It inherits from KingsHandler (which provides handle lifecycle management). The buffer's starting address is exposed through the Ptr property for native (P/Invoke) calls.

The class is sealed and cannot be inherited. All unmanaged memory allocated by this class is automatically released on Dispose or in the finalizer. Callers manage the lifecycle via using statements or explicit Dispose. It is suitable for scenarios that need to pass managed data to C/C++ libraries, especially strings and arrays that require contiguous memory layout.

## Constructors

### NativeArg(byte value)

csharp

public NativeArg(byte value)

**Description**: Wraps a single byte.

**Parameters**

| Parameter | Type | Description               |
|-----------|------|---------------------------|
| value     | byte | The byte value to marshal |

**Usage Example**

csharp

using (var arg = new NativeArg((byte)42))

{

*// Native side reads as unsigned char\**

}

### NativeArg(IntPtr pointer)

csharp

public NativeArg(IntPtr pointer)

**Description**: Directly wraps an existing unmanaged pointer, **without copying data or taking ownership**.

**Parameters**

| Parameter | Type   | Description                   |
|-----------|--------|-------------------------------|
| pointer   | IntPtr | An existing unmanaged pointer |

**Notes**

- This constructor does not allocate new memory, so OwnsBuffer is false and Dispose will not release the memory pointed to by this pointer.

- The caller must ensure that the pointer remains valid throughout the NativeArg's lifetime.

- If IntPtr.Zero is passed, Ptr returns UIntPtr.Zero.

**Usage Example**

csharp

IntPtr external = GetBufferFromSomewhere();

using (var arg = new NativeArg(external))

{

*// arg.OwnsBuffer is false; Dispose won't release external*

}

### NativeArg(string value)

csharp

public NativeArg(string value)

**Description**: Wraps a string as a \0-terminated byte sequence; encoding is automatically selected based on CAP.CharSet.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| value | string | The string to marshal. If null, treated as an empty string |

**Notes**

- When CAP.CharSet == CharEncoding.Utf8, uses Encoding.UTF8; otherwise uses local ANSI encoding.

- The returned buffer is always \0-terminated, conforming to C-style string conventions.

**Usage Example**

csharp

using (var arg = new NativeArg("Kingpool Vision"))

{

*// Pass arg.Ptr to the native interface*

}

### NativeArg(float value) / NativeArg(double value)

csharp

public NativeArg(float value)

public NativeArg(double value)

**Description**: Wraps a single- or double-precision floating-point value.

**Parameters**

| Parameter | Type           | Description                         |
|-----------|----------------|-------------------------------------|
| value     | float / double | The floating-point value to marshal |

**Usage Example**

csharp

using (var arg = new NativeArg(3.14))

{

*// Native side reads as double\**

}

### NativeArg(int value) / NativeArg(long value) / NativeArg(short value)

csharp

public NativeArg(int value)

public NativeArg(long value)

public NativeArg(short value)

**Description**: Wraps an integer, long integer, or short integer.

**Parameters**

| Parameter | Type               | Description                  |
|-----------|--------------------|------------------------------|
| value     | int / long / short | The integer value to marshal |

**Usage Example**

csharp

using (var arg = new NativeArg(1024))

{

*// Native side reads as int\**

}

### NativeArg(bool value)

csharp

public NativeArg(bool value)

**Description**: Wraps a boolean value, serialized as 1 (true) or 0 (false).

**Parameters**

| Parameter | Type | Description                  |
|-----------|------|------------------------------|
| value     | bool | The boolean value to marshal |

**Usage Example**

csharp

using (var arg = new NativeArg(true))

{

*// Native side reads as int\*, value = 1*

}

### NativeArg(byte\[\] data)

csharp

public NativeArg(byte\[\] data)

**Description**: Wraps a byte array, copied as-is to the unmanaged buffer.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| data | byte\[\] | The byte array to marshal. If null or length 0, the buffer is empty and Ptr returns UIntPtr.Zero |

**Usage Example**

csharp

byte\[\] buffer = { 0x01, 0x02, 0x03 };

using (var arg = new NativeArg(buffer))

{

*// Native side reads as unsigned char\**

}

### NativeArg(float\[\] data) / NativeArg(double\[\] data) / NativeArg(int\[\] data) / NativeArg(short\[\] data)

csharp

public NativeArg(float\[\] data)

public NativeArg(double\[\] data)

public NativeArg(int\[\] data)

public NativeArg(short\[\] data)

**Description**: Wraps a corresponding-type array, arranged contiguously by element type.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| data | Corresponding type array | The array to marshal. If null or length 0, the buffer is empty |

**Notes**

- Arrays are stored contiguously in unmanaged memory by element type, suitable for interfaces that receive a pointer plus a length.

- Each element is laid out according to Marshal.SizeOf for that type.

**Usage Example**

csharp

float\[\] points = { 1.0f, 2.0f, 3.0f, 4.0f };

using (var arg = new NativeArg(points))

{

*// Native side reads as float\*, length = 4*

}

### NativeArg(string value, Encoding encoding)

csharp

public NativeArg(string value, Encoding encoding)

**Description**: Wraps a string as a \0-terminated byte sequence using an explicitly specified encoding.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| value | string | The string to marshal. If null, treated as an empty string |
| encoding | Encoding | Specified encoding. If null, defaults to Encoding.UTF8 |

**Usage Example**

csharp

using (var arg = new NativeArg("中文测试", Encoding.GetEncoding("GB2312")))

{

*// Marshalled using GB2312 encoding*

}

### NativeArg(string value, bool utf8)

csharp

public NativeArg(string value, bool utf8)

**Description**: Forces UTF-8 or local ANSI encoding for the string, ignoring the global CAP.CharSet setting.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| value | string | The string to marshal. If null, treated as an empty string |
| utf8 | bool | true forces UTF-8; false forces local ANSI encoding |

**Usage Example**

csharp

*// Force UTF-8, ignoring the global CharSet*

using (var arg = new NativeArg("Kingpool", true)) { }

*// Force local ANSI*

using (var arg = new NativeArg("Kingpool", false)) { }

## Properties

### Ptr

csharp

public UIntPtr Ptr { get; }

**Description**: Gets the starting address of the wrapped data for native interface calls.

**Type**: UIntPtr

**Return Value**

- If the buffer is not empty, returns the starting address.

- If the buffer is empty (e.g., null or an empty array was passed to the constructor), returns UIntPtr.Zero.

**Usage Example**

csharp

using (var arg = new NativeArg("Hello"))

{

IntPtr ptr = (IntPtr)arg.Ptr;

*// Pass ptr to the native interface*

}

**Notes**

- The return value is UIntPtr, which has the same semantics as IntPtr but is unsigned. If the interface requires IntPtr, cast via (IntPtr)arg.Ptr.

- The pointer must be used while the NativeArg is valid; after Dispose, the pointer becomes invalid.

### OwnsBuffer

csharp

public bool OwnsBuffer { get; }

**Description**: Indicates whether the current instance owns the underlying buffer.

**Type**: bool

**Return Value**

| Value | Meaning |
|----|----|
| true | The current instance owns the buffer; Dispose will release it via Marshal.FreeHGlobal |
| false | The buffer is borrowed (e.g., created via the IntPtr constructor), or the buffer is empty; Dispose will not release it |

**Usage Example**

csharp

using (var arg = new NativeArg("Hello"))

{

Console.WriteLine(arg.OwnsBuffer); *// True*

}

IntPtr external = GetBufferFromSomewhere();

using (var arg = new NativeArg(external))

{

Console.WriteLine(arg.OwnsBuffer); *// False*

}

## Static Methods

### StringToAnsiBytes

csharp

public static byte\[\] StringToAnsiBytes(string value)

**Description**: Converts a managed string to a local ANSI byte array.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| value | string | The string to convert. If null, treated as an empty string |

**Return Value**

| Type     | Description                                     |
|----------|-------------------------------------------------|
| byte\[\] | ANSI-encoded byte array, **without terminator** |

**Platform Behavior**

- **.NET Framework**: Uses Encoding.Default, i.e., the current system ANSI code page.

- **.NET Core / .NET 5+**:

  - On Windows, implemented via WideCharToMultiByte(CP_ACP).

  - On non-Windows platforms (Linux, macOS), falls back to UTF-8 encoding.

  - On the first call, probes whether WideCharToMultiByte is available and caches the result.

**Usage Example**

csharp

byte\[\] ansi = NativeArg.StringToAnsiBytes("中文");

*// ansi is a byte array encoded in local ANSI*

### StringToNativeBytes

csharp

public static byte\[\] StringToNativeBytes(string value)

**Description**: Converts a string to a \0-terminated byte array according to the current CAP.CharSet setting.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| value | string | The string to convert. If null, treated as an empty string |

**Return Value**

| Type | Description |
|----|----|
| byte\[\] | \0-terminated byte array. UTF-8 mode uses Encoding.UTF8; local mode uses ANSI encoding |

**Usage Example**

csharp

*// Assume CAP.CharSet == CharEncoding.Utf8*

byte\[\] bytes = NativeArg.StringToNativeBytes("Hello");

*// bytes is UTF-8 encoded, with a trailing \0*

**Notes**

- Unlike StringToAnsiBytes, this method always returns a byte array **with a** \0 **terminator**, conforming to C-style string conventions.

- The encoding depends on the global CAP.CharSet and is not directly controlled by the caller.

## Protected Method

### ReleaseHandle

csharp

protected sealed override void ReleaseHandle()

**Description**: Releases the unmanaged buffer allocated by this instance.

**Behavior**

- Executes Marshal.FreeHGlobal only when OwnsBuffer is true and \_handle != IntPtr.Zero.

- If OwnsBuffer is false (borrowed pointer or empty buffer), no release is performed.

- This method is **sealed**; subclasses cannot override it. However, NativeArg itself is sealed, so there are no subclasses.

- Called by the KingsHandler base class on Dispose or in the finalizer; typically not called directly by users.

## Typical Usage

### String Parameter Marshalling

csharp

using (var arg = new NativeArg("Kingpool Vision"))

{

*// Pass arg.Ptr to the native interface*

NativeCall(arg.Ptr);

}

### Array Parameter Marshalling

csharp

float\[\] points = { 1.0f, 2.0f, 3.0f, 4.0f };

using (var arg = new NativeArg(points))

{

*// Native side reads as float\*, length = 4*

NativeCallWithArray(arg.Ptr, points.Length);

}

### Borrowing an External Pointer

csharp

IntPtr external = GetBufferFromSomewhere();

using (var arg = new NativeArg(external))

{

*// arg.OwnsBuffer is false; Dispose won't release external*

NativeCall(arg.Ptr);

}

*// The caller must release external*

### Image Handle Marshalling

csharp

using (KImage img = new KImage("..\\samples\\waterdrop.png"))

{

using (var arg = new NativeArg(img.Handle))

{

Render.Modify(m_hContext, m_hCurStroke, ModifyType.Image, arg.Ptr);

}

}

### String Encoding Conversion Helpers

csharp

*// Need an ANSI byte array without terminator*

byte\[\] ansi = NativeArg.StringToAnsiBytes("Hello");

*// Need a terminator-appended byte array matching the current global encoding*

byte\[\] native = NativeArg.StringToNativeBytes("Hello");
