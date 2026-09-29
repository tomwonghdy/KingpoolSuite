# KVI C# Tutorials

This directory contains C# sample projects for KVI.  
These projects correspond to the help documents under `KVI/docs/examples`.

## 1. Directory Structure

```text
tutorials/
├── Analyze/                 # Sample project: Analyze
├── BlobBasics/              # Sample project: Blob basics
├── Graphics/                # Sample project: Graphics
├── ImageBasics/             # Sample project: Image basics
├── MaskBasics/              # Sample project: Mask basics
├── Process/                 # Sample project: Process
├── SmartMath/               # Sample project: SmartMath
├── samples/                 # Non-project folder: sample image files
├── unitakeRuntime/          # Non-project folder: UnitakeDotNet.dll
└── KVIExamples.sln          # Visual Studio solution file
```

### Folder and File Description

| Item | Type | Description |
|---|---|---|
| `Analyze/` | Project | C# sample project for analysis examples. |
| `BlobBasics/` | Project | C# sample project for blob basics. |
| `Graphics/` | Project | C# sample project for graphics. |
| `ImageBasics/` | Project | C# sample project for image basics. |
| `MaskBasics/` | Project | C# sample project for mask basics. |
| `Process/` | Project | C# sample project for processing. |
| `SmartMath/` | Project | C# sample project for SmartMath. |
| `samples/` | Non-project folder | Stores image files used by the examples. |
| `unitakeRuntime/` | Non-project folder | Stores `UnitakeDotNet.dll` used by the sample projects. |
| `KVIExamples.sln` | Solution | Visual Studio 2019 solution file. Open this to build and run the samples. |

## 2. Purpose

These sample projects demonstrate how to use KVI from C#.  
Each project corresponds to a topic in the KVI help documentation:

```text
KVI/docs/examples/
```

Use the documentation together with these samples to understand the API usage, expected behavior, and typical workflows.

## 3. Requirements

To build and run these samples, you need:

- **Visual Studio 2019** (16.x recommended).
- **.NET Framework 4.6.1 Developer Pack**.
- **UnitakeDotNet.dll** in `unitakeRuntime/`.
- Native C++ DLLs required by `UnitakeDotNet.dll` (must be installed separately).
- Windows operating system.

Platform requirements:

- The sample projects and `UnitakeDotNet.dll` must use the same platform, such as `x86` or `x64`.
- Native C++ DLLs must also match the same platform.

## 4. Building and Running

### Using Visual Studio 2019

1. Open `KVIExamples.sln` in Visual Studio 2019.
2. Select the desired configuration, such as `Debug` or `Release`.
3. Select the desired platform, such as `x86` or `x64`.
4. Build the solution: `Build` → `Build Solution`.
5. Set one of the sample projects as the startup project.
6. Run it with `Debug` → `Start Without Debugging` or `Start Debugging`.
7. Refer to `KVI/docs/examples` for detailed explanations of each example.

### Using MSBuild Command Line

```cmd
msbuild KVIExamples.sln /p:Configuration=Release /p:Platform=x64
```

Adjust `Configuration` and `Platform` as needed.

## 5. Important Notes on `unitakeRuntime` and `samples`

### `unitakeRuntime`

- This folder is **not** a Visual Studio project.
- It stores `UnitakeDotNet.dll`, which is required by the sample projects.
- The sample projects typically reference this DLL through a relative path.
- **Do not delete or move this folder** unless you also update the references in the projects.
- If you need another version of `UnitakeDotNet.dll`, build it from the source under:
  ```text
  KingpoolSuite/languages/csharp/src/
  ```
  and replace the DLL in `unitakeRuntime/`.

### `samples`

- This folder is **not** a Visual Studio project.
- It stores the image files used by the examples.
- The sample projects expect these files to be available at runtime.
- **Do not delete or move this folder** unless you also update the file paths in the sample code.

## 6. Native C++ DLL Dependency

`UnitakeDotNet.dll` may depend on native C++ DLLs.  
These native DLLs are **not** included in this directory and are **not** copied automatically by project references.

You must obtain and deploy the native C++ DLLs separately:

- Install them via an installer or script;
- Copy them to the application output directory;
- Add their directory to the `PATH` environment variable;
- Use `SetDllDirectory` or a similar mechanism at runtime.

Make sure the platform of the native DLLs matches the platform of `UnitakeDotNet.dll` and the sample projects.

If a native DLL is missing, the sample may throw `DllNotFoundException` or fail to start.

## 7. Troubleshooting

| Symptom | Possible Cause | Action |
|---|---|---|
| `FileNotFoundException` for `UnitakeDotNet.dll` | DLL not found in `unitakeRuntime/` or output directory | Verify `unitakeRuntime/UnitakeDotNet.dll` exists and project references are correct. |
| `BadImageFormatException` | x86/x64 platform mismatch | Align the platform of the sample project, `UnitakeDotNet.dll`, and native DLLs. |
| `DllNotFoundException` | Native C++ DLL is missing | Install the native library or add it to `PATH`. |
| Sample cannot find image files | `samples/` folder moved or missing | Restore the `samples/` folder and verify relative paths in the sample code. |
| Build errors about missing references | `unitakeRuntime` folder moved or renamed | Restore the folder or update the reference paths. |

## 8. Version History

| Date | Version / Commit | Updated By | Notes |
|---|---|---|---|
|  |  |  |  |
|  |  |  |  |

## 9. Conclusion

- This directory contains C# sample projects for KVI.
- The samples correspond to the help documents under `KVI/docs/examples`.
- `unitakeRuntime/` stores `UnitakeDotNet.dll` and must not be moved or deleted.
- `samples/` stores image files used by the examples and must not be moved or deleted.
- Open `KVIExamples.sln` in Visual Studio 2019 to build and run the samples.
- Native C++ DLLs must be installed or deployed separately.
- If you need another version of `UnitakeDotNet.dll`, build it from the source under `KingpoolSuite/languages/csharp/src/`.
