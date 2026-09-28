---
title: "Graphics and Image Drawing"
description: "Overview of the Render class and the OpenGL-based drawing module of KVI."
keywords: ["KVI", "Render", "drawing", "OpenGL", "stroke model"]
---

# Graphics and Image Drawing

## Overview

The `Kingpool.Xgui` namespace provides graphics and image drawing interfaces, with the core class `Render`. This module uses **OpenGL** as the underlying graphics interface, bypassing GDI/GDI+ and interacting directly with the graphics card.

The module adopts a **stroke model**: each drawing call returns a stroke handle that encapsulates all drawing information. Handles can be saved, modified, hidden, deleted, combined, and split.

## Capabilities

- **Context management**: Create and destroy the drawing context bound to a window handle
- **Canvas settings**: Size, depth, background color, background style
- **View control**: Display mode, scale, position, visible region
- **Coordinate conversion**: Window ↔ canvas coordinate conversion
- **Drawing resources**: Pens, brushes, fonts, molds
- **Primitive drawing**: Points, lines, polylines, rectangles, circles, ellipses, arcs, crosses, triangles, polygons, text, images
- **Stroke management**: Dummy stroke, erase, hide, modify, offset, combine, uncombine
- **Clearing and refreshing**: Clear layers, realize, flush
- **Export**: Export canvas to an image
- **Logo and appearance**: RVB LOGO visibility and placement, transparency

## Key Concepts

### Stroke Model

Each drawing call returns an `IntPtr` stroke handle. Passing an existing handle reuses the container; passing `IntPtr.Zero` creates a new one.

### Canvas Layers

The canvas consists of four layers, from bottom to top:

1. Image frame layer
2. Canvas layer
3. Intermediate layer
4. Screen layer

## Detailed Reference

For method signatures, parameters, return values, and examples, see:

- [Render Class Function Usage Guide](RenderClassFunctionUsageGuide.md)

## Related Documentation

- [Basic Data Structures](BasicDataStructures.md) — `GRgb`, `GPoint`, `GRect`, `RvBox2D`