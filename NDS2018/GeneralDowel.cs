using System;
using System.Linq;

namespace NDS_WoodLibrary.NDS2018
{
    /// <summary>
    /// AWC Technical Report 12 (2015), General Dowel Equations for Calculating Lateral Connection Values: Table 1-1, single
    /// shear, members of solid cross section, with a gap g between them (the NDS yield limit equations are the case g = 0;
    /// NDS-2018 12.3.1(a) asks for the faces in contact, TR12 gives the gap). P for each yield mode; Z = the least P / Rd
    /// (TR12 1.3, Rd from NDS Table 12.3.1B).
    /// </summary>
    public static class GeneralDowel
    {
        public const string Source =
            "AWC TR12 (2015) Table 1-1, single shear, solid members: Im P = qm Lm; Is P = qs Ls; II to IV P = (−B + √(B² − 4AC)) / 2A " +
            "with II: A = 1/4qs + 1/4qm, B = Ls/2 + g + Lm/2, C = −qs Ls²/4 − qm Lm²/4; IIIm: A = 1/2qs + 1/4qm, B = g + Lm/2, " +
            "C = −Ms − qm Lm²/4; IIIs: A = 1/4qs + 1/2qm, B = Ls/2 + g, C = −qs Ls²/4 − Mm; IV: A = 1/2qs + 1/2qm, B = g, C = −Ms − Mm; " +
            "q = Fe D (lb/in), M = Fyb D³/6 (in-lb), g the gap (in); Z = min P/Rd (1.3).";

        /// <summary>
        /// P / Rd for the six modes (Im, Is, II, IIIm, IIIs, IV) of a single shear connection. <paramref name="rd"/>: the
        /// reduction term of each mode in that order (NDS Table 12.3.1B).
        /// </summary>
        public static double[] SingleShear(double qs, double qm, double ls, double lm, double g, double ms, double mm, double[] rd)
        {
            double Quad(double a, double b, double c) => (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
            var p = new[]
            {
                qm * lm,                                                                                         // Im
                qs * ls,                                                                                         // Is
                Quad(1 / (4 * qs) + 1 / (4 * qm), ls / 2 + g + lm / 2, -qs * ls * ls / 4 - qm * lm * lm / 4),  // II
                Quad(1 / (2 * qs) + 1 / (4 * qm), g + lm / 2, -ms - qm * lm * lm / 4),                         // IIIm
                Quad(1 / (4 * qs) + 1 / (2 * qm), ls / 2 + g, -qs * ls * ls / 4 - mm),                         // IIIs
                Quad(1 / (2 * qs) + 1 / (2 * qm), g, -ms - mm),                                                  // IV
            };
            return p.Select((x, i) => x / rd[i]).ToArray();
        }

        /// <summary>The dowel's moment resistance Fyb D³/6 (in-lb).</summary>
        public static double MomentResistance(double fyb, double d) => fyb * d * d * d / 6.0;
    }
}
