using System;


namespace RayTracer;

[Flags]
enum LogType : ushort
{
    None = 0x00,
    Information = 0x01,
    Warning = 0x02,
    Error = 0x04,
    Debug = 0x10,
}
