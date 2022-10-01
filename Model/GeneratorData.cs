using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class GeneratorData : ExtensionContainer, IGedcomField
    {
        public string? Value { get; set; }
        public string? Date { get; set; }
        public string? Copr { get; set; }

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
