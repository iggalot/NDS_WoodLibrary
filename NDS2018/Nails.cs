using System;
using System.Linq;

namespace NDS_WoodLibrary.NDS2018
{
    /// <summary>
    /// NDS-2018 Chapter 12 nails (common, box, sinker steel wire): the reference lateral design value Z for a single shear
    /// (two member) wood-to-wood connection by the yield limit equations, as Table 12N is calculated (12.3.2).
    /// </summary>
    public static class Nails
    {
        public const string Source =
            "NDS-2018 12.3.1 and Table 12.3.1A (yield limit equations, single shear: modes Im, Is, II, IIIm, IIIs, IV, the least); " +
            "Table 12.3.1B (Rd = KD: 2.2 for D ≤ 0.17\", 10D + 0.5 for 0.17\" < D < 0.25\"); Table 12.3.3 note 2 (Fe = 16,600 G^1.84 " +
            "for D < 1/4\"); Table 12N note 2 (Fyb 100,000 psi for 0.099\" ≤ D ≤ 0.142\", 90,000 for 0.142\" < D ≤ 0.177\", 80,000 for " +
            "0.177\" < D ≤ 0.236\", 70,000 for 0.236\" < D ≤ 0.273\"); 12.1.6.4 (pmin = 6D); 12.3.5.3 (ℓm ≤ p − E/2, E = 2D for nails); " +
            "12.3.1(a): the faces of the connected members in contact.";

        /// <summary>Dowel bearing strength for D &lt; 1/4" (psi): Table 12.3.3 note 2, Fe = 16,600 G^1.84 (not rounded).</summary>
        public static double BearingStrength(double g) => 16600.0 * Math.Pow(g, 1.84);

        /// <summary>Table 12.3.3's printed Fe for D &lt; 1/4" (psi): the formula rounded to the nearest 50 psi (its note 2).</summary>
        public static double TabulatedBearingStrength(double g) => Math.Round(BearingStrength(g) / 50.0, MidpointRounding.AwayFromZero) * 50.0;

        /// <summary>The nail's bending yield strength Fyb (psi) Table 12N is calculated with (its note 2), by diameter (in).</summary>
        public static double BendingYield(double d)
        {
            if (d >= 0.099 - 1e-9 && d <= 0.142 + 1e-9) return 100000;
            if (d > 0.142 && d <= 0.177 + 1e-9) return 90000;
            if (d > 0.177 && d <= 0.236 + 1e-9) return 80000;
            if (d > 0.236 && d <= 0.273 + 1e-9) return 70000;
            throw new ArgumentOutOfRangeException(nameof(d), "Table 12N covers nails 0.099\" to 0.273\" in diameter.");
        }

        /// <summary>The reduction term Rd for D &lt; 0.25" (Table 12.3.1B): KD, 2.2 for D ≤ 0.17", 10D + 0.5 above.</summary>
        public static double ReductionTerm(double d) => d <= 0.17 + 1e-12 ? 2.2 : 10 * d + 0.5;

        /// <summary>The minimum penetration into the main member (in): 12.1.6.4, 6D.</summary>
        public static double MinPenetration(double d) => 6 * d;

        /// <summary>
        /// Z (lb) for one nail in single shear, side member <paramref name="ts"/> thick (in) of specific gravity
        /// <paramref name="gSide"/>, into a main member of <paramref name="gMain"/> with penetration <paramref name="p"/> (in, the
        /// tip included). The main member's bearing length ℓm = p − E/2 with E = 2D (12.3.5.3(b)): ℓm = p − D. Null when p is less
        /// than 6D (12.1.6.4). Each yield mode's value in <paramref name="modes"/> (Im, Is, II, IIIm, IIIs, IV).
        /// </summary>
        public static double? SingleShearZ(double d, double ts, double p, double gSide, double gMain, out double[] modes)
        {
            modes = null;
            if (p < MinPenetration(d) - 1e-9) return null;
            double fes = TabulatedBearingStrength(gSide), fem = TabulatedBearingStrength(gMain), fyb = BendingYield(d), rd = ReductionTerm(d);
            double ls = ts, lm = p - d;
            double re = fem / fes, rt = lm / ls;
            double k1 = (Math.Sqrt(re + 2 * re * re * (1 + rt + rt * rt) + rt * rt * re * re * re) - re * (1 + rt)) / (1 + re);
            double k2 = -1 + Math.Sqrt(2 * (1 + re) + 2 * fyb * (1 + 2 * re) * d * d / (3 * fem * lm * lm));
            double k3 = -1 + Math.Sqrt(2 * (1 + re) / re + 2 * fyb * (2 + re) * d * d / (3 * fem * ls * ls));
            modes = new[]
            {
                d * lm * fem / rd,                                           // Im   (12.3-1)
                d * ls * fes / rd,                                           // Is   (12.3-2)
                k1 * d * ls * fes / rd,                                      // II   (12.3-3)
                k2 * d * lm * fem / ((1 + 2 * re) * rd),                     // IIIm (12.3-4)
                k3 * d * ls * fem / ((2 + re) * rd),                         // IIIs (12.3-5)
                d * d / rd * Math.Sqrt(2 * fem * fyb / (3 * (1 + re))),     // IV   (12.3-6)
            };
            return modes.Min();
        }

        public static double? SingleShearZ(double d, double ts, double p, double gSide, double gMain) => SingleShearZ(d, ts, p, gSide, gMain, out _);

        /// <summary>
        /// Z (lb) for one nail in single shear with a gap <paramref name="gap"/> (in) between the side and main members (the
        /// faces not in contact: NDS-2018 12.3.1(a) does not cover it), by AWC TR12's general dowel equations
        /// (<see cref="GeneralDowel"/>): the side member <paramref name="ts"/> thick, the penetration <paramref name="p"/> into
        /// the main member past the gap (ℓm = p − D, 12.3.5.3), Fe, Fyb and Rd as for Table 12N. Null when p is less than 6D.
        /// With no gap it is <see cref="SingleShearZ(double, double, double, double, double)"/>.
        /// </summary>
        public static double? SingleShearZWithGap(double d, double ts, double gap, double p, double gSide, double gMain, out double[] modes)
        {
            modes = null;
            if (p < MinPenetration(d) - 1e-9) return null;
            double qs = TabulatedBearingStrength(gSide) * d, qm = TabulatedBearingStrength(gMain) * d;
            double m = GeneralDowel.MomentResistance(BendingYield(d), d), rd = ReductionTerm(d);
            modes = GeneralDowel.SingleShear(qs, qm, ts, p - d, Math.Max(0, gap), m, m, Enumerable.Repeat(rd, 6).ToArray());
            return modes.Min();
        }

        public static double? SingleShearZWithGap(double d, double ts, double gap, double p, double gSide, double gMain) =>
            SingleShearZWithGap(d, ts, gap, p, gSide, gMain, out _);
    }
}
