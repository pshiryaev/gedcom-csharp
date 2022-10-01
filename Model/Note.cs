using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Note : ExtensionContainer, IGedcomField
    {
        public string? Id { get; set; }
        public string? Value { get; set; }
        public string? Rin { get; set; }
        public Change? Chan { get; set; }
        public List<SourceCitation> SourceCitations { get; set; } = new();
        public bool SourceCitationsUnderValue { get; set; } = false; // yuck: Reunion does this: 0 NOTE 1 CONT ... 2 SOUR; remember for round-trip


        public void VisitContainedObjects(FileStream fs, IVisitor visitor, bool includeSourceCitations)
        {
            if (Chan != null)
            {
                Chan.Accept(fs, visitor);
            }
            if (includeSourceCitations)
            {
                foreach (SourceCitation sourceCitation in SourceCitations)
                {
                    sourceCitation.Accept(fs, visitor);
                }
            }
            base.VisitContainedObjects(fs, visitor);
        }

        public override void VisitContainedObjects(FileStream fs, IVisitor visitor)
        {
            VisitContainedObjects(fs, visitor, true);
        }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                this.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
