using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class SpouseRef : ExtensionContainer
    {
        public string? Ref { get; set; }
        public string? Pref { get; set; }


        /**
         * Convenience function to dereference person
         * @param gedcom Gedcom
         * @return referenced person
         */
        public Person? GetPerson(Gedcom gedcom)
        {
            if (Ref == null)
                return null;
            return gedcom.GetPerson(Ref);
        }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            throw new Exception("Not implemented - pass isHusband");
        }

        /**
         * Handle the visitor  
         * @param visitor Visitor
         * @param isHusband false for wife; ChildRef overrides this method
         */
        public void Accept(FileStream fs, IVisitor visitor, bool isHusband)
        {
            if (visitor.Visit(fs, this, isHusband))
            {
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
