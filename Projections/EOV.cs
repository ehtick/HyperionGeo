using static System.Math;

namespace HyperionGeo
{
    public class EOV : IProjection
    {
        EllipsoidalCoordinate IProjection.ProjectInverse(ref ProjectedCoordinate coordinateToProject)
        {
            throw new NotImplementedException();
        }

        bool IProjection.TryProjectForward(ref EllipsoidalCoordinate coordinateToProject, out ProjectedCoordinate projectedCoordinate)
        {
            // https://lechnerkozpont.hu/data/sites/default/files/doc/iny/szabalyzatok/A1_vetuleti_szabalyzat.pdf

            const double R = 6379743.001; // m; Radius of the Gaussian sphere.
            const double n = 1.0007197049;
            const double k = 1.0031100083;
            const double e = 0.0818205679;
            const double LAM0 = 0.33246029532469185650131667237359;
            const double sinfi0 = 0.73254289878737876179290512935584;
            const double cosfi0 = 0.68072086895891781187884673233212;
            const double m0 = .99993;
            const double PIp4 = 0.78539816339744830961566084581988; // ¼·π.


            coordinateToProject.QueryLatLon(out double LAM, out double FI);
            double lam = n * (LAM - LAM0);
            (double sin_lam, double cos_lam) = SinCos(lam);

            double h1 = e * Sin(FI), h2 = (1 - h1) / (1 + h1), h3 = Pow(h2, .5 * n * e);
            double h4 = Pow(Tan(FusedMultiplyAdd(.5, FI, PIp4)), n);
            double fi = ScaleB(Atan(k * h4 * h3) - PIp4, 1);
            (double sin_fi, double cos_fi) = SinCos(fi);

            double fi_v = Asin(FusedMultiplyAdd(cosfi0, sin_fi, -sinfi0 * cos_fi * cos_lam));
            double lam_v = Asin(cos_fi * sin_lam / Cos(fi_v));

            double y = FusedMultiplyAdd(R * m0, lam_v, 650000);
            double x = 200000 + R * m0 * Log(Tan(FusedMultiplyAdd(.5, fi_v, PIp4)));

            projectedCoordinate = new(x, y, 0, false);

            // TODO: Several checks are missing!
            return true;
        }
    }
}
