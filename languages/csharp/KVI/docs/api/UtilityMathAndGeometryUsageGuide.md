## **Usage Instructions for Practical Mathematical Functions**

## Constants

| Name        | Type | Value | Description       |
|-------------|------|-------|-------------------|
| ORI_UNK     | int  | -1    | Unknown direction |
| ORI_RIGHT   | int  | 1     | Right side        |
| ORI_LEFT    | int  | 0     | Left side         |
| ORI_INSIDE  | int  | 1     | Inside            |
| ORI_OUTSIDE | int  | 0     | Outside           |
| ORI_CW      | int  | 1     | Clockwise         |
| ORI_CCW     | int  | 0     | Counter-clockwise |

## 1. Angle Normalization

### NormalizeAngle360

csharp

public static double NormalizeAngle360(double angle)

**Description**: Normalizes an angle to the \[0, 360) range.

**Parameters**

| Parameter | Type   | Description           |
|-----------|--------|-----------------------|
| angle     | double | Input angle (degrees) |

**Return Value**: double — the normalized angle.

### NormalizeAnglePi

csharp

public static double NormalizeAnglePi(double angle)

**Description**: Normalizes a radian to the \[0, 2$\pi$) range.

**Parameters**

| Parameter | Type   | Description  |
|-----------|--------|--------------|
| angle     | double | Input radian |

**Return Value**: double — the normalized radian.

## 2. Arc and Ellipse

### AdaptRect

csharp

public static RvRect AdaptRect(RvRect container, int width, int height, ref double ratio)

public static RvRect AdaptRect(RvRect container, int width, int height, bool bNoAlignCenter, ref double ratio)

public static RvRect AdaptRect(RvRect container, int width, int height, ref int fitness, ref double ratio)

**Description**: Derives an inner rectangle within the container rectangle at the target aspect ratio.

**Parameters**

| Parameter      | Type       | Description                         |
|----------------|------------|-------------------------------------|
| container      | RvRect     | Constraining container rectangle    |
| width          | int        | Target width                        |
| height         | int        | Target height                       |
| bNoAlignCenter | bool       | When true, does not center          |
| fitness        | ref int    | Output fitness                      |
| ratio          | ref double | Output actual computed aspect ratio |

**Return Value**: RvRect — the derived inner rectangle.

### ApproxEllipse

csharp

public static bool ApproxEllipse(RvPoint\[\] edgePoints, out RvBox2D box2d, out double avgError, out double maxError, out double minError)

public static bool ApproxEllipse(RvPointF32\[\] edgePoints, out RvBox2D box2d, out float avgError, out float maxError, out float minError)

public static bool ApproxEllipse(RvPointF64\[\] edgePoints, out RvBox2D box2d, out double avgError, out double maxError, out double minError)

**Description**: Fits a minimum bounding ellipse to an edge point set.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| edgePoints | RvPoint\[\] / RvPointF32\[\] / RvPointF64\[\] | Edge point array |
| box2d | out RvBox2D | Output ellipse bounding box (center, width, height, angle) |
| avgError | out double / out float | Average fitting error |
| maxError | out double / out float | Maximum fitting error |
| minError | out double / out float | Minimum fitting error |

**Return Value**: bool — whether the fitting succeeded.

### GenEllipse

csharp

public static int GenEllipse(int cx, int cy, int radius0, int radius1, RvPoint\[\] pointArray)

public static int GenEllipse(float cx, float cy, float radius0, float radius1, RvPointF32\[\] pointArray)

public static int GenEllipse(double cx, double cy, double radius0, double radius1, RvPointF64\[\] pointArray)

**Description**: Generates a sequence of points on an ellipse, writing to the array.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| cx, cy | int / float / double | Ellipse center |
| radius0 | int / float / double | X-direction semi-axis |
| radius1 | int / float / double | Y-direction semi-axis |
| pointArray | RvPoint\[\] / RvPointF32\[\] / RvPointF64\[\] | Output point array |

**Return Value**: int — actual number of points generated.

### GetArcCenter

csharp

public static bool GetArcCenter(RvPoint p0, RvPoint p1, RvPoint p2, out RvPoint pCenter, out int pRadius)

public static bool GetArcCenter(RvPointF32 p0, RvPointF32 p1, RvPointF32 p2, out RvPointF32 pCenter, out float pRadius)

public static bool GetArcCenter(RvPointF64 p0, RvPointF64 p1, RvPointF64 p2, out RvPointF64 pCenter, out double pRadius)

**Description**: Computes the center and radius of an arc from three points on it.

**Parameters**

| Parameter  | Type                             | Description             |
|------------|----------------------------------|-------------------------|
| p0, p1, p2 | Corresponding point type         | Three points on the arc |
| pCenter    | out corresponding point type     | Output center           |
| pRadius    | out int / out float / out double | Output radius           |

**Return Value**: bool — whether the computation succeeded (returns false when the three points are collinear or coincident).

### IsAdequateArc

csharp

public static bool IsAdequateArc(RvPoint p1, RvPoint p2, RvPoint p3)

public static bool IsAdequateArc(RvPointF32 p1, RvPointF32 p2, RvPointF32 p3)

public static bool IsAdequateArc(RvPointF64 p1, RvPointF64 p2, RvPointF64 p3)

**Description**: Determines whether three points are suitable for determining an arc (non-collinear, non-coincident).

**Parameters**

| Parameter  | Type                     | Description          |
|------------|--------------------------|----------------------|
| p1, p2, p3 | Corresponding point type | Three points to test |

**Return Value**: bool — whether suitable.

### IsPointInsideCircle

csharp

public static RvBool IsPointInsideCircle(RvPointF64 p0, RvPointF64 p1, RvPointF64 p2, RvPointF64 outer)

public static RvBool IsPointInsideCircle(RvPointF64 center, double radius, RvPointF64 outer)

public static RvBool IsPointInsideCircle(RvPointF64 center, double rx, double ry, RvPointF64 outer)

**Description**: Determines whether a point is inside a circle or ellipse.

**Parameters**

| Parameter  | Type       | Description                       |
|------------|------------|-----------------------------------|
| p0, p1, p2 | RvPointF64 | Circle determined by three points |
| center     | RvPointF64 | Circle center                     |
| radius     | double     | Radius                            |
| rx, ry     | double     | Ellipse semi-axes                 |
| outer      | RvPointF64 | Point to test                     |

**Return Value**: RvBool — three-state boolean (True / False / Fuzzy).

## 3. Rectangle and Box

### Box2dToRect / Box2dToRectF32 / Box2dToRectF64

csharp

public static RvRect Box2dToRect(RvBox2D box)

public static RvRectF32 Box2dToRectF32(RvBox2D box)

public static RvRectF64 Box2dToRectF64(RvBox2D box)

**Description**: Converts an RvBox2D rotated rectangle to an axis-aligned bounding rectangle.

**Parameters**

| Parameter | Type    | Description       |
|-----------|---------|-------------------|
| box       | RvBox2D | Rotated rectangle |

**Return Value**: RvRect / RvRectF32 / RvRectF64 — axis-aligned bounding rectangle.

### Box2dToVertex

csharp

public static int Box2dToVertex(RvBox2D box, RvPoint\[\] pointArray)

public static int Box2dToVertex(RvBox2D box, RvPointF32\[\] pointArray)

public static int Box2dToVertex(RvBox2D box, RvPointF64\[\] pointArray)

**Description**: Converts a rotated rectangle to four vertices, writing to the array.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| box | RvBox2D | Rotated rectangle |
| pointArray | RvPoint\[\] / RvPointF32\[\] / RvPointF64\[\] | Output vertex array, length at least 4 |

**Return Value**: int — actual number of vertices written.

### CompareRect

csharp

public static int CompareRect(RvRect rect0, RvRect rect1)

public static int CompareRect(RvRectF32 rect0, RvRectF32 rect1)

public static int CompareRect(RvRectF64 rect0, RvRectF64 rect1)

**Description**: Compares two rectangles.

**Parameters**

| Parameter    | Type                         | Description                   |
|--------------|------------------------------|-------------------------------|
| rect0, rect1 | Corresponding rectangle type | The two rectangles to compare |

**Return Value**: int — comparison result.

### GetRectCenter

csharp

public static RvPoint GetRectCenter(RvRect rect)

public static RvPointF32 GetRectCenter(RvRectF32 rect)

public static RvPointF64 GetRectCenter(RvRectF64 rect)

**Description**: Gets the rectangle center point.

**Parameters**

| Parameter | Type                         | Description      |
|-----------|------------------------------|------------------|
| rect      | Corresponding rectangle type | Target rectangle |

**Return Value**: Corresponding point type — rectangle center.

### GetRectSize

csharp

public static RvSize GetRectSize(RvRect rect, bool bIncludeBorder = false)

public static RvSizeF32 GetRectSize(RvRectF32 rect, bool bIncludeBorder = false)

public static RvSizeF64 GetRectSize(RvRectF64 rect, bool bIncludeBorder = false)

**Description**: Gets the rectangle size.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| rect | Corresponding rectangle type | Target rectangle |
| bIncludeBorder | bool | Whether to include border pixels, default false |

**Return Value**: Corresponding size type — rectangle width and height.

### GetRectWidth / GetRectHeight

csharp

public static int GetRectWidth(RvRect rect, bool bIncludeBorder = false)

public static int GetRectWidth(RvRectF32 rect, bool bIncludeBorder = false)

public static int GetRectWidth(RvRectF64 rect, bool bIncludeBorder = false)

public static int GetRectHeight(RvRect rect, bool bIncludeBorder = false)

public static int GetRectHeight(RvRectF32 rect, bool bIncludeBorder = false)

public static int GetRectHeight(RvRectF64 rect, bool bIncludeBorder = false)

**Description**: Gets the rectangle width or height.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| rect | Corresponding rectangle type | Target rectangle |
| bIncludeBorder | bool | Whether to include border pixels |

**Return Value**: int — width or height.

### IsBox2dInRect

csharp

public static bool IsBox2dInRect(RvBox2D box, RvRect rect)

public static bool IsBox2dInRect(RvBox2D box, RvRectF32 rect)

public static bool IsBox2dInRect(RvBox2D box, RvRectF64 rect)

**Description**: Determines whether the rotated rectangle is fully inside the specified rectangle.

**Parameters**

| Parameter | Type                           | Description       |
|-----------|--------------------------------|-------------------|
| box       | RvBox2D                        | Rotated rectangle |
| rect      | RvRect / RvRectF32 / RvRectF64 | Target rectangle  |

**Return Value**: bool — whether fully inside.

### IsNullRect / IsRectEmpty / IsRectEqual

csharp

public static bool IsNullRect(RvRect rect)

public static bool IsNullRect(RvRectF32 rect)

public static bool IsNullRect(RvRectF64 rect)

public static bool IsRectEmpty(RvRect rect)

public static bool IsRectEmpty(RvRectF32 rect)

public static bool IsRectEmpty(RvRectF64 rect)

public static bool IsRectEqual(RvRect rect0, RvRect rect1)

public static bool IsRectEqual(RvRectF32 rect0, RvRectF32 rect1)

public static bool IsRectEqual(RvRectF64 rect0, RvRectF64 rect1)

**Description**: Determines whether a rectangle is null (IsNullRect / IsRectEmpty) or whether two rectangles are equal (IsRectEqual).

**Parameters**

| Parameter           | Type                         | Description         |
|---------------------|------------------------------|---------------------|
| rect / rect0, rect1 | Corresponding rectangle type | Target rectangle(s) |

**Return Value**: bool — the judgment result.

### IsRectSurround

csharp

public static RvBool IsRectSurround(RvRect rect0, RvRect rect1)

public static RvBool IsRectSurround(RvRectF32 rect0, RvRectF32 rect1)

public static RvBool IsRectSurround(RvRectF64 rect0, RvRectF64 rect1)

**Description**: Determines whether rect1 is fully surrounded by rect0.

**Parameters**

| Parameter | Type                         | Description     |
|-----------|------------------------------|-----------------|
| rect0     | Corresponding rectangle type | Outer rectangle |
| rect1     | Corresponding rectangle type | Inner rectangle |

**Return Value**: RvBool — three-state boolean.

### MoveRect

csharp

public static RvRect MoveRect(RvRect rect, int dx, int dy)

public static RvRectF32 MoveRect(RvRectF32 rect, int dx, int dy)

public static RvRectF64 MoveRect(RvRectF64 rect, int dx, int dy)

**Description**: Translates a rectangle.

**Parameters**

| Parameter | Type                         | Description                     |
|-----------|------------------------------|---------------------------------|
| rect      | Corresponding rectangle type | Rectangle to translate          |
| dx, dy    | int                          | Horizontal and vertical offsets |

**Return Value**: Corresponding rectangle type — the translated new rectangle.

### NormalizeRect

csharp

public static void NormalizeRect(ref RvRect pRect)

public static void NormalizeRect(ref RvRectF32 pRect)

public static void NormalizeRect(ref RvRectF64 pRect)

**Description**: Normalizes a rectangle, ensuring left $\leq$ right and top $\leq$ bottom.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| pRect | ref corresponding rectangle type | Rectangle to normalize; modified in place |

**Return Value**: None.

### RectDeflate / RectShrink

csharp

public static void RectDeflate(ref RvRect pRect, int dx, int dy)

public static void RectDeflate(ref RvRectF32 pRect, int dx, int dy)

public static void RectDeflate(ref RvRectF64 pRect, int dx, int dy)

public static void RectShrink(ref RvRect pRect, int dx, int dy)

public static void RectShrink(ref RvRectF32 pRect, int dx, int dy)

public static void RectShrink(ref RvRectF64 pRect, int dx, int dy)

**Description**: Shrinks a rectangle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| pRect | ref corresponding rectangle type | Rectangle to shrink; modified in place |
| dx, dy | int | Horizontal and vertical shrink amounts |

**Return Value**: None.

### RectIntersect / RectUnion

csharp

public static RvRect RectIntersect(RvRect rect, RvRect other)

public static RvRectF32 RectIntersect(RvRectF32 rect, RvRectF32 other)

public static RvRectF64 RectIntersect(RvRectF64 rect, RvRectF64 other)

public static RvRect RectUnion(RvRect rect, RvRect other)

public static RvRectF32 RectUnion(RvRectF32 rect, RvRectF32 other)

public static RvRectF64 RectUnion(RvRectF64 rect, RvRectF64 other)

**Description**: Computes the intersection or union of two rectangles.

**Parameters**

| Parameter   | Type                         | Description        |
|-------------|------------------------------|--------------------|
| rect, other | Corresponding rectangle type | The two rectangles |

**Return Value**: Corresponding rectangle type — the intersection or union result.

### RectToVertex

csharp

public static int RectToVertex(RvRect rect, RvPoint\[\] pointArray)

public static int RectToVertex(RvRect rect, RvPointF32\[\] pointArray)

public static int RectToVertex(RvRect rect, RvPointF64\[\] pointArray)

**Description**: Converts a rectangle to four vertices, writing to the array.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| rect | RvRect | Target rectangle |
| pointArray | Corresponding point type array | Output vertex array, length at least 4 |

**Return Value**: int — actual number of vertices written.

### RotateRect

csharp

public static RvRect RotateRect(RvRect rect, double angle)

public static RvRectF32 RotateRect(RvRectF32 rect, double angle)

public static RvRectF64 RotateRect(RvRectF64 rect, double angle)

**Description**: Rotates a rectangle around its center, returning the axis-aligned bounding rectangle.

**Parameters**

| Parameter | Type                         | Description              |
|-----------|------------------------------|--------------------------|
| rect      | Corresponding rectangle type | Rectangle to rotate      |
| angle     | double                       | Rotation angle (degrees) |

**Return Value**: Corresponding rectangle type — the rotated axis-aligned bounding rectangle.

### ScaleRect / ScaleRectE2

csharp

public static RvRect ScaleRect(RvRect rect, double scale)

public static RvRect ScaleRect(RvRect rect, double scaleX, double scaleY)

public static RvRect ScaleRectE2(RvRect rect, double scale)

**Description**: Scales a rectangle proportionally.

**Parameters**

| Parameter      | Type   | Description                             |
|----------------|--------|-----------------------------------------|
| rect           | RvRect | Rectangle to scale                      |
| scale          | double | Scaling factor                          |
| scaleX, scaleY | double | Horizontal and vertical scaling factors |

**Return Value**: RvRect — the scaled new rectangle.

## 4. Point and Line

### Calc3PAngle

csharp

public static double Calc3PAngle(RvPoint oripos, RvPoint start, RvPoint end, bool bDegree)

public static double Calc3PAngle(RvPointF32 oripos, RvPointF32 start, RvPointF32 end, bool bDegree)

public static double Calc3PAngle(RvPointF64 oripos, RvPointF64 start, RvPointF64 end, bool bDegree)

**Description**: Computes the angle formed by three points, with oripos as the vertex.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| oripos | Corresponding point type | Vertex of the angle |
| start, end | Corresponding point type | Endpoints of the two sides |
| bDegree | bool | true returns degrees, false returns radians |

**Return Value**: double — the angle value.

### CalcIntersection

csharp

public static bool CalcIntersection(RvLine line0, RvLine line1, out RvPoint pResult)

public static bool CalcIntersection(RvLineF32 line0, RvLineF32 line1, out RvPointF32 pResult)

public static bool CalcIntersection(RvLineF64 line0, RvLineF64 line1, out RvPointF64 pResult)

**Description**: Computes the intersection of two lines.

**Parameters**

| Parameter    | Type                         | Description         |
|--------------|------------------------------|---------------------|
| line0, line1 | Corresponding line type      | The two lines       |
| pResult      | out corresponding point type | Output intersection |

**Return Value**: bool — whether they intersect.

### CalcLineAngle

csharp

public static double CalcLineAngle(RvLine line0, RvLine line1, bool bDegree)

public static double CalcLineAngle(RvLineF32 line0, RvLineF32 line1, bool bDegree)

public static double CalcLineAngle(RvLineF64 line0, RvLineF64 line1, bool bDegree)

**Description**: Computes the angle between two lines.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| line0, line1 | Corresponding line type | The two lines |
| bDegree | bool | true returns degrees, false returns radians |

**Return Value**: double — the angle value.

### CalcPerpendicularPoint

csharp

public static bool CalcPerpendicularPoint(RvLine line, RvPoint outer, out RvPoint pResult)

public static bool CalcPerpendicularPoint(RvLineF32 line, RvPointF32 outer, out RvPointF32 pResult)

public static bool CalcPerpendicularPoint(RvLineF64 line, RvPointF64 outer, out RvPointF64 pResult)

**Description**: Computes the perpendicular foot from point outer to line line.

**Parameters**

| Parameter | Type                         | Description               |
|-----------|------------------------------|---------------------------|
| line      | Corresponding line type      | Target line               |
| outer     | Corresponding point type     | Point outside the line    |
| pResult   | out corresponding point type | Output perpendicular foot |

**Return Value**: bool — whether the computation succeeded.

### CalcPointAlongLine

csharp

public static RvPoint CalcPointAlongLine(RvPoint start, RvPoint end, int dist)

public static RvPointF32 CalcPointAlongLine(RvPointF32 start, RvPointF32 end, float dist)

public static RvPointF64 CalcPointAlongLine(RvPointF64 start, RvPointF64 end, double dist)

**Description**: Advances dist along the segment direction from the start point, returning the point coordinates.

**Parameters**

| Parameter  | Type                     | Description                      |
|------------|--------------------------|----------------------------------|
| start, end | Corresponding point type | The two endpoints of the segment |
| dist       | int / float / double     | Advance distance                 |

**Return Value**: Corresponding point type — the point coordinates after advancing.

### CalcPointToLineDist

csharp

public static int CalcPointToLineDist(RvPoint p0, RvPoint p1, RvPoint outer, bool bUprightDist)

public static float CalcPointToLineDist(RvPointF32 p0, RvPointF32 p1, RvPointF32 outer, bool bUprightDist)

public static double CalcPointToLineDist(RvPointF64 p0, RvPointF64 p1, RvPointF64 outer, bool bUprightDist)

**Description**: Computes the distance from a point to a line.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| p0, p1 | Corresponding point type | Two points on the line |
| outer | Corresponding point type | Point outside the line |
| bUprightDist | bool | true returns the perpendicular distance |

**Return Value**: int / float / double — the distance value.

### DeriveMidPoint

csharp

public static RvPoint DeriveMidPoint(RvPoint p0, RvPoint p1)

public static RvPointF32 DeriveMidPoint(RvPointF32 p0, RvPointF32 p1)

public static RvPointF64 DeriveMidPoint(RvPointF64 p0, RvPointF64 p1)

**Description**: Computes the midpoint of two points.

**Parameters**

| Parameter | Type                     | Description       |
|-----------|--------------------------|-------------------|
| p0, p1    | Corresponding point type | The two endpoints |

**Return Value**: Corresponding point type — midpoint coordinates.

### DeriveParallel

csharp

public static RvLine DeriveParallel(RvLine line, int distance)

public static RvLineF32 DeriveParallel(RvLineF32 line, float distance)

public static RvLineF64 DeriveParallel(RvLineF64 line, double distance)

**Description**: Derives a line parallel to the specified line at the given distance.

**Parameters**

| Parameter | Type                    | Description            |
|-----------|-------------------------|------------------------|
| line      | Corresponding line type | Original line          |
| distance  | int / float / double    | Parallel line distance |

**Return Value**: Corresponding line type — the derived parallel line.

### DeriveParallPoint

csharp

public static RvPoint DeriveParallPoint(RvPoint start, RvPoint end, RvPoint outer)

public static RvPointF32 DeriveParallPoint(RvPointF32 start, RvPointF32 end, RvPointF32 outer)

public static RvPointF64 DeriveParallPoint(RvPointF64 start, RvPointF64 end, RvPointF64 outer)

**Description**: Finds the projection of outer onto a parallel line along the direction parallel to start→end.

**Parameters**

| Parameter  | Type                     | Description                            |
|------------|--------------------------|----------------------------------------|
| start, end | Corresponding point type | Two endpoints of the reference segment |
| outer      | Corresponding point type | Point to project                       |

**Return Value**: Corresponding point type — projected point coordinates.

### DerivePerpend

csharp

public static RvLine DerivePerpend(RvLine line, int length, bool bFullSize, bool bStartPos)

public static RvLineF32 DerivePerpend(RvLineF32 line, float length, bool bFullSize, bool bStartPos)

public static RvLineF64 DerivePerpend(RvLineF64 line, double length, bool bFullSize, bool bStartPos)

**Description**: Derives a perpendicular line from the specified line.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| line | Corresponding line type | Original line |
| length | int / float / double | Perpendicular length |
| bFullSize | bool | Whether to extend length/2 to both sides with a certain point as the midpoint |
| bStartPos | bool | Whether it passes through the start point (false = end point) |

**Return Value**: Corresponding line type — the derived perpendicular line.

### DistLineToLine

csharp

public static double DistLineToLine(RvLine line1, RvLine line2)

public static double DistLineToLine(RvLineF32 line1, RvLineF32 line2)

public static double DistLineToLine(RvLineF64 line1, RvLineF64 line2)

**Description**: Computes the distance between two lines.

**Parameters**

| Parameter    | Type                    | Description   |
|--------------|-------------------------|---------------|
| line1, line2 | Corresponding line type | The two lines |

**Return Value**: double — distance value (gap when parallel; typically 0 when intersecting).

### DistPointToLine

csharp

public static double DistPointToLine(RvPoint point, RvLine line)

public static double DistPointToLine(RvPointF32 point, RvLineF32 line)

public static double DistPointToLine(RvPointF64 point, RvLineF64 line)

**Description**: Computes the distance from a point to a line.

**Parameters**

| Parameter | Type                     | Description      |
|-----------|--------------------------|------------------|
| point     | Corresponding point type | Point to measure |
| line      | Corresponding line type  | Target line      |

**Return Value**: double — distance value.

### DistPointToPoint

csharp

public static double DistPointToPoint(RvPoint point0, RvPoint point1)

public static double DistPointToPoint(RvPointF32 point0, RvPointF32 point1)

public static double DistPointToPoint(RvPointF64 point0, RvPointF64 point1)

**Description**: Computes the distance between two points.

**Parameters**

| Parameter      | Type                     | Description    |
|----------------|--------------------------|----------------|
| point0, point1 | Corresponding point type | The two points |

**Return Value**: double — distance value.

### ExtendLine

csharp

public static RvLine ExtendLine(RvLine line, int length)

public static RvLineF32 ExtendLine(RvLineF32 line, float length)

public static RvLineF64 ExtendLine(RvLineF64 line, double length)

**Description**: Extends or shortens a segment by the specified length, returning a new segment.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| line | Corresponding line type | Original segment |
| length | int / float / double | Extension length (positive = extend, negative = shorten) |

**Return Value**: Corresponding line type — the new segment.

### GetNormalAngle / GetNormalAngleE1 / GetNormalAngleE2

csharp

public static double GetNormalAngle(double x0, double y0, double x1, double y1, bool bDegree = false)

public static double GetNormalAngleE1(double y, double x, bool bDegree = false)

public static float GetNormalAngleE2(float y, float x, bool bDegree = false)

**Description**: Computes the normal angle of the line connecting two points.

**Parameters**

| Parameter      | Type           | Description                                 |
|----------------|----------------|---------------------------------------------|
| x0, y0, x1, y1 | double         | Coordinates of the two points               |
| x, y           | double / float | Components (E1 / E2)                        |
| bDegree        | bool           | true returns degrees, false returns radians |

**Return Value**: double / float — the normal angle.

### IsRightOfLine

csharp

public static RvBool IsRightOfLine(RvLine line, RvPoint outer)

public static RvBool IsRightOfLine(RvLineF32 line, RvPointF32 outer)

public static RvBool IsRightOfLine(RvLineF64 line, RvPointF64 outer)

**Description**: Determines whether a point is on the right or left side of a line.

**Parameters**

| Parameter | Type                     | Description    |
|-----------|--------------------------|----------------|
| line      | Corresponding line type  | Reference line |
| outer     | Corresponding point type | Point to test  |

**Return Value**: RvBool — three-state boolean (True = right, False = left).

### RotateLine

csharp

public static RvLine RotateLine(RvLine line, RvPoint center, float angle)

public static RvLineF32 RotateLine(RvLineF32 line, RvPointF32 center, float angle)

public static RvLineF64 RotateLine(RvLineF64 line, RvPointF64 center, double angle)

**Description**: Rotates a segment around the specified center, returning a new segment.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| line | Corresponding line type | Segment to rotate |
| center | Corresponding point type | Rotation center |
| angle | float / double | Rotation angle (degrees); positive = clockwise |

**Return Value**: Corresponding line type — the rotated new segment.

### RotatePoint

csharp

public static RvPoint RotatePoint(RvPoint point, int x, int y, float angle)

public static RvPointF32 RotatePoint(RvPointF32 point, float x, float y, float angle)

public static RvPointF64 RotatePoint(RvPointF64 point, double x, double y, double angle)

**Description**: Rotates a point around (x, y), returning a new point.

**Parameters**

| Parameter | Type                       | Description              |
|-----------|----------------------------|--------------------------|
| point     | Corresponding point type   | Point to rotate          |
| x, y      | Corresponding numeric type | Rotation center          |
| angle     | float / double             | Rotation angle (degrees) |

**Return Value**: Corresponding point type — the rotated new point.

### TrimLine

csharp

public static RvLine TrimLine(RvLine line, RvLine @base, int index)

public static RvLineF32 TrimLine(RvLineF32 line, RvLineF32 @base, int index)

public static RvLineF64 TrimLine(RvLineF64 line, RvLineF64 @base, int index)

**Description**: Truncates a segment at the baseline; index controls which part is retained.

**Parameters**

| Parameter | Type                    | Description            |
|-----------|-------------------------|------------------------|
| line      | Corresponding line type | Segment to truncate    |
| base      | Corresponding line type | Baseline               |
| index     | int                     | Retained segment index |

**Return Value**: Corresponding line type — the truncated new segment.

## 5. Polygon and Polyline

### ApproxLine

csharp

public static int ApproxLine(RvPoint\[\] srcSet, RvPoint\[\] destSet)

public static int ApproxLine(RvPointF32\[\] srcSet, RvPointF32\[\] destSet)

public static int ApproxLine(RvPointF64\[\] srcSet, RvPointF64\[\] destSet)

**Description**: Performs line approximation on a point set, writing results to the target array.

**Parameters**

| Parameter | Type                           | Description      |
|-----------|--------------------------------|------------------|
| srcSet    | Corresponding point type array | Source point set |
| destSet   | Corresponding point type array | Output point set |

**Return Value**: int — actual output point count.

### CalcBoundRect

csharp

public static RvRect CalcBoundRect(RvPoint\[\] vertices)

public static RvRectF32 CalcBoundRect(RvPointF32\[\] vertices)

public static RvRectF64 CalcBoundRect(RvPointF64\[\] vertices)

**Description**: Computes the axis-aligned bounding rectangle of a point set.

**Parameters**

| Parameter | Type                           | Description  |
|-----------|--------------------------------|--------------|
| vertices  | Corresponding point type array | Vertex array |

**Return Value**: Corresponding rectangle type — bounding rectangle.

### CalcPointToPolylineDistance

csharp

public static int CalcPointToPolylineDistance(RvPoint point, RvPoint\[\] vertices, bool bClosed)

public static int CalcPointToPolylineDistance(RvPointF32 point, RvPointF32\[\] vertices, bool bClosed)

public static int CalcPointToPolylineDistance(RvPointF64 point, RvPointF64\[\] vertices, bool bClosed)

**Description**: Computes the shortest distance from a point to a polyline.

**Parameters**

| Parameter | Type                           | Description           |
|-----------|--------------------------------|-----------------------|
| point     | Corresponding point type       | Point to measure      |
| vertices  | Corresponding point type array | Polyline vertex array |
| bClosed   | bool                           | Whether closed        |

**Return Value**: int — shortest distance.

### CalcPolygonArea

csharp

public static double CalcPolygonArea(RvPoint\[\] vertices)

public static double CalcPolygonArea(RvPointF32\[\] vertices)

public static double CalcPolygonArea(RvPointF64\[\] vertices)

**Description**: Computes the polygon area.

**Parameters**

| Parameter | Type                           | Description          |
|-----------|--------------------------------|----------------------|
| vertices  | Corresponding point type array | Polygon vertex array |

**Return Value**: double — area value (may be negative if vertex order is inconsistent).

### CalcPolygonCenter

csharp

public static RvPoint CalcPolygonCenter(RvPoint\[\] vertices)

public static RvPointF32 CalcPolygonCenter(RvPointF32\[\] vertices)

public static RvPointF64 CalcPolygonCenter(RvPointF64\[\] vertices)

**Description**: Computes the polygon center point.

**Parameters**

| Parameter | Type                           | Description          |
|-----------|--------------------------------|----------------------|
| vertices  | Corresponding point type array | Polygon vertex array |

**Return Value**: Corresponding point type — polygon center.

### CalcPolylineLength

csharp

public static double CalcPolylineLength(RvPoint\[\] vertices, bool bClosed)

public static double CalcPolylineLength(RvPointF32\[\] vertices, bool bClosed)

public static double CalcPolylineLength(RvPointF64\[\] vertices, bool bClosed)

**Description**: Computes the polyline length.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| vertices | Corresponding point type array | Polyline vertex array |
| bClosed | bool | When true, includes the closing edge |

**Return Value**: double — total polyline length.

### GetSubPolygonPerimeter

csharp

public static double GetSubPolygonPerimeter(RvPoint\[\] arrIn, int startIndex, int endIndex)

public static double GetSubPolygonPerimeter(RvPointF32\[\] arrIn, int startIndex, int endIndex)

public static double GetSubPolygonPerimeter(RvPointF64\[\] arrIn, int startIndex, int endIndex)

**Description**: Computes the perimeter of the sub-polygon.

**Parameters**

| Parameter  | Type                           | Description          |
|------------|--------------------------------|----------------------|
| arrIn      | Corresponding point type array | Polygon vertex array |
| startIndex | int                            | Start index          |
| endIndex   | int                            | End index            |

**Return Value**: double — sub-polygon perimeter.

### GetSubPolygonVertex

csharp

public static int GetSubPolygonVertex(RvPoint\[\] arrIn, int startIndex, int endIndex, RvPoint\[\] arrOut)

public static int GetSubPolygonVertex(RvPointF32\[\] arrIn, int startIndex, int endIndex, RvPointF32\[\] arrOut)

public static int GetSubPolygonVertex(RvPointF64\[\] arrIn, int startIndex, int endIndex, RvPointF64\[\] arrOut)

**Description**: Extracts the sub-polygon vertices from startIndex to endIndex.

**Parameters**

| Parameter  | Type                           | Description                 |
|------------|--------------------------------|-----------------------------|
| arrIn      | Corresponding point type array | Source polygon vertices     |
| startIndex | int                            | Start index                 |
| endIndex   | int                            | End index                   |
| arrOut     | Corresponding point type array | Output sub-polygon vertices |

**Return Value**: int — actual output vertex count.

### IsPointInPolygon

csharp

public static bool IsPointInPolygon(RvPoint point, RvPoint\[\] polygon)

public static bool IsPointInPolygon(RvPointF32 point, RvPointF32\[\] polygon)

public static bool IsPointInPolygon(RvPointF64 point, RvPointF64\[\] polygon)

**Description**: Determines whether a point is inside a polygon.

**Parameters**

| Parameter | Type                           | Description          |
|-----------|--------------------------------|----------------------|
| point     | Corresponding point type       | Point to test        |
| polygon   | Corresponding point type array | Polygon vertex array |

**Return Value**: bool — whether inside.

### IsPointInPolygonRing

csharp

public static bool IsPointInPolygonRing(RvPoint point, RvPoint\[\] innerArray, RvPoint\[\] outerArray)

public static bool IsPointInPolygonRing(RvPointF32 point, RvPointF32\[\] innerArray, RvPointF32\[\] outerArray)

public static bool IsPointInPolygonRing(RvPointF64 point, RvPointF64\[\] innerArray, RvPointF64\[\] outerArray)

**Description**: Determines whether a point is inside a polygon ring with a hole (inside the outer polygon and outside the inner polygon).

**Parameters**

| Parameter  | Type                           | Description            |
|------------|--------------------------------|------------------------|
| point      | Corresponding point type       | Point to test          |
| innerArray | Corresponding point type array | Inner polygon vertices |
| outerArray | Corresponding point type array | Outer polygon vertices |

**Return Value**: bool — whether inside the ring.

### IsPointOnPolygonBorder

csharp

public static bool IsPointOnPolygonBorder(RvPoint point, RvPoint\[\] vertexArray)

public static bool IsPointOnPolygonBorder(RvPointF32 point, RvPointF32\[\] vertexArray)

public static bool IsPointOnPolygonBorder(RvPointF64 point, RvPointF64\[\] vertexArray)

**Description**: Determines whether a point lies on the polygon boundary.

**Parameters**

| Parameter   | Type                           | Description          |
|-------------|--------------------------------|----------------------|
| point       | Corresponding point type       | Point to test        |
| vertexArray | Corresponding point type array | Polygon vertex array |

**Return Value**: bool — whether on the boundary.

### IsPointOnPolyline

csharp

public static bool IsPointOnPolyline(RvPoint point, RvPoint\[\] vertexArray)

public static bool IsPointOnPolyline(RvPointF32 point, RvPointF32\[\] vertexArray)

public static bool IsPointOnPolyline(RvPointF64 point, RvPointF64\[\] vertexArray)

**Description**: Determines whether a point lies on a polyline.

**Parameters**

| Parameter   | Type                           | Description           |
|-------------|--------------------------------|-----------------------|
| point       | Corresponding point type       | Point to test         |
| vertexArray | Corresponding point type array | Polyline vertex array |

**Return Value**: bool — whether on the polyline.

### IsSubPolygon

csharp

public static bool IsSubPolygon(RvPoint\[\] bigArr, RvPoint\[\] smallArr, bool bIncludeBorder)

public static bool IsSubPolygon(RvPointF32\[\] bigArr, RvPointF32\[\] smallArr, bool bIncludeBorder)

public static bool IsSubPolygon(RvPointF64\[\] bigArr, RvPointF64\[\] smallArr, bool bIncludeBorder)

**Description**: Determines whether the small polygon is fully inside the large polygon.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| bigArr | Corresponding point type array | Large polygon vertices |
| smallArr | Corresponding point type array | Small polygon vertices |
| bIncludeBorder | bool | Whether the boundary counts as inside |

**Return Value**: bool — whether it is a sub-polygon.

### PolygonOffset

csharp

public static RvPoint\[\] PolygonOffset(RvPoint\[\] vertexArray, int distance)

public static RvPointF32\[\] PolygonOffset(RvPointF32\[\] vertexArray, float distance)

public static RvPointF64\[\] PolygonOffset(RvPointF64\[\] vertexArray, double distance)

**Description**: Offsets the polygon outward (positive) or inward (negative) by the specified distance, returning a new vertex array.

**Parameters**

| Parameter   | Type                           | Description          |
|-------------|--------------------------------|----------------------|
| vertexArray | Corresponding point type array | Polygon vertex array |
| distance    | int / float / double           | Offset distance      |

**Return Value**: Corresponding point type array — the offset new vertex array.

### PolylineMove

csharp

public static void PolylineMove(RvPoint\[\] vertices, int dx, int dy)

public static void PolylineMove(RvPointF32\[\] vertices, float dx, float dy)

public static void PolylineMove(RvPointF64\[\] vertices, double dx, double dy)

**Description**: Translates the polyline as a whole, modifying in place.

**Parameters**

| Parameter | Type                           | Description                     |
|-----------|--------------------------------|---------------------------------|
| vertices  | Corresponding point type array | Polyline vertex array           |
| dx, dy    | Corresponding numeric type     | Horizontal and vertical offsets |

**Return Value**: None.

## 6. Vertex Operations

### RotateVertex

csharp

public static void RotateVertex(RvPoint\[\] vertices, int x, int y, float angle)

public static void RotateVertex(RvPointF32\[\] vertices, float x, float y, float angle)

public static void RotateVertex(RvPointF64\[\] vertices, double x, double y, double angle)

**Description**: Rotates a vertex array as a whole around (x, y), modifying in place.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| vertices | Corresponding point type array | Vertex array |
| x, y | Corresponding numeric type | Rotation center |
| angle | float / double | Rotation angle (degrees); positive = clockwise |

**Return Value**: None.

### ScaleVertex

csharp

public static void ScaleVertex(RvPoint\[\] vertices, float scaleX, float scaleY)

public static void ScaleVertex(RvPointF32\[\] vertices, float scaleX, float scaleY)

public static void ScaleVertex(RvPointF64\[\] vertices, double scaleX, double scaleY)

**Description**: Scales a vertex array proportionally, modifying in place.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| vertices | Corresponding point type array | Vertex array |
| scaleX, scaleY | float / double | Horizontal and vertical scaling factors |

**Return Value**: None.

## 7. Pixel Helpers

### CalcDepth

csharp

public static int CalcDepth(PixelFormat type)

**Description**: Computes bytes per pixel for the specified pixel format.

**Parameters**

| Parameter | Type        | Description  |
|-----------|-------------|--------------|
| type      | PixelFormat | Pixel format |

**Return Value**: int — bytes per pixel.

### CalcPitch

csharp

public static int CalcPitch(int pixbits, int width)

**Description**: Computes bytes per row (row stride).

**Parameters**

| Parameter | Type | Description    |
|-----------|------|----------------|
| pixbits   | int  | Bits per pixel |
| width     | int  | Image width    |

**Return Value**: int — bytes per row.

### CalcPixelStrength

csharp

public static int CalcPixelStrength(RvRgb color, ColorSpace method)

**Description**: Computes pixel strength in the specified color space.

**Parameters**

| Parameter | Type       | Description                           |
|-----------|------------|---------------------------------------|
| color     | RvRgb      | Pixel color                           |
| method    | ColorSpace | Color space: Default, HSV, YCbCr, YUV |

**Return Value**: int — strength value.

### GetBrighterPixel / GetDarkerPixel

csharp

public static RvRgb GetBrighterPixel(RvRgb color, float percent)

public static RvRgb GetDarkerPixel(RvRgb color, float percent)

**Description**: Brightens or darkens a color proportionally.

**Parameters**

| Parameter | Type  | Description    |
|-----------|-------|----------------|
| color     | RvRgb | Original color |
| percent   | float | Ratio (0~1)    |

**Return Value**: RvRgb — the adjusted new color.

### CalcOtsu

csharp

public static int CalcOtsu(int maxValue, int\[\] magArr)

**Description**: Computes the Otsu threshold from a grayscale histogram.

**Parameters**

| Parameter | Type    | Description               |
|-----------|---------|---------------------------|
| maxValue  | int     | Maximum grayscale value   |
| magArr    | int\[\] | Grayscale histogram array |

**Return Value**: int — the computed threshold.

## 8. MIP Scaling

### GetMipUpScale / GetMipUpScaleF / GetMipUpScaleD

csharp

public static int GetMipUpScale(int level)

public static float GetMipUpScaleF(int level)

public static double GetMipUpScaleD(int level)

**Description**: Gets the upsampling ratio corresponding to a MIP level.

**Parameters**

| Parameter | Type | Description |
|-----------|------|-------------|
| level     | int  | MIP level   |

**Return Value**: int / float / double — upsampling ratio.

### GetMipDownScale / GetMipDownScaleF / GetMipDownScaleD

csharp

public static float GetMipDownScale(int level)

public static float GetMipDownScaleF(int level)

public static double GetMipDownScaleD(int level)

**Description**: Gets the downsampling ratio corresponding to a MIP level.

**Parameters**

| Parameter | Type | Description |
|-----------|------|-------------|
| level     | int  | MIP level   |

**Return Value**: float / double — downsampling ratio.

### GetMipScale / GetMipScaleF / GetMipScaleD

csharp

public static float GetMipScale(int level, bool bOpposite = false)

public static float GetMipScaleF(int level, bool bOpposite = false)

public static double GetMipScaleD(int level, bool bOpposite = false)

**Description**: Gets the scaling ratio corresponding to a MIP level.

**Parameters**

| Parameter | Type | Description                       |
|-----------|------|-----------------------------------|
| level     | int  | MIP level                         |
| bOpposite | bool | Whether to take the inverse ratio |

**Return Value**: float / double — scaling ratio.

## 9. Others

### Power

csharp

public static int Power(int @base, int index)

public static float Power(float @base, int index)

public static double Power(double @base, int index)

**Description**: Power operation.

**Parameters**

| Parameter | Type                 | Description |
|-----------|----------------------|-------------|
| @base     | int / float / double | Base        |
| index     | int                  | Exponent    |

**Return Value**: Corresponding numeric type — base^index.

### IsClockWise

csharp

public static RvBool IsClockWise(RvPoint start, RvPoint mid, RvPoint end)

public static RvBool IsClockWise(RvPointF32 start, RvPointF32 mid, RvPointF32 end)

public static RvBool IsClockWise(RvPointF64 start, RvPointF64 mid, RvPointF64 end)

**Description**: Determines the arrangement direction of three points on an arc.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| start, mid, end | Corresponding point type | Three points on the arc, in arc order |

**Return Value**: RvBool — three-state boolean (True = clockwise, False = counter-clockwise).

### IsPointInsideRect

csharp

public static RvBool IsPointInsideRect(RvRect rect, RvPoint pos, bool bOnBorderOnly = false)

public static RvBool IsPointInsideRect(RvRectF32 rect, RvPointF32 pos, bool bOnBorderOnly = false)

public static RvBool IsPointInsideRect(RvRectF64 rect, RvPointF64 pos, bool bOnBorderOnly = false)

**Description**: Determines whether a point is inside a rectangle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| rect | Corresponding rectangle type | Target rectangle |
| pos | Corresponding point type | Point to test |
| bOnBorderOnly | bool | true tests only whether it is on the boundary |

**Return Value**: RvBool — three-state boolean.
