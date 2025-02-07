﻿//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace HyperionGeo
{
    public static class DatumTransformations
    {   
        public static GeocentricTranslation HD72ToWGS84 { [MethodImpl(AggressiveInlining)] get; } =
            new (52.17, -71.82, -14.9);
        public static NullTransformation NullTransformation { [MethodImpl(AggressiveInlining)] get; } =
            new ();
     }
}