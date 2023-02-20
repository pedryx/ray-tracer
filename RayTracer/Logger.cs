using System;
using System.Collections.Generic;
using System.IO;


namespace RayTracer;

/// <summary>
/// Represent a logger for information messages, warnings, errors, etc.
/// </summary>
/// <remarks>
/// Multiple outputs are allowed, to add one use <see cref="AddOutput(string, TextWriter)"/>, to
/// remove one use <see cref="RemoveOutput(string)"/>. By default there is console output with
/// name "Console".
/// </remarks>
static class Logger
{
    private static Dictionary<string, TextWriter> outputs = new();

    /// <summary>
    /// Determine which type of messages should be visible. It is bitmask of <see cref="LogType"/>
    /// values. In default all types are visible.
    /// </summary>
    public static LogType Type { get; set; } = (LogType)ushort.MaxValue;

    public static void AddOutput(string name, TextWriter output)
        => outputs.Add(name, output);

    public static void RemoveOutput(string name)
        => outputs.Remove(name);

    public static void Write(
        string message,
        LogType type = LogType.Information,
        bool newLine = false
    )
    {
        // check for visibility
        if ((type & Type) == LogType.None)
            return;

        // write to all outputs
        string outputText = $"[{DateTime.Now.ToString("HH:mm")}] {message}.";
        foreach (var output in outputs.Values)
        {
            if (newLine)
                output.WriteLine(outputText);
            else
                output.Write(outputText);
        }
    }

    public static void WriteLine(string message, LogType type = LogType.Information)
        => Write(message, type, true);

    /// <summary>
    /// Close all output streams.
    /// </summary>
    public static void CloseOutputs()
    {
        foreach (var output in outputs.Values)
            output.Close();
    }
}
