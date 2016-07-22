namespace CeMaS.Common.Display
{
    /// <summary>
    /// Specifies display mode of a zoomable view.
    /// </summary>
    public enum ZoomDisplayMode :
        byte
    {
        /// <summary>
        /// Zoomed in with detailed view.
        /// </summary>
        ZoomedIn,
        /// <summary>
        /// Zoomed out with simplified non-detailed compact view.
        /// </summary>
        ZoomedOut
    }
}
