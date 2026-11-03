using System;

namespace NDS_WoodLibrary.NDS2018
{
    /// <summary>The load durations of NDS-2018 Table 2.3.2.</summary>
    public enum LoadDuration
    {
        /// <summary>Permanent: dead load, CD 0.9.</summary>
        Permanent,
        /// <summary>Ten years: occupancy live load, CD 1.0.</summary>
        TenYears,
        /// <summary>Two months: snow load, CD 1.15.</summary>
        TwoMonths,
        /// <summary>Seven days: construction load (roof live load), CD 1.25.</summary>
        SevenDays,
        /// <summary>Ten minutes: wind and earthquake, CD 1.6.</summary>
        TenMinutes,
        /// <summary>Impact, CD 2.0.</summary>
        Impact,
    }

    /// <summary>NDS-2018 adjustment factors and the column stability factor.</summary>
    public static class Adjustments
    {
        public const string CDSource = "NDS-2018 Table 2.3.2: CD = 0.9 permanent (dead), 1.0 ten years (occupancy live), 1.15 two months (snow), 1.25 seven days (construction), 1.6 ten minutes (wind, earthquake), 2.0 impact; 2.3.2.2: for a combination of loads, the CD of the shortest-duration load in it.";
        public const string CrSource = "NDS-2018 4.3.9: Cr = 1.15 on Fb for studs, joists, rafters and the like, 2\" to 4\" thick, at most 24\" on centre, not less than 3 in number and joined by a load-distributing element (sheathing).";
        public const string CPSource = "NDS-2018 3.7.1, Eq. 3.7-1: CP = (1 + FcE/Fc*)/(2c) − √[((1 + FcE/Fc*)/(2c))² − (FcE/Fc*)/c], FcE = 0.822 Emin'/(le/d)², c = 0.8 for sawn lumber; Fc* = Fc times every factor but CP; le/d ≤ 50 (3.7.1.4).";
        public const string CombinedSource = "NDS-2018 3.9.2, Eq. 3.9-3 (bending about the strong axis only): (fc/Fc')² + fb1/[Fb1'(1 − fc/FcE1)] ≤ 1.0, with fc < FcE1 = 0.822 Emin'/(le1/d1)².";

        /// <summary>The repetitive member factor (4.3.9).</summary>
        public const double Cr = 1.15;

        /// <summary>c for sawn lumber in Eq. 3.7-1.</summary>
        public const double CSawn = 0.8;

        /// <summary>The largest le/d of a solid column (3.7.1.4).</summary>
        public const double MaxSlenderness = 50;

        public static double CD(LoadDuration d)
        {
            switch (d)
            {
                case LoadDuration.Permanent: return 0.9;
                case LoadDuration.TenYears: return 1.0;
                case LoadDuration.TwoMonths: return 1.15;
                case LoadDuration.SevenDays: return 1.25;
                case LoadDuration.TenMinutes: return 1.6;
                case LoadDuration.Impact: return 2.0;
                default: throw new ArgumentOutOfRangeException(nameof(d));
            }
        }

        public static string Name(LoadDuration d)
        {
            switch (d)
            {
                case LoadDuration.Permanent: return "permanent";
                case LoadDuration.TenYears: return "ten years";
                case LoadDuration.TwoMonths: return "two months";
                case LoadDuration.SevenDays: return "seven days";
                case LoadDuration.TenMinutes: return "ten minutes";
                default: return "impact";
            }
        }

        /// <summary>Euler critical buckling design value FcE = 0.822 Emin' / (le/d)² (psi).</summary>
        public static double FcE(double eminPrime, double leOverD) => 0.822 * eminPrime / (leOverD * leOverD);

        /// <summary>Column stability factor CP (Eq. 3.7-1) for <paramref name="fcStar"/> and <paramref name="fcE"/> (psi).</summary>
        public static double CP(double fcStar, double fcE, double c = CSawn)
        {
            if (!(fcStar > 0)) throw new ArgumentOutOfRangeException(nameof(fcStar));
            double r = fcE / fcStar, a = (1 + r) / (2 * c);
            return a - Math.Sqrt(a * a - r / c);
        }

        /// <summary>
        /// Eq. 3.9-3 for bending about one axis (fb2 = 0): (fc/Fc')² + fb1/[Fb1'(1 − fc/FcE1)]. Infinity when fc ≥ FcE1
        /// (the member buckles).
        /// </summary>
        public static double CombinedCompressionBending(double fc, double fcPrime, double fb1, double fb1Prime, double fcE1)
        {
            if (fc >= fcE1) return double.PositiveInfinity;
            double axial = fc / fcPrime;
            return axial * axial + (fb1 > 0 ? fb1 / (fb1Prime * (1 - fc / fcE1)) : 0);
        }
    }
}
