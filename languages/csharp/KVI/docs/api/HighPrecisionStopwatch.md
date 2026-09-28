---
title: "High-Precision Stopwatch"
description: "Overview of the StopWatch class for high-precision timing in KVI."
keywords: ["KVI", "StopWatch", "timer", "performance"]
---

# High-Precision Stopwatch

## Overview

The `StopWatch` class wraps the high-precision stopwatch in the native library `UniSupport.dll`, used to measure code segment execution time. Compared with `DateTime`, it typically provides higher timing precision.

## Capabilities

- Creates anonymous or named stopwatches
- Starts, stops, and restarts timing
- Returns elapsed time in seconds (double)
- Supports continuous multi-stage measurement

## Detailed Reference

For constructors, methods, properties, and examples, see:

- [StopWatch Class Usage Guide](StopWatchClassUsageGuide.md)

## Related Documentation

- [Clock Class Usage Guide](ClockClassUsageGuide.md) — lightweight timer based on `DateTime.Now`