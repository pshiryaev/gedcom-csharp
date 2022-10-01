using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class ParentRelationship : SourceCitationContainer, IGedcomField
    {
        public string? Value { get; set; }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            throw new Exception("Not implemented - pass isFather");
        }

        public void Accept(FileStream fs, IVisitor visitor, bool isFather)
        {
            if (visitor.Visit(fs, this, isFather))
            {
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
