using NDS_WoodLibrary.NDS2018;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NDS_WoodLibrary.Tests
{
    /// <summary>NDS-2018 Table 12E (1-1/2" side member to concrete, Z∥) and the species' specific gravities, as printed.</summary>
    [TestClass]
    public class BoltTests
    {
        [TestMethod]
        public void Table12E_SillToConcrete_AsPrinted()
        {
            Assert.AreEqual(680, Bolts.SillToConcreteZ(0.5, Species.SouthernPine));
            Assert.AreEqual(650, Bolts.SillToConcreteZ(0.5, Species.DouglasFirLarch));
            Assert.AreEqual(590, Bolts.SillToConcreteZ(0.5, Species.HemFir));
            Assert.AreEqual(590, Bolts.SillToConcreteZ(0.5, Species.SprucePineFir));
            Assert.AreEqual(970, Bolts.SillToConcreteZ(0.625, Species.SouthernPine));
            Assert.AreEqual(930, Bolts.SillToConcreteZ(0.625, Species.DouglasFirLarch));
            Assert.AreEqual(860, Bolts.SillToConcreteZ(0.625, Species.HemFir));
            Assert.AreEqual(850, Bolts.SillToConcreteZ(0.625, Species.SprucePineFir));
            Assert.ThrowsExactly<System.ArgumentOutOfRangeException>(() => Bolts.SillToConcreteZ(0.75, Species.DouglasFirLarch));
        }

        [TestMethod]
        public void SpecificGravity_Table12_3_3A()
        {
            Assert.AreEqual(0.50, SpeciesInfo.SpecificGravity(Species.DouglasFirLarch));
            Assert.AreEqual(0.43, SpeciesInfo.SpecificGravity(Species.HemFir));
            Assert.AreEqual(0.42, SpeciesInfo.SpecificGravity(Species.SprucePineFir));
            Assert.AreEqual(0.55, SpeciesInfo.SpecificGravity(Species.SouthernPine));
        }
    }
}
