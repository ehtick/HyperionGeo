//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;

namespace HyperionGeo
{
    [SkipLocalsInit]
    public record Datum
    {
        public double SemiMajorAxis { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        public double InverseFlattening { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

        private readonly double invaa;

        private readonly double l;
        private readonly double p1mee;
        private readonly double aadc;
        private readonly double bbdcc;

        private readonly double p1meedaa;
        private readonly double ll4;

        private readonly double ll;

        private readonly double Hmin;


        public Datum(
            double semiMajorAxis, double inverseFlattening, double invaa,
            double l, double p1mee, double aadc,
            double bbdcc, double p1meedaa,
            double ll4, double ll, double hmin)
        {
            SemiMajorAxis = semiMajorAxis;
            InverseFlattening = inverseFlattening;

            this.invaa = invaa;
            this.l = l;
            this.p1mee = p1mee;
            this.aadc = aadc;
            this.bbdcc = bbdcc;
            this.p1meedaa = p1meedaa;
            this.ll4 = ll4;
            this.ll = ll;
            Hmin = hmin;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void QueryParamsForECEFCalculations(out double aadc, out double bbdcc, out double p1mee)
        {
            aadc = this.aadc; bbdcc = this.bbdcc; p1mee = this.p1mee;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void QueryParamsForLatLonCalculations(out double invaa,
                                          out double p1meedaa,
                                          out double ll4,
                                          out double ll,
                                          out double hmin,
                                          out double l,
                                          out double p1mee)
        {
            invaa = this.invaa; p1meedaa = this.p1meedaa;
            ll4 = this.ll4; ll = this.ll;
            hmin = this.Hmin; l = this.l;
            p1mee = this.p1mee;
        }

        public override int GetHashCode() => HashCode.Combine(HashCode.Combine(SemiMajorAxis,
                InverseFlattening,
                invaa,
                l,
                p1mee,
                aadc,
                bbdcc,
                p1meedaa), ll4, ll, Hmin);

        public virtual bool Equals(Datum? other) =>
            other is not null && SemiMajorAxis == other.SemiMajorAxis
            && InverseFlattening == other.InverseFlattening
            && invaa == other.invaa
            && l == other.l
            && p1mee == other.p1mee
            && aadc == other.aadc
            && bbdcc == other.bbdcc
            && p1meedaa == other.p1meedaa
            && ll4 == other.ll4
            && ll == other.ll
            && Hmin == other.Hmin;
    }
}