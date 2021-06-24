// Written by Ákos Halmai, University of Pécs, Hungary.

namespace HyperionGeo
{
    /// <summary>
    /// Simple interface to indicate common tools for geodetic datum transformations.
    /// </summary>
    /// <remarks>Every transformation class/record have to implement this interface to use for
    /// <see cref="EcefCoordinate"/> transformations, like 
    /// <see cref="Ellipsoids.IUGG67" → <see cref="Ellipsoids.WGS84"/>.
    /// </remarks>

    public interface ITransformation
    {
        /// <summary>
        /// Simplified method to implement geodetic datum transformations in
        /// a generic form.
        /// </summary>
        /// <param name="coordinateToTransform">Incoming <see cref="EcefCoordinate"/> coordinate.
        /// It is marked with <see cref="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier"> 
        /// “in” parameter modifier (C♯)</see>.</param>
        /// <param name="forward">Incoming <see cref="bool"/> switch to indicate the direction of
        /// the transformation. If <paramref name="forward"/> is <see cref="true"/> it indicates the
        /// forward direction of “A” to “B” if the implemted transformation called “AToB”.
        /// For example if the transformation called <see cref="Transformations.HD72ToWGS84"/> 
        /// than the <see cref="true"/> indicates a transformation pointing from HD72
        /// to WGS84.</param>
        /// <returns>
        /// Returns the transformed <see cref="EcefCoordinate"/>.
        /// </returns>
         public EcefCoordinate Transform(
             in EcefCoordinate coordinateToTransform, 
             bool forward);
    }
}