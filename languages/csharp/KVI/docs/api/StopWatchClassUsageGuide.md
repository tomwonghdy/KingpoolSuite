# StopWatch Class Detailed Usage Guide

## Overview

StopWatch is a high-precision timing utility class used to measure the execution time of code segments or record time intervals between events. Compared with DateTime based on the system clock, it typically offers higher timing precision and is suitable for time-sensitive measurement scenarios. All times are returned as double results in **seconds**.

StopWatch supports choosing whether to immediately restart on stop, making it suitable for continuously measuring multiple time intervals. This class holds resources that need to be released; it is recommended to use a using statement or call Dispose at the appropriate time.

## Constructors

### StopWatch()

csharp

public StopWatch()

**Description**: Creates an anonymous stopwatch; the required resources are allocated internally.

**Parameters**: None.

**Return Value**: None (constructor).

**Usage Example**

csharp

StopWatch sw = new StopWatch();

**Notes**

- Throws InvalidOperationException on creation failure.

### StopWatch(string strName)

csharp

public StopWatch(string strName)

**Description**: Creates a named stopwatch; the name is used to identify purpose or for debugging.

**Parameters**

| Parameter | Type   | Description                            |
|-----------|--------|----------------------------------------|
| strName   | string | Stopwatch name for easy identification |

**Return Value**: None (constructor).

**Usage Example**

csharp

StopWatch sw = new StopWatch("MultiStage");

**Notes**

- Throws InvalidOperationException on creation failure.

### StopWatch(IntPtr existingHandle, bool attach)

csharp

public StopWatch(IntPtr existingHandle, bool attach)

**Description**: Wraps an existing stopwatch handle.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| existingHandle | IntPtr | Existing stopwatch handle |
| attach | bool | true means the handle is externally owned and this instance will not release it; false means this instance takes ownership and will release it on destruction |

**Return Value**: None (constructor).

**Usage Example**

csharp

StopWatch sw = new StopWatch(handle, false);

**Notes**

- If existingHandle is IntPtr.Zero, throws ArgumentException.

## Properties

### ElapsedTime

csharp

public double ElapsedTime { get; }

**Description**: Gets the elapsed time in seconds. Equivalent to calling GetElapsedTime().

**Type**: double

**Return Value**

- If the stopwatch is running, returns the actual elapsed seconds from start to the current moment.

- If the stopwatch is stopped, returns the elapsed time last measured before stopping.

- If never started, returns 0.

**Usage Example**

csharp

double t = sw.ElapsedTime;

### Running

csharp

public bool Running { get; }

**Description**: Gets whether the stopwatch is currently running. Equivalent to calling IsStarted().

**Type**: bool

**Return Value**

- Returns true if running; otherwise false.

**Usage Example**

csharp

if (sw.Running) { */\* ... \*/* }

## Methods

### Create

csharp

public bool Create(string strName)

**Description**: Recreates the underlying stopwatch, releasing the old resources. Suitable for reusing the same instance while needing to change the name or reset the state.

**Parameters**

| Parameter | Type   | Description        |
|-----------|--------|--------------------|
| strName   | string | New stopwatch name |

**Return Value**

| Type | Description                                          |
|------|------------------------------------------------------|
| bool | Returns true on successful creation; otherwise false |

**Usage Example**

csharp

sw.Create("NewName");

### Start

csharp

public void Start()

**Description**: Starts or restarts the stopwatch, resetting the timing origin to the current moment and clearing the accumulated time.

**Parameters**: None.

**Return Value**: None (void).

**Behavior**

- If the stopwatch is not running, calling this begins timing.

- If the stopwatch is already running, calling this **discards the previously accumulated time** and restarts timing.

**Usage Example**

csharp

sw.Start();

### Stop(bool bStartAgain)

csharp

public void Stop(bool bStartAgain)

**Description**: Stops the stopwatch.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| bStartAgain | bool | When true, immediately restarts after stopping; when false, stays stopped |

**Return Value**: None (void).

**Behavior**

- This method does **not return** the elapsed time of this measurement. To obtain the time, read it before or after the call via ElapsedTime or GetElapsedTime().

- If bStartAgain is true, Start is called immediately after stopping, entering the next timing round.

- If the stopwatch has not been started, calling Stop throws no exception, but there is also no timing result.

**Usage Example**

csharp

sw.Stop(true); *// Stop and immediately restart*

sw.Stop(false); *// Stop without restarting*

### Stop()

csharp

public void Stop()

**Description**: Stops the stopwatch without restarting. Equivalent to Stop(false).

**Parameters**: None.

**Return Value**: None (void).

**Usage Example**

csharp

sw.Stop();

### GetElapsedTime

csharp

public double GetElapsedTime()

**Description**: Gets the number of seconds elapsed since the most recent Start.

**Parameters**: None.

**Return Value**

| Type | Description |
|----|----|
| double | Returns the live seconds while running; returns the seconds saved at the most recent stop after stopping; returns 0 if invalid or never started |

**Usage Example**

csharp

double seconds = sw.GetElapsedTime();

**Notes**

- This method can be called repeatedly without stopping the stopwatch, for real-time observation of elapsed time, suitable for progress display or timeout determination.

### IsStarted

csharp

public bool IsStarted()

**Description**: Determines whether the stopwatch is currently running.

**Parameters**: None.

**Return Value**

| Type | Description                              |
|------|------------------------------------------|
| bool | Returns true if running; otherwise false |

**Usage Example**

csharp

bool running = sw.IsStarted();

### ReleaseHandle

csharp

protected sealed override void ReleaseHandle()

**Description**: Releases the resources held by this instance.

**Parameters**: None.

**Return Value**: None (void).

**Notes**

- This method is automatically called by the runtime on Dispose or in the finalizer; **users do not need to call it directly**.

- It is recommended to use a using statement or call Dispose at the appropriate time to ensure timely resource release.

## Typical Usage

### Single Measurement

csharp

using (StopWatch sw = new StopWatch())

{

sw.Start();

*// Code segment to be measured*

DoSomething();

sw.Stop();

double seconds = sw.ElapsedTime;

Console.WriteLine(\$"Elapsed {seconds:F6} seconds");

}

### Continuous Multi-Stage Measurement

csharp

using (StopWatch sw = new StopWatch("MultiStage"))

{

sw.Start();

DoFirst();

sw.Stop(true); *// Stop and immediately restart*

double t1 = sw.ElapsedTime;

DoSecond();

sw.Stop(true); *// Stop again and restart*

double t2 = sw.ElapsedTime;

DoThird();

sw.Stop(); *// Last stage; stop without restarting*

double t3 = sw.ElapsedTime;

Console.WriteLine(\$"Stage 1 {t1:F6} s, Stage 2 {t2:F6} s, Stage 3 {t3:F6} s");

}

### Querying While Running

csharp

using (StopWatch sw = new StopWatch())

{

sw.Start();

while (!finished)

{

if (sw.ElapsedTime \> 5.0)

{

Console.WriteLine("Timeout");

break;

}

DoWork();

}

sw.Stop();

}

### Reusing an Instance

csharp

StopWatch sw = new StopWatch();

sw.Start();

DoFirst();

sw.Stop();

double t1 = sw.ElapsedTime;

sw.Start(); *// Restart, discarding the previous time*

DoSecond();

sw.Stop();

double t2 = sw.ElapsedTime;

sw.Create("NewName"); *// Recreate the underlying resources, fully reset*

sw.Start();

DoThird();

sw.Stop();

double t3 = sw.ElapsedTime;
