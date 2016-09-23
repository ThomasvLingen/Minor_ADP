using System;
using DPA_Musicsheets;
using NUnit.Framework;

namespace DPA_Test {
    [Category("D_Staff")]
    public class test_D_Staff {
        [Test]
        public void TestMeasures()
        {
            D_Staff staff = new D_Staff();
            staff.addMeasure(0, 4, 4);
            staff.addMeasure(500, 8, 8);
            staff.addMeasure(1000, 11, 12);

            Assert.AreEqual(new Tuple<int, int>(4, 4), staff.getMeasure(0));
            Assert.AreEqual(new Tuple<int, int>(4, 4), staff.getMeasure(250));
            Assert.AreEqual(new Tuple<int, int>(8, 8), staff.getMeasure(500));
            Assert.AreEqual(new Tuple<int, int>(8, 8), staff.getMeasure(750));
            Assert.AreEqual(new Tuple<int, int>(11, 12), staff.getMeasure(1200));
        }
    }
}
