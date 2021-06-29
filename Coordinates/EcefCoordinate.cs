//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static System.Math;
using static HyperionGeo.FiniteChecks;

namespace HyperionGeo
{
    [DebuggerDisplay("{ToString()}")]
    [StructLayout(LayoutKind.Sequential, Size = 3 * sizeof(double))]
    public struct EcefCoordinate : IEquatable<EcefCoordinate>, ICoordinate
    {
        #region Common XYZ coordinate management

        private const string XMustBeFinite = "X must be finite.";
        private const string YMustBeFinite = "Y must be finite.";
        private const string ZMustBeFinite = "Z must be finite.";

        public double X { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        public double Y { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        public double Z { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

        public bool IsValid => IsFinite(X)
                               && IsFinite(Y)
                               && IsFinite(Z);
        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcefCoordinate(double x,
                              double y,
                              double z,
                              bool untrusted = true)
        {
            X = x; Y = y; Z = z;
            if (untrusted) CheckUntrustedInput();
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe EcefCoordinate(in byte[] pByte, bool untrusted = true)
        {
            fixed (byte* p = &pByte[0]) this = *(EcefCoordinate*)p[0];

            if (untrusted) CheckUntrustedInput();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public void QueryXYZ(
            out double x,
            out double y,
            out double z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        [SkipLocalsInit]
        public double GetDistance(ref EcefCoordinate other)
        {
            double dx = X - other.X, dy = Y - other.Y, dz = Z - other.Z;
            return Sqrt(FusedMultiplyAdd(dx, dx, FusedMultiplyAdd(dy, dy, dz * dz)));
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckUntrustedInput()
        {
            if (IsNonFinite(X)) throw new NotFiniteNumberException(XMustBeFinite, X);
            if (IsNonFinite(Y)) throw new NotFiniteNumberException(YMustBeFinite, Y);
            if (IsNonFinite(Z)) throw new NotFiniteNumberException(ZMustBeFinite, Z);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj) => obj is EcefCoordinate coordinate && Equals(coordinate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(EcefCoordinate other) =>
                           X == other.X &&
                           Y == other.Y &&
                           Z == other.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(EcefCoordinate left, EcefCoordinate right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(EcefCoordinate left, EcefCoordinate right) => !(left == right);

        #endregion Common XYZ coordinate management

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool TryGetAsEllipsoidal([DisallowNull] in Ellipsoid ellipsoid,
                                               out EllipsoidalCoordinate ellipsoidalCoordinate)
        {
            const double inv6 = .16666666666666666666666666666666666666666666666666;
            const double inv3 = .33333333333333333333333333333333333333333333333333;
            const double invcbrt2 = .79370052598409973737585281963615413019574666394992;

            if (ellipsoid is null) throw new ArgumentNullException(nameof(ellipsoid));

            ellipsoid.QueryParamsForLatLonCalculations(out double invaa, out double p1meedaa, out double ll4,
                                            out double ll, out double Hmin, out double l,
                                            out double p1mee);
            double x = X, y = Y, z = Z;

            double ww = FusedMultiplyAdd(x, x, y * y);
            double m = invaa * ww;
            double n = p1meedaa * z * z;
            double mpn = m + n;
            double p = inv6 * (mpn - ll4);
            double P = p * p;
            double G = ll * m * n;
            double H = FusedMultiplyAdd(ScaleB(P, 1), p, G);

            if (H < Hmin)
            {
                ellipsoidalCoordinate = default;
                return false;
            }

            double C = invcbrt2 * Cbrt(H + G + ScaleB(Sqrt(H * G), 1));
            double i = -ll - ScaleB(mpn, -1);

            double beta = FusedMultiplyAdd(inv3, i, -C) - P / C;
            double k = ll * (ll - mpn);
            // Compute left part of t
            double t1 = Sqrt(FusedMultiplyAdd(beta, beta, -k));
            double t3 = Sqrt(t1 - ScaleB(beta + i, -1));

            // Compute right part of t
            // t5 may accidentally drop just below zero due to numeric turbulence
            // This only occurs at latitudes close to +- 45.3 degrees
            double t5 = Sqrt(Abs(ScaleB(beta - i, -1)));
            double t7 = (m < n) ? t5 : -t5;
            // Add left and right parts

            // Use Newton-Raphson's method to compute t correction
            double t = t7 + t3, tt = t * t;
            double g = ScaleB(l * (m - n), 1);
            double F = -FusedMultiplyAdd(tt, tt, FusedMultiplyAdd(ScaleB(i, 1), tt, FusedMultiplyAdd(g, t, k)));
            double dFdt = FusedMultiplyAdd(ScaleB(tt, 2), t, FusedMultiplyAdd(ScaleB(i, 2), t, g));
            double dt = F / dFdt;

            double tplusdt = t + dt;
            double u = l + tplusdt;
            double uz = u * z;

            double v = tplusdt - l;
            double w = Sqrt(ww);
            double wv = w * v;

            double invuv = 1.0 / (u * v);
            double dw = w - wv * invuv;
            double dz = z - p1mee * uz * invuv;
            double da = Sqrt(FusedMultiplyAdd(dw, dw, dz * dz));
            double alt = (u < 1.0) ? -da : da;

            ellipsoidalCoordinate = new(
                lon: Atan2(y, x),
                lat: Atan2(uz, wv),
                height: alt,
                untrusted: false,
                radianLonAndLat: true);
            return true;
        }

        [SkipLocalsInit]
        public override string ToString() => new StringBuilder(X.ToString(), 65).
                Append("\u00a0m; ").
                Append(Y).
                Append("\u00a0m; ").
                Append(Z).
                Append("\u00a0m").
                ToString();
    }
}