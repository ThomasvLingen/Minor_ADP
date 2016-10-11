using System;
using NUnit.Framework;
using DPA_Musicsheets;


namespace DPA_Test {
    [Category("MidiNoteParser")]
    public class test_MidiNoteParser {
        [Test]
        public void TestCreateNote()
        {
            D_Note test_note;

            // Central C (c4)
            test_note = MidiNoteParser.create_note(60, 10);
            Assert.AreEqual(NoteLevel.C, test_note.level);
            Assert.AreEqual(4, test_note.octave);
            Assert.AreEqual(NoteAlteration.none, test_note.alteration);

            // Central F# (f#4)
            test_note = MidiNoteParser.create_note(66, 10);
            Assert.AreEqual(NoteLevel.F, test_note.level);
            Assert.AreEqual(4, test_note.octave);
            Assert.AreEqual(NoteAlteration.sharp, test_note.alteration);

            // High Bb (Bb5)
            test_note = MidiNoteParser.create_note(82, 10);
            Assert.AreEqual(NoteLevel.B, test_note.level);
            Assert.AreEqual(5, test_note.octave);
            Assert.AreEqual(NoteAlteration.flat, test_note.alteration);

            // Low Eb (Eb3)
            test_note = MidiNoteParser.create_note(51, 10);
            Assert.AreEqual(NoteLevel.E, test_note.level);
            Assert.AreEqual(3, test_note.octave);
            Assert.AreEqual(NoteAlteration.flat, test_note.alteration);
        }
    }
}
