using System.Runtime.CompilerServices;

namespace HyperionGeo
{
    public static class Projections
    {
        public static PseudoMercator WGS84_WebMercator_AuxSphere { [MethodImpl(MethodImplOptions.AggressiveInlining)]get; } =
            new(Ellipsoids.WGS84.SemiMajorAxis);     
    }
}