//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;
using System.Security;

namespace HyperionGeo
{
    public static unsafe class FiniteChecks
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe bool IsNonFinite(double d) => (*(long*)&d & long.MaxValue) >= 9218868437227405312L;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe bool IsFinite(double d) => (*(long*)&d & long.MaxValue) < 9218868437227405312L;
    }
}