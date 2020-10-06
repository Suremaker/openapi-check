using System;

namespace OpenApiCheck.Model
{
    [Flags]
    public enum CompareStatus
    {
        OK = 0,
        Warning = 1,
        Error = 2
    }
}