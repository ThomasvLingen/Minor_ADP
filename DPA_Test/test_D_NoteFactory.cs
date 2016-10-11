using System;
using DPA_Musicsheets;
using NUnit.Framework;

namespace DPA_Test {
    [Category("D_NoteFactory")]
    public class test_D_NoteFactory {
        [Test]
        public void Test_create_notes()
        {
            D_Note test_note;

            test_note = D_NoteFactory.create_note(NoteLevel.C, NoteAlteration.sharp, 4, 4);
            Assert.AreEqual(NoteLevel.C, test_note.level);
            Assert.AreEqual(false, test_note.is_rest);
            Assert.AreEqual(4, test_note.length);
            Assert.AreEqual(false, test_note.length_modifier);
            Assert.AreEqual(NoteAlteration.sharp, test_note.alteration);

            test_note = D_NoteFactory.create_note(NoteLevel.C, NoteAlteration.sharp, 4, 6);
            Assert.AreEqual(true, test_note.length_modifier);
        }
    }
}
