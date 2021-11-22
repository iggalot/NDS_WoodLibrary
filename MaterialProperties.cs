using System;
using System.Collections.Generic;

namespace NDS_WoodLibrary
{
    public class TableEntry
    {
        public float Fb;
        public float Ft;
        public float Fv;
        public float Fc_perp;
        public float Fc;
        public float E;
        public float Emin;
        public float G;

        public TableEntry(float fb, float ft, float fv, float fc_perp, float fc, float e, float emin, float g)
        {
            Fb = fb;
            Ft = ft;
            Fv = fv;
            Fc_perp = fc_perp;
            Fc = fc;
            E = e;
            Emin = emin;
            G = g;
        }
    };



    public class MaterialProperties
    {
        // NDS2015 Table 4B pg 40
        public TableEntry[] SYP_2to4wide = new TableEntry[]{
             new TableEntry(2700, 1900, 175,    660,    2050,   1900000,    690000, 0.55f),
             new TableEntry(2350, 1650, 175,    565,    1900,   1800000,    660000, 0.55f),
             new TableEntry(2050, 1450, 175,    480,    1800,   1600000,    580000, 0.55f),
             new TableEntry(1650, 1100, 175,    660,    1750,   1800000,    660000, 0.55f),
             new TableEntry(1500, 1000, 175,    565,    1650,   1600000,    580000, 0.55f),
             new TableEntry(1300,  875, 175,    480,    1550,   1400000,    510000, 0.55f),
             new TableEntry(1200,  750, 175,    660,    1500,   1600000,    580000, 0.55f),
             new TableEntry(1100,  675, 175,    565,    1450,   1400000,    510000, 0.55f),
             new TableEntry(1050,  600, 175,    480,    1450,   1300000,    470000, 0.55f),
             new TableEntry(650,   400, 175,    565,     850,   1300000,    470000, 0.55f)
        };

        // NDS2015 Table 4B pg 40
        public TableEntry[] SYP_4wide = new TableEntry[]{
             new TableEntry(875, 500, 175,    565,    1600,   1400000,    510000, 0.55f),
             new TableEntry(475, 275, 175,    565,    1300,   1200000,    440000, 0.55f),
             new TableEntry(225, 125, 175,    565,    850,    1200000,    440000, 0.55f),
        };

        // NDS2015 Table 4B pg 40
        public TableEntry[] SYP_5to6wide = new TableEntry[]{
             new TableEntry(2400, 1650, 175,    660,    1900,   1900000,    690000, 0.55f),
             new TableEntry(2100, 1450, 175,    565,    1800,   1800000,    660000, 0.55f),
             new TableEntry(1850, 1300, 175,    480,    1700,   1600000,    580000, 0.55f),
             new TableEntry(1500, 1000, 175,    660,    1650,   1800000,    660000, 0.55f),
             new TableEntry(1350,  875, 175,    565,    1550,   1600000,    580000, 0.55f),
             new TableEntry(1200,  775, 175,    480,    1450,   1400000,    510000, 0.55f),
             new TableEntry(1050,  650, 175,    660,    1450,   1600000,    580000, 0.55f),
             new TableEntry(1000,  600, 175,    565,    1400,   1400000,    510000, 0.55f),
             new TableEntry( 950,  525, 175,    480,    1350,   1300000,    470000, 0.55f),
             new TableEntry( 575,  350, 175,    565,     800,   1300000,    470000, 0.55f)
        };

        // NDS2015 Table 4B pg 40
        public TableEntry[] SYP_8wide = new TableEntry[]{
             new TableEntry(2200, 1550, 175,    660,    1850,   1900000,    690000, 0.55f),
             new TableEntry(1950, 1350, 175,    565,    1700,   1800000,    660000, 0.55f),
             new TableEntry(1700, 1200, 175,    480,    1650,   1600000,    580000, 0.55f),
             new TableEntry(1350,  900, 175,    660,    1600,   1800000,    660000, 0.55f),
             new TableEntry(1250,  800, 175,    565,    1500,   1600000,    580000, 0.55f),
             new TableEntry(1100,  700, 175,    480,    1400,   1400000,    510000, 0.55f),
             new TableEntry( 975,  600, 175,    660,    1400,   1600000,    580000, 0.55f),
             new TableEntry( 925,  550, 175,    565,    1350,   1400000,    510000, 0.55f),
             new TableEntry( 875,  500, 175,    480,    1300,   1300000,    470000, 0.55f),
             new TableEntry( 525,  325, 175,    565,     775,   1300000,    470000, 0.55f)
        };


        private List<string> StatusMessageList { get; set; }

        public string Name { get; set; } = "Wood property name here";
        public WoodTypes Type { get; set; } = WoodTypes.WOODTYPE_UNKNOWN;
        public WoodGrades Grade { get; set; } = WoodGrades.WOODGRADE_NO2;
        public WoodLoadDurations Duration { get; set; } = WoodLoadDurations.WOOD_DUR_TENYEARS;
        public WoodTemperatureRanges TemperatureRange { get; set; } = WoodTemperatureRanges.WOOD_TEMP_LESS100F;
        public WoodSizes WoodSize { get; set; } = WoodSizes.WOOD_SIZE_2x4;

        public float Fb { get; set; }
        public float Fc { get; set; }
        public float Fv { get; set; }
        public float Ft { get; set; }
        public float Fc_perp { get; set; }
        public float E { get; set; }
        public float Emin { get; set; }
        public float SpecificGravity { get; set; }


        public float Fb_Adjusted { get; set; }
        public float Fc_Adjusted { get; set; }
        public float Fv_Adjusted { get; set; }
        public float Ft_Adjusted { get; set; }
        public float Fc_Perp_Adjusted { get; set; }
        public float E_Adjusted { get; set; }
        public float Emin_Adjusted { get; set; }

        public MaterialProperties(WoodTypes type, WoodGrades grade, WoodSizes wood_size, WoodLoadDurations dur, WoodTemperatureRanges temp)
        {
            Type = type;
            Grade = grade;
            Duration = dur;
            TemperatureRange = temp;
            WoodSize = wood_size;

            StatusMessageList = new List<string>();


            TableEntry te = SetBaseProperties(type, grade);
            if (te == null)
                throw new NotImplementedException("Table search did not return a value");
            else
            {
                Fb = te.Fb;
                Ft = te.Ft;
                Fv = te.Fv;
                Fc_perp = te.Fc_perp;
                Fc = te.Fc;
                E = te.E;
                Emin = te.Emin;
                SpecificGravity = te.G;
            }

            Fb_Adjusted = Fb*Fb_AdjFactors();
        }

        private TableEntry SetBaseProperties(WoodTypes wood_type, WoodGrades grade)
        {
            float w = GetSizeClassification();
            switch (wood_type)
            {
                case WoodTypes.WOODTYPE_SYP:
                    // Read Table 4B values
                    if (w <= 4.0f)
                    {
                        if (grade != WoodGrades.WOODGRADE_CONSTRUCTION && grade != WoodGrades.WOODGRADE_STANDARD && grade != WoodGrades.WOODGRADE_UTILITY)
                        {
                            return SYP_2to4wide[(int)grade];
                        }
                        else if (grade == WoodGrades.WOODGRADE_CONSTRUCTION || grade == WoodGrades.WOODGRADE_STANDARD || grade == WoodGrades.WOODGRADE_UTILITY)
                        {
                            if(w==4)
                                return SYP_4wide[(int)(grade - (int)WoodGrades.WOODGRADE_CONSTRUCTION)];
                        }
                        else
                        {
                            throw new NotImplementedException("Wood type: " + wood_type + " with width smaller than 4 in. is not current implemented yet");
                        }
                    }
                    else if (w<= 6.0f)
                    {
                        if (grade != WoodGrades.WOODGRADE_CONSTRUCTION && grade != WoodGrades.WOODGRADE_STANDARD && grade != WoodGrades.WOODGRADE_UTILITY)
                        {
                            return SYP_5to6wide[(int)grade];
                        }
                        else
                        {
                            throw new NotImplementedException("Wood type: " + wood_type + " with width smaller than 6 in. is not current implemented yet");
                        }
                    }
                    else if (w==8.0f)
                    {
                        if (grade != WoodGrades.WOODGRADE_CONSTRUCTION && grade != WoodGrades.WOODGRADE_STANDARD && grade != WoodGrades.WOODGRADE_UTILITY)
                        {
                            return SYP_8wide[(int)grade];
                        }
                        else
                        {
                            throw new NotImplementedException("Wood type: " + wood_type + " with width equal to 8 in. is not current implemented yet");
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException("Wood type: " + wood_type + " is not currently implemented yet");
            }

            return null;
        }

        private float Fb_AdjFactors()
        {
            float factor = 1.0f;
            string msg = "";
            // CD - load duration factor
            factor *= ComputeCD(out msg);
            StatusMessageList.Add(msg);

            // CM - wet service factor

            // Ctemp - temperature factor

            // CL - beam stability factor

            // CF - size factor

            // CFU - flat use factor

            // CI - incising factor

            // CREP - repetitive member factor

            // CP - column stability factor

            // CT - buckling stiffness factor

            // Cb - bearing area factor

            return factor;
        }

        private float ComputeCD(out string status_msg)
        {
            float factor = 0.0f;

            switch (Duration)
            {
                case WoodLoadDurations.WOOD_DUR_PERMANENT:
                    factor = 0.9f;
                    break;
                case WoodLoadDurations.WOOD_DUR_TENYEARS:
                    factor = 1.0f;
                    break;
                case WoodLoadDurations.WOOD_DUR_TWOMONTH:
                    factor = 1.15f;
                    break;
                case WoodLoadDurations.WOOD_DUR_SEVENDAYS:
                    factor = 1.25f;
                    break;
                case WoodLoadDurations.WOOD_DUR_TENMINUTES:
                    factor = 1.60f;
                    break;
                case WoodLoadDurations.WOOD_DUR_IMPACT:
                    factor = 2.0f;
                    break;
                default:
                    string str = "In ComputeCD(): " + Duration + "is not current implemented";
                    throw new NotImplementedException(str);

            }
            status_msg = "Duration: " + Duration + "  CD = " + factor.ToString();
            return factor;

        }

        /// <summary>
        /// Returns the size classification for specified wood sizes
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private float GetSizeClassification()
        {
            var width = 0.0f;
            switch (WoodSize)
            {
                case WoodSizes.WOOD_SIZE_2x4:
                case WoodSizes.WOOD_SIZE_2x6:
                case WoodSizes.WOOD_SIZE_2x8:
                case WoodSizes.WOOD_SIZE_2x10:
                case WoodSizes.WOOD_SIZE_2x12:
                    width = 2.0f;
                    break;
                case WoodSizes.WOOD_SIZE_4x4:
                    width = 4.0f;
                    break;
                case WoodSizes.WOOD_SIZE_6x6:
                    width = 6.0f;
                    break;
                default:
                    throw new NotImplementedException("In GetSizeClassigication: Wood size: " + WoodSize + " has not been implemented");
                    break;
            }

            return width;
        }

        public string ToString()
        {
            string str = "";

            str += "Fb: " + Fb.ToString();
            str += "   Ft: " + Ft.ToString();
            str += "   Fv: " + Fv.ToString();
            str += "   Fc_perp :" + Fc_perp.ToString();
            str += "\n";

            str += "   Fc: " + Fc.ToString();
            str += "   E: " + E.ToString();
            str += "   Emin: " + Emin.ToString();
            str += "   G: " + SpecificGravity.ToString();
            str += "\n";

            return str;
        }

    }
}
