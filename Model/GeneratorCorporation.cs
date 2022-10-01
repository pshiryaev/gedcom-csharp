using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class GeneratorCorporation : ExtensionContainer, IGedcomField
    {
        public string? Value { get; set; }
        public Address? Addr { get; set; }
        public string? Phon { get; set; }
        public string? Email { get; set; }
        public string? EmailTag { get; set; }
        public string? Fax { get; set; }
        public string? Www { get; set; }
        public string? WwwTag { get; set; }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                if (Addr != null)
                {
                    Addr.Accept(fs, visitor);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }

    }
}
