# Render Class Detailed Function Usage Guide

## Overview

Render is a graphics and image drawing class implemented on top of OpenGL hardware acceleration. It bypasses the GDI/GDI+ drawing methods provided by Windows itself and interacts directly with the graphics card, making it suitable for high-frame-rate real-time rendering of large-resolution images and complex graphics.

The class adopts a **stroke model**: each drawing call returns a stroke handle that encapsulates all information for that drawing. Handles can be saved, modified, hidden, deleted, combined, and split, enabling independent control and efficient reuse of graphical elements. All drawing operations are based on a drawing context (dc), which binds to a window handle and manages the canvas, layers, view, and drawing resources.

The methods in Render are divided by function into: constants, constructor, context management, canvas settings, view control, coordinate conversion, drawing resources, primitive drawing, stroke management, clearing and refreshing, export, and logo and appearance.

## Constants

### Drawing Style (GS Series)

Used by SetDrawStyle / GetDrawStyle.

| Name              | Type | Value | Description                               |
|-------------------|------|-------|-------------------------------------------|
| GS_ANTIALIAS      | int  | 1     | Antialiasing                              |
| GS_LINE_QUALITY   | int  | 2     | Best line quality                         |
| GS_TRANSPARENT    | int  | 4     | Transparent                               |
| GS_TEXTURE_QULITY | int  | 8     | Texture quality                           |
| GS_USE_MIP        | int  | 16    | Texture MIP                               |
| GS_INVISIBLE_CHAR | int  | 32    | Show placeholder for invisible characters |

### Text Format (DT Series)

Used by the nFormat parameter of DrawTextEx, combined bitwise.

| Name           | Type | Value  | Description                     |
|----------------|------|--------|---------------------------------|
| DT_MULTILINE   | int  | 65536  | Multi-line; breaks at line feed |
| DT_WORDBREAK   | int  | 131072 | Word wrap                       |
| DT_TRANSPARENT | int  | 524288 | Transparent background          |

### Triangle Drawing Types

Used by the type parameter of DrawTriangles.

| Name        | Type | Value | Description        |
|-------------|------|-------|--------------------|
| DT_SEPARATE | int  | 0     | Separate triangles |
| DT_FAN      | int  | 1     | Triangle fan       |
| DT_STRIP    | int  | 2     | Triangle strip     |

### Other Constants

| Name              | Type | Value | Description                          |
|-------------------|------|-------|--------------------------------------|
| MAX_FONT_NAME_LEN | int  | 63    | Maximum font name length             |
| IGNORE_COLOR      | GRgb | —     | Ignore color, used for image drawing |

## Constructor

### Render()

csharp

public Render()

**Description**: Creates a Render instance. Instantiation is usually unnecessary because all methods are static and can be called directly via Render.methodName.

**Parameters**: None.

**Return Value**: None (constructor).

## Context Management

### CreateContext

csharp

public static IntPtr CreateContext(ContextType type, IntPtr hBindWnd, int canvasWidth, int canvasHeight)

**Description**: Creates a drawing context bound to a window handle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| type | ContextType | Context type: Generic (no canvas object) or Enhanced (with canvas object) |
| hBindWnd | IntPtr | Bound window handle |
| canvasWidth | int | Canvas width; pass -1 to use window width |
| canvasHeight | int | Canvas height; pass -1 to use window height |

**Return Value**

| Type   | Description                                            |
|--------|--------------------------------------------------------|
| IntPtr | Drawing context handle; returns IntPtr.Zero on failure |

**Usage Example**

csharp

IntPtr dc = Render.CreateContext(ContextType.Generic, xguiPanel1.Handle, -1, -1);

### DestroyContext

csharp

public static void DestroyContext(IntPtr dc)

**Description**: Destroys the context, releasing resources. Should be called before window destruction.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |

**Return Value**: None.

### GetContextType

csharp

public static ContextType GetContextType(IntPtr dc)

**Description**: Gets the context type.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |

**Return Value**

| Type        | Description  |
|-------------|--------------|
| ContextType | Context type |

### IsFullScreen

csharp

public static bool IsFullScreen(IntPtr dc)

**Description**: Determines whether the current mode is full screen.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |

**Return Value**

| Type | Description         |
|------|---------------------|
| bool | Whether full screen |

### GetDriverVersion

csharp

public static int GetDriverVersion(IntPtr dc)

**Description**: Gets the OpenGL driver version, encoded as (major \<\< 16) \| minor.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |

**Return Value**

| Type | Description    |
|------|----------------|
| int  | Version number |

**Usage Example**

csharp

int v = Render.GetDriverVersion(dc);

string ver = \$"{(v \>\> 16)}.{v & 0xFFFF}";

### GetSurfaceSize / GetMaxSurfaceSize / SetSurfaceSize

csharp

public static int GetSurfaceSize(IntPtr dc)

public static int GetMaxSurfaceSize(IntPtr dc)

public static void SetSurfaceSize(IntPtr dc, int size)

**Description**: Gets or sets the rendering surface size.

**Parameters**

| Parameter | Type   | Description                   |
|-----------|--------|-------------------------------|
| dc        | IntPtr | Drawing context handle        |
| size      | int    | Surface size (SetSurfaceSize) |

**Return Value**

| Method            | Return Type | Description          |
|-------------------|-------------|----------------------|
| GetSurfaceSize    | int         | Current surface size |
| GetMaxSurfaceSize | int         | Maximum surface size |
| SetSurfaceSize    | None        | —                    |

## Canvas Settings

### SetCanvasSize / SetCanvasSizeEx

csharp

public static bool SetCanvasSize(IntPtr dc, int width, int height, bool bRefresh = false)

public static bool SetCanvasSizeEx(IntPtr dc, RvSize size, bool bRefresh = false)

**Description**: Sets the canvas size.

**Parameters**

| Parameter     | Type   | Description                                   |
|---------------|--------|-----------------------------------------------|
| dc            | IntPtr | Drawing context handle                        |
| width, height | int    | Canvas width and height                       |
| size          | RvSize | Canvas size structure (SetCanvasSizeEx)       |
| bRefresh      | bool   | Whether to refresh immediately; default false |

**Return Value**

| Type | Description                   |
|------|-------------------------------|
| bool | Whether the setting succeeded |

### SetCanvasDepth / GetCanvasDepth

csharp

public static bool SetCanvasDepth(IntPtr dc, int depth, bool bRefresh = false)

public static int GetCanvasDepth(IntPtr dc)

**Description**: Sets or gets the canvas depth. Supports 8, 24, and 32 bits.

**Parameters**

| Parameter | Type   | Description                    |
|-----------|--------|--------------------------------|
| dc        | IntPtr | Drawing context handle         |
| depth     | int    | Canvas depth                   |
| bRefresh  | bool   | Whether to refresh immediately |

**Return Value**

| Method         | Return Type | Description                   |
|----------------|-------------|-------------------------------|
| SetCanvasDepth | bool        | Whether the setting succeeded |
| GetCanvasDepth | int         | Current canvas depth          |

### GetCanvasWidth / GetCanvasHeight / GetCanvasSize

csharp

public static int GetCanvasWidth(IntPtr dc)

public static int GetCanvasHeight(IntPtr dc)

public static RvSize GetCanvasSize(IntPtr dc)

**Description**: Gets the canvas width, height, or size.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |

**Return Value**

| Method          | Return Type | Description   |
|-----------------|-------------|---------------|
| GetCanvasWidth  | int         | Canvas width  |
| GetCanvasHeight | int         | Canvas height |
| GetCanvasSize   | RvSize      | Canvas size   |

### SetBlankColor / GetBlankColor

csharp

public static void SetBlankColor(IntPtr dc, GRgb color)

public static GRgb GetBlankColor(IntPtr dc)

**Description**: Sets or gets the color of the window's blank area (visible when the canvas is smaller than the window or does not fill it after scaling).

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| color     | GRgb   | Color value            |

**Return Value** (only GetBlankColor)

| Type | Description         |
|------|---------------------|
| GRgb | Current blank color |

### SetBackgroundColor / GetBackgroundColor

csharp

public static void SetBackgroundColor(IntPtr dc, GRgb color)

public static GRgb GetBackgroundColor(IntPtr dc)

**Description**: Sets or gets the canvas background color (the color displayed after the image frame layer is cleared).

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| color     | GRgb   | Color value            |

**Return Value** (only GetBackgroundColor)

| Type | Description              |
|------|--------------------------|
| GRgb | Current background color |

### SetCanvasBackStyle / GetCanvasBackStyle

csharp

public static void SetCanvasBackStyle(IntPtr dc, int style)

public static int GetCanvasBackStyle(IntPtr dc)

**Description**: Sets or gets the canvas background style (solid or checkerboard).

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| style     | int    | Style identifier       |

**Return Value** (only GetCanvasBackStyle)

| Type | Description   |
|------|---------------|
| int  | Current style |

### SetCanvasGridSize / GetCanvasGridSize

csharp

public static void SetCanvasGridSize(IntPtr dc, int sx, int sy)

public static RvSize GetCanvasGridSize(IntPtr dc)

**Description**: Sets or gets the canvas grid size.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| sx, sy    | int    | Grid width and height  |

**Return Value** (only GetCanvasGridSize)

| Type   | Description       |
|--------|-------------------|
| RvSize | Current grid size |

### SetCanvasBackColor / GetCanvasBackColor

csharp

public static void SetCanvasBackColor(IntPtr dc, uint color)

public static uint GetCanvasBackColor(IntPtr dc)

**Description**: Sets or gets the canvas background color (32-bit integer color format).

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| color     | uint   | 32-bit color value     |

**Return Value** (only GetCanvasBackColor)

| Type | Description              |
|------|--------------------------|
| uint | Current background color |

### SetBlankImage / GetBlankImage

csharp

public static void SetBlankImage(IntPtr dc, IntPtr image)

public static IntPtr GetBlankImage(IntPtr dc)

**Description**: Sets or gets the background image of the canvas blank area. If both a blank image and a blank color are set, the blank image takes priority.

**Parameters**

| Parameter | Type   | Description             |
|-----------|--------|-------------------------|
| dc        | IntPtr | Drawing context handle  |
| image     | IntPtr | Background image handle |

**Return Value** (only GetBlankImage)

| Type   | Description                     |
|--------|---------------------------------|
| IntPtr | Current background image handle |

### ResetCanvas

csharp

public static void ResetCanvas(IntPtr dc, bool bRefresh = false)

**Description**: Resets the canvas, releasing all strokes and layers.

**Parameters**

| Parameter | Type   | Description                    |
|-----------|--------|--------------------------------|
| dc        | IntPtr | Drawing context handle         |
| bRefresh  | bool   | Whether to refresh immediately |

**Return Value**: None.

## View Control

### SetViewMode / SetViewModeEx / GetViewMode

csharp

public static void SetViewMode(IntPtr dc, ViewMode mode)

public static void SetViewModeEx(IntPtr dc, ViewMode mode, IntPtr pParam)

public static ViewMode GetViewMode(IntPtr dc)

**Description**: Sets or gets the canvas display mode.

**Parameters**

| Parameter | Type     | Description                                          |
|-----------|----------|------------------------------------------------------|
| dc        | IntPtr   | Drawing context handle                               |
| mode      | ViewMode | Display mode: Default, Cender, Stretch, Zoom, Custom |
| pParam    | IntPtr   | Additional parameter (SetViewModeEx)                 |

**Return Value** (only GetViewMode)

| Type     | Description          |
|----------|----------------------|
| ViewMode | Current display mode |

### SetViewScale / SetViewScaleEx / GetViewScale / GetViewScaleEx

csharp

public static void SetViewScale(IntPtr dc, float scale, bool bScaleY = false)

public static void SetViewScaleEx(IntPtr dc, float scaleX, float scaleY)

public static float GetViewScale(IntPtr dc, bool bScaleY = false)

public static void GetViewScaleEx(IntPtr dc, ref float pScaleX, ref float pScaleY)

**Description**: Sets or gets the canvas display scale. Effective only in Default or Center mode.

**Parameters**

| Parameter        | Type      | Description                                 |
|------------------|-----------|---------------------------------------------|
| dc               | IntPtr    | Drawing context handle                      |
| scale            | float     | Scale ratio                                 |
| bScaleY          | bool      | Whether it applies to the Y direction       |
| scaleX, scaleY   | float     | Horizontal and vertical scale ratios        |
| pScaleX, pScaleY | ref float | Output horizontal and vertical scale ratios |

**Return Value**

| Method       | Return Type | Description         |
|--------------|-------------|---------------------|
| GetViewScale | float       | Current scale ratio |
| Others       | None        | —                   |

### SetViewPos

csharp

public static void SetViewPos(IntPtr dc, int pos, bool bVertical)

public static void SetViewPos(IntPtr dc, int x, int y)

**Description**: Sets the canvas display position (offset). Suitable for panning when the canvas is larger than the window.

**Parameters**

| Parameter | Type   | Description                                    |
|-----------|--------|------------------------------------------------|
| dc        | IntPtr | Drawing context handle                         |
| pos       | int    | Offset                                         |
| bVertical | bool   | true for vertical offset; false for horizontal |
| x, y      | int    | Horizontal and vertical offsets                |

**Return Value**: None.

### GetViewPos / GetViewPosE1 / GetViewDelta / GetViewDeltaEx

csharp

public static GPoint GetViewPos(IntPtr dc)

public static int GetViewPosE1(IntPtr dc, bool bVertical)

public static void GetViewDelta(IntPtr dc, ref int dx, ref int dy)

public static int GetViewDelta(IntPtr dc, bool bVertical)

public static GSize GetViewDeltaEx(IntPtr dc)

**Description**: Gets the current display position and offset.

**Parameters**

| Parameter | Type    | Description                           |
|-----------|---------|---------------------------------------|
| dc        | IntPtr  | Drawing context handle                |
| bVertical | bool    | Whether to get the vertical direction |
| dx, dy    | ref int | Output offsets                        |

**Return Value**

| Method         | Return Type | Description                     |
|----------------|-------------|---------------------------------|
| GetViewPos     | GPoint      | Current display position        |
| GetViewPosE1   | int         | Horizontal or vertical position |
| GetViewDeltaEx | GSize       | Offset                          |

### SetViewRect / GetViewRect / GetViewSize

csharp

public static void SetViewRect(IntPtr dc, GRect rect)

public static GRect GetViewRect(IntPtr dc)

public static GSize GetViewSize(IntPtr dc)

**Description**: Sets or gets the visible canvas region.

**Parameters**

| Parameter | Type   | Description             |
|-----------|--------|-------------------------|
| dc        | IntPtr | Drawing context handle  |
| rect      | GRect  | Target rectangle region |

**Return Value**

| Method      | Return Type | Description           |
|-------------|-------------|-----------------------|
| GetViewRect | GRect       | Visible canvas region |
| GetViewSize | GSize       | Visible canvas size   |

### GetDisplayRect / GetClientRect / ResizeDisplayRect

csharp

public static GRect GetDisplayRect(IntPtr dc)

public static GRect GetClientRect(IntPtr dc)

public static void ResizeDisplayRect(IntPtr dc, int width, int height, bool bRefresh)

**Description**: Gets or adjusts the display region.

**Parameters**

| Parameter     | Type   | Description                    |
|---------------|--------|--------------------------------|
| dc            | IntPtr | Drawing context handle         |
| width, height | int    | New size                       |
| bRefresh      | bool   | Whether to refresh immediately |

**Return Value**

| Method         | Return Type | Description    |
|----------------|-------------|----------------|
| GetDisplayRect | GRect       | Display region |
| GetClientRect  | GRect       | Client area    |

### SetDisplayLeftTop / GetDisplayLeftTop

csharp

public static void SetDisplayLeftTop(IntPtr dc, int x, int y)

public static GPoint GetDisplayLeftTop(IntPtr dc)

**Description**: Sets or gets the top-left position of the display layer.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| x, y      | int    | Top-left coordinates   |

**Return Value** (only GetDisplayLeftTop)

| Type   | Description          |
|--------|----------------------|
| GPoint | Top-left coordinates |

### SetGraphAccuracy / GetGraphAccuracy

csharp

public static void SetGraphAccuracy(IntPtr dc, float accuracy)

public static float GetGraphAccuracy(IntPtr dc)

**Description**: Sets or gets the graph accuracy (precision parameter for curve fitting).

**Parameters**

| Parameter | Type   | Description                            |
|-----------|--------|----------------------------------------|
| dc        | IntPtr | Drawing context handle                 |
| accuracy  | float  | Accuracy value, e.g., 0.1, 0.01, 0.001 |

**Return Value** (only GetGraphAccuracy)

| Type  | Description      |
|-------|------------------|
| float | Current accuracy |

## Coordinate Conversion

### ConvertPointToCanvas / ConvertPointToCanvasE1 / ConvertPointToCanvasEx

csharp

public static GPoint ConvertPointToCanvas(IntPtr dc, int x, int y)

public static RvPointF32 ConvertPointToCanvasE1(IntPtr dc, float x, float y)

public static GPoint ConvertPointToCanvasEx(IntPtr dc, ref GPoint pPoint)

**Description**: Converts window coordinates to canvas coordinates.

**Parameters**

| Parameter | Type        | Description            |
|-----------|-------------|------------------------|
| dc        | IntPtr      | Drawing context handle |
| x, y      | int / float | Window coordinates     |
| pPoint    | ref GPoint  | Coordinate point       |

**Return Value**

| Method                 | Return Type | Description                       |
|------------------------|-------------|-----------------------------------|
| ConvertPointToCanvas   | GPoint      | Canvas coordinates                |
| ConvertPointToCanvasE1 | RvPointF32  | High-precision canvas coordinates |
| ConvertPointToCanvasEx | GPoint      | Converted coordinates             |

### ConvertPointToDisplay / ConvertPointToDisplayE1 / ConvertPointToDisplayEx

csharp

public static GPoint ConvertPointToDisplay(IntPtr dc, int x, int y)

public static RvPointF32 ConvertPointToDisplayE1(IntPtr dc, float x, float y)

public static GPoint ConvertPointToDisplayEx(IntPtr dc, ref GPoint pPoint)

**Description**: Converts canvas coordinates to window coordinates.

**Parameters**

| Parameter | Type        | Description            |
|-----------|-------------|------------------------|
| dc        | IntPtr      | Drawing context handle |
| x, y      | int / float | Canvas coordinates     |
| pPoint    | ref GPoint  | Coordinate point       |

**Return Value**: Converted window coordinates.

### ConvertXToCanvas / ConvertXToCanvasE1 / ConvertYToCanvas / ConvertYToCanvasE1

csharp

public static int ConvertXToCanvas(IntPtr dc, int value)

public static double ConvertXToCanvasE1(IntPtr dc, double value)

public static int ConvertYToCanvas(IntPtr dc, int value)

public static double ConvertYToCanvasE1(IntPtr dc, double value)

**Description**: Converts window X/Y coordinates to canvas coordinates. The E1 version returns a double-precision result.

**Parameters**

| Parameter | Type         | Description                 |
|-----------|--------------|-----------------------------|
| dc        | IntPtr       | Drawing context handle      |
| value     | int / double | Coordinate value to convert |

**Return Value**

| Method          | Return Type | Description                      |
|-----------------|-------------|----------------------------------|
| Integer version | int         | Canvas coordinate                |
| E1 version      | double      | High-precision canvas coordinate |

### ConvertXToDisplay / ConvertXToDisplayE1 / ConvertYToDisplay / ConvertYToDisplayE1

csharp

public static int ConvertXToDisplay(IntPtr dc, int value)

public static double ConvertXToDisplayE1(IntPtr dc, double value)

public static int ConvertYToDisplay(IntPtr dc, int value)

public static double ConvertYToDisplayE1(IntPtr dc, double value)

**Description**: Converts canvas X/Y coordinates to window coordinates.

**Parameters**

| Parameter | Type         | Description                 |
|-----------|--------------|-----------------------------|
| dc        | IntPtr       | Drawing context handle      |
| value     | int / double | Coordinate value to convert |

**Return Value**: Converted window coordinates.

### ConvertRectToCanvas / ConvertRectToCanvasEx / ConvertRectToDisplay / ConvertRectToDisplayEx

csharp

public static GRect ConvertRectToCanvas(IntPtr dc, int left, int top, int right, int bottom)

public static GRect ConvertRectToCanvasEx(IntPtr dc, ref GRect pRect)

public static GRect ConvertRectToDisplay(IntPtr dc, int left, int top, int right, int bottom)

public static GRect ConvertRectToDisplayEx(IntPtr dc, ref GRect pRect)

**Description**: Converts a rectangular region between window and canvas coordinates.

**Parameters**

| Parameter                | Type      | Description            |
|--------------------------|-----------|------------------------|
| dc                       | IntPtr    | Drawing context handle |
| left, top, right, bottom | int       | Rectangle boundaries   |
| pRect                    | ref GRect | Rectangle structure    |

**Return Value**

| Type  | Description         |
|-------|---------------------|
| GRect | Converted rectangle |

## Drawing Resources

### Pen

#### CreatePen / DestroyPen / SelectPen / GetCurPen

csharp

public static IntPtr CreatePen(GRgb color, int size, LinePattern style)

public static void DestroyPen(IntPtr hPen)

public static IntPtr SelectPen(IntPtr dc, IntPtr hPen)

public static IntPtr GetCurPen(IntPtr dc)

**Description**: Creates, destroys, selects, or gets the current pen.

**Parameters**

| Parameter | Type        | Description                                    |
|-----------|-------------|------------------------------------------------|
| color     | GRgb        | Pen color                                      |
| size      | int         | Line width                                     |
| style     | LinePattern | Line pattern: Solid, Hidden, Center, Inclusion |
| hPen      | IntPtr      | Pen handle                                     |
| dc        | IntPtr      | Drawing context handle                         |

**Return Value**

| Method    | Return Type | Description        |
|-----------|-------------|--------------------|
| CreatePen | IntPtr      | New pen handle     |
| SelectPen | IntPtr      | Old pen handle     |
| GetCurPen | IntPtr      | Current pen handle |

**Usage Example**

csharp

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(dc, hPen);

*// ... draw ...*

Render.SelectPen(dc, hOldPen);

Render.DestroyPen(hPen);

#### SetPenColor / GetPenColor / SetPenSize / GetPenSize / SetPenPattern / GetPenPattern

csharp

public static void SetPenColor(IntPtr hPen, GRgb color)

public static GRgb GetPenColor(IntPtr hPen)

public static void SetPenSize(IntPtr hPen, int size)

public static int GetPenSize(IntPtr hPen)

public static void SetPenPattern(IntPtr hPen, int pattern)

public static int GetPenPattern(IntPtr hPen)

**Description**: Sets or gets the pen color, line width, or line pattern.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| hPen      | IntPtr | Pen handle   |
| color     | GRgb   | Color        |
| size      | int    | Line width   |
| pattern   | int    | Line pattern |

**Return Value**: The corresponding getter returns the current value.

### Brush

#### CreateBrush / DestroyBrush / SelectBrush / GetCurBrush

csharp

public static IntPtr CreateBrush(BrushPattern type, int size, GRgb color)

public static void DestroyBrush(IntPtr hBrush)

public static IntPtr SelectBrush(IntPtr dc, IntPtr hBrush)

public static IntPtr GetCurBrush(IntPtr dc)

**Description**: Creates, destroys, selects, or gets the current brush.

**Parameters**

| Parameter | Type         | Description                                        |
|-----------|--------------|----------------------------------------------------|
| type      | BrushPattern | Fill pattern: Solid, BackwardDiagonal, Cross, etc. |
| size      | int          | Pattern size                                       |
| color     | GRgb         | Color                                              |
| hBrush    | IntPtr       | Brush handle                                       |

**Return Value**

| Method      | Return Type | Description          |
|-------------|-------------|----------------------|
| CreateBrush | IntPtr      | New brush handle     |
| SelectBrush | IntPtr      | Old brush handle     |
| GetCurBrush | IntPtr      | Current brush handle |

#### SetBrushColor / GetBrushColor / SetBrushSize / GetBrushSize / SetBrushPattern / GetBrushPattern

csharp

public static void SetBrushColor(IntPtr hBrush, GRgb color)

public static GRgb GetBrushColor(IntPtr hBrush)

public static void SetBrushSize(IntPtr hBrush, int size)

public static int GetBrushSize(IntPtr hBrush)

public static void SetBrushPattern(IntPtr hBrush, int pattern)

public static int GetBrushPattern(IntPtr hBrush)

**Description**: Sets or gets the brush color, pattern size, or pattern type.

### Font

#### CreateFont / CreateFontEx / DestroyFont / SelectFont / GetCurFont

csharp

public static IntPtr CreateFont(string strFamilyName, int size, FontFlags flags)

public static IntPtr CreateFontEx(string strFamilyName, int fontSize, GRgb textColor, GRgb backColor, FontFlags flags)

public static void DestroyFont(IntPtr pFont)

public static IntPtr SelectFont(IntPtr dc, IntPtr pFont)

public static IntPtr GetCurFont(IntPtr dc)

**Description**: Creates, destroys, selects, or gets the current font.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| strFamilyName | string | Font name, e.g., "Arial" |
| size / fontSize | int | Font size |
| flags | FontFlags | Style: Bold, Italic, Underline, Strikeout, Outline |
| textColor | GRgb | Text color (CreateFontEx) |
| backColor | GRgb | Background color (CreateFontEx) |
| pFont | IntPtr | Font handle |

**Return Value**

| Method                    | Return Type | Description         |
|---------------------------|-------------|---------------------|
| CreateFont / CreateFontEx | IntPtr      | New font handle     |
| SelectFont                | IntPtr      | Old font handle     |
| GetCurFont                | IntPtr      | Current font handle |

#### rvgSetFontTextColor / rvgGetFontTextColor / rvgSetFontBackColor / rvgGetFontBackColor

csharp

public static void rvgSetFontTextColor(IntPtr hFont, GRgb color)

public static GRgb rvgGetFontTextColor(IntPtr hFont)

public static void rvgSetFontBackColor(IntPtr hFont, GRgb color)

public static GRgb rvgGetFontBackColor(IntPtr hFont)

**Description**: Sets or gets the font's text color and background color.

### Mold

Used to control the transformation applied when drawing images.

#### CreateMold / DestroyMold / SelectMold / GetCurMold

csharp

public static IntPtr CreateMold(MoldType type, int prime, int minor)

public static void DestroyMold(IntPtr pMold)

public static IntPtr SelectMold(IntPtr dc, IntPtr pModel)

public static IntPtr GetCurMold(IntPtr dc)

**Description**: Creates, destroys, selects, or gets the current mold.

**Parameters**

| Parameter    | Type     | Description              |
|--------------|----------|--------------------------|
| type         | MoldType | Mold type                |
| prime, minor | int      | Type-specific parameters |
| pMold        | IntPtr   | Mold handle              |

**Return Value**

| Method     | Return Type | Description         |
|------------|-------------|---------------------|
| CreateMold | IntPtr      | New mold handle     |
| SelectMold | IntPtr      | Old mold handle     |
| GetCurMold | IntPtr      | Current mold handle |

#### CreateAlignMold

csharp

public static IntPtr CreateAlignMold(int horizonAlign, int verticalAlign)

**Description**: Creates an align mold. Controls how the image aligns within the target rectangle.

**Parameters**

| Parameter     | Type | Description                                |
|---------------|------|--------------------------------------------|
| horizonAlign  | int  | Horizontal alignment: Left, Hcenter, Right |
| verticalAlign | int  | Vertical alignment: Top, Vcenter, Bottom   |

**Return Value**

| Type   | Description |
|--------|-------------|
| IntPtr | Mold handle |

#### CreateRotateMold

csharp

public static IntPtr CreateRotateMold(float angle, bool bKeepSize)

**Description**: Creates a rotate mold.

**Parameters**

| Parameter | Type  | Description                                    |
|-----------|-------|------------------------------------------------|
| angle     | float | Rotation angle (degrees); positive = clockwise |
| bKeepSize | bool  | Whether to keep the original image size        |

**Return Value**

| Type   | Description |
|--------|-------------|
| IntPtr | Mold handle |

#### CreateStretchMold

csharp

public static IntPtr CreateStretchMold(bool bKeepRatio, bool bKeepSize)

**Description**: Creates a stretch mold.

**Parameters**

| Parameter  | Type | Description                       |
|------------|------|-----------------------------------|
| bKeepRatio | bool | Whether to keep the aspect ratio  |
| bKeepSize  | bool | Whether to keep the original size |

**Return Value**

| Type   | Description |
|--------|-------------|
| IntPtr | Mold handle |

#### CreateScaleMold

csharp

public static IntPtr CreateScaleMold(float scaleX, float scaleY, bool bCentered)

**Description**: Creates a scale mold.

**Parameters**

| Parameter      | Type  | Description                           |
|----------------|-------|---------------------------------------|
| scaleX, scaleY | float | Horizontal and vertical scale factors |
| bCentered      | bool  | Whether to center                     |

**Return Value**

| Type   | Description |
|--------|-------------|
| IntPtr | Mold handle |

#### CreateSkewMold

csharp

public static IntPtr CreateSkewMold(RvPoint\[\] vertex, bool bKeepSize)

**Description**: Creates a skew mold. The target quadrilateral is defined by four vertices.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| vertex | RvPoint\[\] | Four vertices: top-left, top-right, bottom-right, bottom-left |
| bKeepSize | bool | Whether to keep the original size |

**Return Value**

| Type   | Description |
|--------|-------------|
| IntPtr | Mold handle |

#### CreateTileMold

csharp

public static IntPtr CreateTileMold(int rows, int cols, int mirrorType)

**Description**: Creates a tile mold.

**Parameters**

| Parameter  | Type | Description                                   |
|------------|------|-----------------------------------------------|
| rows, cols | int  | Row and column counts                         |
| mirrorType | int  | Mirror type: None, Horizontal, Vertical, Both |

**Return Value**

| Type   | Description |
|--------|-------------|
| IntPtr | Mold handle |

#### CreateFlipMold

csharp

public static IntPtr CreateFlipMold(int type)

**Description**: Creates a flip mold.

**Parameters**

| Parameter | Type | Description                                  |
|-----------|------|----------------------------------------------|
| type      | int  | Flip type: Never, Horizontal, Vertical, Both |

**Return Value**

| Type   | Description |
|--------|-------------|
| IntPtr | Mold handle |

## Primitive Drawing

All drawing functions return a stroke handle (IntPtr). An existing handle may be passed to reuse the container.

### Points

csharp

public static IntPtr DrawPoint(IntPtr dc, int x, int y, IntPtr hStroke)

public static IntPtr DrawPointE1(IntPtr dc, float x, float y, IntPtr hStroke)

public static IntPtr DrawPointEx(IntPtr dc, GPoint pos, IntPtr hStroke)

public static IntPtr DrawDots(IntPtr dc, RvPoint\[\] arr, IntPtr hStroke)

public static IntPtr DrawDots(IntPtr dc, RvPointF32\[\] arr, IntPtr hStroke)

**Description**: Draws a single point or multiple independent points.

**Parameters**

| Parameter | Type                         | Description            |
|-----------|------------------------------|------------------------|
| dc        | IntPtr                       | Drawing context handle |
| x, y      | int / float                  | Point coordinates      |
| pos       | GPoint                       | Coordinate structure   |
| arr       | RvPoint\[\] / RvPointF32\[\] | Point array            |
| hStroke   | IntPtr                       | Optional stroke handle |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Lines

csharp

public static IntPtr DrawLine(IntPtr dc, int x0, int y0, int x1, int y1, IntPtr hStroke)

public static IntPtr DrawLineE1(IntPtr dc, float x0, float y0, float x1, float y1, IntPtr hStroke)

public static IntPtr DrawLineEx(IntPtr dc, GPoint p0, GPoint p1, IntPtr hStroke)

public static IntPtr DrawSegments(IntPtr dc, RvPoint\[\] arr, IntPtr hStroke)

public static IntPtr DrawSegments(IntPtr dc, RvPointF32\[\] arr, IntPtr hStroke)

public static IntPtr DrawSegments(IntPtr dc, IntPtr pPointArray, int nPointCount, IntPtr hStroke)

public static IntPtr DrawSegmentsE1(IntPtr dc, IntPtr pPointArray, int nPointCount, IntPtr hStroke)

**Description**: Draws a single line or multiple independent line segments. In DrawSegments, every two points in the array form one segment.

**Parameters**

| Parameter      | Type        | Description               |
|----------------|-------------|---------------------------|
| dc             | IntPtr      | Drawing context handle    |
| x0, y0, x1, y1 | int / float | Start and end coordinates |
| p0, p1         | GPoint      | Start and end points      |
| arr            | Point array | Segment endpoint array    |
| pPointArray    | IntPtr      | Point array pointer       |
| nPointCount    | int         | Point count               |
| hStroke        | IntPtr      | Optional stroke handle    |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Polyline

csharp

public static IntPtr DrawPolyline(IntPtr dc, RvPoint\[\] arr, IntPtr hStroke)

public static IntPtr DrawPolyline(IntPtr dc, RvPointF32\[\] arr, IntPtr hStroke)

public static IntPtr DrawPolyline(IntPtr dc, IntPtr pVertexSet, int nVertexCount, IntPtr hStroke)

public static IntPtr DrawPolylineE1(IntPtr dc, IntPtr pVertexSet, int nVertexCount, IntPtr hStroke)

**Description**: Draws a polyline; vertices are connected in order.

**Parameters**

| Parameter    | Type        | Description            |
|--------------|-------------|------------------------|
| dc           | IntPtr      | Drawing context handle |
| arr          | Point array | Vertex array           |
| pVertexSet   | IntPtr      | Vertex array pointer   |
| nVertexCount | int         | Vertex count           |
| hStroke      | IntPtr      | Optional stroke handle |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Rectangles

csharp

public static IntPtr DrawRect(IntPtr dc, int left, int top, int right, int bottom, bool bFill, IntPtr hStroke)

public static IntPtr DrawRectE1(IntPtr dc, int left, int top, int right, int bottom, float rotate, bool bFill, IntPtr hStroke)

public static IntPtr DrawRectEx(IntPtr dc, float left, float top, float right, float bottom, float rotate, bool bFill, IntPtr hStroke)

**Description**: Draws a rectangle. DrawRect draws an axis-aligned rectangle; DrawRectE1 / DrawRectEx support rotation.

**Parameters**

| Parameter                | Type        | Description              |
|--------------------------|-------------|--------------------------|
| dc                       | IntPtr      | Drawing context handle   |
| left, top, right, bottom | int / float | Boundary coordinates     |
| rotate                   | float       | Rotation angle (degrees) |
| bFill                    | bool        | Whether to fill          |
| hStroke                  | IntPtr      | Optional stroke handle   |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Circles and Ellipses

csharp

public static IntPtr DrawCircle(IntPtr dc, int x0, int y0, int radius, bool bFill, IntPtr hStroke)

public static IntPtr DrawCircleE1(IntPtr dc, float cx, float cy, float radius, bool bFill, IntPtr hStroke)

public static IntPtr DrawCircleEx(IntPtr dc, GPoint center, int radius, bool bFill, IntPtr hStroke)

public static IntPtr DrawEllipse(IntPtr dc, int x0, int y0, int rx, int ry, float angle, bool bFill, IntPtr hStroke)

public static IntPtr DrawEllipseE1(IntPtr dc, float cx, float cy, float rx, float ry, float angle, bool bFill, IntPtr hStroke)

public static IntPtr DrawEllipseEx(IntPtr dc, GPoint center, int rx, int ry, float angle, bool bFill, IntPtr hStroke)

**Description**: Draws a circle or ellipse.

**Parameters**

| Parameter       | Type        | Description               |
|-----------------|-------------|---------------------------|
| dc              | IntPtr      | Drawing context handle    |
| x0, y0 / cx, cy | int / float | Center coordinates        |
| center          | GPoint      | Center point              |
| radius          | int / float | Circle radius             |
| rx, ry          | int / float | Ellipse semi-axis lengths |
| angle           | float       | Rotation angle (degrees)  |
| bFill           | bool        | Whether to fill           |
| hStroke         | IntPtr      | Optional stroke handle    |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Arcs

csharp

public static IntPtr DrawArc(IntPtr dc, int x0, int y0, int radius, float startAngle, float endAngle, IntPtr hStroke)

public static IntPtr DrawArcE1(IntPtr dc, float cx, float cy, float radius, float startAngle, float endAngle, IntPtr hStroke)

public static IntPtr DrawArcEx(IntPtr dc, GPoint center, int radius, float startAngle, float endAngle, bool bSector, IntPtr hStroke)

**Description**: Draws an arc or sector.

**Parameters**

| Parameter            | Type        | Description                          |
|----------------------|-------------|--------------------------------------|
| dc                   | IntPtr      | Drawing context handle               |
| x0, y0 / cx, cy      | int / float | Center coordinates                   |
| center               | GPoint      | Center point                         |
| radius               | int / float | Radius                               |
| startAngle, endAngle | float       | Start and end angles (degrees)       |
| bSector              | bool        | Whether to draw a sector (DrawArcEx) |
| hStroke              | IntPtr      | Optional stroke handle               |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Crosses

csharp

public static IntPtr DrawCross(IntPtr dc, int cx, int cy, int sx, int sy, float angle, IntPtr hStroke)

public static IntPtr DrawCrossE1(IntPtr dc, float cx, float cy, float sx, float sy, float angle, IntPtr hStroke)

**Description**: Draws a cross.

**Parameters**

| Parameter | Type        | Description                                       |
|-----------|-------------|---------------------------------------------------|
| dc        | IntPtr      | Drawing context handle                            |
| cx, cy    | int / float | Cross center coordinates                          |
| sx, sy    | int / float | Arm lengths in horizontal and vertical directions |
| angle     | float       | Rotation angle (degrees)                          |
| hStroke   | IntPtr      | Optional stroke handle                            |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Triangles

csharp

public static IntPtr DrawTriangle(IntPtr dc, int cx, int cy, int size, float angle, bool bFill, IntPtr hStroke)

public static IntPtr DrawTriangles(IntPtr dc, RvPoint\[\] arr, int type, IntPtr hStroke)

public static IntPtr DrawTriangles(IntPtr dc, RvPointF32\[\] arr, int type, IntPtr hStroke)

**Description**: Draws an equilateral triangle or multiple triangles. DrawTriangles supports three modes: separate, fan, and strip.

**Parameters**

| Parameter | Type        | Description                                  |
|-----------|-------------|----------------------------------------------|
| dc        | IntPtr      | Drawing context handle                       |
| cx, cy    | int         | Triangle center                              |
| size      | int         | Triangle size                                |
| angle     | float       | Rotation angle (degrees)                     |
| arr       | Point array | Vertex array                                 |
| type      | int         | Triangle type: DT_SEPARATE, DT_FAN, DT_STRIP |
| bFill     | bool        | Whether to fill                              |
| hStroke   | IntPtr      | Optional stroke handle                       |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Polygons

csharp

public static IntPtr DrawPolygon(IntPtr dc, RvPoint\[\] arr, bool bFill, IntPtr hStroke)

public static IntPtr DrawPolygon(IntPtr dc, RvPointF32\[\] arr, bool bFill, IntPtr hStroke)

public static IntPtr DrawPolygon(IntPtr dc, RvPointF64\[\] arr64, bool bFill, IntPtr hStroke)

**Description**: Draws a polygon.

**Parameters**

| Parameter   | Type        | Description            |
|-------------|-------------|------------------------|
| dc          | IntPtr      | Drawing context handle |
| arr / arr64 | Point array | Vertex array           |
| bFill       | bool        | Whether to fill        |
| hStroke     | IntPtr      | Optional stroke handle |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### RvBox2D

csharp

public static IntPtr DrawBox2D(IntPtr dc, RvBox2D box, IntPtr hStroke)

**Description**: Draws a rotated rectangle outline.

**Parameters**

| Parameter | Type    | Description                   |
|-----------|---------|-------------------------------|
| dc        | IntPtr  | Drawing context handle        |
| box       | RvBox2D | Rotated rectangle description |
| hStroke   | IntPtr  | Optional stroke handle        |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

### Text

csharp

public static IntPtr DrawText(IntPtr dc, string strText, int x, int y, IntPtr hStroke)

public static IntPtr DrawText(IntPtr dc, string strText, GRect rect, IntPtr hStroke)

public static IntPtr DrawTextEx(IntPtr dc, string strText, GRect rect, int nFormat, int lineSpace, IntPtr hStroke)

public static void TextOut(IntPtr dc, string strText)

public static void TextOut(IntPtr dc, string strText, int fontSize)

public static GSize GetTextSize(IntPtr dc, string strText, int count)

public static void ArrangeText(IntPtr dc, int charSpace, int lineGap)

**Description**: Draws text or sets text layout parameters.

- DrawText / DrawTextEx: returns a stroke handle, supporting subsequent modification, hiding, and deletion.

- TextOut: outputs directly to the image frame layer without returning a handle; irreversible.

- GetTextSize: computes text size.

- ArrangeText: sets character spacing and line spacing.

**Parameters**

| Parameter | Type   | Description                                         |
|-----------|--------|-----------------------------------------------------|
| dc        | IntPtr | Drawing context handle                              |
| strText   | string | Text content                                        |
| x, y      | int    | Starting coordinates                                |
| rect      | GRect  | Layout region                                       |
| nFormat   | int    | Format flags, combined bitwise from the DT\_ series |
| lineSpace | int    | Line spacing                                        |
| fontSize  | int    | Font size                                           |
| count     | int    | Character count                                     |
| charSpace | int    | Character spacing                                   |
| lineGap   | int    | Line spacing                                        |
| hStroke   | IntPtr | Optional stroke handle                              |

**Return Value**

| Method                | Return Type | Description   |
|-----------------------|-------------|---------------|
| DrawText / DrawTextEx | IntPtr      | Stroke handle |
| GetTextSize           | GSize       | Text size     |
| TextOut / ArrangeText | None        | —             |

### Images

csharp

public static IntPtr DrawImage(IntPtr dc, IntPtr image, IntPtr hStroke)

public static IntPtr DrawImageEx(IntPtr dc, IntPtr image, int x, int y, int width, int height, IntPtr hStroke)

public static IntPtr DrawImageE1(IntPtr dc, IntPtr image, IntPtr vertexArray, IntPtr hStroke)

public static IntPtr DrawImageE2(IntPtr dc, IntPtr image, GRect rect, IntPtr hStroke)

public static void PaintImage(IntPtr dc, IntPtr image, int left, int top)

public static bool FeedFrame(IntPtr dc, IntPtr image)

public static void BitBlt(IntPtr dc, IntPtr image, int canvasLeft, int canvasTop, int canvasWidth, int canvasHeight, int imageLeft, int imageTop, int imageWidth, int imageHeight, uint ignore)

**Description**: Draws images.

- DrawImage / DrawImageEx / DrawImageE1 / DrawImageE2: returns a stroke handle, supporting mold transforms and subsequent control.

- PaintImage: draws directly to the image frame layer without returning a handle; irreversible.

- FeedFrame: feeds the image into the image frame layer for real-time display.

- BitBlt: bit-block transfer.

**Parameters**

| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| dc            | IntPtr | Drawing context handle     |
| image         | IntPtr | Image handle               |
| x, y          | int    | Target origin              |
| width, height | int    | Target width and height    |
| vertexArray   | IntPtr | Quadrilateral vertex array |
| rect          | GRect  | Target rectangle           |
| left, top     | int    | Drawing origin             |
| ignore        | uint   | Ignore color               |
| hStroke       | IntPtr | Optional stroke handle     |

**Return Value**

| Method      | Return Type | Description        |
|-------------|-------------|--------------------|
| DrawImage\* | IntPtr      | Stroke handle      |
| FeedFrame   | bool        | Whether successful |
| Others      | None        | —                  |

**Usage Example**

csharp

KImage im = new KImage("..\\samples\\waterdrop.png");

GRect rect = new GRect(30, 30, 150, 120);

m_hPictureStroke = Render.DrawImageE2(dc, im.GetHandle(), rect, IntPtr.Zero);

### Masks and BLOBs

csharp

public static IntPtr DrawMask(IntPtr dc, IntPtr mask, int dx, int dy, IntPtr hStroke)

public static IntPtr DrawBlob(IntPtr dc, IntPtr blob, int dx, int dy, IntPtr hStroke)

public static IntPtr DrawContour(IntPtr dc, IntPtr contour, int dx, int dy, IntPtr hStroke)

public static IntPtr DrawMatrix(IntPtr dc, IntPtr matrix, IntPtr hStroke)

**Description**: Draws mask, BLOB, contour, or matrix objects.

**Parameters**

| Parameter                      | Type   | Description            |
|--------------------------------|--------|------------------------|
| dc                             | IntPtr | Drawing context handle |
| mask / blob / contour / matrix | IntPtr | Target object handle   |
| dx, dy                         | int    | Position offset        |
| hStroke                        | IntPtr | Optional stroke handle |

**Return Value**

| Type   | Description   |
|--------|---------------|
| IntPtr | Stroke handle |

## Stroke Management

### DrawDummy

csharp

public static IntPtr DrawDummy(IntPtr dc, IntPtr hStroke)

**Description**: Creates a dummy stroke that performs no actual drawing, returning an empty handle. It can be reused as a general container, saving resource release overhead.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| hStroke   | IntPtr | Optional stroke handle |

**Return Value**

| Type   | Description         |
|--------|---------------------|
| IntPtr | Empty stroke handle |

**Usage Example**

csharp

m_hCurStroke = Render.DrawDummy(dc, IntPtr.Zero);

### Erase

csharp

public static void Erase(IntPtr dc, IntPtr hStroke, EraseType type, bool bRefresh = false)

**Description**: Deletes or hides a stroke.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| dc | IntPtr | Drawing context handle |
| hStroke | IntPtr | Stroke handle |
| type | EraseType | Operation type: Hide (hide), Delete (delete and release resources), Restore (restore display) |
| bRefresh | bool | Whether to refresh immediately |

**Return Value**: None.

**Notes**

- Delete releases the stroke's resources; the handle becomes invalid immediately and the related variable must be set to IntPtr.Zero.

### Hide

csharp

public static void Hide(IntPtr dc, IntPtr hStroke, bool flag, bool bRefresh = false)

**Description**: Hides or shows a stroke without releasing resources.

**Parameters**

| Parameter | Type   | Description                        |
|-----------|--------|------------------------------------|
| dc        | IntPtr | Drawing context handle             |
| hStroke   | IntPtr | Stroke handle                      |
| flag      | bool   | true hides; false restores display |
| bRefresh  | bool   | Whether to refresh immediately     |

**Return Value**: None.

### Modify

csharp

public static void Modify(IntPtr dc, IntPtr hStroke, ModifyType type, UIntPtr value, bool bRefresh = false)

**Description**: Modifies stroke content. In reuse mode, the returned handle is the same as the passed handle; only the internal definition is replaced.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| dc | IntPtr | Drawing context handle |
| hStroke | IntPtr | Stroke handle |
| type | ModifyType | Modification type: Transparence, Color, Image, Text |
| value | UIntPtr | Modification parameter (usually a pointer to a handle or value) |
| bRefresh | bool | Whether to refresh immediately |

**Return Value**: None.

**Usage Example**

csharp

KImage im = new KImage("..\\samples\\waterdrop.png");

NativeArg arg = new NativeArg(im.Handle);

Render.Modify(dc, m_hCurStroke, ModifyType.Image, arg.Ptr);

### Offset

csharp

public static void Offset(IntPtr dc, IntPtr hStroke, float dx, float dy, bool bRefresh = false)

**Description**: Translates a stroke.

**Parameters**

| Parameter | Type   | Description                     |
|-----------|--------|---------------------------------|
| dc        | IntPtr | Drawing context handle          |
| hStroke   | IntPtr | Stroke handle                   |
| dx, dy    | float  | Horizontal and vertical offsets |
| bRefresh  | bool   | Whether to refresh immediately  |

**Return Value**: None.

### Combine

csharp

public static IntPtr Combine(IntPtr dc, IntPtr\[\] strokeArray)

**Description**: Combines multiple strokes into a composite stroke. Original handles become invalid.

**Parameters**

| Parameter   | Type       | Description            |
|-------------|------------|------------------------|
| dc          | IntPtr     | Drawing context handle |
| strokeArray | IntPtr\[\] | Strokes to combine     |

**Return Value**

| Type   | Description                |
|--------|----------------------------|
| IntPtr | New combined stroke handle |

**Usage Example**

csharp

IntPtr\[\] arr = new IntPtr\[2\] { m_hPictureStroke, m_hRectangleStroke };

m_hMergeStroke = Render.Combine(dc, arr);

m_hPictureStroke = IntPtr.Zero;

m_hRectangleStroke = IntPtr.Zero;

### Uncombine

csharp

public static IntPtr\[\] Uncombine(IntPtr dc, IntPtr hMergedStroke, int subStrokeCount)

**Description**: Splits a composite stroke into multiple independent strokes. The original handle becomes invalid.

**Parameters**

| Parameter      | Type   | Description                    |
|----------------|--------|--------------------------------|
| dc             | IntPtr | Drawing context handle         |
| hMergedStroke  | IntPtr | Composite stroke handle        |
| subStrokeCount | int    | Expected number of sub-strokes |

**Return Value**

| Type       | Description                                                |
|------------|------------------------------------------------------------|
| IntPtr\[\] | Array of split sub-stroke handles; returns null on failure |

**Usage Example**

csharp

IntPtr\[\] subarr = Render.Uncombine(dc, m_hMergeStroke, 2);

m_hMergeStroke = IntPtr.Zero;

## Clearing and Refreshing

### Clear

csharp

public static void Clear(IntPtr dc, CanvasLayer layers, bool bRefresh = false)

**Description**: Clears the specified layers. Clearing the image frame layer makes the canvas display the background color; clearing the foreground layer invalidates all stroke handles.

**Parameters**

| Parameter | Type        | Description                                      |
|-----------|-------------|--------------------------------------------------|
| dc        | IntPtr      | Drawing context handle                           |
| layers    | CanvasLayer | Target layers: Background, Foreground, All, etc. |
| bRefresh  | bool        | Whether to refresh immediately                   |

**Return Value**: None.

**Usage Example**

csharp

Render.Clear(dc, CanvasLayer.Background); *// Clear the image frame layer*

Render.Clear(dc, CanvasLayer.Foreground); *// Clear all strokes*

### Realize / Flush

csharp

public static void Realize(IntPtr dc, CanvasLayer layers, bool bRefresh = false)

public static bool Flush(IntPtr dc)

**Description**: Commits drawing commands and refreshes to the window.

- Realize: commits drawing commands to the specified layers.

- Flush: flushes the drawing context, presenting the rendered result to the window.

**Parameters**

| Parameter | Type        | Description                    |
|-----------|-------------|--------------------------------|
| dc        | IntPtr      | Drawing context handle         |
| layers    | CanvasLayer | Target layers                  |
| bRefresh  | bool        | Whether to refresh immediately |

**Return Value** (only Flush)

| Type | Description               |
|------|---------------------------|
| bool | Whether refresh succeeded |

**Usage Example**

csharp

Render.Realize(dc, CanvasLayer.All);

Render.Flush(dc);

## Export

### ExportCanvas

csharp

public static IntPtr ExportCanvas(IntPtr dc, IntPtr image)

**Description**: Exports canvas content to the specified image.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| image     | IntPtr | Target image handle    |

**Return Value**

| Type   | Description         |
|--------|---------------------|
| IntPtr | Result image handle |

**Notes**

- A Generic context exports only the image frame layer; an Enhanced context exports image frame and canvas layer content. Intermediate and screen layers are not exported.

**Usage Example**

csharp

KImage im = new KImage();

Render.ExportCanvas(dc, im.GetHandle());

im.Save("output.jpg");

## Logo and Appearance

### SetRvbLogoVisible / IsRvbLogoVisible

csharp

public static void SetRvbLogoVisible(IntPtr dc, bool flag)

public static bool IsRvbLogoVisible(IntPtr dc)

**Description**: Sets or determines whether the RVB LOGO is visible.

**Parameters**

| Parameter | Type   | Description             |
|-----------|--------|-------------------------|
| dc        | IntPtr | Drawing context handle  |
| flag      | bool   | true shows; false hides |

**Return Value** (only IsRvbLogoVisible)

| Type | Description              |
|------|--------------------------|
| bool | Current visibility state |

### SetRvbLogoPlacement / GetRvbLogoPlacement

csharp

public static void SetRvbLogoPlacement(IntPtr dc, LogoPlacement placement)

public static LogoPlacement GetRvbLogoPlacement(IntPtr dc)

**Description**: Sets or gets the LOGO display position.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| dc | IntPtr | Drawing context handle |
| placement | LogoPlacement | Position: LeftTop, RightTop, LeftBottom, RightBottom |

**Return Value** (only GetRvbLogoPlacement)

| Type          | Description      |
|---------------|------------------|
| LogoPlacement | Current position |

### SetTrasparence / GetTrasparence

csharp

public static float SetTrasparence(IntPtr self, float rate)

public static float GetTrasparence(IntPtr self)

**Description**: Sets or gets the drawing transparency. 0 is opaque, 1 is fully transparent.

**Parameters**

| Parameter | Type   | Description              |
|-----------|--------|--------------------------|
| self      | IntPtr | Drawing context handle   |
| rate      | float  | Transparency ratio (0~1) |

**Return Value**

| Method         | Return Type | Description           |
|----------------|-------------|-----------------------|
| SetTrasparence | float       | Previous transparency |
| GetTrasparence | float       | Current transparency  |

## Other

### SetDrawStyle / GetDrawStyle

csharp

public static void SetDrawStyle(IntPtr dc, int flag)

public static int GetDrawStyle(IntPtr dc)

**Description**: Sets or gets the drawing style flag, combined bitwise from the GS\_ series.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| flag      | int    | Style flag             |

**Return Value** (only GetDrawStyle)

| Type | Description        |
|------|--------------------|
| int  | Current style flag |

### SetCurrentLayer / GetCurrentLayer

csharp

public static int SetCurrentLayer(IntPtr dc, int nLayer)

public static int GetCurrentLayer(IntPtr dc)

**Description**: Sets or gets the current drawing layer.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| dc        | IntPtr | Drawing context handle |
| nLayer    | int    | Layer identifier       |

**Return Value**

| Method          | Return Type | Description    |
|-----------------|-------------|----------------|
| SetCurrentLayer | int         | Previous layer |
| GetCurrentLayer | int         | Current layer  |

### MoveTo / MoveToEx / LineTo / LineToEx

csharp

public static void MoveTo(IntPtr dc, int x, int y)

public static void MoveToEx(IntPtr dc, GPoint pos)

public static void LineTo(IntPtr dc, int x, int y)

public static void LineToEx(IntPtr dc, GPoint pos)

**Description**: Moves the current position or draws a line from the current position to the target point. Commonly used for continuous drawing of text and polylines.

**Parameters**

| Parameter | Type   | Description                 |
|-----------|--------|-----------------------------|
| dc        | IntPtr | Drawing context handle      |
| x, y      | int    | Target coordinates          |
| pos       | GPoint | Target coordinate structure |

**Return Value**: None.

**Usage Example**

csharp

Render.MoveTo(dc, 30, 40);

Render.TextOut(dc, "Hello", 16);

### SetDock / GetDock

csharp

public static IntPtr SetDock(IntPtr self, IntPtr pDock)

public static IntPtr GetDock(IntPtr self)

**Description**: Sets or gets the dock layout.

**Parameters**

| Parameter | Type   | Description            |
|-----------|--------|------------------------|
| self      | IntPtr | Drawing context handle |
| pDock     | IntPtr | Dock layout handle     |

**Return Value**

| Method  | Return Type | Description          |
|---------|-------------|----------------------|
| SetDock | IntPtr      | Previous dock layout |
| GetDock | IntPtr      | Current dock layout  |
