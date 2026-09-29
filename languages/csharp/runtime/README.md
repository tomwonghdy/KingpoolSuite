# C# Runtime Directory

This directory stores the managed assemblies required by the C# runtime for KingpoolSuite.

Current path:

```text
KingpoolSuite/languages/csharp/runtime/
└── UnitakeDotNet.dll
```

## 1. File Overview

| File | Required | Description |
|---|---:|---|
| `UnitakeDotNet.dll` | Yes | Managed C# assembly. This is the core runtime file and must be kept. |
| `UnitakeDotNet.pdb` | No | Debug symbols. Usually not required in production. Keep it only if exception line numbers are needed. |
| `UnitakeDotNet.dll.config` | Usually No | .NET Framework does not automatically load a class library's `.dll.config`. Startup configuration should be placed in the main application's `.exe.config`. |
| Native C++ DLLs | Depends on features | They are not copied automatically by project references. They must be installed or deployed separately. |

## 2. Deployment Requirements

For a minimal deployment, keep at least:

```text
UnitakeDotNet.dll
```

If the main application requires .NET Framework 4.6.1 startup configuration, add the following to the main application's `.exe.config`, not to `UnitakeDotNet.dll.config`:

```xml
<configuration>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
  </startup>
</configuration>
```

## 3. Relationship with Project References

If a C# project references the `UnitakeDotNet` project through a **ProjectReference**, the build process automatically:

1. Builds `UnitakeDotNet`;
2. Generates `UnitakeDotNet.dll`;
3. Copies `UnitakeDotNet.dll` to the C# project output directory.

Therefore, when building from source, users usually do not need to download `UnitakeDotNet.dll` separately.

The `UnitakeDotNet.dll` in this directory is mainly used for:

- Running published programs directly;
- Runtime layout in release packages;
- Integration scenarios that do not compile from source.

Do not download a same-named DLL from third-party sources. Use the official build output from this repository or the official release package.

## 4. Native C++ DLL Notes

Important: `UnitakeDotNet.dll` depends on native C++ DLLs.  
Project references only solve the generation and copying of the managed `UnitakeDotNet.dll`. They do not automatically solve native C++ DLL deployment.

Native C++ DLLs must still be handled in one of the following ways:

- Installed separately by an installer or installation script;
- Copied to the main application output directory;
- Copied to the runtime directory and added to `PATH`;
- Copied automatically through a post-build event;
- Published as content files together with the release package.

Make sure the platform matches:

| Component | Requirement |
|---|---|
| Main application | x86 or x64 |
| `UnitakeDotNet.dll` | Must match the main application |
| Native C++ DLLs | Must match the main application |

Mixing x86 and x64 will cause loading failures.

## 5. Loading Path Recommendations

Choose one of the following deployment methods:

1. Place `UnitakeDotNet.dll` in the main application output directory;
2. Add this directory to the assembly probing path;
3. Load `UnitakeDotNet.dll` explicitly from this directory in code;
4. Keep the `KingpoolSuite/languages/csharp/runtime/` directory structure unchanged during release.

If the main application cannot find `UnitakeDotNet.dll`, it may throw `FileNotFoundException` or a similar assembly loading error.

## 6. Verification

After deployment, check the following:

- `UnitakeDotNet.dll` exists;
- The file size looks normal;
- The main application platform matches the DLL platform;
- Native C++ DLLs are installed or available in `PATH`;
- C# features can be called normally from the main application.

Common issues:

| Symptom | Possible Cause | Action |
|---|---|---|
| `FileNotFoundException` | `UnitakeDotNet.dll` cannot be found | Check this directory and the assembly probing path |
| `BadImageFormatException` | x86/x64 platform mismatch | Align the platform of the main application, managed DLL, and native DLLs |
| `DllNotFoundException` | Native C++ DLL is missing | Install the native library or add it to `PATH` |
| `TypeLoadException` / `MissingMethodException` | Version mismatch | Use a matching version of `UnitakeDotNet.dll` |
| No line numbers in exception stack traces | PDB is missing | Optionally place `UnitakeDotNet.pdb` in this directory |

## 7. Updating UnitakeDotNet.dll

To update `UnitakeDotNet.dll`:

1. Build the Release version from the `UnitakeDotNet` project;
2. Take the `UnitakeDotNet.dll` for the correct platform;
3. Replace the old file in this directory;
4. If debugging is needed, also place `UnitakeDotNet.pdb` here;
5. `UnitakeDotNet.dll.config` is usually not required;
6. Confirm that the native C++ DLL version and platform match;
7. Run the main application for verification;
8. Record the updated version, date, and source.

## 8. Building Other Versions

The prebuilt `UnitakeDotNet.dll` in this directory corresponds to one specific version and platform.

If you need another version, download the source code from the `src` folder and compile it yourself. Make sure the target framework and platform match your main application.

After building, replace `UnitakeDotNet.dll` in this directory with the newly compiled DLL, and verify that the native C++ DLLs are also compatible.

## 9. Version History

| Date | Version / Commit | Updated By | Notes |
|---|---|---|---|
|  |  |  |  |
|  |  |  |  |

## 10. Conclusion

- `UnitakeDotNet.dll` must be kept.
- `UnitakeDotNet.pdb` is optional and usually not needed in production.
- `UnitakeDotNet.dll.config` is usually not required. Configuration should be placed in the main application's `.exe.config`.
- Native C++ DLLs cannot be resolved automatically by project references. They must be installed or deployed separately.
- If you need another version, download the source code from the `src` folder and compile it yourself.
