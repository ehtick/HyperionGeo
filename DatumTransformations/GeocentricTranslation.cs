//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace HyperionGeo
{
    public record GeocentricTranslation : IDatumTransformation
    {
        public GeocentricTranslation(double dX, double dY, double dZ)
        {
            DX = dX;
            DY = dY;
            DZ = dZ;
        }

        public double DX { [MethodImpl(AggressiveInlining)] get; }
        public double DY { [MethodImpl(AggressiveInlining)] get; }
        public double DZ { [MethodImpl(AggressiveInlining)] get; }

        [MethodImpl(AggressiveInlining)]
        EcefCoordinate IDatumTransformation.Transform(ref EcefCoordinate ecefCoordinate, bool forward)
        {
            ecefCoordinate.QueryXYZ(out double x,
                                    out double y,
                                    out double z);
            return forward ? new(x + DX, y + DY, z + DZ, false) : new(x - DX, y - DY, z - DZ, false);
        }
    }
}