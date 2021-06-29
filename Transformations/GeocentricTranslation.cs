Wr
using System.Runtime.CompilerServices;

namespace HyperionGeo
{
    public record GeocentricTranslation : ITransformation
    {
        public GeocentricTranslation(double dX, double dY, double dZ)
        {
            DX = dX;
            DY = dY;
            DZ = dZ;
        }

        public double DX { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        public double DY { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        public double DZ { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        EcefCoordinate ITransformation.Transform(ref EcefCoordinate ecefCoordinate, bool forward)
        {
            ecefCoordinate.QueryXYZ(out double x,
                                    out double y,
                                    out double z);
            return forward ? new(x + DX, y + DY, z + DZ, false) : new(x - DX, y - DY, z - DZ, false);
        }
    }
}