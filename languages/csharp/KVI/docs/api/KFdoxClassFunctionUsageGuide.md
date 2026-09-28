# KFdox Class Detailed Function Usage Guide

## Overview

KFdox is a tree-structured data container used to organize multi-dimensional, multi-level data results. It can hold nodes of various types: null, bool, 32-bit integer, 64-bit integer, double-precision floating point, string, and tuple (child-node container). Each node may carry a name, and child nodes are accessed by name or index, forming a JSON-like hierarchical structure.

KFdox is commonly used to encapsulate image analysis results: histogram data organized by channel, projection data organized by channel, grayscale statistics organized by item, etc. It supports both JSON and binary serialization formats, and can be persisted to files or transmitted over networks.

Node types are identified by the RVF_TYPE\_ series constants. The main APIs are divided into: construction, appending child nodes, query and access, type checking, setting and modification, iteration, cloning and copying, and serialization/deserialization.

## Constants

### Node Type Identifiers (RVF_TYPE Series)

Used to interpret the return value of GetFdoxType.

| Name            | Type | Value | Description                          |
|-----------------|------|-------|--------------------------------------|
| RVF_TYPE_NULL   | int  | 0     | Null node                            |
| RVF_TYPE_BOOL   | int  | 1     | Boolean node                         |
| RVF_TYPE_INT32  | int  | 2     | 32-bit integer node                  |
| RVF_TYPE_INT64  | int  | 3     | 64-bit integer node                  |
| RVF_TYPE_DOUBLE | int  | 4     | Double-precision floating-point node |
| RVF_TYPE_STRING | int  | 5     | String node                          |
| RVF_TYPE_TUPLE  | int  | 6     | Tuple node (contains child nodes)    |

## Constructors

### KFdox()

csharp

public KFdox()

**Description**: Creates an empty KFdox object (a tuple node).

**Parameters**: None.

**Return Value**: None (constructor).

**Usage Example**

csharp

KFdox root = new KFdox();

### KFdox(IntPtr existingHandle, bool attach)

csharp

public KFdox(IntPtr existingHandle, bool attach)

**Description**: Wraps an existing node handle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| existingHandle | IntPtr | Existing node handle |
| attach | bool | true means the handle is externally owned and this instance will not release it; false means this instance takes ownership and will release it on destruction |

**Return Value**: None (constructor).

**Usage Example**

csharp

KFdox dox = new KFdox(handle, false);

**Notes**

- Mainly used to wrap handles returned by analysis functions (such as Dia.HistogramEx).

## Appending Child Nodes

The following methods append a named child node under the current node and return the child node, facilitating chained calls.

### AppendNull

csharp

public KFdox AppendNull(string name)

**Description**: Appends a null-value child node.

**Parameters**

| Parameter | Type   | Description     |
|-----------|--------|-----------------|
| name      | string | Child node name |

**Return Value**

| Type  | Description                  |
|-------|------------------------------|
| KFdox | The newly created child node |

### AppendBool / AppendInt32 / AppendInt64 / AppendDouble / AppendString

csharp

public KFdox AppendBool(string name, bool v)

public KFdox AppendInt32(string name, int v)

public KFdox AppendInt64(string name, long v)

public KFdox AppendDouble(string name, double v)

public KFdox AppendString(string name, string v)

**Description**: Appends a child node of the corresponding type and assigns a value.

**Parameters**

| Parameter | Type               | Description     |
|-----------|--------------------|-----------------|
| name      | string             | Child node name |
| v         | Corresponding type | Initial value   |

**Return Value**

| Type  | Description                  |
|-------|------------------------------|
| KFdox | The newly created child node |

**Usage Example**

csharp

KFdox root = new KFdox();

root.AppendInt32("Width", 640)

.AppendInt32("Height", 480)

.AppendString("Name", "Camera-01")

.AppendDouble("Exposure", 12.5);

### AppendTuple

csharp

public KFdox AppendTuple(string name)

**Description**: Appends a tuple child node (used to carry the next level of child nodes).

**Parameters**

| Parameter | Type   | Description     |
|-----------|--------|-----------------|
| name      | string | Child node name |

**Return Value**

| Type  | Description                        |
|-------|------------------------------------|
| KFdox | The newly created tuple child node |

**Usage Example**

csharp

KFdox root = new KFdox();

KFdox sub = root.AppendTuple("Camera");

sub.AppendInt32("Width", 640);

sub.AppendInt32("Height", 480);

## Query and Access

### GetCount

csharp

public int GetCount()

**Description**: Gets the number of child nodes of the current node.

**Return Value**

| Type | Description           |
|------|-----------------------|
| int  | Number of child nodes |

**Usage Example**

csharp

for (int i = 0; i \< dox.GetCount(); i++)

{

KFdox child = dox.GetChild(i);

}

### GetChild

csharp

public KFdox GetChild(int index)

**Description**: Gets a child node by index.

**Parameters**

| Parameter | Type | Description      |
|-----------|------|------------------|
| index     | int  | Child node index |

**Return Value**

| Type  | Description                                         |
|-------|-----------------------------------------------------|
| KFdox | Child node; may return null if the index is invalid |

### GetName

csharp

public string GetName(int index)

**Description**: Gets the name of the specified child node.

**Parameters**

| Parameter | Type | Description      |
|-----------|------|------------------|
| index     | int  | Child node index |

**Return Value**

| Type   | Description     |
|--------|-----------------|
| string | Child node name |

### FindChild / FindIndex

csharp

public KFdox FindChild(string name)

public int FindIndex(string name)

**Description**: Finds a child node or index by name.

**Parameters**

| Parameter | Type   | Description     |
|-----------|--------|-----------------|
| name      | string | Child node name |

**Return Value**

| Method | Return Type | Description |
|----|----|----|
| FindChild | KFdox | Matching child node; returns null if not found |
| FindIndex | int | Matching index; returns a negative number if not found |

**Usage Example**

csharp

KFdox widthNode = root.FindChild("Width");

int idx = root.FindIndex("Height");

### GetIndex

csharp

public int GetIndex(KFdox child)

**Description**: Gets the index of the specified child node object in the current node.

**Parameters**

| Parameter | Type  | Description       |
|-----------|-------|-------------------|
| child     | KFdox | Target child node |

**Return Value**

| Type | Description                                   |
|------|-----------------------------------------------|
| int  | Index; returns a negative number if not found |

### Query

csharp

public KFdox Query(string path)

**Description**: Queries a nested child node by path, with path segments separated by a delimiter.

**Parameters**

| Parameter | Type   | Description                      |
|-----------|--------|----------------------------------|
| path      | string | Query path, e.g., "Camera/Width" |

**Return Value**

| Type  | Description                              |
|-------|------------------------------------------|
| KFdox | Matching node; returns null if not found |

**Usage Example**

csharp

KFdox widthNode = root.Query("Camera/Width");

## Value Access

The following methods read values from the current node. The return value bool indicates whether reading succeeded; on failure, the output parameter is set to its default value.

### GetBool / GetInt32 / GetInt64 / GetDouble

csharp

public bool GetBool(out bool v)

public bool GetInt32(out int v)

public bool GetInt64(out long v)

public bool GetDouble(out double v)

**Description**: Reads the corresponding type value of the current node.

**Parameters**

| Parameter | Type                   | Description              |
|-----------|------------------------|--------------------------|
| v         | out corresponding type | Output of the read value |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether reading succeeded |

**Usage Example**

csharp

int n;

if (node.GetInt32(out n))

{

Console.WriteLine(\$"Value: {n}");

}

### GetBoolAt / GetInt32At / GetInt64At / GetDoubleAt

csharp

public bool GetBoolAt(int index, out bool v)

public bool GetInt32At(int index, out int v)

public bool GetInt64At(int index, out long v)

public bool GetDoubleAt(int index, out double v)

**Description**: Reads the corresponding type value of the child node at the specified index.

**Parameters**

| Parameter | Type                   | Description              |
|-----------|------------------------|--------------------------|
| index     | int                    | Child node index         |
| v         | out corresponding type | Output of the read value |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether reading succeeded |

**Usage Example**

csharp

double d;

dox.GetDoubleAt(0, out d); *// Read the floating-point value of the 0th child node*

### GetString / GetStringAt

csharp

public string GetString()

public string GetStringAt(int index)

**Description**: Reads the string value of the current node or the child node at the specified index.

**Parameters**

| Parameter | Type | Description                    |
|-----------|------|--------------------------------|
| index     | int  | Child node index (GetStringAt) |

**Return Value**

| Type   | Description                                                 |
|--------|-------------------------------------------------------------|
| string | String value; may return null or an empty string on failure |

## Type Checking

The following methods determine the data type of the current node, returning bool.

| Method     | Description                                              |
|------------|----------------------------------------------------------|
| IsNull()   | Whether it is a null node                                |
| IsBool()   | Whether it is a boolean node                             |
| IsInt32()  | Whether it is a 32-bit integer node                      |
| IsInt64()  | Whether it is a 64-bit integer node                      |
| IsDouble() | Whether it is a double-precision floating-point node     |
| IsString() | Whether it is a string node                              |
| IsTuple()  | Whether it is a tuple node (contains child nodes)        |
| IsNumber() | Whether it is a numeric node (integer or floating point) |

**Usage Example**

csharp

if (node.IsTuple())

{

for (int i = 0; i \< node.GetCount(); i++) { */\* ... \*/* }

}

else if (node.IsNumber())

{

double v;

node.GetDouble(out v);

}

### GetFdoxType

csharp

public int GetFdoxType()

**Description**: Gets the type identifier of the current node.

**Return Value**

| Type | Description                                                     |
|------|-----------------------------------------------------------------|
| int  | Type identifier, represented by the RVF_TYPE\_ series constants |

## Setting and Modification

The following methods modify the value of the current node or the child node at the specified index.

### SetNull / SetBool / SetInt32 / SetInt64 / SetDouble / SetString

csharp

public bool SetNull()

public bool SetBool(bool v)

public bool SetInt32(int v)

public bool SetInt64(long v)

public bool SetDouble(double v)

public bool SetString(string v)

**Description**: Sets the value and type of the current node.

**Parameters**

| Parameter | Type               | Description  |
|-----------|--------------------|--------------|
| v         | Corresponding type | Value to set |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether setting succeeded |

### SetNullAt / SetBoolAt / SetInt32At / SetInt64At / SetDoubleAt / SetStringAt

csharp

public bool SetNullAt(int index)

public bool SetBoolAt(int index, bool v)

public bool SetInt32At(int index, int v)

public bool SetInt64At(int index, long v)

public bool SetDoubleAt(int index, double v)

public bool SetStringAt(int index, string v)

**Description**: Sets the value of the child node at the specified index.

**Parameters**

| Parameter | Type               | Description      |
|-----------|--------------------|------------------|
| index     | int                | Child node index |
| v         | Corresponding type | Value to set     |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether setting succeeded |

### SetName

csharp

public bool SetName(int index, string name)

**Description**: Modifies the name of the child node at the specified index.

**Parameters**

| Parameter | Type   | Description      |
|-----------|--------|------------------|
| index     | int    | Child node index |
| name      | string | New name         |

**Return Value**

| Type | Description               |
|------|---------------------------|
| bool | Whether setting succeeded |

## Iteration

### GetChildren

csharp

public IEnumerable\<KFdox\> GetChildren()

**Description**: Enumerates all child nodes.

**Parameters**: None.

**Return Value**

| Type                 | Description                          |
|----------------------|--------------------------------------|
| IEnumerable\<KFdox\> | Enumerable collection of child nodes |

**Usage Example**

csharp

foreach (KFdox child in dox.GetChildren())

{

Console.WriteLine(child.GetName(0));

}

### IterFirst / IterLast / IterNext / IterPrev

csharp

public int IterFirst()

public int IterLast()

public int IterNext(int currentIndex)

public int IterPrev(int currentIndex)

**Description**: Iterates child nodes by index, suitable for scenarios requiring manual control of the iteration flow.

**Parameters**

| Parameter    | Type | Description                        |
|--------------|------|------------------------------------|
| currentIndex | int  | Current index (IterNext, IterPrev) |

**Return Value**

| Method | Return Type | Description |
|----|----|----|
| IterFirst | int | Index of the first child node |
| IterLast | int | Index of the last child node |
| IterNext | int | Index of the next child node; returns a negative number at the end |
| IterPrev | int | Index of the previous child node; returns a negative number at the beginning |

**Usage Example**

csharp

int idx = dox.IterFirst();

while (idx \>= 0)

{

KFdox child = dox.GetChild(idx);

*// Process child*

idx = dox.IterNext(idx);

}

## Cloning and Copying

### Clone

csharp

public KFdox Clone()

**Description**: Deep copy of the current node and all its child nodes, generating a fully independent copy.

**Parameters**: None.

**Return Value**

| Type  | Description         |
|-------|---------------------|
| KFdox | The new copy object |

**Usage Example**

csharp

KFdox copy = dox.Clone();

**Notes**

- The copy is fully independent of the original node; modifying either does not affect the other.

### CopyTo

csharp

public void CopyTo(KFdox other)

**Description**: Copies the content of the current node to the specified target node. The target node's existing content is overwritten.

**Parameters**

| Parameter | Type  | Description |
|-----------|-------|-------------|
| other     | KFdox | Target node |

**Return Value**: None (void).

**Usage Example**

csharp

KFdox target = new KFdox();

dox.CopyTo(target);

*// Now target has the same content as dox*

**Notes**

- Unlike Clone, CopyTo does not create a new object; it writes content into an existing target node.

- Suitable for scenarios that need to reuse a target instance or avoid frequent object allocation.

## Serialization and Deserialization

### ToJson / FromJson

csharp

public string ToJson(bool pretty = false)

public bool FromJson(string json)

**Description**: Serializes to a JSON string, or deserializes from a JSON string.

**Parameters**

| Parameter | Type   | Description                                               |
|-----------|--------|-----------------------------------------------------------|
| pretty    | bool   | When true, outputs indented formatted JSON; default false |
| json      | string | JSON string to parse (FromJson)                           |

**Return Value**

| Method   | Return Type | Description               |
|----------|-------------|---------------------------|
| ToJson   | string      | JSON string               |
| FromJson | bool        | Whether parsing succeeded |

**Usage Example**

csharp

string json = dox.ToJson(true);

Console.WriteLine(json);

KFdox parsed = new KFdox();

if (parsed.FromJson(json))

{

*// Use parsed*

}

### ToBinary / FromBinary

csharp

public byte\[\] ToBinary()

public bool FromBinary(byte\[\] data)

**Description**: Serializes to a binary byte array, or deserializes from a binary byte array. The binary format is more compact than JSON, suitable for network transmission and persistence.

**Parameters**

| Parameter | Type     | Description                       |
|-----------|----------|-----------------------------------|
| data      | byte\[\] | Binary data to parse (FromBinary) |

**Return Value**

| Method     | Return Type | Description               |
|------------|-------------|---------------------------|
| ToBinary   | byte\[\]    | Binary data               |
| FromBinary | bool        | Whether parsing succeeded |

**Usage Example**

csharp

byte\[\] data = dox.ToBinary();

KFdox restored = new KFdox();

if (restored.FromBinary(data))

{

*// Use restored*

}

## Other

### Clear

csharp

public void Clear()

**Description**: Clears all child nodes of the current node.

**Parameters**: None.

**Return Value**: None.

## Typical Usage

### Building and Traversing

csharp

KFdox root = new KFdox();

root.AppendInt32("Width", 640);

root.AppendInt32("Height", 480);

KFdox cam = root.AppendTuple("Camera");

cam.AppendString("Name", "Cam-01");

cam.AppendDouble("Exposure", 12.5);

for (int i = 0; i \< root.GetCount(); i++)

{

Console.WriteLine(\$"{root.GetName(i)}: {root.GetChild(i).GetFdoxType()}");

}

### Querying by Path

csharp

KFdox node = root.Query("Camera/Name");

if (node != null && node.IsString())

{

Console.WriteLine(node.GetString());

}

### Parsing Analysis Results (e.g., Histogram)

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

if (child.GetInt64(out n))

{

*// n is the pixel count at gray level i for this channel*

}

}

}

}

### Cloning and Copying

csharp

*// Deep copy: create an independent copy*

KFdox copy = root.Clone();

*// Copy to an existing node: reuse the instance*

KFdox target = new KFdox();

root.CopyTo(target);

### JSON Persistence

csharp

string json = root.ToJson(true);

File.WriteAllText("data.json", json);

KFdox restored = new KFdox();

restored.FromJson(File.ReadAllText("data.json"));
