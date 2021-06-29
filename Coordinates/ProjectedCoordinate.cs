using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static System.Math;
using static HyperionGeo.FiniteChecks;

namespace HyperionGeo
{
    public struct ProjectedCoordinate : ICoordinate, IEquatable<ProjectedCoordinate>
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
        public ProjectedCoordinate(double x,
                              double y,
                              double z,
                              bool untrusted = true)
        {
            X = x; Y = y; Z = z;
            if (untrusted) CheckUntrustedInput();
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ProjectedCoordinate(in byte[] pByte, bool untrusted = true)
        {
            fixed (byte* p = &pByte[0]) this = *(ProjectedCoordinate*)p[0];

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
        public double GetDistance(ref ProjectedCoordinate other)
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
        public override bool Equals(object? obj) => obj is ProjectedCoordinate coordinate && Equals(coordinate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ProjectedCoordinate other) =>
                           X == other.X &&
                           Y == other.Y &&
                           Z == other.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ProjectedCoordinate left, ProjectedCoordinate right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ProjectedCoordinate left, ProjectedCoordinate right) => !(left == right);

        #endregion Common XYZ coordinate management

        public double GetDistance3D(ref ProjectedCoordinate other) => GetDistance(ref other);

        [SkipLocalsInit]
        public double GetDistance2D(ref ProjectedCoordinate other)
        {
            double dx = X - other.X, dy = Y - other.Y;
            return Sqrt(FusedMultiplyAdd(dx, dx, dy * dy));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EllipsoidalCoordinate GetAsEllipsidalCoordinate(
            [DisallowNull] in IProjection projection) => projection.ProjectInverse(ref this);
    }
}
