//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;

namespace HyperionGeo
{
    public static class Projections
    {
        public static PseudoMercator WGS84_PseudoMercator { [MethodImpl(MethodImplOptions.AggressiveInlining)]get; } =
            new(Datums.WGS84.SemiMajorAxis);
        public static EOV EOV { [MethodImpl(MethodImplOptions.AggressiveInlining)]get; } =
            new();
    }
}