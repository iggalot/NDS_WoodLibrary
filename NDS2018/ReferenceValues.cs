using System;
using System.Collections.Generic;

namespace NDS_WoodLibrary.NDS2018
{
    /// <summary>Reference design values (psi) of a visually graded dimension lumber grade, before any adjustment factor.</summary>
    public sealed class ReferenceValues
    {
        public double Fb { get; set; }
        public double Ft { get; set; }
        public double Fv { get; set; }
        public double FcPerp { get; set; }
        public double Fc { get; set; }
        public double E { get; set; }
        public double Emin { get; set; }
        /// <summary>Where the row is from, and anything substituted ("Stud 2x8: the No. 3 values, Table 4A footnote").</summary>
        public string Source { get; set; }
    }

    /// <summary>The size factors CF of Table 4A (applied to Fb, Ft and Fc); 1.0 for Table 4B's size-specific values.</summary>
    public sealed class SizeFactors
    {
        public double Fb { get; set; } = 1;
        public double Ft { get; set; } = 1;
        public double Fc { get; set; } = 1;
    }

    /// <summary>
    /// NDS Supplement 2018 Table 4A (Douglas Fir-Larch, Hem-Fir, Spruce-Pine-Fir; 2" to 4" thick, 2" and wider) and Table
    /// 4B (Southern Pine, size-specific values), for the grades framing uses: No. 2, No. 3 and Stud. Values were read from
    /// AWC's 2018 Supplement.
    /// </summary>
    public static class Table4
    {
        public const string Source4A = "NDS Supplement 2018, Table 4A (visually graded dimension lumber), with its size factors CF.";
        public const string Source4B = "NDS Supplement 2018, Table 4B (visually graded Southern Pine dimension lumber, size-specific values; CF = 1.0 for 2\" to 12\" wide).";

        private static ReferenceValues V(double fb, double ft, double fv, double fcp, double fc, double e, double emin) =>
            new ReferenceValues { Fb = fb, Ft = ft, Fv = fv, FcPerp = fcp, Fc = fc, E = e, Emin = emin };

        // Table 4A: species → grade → values (psi)
        private static readonly Dictionary<Species, Dictionary<Grade, ReferenceValues>> T4A = new Dictionary<Species, Dictionary<Grade, ReferenceValues>>
        {
            { Species.DouglasFirLarch, new Dictionary<Grade, ReferenceValues> {
                { Grade.No2,  V(900, 575, 180, 625, 1350, 1600000, 580000) },
                { Grade.No3,  V(525, 325, 180, 625,  775, 1400000, 510000) },
                { Grade.Stud, V(700, 450, 180, 625,  850, 1400000, 510000) } } },
            { Species.HemFir, new Dictionary<Grade, ReferenceValues> {
                { Grade.No2,  V(850, 525, 150, 405, 1300, 1300000, 470000) },
                { Grade.No3,  V(500, 300, 150, 405,  725, 1200000, 440000) },
                { Grade.Stud, V(675, 400, 150, 405,  800, 1200000, 440000) } } },
            { Species.SprucePineFir, new Dictionary<Grade, ReferenceValues> {
                { Grade.No2,  V(875, 450, 135, 425, 1150, 1400000, 510000) },   // the "No. 1/No. 2" row
                { Grade.No3,  V(500, 250, 135, 425,  650, 1200000, 440000) },
                { Grade.Stud, V(675, 350, 135, 425,  725, 1200000, 440000) } } },
        };

        // Table 4B, Southern Pine: grade → nominal size → values (psi); "No. 3 and Stud" is one row. The 2x10 and 2x12 rows
        // (2026-11-09) from SFPA's Southern Pine Use Guide 2013, Table 1 (SPIB values effective June 1, 2013, the values Table
        // 4B gives), which matches every narrower row read here before.
        private static readonly Dictionary<Grade, Dictionary<string, ReferenceValues>> T4B = new Dictionary<Grade, Dictionary<string, ReferenceValues>>
        {
            { Grade.No2, new Dictionary<string, ReferenceValues> {
                { "2x4", V(1100, 675, 175, 565, 1450, 1400000, 510000) },
                { "2x6", V(1000, 600, 175, 565, 1400, 1400000, 510000) },
                { "2x8", V( 925, 550, 175, 565, 1350, 1400000, 510000) },
                { "2x10", V(800, 475, 175, 565, 1300, 1400000, 510000) },
                { "2x12", V(750, 450, 175, 565, 1250, 1400000, 510000) } } },
            { Grade.No3, new Dictionary<string, ReferenceValues> {
                { "2x4", V(650, 400, 175, 565, 850, 1300000, 470000) },
                { "2x6", V(575, 350, 175, 565, 800, 1300000, 470000) },
                { "2x8", V(525, 325, 175, 565, 775, 1300000, 470000) },
                { "2x10", V(475, 275, 175, 565, 750, 1300000, 470000) },
                { "2x12", V(450, 250, 175, 565, 725, 1300000, 470000) } } },
        };

        /// <summary>
        /// The nominal sizes these tables are read for, smallest first: any 2x member (studs, plates, rims, floor and ceiling
        /// joists, rafters) takes its values here. Table 4A's species: all of them; Southern Pine (Table 4B): 2x4 to 2x12
        /// (<see cref="SouthernPineSizes"/>).
        /// </summary>
        public static readonly string[] Sizes = { "2x4", "2x6", "2x8", "2x10", "2x12" };
        public static readonly string[] SouthernPineSizes = { "2x4", "2x6", "2x8", "2x10", "2x12" };

        /// <summary>The sizes <paramref name="species"/> has values for here.</summary>
        public static string[] SizesOf(Species species) => species == Species.SouthernPine ? SouthernPineSizes : Sizes;

        /// <summary>
        /// The reference values of <paramref name="grade"/> <paramref name="species"/> in <paramref name="nominal"/> ("2x4",
        /// "2x6", "2x8", "2x10", "2x12"; Table 4A's with their size factors, Table 4B's size-specific). Table 4A, Stud grade 8" and wider: "use No. 3 Grade design values and size factors" (the
        /// table's note on the Stud row), so a Stud 2x8 is given the No. 3 values.
        /// </summary>
        public static ReferenceValues Lookup(Species species, Grade grade, string nominal)
        {
            Section.Of(nominal); // a size it has
            ReferenceValues r;
            if (species == Species.SouthernPine)
            {
                var g = grade == Grade.Stud ? Grade.No3 : grade;
                if (!T4B[g].ContainsKey(nominal))
                    throw new ArgumentException($"Table 4B's Southern Pine {nominal} values are not read into this library (2x4 to 2x12).", nameof(nominal));
                r = Copy(T4B[g][nominal]);
                r.Source = Source4B + (grade == Grade.Stud ? " The \"No. 3 and Stud\" row." : "");
                return r;
            }
            bool studAsNo3 = grade == Grade.Stud && Width(nominal) >= 8;
            r = Copy(T4A[species][studAsNo3 ? Grade.No3 : grade]);
            r.Source = Source4A + (studAsNo3 ? " Stud grade 8\" and wider: the No. 3 values and size factors (Table 4A)." : "");
            return r;
        }

        /// <summary>
        /// Table 4A size factors CF by grade and width (2" and 3" thick): No. 2 and No. 3: 2", 3", 4" wide Fb 1.5, Ft 1.5,
        /// Fc 1.15; 6" 1.3, 1.3, 1.1; 8" 1.2, 1.2, 1.05; 10" 1.1, 1.1, 1.0; 12" 1.0, 1.0, 1.0. Stud: 2", 3", 4" 1.1, 1.1, 1.05; 5", 6" 1.0, 1.0, 1.0; 8" and
        /// wider: No. 3's. Southern Pine (Table 4B): 1.0 (the values are size-specific).
        /// </summary>
        public static SizeFactors SizeFactor(Species species, Grade grade, string nominal)
        {
            int w = Width(nominal);
            if (species == Species.SouthernPine) return new SizeFactors();
            if (grade == Grade.Stud && w < 8)
                return w <= 4 ? new SizeFactors { Fb = 1.1, Ft = 1.1, Fc = 1.05 } : new SizeFactors();
            if (w <= 4) return new SizeFactors { Fb = 1.5, Ft = 1.5, Fc = 1.15 };
            if (w <= 6) return new SizeFactors { Fb = 1.3, Ft = 1.3, Fc = 1.1 };
            if (w <= 8) return new SizeFactors { Fb = 1.2, Ft = 1.2, Fc = 1.05 };
            if (w <= 10) return new SizeFactors { Fb = 1.1, Ft = 1.1, Fc = 1.0 };
            if (w <= 12) return new SizeFactors();
            throw new ArgumentOutOfRangeException(nameof(nominal), "Size factors here go to 12\" wide.");
        }

        private static int Width(string nominal) => int.Parse(nominal.Substring(nominal.IndexOf('x') + 1));

        private static ReferenceValues Copy(ReferenceValues v) => V(v.Fb, v.Ft, v.Fv, v.FcPerp, v.Fc, v.E, v.Emin);
    }
}
