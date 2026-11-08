using System;
using NDS_WoodLibrary.NDS2018;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NDS_WoodLibrary.Tests
{
    /// <summary>
    /// NDS-2018 nails: the yield limit equations (Table 12.3.1A) against Table 12N as printed (common, box or sinker steel
    /// wire nails, single shear, both members of one G, p = 10D), read from AWC's NDS-2018 Chapter 12, page 111.
    /// </summary>
    [TestClass]
    public class NailTests
    {
        // Table 12N, side member 1-1/2": D → (G 0.55, 0.50, 0.42), as printed
        private static readonly (double d, double sp, double df, double spf)[] T12N_1_5 =
        {
            (0.131, 106, 97, 82), (0.148, 128, 118, 100), (0.162, 154, 141, 120),
            (0.177, 178, 163, 138), (0.192, 185, 170, 144), (0.244, 230, 211, 175),
        };

        // Table 12N, side member 3/4": D → (G 0.55, 0.50, 0.42), as printed
        private static readonly (double d, double sp, double df, double spf)[] T12N_0_75 =
        {
            (0.162, 138, 121, 96), (0.177, 153, 134, 107),
        };

        private static void Check((double d, double sp, double df, double spf)[] rows, double ts)
        {
            foreach (var r in rows)
            {
                Assert.AreEqual(r.sp, Math.Round(Nails.SingleShearZ(r.d, ts, 10 * r.d, 0.55, 0.55).Value), $"{r.d}\" ts {ts}\" G 0.55");
                Assert.AreEqual(r.df, Math.Round(Nails.SingleShearZ(r.d, ts, 10 * r.d, 0.50, 0.50).Value), $"{r.d}\" ts {ts}\" G 0.50");
                Assert.AreEqual(r.spf, Math.Round(Nails.SingleShearZ(r.d, ts, 10 * r.d, 0.42, 0.42).Value), $"{r.d}\" ts {ts}\" G 0.42");
            }
        }

        [TestMethod]
        public void Table12N_OneAndAHalfInchSide_AsPrinted() => Check(T12N_1_5, 1.5);

        [TestMethod]
        public void Table12N_ThreeQuarterInchSide_AsPrinted() => Check(T12N_0_75, 0.75);

        /// <summary>Table 12.3.3: Fe for D &lt; 1/4" printed 4650 psi at G 0.50, 3350 at 0.42, 5550 at 0.55; Rd and Fyb by D.</summary>
        [TestMethod]
        public void BearingStrength_ReductionTerm_BendingYield()
        {
            Assert.AreEqual(4650, Nails.TabulatedBearingStrength(0.50));
            Assert.AreEqual(3350, Nails.TabulatedBearingStrength(0.42));
            Assert.AreEqual(5550, Nails.TabulatedBearingStrength(0.55));
            Assert.AreEqual(4637, Nails.BearingStrength(0.50), 1.0, "16,600 × 0.5^1.84");
            Assert.AreEqual(2.2, Nails.ReductionTerm(0.162), 1e-9);
            Assert.AreEqual(10 * 0.192 + 0.5, Nails.ReductionTerm(0.192), 1e-9);
            Assert.AreEqual(100000, Nails.BendingYield(0.131));
            Assert.AreEqual(90000, Nails.BendingYield(0.162));
            Assert.AreEqual(80000, Nails.BendingYield(0.192));
        }

        /// <summary>
        /// Less penetration: 6D the least (12.1.6.4: under it, no value); between 6D and 10D the equations with ℓm = p − D
        /// (12.3.5.3), never more than at 10D, and at least Table 12N's own allowance, Z × p/10D (its note 3).
        /// </summary>
        [TestMethod]
        public void Penetration_SixDiametersTheLeast()
        {
            double d = 0.162, z10 = Nails.SingleShearZ(d, 1.5, 10 * d, 0.5, 0.5).Value;
            Assert.IsNull(Nails.SingleShearZ(d, 1.5, 5.9 * d, 0.5, 0.5), "under 6D: no value");
            double z6 = Nails.SingleShearZ(d, 1.5, 6 * d, 0.5, 0.5).Value;
            Assert.IsTrue(z6 <= z10 + 1e-9, "never more than at 10D");
            Assert.IsTrue(z6 >= z10 * 0.6 - 1e-9, "at least note 3's p/10D");
        }
    }
}
