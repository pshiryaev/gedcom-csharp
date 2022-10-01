using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Submitter : ExtensionContainer, IGedcomField
    {
        public string? Id { get; set; }
        public string? Value { get; set; }
        public Address? Addr { get; set; }
        public string? Phon { get; set; }
        public string? Fax { get; set; }
        public string? Name { get; set; }
        public Change? Chan { get; set; }
        public string? Rin { get; set; }
        public string? Lang { get; set; }
        public string? Www { get; set; }
        public string? WwwTag { get; set; }
        public string? Email { get; set; }
        public string? EmailTag { get; set; }

        public override void Accept(FileStream fs, IVisitor visitor)
        { 
            if (visitor.Visit(fs, this))
            {
                if (Addr != null)
                {
                    Addr.Accept(fs, visitor);
                }
                if (Chan != null)
                {
                    Chan.Accept(fs, visitor);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
