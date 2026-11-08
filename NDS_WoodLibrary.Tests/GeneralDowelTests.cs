using System;
using System.Linq;
using NDS_WoodLibrary.NDS2018;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NDS_WoodLibrary.Tests
{
    /// <summary>AWC TR12 (2015) general dowel equations with a gap: its Example 3.1 as printed, and nails with no gap equal to NDS-2018.</summary>
    [TestClass]
    public class GeneralDowelTests
    {
        /// <summary>
        /// TR12 Example 3.1, bolted connection with gap: 1/2" bolt, Fyb 45,000 psi, Fe∥ 4800, Fe⊥ 2550 psi, Ls = Lm = 1.5";
        /// Rd (NDS Table 12.3.1B) 4 Kθ for I, 3.6 Kθ for II, 3.2 Kθ for III and IV, Kθ = 1.25 when either member is loaded
        /// perpendicular. P/Rd for Im, Is, II, IIIm, IIIs, IV as printed, for g = 0, 1/4", 1/2".
        /// </summary>
        [TestMethod]
        public void Tr12_Example3_1_BoltWithGap_AsPrinted()
        {
            double d = 0.5, m = GeneralDowel.MomentResistance(45000, d);
            // (main ∥?, side ∥?) → rows for g = 0, 0.25, 0.5
            var rows = new (bool mainPar, bool sidePar, double g, double[] printed)[]
            {
                (true, true, 0, new double[] { 900, 900, 414, 550, 550, 663 }),
                (true, false, 0, new double[] { 720, 383, 250, 380, 324, 442 }),
                (false, false, 0, new double[] { 383, 383, 176, 289, 289, 387 }),
                (true, true, 0.25, new double[] { 900, 900, 370, 482, 482, 576 }),
                (true, false, 0.25, new double[] { 720, 383, 224, 341, 284, 393 }),
                (false, false, 0.25, new double[] { 383, 383, 157, 258, 258, 349 }),
                (true, true, 0.5, new double[] { 900, 900, 333, 426, 426, 501 }),
                (true, false, 0.5, new double[] { 720, 383, 202, 307, 250, 350 }),
                (false, false, 0.5, new double[] { 383, 383, 142, 231, 231, 315 }),
            };
            foreach (var r in rows)
            {
                double k = r.mainPar && r.sidePar ? 1.0 : 1.25;
                var rd = new[] { 4 * k, 4 * k, 3.6 * k, 3.2 * k, 3.2 * k, 3.2 * k };
                double qm = (r.mainPar ? 4800 : 2550) * d, qs = (r.sidePar ? 4800 : 2550) * d;
                var z = GeneralDowel.SingleShear(qs, qm, 1.5, 1.5, r.g, m, m, rd);
                for (int i = 0; i < 6; i++)
                    Assert.AreEqual(r.printed[i], Math.Round(z[i], MidpointRounding.AwayFromZero), $"main {(r.mainPar ? "∥" : "⊥")} side {(r.sidePar ? "∥" : "⊥")} g {r.g}: mode {i}");
                Assert.AreEqual(r.printed.Min(), Math.Round(z.Min(), MidpointRounding.AwayFromZero), "Z");
            }
        }

        /// <summary>With no gap TR12 is the NDS yield limit equations: nails as Table 12N, mode by mode.</summary>
        [TestMethod]
        public void Nails_NoGap_TheNdsEquations()
        {
            foreach (var d in new[] { 0.131, 0.148, 0.162, 0.177, 0.192 })
                foreach (var g in new[] { 0.42, 0.50, 0.55 })
                    foreach (var ts in new[] { 0.75, 1.5 })
                    {
                        Nails.SingleShearZ(d, ts, 10 * d, g, g, out var nds);
                        Nails.SingleShearZWithGap(d, ts, 0, 10 * d, g, g, out var tr12);
                        for (int i = 0; i < 6; i++) Assert.AreEqual(nds[i], tr12[i], 1e-6 * nds[i], $"{d}\" G {g} ts {ts}: mode {i}");
                    }
        }

        /// <summary>A gap only lowers it; under 6D past the gap, no value.</summary>
        [TestMethod]
        public void Nails_AGap_LowersIt()
        {
            double d = 0.162, z0 = Nails.SingleShearZWithGap(d, 1.5, 0, 10 * d, 0.5, 0.5).Value;
            double z = Nails.SingleShearZWithGap(d, 1.5, 0.75, 10 * d, 0.5, 0.5).Value;
            Assert.IsTrue(z < z0, $"{z:0} < {z0:0}");
            Assert.IsNull(Nails.SingleShearZWithGap(d, 1.5, 0.75, 5 * d, 0.5, 0.5));
        }
    }
}
