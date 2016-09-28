using System;
using NUnit.Framework;
using DPA_Musicsheets;

namespace DPA_Test {
    [Category("D_Bar")]
    public class test_D_Bar {
        [Test]
        public void TestAddNote()
        {
            D_Bar uut;
            D_Note returned;

            uut = new D_Bar(4);
            uut.addNote(new D_Note(NoteLevel.C, 12, NoteTie.none));
            returned = uut.addNote(new D_Note(NoteLevel.C, 12, NoteTie.none));
            Assert.AreEqual(8, returned.length);
            Assert.AreEqual(NoteLevel.C, returned.level);

            uut = new D_Bar(2);
            returned = uut.addNote(new D_Note(NoteLevel.C, 12, NoteTie.none));
            Assert.AreEqual(4, returned.length);
            Assert.AreEqual(NoteLevel.C, returned.level);
        }
    }
}
