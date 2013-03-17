using System;

namespace NRegFreeCom
{
    /// <summary>
    /// The REGCLS enumeration defines values used in CoRegisterClassObject to 
    /// control the type of connections to a class object.
    /// </summary>
    [Flags]
    public enum REGCLS : uint
    {
        SINGLEUSE = 0,
        MULTIPLEUSE = 1,
        MULTI_SEPARATE = 2,
        SUSPENDED = 4,
        SURROGATE = 8,
    }
}