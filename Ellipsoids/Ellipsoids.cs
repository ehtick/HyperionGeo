using System.Runtime.CompilerServices;

namespace HyperionGeo
{
    [SkipLocalsInit]
    public static class Ellipsoids
    {
        public static Ellipsoid IUGG67 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
             = new(semiMajorAxis: 6378160,
                  inverseFlattening: 298.247167427,
                  invaa: .00000000000002458154529067306734509336180948059588,
                  l: .00334730266428382723018727461848360215939925495480,
                  p1mee: .99330539467143234553962545076303279568120149009040,
                  aadc: 77953015.54472138756884255225116071784371923853312579483747,
                  bbdcc: 148.37400353277512592309407676358791088664820797948172,
                  p1meedaa: .00000000000002441698154658570029591164741274099638,
                  ll4: .00004481774050528643273409507598118108187763457794,
                  ll: .00001120443512632160818352376899529527046940864448,
                  hmin: .00000000000002250556300397114362153161399772866119);
        public static Ellipsoid WGS84 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
             = new(semiMajorAxis: +6.37813700000000000000e+0006,
                  inverseFlattening: +2.98257223563000000000e+0002,
                  invaa: +2.45817225764733181057e-0014,
                  l: +3.34718999507065852867e-0003,
                  p1mee: +9.93305620009858682943e-0001,
                  aadc: +7.79540464078689228919e+0007,
                  bbdcc: +1.48379031586596594555e+0002,
                  p1meedaa: +2.44171631847341700642e-0014,
                  ll4: +4.48147234524044602618e-0005,
                  ll: +1.12036808631011150655e-0005,
                  hmin: +2.25010182030430273673e-0014);
    }
}