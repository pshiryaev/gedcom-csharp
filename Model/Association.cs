using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Association : ExtensionContainer
    {
        public string? Ref { get; set; }
        public string? Type { get; set; }
        public string? Rela { get; set; }


        public Person? GetPerson(Gedcom gedcom)
        {
            if (Ref == null)
                return null;

            return gedcom.GetPerson(Ref);
        }

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
