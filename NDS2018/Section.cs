using System;

namespace NDS_WoodLibrary.NDS2018
{
    /// <summary>
    /// A dressed (S4S) sawn lumber section, NDS Supplement 2018 Table 1B: actual b × d (in), area, section modulus and
    /// moment of inertia about the strong (x-x) axis.
    /// </summary>
    public sealed class Section
    {
        public string Nominal { get; private set; }
        public double B { get; private set; }   // in, the thin side
        public double D { get; private set; }   // in, the depth
        public double A => B * D;
        public double Sx => B * D * D / 6;
        public double Ix => B * D * D * D / 12;

        /// <summary>2x4 1-1/2 × 3-1/2, 2x6 1-1/2 × 5-1/2, 2x8 1-1/2 × 7-1/4 (Table 1B).</summary>
        public static Section Of(string nominal)
        {
            switch (nominal)
            {
                case "2x4": return new Section { Nominal = nominal, B = 1.5, D = 3.5 };
                case "2x6": return new Section { Nominal = nominal, B = 1.5, D = 5.5 };
                case "2x8": return new Section { Nominal = nominal, B = 1.5, D = 7.25 };
                default: throw new ArgumentException($"Not a size here: \"{nominal}\" (2x4, 2x6, 2x8).", nameof(nominal));
            }
        }
    }
}
