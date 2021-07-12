//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

namespace HyperionGeo
{
    class Program
    {
        static void Main()
        {

            var wgs = new EllipsoidalCoordinate(18, 46);
            //var wgs_ecef = wgs.GetAsECEF(Ellipsoids.WGS84);
            var prj = wgs.TryProject(Projections.WGS84_PseudoMercator, out ProjectedCoordinate wemerc);
            var rew = wemerc.GetAsEllipsidalCoordinate(Projections.WGS84_PseudoMercator);
            //var dist = wgs.GetDistance(ref wgs);

            var wgs_ecef = wgs.GetAsECEF(Datums.WGS84);
            wgs_ecef.TryGetAsEllipsoidal(Datums.WGS84, out EllipsoidalCoordinate wgs_lalo);
            var u = GeoidModels.EGM96.GetUndulationValue(wgs);

            var wgs_ecef2 = wgs_lalo.GetAsECEF(Datums.WGS84);
            var di = wgs_ecef2.GetDistance(ref wgs_ecef);

            var gm = GeoidModels.EGM96;

            //var x = wgs_ecef.ToString();
            //var hd72_ecef = Trans_WGS84_HD72.WGS84ToHD72(wgs_ecef);

            //var scss = hd72_ecef.TryGetAsEllipsoidal(Ellipsoids.IUGG67, out EllipsoidalCoordinate iu);
            // Lat: 17.999999999999996°; Lon: 46.00000000000001°; Alt: 4.656612873077393E-10 m
            //Debug.Print(relli.ToString());
            //Debug.Print(iu.ToString());
        }
    }
}
