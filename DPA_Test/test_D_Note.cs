using System;
using System.Text;
using System.Collections.Generic;
using DPA_Musicsheets;
using NUnit.Framework;

namespace DPA_Test {
    [Category("D_Note")]
    public class test_D_Note {

        [Test]
        public void TestSplitNote()
        {
            D_Note original_note = new D_Note(NoteLevel.C, 12, NoteTie.none);
            Tuple<D_Note, D_Note> returned = D_Note.splitNote(original_note, 8);

            Assert.AreEqual(8, returned.Item1.length);
            Assert.AreEqual(4, returned.Item2.length);
            Assert.AreEqual(NoteLevel.C, returned.Item1.level);
            Assert.AreEqual(NoteLevel.C, returned.Item2.level);
            Assert.AreEqual(NoteTie.start, returned.Item1.note_tie);
            Assert.AreEqual(NoteTie.stop, returned.Item2.note_tie);
        }
    }
}
