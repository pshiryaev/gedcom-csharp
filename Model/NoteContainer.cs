using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    abstract class NoteContainer : ExtensionContainer
    {
        public List<NoteRef> NoteRefs { get; set; } = new();
        public List<Note> Notes { get; set; } = new();

        /**
         * Use this function in place of getNotes and getNoteRefs
         * @param gedcom Gedcom
         * @return inline notes as well as referenced notes
         */
        public List<Note> GetAllNotes(Gedcom gedcom)
        {
            List<Note> notes = new();
            foreach (NoteRef noteRef in NoteRefs)
            {
                var note = noteRef.GetNote(gedcom);
                if (note != null)
                {
                    notes.Add(note);
                }
            }
            notes.AddRange(Notes);
            return notes;
        }

        public override void VisitContainedObjects(FileStream fs, IVisitor visitor)
        {
            foreach (NoteRef noteRef in NoteRefs)
            {
                noteRef.Accept(fs, visitor);
            }
            foreach (Note note in Notes)
            {
                note.Accept(fs, visitor);
            }
            base.VisitContainedObjects(fs, visitor);
        }
    }
}
