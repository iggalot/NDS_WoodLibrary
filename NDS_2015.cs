using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDS_WoodLibrary
{
    // Wood types
    public enum WoodTypes {
        WOODTYPE_UNKNOWN,
        WOODTYPE_SYP,  // southern yellow pine
        WOODTYPE_SPF,  // sprice pine fur
        WOODTYPE_DF,   // douglas fir
        WOODTYPE_OTHER
    }

    // Wood grades
    public enum WoodGrades
    {
        WOODGRADE_SELECT_DENSE = 0,
        WOODGRADE_SELECT = 1,
        WOODGRADE_SELECT_NONDENSE = 2,
        WOODGRADE_NO1_DENSE = 3,
        WOODGRADE_NO1 = 4,
        WOODGRADE_NO1_NONDENSE = 5,
        WOODGRADE_NO2_DENSE = 6,
        WOODGRADE_NO2 = 7,
        WOODGRADE_NO2_NONDENSE = 8,
        WOODGRADE_NO_3 = 9,
        WOODGRADE_STUD = 10,
        WOODGRADE_CONSTRUCTION = 11,
        WOODGRADE_STANDARD = 12,
        WOODGRADE_UTILITY = 13
    }

    // Sustained load durations
    public enum WoodLoadDurations
    {
        WOOD_DUR_PERMANENT,     // typical design load -- dead loads
        WOOD_DUR_TENYEARS,      // occupancy live load
        WOOD_DUR_TWOMONTH,      // snow load
        WOOD_DUR_SEVENDAYS,     // construction load
        WOOD_DUR_TENMINUTES,    // wind/earthquake load
        WOOD_DUR_IMPACT         // impact load
    }

    // Sustained temperature ranges
    public enum WoodTemperatureRanges
    {
        WOOD_TEMP_LESS100F,
        WOOD_TEMP_LESS125F,
        WOOD_TEMP_LESS150F
    }

    public enum WoodSizes
    {
        WOOD_SIZE_2x4,
        WOOD_SIZE_2x6,
        WOOD_SIZE_2x8,
        WOOD_SIZE_2x10,
        WOOD_SIZE_2x12,
        WOOD_SIZE_4x4,
        WOOD_SIZE_6x6

    }

    public class NDS_2015
    {
        const WoodTypes DEFAUL_WOODTYPE = WoodTypes.WOODTYPE_SYP;
        const WoodGrades DEFAULT_WOODGRADE = WoodGrades.WOODGRADE_NO2;
        const WoodLoadDurations DEFAULT_WOODLOADDURATION = WoodLoadDurations.WOOD_DUR_TENYEARS;
        const WoodTemperatureRanges DEFAULT_TEMPERATURE = WoodTemperatureRanges.WOOD_TEMP_LESS100F;

        public MaterialProperties Property { get; private set; }
        public bool IsWetService { get; set; } = false;
        public bool IsPressureTreated { get; set; } = false;

        public NDS_2015(WoodTypes type, WoodGrades grade, WoodSizes wood_size, WoodLoadDurations dur = WoodLoadDurations.WOOD_DUR_TENYEARS, WoodTemperatureRanges temp = WoodTemperatureRanges.WOOD_TEMP_LESS100F)
        {
            Property = new MaterialProperties(type, grade, wood_size, dur, temp);
        }

        public string ToString()
        {
            return this.Property.ToString();
        }
    }
}
