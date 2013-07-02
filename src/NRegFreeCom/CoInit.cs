namespace NRegFreeCom
{
    /// <summary>Determines the concurrency model used for incoming calls to objects created by this thread. This concurrency model can be either apartment-threaded or multi-threaded.</summary>
    public enum CoInit
    {
        /// <summary>
        /// Initializes the thread for apartment-threaded object concurrency.
        /// </summary>
        MultiThreaded = 0x0,

        /// <summary>
        /// Initializes the thread for multi-threaded object concurrency.
        /// </summary>
        ApartmentThreaded = 0x2,

        /// <summary>
        /// Disables DDE for OLE1 support.
        /// </summary>
        DisableOle1Dde = 0x4,

        /// <summary>
        /// Trade memory for speed.
        /// </summary>
        SpeedOverMemory = 0x8
    }
}