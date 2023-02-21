using System;
using System.Collections.Generic;
using System.IO;


namespace RayTracer;

[Flags]
enum LogType : ushort
{
    None = 0x00,
    Information = 0x01,
    Message = 0x02,
    Warning = 0x04,
    Error = 0x10,
    Debug = 0x20,
}

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
    private static Dictionary<string, Output> outputs = new();

    /// <summary>
    /// Add output for logger.
    /// </summary>
    /// <param name="bitmask">Determine which type of messages should be visible.</param>
    public static void AddOutput(
        string name,
        TextWriter writer,
        LogType bitmask = (LogType)ushort.MaxValue,
        bool timeStampEnabled = true
    )
        => outputs.Add(
            name,
            new Output() { Writer = writer, BitMask = bitmask, TimeStampEnabled = timeStampEnabled }
        );

    public static void RemoveOutput(string name)
        => outputs.Remove(name);

    public static void Write(
        string message,
        LogType type = LogType.Information,
        bool newLine = false
    )
    {
        // write to all outputs
        string timeStamp = $"[{DateTime.Now.ToString("HH:mm")}] ";
        string text = message + (newLine ? Environment.NewLine : string.Empty);
        foreach (var output in outputs.Values)
        {
            // check for visibility
            if ((type & output.BitMask) != 0)
            {
                if (output.TimeStampEnabled)
                    output.Writer.Write(timeStamp);
                output.Writer.Write(text);
            }    
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
            output.Writer.Close();
        outputs.Clear();
    }

    private struct Output
    {
        public TextWriter Writer { get; set; }
        public LogType BitMask { get; set; }
        public bool TimeStampEnabled { get; set; }
    }
}
