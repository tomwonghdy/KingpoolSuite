##  Practical Mathematical Functions

Corresponding project: SmartMath

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\PraticalMathematics_media/media/image1.png" style="width:6.74931in;height:3.98681in" />RVB provides a large number of practical functions for determining geometric entities and calculating their areas and properties, converting unstructured image data into structured geometric entities for tasks such as measurement, localization, inspection, and recognition. These geometric entities include points, lines, circles, arcs, ellipses, rectangles, polygons, etc. Computable properties include area, perimeter, centroid, orientation, circularity, bounding rectangle, minimum bounding rectangle, etc. With these functions, developers do not need to derive geometric relationships from raw pixels themselves; they can directly obtain structured measurement results for scenarios such as size measurement, position localization, shape classification, and defect determination.

RVB also provides practical functions related to rectangle operations and pixel operations. Rectangle operations include creation, clipping, merging, intersection, containment testing, dilation, and shrinking, facilitating ROI management and region operations; pixel operations include format conversion, arithmetic operations, grayscale intensity calculation, etc., used for direct processing and debugging of image data. These function interfaces are unified and flexible, and can be called at any stage without additional initialization or state preparation. Developers can focus on business logic and algorithm composition while leaving low-level geometric calculations and pixel processing to the library, thereby significantly accelerating development progress and shortening the development cycle.

## Determining Circle Center and Radius from Three Points

The example calculates the center and radius of a circumscribed circle from three known points, then marks the three points as crosshairs and draws the circle according to the calculation result.

csharp

RvPoint p1 = new RvPoint(130, 150);

RvPoint p2 = new RvPoint(180, 210);

RvPoint p3 = new RvPoint(225, 155);

RvPoint center;

int radius;

bool b = Smath.GetArcCenter(p1, p2, p3, out center, out radius);

Pool.Assert(b);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawCross(m_hContext, p1.x, p1.y, 12, 12, 0, IntPtr.Zero);

Render.DrawCross(m_hContext, p2.x, p2.y, 12, 12, 0, IntPtr.Zero);

Render.DrawCross(m_hContext, p3.x, p3.y, 12, 12, 0, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

*// Draw circle*

Render.DrawCircle(m_hContext, center.x, center.y, radius, false, IntPtr.Zero);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

In machine vision and geometric measurement, it is often necessary to derive geometric parameters from known points. Determining a circle from three points is a typical problem: any three non-collinear points uniquely determine a circle, whose center is the circumcenter of the triangle formed by the three points, and the radius is the distance from that circumcenter to any of the points. This calculation is commonly used in circle fitting, arc detection, target localization, and calibration. After calculating the center and radius, they can be visualized on the canvas for comparison with the original points, verification of fitting results, or subsequent measurement.

The return value of Smath.GetArcCenter is Boolean and must be checked before using the output center and radius; otherwise, in failure cases such as collinear points, the output values may be invalid. The three points should be as spread out as possible and not collinear; being too close or nearly collinear will cause numerical instability, resulting in large deviations in the center and radius.

## Polyline Scaling

The example defines three vertices forming a triangular polyline, draws the original shape with a green pen, then calls Smath.ScaleVertex to scale the vertex coordinates by 1.2 times, and draws the scaled shape again.

csharp

RvPoint\[\] ptarr = new RvPoint\[3\] { new RvPoint(216, 102), new RvPoint(372, 299), new RvPoint(216, 299) };

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawPolyline(m_hContext, ptarr, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

Smath.ScaleVertex(ptarr, 1.2f, 1.2f);

Render.DrawPolyline(m_hContext, ptarr, IntPtr.Zero);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

In geometric processing and image annotation, existing shapes often need to be scaled. Although directly modifying each vertex coordinate is feasible, manual calculation is cumbersome and error-prone. Smath.ScaleVertex provides a batch method for scaling vertices, adjusting the coordinates of all points in an array by a specified ratio at once. It is suitable for polylines, polygons, contours, and other graphics described by vertex sequences. The scaled vertices can be reused for drawing, calculation, or combination with other shapes, commonly used in ROI adjustment, shape matching, and size normalization.

ScaleVertex directly modifies the passed array, so the original vertex data is overwritten. If the original shape needs to be preserved, clone the array before scaling, e.g., RvPoint\[\] original = (RvPoint\[\])ptarr.Clone();. Scaling is based on the origin; to scale around a center point, first translate the vertices so that the center is the origin, scale, then translate back, or use an overload that supports specifying a center (if available). The scaling factor can be positive or negative; a negative value produces a mirror effect, but note that the shape orientation may be reversed.

## Bounding Regular Rectangle and Minimum Rectangle

The example extracts the edge point set of a BLOB from a binary image, calculates its axis-aligned bounding rectangle, calls Smath.ApproxEllipse to fit a minimum bounding ellipse to the edge points, and finally draws the bounding rectangle, the bounding box of the fitted ellipse, and the edge point polygon onto the canvas.

csharp

KImage im = new KImage("..\\samples\\heart.jpg");

im.Cast(PixelFormat.Gray);

Dip.MinError(im.Handle, MinErrorMode.Poinssen);

KBlob blob = KBlob.FromBinaryImageCluster(im);

RvPoint\[\] arr = blob.GetEdges();

Pool.Assert(arr != null);

RvRect rect = Smath.CalcBoundRect(arr);

RvBox2D box = new RvBox2D();

double e1, e2, e3;

bool b = Smath.ApproxEllipse(arr, out box, out e1, out e2, out e3);

Pool.Assert(b);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawRect(m_hContext, rect.left, rect.top, rect.right, rect.bottom, false, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(0, 123, 255));

Render.DrawBox2D(m_hContext, box, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(255, 123, 0));

Render.DrawPolygon(m_hContext, arr, false, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

Geometric fitting is a key step in image analysis, aiming to extract structured geometric descriptions from discrete edge points. The edge point set is provided by a BLOB or contour. Through statistical analysis of the point set, geometric entities such as the axis-aligned bounding rectangle, minimum bounding rectangle, and fitted ellipse can be calculated. The axis-aligned bounding rectangle is simple to compute and suitable for quickly locating the object range; the fitted ellipse reflects the object's shape, orientation, and aspect ratio, suitable for describing approximately elliptical targets. Combining both allows quick determination of the object position and finer shape features, facilitating measurement, localization, shape classification, and defect determination.

The return value of GetEdges must be checked for null; in the example, Pool.Assert is used to assert success. The return value of ApproxEllipse must also be checked; when fitting fails, the output box and error parameters may be invalid. If the object shape differs greatly from an ellipse (such as a heart shape or polygon), the fitting result can only serve as an approximate reference and should not be used as a basis for precise measurement.

## Determining Whether a Point Is Inside a Polygon

The example extracts the edge point set of a BLOB from a binary image as polygon vertices, translates the vertices, determines whether two given points are inside the polygon, and draws the two points and the polygon on the canvas, distinguishing inside/outside by color.

csharp

KImage im = new KImage("..\\samples\\triangle.png");

im.Cast(PixelFormat.Gray);

Dip.MinError(im.Handle, MinErrorMode.Poisson);

KBlob blob = KBlob.FromBinaryImageCluster(im);

RvPoint\[\] arr = blob.GetEdges();

Pool.Assert(arr != null);

Smath.PolylineMove(arr, 100, 100);

RvPoint pt1 = new RvPoint(178, 173);

RvPoint pt2 = new RvPoint(129, 140);

bool b1 = Smath.IsPointInPolygon(pt1, arr);

bool b2 = Smath.IsPointInPolygon(pt2, arr);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawCross(m_hContext, pt1.x, pt1.y, 12, 12, 0, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(255, 0, 0));

Render.DrawCross(m_hContext, pt2.x, pt2.y, 12, 12, 0, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(255, 123, 0));

Render.DrawPolygon(m_hContext, arr, false, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

Determining the positional relationship between a point and a polygon is a common problem in machine vision and geometric processing, used in target selection, region determination, hit testing, and interactive annotation. A polygon is described by a vertex sequence, which can come from BLOB edges, contour extraction, or manual definition.

Smath.IsPointInPolygon uses the ray casting method or a similar algorithm to determine whether a point is inside a polygon: a ray is cast from the point in any direction, and the number of intersections with polygon edges is counted. An odd number means inside, an even number means outside. This algorithm works for both convex and concave polygons but is not suitable for self-intersecting polygons.

The return value of GetEdges must be checked for null; in the example, Pool.Assert is used to assert success. IsPointInPolygon requires the polygon vertex array to be ordered and non-self-intersecting; otherwise the result may be unreliable.

## Polygon Expansion or Shrinking Copy

The example defines a pentagon vertex array, calls Smath.PolygonOffset to offset it outward and inward respectively to generate two new polygons, and draws the original polygon and the two offset results onto the canvas.

csharp

RvPoint\[\] arr = new RvPoint\[5\] { new RvPoint(67, 94), new RvPoint(179, 39), new RvPoint(271, 130), new RvPoint(208, 240), new RvPoint(91, 229) };

RvPoint\[\] newarr1 = Smath.PolygonOffset(arr, 22);

RvPoint\[\] newarr2 = Smath.PolygonOffset(arr, -22);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawPolygon(m_hContext, arr, false, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(255, 0, 0));

Render.DrawPolygon(m_hContext, newarr1, false, IntPtr.Zero);

Render.DrawPolygon(m_hContext, newarr2, false, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

Polygon offset translates each edge of a polygon along its normal direction by a given distance, generating a new polygon of similar shape but different size or position (inward/outward). Unlike overall scaling, offset keeps the relative distances between edges constant, so it does not produce proportional distortion due to distance from the origin. Offset is divided into outward offset (dilation) and inward offset (shrinkage): outward offset generates a larger polygon, inward offset a smaller one. This operation is widely used in region dilation/shrinkage, tolerance range generation, ROI inner/outer ring construction, collision detection, path planning, and shape matching. For example, in industrial inspection, a small outward expansion of the target contour is often used as a tolerance boundary, or a small inward shrinkage to remove edge noise.

When the offset distance is positive, each edge is translated along the outer normal, expanding the polygon overall; when negative, along the inner normal, shrinking the polygon overall. The absolute value of the offset distance determines the magnitude of expansion or shrinkage: the larger the distance, the greater the expansion or shrinkage. When a negative offset distance exceeds the inscribed circle radius of the polygon, the polygon completely disappears or produces self-intersection, and the result may not meet expectations. Therefore, the inward shrinkage amount should be smaller than the minimum inscribed circle radius of the polygon.

Offset preserves the parallel relationship of each edge and the angles between adjacent edges, so the offset polygon is similar to the original, but the edge lengths and area change. For convex polygons, the offset result is still convex; for concave polygons, when the offset distance is large, self-intersection or degeneration may occur at concave corners, depending on the algorithm implementation. PolygonOffset usually handles self-intersection or corner merging internally, but the result should still be judged by actual drawing.

PolygonOffset does not modify the original vertex array; it returns a new array, so the original polygon can be retained for comparison or subsequent operations.

## Calculating Area, Angle, or Length

The example defines a pentagon vertex array, calculates the polygon area, polyline length, and the angle formed by three points in sequence, and displays the results on the canvas.

csharp

RvPointF64\[\] arr = new RvPointF64\[5\]

{

new RvPointF64(248, 142), new RvPointF64(338, 67), new RvPointF64(428, 144),

new RvPointF64(393, 264), new RvPointF64(282, 264)

};

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

string s;

double n = Smath.CalcPolygonArea(arr);

s = \$"The area of the polygon is {n}";

Render.MoveTo(m_hContext, 30, 40);

Render.TextOut(m_hContext, s, 16);

n = Smath.CalcPolylineLength(arr, false);

s = \$"The length of the polyline is {n}";

Render.MoveTo(m_hContext, 30, 40 \* 2);

Render.TextOut(m_hContext, s, 16);

n = Smath.Calc3PAngle(new RvPointF64(192, 205), new RvPointF64(399, 208), new RvPointF64(389, 76), true);

s = \$"The angle formed by the three points is {n} degrees.";

Render.MoveTo(m_hContext, 30, 40 \* 3);

Render.TextOut(m_hContext, s, 16);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

Geometric calculation is a fundamental part of machine vision and image analysis, used to extract structured measurement information from vertex sequences or discrete points. Polygon area, polyline length, and the angle formed by three points are among the most common calculations, used respectively for region size evaluation, path length measurement, and angular relationship determination. These calculations do not depend on image pixels; they are determined solely by coordinate points, so they can be applied to BLOB edges, contour point sets, manually annotated points, or fitting results—any vertex sequence. They are important tools for measurement, localization, shape classification, and geometric verification. The example outputs the calculation results directly as text on the canvas for intuitive viewing, also demonstrating the combined use of geometric calculation and text drawing.

CalcPolygonArea requires consistent vertex order; if the vertex order is chaotic or self-intersecting, the area result may be inaccurate. The bClosed parameter of CalcPolylineLength determines whether the closing edge is included; in the example, false calculates the open polyline length; to calculate the full perimeter, pass true. In Calc3PAngle, the second point is the vertex of the angle; the order cannot be reversed, otherwise the angle will differ. When bDegrees is true, it returns degrees; when false, radians; in the example it is true. None of the three functions modify the input point array; they only return calculation results, so they can be safely called repeatedly.

## **Extracting Geometric Points**

The example defines a pentagon, a line segment, and a point outside the line, calculates the center of the polygon, the midpoint of the line segment, and the perpendicular foot from the point to the line, and draws the polygon, line segment, and four marker points on the canvas.

csharp

RvPointF64\[\] polygon = new RvPointF64\[5\]

{

new RvPointF64(248, 142), new RvPointF64(338, 67), new RvPointF64(428, 144),

new RvPointF64(393, 264), new RvPointF64(282, 264)

};

RvLineF64 line = new RvLineF64(new RvPointF64(81, 377), new RvPointF64(378, 484));

RvPointF64 outer = new RvPointF64(170, 313);

RvPointF64 perd;

bool b = Smath.CalcPerpendicularPoint(line, outer, out perd);

Pool.Assert(b);

RvPointF64 center = Smath.CalcPolygonCenter(polygon);

RvPointF64 mid = Smath.DeriveMidPoint(line.p0, line.p1);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

Render.DrawLineE1(m_hContext, (float)line.p0.x, (float)line.p0.y, (float)line.p1.x, (float)line.p1.y, IntPtr.Zero);

Render.DrawPolygon(m_hContext, polygon, false, IntPtr.Zero);

Render.DrawCross(m_hContext, (int)outer.x, (int)outer.y, 12, 12, 0, IntPtr.Zero);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawCross(m_hContext, (int)perd.x, (int)perd.y, 12, 12, 0, IntPtr.Zero);

Render.DrawCross(m_hContext, (int)center.x, (int)center.y, 12, 12, 0, IntPtr.Zero);

Render.DrawCross(m_hContext, (int)mid.x, (int)mid.y, 12, 12, 0, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

In geometric calculation, besides metrics such as area, length, and angle, it is often necessary to obtain positional relationships between points and figures, such as the center of a polygon, the midpoint of a line segment, and the perpendicular foot from a point to a line. These points play an important role in geometric analysis, target localization, path planning, measurement calibration, and graphic editing. The polygon center can represent the overall position of a region, the line segment midpoint can determine the middle position of a path, and the perpendicular foot is the landing point of the shortest distance from a point to a line, also the basis for calculating point-to-line distance, performing projection, and determining positional relationships. The example draws the three together with the original figures for intuitive verification of the calculation results.

The return value of CalcPerpendicularPoint is Boolean and must be checked before using the output perpendicular foot coordinates; in the example, Pool.Assert(b) asserts success. The two endpoints of the line segment should not coincide; otherwise the line degenerates and calculation may fail. Whether the perpendicular foot falls within the line segment must be determined separately: if the foot coordinates exceed the range between p0 and p1, the foot lies on the extension line of the segment, and the shortest distance from the point to the segment should be calculated using the endpoints instead. CalcPolygonCenter and DeriveMidPoint do not modify input data; they only return calculation results.

## Adapting Minimum Rectangle

In image processing and human-computer interaction, it is often necessary to place a sub-region within a given rectangular region, with the sub-region's aspect ratio meeting specific requirements. For example, placing a 4:3 preview window within a fixed-size ROI, or generating a tolerance box scaled to the target size within a detection region. Directly using the container rectangle would cause aspect ratio distortion, while manually calculating the position and size of the inscribed rectangle is cumbersome. Smath.AdaptRect solves this problem: taking a container rectangle as a constraint, based on the specified target width and height, it calculates an inscribed rectangle that maintains the same aspect ratio, is as large as possible, and is completely inside the container, and returns the actual calculated ratio.

The example defines a container rectangle, calls Smath.AdaptRect to derive an inscribed rectangle with an aspect ratio of 380:340, and draws the container rectangle and the derived rectangle on the canvas for intuitive comparison of their position and ratio.

csharp

double ratio = 0;

RvRect container = new RvRect(new RvPoint(117, 187), new RvPoint(476, 386));

RvRect insideRect = Smath.AdaptRect(container, 380, 340, ref ratio);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawRect(m_hContext, container.left, container.top, container.right, container.bottom, false, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(0, 123, 255));

Render.DrawRect(m_hContext, insideRect.left, insideRect.top, insideRect.right, insideRect.bottom, false, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

The core of AdaptRect is to maximize the inscribed rectangle while maintaining the aspect ratio. The algorithm usually compares the container aspect ratio with the target aspect ratio: if the container is wider, the container height is used as the basis and the width is calculated according to the target ratio; if the container is taller, the container width is used as the basis and the height is calculated according to the target ratio. The derived rectangle is centered within the container. Therefore, the resulting rectangle may differ in shape from the container but is always completely inside it, with an aspect ratio consistent with the input width:height.

**Input Parameters and Output Results Comparison**

| Item | Value | Description |
|:---|----|----|
| Container rectangle | (117, 187) - (476, 386) | Width 359, height 199, aspect ratio \approx 1.80 |
| Target width/height | 380 \times 340 | Aspect ratio \approx 1.118 |
| Derived rectangle | Returned by function | Aspect ratio \approx 1.118, centered in container |
| Actual ratio | Returned by ratio | Writes back the actually calculated ratio |

The container aspect ratio (1.80) is greater than the target aspect ratio (1.118), meaning the container is relatively wide, so the height of the derived rectangle will be limited; based on the container height, the width is reduced according to the target ratio. The derived result does not fill the container horizontally, leaving gaps on both sides.

ratio is a ref parameter and must be initialized before calling; after execution, its value is written back as the actual calculated ratio. The width and height of container must be positive; otherwise calculation may fail or return an invalid result. width and height only need to reflect the ratio relationship; their absolute values do not affect the shape of the derived rectangle, only the ratio calculation; however, for clarity, values consistent with the actual target size are usually passed. The derived rectangle is always completely inside the container and centered, so the result will not exceed the container boundary. This method differs from CalcBoundRect: the former derives a rectangle within a container based on ratio constraints, while the latter calculates a bounding rectangle from a point set; their purposes are different.

## Polyline Rotation

In geometric processing and graphic editing, it is often necessary to rotate points or figures around a reference point by a specified angle. For example, rotating annotation points around a target center, correcting tilted contours, constructing symmetric figures, or simulating the rotational motion of a robotic arm. Rotation is a rigid body transformation that keeps the shape and size of the figure unchanged, only changing its orientation and position. Rotation around a single point and around multiple vertices are identical in principle—both transform each vertex's coordinates relative to the rotation center—but the interfaces differ: RotatePoint targets a single point, and RotateVertex targets a vertex array, allowing rotation of an entire polyline or polygon at once.

The example defines a point p0 and rotates it around point p1 by -45 degrees to obtain a new point, and also defines a polyline of four vertices, rotates it around (60, 70) by 25 degrees, and draws the original and rotated figures on the canvas for intuitive comparison.

csharp

RvPoint\[\] ptarr = new RvPoint\[4\] { new RvPoint(216, 102), new RvPoint(372, 299), new RvPoint(216, 299), new RvPoint(216, 102) };

RvPointF32 newpt = Smath.RotatePoint(p0, p1.x, p1.y, -45);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawLineE1(m_hContext, p0.x, p0.y, p1.x, p1.y, IntPtr.Zero);

Render.DrawPolyline(m_hContext, ptarr, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

Render.DrawLineE1(m_hContext, p1.x, p1.y, newpt.x, newpt.y, IntPtr.Zero);

Smath.RotateVertex(ptarr, 60, 70, 25);

Render.DrawPolyline(m_hContext, ptarr, IntPtr.Zero);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

**Rotation Direction and Angle**

A positive angle indicates clockwise rotation, and a negative angle indicates counterclockwise rotation. This is because the y-axis points downward in the image coordinate system, opposite to the mathematical coordinate system. The choice of rotation center directly determines the position after rotation: rotating around the origin moves the entire figure, while rotating around the figure's own center keeps its position basically unchanged, only changing its orientation.

RotatePoint does not modify the input point; it returns a new point, suitable for scenarios where the original coordinates need to be preserved. RotateVertex directly modifies the array; if the original vertices need to be preserved, clone the array before calling, e.g., RvPoint\[\] original = (RvPoint\[\])ptarr.Clone();. The rotation angle is in degrees; positive is clockwise, negative is counterclockwise, consistent with the image coordinate system. The rotation center can be inside, outside, or on a vertex of the figure; choosing different centers yields different rotation results. The input array type of RotateVertex is RvPoint (integer); rotated coordinates are rounded, possibly introducing a small precision loss; if floating-point precision is required, use the corresponding RvPointF32 or RvPointF64 version.

## Line Extension and Trimming

In geometric processing and machine vision applications, it is often necessary to adjust the length of line segments. For example, extending a detected edge line at both ends to intersect with another line, clipping a segment that exceeds a region to a specified boundary, or correcting endpoint positions during measurement. Smath.ExtendLine is used to extend or shorten a line segment along its direction by a specified length, and Smath.TrimLine is used to truncate a line segment at the intersection with a specified reference line. Both keep the direction and position of the segment unchanged, only adjusting its endpoints, belonging to length transformation of line segments.

The example defines a line segment line and a reference line baseln, calls ExtendLine to extend by 60 pixels, calls TrimLine to truncate at the reference line, and draws the original segment, reference line, extension result, and truncation result on the canvas for intuitive comparison.

csharp

RvLineF64 line = new RvLineF64(new RvPointF64(30, 80), new RvPointF64(150, 50));

RvLineF64 baseln = new RvLineF64(new RvPointF64(130, 30), new RvPointF64(130, 150));

RvPointF64 outer = new RvPointF64(120, 136);

RvLineF64 newline1 = Smath.ExtendLine(line, 60);

RvLineF64 newline2 = Smath.TrimLine(line, baseln, 1);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

*// Move for better view*

newline1.Move(300, 130);

newline2.Move(0, 130);

Render.DrawLineE1(m_hContext, (float)newline1.p0.x, (float)newline1.p0.y, (float)newline1.p1.x, (float)newline1.p1.y, IntPtr.Zero);

Render.DrawLineE1(m_hContext, (float)newline2.p0.x, (float)newline2.p0.y, (float)newline2.p1.x, (float)newline2.p1.y, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

Render.DrawLineE1(m_hContext, (float)line.p0.x, (float)line.p0.y, (float)line.p1.x, (float)line.p1.y, IntPtr.Zero);

Render.DrawLineE1(m_hContext, (float)baseln.p0.x, (float)baseln.p0.y, (float)baseln.p1.x, (float)baseln.p1.y, IntPtr.Zero);

Render.DrawCross(m_hContext, (int)outer.x, (int)outer.y, 12, 12, 0, IntPtr.Zero);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

ExtendLine and TrimLine do not modify the input segment; they return a new segment, so the original segment can be retained for comparison.

The extension length of ExtendLine can be positive or negative; positive extends, negative shortens. Confirm the library's convention for the extension direction (whether symmetric extension, which end is fixed). TrimLine requires the segment to intersect the reference line; if parallel or the intersection is not within the valid range, the result is invalid.

## Regular Rectangle Operations

Rectangles are one of the most commonly used geometric regions in machine vision and image processing, widely used for ROI definition, target bounding boxes, detection regions, and UI layout. In practical applications, rectangles are often not fixed but need to undergo rotation, translation, scaling, or intersection/union operations with other rectangles to meet requirements such as region alignment, range merging, and overlap determination. Smith provides a set of rectangle operation functions covering five common operations: rotation, translation, scaling, union, and intersection. All return new rectangle objects without modifying the original rectangle, facilitating comparison and combined use.

The example defines two original rectangles re1 and re2, performs rotation, translation, scaling, union, and intersection operations respectively, and draws the original rectangles and the operation results on the canvas, distinguishing categories by color for intuitive comparison.

csharp

RvRect re1 = new RvRect(89, 45, 253, 181);

RvRect re2 = new RvRect(170, 118, 343, 215);

RvRect rect1 = Smath.RotateRect(re1, 90);

RvRect rect2 = Smath.MoveRect(re2, 0, 200);

RvRect rect3 = Smath.ScaleRect(rect2, 1.2);

RvRect rect4 = Smath.RectUnion(re1, re2);

RvRect rect5 = Smath.RectIntersect(re1, re2);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

Render.DrawRect(m_hContext, re1.left, re1.top, re1.right, re1.bottom, false, IntPtr.Zero);

Render.DrawRect(m_hContext, re2.left, re2.top, re2.right, re2.bottom, false, IntPtr.Zero);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawRect(m_hContext, rect1.left, rect1.top, rect1.right, rect1.bottom, false, IntPtr.Zero);

Render.DrawRect(m_hContext, rect2.left, rect2.top, rect2.right, rect2.bottom, false, IntPtr.Zero);

Render.DrawRect(m_hContext, rect3.left, rect3.top, rect3.right, rect3.bottom, false, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(255, 0, 0));

Render.DrawRect(m_hContext, rect4.left, rect4.top, rect4.right, rect4.bottom, false, IntPtr.Zero);

Render.DrawRect(m_hContext, rect5.left, rect5.top, rect5.right, rect5.bottom, false, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

**Key Function Description**

- Smath.RotateRect(rect, angle): Rotates the rectangle around its center by a specified angle.

  - rect: The rectangle to rotate.

  - angle: Rotation angle in degrees; positive means clockwise.

  - Return value: RvRect, representing the bounding rectangle after rotation. Since RvRect is an axis-aligned rectangle and cannot directly represent a tilted rectangle, the result is usually the axis-aligned bounding rectangle enclosing the rotated shape, and its size may increase.

- Smath.MoveRect(rect, dx, dy): Translates the rectangle by a specified offset.

  - rect: The rectangle to translate.

  - dx, dy: Horizontal and vertical displacements; positive values move right and down, respectively.

  - Return value: RvRect, the new translated rectangle with unchanged size.

- Smath.ScaleRect(rect, ratio): Scales the rectangle proportionally.

  - rect: The rectangle to scale.

  - ratio: Scaling factor; greater than 1 enlarges, less than 1 shrinks.

  - Return value: RvRect, the new scaled rectangle. Scaling is usually based on the rectangle center, with size changing proportionally.

- Smath.RectUnion(rect1, rect2): Calculates the union of two rectangles.

  - Return value: RvRect, the smallest axis-aligned rectangle that can contain both rectangles, i.e., their bounding rectangle.

- Smath.RectIntersect(rect1, rect2): Calculates the intersection of two rectangles.

  - Return value: RvRect, the overlapping region of the two rectangles. If the rectangles do not intersect, the return value may be an empty rectangle (width/height 0 or negative); check validity before use.

**Comparison of Five Rectangle Operations**

| Operation | Function | Input | Output | Typical Use |
|:---|----|----|----|----|
| Rotate | RotateRect | Rectangle, angle | Bounding rectangle after rotation | Orientation correction, tilted region processing |
| Translate | MoveRect | Rectangle, offset | Translated rectangle | Position adjustment, coordinate alignment |
| Scale | ScaleRect | Rectangle, ratio | Scaled rectangle | Region enlargement/shrinkage, tolerance generation |
| Union | RectUnion | Two rectangles | Smallest rectangle containing both | Range merging, overall bounding |
| Intersect | RectIntersect | Two rectangles | Overlapping region | Overlap determination, common region extraction |

RotateRect returns the axis-aligned bounding rectangle after rotation, not the tilted rectangle itself. If the rotation angle is not a multiple of 90 degrees, the bounding rectangle's width and height are usually larger than the original, containing more blank area. To accurately represent a tilted rectangle, use the RvBox2D type with DrawBox2D. RectIntersect may return an empty rectangle when the two rectangles do not intersect; check whether its width and height are positive before use to avoid subsequent calculation errors. The results of RectUnion and RectIntersect are both axis-aligned rectangles without rotation. All operations do not modify the input rectangles and return new objects, so the original rectangles can be retained for comparison or subsequent operations.

## Deriving Perpendicular Line and Parallel Line

In geometric construction and machine vision measurement, it is often necessary to derive new geometric elements based on existing lines. For example, generating a line parallel to an existing line for constructing equidistant boundaries, tolerance regions, or offset paths; deriving a perpendicular from a line for establishing an orthogonal coordinate system, measuring the perpendicular distance from a point to a line, or constructing a rectangular region. These operations belong to line segment derivation, where the input is an existing line and the output is a new line, without modifying the original line. The example demonstrates deriving a parallel line and deriving a perpendicular line, and draws the original lines and derived results on the canvas, distinguished by color for intuitive comparison.

The example defines two lines ln1 and ln2, derives a parallel line at a distance of 35 from ln1, derives a perpendicular line of length 120 from ln2, and finally draws the original lines and the two derived lines.

csharp

RvLineF64 ln1 = new RvLineF64(new RvPointF64(85, 40), new RvPointF64(247, 166));

RvLineF64 ln2 = new RvLineF64(new RvPointF64(40, 135), new RvPointF64(260, 48));

RvLineF64 newline1 = Smath.DeriveParallel(ln1, 35);

RvLineF64 newline2 = Smath.DerivePerpend(ln2, 120, false, false);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

IntPtr hPen = Render.CreatePen(new GRgb(0, 255, 0), 1, LinePattern.Solid);

IntPtr hOldPen = Render.SelectPen(m_hContext, hPen);

Render.DrawLineE1(m_hContext, (float)newline1.p0.x, (float)newline1.p0.y, (float)newline1.p1.x, (float)newline1.p1.y, IntPtr.Zero);

Render.DrawLineE1(m_hContext, (float)newline2.p0.x, (float)newline2.p0.y, (float)newline2.p1.x, (float)newline2.p1.y, IntPtr.Zero);

Render.SetPenColor(hPen, new GRgb(255, 123, 0));

Render.DrawLineE1(m_hContext, (float)ln1.p0.x, (float)ln1.p0.y, (float)ln1.p1.x, (float)ln1.p1.y, IntPtr.Zero);

Render.DrawLineE1(m_hContext, (float)ln2.p0.x, (float)ln2.p0.y, (float)ln2.p1.x, (float)ln2.p1.y, IntPtr.Zero);

Render.SelectPen(m_hContext, hOldPen);

Render.DestroyPen(hPen);

*// Refresh*

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

**Key Function Description**

- Smath.DeriveParallel(line, distance): Derives a line parallel to the specified line.

  - line: Original line.

  - distance: Perpendicular distance from the parallel line to the original line. Positive and negative values indicate different sides of the offset direction.

  - Return value: RvLineF64, the derived parallel line, with the same direction as the original line and offset by the specified distance.

- Smath.DerivePerpend(line, length, bFullSize, bStartPos): Derives a perpendicular line from the specified line.

  - line: Original line.

  - length: Length of the perpendicular line.

  - bFullSize: Whether the line passes through the endpoint, forming a line with half the distance on each side. When true, the perpendicular line takes a point on the original line as its midpoint and extends length / 2 to each side; when false, the perpendicular line extends length to one side from a point.

  - bStartPos: Whether it passes through the start point. When true, the perpendicular passes through the start point p0 of the original line; when false, it passes through the end point p1.

  - Return value: RvLineF64, the derived perpendicular line, perpendicular to the original line, with length specified by length.

**Comparison of the Two Derivation Operations**

| Operation | Input | Output | Typical Use |
|:---|----|----|----|
| Parallel line | Line, distance | Parallel line | Equidistant boundary, tolerance region, offset path |
| Perpendicular line | Line, length, bFullSize, bStartPos | Perpendicular line | Orthogonal construction, distance measurement, rectangular region |

**Effect of** DerivePerpend **Parameter Combinations**

| bFullSize | bStartPos | Perpendicular Position |
|:---|----|----|
| false | false | Passes through end point, extends to one side by length |
| false | true | Passes through start point, extends to one side by length |
| true | false | End point as midpoint, extends length/2 on both sides |
| true | true | Start point as midpoint, extends length/2 on both sides |

In the example, both bFullSize and bStartPos are false, meaning the perpendicular passes through the end point of ln2 and extends 120 pixels to one side.

The sign of the distance parameter of DeriveParallel determines which side of the original line the parallel line lies on; refer to the library documentation for the specific convention, and try positive and negative values to observe the effect. The bFullSize and bStartPos parameters of DerivePerpend jointly determine the position and extension mode of the perpendicular: bStartPos selects whether the perpendicular passes through the start or end point, and bFullSize determines whether it extends to one side or both sides with that point as the midpoint. If the perpendicular needs to pass through the midpoint of the segment, first calculate the midpoint and then construct it; if a perpendicular needs to be derived from any position on the segment, combine with other geometric functions. All derivation functions do not modify the original line; they return new objects, so the original line can be retained for comparison or subsequent operations.

## Determining the Relative Position of a Point and Other Geometric Entities

In machine vision and geometric analysis, determining the positional relationship between geometric entities is a basic and common task. For example, determining the arc direction from three points to judge path direction; determining whether one rectangle completely contains another for region nesting analysis; determining whether a point falls inside a rectangle, circle, or on one side of a line, for hit testing, region determination, direction analysis, and geometric verification. These determination functions all return the RvBool tri-state value, typically including True, False, and possibly Unknown or Invalid, used to represent the determination result or an undeterminable situation. The example demonstrates five relationship determinations in sequence and outputs the results as text on the canvas.

csharp

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

string s;

RvBool b = Smath.IsClockWise(new RvPoint(561, 223), new RvPoint(681, 333), new RvPoint(547, 388));

s = "The three points on the same arc are in clockwise order";

if (b == RvBool.False)

{

s = "The three points on the same arc are in counter clockwise order";

}

Render.MoveTo(m_hContext, 30, 40);

Render.TextOut(m_hContext, s, 16);

RvRect re1 = new RvRect(89, 45, 253, 181);

RvRect re2 = new RvRect(170, 118, 343, 215);

b = Smath.IsRectSurround(re1, re2);

s = "The first rect is surrounded by the second rectangle";

if (b == RvBool.False)

{

s = "The first rect is not surrounded by the second rectangle";

}

Render.MoveTo(m_hContext, 30, 40 \* 2);

Render.TextOut(m_hContext, s, 16);

b = Smath.IsPointInsideRect(re1, new RvPoint(145, 105));

s = "The point is inside the first rectangle ";

if (b == RvBool.False)

{

s = "The point is not inside the first rectangle ";

}

Render.MoveTo(m_hContext, 30, 40 \* 3);

Render.TextOut(m_hContext, s, 16);

b = Smath.IsRightOfLine(new RvLine(new RvPoint(250, 307), new RvPoint(430, 167)), new RvPoint(314, 219));

s = "The point is on the right of the line";

if (b == RvBool.False)

{

s = "The point is on the left of the line";

}

Render.MoveTo(m_hContext, 30, 40 \* 4);

Render.TextOut(m_hContext, s, 16);

b = Smath.IsPointInsideCircle(new RvPointF64(514, 321), 90, new RvPointF64(506, 292));

s = "The point is inside the circle";

if (b == RvBool.False)

{

s = "The point is outside the circle";

}

Render.MoveTo(m_hContext, 30, 40 \* 5);

Render.TextOut(m_hContext, s, 16);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

**Key Function Description**

- Smath.IsClockWise(p1, p2, p3): Determines the arrangement direction of three points on an arc.

  - p1, p2, p3: The three points on the arc, given in arc order.

  - Return value: RvBool. True means clockwise, False means counterclockwise.

  - The algorithm is based on the sign of the cross product: calculate the cross product of vectors p1-p2 and p2-p3; a positive value indicates counterclockwise, a negative value indicates clockwise; the specific sign convention depends on the image coordinate system.

- Smath.IsRectSurround(rect1, rect2): Determines whether one rectangle completely surrounds another.

  - Return value: RvBool. True means surrounded, False means not surrounded.

- Smath.IsPointInsideRect(rect, point): Determines whether a point is inside a rectangle.

  - Return value: RvBool. True means inside, False means outside.

- Smath.IsRightOfLine(line, point): Determines whether a point is on the right side of a line.

  - Return value: RvBool. True means on the right, False means on the left.

  - The left/right determination depends on the direction of the line, determined by the direction from p0 to p1. If the endpoint order is reversed, the left/right determination is also reversed.

- Smath.IsPointInsideCircle(center, radius, point): Determines whether a point is inside a circle.

  - center: Circle center, type RvPointF64.

  - radius: Circle radius.

  - point: Point to determine, type RvPointF64.

  - Return value: RvBool. True means inside, False means outside.

  - The determination is based on comparing the distance from the point to the center with the radius; if the distance is less than the radius, it is inside.

**Comparison of Five Relationship Determinations**

| Determination | Function | Input | Output | Typical Use |
|:---|----|----|----|----|
| Arc direction | IsClockWise | Three points | Clockwise/counterclockwise | Path direction determination |
| Rectangle containment | IsRectSurround | Two rectangles | Whether contained | Region nesting analysis |
| Point in rectangle | IsPointInsideRect | Rectangle, point | Whether inside | Hit testing, ROI determination |
| Point on line side | IsRightOfLine | Line, point | Left/right side | Direction analysis, region division |
| Point in circle | IsPointInsideCircle | Center, radius, point | Whether inside | Circular region determination |

**RvBool Tri-state Value**

RvBool is the type used in the library to represent Boolean determination results, typically including True, False, and possibly Unknown, Invalid, etc. The example only checks RvBool.False; other cases (including True and possible abnormal states) are treated as True by default. In actual use, if it is necessary to distinguish undeterminable situations, explicitly check for states such as RvBool.Fuzzy or similar to avoid misjudgment.

The order of the three points in IsClockWise determines the result; if given in arc order, it reflects the actual direction; reversing the order reverses the result. The left/right determination of IsRightOfLine depends on the direction of the line, determined by the direction from p0 to p1. IsRectSurround requires the inner rectangle to be completely inside the outer rectangle; whether boundary coincidence counts as containment should refer to the library documentation. IsPointInsideCircle uses floating-point coordinates, suitable for precise calculation; IsPointInsideRect uses integer coordinates; note the coordinate type matching.

## Calculating the Distance Between a Point and a Line

Distance calculation is one of the most basic operations in geometric analysis and machine vision measurement, used to measure the spatial proximity between geometric entities. Common forms include point-to-point distance, point-to-line distance, and line-to-line distance. These calculations are widely used in size measurement, target localization, deviation detection, path planning, and geometric verification. For example, measuring the actual distance between two marker points, determining the degree to which a point deviates from a reference line, or evaluating whether two edge lines are parallel and whether their spacing meets requirements. Smith provides corresponding distance functions, all returning double results, without modifying input objects, and can be safely called repeatedly. The example calculates point-to-point, point-to-line, and line-to-line distances in sequence and outputs the results as text on the canvas.

csharp

RvLineF64 line1 = new RvLineF64(new RvPointF64(224, 221), new RvPointF64(666, 128));

RvLineF64 line2 = new RvLineF64(new RvPointF64(254, 345), new RvPointF64(298, 247));

RvPointF64 pt1 = new RvPointF64(239, 117);

RvPointF64 pt2 = new RvPointF64(422, 78);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

string s;

double n = Smath.DistPointToPoint(pt1, pt2);

s = \$"The distance between two points is {n}";

Render.MoveTo(m_hContext, 30, 40);

Render.TextOut(m_hContext, s, 16);

n = Smath.DistPointToLine(pt1, line1);

s = \$"The distance between point 1 and line 1 is {n}";

Render.MoveTo(m_hContext, 30, 40 \* 2);

Render.TextOut(m_hContext, s, 16);

n = Smath.DistLineToLine(line1, line2);

s = \$"The distance between two lines is {n}.";

Render.MoveTo(m_hContext, 30, 40 \* 3);

Render.TextOut(m_hContext, s, 16);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

**Function Description**

- Smath.DistPointToPoint(p1, p2): Calculates the Euclidean distance between two points.

  - p1, p2: Two points, type RvPointF64.

  - Return value: double, the straight-line distance between the two points, calculated as sqrt((x2 - x1)^2 + (y2 - y1)^2).

- Smath.DistPointToLine(point, line): Calculates the perpendicular distance from a point to a line.

  - point: Point to measure, type RvPointF64.

  - line: Reference line, type RvLineF64.

  - Return value: double, the shortest distance from the point to the line. This is the perpendicular distance, i.e., the length of the perpendicular from the point to the line. If the perpendicular foot falls outside the line segment, the function usually still returns the distance to the extension line; refer to the library documentation for specific behavior.

- Smath.DistLineToLine(line1, line2): Calculates the distance between two lines.

  - line1, line2: Two lines, type RvLineF64.

  - Return value: double, the average distance between the two lines.

**Comparison of Three Distance Calculations**

| Calculation | Function | Input | Output | Typical Use |
|:---|----|----|----|----|
| Point to point | DistPointToPoint | Two points | Straight-line distance | Size measurement, marker spacing |
| Point to line | DistPointToLine | Point, line | Perpendicular distance | Deviation detection, perpendicular distance measurement |
| Line to line | DistLineToLine | Two lines | Distance | Parallelism determination, spacing measurement |

All three functions return double, with units consistent with the coordinate units, usually pixels. To convert to actual physical units, combine with calibration parameters. DistPointToLine calculates the perpendicular distance from a point to a line, which differs from the distance from a point to a line segment: if the perpendicular foot falls outside the segment, the shortest distance from the point to the segment should be the distance to the nearer endpoint. If the application requires segment distance, first determine whether the foot falls within the segment, and if necessary use CalcPerpendicularPoint to calculate the foot and then judge. In the 2D case, DistLineToLine returns 0 if the two lines are not parallel (they must intersect); this function is more suitable for determining whether two lines are parallel and their parallel spacing, rather than a general line-to-line distance.

## Pixel-Related Calculations

A pixel is the basic unit of a digital image, and its color is determined by the components of each channel. In image processing and machine vision, it is often necessary to calculate pixel-related quantities, such as: calculating the number of bytes per row based on image width and pixel format, for memory allocation and row stride calculation; brightening or darkening pixels to adjust image brightness or generate highlight effects; calculating the intensity or grayscale value of a pixel in a specific color space, for color analysis, brightness evaluation, and feature extraction. Smath provides a set of pixel-related practical functions, covering depth calculation, row stride calculation, pixel brightening, and color space intensity calculation. The example demonstrates these four types of calculations using randomly generated colors and widths, and outputs the results on the canvas.

csharp

Random rand = new Random();

RvRgb rgb = new RvRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));

int width = rand.Next(4094);

*// Clear canvas (optional)*

Render.Clear(m_hContext, CanvasLayer.All);

string s;

int n = Smath.CalcPitch(Smath.CalcDepth(PixelFormat.BGR), width);

s = \$"The number of bytes in one row of a BGR image with width {width} is {n}.";

Render.MoveTo(m_hContext, 30, 40);

Render.TextOut(m_hContext, s, 16);

RvRgb brighter = Smath.GetBrighterPixel(rgb, 0.3f);

s = \$"A pixel with red = {rgb.red}, green = {rgb.green}, blue = {rgb.blue} becomes {brighter.red}, {brighter.green}, {brighter.blue} after brightening";

Render.MoveTo(m_hContext, 30, 40 \* 2);

Render.TextOut(m_hContext, s, 16);

n = Smath.CalcPixelStrength(rgb, ColorSpace.HSV);

s = \$"The strength of a pixel with red {rgb.red}, green {rgb.green}, blue {rgb.blue}";

Render.MoveTo(m_hContext, 30, 40 \* 3);

Render.TextOut(m_hContext, s, 16);

s = \$"is {n} in the color space HSV";

Render.MoveTo(m_hContext, 30, 40 \* 3 + 17);

Render.TextOut(m_hContext, s, 16);

Render.Realize(m_hContext, CanvasLayer.All);

Render.Flush(m_hContext);

The factor of GetBrighterPixel is the brightening ratio, ranging from 0 to 1; the larger the value, the more obvious the brightening. To darken, use the corresponding GetDarkerPixel. After brightening, each channel component is clipped to 255, so colors originally near saturation may lose proportional differences after brightening. The return value of CalcPixelStrength depends on the selected color space: in HSV it is usually V (value), in HSL it is L (lightness), and in grayscale space it is the grayscale value. Choose the appropriate color space according to application requirements. The row stride calculation of CalcPitch depends on whether the storage format is aligned: if the library uses 1-byte alignment, the result equals depth $\times$ width; if aligned to 4 or 8 bytes, the result is rounded up.
