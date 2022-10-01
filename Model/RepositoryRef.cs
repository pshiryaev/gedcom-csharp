using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class RepositoryRef : NoteContainer, IGedcomField
    {
        public string? Ref { get; set; }
        public string? Value { get; set; }
        public string? Caln { get; set; }
        public string? Medi { get; set; } // MediaType
        public bool IsMediUnderCalnTag { get; set; } // use string instead of boolean so it isn't saved to json when false
        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
