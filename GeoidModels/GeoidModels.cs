using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperionGeo
{
    public static class GeoidModels
    {
        private static GlobalGeoidModel? _EGM96 = null;
        public static GlobalGeoidModel EGM96
        {
            get
            {
                if (_EGM96 is null)
                    _EGM96 = new("EGM96", @".\GeoidModels\egm96.flt", 721, 1441, .25, .25);
                return _EGM96;
            }
        }
    }
}