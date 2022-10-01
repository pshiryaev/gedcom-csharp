using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class ChildRef : SpouseRef
    {
        public ParentRelationship? Frel { get; set; }
        public ParentRelationship? Mrel { get; set; }
        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                if (Frel != null)
                {
                    Frel.Accept(fs, visitor, true);
                }
                if (Mrel != null)
                {
                    Mrel.Accept(fs, visitor, false);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
