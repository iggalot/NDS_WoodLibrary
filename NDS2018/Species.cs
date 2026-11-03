using System;

namespace NDS_WoodLibrary.NDS2018
{
    /// <summary>The species combinations of the framing lumber the programs offer (NDS Supplement 2018, Tables 4A and 4B).</summary>
    public enum Species { DouglasFirLarch, HemFir, SprucePineFir, SouthernPine }

    /// <summary>Visually graded dimension lumber grades (NDS Supplement 2018, Tables 4A and 4B).</summary>
    public enum Grade { No2, No3, Stud }

    public static class SpeciesInfo
    {
        /// <summary>
        /// Specific gravity G for the fastener tables (NDS-2018 Table 12.3.3A): DF-L 0.50, Hem-Fir 0.43, SPF 0.42,
        /// Southern Pine 0.55.
        /// </summary>
        public static double SpecificGravity(Species s)
        {
            switch (s)
            {
                case Species.DouglasFirLarch: return 0.50;
                case Species.HemFir: return 0.43;
                case Species.SprucePineFir: return 0.42;
                case Species.SouthernPine: return 0.55;
                default: throw new ArgumentOutOfRangeException(nameof(s));
            }
        }

        public static string Name(Species s)
        {
            switch (s)
            {
                case Species.DouglasFirLarch: return "Douglas Fir-Larch";
                case Species.HemFir: return "Hem-Fir";
                case Species.SprucePineFir: return "Spruce-Pine-Fir";
                case Species.SouthernPine: return "Southern Pine";
                default: throw new ArgumentOutOfRangeException(nameof(s));
            }
        }

        public static string Abbreviation(Species s)
        {
            switch (s)
            {
                case Species.DouglasFirLarch: return "DF-L";
                case Species.HemFir: return "HF";
                case Species.SprucePineFir: return "SPF";
                case Species.SouthernPine: return "SP";
                default: throw new ArgumentOutOfRangeException(nameof(s));
            }
        }

        public static string Name(Grade g) => g == Grade.No2 ? "No. 2" : g == Grade.No3 ? "No. 3" : "Stud";
    }
}
