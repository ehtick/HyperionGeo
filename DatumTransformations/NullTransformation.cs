﻿//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;

namespace HyperionGeo
{
    public class NullTransformation : IDatumTransformation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        EcefCoordinate IDatumTransformation.Transform(
                                    ref EcefCoordinate coordinateToTransform,
                                    bool _)
            => coordinateToTransform;
    }
}