using System;
using DPA_Musicsheets;
using NUnit.Framework;

namespace DPA_Test {
    [Category("D_Staff")]
    public class test_D_Staff {
        [Test]
        public void TestGetMeasures()
        {
            D_Staff staff = new D_Staff();
            staff.addMeasure(4, 4, 0, 500);
            staff.addMeasure(8, 8, 500, 1000);
            staff.addMeasure(11, 12, 1000, 2000);

            D_Measure test_measure;

            // 4,4 at 0
            test_measure = staff.getMeasure(0);
            Assert.AreEqual(test_measure.beats_per_bar, 4);
            Assert.AreEqual(test_measure.beat_length, 4);

            // 4,4 at 250
            test_measure = staff.getMeasure(250);
            Assert.AreEqual(test_measure.beats_per_bar, 4);
            Assert.AreEqual(test_measure.beat_length, 4);

            // 8,8 at 500
            test_measure = staff.getMeasure(500);
            Assert.AreEqual(test_measure.beats_per_bar, 8);
            Assert.AreEqual(test_measure.beat_length, 8);

            // 8,8 at 750
            test_measure = staff.getMeasure(750);
            Assert.AreEqual(test_measure.beats_per_bar, 8);
            Assert.AreEqual(test_measure.beat_length, 8);

            // 11,12 at 1200
            test_measure = staff.getMeasure(1200);
            Assert.AreEqual(test_measure.beats_per_bar, 11);
            Assert.AreEqual(test_measure.beat_length, 12);
        }

        [Test]
        public void TestSetMeasureEndTimes()
        {
            D_Staff staff = new D_Staff();
            staff.num_of_beats = 1000;

            staff.addMeasure(4, 4, 0);
            staff.addMeasure(4, 4, 500);
            staff.addMeasure(4, 4, 750);

            staff.setMeasureEndTimes();

            Assert.AreEqual(500, staff.measures[0].end_beat);
            Assert.AreEqual(750, staff.measures[1].end_beat);
            Assert.AreEqual(1001, staff.measures[2].end_beat);
        }
    }
}
