# Clock Class Detailed Usage Guide

## Overview

Clock is a lightweight timing utility class used to measure the execution time of code segments or record time intervals between events. The class is based on DateTime.Now and provides basic operations such as starting, stopping, restarting, and querying the current elapsed time. It returns double-precision floating-point results in seconds, making it suitable for scenarios that require continuous measurement of multiple time intervals.

Clock is a general-purpose utility type in Kingpool.Core. It does not involve unmanaged resources and does not require manual release. The class does not define any public fields or properties; all state is maintained through private fields, and external code can only operate through four public members (one constructor and three methods).

## Constructor

csharp

public Clock()

**Description**: Creates a Clock instance. After creation, the timer is in a stopped state, and the internally elapsed time is initialized to 0.

**Parameters**: None.

**Return Value**: None (constructor).

**Usage Example**

csharp

Clock clock = new Clock();

**Notes**

Clock does not define a parameterized constructor; only the default parameterless constructor can be used. After creating an instance, you must explicitly call Start() to begin timing. Calling Stop() or GetElapsedSeconds() before calling Start() will not produce valid results.

## Methods

### Start

csharp

public void Start()

**Description**: Starts the timer. Resets the start time to the current system time, clears the elapsed time, and sets the running flag to true.

**Parameters**: None.

**Return Value**: None (void).

**Behavior**

- If the timer is not running, calling Start enters the running state and begins timing from the current moment.

- If the timer is already running, calling Start again **discards the previously accumulated time**, resets the start time to the current moment, and restarts timing.

- The internal elapsed time field is cleared, so subsequent calls to GetElapsedSeconds() return the time since this Start call.

**Usage Example**

csharp

Clock clock = new Clock();

clock.Start();

*// Code segment to be measured*

DoSomething();

### Stop

csharp

public double Stop(bool bStartAgain)

**Description**: Stops the timer and returns the elapsed time of this measurement, in seconds.

**Parameters**

| Parameter | Type | Description |
|----|----|----|
| bStartAgain | bool | When true, immediately calls Start to restart the timer after returning the elapsed time; when false, the timer remains stopped until Start is called again |

**Return Value**

| Type | Description |
|----|----|
| double | The elapsed seconds of this measurement. If the timer is not currently running (m_bStarted is false), returns 0 without modifying internal state |

**Behavior**

- When Stop is called, the current elapsed time is first written to an internal field, then the running flag is set to false.

- After stopping, subsequent calls to GetElapsedSeconds() return the fixed value measured at this stop, rather than continuing to grow.

- If bStartAgain is true, after saving the elapsed time, Start is called immediately, resetting the start time to the current moment and clearing elapsed time for the next measurement.

- If the timer has never been started, or is already stopped, calling Stop returns 0, and bStartAgain will not trigger a restart (because the internal logic returns early when m_bStarted is false).

**Usage Example**

csharp

*// Single measurement: stop without restarting*

Clock clock = new Clock();

clock.Start();

DoSomething();

double seconds = clock.Stop(false);

Console.WriteLine(\$"Elapsed {seconds:F3} seconds");

*// Continuous measurement: stop and immediately restart*

clock.Start();

DoFirst();

double t1 = clock.Stop(true); *// Record stage 1 and immediately restart*

DoSecond();

double t2 = clock.Stop(true); *// Record stage 2 and immediately restart*

DoThird();

double t3 = clock.Stop(false); *// Record stage 3 and stop*

### GetElapsedSeconds

csharp

public double GetElapsedSeconds()

**Description**: Gets the number of seconds elapsed since the most recent Start.

**Parameters**: None.

**Return Value**

| Type | Description |
|----|----|
| double | If the timer is running, returns the actual elapsed seconds from the start time to the current moment; if the timer is stopped (or never started), returns the elapsed time saved at the most recent Stop; if it has never been stopped, returns 0 |

**Behavior**

- This method can be called repeatedly **without stopping the timer**, for real-time observation of elapsed time. It is suitable for progress display or timeout determination.

- Internally, the result is computed from the difference between DateTime.Now and the start time, converted to seconds via TotalMilliseconds / 1000.

- If the timer is running, each call recomputes the value, so the return value grows over time; if stopped, it returns a fixed value.

**Usage Example**

csharp

*// Real-time observation*

Clock clock = new Clock();

clock.Start();

while (!finished)

{

double elapsed = clock.GetElapsedSeconds();

if (elapsed \> 5.0)

{

Console.WriteLine("Timeout");

break;

}

DoWork();

}

clock.Stop(false);

## Typical Usage

### Single Measurement

csharp

Clock clock = new Clock();

clock.Start();

DoSomething();

double seconds = clock.Stop(false);

Console.WriteLine(\$"Elapsed {seconds:F3} seconds");

### Continuous Multi-Stage Measurement

csharp

Clock clock = new Clock();

clock.Start();

DoFirst();

double t1 = clock.Stop(true); *// Record stage 1 and immediately restart*

DoSecond();

double t2 = clock.Stop(true); *// Record stage 2 and immediately restart*

DoThird();

double t3 = clock.Stop(false); *// Record stage 3 and stop*

Console.WriteLine(\$"Stage 1 {t1:F3} s, Stage 2 {t2:F3} s, Stage 3 {t3:F3} s");

### Querying While Running

csharp

Clock clock = new Clock();

clock.Start();

while (!finished)

{

double elapsed = clock.GetElapsedSeconds();

if (elapsed \> 5.0)

{

Console.WriteLine("Timeout");

break;

}

DoWork();

}
