using NDS_WoodLibrary.NDS2018;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NDS_WoodLibrary.Tests
{
    /// <summary>NDS Supplement 2018 Tables 4A / 4B and 1B, and NDS-2018 2.3.2, 3.7.1 and 3.9.2, against hand calculations.</summary>
    [TestClass]
    public class MemberTests
    {
        private const double Tol = 1e-3;

        [TestMethod]
        public void Table4A_RowsAsPrinted()
        {
            var df = Table4.Lookup(Species.DouglasFirLarch, Grade.No2, "2x6");
            Assert.AreEqual(900, df.Fb); Assert.AreEqual(575, df.Ft); Assert.AreEqual(180, df.Fv);
            Assert.AreEqual(625, df.FcPerp); Assert.AreEqual(1350, df.Fc); Assert.AreEqual(1600000, df.E); Assert.AreEqual(580000, df.Emin);
            Assert.AreEqual(800, Table4.Lookup(Species.HemFir, Grade.Stud, "2x4").Fc);
            Assert.AreEqual(1150, Table4.Lookup(Species.SprucePineFir, Grade.No2, "2x4").Fc);
            Assert.AreEqual(440000, Table4.Lookup(Species.SprucePineFir, Grade.No3, "2x6").Emin);
        }

        [TestMethod]
        public void Table4A_StudEightInchIsNo3()
        {
            // the Stud row: "use No. 3 Grade design values and size factors" for 8" and wider
            var v = Table4.Lookup(Species.DouglasFirLarch, Grade.Stud, "2x8");
            Assert.AreEqual(775, v.Fc);
            Assert.AreEqual(525, v.Fb);
            var cf = Table4.SizeFactor(Species.DouglasFirLarch, Grade.Stud, "2x8");
            Assert.AreEqual(1.2, cf.Fb); Assert.AreEqual(1.05, cf.Fc);
        }

        [TestMethod]
        public void Table4A_SizeFactors()
        {
            var a = Table4.SizeFactor(Species.DouglasFirLarch, Grade.No2, "2x4");
            Assert.AreEqual(1.5, a.Fb); Assert.AreEqual(1.5, a.Ft); Assert.AreEqual(1.15, a.Fc);
            var b = Table4.SizeFactor(Species.HemFir, Grade.No3, "2x6");
            Assert.AreEqual(1.3, b.Fb); Assert.AreEqual(1.1, b.Fc);
            var c = Table4.SizeFactor(Species.SprucePineFir, Grade.Stud, "2x4");
            Assert.AreEqual(1.1, c.Fb); Assert.AreEqual(1.05, c.Fc);
            var d = Table4.SizeFactor(Species.SprucePineFir, Grade.Stud, "2x6");
            Assert.AreEqual(1.0, d.Fb); Assert.AreEqual(1.0, d.Fc);
        }

        [TestMethod]
        public void Table4B_SouthernPine_SizeSpecific()
        {
            Assert.AreEqual(1100, Table4.Lookup(Species.SouthernPine, Grade.No2, "2x4").Fb);
            Assert.AreEqual(1400, Table4.Lookup(Species.SouthernPine, Grade.No2, "2x6").Fc);
            Assert.AreEqual(800, Table4.Lookup(Species.SouthernPine, Grade.Stud, "2x6").Fc);  // "No. 3 and Stud"
            Assert.AreEqual(1.0, Table4.SizeFactor(Species.SouthernPine, Grade.No2, "2x4").Fb);
        }

        [TestMethod]
        public void Section_Table1B()
        {
            var s = Section.Of("2x6");
            Assert.AreEqual(8.25, s.A, Tol);
            Assert.AreEqual(7.5625, s.Sx, Tol);    // 1.5 × 5.5² / 6
            Assert.AreEqual(20.797, s.Ix, Tol);    // 1.5 × 5.5³ / 12
            Assert.AreEqual(13.141, Section.Of("2x8").Sx, Tol);
            Assert.AreEqual(13.875, Section.Of("2x10").A, Tol);   // 1.5 × 9.25
            Assert.AreEqual(16.875, Section.Of("2x12").A, Tol);   // 1.5 × 11.25
        }

        [TestMethod]
        public void SizeFactors_TenAndTwelveWide_Table4A()
        {
            var ten = Table4.SizeFactor(Species.DouglasFirLarch, Grade.No2, "2x10");
            Assert.AreEqual(1.1, ten.Fb); Assert.AreEqual(1.1, ten.Ft); Assert.AreEqual(1.0, ten.Fc);
            var twelve = Table4.SizeFactor(Species.HemFir, Grade.No2, "2x12");
            Assert.AreEqual(1.0, twelve.Fb); Assert.AreEqual(1.0, twelve.Ft); Assert.AreEqual(1.0, twelve.Fc);
            Assert.AreEqual(575, Table4.Lookup(Species.DouglasFirLarch, Grade.No2, "2x10").Ft);
            Assert.AreEqual(325, Table4.Lookup(Species.DouglasFirLarch, Grade.Stud, "2x10").Ft, "Stud 8\" and wider: No. 3's");
            // Table 4B's wide rows (SFPA Use Guide 2013 Table 1, the June 1, 2013 values), typed here from the page
            var sp10 = Table4.Lookup(Species.SouthernPine, Grade.No2, "2x10");
            Assert.AreEqual(800, sp10.Fb); Assert.AreEqual(475, sp10.Ft); Assert.AreEqual(1300, sp10.Fc); Assert.AreEqual(1400000, sp10.E);
            var sp12 = Table4.Lookup(Species.SouthernPine, Grade.No2, "2x12");
            Assert.AreEqual(750, sp12.Fb); Assert.AreEqual(450, sp12.Ft); Assert.AreEqual(1250, sp12.Fc);
            var st10 = Table4.Lookup(Species.SouthernPine, Grade.Stud, "2x10");
            Assert.AreEqual(475, st10.Fb); Assert.AreEqual(275, st10.Ft); Assert.AreEqual(750, st10.Fc);
            var n312 = Table4.Lookup(Species.SouthernPine, Grade.No3, "2x12");
            Assert.AreEqual(450, n312.Fb); Assert.AreEqual(250, n312.Ft); Assert.AreEqual(725, n312.Fc); Assert.AreEqual(470000, n312.Emin);
            Assert.AreEqual(1.0, Table4.SizeFactor(Species.SouthernPine, Grade.No2, "2x12").Ft, "size-specific: CF 1.0");
        }

        /// <summary>
        /// Every size the tables hold, for any 2x member (studs, rims, joists): each has a section, values and a size factor,
        /// No. 2 DF-L's Ft × CF by Table 4A: 2x4 1.5, 2x6 1.3, 2x8 1.2, 2x10 1.1, 2x12 1.0.
        /// </summary>
        [TestMethod]
        public void EverySize_TwoByFourToTwoByTwelve_ForAnyMember()
        {
            CollectionAssert.AreEqual(new[] { "2x4", "2x6", "2x8", "2x10", "2x12" }, Table4.Sizes);
            var ft = new[] { 1.5, 1.3, 1.2, 1.1, 1.0 };
            var depth = new[] { 3.5, 5.5, 7.25, 9.25, 11.25 };
            for (int i = 0; i < Table4.Sizes.Length; i++)
            {
                string n = Table4.Sizes[i];
                Assert.AreEqual(depth[i], Section.Of(n).D, Tol, n);
                Assert.AreEqual(ft[i], Table4.SizeFactor(Species.DouglasFirLarch, Grade.No2, n).Ft, n);
                foreach (var sp in new[] { Species.DouglasFirLarch, Species.HemFir, Species.SprucePineFir })
                    Assert.IsTrue(Table4.Lookup(sp, Grade.No2, n).Ft > 0, $"{sp} {n}");
            }
            foreach (var n in Table4.SizesOf(Species.SouthernPine)) Assert.IsTrue(Table4.Lookup(Species.SouthernPine, Grade.No2, n).Ft > 0, n);
        }

        [TestMethod]
        public void LoadDuration_Table2_3_2()
        {
            Assert.AreEqual(0.9, Adjustments.CD(LoadDuration.Permanent));
            Assert.AreEqual(1.0, Adjustments.CD(LoadDuration.TenYears));
            Assert.AreEqual(1.15, Adjustments.CD(LoadDuration.TwoMonths));
            Assert.AreEqual(1.25, Adjustments.CD(LoadDuration.SevenDays));
            Assert.AreEqual(1.6, Adjustments.CD(LoadDuration.TenMinutes));
        }

        [TestMethod]
        public void ColumnStability_HandCalc()
        {
            // DF-L No. 2 2x6, le = 175.5" (14'-7 1/2"): le/d = 31.909; FcE = 0.822 × 580,000 / 31.909² = 468.24 psi;
            // Fc* = 1350 × 1.6 × 1.1 = 2376 psi; r = 0.19707, a = 1.19707 / 1.6 = 0.74817; CP = a − √(a² − r/0.8) = 0.18833
            double fcE = Adjustments.FcE(580000, 175.5 / 5.5);
            Assert.AreEqual(468.243, fcE, Tol);
            double cp = Adjustments.CP(2376, fcE);
            Assert.AreEqual(0.18833, cp, 1e-5);
            // DF-L No. 2 2x4, 8 ft, CD 1.0: FcE = 0.822 × 580,000 / (96/3.5)² = 633.71; Fc* = 1350 × 1.15 = 1552.5; CP = 0.36595
            Assert.AreEqual(0.36595, Adjustments.CP(1552.5, Adjustments.FcE(580000, 96 / 3.5)), 1e-5);
        }

        [TestMethod]
        public void CombinedBendingAndCompression_Eq3_9_3()
        {
            // fc = 100, Fc' = 400, fb = 600, Fb' = 1500, FcE = 500: (0.25)² + 600 / (1500 × 0.8) = 0.0625 + 0.5 = 0.5625
            Assert.AreEqual(0.5625, Adjustments.CombinedCompressionBending(100, 400, 600, 1500, 500), 1e-9);
            Assert.AreEqual(double.PositiveInfinity, Adjustments.CombinedCompressionBending(500, 400, 600, 1500, 500));
            Assert.AreEqual(0.0625, Adjustments.CombinedCompressionBending(100, 400, 0, 1500, 500), 1e-9);
        }
    }
}
