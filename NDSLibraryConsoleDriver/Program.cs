using NDS_WoodLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDSLibraryConsoleDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            NDS_2015 nds = new NDS_2015(WoodTypes.WOODTYPE_SYP,WoodGrades.WOODGRADE_NO2,WoodSizes.WOOD_SIZE_2x4);
            Console.Write(nds.ToString());

            nds = new NDS_2015(WoodTypes.WOODTYPE_SYP, WoodGrades.WOODGRADE_NO2, WoodSizes.WOOD_SIZE_6x6);
            Console.WriteLine(nds.ToString());

            nds = new NDS_2015(WoodTypes.WOODTYPE_DF, WoodGrades.WOODGRADE_NO2, WoodSizes.WOOD_SIZE_6x6);
            Console.WriteLine(nds.ToString());
        }
    }
}
