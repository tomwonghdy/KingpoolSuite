# UnitakeDotNet Source Code

This directory contains the source code for the `UnitakeDotNet` managed assembly used by KingpoolSuite.

Path:

```text
KingpoolSuite/languages/csharp/src/
```

## 1. Directory Structure

```text
src/
├── unitake/                     # Source code directory
├── App.config                   # Application configuration for the project
├── README.md                    # This file
├── UnitakeDotNet.csproj         # C# project file
├── UnitakeDotNet.csproj.user    # User-specific project settings (should not be committed)
└── UnitakeDotNet.sln            # Visual Studio solution file
```

### File Description

| File / Directory | Description |
|---|---|
| `unitake/` | Contains the C# source files for the `UnitakeDotNet` assembly. |
| `App.config` | Configuration file for the project. Typically contains supported runtime settings. |
| `UnitakeDotNet.csproj` | MSBuild project file. Defines target framework, references, and build settings. |
| `UnitakeDotNet.csproj.user` | User-specific settings, such as debugging paths. Usually not committed to version control. |
| `UnitakeDotNet.sln` | Visual Studio solution file. Open this to build the project. |

## 2. Requirements

This project was created with **Visual Studio 2019**.

To build this project, you need:

- **Visual Studio 2019** (16.x recommended).
- **.NET Framework 4.6.1 Developer Pack**.
- **MSBuild 16.x**, which is included with Visual Studio 2019.
- Windows operating system.
- If the project contains C++/CLI or native components, install the **Desktop development with C++** workload and the **v142** platform toolset.

Compatibility notes:

- Visual Studio 2022 may open and build this project, but Visual Studio 2019 is the original baseline.
- If you use a newer Visual Studio version, verify the target framework and platform toolset before building.
- If the project uses C++/CLI, make sure the matching v142 toolset or a compatible retargeted toolset is installed.

## 3. Building the Project

### Using Visual Studio 2019

1. Open `UnitakeDotNet.sln` in Visual Studio 2019.
2. Select the desired configuration, such as `Debug` or `Release`.
3. Select the desired platform, such as `x86` or `x64`.
4. Build the solution: `Build` → `Build Solution`.
5. The output will be placed in:

```text
bin\<Platform>\<Configuration>\
```

Example:

```text
bin\x64\Release\
```

### Using MSBuild Command Line

Use the MSBuild that comes with Visual Studio 2019:

```cmd
msbuild UnitakeDotNet.sln /p:Configuration=Release /p:Platform=x64
```

Adjust `Configuration` and `Platform` as needed.

## 4. Build Output

After a successful build, the following files are typically generated in the output directory:

| File | Description |
|---|---|
| `UnitakeDotNet.dll` | Managed assembly. This is the main output. |
| `UnitakeDotNet.pdb` | Debug symbols. Optional for production. |
| `UnitakeDotNet.dll.config` | Copy of `App.config`. Usually not required at runtime. |

Only `UnitakeDotNet.dll` is required for runtime deployment.  
See the `runtime` directory README for deployment details.

## 5. Integration with the Runtime Directory

The compiled `UnitakeDotNet.dll` is used by the C# runtime located at:

```text
KingpoolSuite/languages/csharp/runtime/
```

If you build from source, you can either:

- Copy `UnitakeDotNet.dll` to the runtime directory manually, or
- Use a **ProjectReference** in your main application so that the DLL is built and copied automatically.

When using a ProjectReference, the build system will:

1. Build `UnitakeDotNet`;
2. Generate `UnitakeDotNet.dll`;
3. Copy it to the main application's output directory.

In this case, you do not need to manually download or copy `UnitakeDotNet.dll`.

## 6. Native C++ DLL Dependency

`UnitakeDotNet.dll` may depend on native C++ DLLs.  
These native DLLs are **not** included in this source tree and are **not** copied automatically by project references.

You must obtain and deploy the native C++ DLLs separately:

- Install them via an installer or script;
- Copy them to the application output directory;
- Add their directory to the `PATH` environment variable;
- Use `SetDllDirectory` or a similar mechanism at runtime.

Make sure the platform, such as x86 or x64, of the native DLLs matches the platform of `UnitakeDotNet.dll` and your main application.

## 7. Building Other Versions

The source code in this directory can be used to build other versions of `UnitakeDotNet.dll`.

If you need a different version, platform, or configuration:

1. Open `UnitakeDotNet.sln` in Visual Studio 2019 or modify `UnitakeDotNet.csproj` as needed.
2. Change the target framework or platform if required.
3. Build the project.
4. Replace the `UnitakeDotNet.dll` in the runtime directory with your newly compiled DLL.
5. Verify that the native C++ DLLs are also compatible.

## 8. Notes

- This project was created with Visual Studio 2019.
- Do not commit `UnitakeDotNet.csproj.user` to version control. It contains user-specific settings.
- `App.config` is used at build time to generate `UnitakeDotNet.dll.config`. Runtime configuration for a .NET Framework application should normally be placed in the main application's `.exe.config`, not in `UnitakeDotNet.dll.config`.
- Always keep the source code and the runtime DLL in sync when making changes.
- If you upgrade the project to a newer Visual Studio version, document the new baseline and verify all dependencies.

## 9. Version History

| Date | Version / Commit | Updated By | Notes |
|---|---|---|---|
|  |  |  |  |
|  |  |  |  |

## 10. Conclusion

- This directory contains the source code for `UnitakeDotNet`.
- The project was created with Visual Studio 2019.
- Build with Visual Studio 2019 or MSBuild 16.x.
- The main output is `UnitakeDotNet.dll`.
- Native C++ DLLs must be deployed separately.
- If you need another version, download the source from this `src` folder and compile it yourself.
