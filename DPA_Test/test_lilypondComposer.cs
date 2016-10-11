using System;
using NUnit.Framework;
using DPA_Musicsheets;

namespace DPA_Test {
    [Category("lilypondComposer")]
    public class test_lilypondComposer {
        [Test]
        public void staffInformationTest()
        {
            LilypondComposer comp = new LilypondComposer();
            D_Staff staff = new D_Staff();
            staff.tempo = 120;
            staff.clef = clef.g_key;
            staff.addMeasure(2, 4, 0);
            D_Bar bar = new D_Bar(2);
            bar.addNote(new D_Note(NoteLevel.A, 1, 4));
            staff.addBar(bar);
            Assert.AreEqual("\\relative c{\r\n\\clef treble\r\n\\tempo 4=120\r\n\\time 2/4\r\n", comp.composeLilypondFromStaff(staff));
        }
    }
}
