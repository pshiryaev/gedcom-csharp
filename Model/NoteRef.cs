using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class NoteRef : ExtensionContainer
    {
        public string? Ref { get; set; }
        public List<SourceCitation> SourceCitations { get; set; } = new();

        /**
         * Convenience function to dereference note
         * @param gedcom Gedcom
         * @return referenced note
         */
        public Note? GetNote(Gedcom gedcom)
        {
            if (Ref == null)
                return null;
            return gedcom.GetNote(Ref);
        }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                foreach (SourceCitation sourceCitation in SourceCitations)
                {
                    sourceCitation.Accept(fs, visitor);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
