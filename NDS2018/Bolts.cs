using System;
using System.Collections.Generic;

namespace NDS_WoodLibrary.NDS2018
{
    /// <summary>NDS-2018 Chapter 12 bolts.</summary>
    public static class Bolts
    {
        public const string Table12ESource =
            "NDS-2018 Table 12E, bolts: reference lateral design values Z for single shear (two member) connections, sawn " +
            "lumber to concrete, 1-1/2\" side member, 6\" or more embedment: Z∥ for 1/2\" bolts SP (G = 0.55) 680, DF-L (0.50) " +
            "650, HF (0.43) 590, SPF (0.42) 590 lb; for 5/8\" bolts 970, 930, 860, 850 lb.";

        // Z parallel to grain (lb), 1-1/2" side member, 6" embedment: by bolt diameter (in), then species
        private static readonly Dictionary<double, Dictionary<Species, double>> ZParallel = new Dictionary<double, Dictionary<Species, double>>
        {
            { 0.5,   new Dictionary<Species, double> { { Species.SouthernPine, 680 }, { Species.DouglasFirLarch, 650 }, { Species.HemFir, 590 }, { Species.SprucePineFir, 590 } } },
            { 0.625, new Dictionary<Species, double> { { Species.SouthernPine, 970 }, { Species.DouglasFirLarch, 930 }, { Species.HemFir, 860 }, { Species.SprucePineFir, 850 } } },
        };

        /// <summary>The bolt diameters (in) <see cref="SillToConcreteZ"/> has.</summary>
        public static IEnumerable<double> SillBoltDiameters => ZParallel.Keys;

        /// <summary>
        /// Table 12E Z∥ (lb) for a bolt through a 1-1/2" sill (side member) into concrete, embedded 6" or more: the sill's
        /// load along the wall is parallel to its grain.
        /// </summary>
        public static double SillToConcreteZ(double diameterIn, Species s)
        {
            foreach (var row in ZParallel)
                if (Math.Abs(row.Key - diameterIn) < 1e-6) return row.Value[s];
            throw new ArgumentOutOfRangeException(nameof(diameterIn), "Table 12E here has 1/2\" and 5/8\" bolts only.");
        }
    }
}
