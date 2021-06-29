//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

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