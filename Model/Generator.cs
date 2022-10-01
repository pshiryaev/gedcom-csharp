using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Generator : ExtensionContainer, IGedcomField
    {
        public string? Value { get; set; }
        public string? Name { get; set; }
        public string? Vers { get; set; }
        public GeneratorCorporation? Corp { get; set; }
        public GeneratorData? Data { get; set; }


        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                if (Corp != null)
                {
                    Corp.Accept(fs, visitor);
                }
                if (Data != null)
                {
                    Data.Accept(fs, visitor);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
