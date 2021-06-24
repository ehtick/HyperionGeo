using static System.Math;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static HyperionGeo.FiniteChecks;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace HyperionGeo
{
    [DebuggerDisplay("{ToString()}")]
    [StructLayout(LayoutKind.Explicit, Size = Size)]
    public unsafe struct EllipsoidalCoordinate : IEquatable<EllipsoidalCoordinate>, ICoordinate
    {
        private const string LongitudeMustBeFinite = "Longitude must be finite.";
        private const string LatitudeMustBeFinite = "Latitude must be finite.";
        private const string HeightMustBeFinite = "Height must be finite.";

        private const double DegToRad = PI / 180.0;
        private const double RadToDeg = 180.0 / PI;

        public const int Size = 3 * sizeof(double);

        [field: FieldOffset(0 * sizeof(double))]
        public double Lon_Radians { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

        [field: FieldOffset(1 * sizeof(double))]
        public double Lat_Radians { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

        [field: FieldOffset(2 * sizeof(double))]
        public double Height_Meters { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

        public double Lon_Degrees
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => RadToDeg * Lon_Radians;
        }

        public double Lat_Degrees
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => RadToDeg * Lat_Radians;
        }

        public bool IsValid => IsFinite(Lon_Radians) && IsFinite(Lat_Radians) && IsFinite(Height_Meters);

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EllipsoidalCoordinate(double lon, double lat, double height = 0d, bool untrusted = true,
                                     bool radianLonAndLat = false) : this()
        {
            Height_Meters = height;

            if (radianLonAndLat)
            { Lon_Radians = lon; Lat_Radians = lat; }
            else
            { Lon_Radians = DegToRad * lon; Lat_Radians = DegToRad * lat; }

            if (untrusted) CheckUntrustedInput();
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe EllipsoidalCoordinate(in byte[] pByte, bool untrusted = true) : this()
        {
            fixed (byte* p = &pByte[0])
                this = *(EllipsoidalCoordinate*)p[0];

            if (untrusted) CheckUntrustedInput();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void QueryLatLon(out double lon_radians, out double lat_radians)
        {
            lon_radians = Lon_Radians;
            lat_radians = Lat_Radians;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void QueryLatLonHeight(
            out double lon_radians,
            out double lat_radians,
            out double height_meters)
        {
            lon_radians = Lon_Radians;
            lat_radians = Lat_Radians;
            height_meters = Height_Meters;
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void QueryLatLonSinCosHeight(
            out double sinlon, out double coslon,
            out double sinlat, out double coslat,
            out double alt)
        {
            (sinlon, coslon) = SinCos(Lon_Radians);
            (sinlat, coslat) = SinCos(Lat_Radians);
            alt = Height_Meters;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckUntrustedInput()
        {
            if (IsNonFinite(Lon_Radians)) throw new NotFiniteNumberException(LongitudeMustBeFinite, Lon_Radians);
            if (IsNonFinite(Lat_Radians)) throw new NotFiniteNumberException(LatitudeMustBeFinite, Lat_Radians);
            if (IsNonFinite(Height_Meters)) throw new NotFiniteNumberException(HeightMustBeFinite, Height_Meters);
        }
        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public EcefCoordinate GetAsECEF([DisallowNull] in Ellipsoid ellipsoid)
        {
            if (ellipsoid is null) throw new ArgumentNullException(nameof(ellipsoid));

            ellipsoid.QueryParamsForECEFCalculations(out double aadc, out double bbdcc, out double p1mee);
            QueryLatLonSinCosHeight(
                                    out double sinlon, out double coslon,
                                    out double sinlat, out double coslat,
                                    out double alt);


            double N = aadc / Sqrt(FusedMultiplyAdd(coslat, coslat, bbdcc));
            double d = (N + alt) * coslat;

            return new(d * coslon, d * sinlon, FusedMultiplyAdd(p1mee, N, alt) * sinlat, untrusted: false);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryProject(
            [DisallowNull] in Ellipsoid sourceEllipsoid,
            [DisallowNull] in Ellipsoid targetEllipsoid,
            out EllipsoidalCoordinate ellipsoidalCoordinate,
            [DisallowNull] in ITransformation transformation,
            bool forward = true) =>
            transformation.Transform(GetAsECEF(in sourceEllipsoid), forward).
            TryGetAsEllipsoidal(in targetEllipsoid, out ellipsoidalCoordinate);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<EcefCoordinate> GetAsECEF(
            [DisallowNull] IEnumerable<EllipsoidalCoordinate> ellipsoidalCoordinates,
            [DisallowNull] Ellipsoid ellipsoid)
        {
            if (ellipsoid is null) throw new ArgumentNullException(nameof(ellipsoid));

            ellipsoid.QueryParamsForECEFCalculations(out double aadc, out double bbdcc, out double p1mee);

            foreach (EllipsoidalCoordinate ellipsoidal in ellipsoidalCoordinates)
            {
                ellipsoidal.QueryLatLonSinCosHeight(
                                         out double sinlon, out double coslon,
                                         out double sinlat, out double coslat,
                                         out double alt);

                double N = aadc / Sqrt(FusedMultiplyAdd(coslat, coslat, bbdcc));
                double d = (N + alt) * coslat;

                yield return new(d * coslon, d * sinlon, FusedMultiplyAdd(p1mee, N, alt) * sinlat, untrusted: false);
            }
        }


        [SkipLocalsInit]
        public override string ToString() => "Lat: " + Lat_Degrees + "°; Lon: " + Lon_Degrees + "°; Alt: " + Height_Meters + "\u00a0m";

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj) => obj is EllipsoidalCoordinate coordinate && Equals(coordinate);

        public bool Equals(EllipsoidalCoordinate other) =>
            Lon_Radians == other.Lon_Radians
            && Lat_Radians == other.Lat_Radians
            && Height_Meters == other.Height_Meters;

        public override int GetHashCode() => HashCode.Combine(Lon_Radians, Lat_Radians, Height_Meters);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(EllipsoidalCoordinate left, EllipsoidalCoordinate right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(EllipsoidalCoordinate left, EllipsoidalCoordinate right) => !(left == right);
    }
}