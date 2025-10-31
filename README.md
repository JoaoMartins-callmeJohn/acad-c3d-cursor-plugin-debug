# AutoCAD .NET Plugin Template

A minimal .NET 8 class library template for developing AutoCAD/Civil 3D plugins with debugging support in Visual Studio Code with Cursor.

## Prerequisites

- .NET 8.0 SDK or later
- AutoCAD 2025 (or your version)
- Visual Studio Code with C# extension or Cursor IDE
- (Optional) Civil 3D 2025 for Civil 3D development

## Quick Setup

### 1. Configure AutoCAD Paths

You need to update the AutoCAD path in **two locations**:

#### In `AutoCADPlugin.csproj` (for assembly references):
```xml
<!-- Edit these paths to match your AutoCAD installation -->
<AutoCADPath>C:\Program Files\Autodesk\AutoCAD 2025</AutoCADPath>

<!-- For Civil 3D, uncomment and edit this path -->
<!-- <Civil3DPath>C:\Program Files\Autodesk\AutoCAD 2025\C3D</Civil3DPath> -->
```

Common AutoCAD installation paths:
- AutoCAD 2026: `C:\Program Files\Autodesk\AutoCAD 2026`
- AutoCAD 2025: `C:\Program Files\Autodesk\AutoCAD 2025`

### 2. Build the Project

Open a terminal in the project directory and run:

```bash
dotnet build -c Debug -p:Platform=x64
```

The DLL will be created in `bin\Debug\AutoCADPlugin.dll`

> **Note:** The build may also create a copy in `bin\x64\Debug\`, but use the `bin\Debug\` path for NETLOAD.

### 3. Debug in Cursor/VS Code

1. Set breakpoints in `Commands.cs` (e.g., in the `HelloCommand` method)
2. Start AutoCAD manually
3. In Cursor/VS Code, press `F5` to attach debugger
4. Select the `acad.exe` process when prompted
5. In AutoCAD, type `NETLOAD` and browse to `bin\Debug\AutoCADPlugin.dll`
6. Test with commands: `HELLO` or `DRAWCIRCLE`

Your breakpoints will be hit!


## Debugging Configuration

The template includes a simple "Attach to AutoCAD" configuration in `.vscode/launch.json` that works for both AutoCAD and Civil 3D.

## Adding Civil 3D Support

To enable Civil 3D development:

1. Uncomment the Civil 3D path in `AutoCADPlugin.csproj`:
   ```xml
   <Civil3DPath>C:\Program Files\Autodesk\AutoCAD 2025\C3D</Civil3DPath>
   ```

2. Uncomment the Civil 3D references:
   ```xml
   <Reference Include="AecBaseMgd">
     <SpecificVersion>False</SpecificVersion>
     <HintPath>$(AutoCADPath)\ACA\AecBaseMgd.dll</HintPath>
     <Private>False</Private>
   </Reference>
   <Reference Include="AeccDbMgd">
     <SpecificVersion>False</SpecificVersion>
     <HintPath>$(Civil3DPath)\AeccDbMgd.dll</HintPath>
     <Private>False</Private>
   </Reference>
   ```

3. Use Civil3D-Metric or Civil3D-Imperial launch profile when debugging

## Project Structure

```
AutoCADPlugin/
├── AutoCADPlugin.csproj      # Project configuration
├── Commands.cs               # Sample AutoCAD commands
├── Properties/
│   └── launchSettings.json  # Launch profiles
├── .vscode/
│   ├── launch.json          # Debug configuration
│   └── tasks.json           # Build tasks
└── README.md                # This file
```

## Customization

### Adding New Commands

Create new command methods in `Commands.cs` or new class files:

```csharp
[CommandMethod("MYCOMMAND")]
public void MyCommand()
{
    Document doc = Application.DocumentManager.MdiActiveDocument;
    Editor ed = doc.Editor;
    ed.WriteMessage("\nMy custom command executed!\n");
}
```

### Changing Target AutoCAD Version

1. Update the `<AutoCADPath>` in the project file
2. Ensure the AutoCAD .NET API version matches your target version
3. For older versions, you might need to change `<TargetFramework>` to `net48` or `net472`

### Loading Plugin Automatically

To auto-load your plugin when AutoCAD starts:

1. Create an `acad.lsp` file in a support path
2. Add: `(command "NETLOAD" "C:\\path\\to\\your\\AutoCADPlugin.dll")`

Or use the Registry (for deployment):
- Add registry key under `HKEY_CURRENT_USER\Software\Autodesk\AutoCAD\R25.0\ACAD-6101:409\Applications`

## Troubleshooting

### "Could not load file or assembly" error
- Ensure AutoCAD path is correct in the project file
- Verify you're building for x64 platform
- Check that the AutoCAD version matches the referenced DLLs

### Breakpoints not hitting
- Ensure you're running in Debug configuration
- Check that the loaded DLL matches the one being debugged
- Try rebuilding the project and reloading in AutoCAD

### Civil 3D specific issues
- Ensure Civil 3D is installed
- Verify the Civil 3D path is correct
- Use the appropriate Civil 3D launch profile

## Additional Resources

- [AutoCAD .NET Developer's Guide](https://help.autodesk.com/view/ACD/2025/ENU/?guid=GUID-C3F3C736-40CF-44A0-9210-55F6A939B6F2)
- [AutoCAD .NET API Reference](https://help.autodesk.com/view/ACD/2025/ENU/?guid=GUID-36BF58F3-537D-4B59-BEFE-2D0FEF5A4443)
- [Civil 3D .NET API Guide](https://help.autodesk.com/view/CIV3D/2025/ENU/?guid=GUID-90F8534C-2D8A-4E6A-9DCD-A603E1987B04)

## License

This template is provided as-is for educational and development purposes.