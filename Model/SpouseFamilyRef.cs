using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class SpouseFamilyRef : ExtensionContainer
    {
        public string? Ref { get; set; }

        /**
         * Convenience function to dereference family
         * @param gedcom Gedcom
         * @return referenced family
         */
        public Family? getFamily(Gedcom gedcom)
        {
            if (Ref == null)
                return null;

            return gedcom.GetFamily(Ref);
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
