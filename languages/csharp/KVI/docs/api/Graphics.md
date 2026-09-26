## Graphics and Image Drawing

Corresponding project: Graphics

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\Graphics_media/media/image1.png" style="width:6.74444in;height:3.95486in" />The graphics and image drawing module is developed based on the paper "A Method for Graphics and Image Drawing Based on Strokes" (authors: Huang Dongyun et al.). This stroke model uses OpenGL as the underlying graphics interface and does not rely on image drawing methods such as GDI/GDI+ provided by the Windows operating system itself. Instead, it interacts directly with the graphics card, and the GPU completes the drawing and compositing of graphics and images. This architecture, which bypasses the system drawing layer and connects directly to hardware, reduces the overhead of data copying and state switching between the CPU and system interfaces, significantly improves drawing speed, and can meet the high-frame-rate real-time drawing requirements of large-resolution images and complex graphics.

In the stroke model, graphics and images are decomposed into a series of basic geometric and texture unit strokes, and the final picture is constructed by arranging, superimposing, and transforming strokes. Compared with pixel-by-pixel operations or traditional bitmap drawing, the stroke model is closer to the human painting process and facilitates parallel processing while maintaining drawing flexibility. OpenGL provides hardware-accelerated vertex processing, texture mapping, blending, and shading capabilities, and can efficiently submit a large number of strokes to the GPU for execution, fully utilizing the parallel computing performance of the graphics card. This solution is particularly suitable for fields requiring real-time interaction and ultra-high-resolution display, such as medical imaging, remote sensing images, industrial inspection, and scientific visualization.

Overall, this module takes the stroke model as its core, uses OpenGL hardware acceleration as its means, bypasses system drawing interfaces, and fully leverages graphics card performance, providing an efficient and reliable solution for real-time drawing of high-resolution, complex graphics.

## Creating and Releasing the Drawing Context

XguiPanel is a custom control used to provide a rendering host window for the graphics and image drawing module. After dragging it onto a WinForms form, a drawing context can be created at runtime based on the control's handle, and then real-time drawing and display of graphics and images can be completed through OpenGL hardware acceleration.

csharp

public partial class XguiPanel : Control

{

public XguiPanel()

{

InitializeComponent();

if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

SetStyle(ControlStyles.AllPaintingInWmPaint \| *// Draw everything in WM_PAINT*

ControlStyles.Opaque, *// Control is opaque, skip background erase*

true);

SetStyle(ControlStyles.UserPaint, true); *// User-defined painting (but we don't actually paint)*

*// SetStyle(ControlStyles.DoubleBuffer, false); // Disable double buffering (decided by user)*

UpdateStyles();

}

protected override void OnPaint(PaintEventArgs pe)

{

base.OnPaint(pe);

if (this.DesignMode \|\| this.Handle == IntPtr.Zero) return;

*// base.OnPaint(pe);*

}

}

XguiPanel inherits from Control. It is a lightweight custom control that does not perform any drawing logic itself and only serves as a host container for the rendering context. Since drawing work is handed over to OpenGL through the image drawing module (xgui.dll) to be completed directly by the graphics card, the control itself does not need to handle the Paint event. This design decouples window management from graphics rendering: the control is responsible for providing the window handle and message loop, and the drawing module is responsible for interacting with the graphics card through that handle. Each performs its own role, avoiding the performance overhead brought by system drawing interfaces.

**Control Style Description**

In the constructor, two key styles are set via SetStyle:

- ControlStyles.AllPaintingInWmPaint: Concentrates all drawing in the WM_PAINT message, avoiding the two-step operation of first erasing the background and then drawing, reducing flicker.

- ControlStyles.Opaque: Declares the control as opaque, so the system skips background erasure, further reducing drawing overhead and preventing the OpenGL rendering result from being covered by the system background.

Although ControlStyles.UserPaint is set to true, no actual drawing is performed in OnPaint; the drawing work is completed externally. The LicenseManager.UsageMode check at the beginning of the constructor is used to return early at design time, avoiding initialization exceptions in the designer environment.

**Creating and Releasing the Drawing Context**

After the control is dragged onto the form, the drawing context must be created in the form's Load event and released in the FormClosing event to ensure correct application and reclamation of rendering resources:

csharp

IntPtr m_hContext = IntPtr.Zero;

private void Form1_Load(object sender, EventArgs e)

{

m_hContext = Render.CreateContext(ContextType.Generic, xguiPanel1.Handle, -1, -1);

}

private void Form1_FormClosing(object sender, FormClosingEventArgs e)

{

if (m_hContext != IntPtr.Zero)

{

Render.DestroyContext(m_hContext);

m_hContext = IntPtr.Zero;

}

}

The parameters of Render.CreateContext are as follows:

- ContextType.Generic: Context type. Generic indicates a general drawing context, suitable for general graphics and image drawing scenarios. The specific available types depend on the library definition and can be selected according to rendering requirements.

- xguiPanel1.Handle: Handle of the host window. After the drawing context is bound to this handle, the OpenGL rendering result will be output to the window area where the control is located. The handle must be obtained after the control is created, so the creation operation is placed in the Load event to ensure the control has completed initialization.

- -1, -1: Viewport width and height. Passing -1 means using the control's current width and height as the default viewport size, i.e., the drawing area matches the control size. To fix the viewport size, pass specific pixel values.

Render.CreateContext returns a context handle of type IntPtr, which must be saved as a member variable for subsequent drawing and release. If creation fails, the return value is usually IntPtr.Zero; a non-zero check should be performed before use.

**Releasing the Drawing Context**

When the form is closed, Render.DestroyContext(m_hContext) must be called to release the context; otherwise, graphics card resources will be occupied, causing memory or video memory leaks. After release, set the handle to IntPtr.Zero to avoid repeated release or dangling references. The release operation should be placed in the FormClosing event to ensure resource reclamation is completed before the form is destroyed.

## Geometric Shape Drawing

Geometric shape drawing is a basic operation in computer graphics and image processing. It refers to drawing basic primitives such as points, lines, arcs, polygons, and rectangles on a canvas or image at specified coordinates and with specified attributes through a graphics interface. These primitives can be used individually or combined into complex graphics such as arrows, cross markers, outline boxes, region boundaries, and coordinate systems. Each primitive is usually defined by geometric parameters: a point is determined by coordinates, a line by start and end points, an arc by center, radius, and start/end angles, a polygon by a vertex sequence, and a rectangle by position and width/height. During drawing, attributes such as color, line width, fill style, and transparency can be specified, and some interfaces also support anti-aliasing and line style settings. Geometric shape drawing is widely used in image annotation, algorithm result visualization, ROI definition, mask construction, and UI element drawing.

### 1. Drawing Lines

The example, under the premise that a drawing context has been created, demonstrates the drawing of three basic geometric primitives: a single line, multiple independent dots, and a polyline. Before drawing, the canvas is cleared and the background color is set. During drawing, a pen controls color and line width. Finally, the drawing result is submitted.

csharp

if (m_hContext == IntPtr.Zero) return;

GRgb lineColor = new GRgb(lblLineColor.BackColor.R, lblLineColor.BackColor.G, lblLineColor.BackColor.B);

GRgb fillColor = new GRgb(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

int lineWidth = 1;

int n = 1;

if (int.TryParse(tbxLineWidth.Text, out n))

{

lineWidth = Math.Max(1, Math.Min(7, n));

}

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

*// Set canvas back color to white for better viewing (optional)*

Render.SetCanvasBackStyle(m_hContext, 0);

GRgb color = new GRgb(255, 255, 255);

Render.SetBackgroundColor(m_hContext, color);

IntPtr hPen = Render.CreatePen(lineColor, lineWidth, (LinePattern)cmbLineType.SelectedIndex);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

*// Draw a single line*

Render.DrawLine(m_hContext, 30, 120, 100, 120, IntPtr.Zero);

*// Draw dots*

RvPoint\[\] pnDots = new RvPoint\[12\] {

new RvPoint(120, 220), new RvPoint(440, 220), new RvPoint(260, 220), new RvPoint(480, 220),

new RvPoint(120, 230), new RvPoint(440, 230), new RvPoint(460, 230), new RvPoint(480, 230),

new RvPoint(120, 240), new RvPoint(440, 240), new RvPoint(460, 240), new RvPoint(480, 240),

};

Render.DrawDots(m_hContext, pnDots, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(255, 0, 0));

Array.Resize(ref pnDots, 4);

pnDots\[0\].x += 30; pnDots\[0\].y += 40;

pnDots\[3\].x += 50; pnDots\[3\].y += 60;

Render.DrawPolyline(m_hContext, pnDots, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

Geometric primitive drawing is the basic function of the graphics and image drawing module. It generates points, lines, polylines, arcs, polygons, and other graphics on the canvas through coordinates and attribute descriptions. Unlike pixel operations that directly modify the buffer, geometric drawing generates pixels based on coordinate descriptions and a graphics interface, and the drawing result is rendered in real time by the graphics card. The drawing process usually includes: preparing a pen, selecting the pen, calling the drawing function, restoring the original pen, releasing the pen, and submitting the drawing. A pen determines the line color, width, and line style, and is a resource that must be created and selected before drawing.

**Drawing Process Description**

- **Clear the canvas**: Render.Clear(m_hContext, CanvasLayer.All) clears existing content on all layers to avoid superimposing new and old graphics. If historical drawing results need to be retained, this step can be omitted.

- **Set the background**: Render.SetCanvasBackStyle and Render.SetBackgroundColor set the canvas background to white, making dark lines easier to observe. Both are optional operations, and the background color can be adjusted according to actual needs.

- **Create and select a pen**: Render.CreatePen creates a pen handle based on color, line width, and line style. Render.SelectPen selects it into the context, and subsequent drawing uses this pen. SelectPen returns the old pen handle for restoration after drawing is complete.

- **Draw primitives**: Call DrawLine, DrawDots, and DrawPolyline in sequence to draw a line, a dot set, and a polyline.

- **Restore and release**: After drawing, SelectPen restores the old pen, and DestroyPen releases the newly created pen to avoid resource leaks.

- **Submit the drawing**: Render.Realize and Render.Flush submit the drawing commands to the graphics card and refresh the display, presenting the result to the window.

### 2. Drawing Rectangles

The example, under the premise that a drawing context has been created, demonstrates two ways to draw rectangles: one that draws only the border without filling, and another that uses a brush for filling. Before drawing, a pen and brush are created. After drawing, resources are restored and released, and finally the drawing result is submitted.

csharp

if (m_hContext == IntPtr.Zero) return;

GRgb lineColor = new GRgb(lblLineColor.BackColor.R, lblLineColor.BackColor.G, lblLineColor.BackColor.B);

GRgb fillColor = new GRgb(lblFillColor.BackColor.R, lblFillColor.BackColor.G, lblFillColor.BackColor.B);

int lineWidth = 1;

int n = 1;

if (int.TryParse(tbxLineWidth.Text, out n))

{

lineWidth = Math.Max(1, Math.Min(7, n));

}

int brushSize = 6;

if (int.TryParse(tbxBrushSize.Text, out n))

{

brushSize = Math.Max(1, Math.Min(15, n));

}

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

*// Set canvas back color to white for better viewing (optional)*

Render.SetCanvasBackStyle(m_hContext, 0);

GRgb color = new GRgb(255, 255, 255);

Render.SetBackgroundColor(m_hContext, color);

IntPtr hPen = Render.CreatePen(lineColor, lineWidth, (LinePattern)cmbLineType.SelectedIndex);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

*// Draw rect without filling anything*

Render.DrawRect(m_hContext, 30, 50, 100, 100, false, IntPtr.Zero);

IntPtr hBrush = Render.CreateBrush((BrushPattern)cmbHatchType.SelectedIndex, brushSize, fillColor);

IntPtr hOldBrush = Render.SelectBrush(m_hContext, hBrush);

*// Filling a rectangle*

Render.DrawRect(m_hContext, 30 + 100, 50, 100 + 100, true, IntPtr.Zero);

Render.SelectBrush(m_hContext, hOldBrush);

Render.DestroyBrush(hBrush);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

Rectangle drawing is one of the most commonly used forms of geometric primitives. The rectangle range is determined by specifying the top-left and bottom-right coordinates (or position and width/height), and you can choose to draw only the border or fill the interior at the same time. The border is controlled by a pen, which determines line color, width, and line style; filling is controlled by a brush, which determines fill color and fill pattern. The drawing process is similar to lines and polylines, but involves the creation, selection, restoration, and release of two types of resources: pens and brushes. Pay attention to paired management of the two resource types.

### 3. Drawing Arcs

The example, under the premise that a drawing context has been created and a pen is ready, demonstrates the drawing of arcs, circles, and ellipses. Circles and ellipses are each shown in both unfilled and filled forms, and the filling effect depends on a pre-created and selected brush.

csharp

*// Draw an arc*

Render.DrawArc(m_hContext, 320, 350, 50, 0, 135, IntPtr.Zero);

*// Draw an unfilled circle*

Render.DrawCircle(m_hContext, 120, 200, 35, false, IntPtr.Zero);

*// Draw an unfilled ellipse*

Render.DrawEllipse(m_hContext, 160, 380, 50, 80, 40, false, IntPtr.Zero);

*// After creating and selecting a brush, draw filled circles and ellipses*

IntPtr hBrush = Render.CreateBrush((BrushPattern)cmbHatchType.SelectedIndex, brushSize, fillColor);

IntPtr hOldBrush = Render.SelectBrush(m_hContext, hBrush);

Render.DrawCircle(m_hContext, 360, 200, 35, true, IntPtr.Zero);

Render.DrawEllipse(m_hContext, 260, 80, 50, 80, 72, true, IntPtr.Zero);

Arcs, circles, and ellipses are common curve primitives used to draw contours, mark regions, represent rotating objects, or construct masks. A circle can be regarded as a special case of an ellipse, while an ellipse describes an ellipse in any direction through major/minor semi-axes and a rotation angle. The filled version can draw a solid region for highlighting or generating a mask; the unfilled version draws only the contour for marking boundaries. Arcs are commonly used to represent angles, sector regions, or partial curves.

### 4. Drawing Polygons

The example, under the premise that a drawing context has been created and a pen is ready, demonstrates the drawing of triangles and arbitrary polygons. Triangles are drawn by DrawTriangle, and arbitrary polygons by DrawPolygon. Both show unfilled and filled forms. The filling effect depends on a pre-created and selected brush.

csharp

*// Draw an unfilled equilateral triangle*

Render.DrawTriangle(m_hContext, 220, 350, 90, 30, false, IntPtr.Zero);

*// Draw an unfilled polygon*

RvPoint\[\] pnPolyon = new RvPoint\[6\] {

new RvPoint(120, 50), new RvPoint(270, 50), new RvPoint(320, 100),

new RvPoint(270, 150), new RvPoint(120, 150), new RvPoint(170, 100)

};

Render.DrawPolygon(m_hContext, pnPolyon, false, IntPtr.Zero);

*// After creating and selecting a brush, draw filled triangles and polygons*

IntPtr hBrush = Render.CreateBrush((BrushPattern)cmbHatchType.SelectedIndex, brushSize, fillColor);

IntPtr hOldBrush = Render.SelectBrush(m_hContext, hBrush);

Render.DrawTriangle(m_hContext, 100, 350, 90, 30, true, IntPtr.Zero);

pnPolyon = new RvPoint\[6\] {

new RvPoint(120, 160), new RvPoint(270, 160), new RvPoint(320, 210),

new RvPoint(270, 260), new RvPoint(120, 260), new RvPoint(170, 210)

};

Render.DrawPolygon(m_hContext, pnPolyon, true, IntPtr.Zero);

Render.SelectBrush(m_hContext, hOldBrush);

Render.DestroyBrush(hBrush);

A triangle, as the simplest polygon, is often used to construct arrows, markers, and partitioned regions; an arbitrary polygon describes any shape through a vertex sequence and can fit complex target contours, mark irregular regions, or construct masks. Both support filled and unfilled modes: when unfilled, only the pen is used to draw the boundary; when filled, the brush is used to fill the interior. Compared with regular primitives such as rectangles and circles, the shape of a polygon is entirely determined by vertex coordinates, so it is more suitable for expressing arbitrary boundaries.

### 5. Drawing Composite Graphics

The example, under the premise that a drawing context has been created and a pen is ready, demonstrates the drawing of three composite graphics: a crosshair, independent line segments, and an RvBox2D rotated rectangle box.

csharp

*// Draw a crosshair*

Render.DrawCross(m_hContext, 220, 80, 70, 60, 45.0f, IntPtr.Zero);

*// Draw independent line segments: every two points form a line segment*

RvPoint\[\] pnSegments = new RvPoint\[4\] {

new RvPoint(170, 370), new RvPoint(220, 400),

new RvPoint(320, 370), new RvPoint(370, 400)

};

Render.DrawSegments(m_hContext, pnSegments, IntPtr.Zero);

*// Draw an RvBox2D rotated rectangle box*

RvBox2D box = new RvBox2D(230, 230, 150, 80, -50);

Render.DrawBox2D(m_hContext, box, IntPtr.Zero);

Composite graphics are drawing objects formed by combining basic primitives according to fixed rules, used to express markers or regions with specific semantics. A crosshair is commonly used to mark a center point, locate a target, or indicate coordinate system orientation; independent line segments are used to draw multiple disconnected line segments simultaneously, avoiding calling DrawLine one by one; RvBox2D represents a rectangle with a rotation angle, used to describe a minimum bounding rectangle, target bounding box, or directional detection region. These composite graphics are widely used in image annotation, target localization, measurement result visualization, and debugging display.

### Text Drawing

Text drawing is used to output strings to the canvas. It supports specifying font, font size, color, and font styles such as bold, italic, and underline. It can position text anywhere on the canvas and display it aligned horizontally or vertically within a specified rectangular area. The drawing result directly acts on the canvas and is suitable for scenarios such as image annotation, result description, UI prompts, and debugging information display.

The example, under the premise that a drawing context has been created, demonstrates drawing text according to user-set font, font size, color, and style. Text is drawn within a specified rectangular area and supports horizontal and vertical alignment, bold, italic, underline, strikeout, transparent, multiline, and word-wrap effects.

csharp

string strText = "This is Kingpool world";

string strFamilyName = cmbFamilyName.Text;

GRgb textColor = new GRgb(lblTextColor.BackColor.R, lblTextColor.BackColor.G, lblTextColor.BackColor.B);

GRgb backColor = new GRgb(lblTextBackColor.BackColor.R, lblTextBackColor.BackColor.G, lblTextBackColor.BackColor.B);

int fontHeight = 11;

int n = 11;

if (int.TryParse(cmbFontHeight.Text, out n))

{

fontHeight = n;

}

int x = int.Parse(txbTextAlignX.Text);

int y = int.Parse(txbTextAlignY.Text);

int w = int.Parse(txbTextAlignW.Text);

int h = int.Parse(txbTextAlignH.Text);

FontFlags fontflags = FontFlags.Default;

int format = 0;

int linespace = 0;

int.TryParse(tbxLineSpace.Text, out linespace);

GRect rect = new GRect(x, y, x + w, y + h);

if (ckbBold.Checked) { fontflags \|= FontFlags.Bold; }

if (ckbItalic.Checked) { fontflags \|= FontFlags.Italic; }

if (ckbUnderline.Checked) { fontflags \|= FontFlags.Underline; }

if (ckbStrikeOut.Checked) { fontflags \|= FontFlags.Strikeout; }

*// Create font*

IntPtr hFont = Render.CreateFontEx(strFamilyName, fontHeight, textColor, backColor, fontflags);

IntPtr hOld = Render.SelectFont(m_hContext, hFont);

if (cmbTextHorizonAlign.SelectedIndex == 0) { format \|= (int)TextAlign.Left; }

else if (cmbTextHorizonAlign.SelectedIndex == 1) { format \|= (int)TextAlign.Hcenter; }

else if (cmbTextHorizonAlign.SelectedIndex == 2) { format \|= (int)TextAlign.Right; }

if (cmbTextVerticalAlign.SelectedIndex == 0) { format \|= (int)TextAlign.Top; }

else if (cmbTextVerticalAlign.SelectedIndex == 1) { format \|= (int)TextAlign.Vcenter; }

else if (cmbTextVerticalAlign.SelectedIndex == 2) { format \|= (int)TextAlign.Bottom; }

float prevTransRatio = Render.GetTrasparence(m_hContext);

if (ckbTextTranparent.Checked)

{

format \|= Render.DT_TRANSPARENT;

Render.SetTrasparence(m_hContext, ((int)(nudTransRatio.Value)) / 100.0f);

}

if (ckbMultiLines.Checked)

{

format \|= Render.DT_MULTILINE;

string s = strText;

for (int i = 0; i \< 4; i++)

{

s += "\n";

s += strText;

}

s += "\t";

for (int i = 0; i \< 4; i++)

{

s += strText;

s += " ";

}

strText = s;

}

if (ckbWordBreak.Checked)

{

format \|= Render.DT_WORDBREAK;

}

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

*// Draw rectangle (optional)*

Render.DrawRect(m_hContext, x, y, x + w, y + h, false, IntPtr.Zero);

Render.DrawTextEx(m_hContext, strText, rect, format, linespace, IntPtr.Zero);

Render.SelectFont(m_hContext, hOld);

Render.DestroyFont(hFont);

*// Restore current transparency ratio (optional)*

if (ckbTextTranparent.Checked)

{

Render.SetTrasparence(m_hContext, prevTransRatio);

}

Text drawing is used to output strings on the canvas and is a basic function for image annotation, result description, UI prompts, and debugging information display. Unlike GDI+'s Graphics.DrawString, this module draws text through a hardware-accelerated interface. Font, color, style, and alignment can all be flexibly specified, and the drawing result directly enters the graphics card rendering pipeline, suitable for high-resolution canvases and real-time interaction scenarios. Text drawing depends on two key objects: a font determines glyph, size, color, and style; a rectangular area (GRect) determines the text layout range and alignment reference.

**Key Function Description**

- Render.CreateFontEx(familyName, height, textColor, backColor, flags): Creates a font. familyName is the font name, such as "Arial" or "SimSun"; height is the font height; textColor is the text color; backColor is the text background color (invalid in transparent mode); flags is a bitwise combination of the FontFlags enumeration, controlling bold, italic, underline, and strikeout.

- Render.SelectFont(context, font): Selects the font into the context and returns the old font handle.

- Render.DrawTextEx(context, text, rect, format, lineSpace, maskHandle): Draws text within the specified rectangular area. rect is the layout area; format is a bitwise combination of format flags; lineSpace is the line spacing; maskHandle is an optional mask; passing IntPtr.Zero means not used.

- Render.GetTrasparence(context), Render.SetTrasparence(context, ratio): Gets and sets the current transparency ratio, usually ranging from 0 to 1.

- Render.SelectFont(context, oldFont), Render.DestroyFont(font): Restores and releases font resources; creation and release must appear in pairs.

**Style and Format Flags**

- Font styles (FontFlags): Bold, Italic, Underline, Strikeout. Multiple styles are combined with bitwise OR (\|=) and can be enabled simultaneously.

- Horizontal alignment (TextAlign): Left, Hcenter, Right; determines the horizontal position of the text within the rectangular area.

- Vertical alignment (TextAlign): Top, Vcenter, Bottom; determines the vertical position of the text within the rectangular area.

- Transparent mode (DT_TRANSPARENT): When enabled, the text background is not filled and is directly overlaid on the canvas; transparency is controlled by SetTrasparence; a value of 1 is opaque, and 0 is fully transparent.

- Multiline mode (DT_MULTILINE): Allows the text to contain newline characters \n and draw line by line.

- Word wrap (DT_WORDBREAK): When a line of text exceeds the rectangle width, it automatically wraps to the next line.

**Rectangular Area and Alignment**

The text layout range is defined by GRect(x, y, x + w, y + h); the first two parameters are the top-left coordinates, and the last two are the bottom-right coordinates. The horizontal and vertical alignment flags determine where the text lands within this area: for example, setting both Hcenter and Vcenter centers the text within the rectangle. In the example, DrawRect is called first to draw the rectangle border, making it easy to visually check the text alignment; this step is optional.

**Constructing Multiline Text**

When ckbMultiLines is checked, multiline mode is enabled. Since the original text is short and may not be enough to trigger wrapping, the example generates sufficiently long text by concatenation: first copying the text 4 times separated by \n to force multiple lines; then appending a tab and several repeated text segments so that a single line exceeds the rectangle width, thereby observing the wrapping effect of DT_WORDBREAK. This construction is only for demonstration; in actual use, simply pass text containing newline characters or sufficiently long text.

**Key Points for Use**

The order of font creation, selection, restoration, and release must be strictly followed: first create, then select and save the old font, restore the old font after drawing, and finally release the new font. Transparent mode and global transparency interact: the example saves the current transparency prevTransRatio before drawing and restores it afterward, avoiding affecting other drawing operations. Both format and fontflags are bitwise-combined integers or enumerations and can be appended with \|=, but note that only one alignment flag of the same category should be set; repeated settings may cause undefined behavior. The backColor of CreateFontEx only takes effect in non-transparent mode; in transparent mode, the background is not filled. Coordinates and sizes are parsed from text boxes; ensure valid input before use to avoid format exceptions. The font name must be installed on the system; otherwise it may fall back to a default font. After drawing, Render.Realize and Render.Flush must be called to submit the result.

## Digital Image Drawing

Image drawing supports drawing digital images onto the canvas in various modes (such as stretch, tile, rotate, scale, flip, etc.). For images without an alpha channel, the drawing transparency can be uniformly controlled via a global transparency variable. This module exceeds most GUI libraries in the flexibility and functional coverage of image drawing.

The example, under the premise that a drawing context has been created, demonstrates how to draw an image onto the canvas and control transformations such as alignment, rotation, stretching, scaling, skewing, tiling, and flipping during drawing through different molds (Mold).

csharp

KImage im = new KImage("..\\samples\\waterdrop.png");

IntPtr hMold = IntPtr.Zero;

IntPtr hOld = IntPtr.Zero;

int x = int.Parse(tbxRegionX.Text);

int y = int.Parse(tbxRegionY.Text);

int w = int.Parse(tbxRegionW.Text);

int h = int.Parse(tbxRegionH.Text);

Render.SetTrasparence(m_hContext, ((int)(nudTransRatio.Value)) / 100.0f);

*// Remove alpha channel (if it exists) so the transparent ratio is applied properly,*

*// otherwise the original alpha channel will be used (Optional)*

im.Cast(PixelFormat.BGR);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

*// Draw rectangle (optional)*

Render.DrawRect(m_hContext, x, y, x + w, y + h, false, IntPtr.Zero);

switch (cmbMoldType.SelectedIndex)

{

case 1: *// Align mold*

hMold = Render.CreateAlignMold(cmbAlignHori.SelectedIndex + 1, cmbAlignVert.SelectedIndex + 1);

break;

case 2:

hMold = Render.CreateRotateMold(float.Parse(tbxRotateAngle.Text), ckbRotateKeepSize.Checked);

break;

case 3:

hMold = Render.CreateStretchMold(ckbStretchKeepRatio.Checked, ckbStretchCentered.Checked);

break;

case 4:

hMold = Render.CreateScaleMold(float.Parse(tbxScaleValue.Text), float.Parse(tbxScaleValue.Text), ckbScaleCentered.Checked);

break;

case 5:

RvPoint\[\] vtx = new RvPoint\[4\];

vtx\[0\].x = 15; vtx\[0\].y = 30;

vtx\[1\].x = 15 + 300; vtx\[1\].y = 30 + 50;

vtx\[2\].x = 15 + 300; vtx\[2\].y = 30 + 350;

vtx\[3\].x = 15 + 10; vtx\[3\].y = 30 + 320;

hMold = Render.CreateSkewMold(vtx, ckbKeepSize.Checked);

break;

case 6:

TileType type = TileType.None;

if (cbxHori.Checked && cbxVert.Checked) type = TileType.Both;

else if (cbxHori.Checked) type = TileType.Horizontal;

else if (cbxVert.Checked) type = TileType.Vertical;

hMold = Render.CreateTileMold(int.Parse(tbxTileRows.Text), int.Parse(tbxTileCols.Text), (int)type);

break;

case 7:

FlipType type = FlipType.Never;

if (cbxHori.Checked && cbxVert.Checked) type = FlipType.Both;

else if (cbxHori.Checked) type = FlipType.Horizontal;

else if (cbxVert.Checked) type = FlipType.Vertical;

hMold = Render.CreateFlipMold((int)type);

break;

default:

hMold = Render.CreateMold(MoldType.Default, 0, 0);

break;

}

hOld = Render.SelectMold(m_hContext, hMold);

Render.DrawImageEx(m_hContext, im.GetHandle(), x, y, w, h, IntPtr.Zero);

Render.SelectMold(m_hContext, hOld);

Render.DestroyMold(hMold);

A mold is a transformation object in the drawing module that controls how an image is mapped to a target region. Unlike performing geometric transformations directly on image data, a mold acts during the drawing stage and applies the transformation in real time when the image is submitted to the canvas, so it does not modify the original image data and can be switched or combined at any time. The mold mechanism decouples "image content" from "drawing method": the same image can be drawn as aligned, rotated, stretched, scaled, skewed, tiled, or flipped results through different molds. It is commonly used in image layout, image adaptation within annotation boxes, texture tiling, mirror display, and perspective simulation.

**Key Function Description**

- Render.SetTrasparence(context, ratio): Sets drawing transparency, ranging from 0 to 1. In the example, it is obtained by dividing the value of the numeric control nudTransRatio by 100.

- Render.CreateAlignMold(horizontal, vertical): Creates an alignment mold. horizontal and vertical are horizontal and vertical alignment, respectively; in the example, they are obtained by adding 1 to the combo box index. The alignment mold controls the alignment position of the image within the target rectangle without changing its size, suitable for scenarios where the original image ratio is maintained and the landing point is adjusted.

- Render.CreateRotateMold(angle, bKeepSize): Creates a rotation mold. angle is the rotation angle (in degrees); bKeepSize indicates whether to keep the original image size. After rotation, parts exceeding the target region may be clipped; keeping the size avoids overall scaling.

- Render.CreateStretchMold(bKeepRatio, bCentered): Creates a stretch mold. Stretches the image to fill the target rectangle. When bKeepRatio is true, the aspect ratio is maintained (possibly leaving blank space); when false, it is fully stretched to fill. bCentered controls whether it is centered in the target region.

- Render.CreateScaleMold(scaleX, scaleY, bCentered): Creates a scaling mold. scaleX and scaleY are horizontal and vertical scaling factors, respectively; in the example, they are the same, indicating proportional scaling. bCentered controls whether it is centered after scaling.

- Render.CreateSkewMold(vertices, bKeepSize): Creates a skew mold. vertices are four vertex coordinates defining the mapping positions of the four corners of the image on the canvas. A non-rectangular arrangement of the four points achieves skew or approximate perspective effects. bKeepSize indicates whether to keep the original size.

- Render.CreateTileMold(rows, cols, tileType): Creates a tile mold. rows and cols are the number of rows and columns, respectively. tileType is specified by the TileType enumeration: Horizontal, Vertical, Both, None. The tile mold repeats the image to fill the target region.

- Render.CreateFlipMold(flipType): Creates a flip mold. flipType is specified by the FlipType enumeration: Horizontal, Vertical, Both, Never. The flip mold mirrors the image during drawing without modifying the original data.

- Render.CreateMold(type, param1, param2): Creates a general mold. type is specified by the MoldType enumeration; in the example, Default is passed, indicating no transformation and drawing in the original way.

- Render.SelectMold(context, mold): Selects the mold into the context and returns the old mold handle. After drawing, the old mold must be restored.

- Render.DrawImageEx(context, imageHandle, x, y, w, h, maskHandle): Draws the image to the specified rectangular area of the canvas. x, y, w, h define the target region; maskHandle is an optional mask; passing IntPtr.Zero means not used. The currently selected mold is automatically applied to the image during drawing.

- Render.DestroyMold(mold): Releases mold resources; creation and release must appear in pairs.

**Mold Type Comparison**

| Mold | Key Parameters | Typical Use |
|:---|----|----|
| Align | Horizontal, vertical alignment | Position adjustment while maintaining ratio |
| Rotate | Angle, whether to keep size | Rotated display, tilted annotation |
| Stretch | Keep ratio, centered | Fill target region, fit box |
| Scale | Scale factors, centered | Proportional or non-proportional scaling |
| Skew | Four vertex coordinates | Skew, approximate perspective |
| Tile | Rows, columns, direction | Texture tiling, background fill |
| Flip | Flip direction | Mirror display, orientation correction |
| Default | None | Draw as-is |

The order of mold creation, selection, restoration, and release must be strictly followed: first create, then select and save the old mold, restore the old mold after drawing, and finally release the new mold. Only one mold can be effective in a context at a time, so when drawing images with different transformation effects, molds must be created and switched separately.

## Stroke-Related Operations

In the graphics, image, and text drawing processes described above, each drawing function returns a stroke handle, and these functions also allow an existing stroke handle to be specified as a parameter. The core value of this design is that a stroke handle represents a drawing object that has already been described, encapsulating all drawing information such as primitive type, coordinates, color, line width, and fill style. Once created, the object can be reused repeatedly without reconstructing drawing parameters each time or recalculating coordinates and setting states.

Through stroke handles, developers can individually control the drawing timing and number of times for a primitive, for example, redrawing only changed primitives in an interactive interface while keeping other primitives unchanged; or repeatedly submitting the same stroke object to different positions or layers to achieve batch drawing. This mechanism separates "primitive description" from "primitive drawing", avoiding repeated construction overhead and state switching, and can significantly reduce drawing time and improve real-time responsiveness when handling complex pictures containing a large number of primitives.

This section focuses on stroke-related operations, including stroke creation, selection, drawing, modification, copying, release, and how to achieve independent control and efficient reuse of primitives through stroke handles.

## Drawing a Dummy

Calling Render.DrawDummy creates a dummy stroke that does not perform any actual drawing and returns its handle:

csharp

m_hCurStroke = Render.DrawDummy(m_hContext, IntPtr.Zero);

The handle itself contains no visible primitive but can serve as a general stroke container for use by subsequent primitive drawing operations.

## Drawing an Image and Returning a Stroke Handle

After loading an image, use Render.DrawImageE2 to draw the image in stretch mode to the specified rectangular area, and obtain the stroke handle corresponding to this drawing for subsequent operations.

csharp

KImage im = new KImage("..\\samples\\lenna.png");

GRect rect = new GRect(30, 30, 150, 120);

m_hPictureStroke = Render.DrawImageE2(m_hContext, im.GetHandle(), rect, IntPtr.Zero);

The returned m_hPictureStroke represents this image drawing operation. Through this handle, this primitive can later be individually selected, modified, redrawn, or released without reconstructing drawing parameters. Saving the handle as a member variable enables independent control of the image primitive, such as moving, scaling, or deleting the image during interaction without affecting other primitives.

## Drawing Text and Returning a Stroke Handle

Draw a non-filled rectangle at the specified position and obtain the stroke handle corresponding to this drawing for subsequent operations.

csharp

int left = 160;

int top = 140;

int width = 100;

int height = 140;

m_hRectangleStroke = Render.DrawRect(m_hContext, left, top, left + width, top + height, false, IntPtr.Zero);

## Modifying an Image Drawing Stroke

Using the previously saved image drawing stroke handle, Render.Modify replaces the existing image drawing result on the canvas with new image content, without recreating the stroke or resubmitting drawing parameters.

csharp

if (m_hCurStroke == m_hPictureStroke)

{

KImage im = new KImage("..\\samples\\waterdrop.png");

NativeArg arg = new NativeArg(im.Handle);

Render.Modify(m_hContext, m_hCurStroke, ModifyType.Image, arg.Ptr);

}

In the stroke model, the stroke handle returned by each drawing function not only identifies a drawing operation but also holds the parameters and resources used for that drawing. Render.Modify allows directly modifying the content attributes of an existing stroke without recreating the stroke. This approach separates "primitive creation" from "primitive update": once a primitive is created, its position, size, style, and other framework remain unchanged, and only the internal data is replaced. In the example, the image originally drawn by DrawImageE2 is replaced with waterdrop.png, while the drawing position, rectangular area, stretch mode, etc. are all determined by the original stroke and need not be respecified.

The check if (m_hCurStroke == m_hPictureStroke) determines whether the currently selected stroke is exactly the previously saved image stroke. m_hCurStroke usually represents the target stroke of the current operation and may change with user interaction; m_hPictureStroke is the handle saved when the image was created. Only when the two are consistent does the modification act on that image primitive; otherwise it should be skipped or changed to modify another target. This check ensures the accuracy of the modification and avoids mistakenly modifying unrelated primitives.

Directly calling DrawImageE2 again would create a new stroke, the original stroke would still exist, two drawings might appear on the canvas, and coordinates, rectangle, and other parameters would need to be respecified. Using Modify, however, reuses the original stroke and only replaces its content; the drawing position, size, transformation method, etc. are all preserved, and no extra primitive is generated. This is particularly efficient in scenarios requiring frequent updates of image content (such as video frame refresh, real-time preview, or switching displayed images), avoiding the overhead of repeatedly creating and destroying strokes.

## Reusing a Drawing Stroke

Pass the previously created dummy stroke handle as a parameter to Render.DrawText, so that text drawing reuses this stroke container. After the function executes, the passed handle and the returned handle are exactly the same, but the drawing definition encapsulated inside the handle has been replaced from its previous content (dummy or image) with a new text drawing definition.

csharp

if (IntPtr.Zero != m_hCurStroke)

{

Render.DrawText(m_hContext, "This is Kingpool world", 13, 13, m_hCurStroke);

tbxStrokePageOut.Text = \$"The Stroke was a picture drawing, and its content is a text drawing now";

}

In the stroke model, the last parameter of a drawing function is usually an optional stroke handle, used to specify the storage target for the drawing result. If IntPtr.Zero is passed, the function internally creates a new stroke and returns its handle; if a valid stroke handle is passed, the function directly reuses that handle and writes the content of this drawing into it. During reuse, the handle itself is not replaced, i.e., the return value equals the passed value, but the drawing definition encapsulated inside the handle is updated to the new primitive. This mechanism allows a stroke handle to be reused repeatedly, avoiding creating a new object for each drawing. A dummy stroke is one such special container: it is created by DrawDummy, contains no visible primitive, and holds no heavyweight resources such as images or fonts, so using it as a general container has extremely low release overhead and is more efficient.

Each stroke handle internally maintains the data structures needed for drawing. If the handle already has content (such as an image or graphic), reusing it requires first releasing the original resources before writing new content; a dummy stroke has empty content, so reuse requires no release of any substantial resources and only needs to write new drawing information, resulting in minimal overhead. Theoretically, any valid stroke handle can be passed as a container, but if the handle previously held resources such as images or fonts, reuse triggers resource release and replacement, which is less efficient than a dummy stroke. Therefore, in scenarios requiring frequent reuse of the same container, prefer using a dummy stroke as the initial container.

## Deleting or Hiding a Stroke

Use Render.Erase to delete the specified stroke and set all variables referencing that stroke to IntPtr.Zero to prevent subsequent misuse of the released handle.

csharp

Render.Erase(m_hContext, m_hCurStroke, EraseType.Delete);

if (m_hCurStroke == m_hPictureStroke) { m_hPictureStroke = IntPtr.Zero; }

if (m_hCurStroke == m_hRectangleStroke) { m_hRectangleStroke = IntPtr.Zero; }

m_hCurStroke = IntPtr.Zero;

In the stroke model, the stroke handle returned by each drawing function holds the corresponding drawing resources and internal definition. When a primitive is no longer needed, it can be deleted from the canvas and its resources released via Render.Erase. However, deletion is not the only way: calling Render.Clear can clear the entire canvas or a specified drawing layer on the canvas, and all strokes on that layer are cleared together, without needing to call Render.Erase one by one. The difference between the two is: Clear targets the canvas or a drawing layer and removes primitives in batches by range, suitable for overall reset or layer-by-layer cleanup; Erase targets a single stroke and precisely removes a specified primitive, suitable for local deletion. In actual use, choose according to the need: if all content or a certain layer needs to be cleared, use Clear directly; if only one primitive needs to be deleted while other content is retained, use Erase.

Use Render.Hide to control the visibility of a specified stroke, hiding or redisplaying it, while the stroke itself and its resources remain unchanged.

csharp

Render.Hide(m_hContext, m_hCurStroke, true);

In the stroke model, the stroke handle returned by a drawing function can not only be used to modify content or delete, but also to control visibility. Render.Hide provides a non-destructive display control method: when a stroke is hidden, it disappears from the canvas, but its internal definition, resources, and handle remain unchanged; when redisplayed, the stroke returns to its original state without needing to be recreated or redrawn. This is fundamentally different from the deletion operation of Render.Erase: deletion releases stroke resources and invalidates the handle, while hiding only changes the visible state; the handle remains valid and can be toggled at any time. Hide is suitable for scenarios requiring frequent visibility toggling, such as layer switches, animation blinking, and interactive show/hide; Erase is suitable for scenarios where a primitive is no longer needed at all. If you hide first and then delete, the two operations do not affect each other: a hidden stroke can still be deleted via Erase.

## Combining and Splitting Strokes

Combine two saved stroke handles into one composite stroke, and replace the current stroke with the combined handle. The original two independent handles then become invalid and are set to zero.

csharp

if (m_hPictureStroke == IntPtr.Zero \|\| m_hRectangleStroke == IntPtr.Zero) return;

IntPtr\[\] arr = new IntPtr\[2\] { m_hPictureStroke, m_hRectangleStroke };

m_hMergeStroke = Render.Combine(m_hContext, arr);

Pool.Assert(m_hMergeStroke != IntPtr.Zero);

m_hPictureStroke = IntPtr.Zero;

m_hRectangleStroke = IntPtr.Zero;

m_hCurStroke = m_hMergeStroke;

In the stroke model, the stroke handle returned by each drawing function represents an independent primitive. When multiple primitives need to be managed as a whole, Render.Combine can be called to combine them into a composite stroke. After combination, the previously separate primitives are uniformly represented by a new handle, and subsequent operations such as selection, modification, hiding, and deletion on this handle are equivalent to acting on all combined primitives simultaneously. This simplifies the management logic of multiple primitives, reduces the number of handles, and is particularly efficient when overall transformation or batch control is needed, such as combining an image and a border to move, rotate, or hide them together.

The combine operation incorporates the content of the original strokes into the new composite stroke, and the original handles usually become invalid immediately and no longer represent any valid primitive. In the example, after successful combination, m_hPictureStroke and m_hRectangleStroke are immediately set to IntPtr.Zero, explicitly indicating that these two handles are no longer usable. Continuing to retain or use them may cause dangling references, repeated release, or drawing exceptions. Then the current operation target m_hCurStroke is updated to the combined m_hMergeStroke, so that subsequent interactions act on the entire composite primitive.

The previously combined composite stroke can be split back into two independent strokes, and the original composite handle is released.

csharp

if (m_hMergeStroke == IntPtr.Zero) return;

IntPtr\[\] subarr = Render.Uncombine(m_hContext, m_hMergeStroke, 2);

m_hMergeStroke = IntPtr.Zero;

After splitting, the sub-strokes can be individually selected, modified, hidden, deleted, etc. After splitting, each sub-stroke regains its independent identity and does not affect the others, facilitating local adjustment or separate management. In the example, the composite stroke is formed by combining an image and a rectangle, so after splitting, two sub-handles should be obtained, corresponding to the original image primitive and rectangle primitive, respectively.

The split operation separates each sub-primitive from the composite stroke, and the original composite handle becomes invalid immediately and no longer represents any valid primitive. In the example, after successful splitting, m_hMergeStroke is immediately set to IntPtr.Zero, explicitly indicating that the handle is no longer usable. Continuing to use it may cause dangling references or repeated release. The sub-handles obtained from splitting should be saved to the corresponding variables for subsequent independent operations; if no longer needed, they should be deleted one by one via Render.Erase or the references set to zero.

## Canvas-Related Operations

Strokes are usually drawn on a canvas, and the canvas size is not necessarily the same as the window size. In machine vision applications, the canvas is generally set to match the camera resolution, often larger than the window, so the occluded content can be viewed by moving the canvas or zooming out. The canvas consists of four layers, from bottom to top: the image frame layer, the canvas layer, the middle layer, and the screen layer. The image frame layer stores image data consistent with the current canvas format; the canvas layer and middle layer have the same size as the canvas and support zoom display, and the drawing content of the canvas layer can be exported (available for enhanced contexts); the screen layer is based on window coordinates, does not scale with the canvas, and cannot be zoomed. During display, layers are drawn in the order of image frame layer, canvas layer, middle layer, and screen layer, with later-drawn layers covering earlier ones.

**Setting the Canvas Format and Drawing an Image Frame**

The example first loads an image, then adjusts the canvas size and pixel depth to match the image, feeds the image into the image frame layer, and finally submits and refreshes the display. If the image format does not match the canvas format, FeedFrame will fail, so adjusting the canvas format is a necessary prerequisite.

csharp

KImage im = new KImage("..\\samples\\Earth.png");

*// Before feeding image, the format of canvas should be*

*// adjusted as the image (optional)*

Render.SetCanvasSize(m_hContext, im.GetWidth(), im.GetHeight());

Render.SetCanvasDepth(m_hContext, im.GetDepth());

Render.FeedFrame(m_hContext, im.GetHandle());

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

The canvas is the carrier container for strokes, and its size and pixel format must match the image to be drawn; otherwise, the image cannot be correctly fed into the image frame layer. The image frame layer is at the bottom of the canvas and stores image data consistent with the current canvas format, equivalent to the background base image of the canvas. Unlike drawing an image as a stroke via DrawImageEx, FeedFrame directly writes image data into the image frame layer as the underlying content of the entire canvas, and subsequently drawn strokes are overlaid on top of it. This method is suitable for quickly feeding camera-acquired images, source images to be processed, or images that need to be displayed as a background into the canvas.

The image frame layer stores data consistent with the canvas format, so the fed image must match the canvas format. Pixel formats include BGR, BGRA, GRAY, BIN, etc., and the depth is usually 8 bits. If the image is BGRA (with an alpha channel) while the canvas is BGR, or the depths are inconsistent, FeedFrame may return failure or raise an exception. In the example, the canvas size and depth are set first precisely to ensure format matching.

FeedFrame writes the image into the image frame layer as the bottommost background of the canvas, usually used to load a source image or camera frame; DrawImageEx draws the image as a stroke on the canvas layer, can overlay multiple images, and supports mold transformation and transparency control. The former is suitable for a single background image, and the latter for multi-layer composition. The image frame layer does not participate in stroke management and cannot be controlled through stroke handles; image strokes on the canvas layer can be modified, hidden, or deleted through handles.

## Drawing Text on the Image Frame Layer

The example uses Render.TextOut to draw text directly on the image frame layer, uses Render.MoveTo to move the current position, and outputs a second text segment with font and pen settings. This method directly modifies the image frame data and does not return a stroke handle; the drawing result cannot be undone or hidden.

csharp

if (m_hContext == IntPtr.Zero) return;

Render.TextOut(m_hContext, "This is Kingpool World");

*// Move current position to new coordinates*

Render.MoveTo(m_hContext, 12, 36);

IntPtr hFont = Render.CreateFont("Arial", 12, 0);

IntPtr hOldFont = Render.SelectFont(m_hContext, hFont);

IntPtr hPen = Render.CreatePen(new GRgb(255, 0, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.TextOut(m_hContext, "This is Kingpool World");

Render.SelectFont(m_hContext, hOldFont);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyFont(hFont);

The first TextOut call did not select a custom font and pen before calling, so it uses the default font and default color, and the output position is the canvas origin or the last cursor position. Then MoveTo(12, 36) moves the output position to (12, 36), and an Arial 12-point font and a red pen are created and selected. The second TextOut uses the new settings and outputs text in red at the specified position. The comparison of the two text segments shows the effect of font, color, and position settings on TextOut.

TextOut is an immediate text output function that directly draws text into the data buffer of the image frame layer. Unlike DrawText or DrawTextEx, which return a stroke handle and can be subsequently modified or hidden, TextOut writes the drawing result directly into the image frame pixel data, equivalent to "burning" text onto the base image. This method does not produce a stroke object and cannot be undone, hidden, or deleted through a handle; once drawn, it becomes part of the image data. Its advantage is simple calling and no need to manage stroke resources, suitable for one-time output of fixed text, debugging information, or annotations that do not need subsequent modification.

TextOut directly modifies the image frame layer data and does not return a stroke handle, so it cannot be undone, hidden, or modified through functions such as Erase, Hide, or Modify. If these capabilities are needed, use DrawText or DrawTextEx instead, which return stroke handles that can be subsequently managed. Since the drawing result is directly burned into the image frame data, if Render.Clear clears the image frame layer, the text is cleared along with it; but if only other layers are cleared, the text in the image frame layer remains. Before drawing, ensure that an appropriate font and pen have been selected; otherwise, default settings will be used, which may cause unexpected color or glyphs.

## Drawing an Image on the Image Frame Layer

The example loads an image and converts it to BGR format, then uses Render.PaintImage to draw the image directly to the specified position on the image frame layer. The drawing result cannot be undone and no stroke handle is returned.

csharp

KImage im = new KImage("..\\samples\\lenna.png");

im.Cast(PixelFormat.BGR);

Render.PaintImage(m_hContext, im.GetHandle(), 10, 10);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

PaintImage is an immediate image output function that directly draws image data to the specified position of the image frame layer (the underlying image data buffer). Unlike DrawImageEx or DrawImageE2, which return a stroke handle and can be modified, hidden, or deleted through the handle, PaintImage writes the drawing result directly into the image frame pixel data, equivalent to "burning" the image onto the base image. This method does not produce a stroke object and cannot be undone, hidden, or deleted through a handle; once drawing is complete, it becomes part of the image data. Its advantage is simple calling and no need to manage stroke resources, suitable for one-time drawing of fixed images, use as a background texture, or compositing operations that do not require subsequent modification.

**Difference from FeedFrame**

FeedFrame feeds an image into the image frame layer, usually covering the entire canvas according to the canvas size, as the background base image of the entire canvas; PaintImage draws the image to specified coordinates on the image frame layer, and can overlay multiple images on the same frame layer or composite with existing content. The former is suitable for setting a single background, and the latter for positioning and drawing multiple images on the base image.

**Difference from DrawImageEx**

DrawImageEx draws the image as a stroke on the canvas layer, returns a stroke handle, and supports mold transformation, transparency control, and subsequent modification, hiding, and deletion; PaintImage directly writes into the image frame layer, does not return a handle, and cannot be individually controlled after drawing. If interactive adjustment of the image or unified management with other strokes is needed, use DrawImageEx; if only one-time compositing onto the base image is needed, PaintImage is more direct.

**Format Matching**

The image format should match the canvas format; otherwise drawing may fail or colors may be abnormal. In the example, the image is first converted to BGR to ensure it matches the canvas format. If the canvas uses BGRA or GRAY format, the image should be converted accordingly. In addition, the image size and target position should be within the canvas range; the excess will be clipped.

PaintImage directly modifies the image frame layer data and does not return a stroke handle, so it cannot be undone, hidden, or modified through functions such as Erase, Hide, or Modify. If these capabilities are needed, use DrawImageEx or DrawImageE2 instead. The drawing result is directly burned into the image frame data; if Render.Clear clears the image frame layer, the drawn image is cleared along with it; but if only other layers are cleared, the content of the image frame layer remains. Before drawing, ensure the image format matches the canvas format and confirm that the drawing coordinates are within the canvas range to avoid failure or clipping. Submitting and refreshing requires calling Render.Realize and Render.Flush.

## Clearing the Image Frame and Strokes

The example calls Render.Clear twice to clear the image frame layer and all strokes, respectively, then submits and refreshes, restoring the canvas to a blank state displayed with the background color.

csharp

if (m_hContext == IntPtr.Zero) return;

*// Clear current frame*

Render.Clear(m_hContext, CanvasLayer.Background);

*// Remove all strokes*

Render.Clear(m_hContext, CanvasLayer.Foreground);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

The canvas consists of multiple layers, and Render.Clear can clear a specified layer rather than only the entire canvas. In the example, the image frame layer and the stroke layer are cleared separately: the former removes the image data serving as the base image, and the latter removes all drawn strokes. The two calls do not interfere with each other, and either can be executed alone to retain the other layer's content. This layer-by-layer clearing provides flexible control for canvas reset: if only the base image needs to be replaced while annotations are retained, only the background layer needs to be cleared; if only annotations need to be deleted while the base image is retained, only the foreground layer needs to be cleared.

**Display of Background Color After Clearing**

After the image frame layer is cleared, that layer no longer contains image data, and the background display of the canvas is determined by the background color set by SetBackgroundColor. If SetBackgroundColor has been called previously, the canvas presents that color after clearing; if not set, the library's default background color is used (usually black or white, depending on the context type and initialization parameters). Therefore, the background color is both the fill color at canvas initialization and the fallback display color after the image frame layer is emptied. If a specific color needs to be displayed after clearing, call SetBackgroundColor before or after clearing.

**Relationship Between Clearing and Handles**

Clear acts on layers rather than individual strokes, so it does not return information about the cleared handles. After clearing the foreground layer, all previously saved stroke handles become dangling handles, and the developer must set the corresponding variables to IntPtr.Zero; otherwise, subsequent use of these handles may cause exceptions. The example does not involve specific handle variables. In actual projects, if a list of stroke handles is maintained, they should be zeroed one by one after clearing.

The Clear operation is irreversible; the cleared content cannot be recovered. If it needs to be retained, export or back it up before clearing. Clearing the foreground layer invalidates stroke handles, and the related variables must be set to zero promptly to avoid dangling references. After clearing, Render.Realize and Render.Flush must be called to submit and refresh; only then will the canvas reflect the clearing result. If a specific background color is desired after clearing, ensure that the color has been set via SetBackgroundColor; otherwise, the default background color will be used.

## Exporting the Canvas

The example selects a target path through a save file dialog, then exports the canvas content as a KImage object and saves it to disk.

csharp

saveFileDialog1.Filter = "jpg file\|\*.jpg";

if (saveFileDialog1.ShowDialog() == DialogResult.OK)

{

if (m_hContext == IntPtr.Zero) return;

KImage im = new KImage();

Render.ExportCanvas(m_hContext, im.GetHandle());

im.Save(saveFileDialog1.FileName);

}

The canvas is the intermediate carrier of the drawing process, and its content usually needs to be exported as an image file for archiving, sharing, or subsequent processing. Render.ExportCanvas composites all currently visible layers of the canvas and outputs them to the specified KImage object, which can then be saved as a common image format via KImage.Save. What is exported is the final rendering result of the canvas, including the base image of the image frame layer and all strokes on the canvas layer, but not the screen layer (the screen layer is based on window coordinates and does not participate in export). Therefore, the exported image is a snapshot of what the user sees.

The canvas is the intermediate carrier of the drawing process, and its content usually needs to be exported as an image file for archiving, sharing, or subsequent processing. Render.ExportCanvas outputs the canvas content to the specified KImage object, which can then be saved as a common image format via KImage.Save. The exported content depends on the type used when the drawing context was created; not all layers are exported.

**Relationship Between Exported Content and Context Type**

- **Generic context**: Exports only the content of the image frame layer, i.e., the image data serving as the base image. Strokes on the canvas layer are not included in the export result.

- **Enhanced context**: Exports the drawing content of the image frame layer and the canvas layer, i.e., the composite result of the base image and all strokes. The middle layer and screen layer do not participate in export.

Therefore, if the exported image is expected to include drawn strokes, an Enhanced context must be used; if only the base image needs to be exported, a Generic context is sufficient. The middle layer and screen layer are not exported regardless of the context type.

## Canvas Zoom Display and Movement

Select the display mode through the combo box and apply it to the canvas by calling Render.SetViewMode:

csharp

if (cmbViewMode.SelectedIndex == 1)

{

Render.SetViewMode(m_hContext, ViewMode.Center);

}

else if (cmbViewMode.SelectedIndex == 2)

{

*// ...*

}

The third parameter of SetViewPos being false indicates horizontal offset, and true indicates vertical offset. The offset should be within a valid range; the maximum horizontal offset is the canvas width minus the window width. If the display mode is Stretch or Zoom, the canvas has been compressed into the window, and there is no excess part, so SetViewPos has no actual effect.

**Display Zoom**

In addition to controlling the overall fit mode through the display mode, you can also directly specify the zoom ratio via SetViewScaleEx to zoom in or out the canvas content:

csharp

float scale = tkbScale.Value / 10.0f;

Render.SetViewScaleEx(m_hContext, scale, scale);

Unlike modes such as ViewMode.Zoom that automatically fit the window, SetViewScaleEx provides manual precise control and can specify any zoom factor. This function only takes effect when the display mode is Default or Center, because these two modes do not perform scaling themselves, leaving room for manual adjustment; Stretch and Zoom modes are automatically scaled by the system, and manual settings have no effect. After scaling, if the canvas is still larger than the window, you can continue to adjust the display position via SetViewPos. The two together can achieve zoom and pan browsing similar to an image editor.

The display mode should be set after the drawing context is created. After switching modes, adjusting position, or changing the zoom ratio, Render.Realize and Render.Flush must be called to submit and refresh before the window reflects the new display effect. Display mode, display position, and display zoom only affect the window presentation, not the canvas data, stroke coordinates, or export result. ExportCanvas always exports the complete canvas content, regardless of the window display.

## Other Related Operations

In addition to drawing functions, this module provides the following auxiliary functions: setting the display position of the RVB LOGO or hiding it; modifying the default color and style of the canvas; and obtaining the current OpenGL version. These functions are used for UI customization, canvas appearance adjustment, and runtime environment query, respectively.

## LOGO Display

The example selects the LOGO display position or hides it through a combo box, and calls Render.SetRvbLogoPlacement and Render.SetRvbLogoVisible to complete the setting, finally triggering a refresh to apply the effect.

csharp

if (m_hContext == IntPtr.Zero) return;

int idx = cmbLogoPlacement.SelectedIndex;

if (idx == 0)

{

Render.SetRvbLogoPlacement(m_hContext, LogoPlacement.LeftTop);

Render.SetRvbLogoVisible(m_hContext, true);

btnRefresh.PerformClick();

}

else if (idx == 1)

{

Render.SetRvbLogoPlacement(m_hContext, LogoPlacement.RightTop);

Render.SetRvbLogoVisible(m_hContext, true);

btnRefresh.PerformClick();

}

else if (idx == 2)

{

Render.SetRvbLogoPlacement(m_hContext, LogoPlacement.LeftBottom);

Render.SetRvbLogoVisible(m_hContext, true);

btnRefresh.PerformClick();

}

else if (idx == 3)

{

Render.SetRvbLogoPlacement(m_hContext, LogoPlacement.RightBottom);

Render.SetRvbLogoVisible(m_hContext, true);

btnRefresh.PerformClick();

}

else if (idx == 4)

{

Render.SetRvbLogoVisible(m_hContext, false);

btnRefresh.PerformClick();

}

The RVB LOGO is an identification watermark displayed by the drawing module in the window, usually appearing above the canvas and near the screen layer, used to identify the software source. This LOGO is independent of the canvas content, does not affect the drawing result or exported content, and exists only as a UI element. Through the relevant interfaces, its display position can be set or it can be hidden, so as to adjust the interface appearance in different scenarios: display it in one of the four corners when a brand identity is needed, or hide it when a completely clean screen is needed.

## Default Canvas Display Style

The example first clears the canvas, then adjusts the canvas size and display mode, and then sets the canvas background style, canvas background color, and the color of the blank window area, respectively.

csharp

if (m_hContext == IntPtr.Zero) return;

GRgb color = new GRgb(255);

*// Clear all for a tidy canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

*// Resize the canvas and set the view mode to zoom*

*// so that we can see the blank color and the background color*

*// at the same time (optional)*

Render.SetCanvasSize(m_hContext, 800, 600);

Render.SetViewMode(m_hContext, ViewMode.Zoom);

Render.SetCanvasBackStyle(m_hContext, cmbBaskStyle.SelectedIndex);

color.red = lblBlankColor.BackColor.R;

color.green = lblBlankColor.BackColor.G;

color.blue = lblBlankColor.BackColor.B;

Render.SetBlankColor(m_hContext, color);

color.red = lblBackColor.BackColor.R;

color.green = lblBackColor.BackColor.G;

color.blue = lblBackColor.BackColor.B;

Render.SetBackgroundColor(m_hContext, color);

The canvas and the window are not the same concept: the canvas is the area that carries the drawing content, and the window is the interface area that displays the canvas. When the canvas size and window size are inconsistent, two different types of areas may appear in the window—the area inside the canvas and the blank area outside the canvas. The appearance of the area inside the canvas is determined by the background style and background color; the blank window area outside the canvas is determined by the blank color. Distinguishing and setting these two types of colors separately helps to clearly identify the canvas boundary during debugging and demonstration, and also makes it easy to visually see the zoom and movement state of the canvas through contrasting colors.

**Difference Between the Two Types of Colors**

| Color | Area of Effect | When Visible | Related Function |
|:---|----|----|----|
| Canvas background color | Inside the canvas | Always | SetBackgroundColor |
| Window blank color | Blank area outside the canvas | When the canvas is smaller than the window or does not fill the window after zooming | SetBlankColor |

### Querying the OpenGL Version

The example obtains the OpenGL driver version of the current drawing context, the current rendering surface size, and the maximum supported surface size, and displays the concatenated information in a message box.

csharp

if (m_hContext == IntPtr.Zero) return;

int version = Render.GetDriverVersion(m_hContext);

int sz = Render.GetSurfaceSize(m_hContext);

int msz = Render.GetMaxSurfaceSize(m_hContext);

string s = \$"OpenGL version: {version \>\> 16}.{version & 0xFFFF}, Surface size: {sz}, Max surface size: {msz}";

MessageBox.Show(s);

The drawing module interacts directly with the graphics card based on OpenGL, and its available functions and performance are limited by the OpenGL version and surface size supported by the graphics card driver. During development and deployment, obtaining the OpenGL version of the current runtime environment, the current rendering surface size, and the maximum supported surface size helps to determine whether functions are available, troubleshoot compatibility issues, and estimate the maximum image or canvas size that can be processed. For example, older OpenGL versions may not support certain rendering features; if the canvas size exceeds the maximum surface size, drawing may fail or require block processing.

### Window Coordinate System and Canvas Coordinate System

The window coordinate system and the canvas coordinate system are two independent coordinate systems. Window coordinates use the top-left corner of the control as the origin, with x increasing to the right and y increasing downward; the unit is the same as window pixels, and it only describes the mouse position on the interface. Canvas coordinates use the top-left corner of the canvas as the origin and describe the position of primitives within the canvas; the unit is the same as the canvas size. When the canvas size differs from the window size, or after the canvas is zoomed, panned, or centered, the values of the same point in the two coordinate systems are not the same. Therefore, the window coordinates obtained from a mouse click must be converted to obtain the corresponding canvas coordinates, which are used to draw primitives or select targets at the correct position.

The example demonstrates how to convert mouse coordinates in the window to coordinates in the canvas coordinate system. This is a common requirement in human-computer interaction: the user clicks a point in the window, and the program needs to determine which position on the canvas that point corresponds to, in order to draw a primitive, select an object, or perform measurement at that position.

csharp

if (m_hContext == IntPtr.Zero) return;

if (ckbCanvasCoordSystem.Checked)

{

Render.SetCanvasSize(m_hContext, 800, 600);

Render.SetViewMode(m_hContext, ViewMode.Zoom);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

MessageBox.Show("Click at any place of the xguiPanel1, \n\nyou will get the mouse position in both canvas and display coordination systems");

}

private void xguiPanel1_MouseClick(object sender, MouseEventArgs e)

{

if (m_hContext == IntPtr.Zero) return;

if (ckbCanvasCoordSystem.Checked)

{

Point pt = e.Location;

GPoint pnWin = Render.ConvertPointToCanvas(m_hContext, pt.X, pt.Y);

string str = \$"Mouse clicked at ({pt.X},{pt.Y}) in the panel window, and ({pnWin.x},{pnWin.y}) in the canvas correspondingly";

MessageBox.Show(str);

ckbCanvasCoordSystem.Checked = false;

}

}

The first part of the code executes when the check box is checked, used to initialize the demonstration environment: set the canvas to 800\times600 and the display mode to ViewMode.Zoom, so that the canvas is fully presented in the window for easy observation of the conversion result. Then submit and refresh, and pop up a prompt telling the user that they can click anywhere on the panel to view the coordinates in the two coordinate systems.

The second part of the code is the mouse click event handler for the panel. First determine whether the drawing context is valid, then whether the check box is checked. If checked, read the click position e.Location, call ConvertPointToCanvas to obtain the canvas coordinates pnWin, and then display the two sets of coordinates in a message box. After display, uncheck the check box to avoid repeated pop-ups.

Window coordinates come directly from the mouse event and reflect the user's physical click position on the interface; canvas coordinates reflect the logical position of that point in the drawing content. The mapping between the two is jointly determined by the current display mode, display position, and display zoom: when the display mode is Default or Center and the zoom is 1, the two coordinate systems differ only by an offset at the origin; when the display mode is Zoom or Stretch, or after scaling via SetViewScaleEx, the mapping also includes a scale transformation. Using ConvertPointToCanvas automatically completes these calculations without manual derivation, avoiding coordinate errors caused by changes in display state.

**Notes**

Before execution, ensure the drawing context is valid; the example returns early via an IntPtr.Zero check. The conversion result is related to the display state at the time of the call: if the canvas size, display mode, display position, or zoom ratio is modified before conversion, submit and refresh again to ensure the conversion reference is consistent with the actual display. Mouse events should be bound to the control that hosts the canvas, in the example xguiPanel1, not the form itself; otherwise the coordinate origin may be inconsistent. If drag, continuous selection, or other interactions need to be supported, the conversion function should also be called in events such as MouseMove, MouseDown, and MouseUp, and the start and end coordinates recorded as needed. The conversion itself does not modify the canvas or drawing state and can be safely called repeatedly. This function is commonly used in interactive drawing, target selection, region annotation, coordinate measurement, and mouse localization.

### Other RVB Object Drawing

KMask, KBlob, and KContour are three core result objects in visual analysis, representing mask regions, connected components, and contours, respectively. Drawing the three directly onto the canvas allows intuitive checking of whether the analysis results are correct, facilitating debugging and verification. The implementation flow of the three buttons is similar: load image, pre-process, extract target objects, clear the canvas, set drawing resources, call the corresponding drawing function, and submit and refresh. The difference lies in the extraction method and the type of drawing resource: masks and BLOBs are drawn as regions using a brush for filling; contours are drawn as lines using a pen for outlining.

The example contains three button events, demonstrating the drawing of KMask, KBlob, and KContour objects from the RVB vision module onto the canvas. All three are result objects of visual analysis and can be directly visualized on the canvas through the corresponding drawing functions, facilitating debugging and verification of analysis results.

csharp

private void btnDrawMask_Click(object sender, EventArgs e)

{

if (m_hContext == IntPtr.Zero) return;

KImage im = new KImage("..\\samples\\shapes.png");

im.Cast(PixelFormat.Gray);

Dip.MinError(im.GetHandle(), MinErrorMode.Poinssen);

KMask msk = new KMask(im);

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hBrush = Render.GetCurBrush(m_hContext);

Render.SetBrushColor(hBrush, new GRgb(255, 0, 0));

Render.DrawMask(m_hContext, msk.GetHandle(), 0, 0, IntPtr.Zero);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

}

private void btnDrawBlob_Click(object sender, EventArgs e)

{

if (m_hContext == IntPtr.Zero) return;

KImage im = new KImage("..\\samples\\triangleangle.png");

im.Cast(PixelFormat.Gray);

Dip.Simple(im.GetHandle(), 128, false);

KBlob blob = KBlob.FromBinaryImage(im);

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hBrush = Render.GetCurBrush(m_hContext);

Render.SetBrushColor(hBrush, new GRgb(0, 255, 0));

Render.DrawBlob(m_hContext, blob.GetHandle(), 0, 0, IntPtr.Zero);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

}

private void btnDrawCountour_Click(object sender, EventArgs e)

{

if (m_hContext == IntPtr.Zero) return;

KImage im = new KImage("..\\samples\\shapes.png");

im.Cast(PixelFormat.Gray);

Dip.Simple(im.GetHandle(), 128, false);

KSequence seq = new KSequence();

KContour.FindContours(im, ContourCoder.Default, ContourFilter.Any, 12, seq);

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.GetCurPen(m_hContext);

Render.SetPenColor(hPen, new GRgb(0, 0, 255));

for (int i = 0; i \< seq.Count; i++)

{

Render.DrawCountour(m_hContext, seq.GetAt(i), 0, 0, IntPtr.Zero);

}

for (int i = 0; i \< seq.Count; i++)

{

KContour.ReleaseCountour(seq.GetAt(i));

}

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

}

**Key Function Description**

- Render.DrawMask(context, maskHandle, x, y, maskHandle2): Draws a KMask object to the specified position on the canvas. The second parameter is the mask handle; x and y are drawing offsets; the last parameter is an optional mask; passing IntPtr.Zero means not used. During drawing, the valid region of the mask (value 1) is filled with the current brush.

- Render.DrawBlob(context, blobHandle, x, y, maskHandle): Draws a KBlob object to the specified position on the canvas. During drawing, the foreground pixels of the BLOB are filled with the current brush.

- Render.DrawContour(context, contourHandle, x, y, maskHandle): Draws a KContour contour object to the specified position on the canvas. During drawing, the contour lines are outlined with the current pen, and the interior is not filled.

- Render.GetCurBrush(context), Render.GetCurPen(context): Obtains the currently selected brush or pen handle in the context, used to modify its color without recreating it.

- Render.SetBrushColor(brush, color), Render.SetPenColor(pen, color): Modifies the color of the brush or pen.

All three events first check whether the drawing context is valid to avoid null handle operations. Before drawing, Render.Clear(CanvasLayer.All) is called to clear the canvas, ensuring that each click displays only the current result without superimposing historical content. The color of the drawing resource is modified directly after obtaining the current handle via GetCurBrush/GetCurPen, which is simpler than recreating the resource; what is modified is the resource already selected in the context, so there is no need to reselect. The offset parameters of DrawMask, DrawBlob, and DrawContour are all (0,0), indicating drawing starts from the canvas origin; to adjust the position, modify these two parameters. After drawing, Render.Realize and Render.Flush must be called to submit and refresh before the canvas displays the result. KContour objects must be released after use; for KMask and KBlob objects, if they are constructed from an image or returned by a static method, their resource management method should refer to the library documentation, and they should also be released in a timely manner when necessary. This group of functions is commonly used in analysis result visualization, algorithm debugging, result verification, and interactive display.
