
namespace Microsoft.Win32
{
    /// <summary>
    /// Specifies which registry view to target on a 64-bit operating system.
    /// </summary>
    public enum RegistryView
    {
        Default = 0,
        Registry64 = 256,
        Registry32 = 512,
    }
}
