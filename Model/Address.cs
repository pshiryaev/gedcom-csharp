using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Address : ExtensionContainer, IGedcomField
    {
        public string? Value { get; set; }
        public string? Adr1 { get; set; }
        public string? Adr2 { get; set; }
        public string? Adr3 { get; set; }
        public string? City { get; set; }
        public string? Stae { get; set; }
        public string? Post { get; set; }
        public string? Ctry { get; set; }
        public string? Name { get; set; }

        private void appendValue(StringBuilder buf, string? value)
        {
            if (value != null)
            {
                if (buf.Length > 0)
                {
                    buf.Append('\n');
                }
                buf.Append(value);
            }
        }

        public string getDisplayValue()
        {
            StringBuilder buf = new();
            appendValue(buf, Value);
            appendValue(buf, Adr1);
            appendValue(buf, Adr2);
            appendValue(buf, Adr3);
            appendValue(buf, (City ?? "") + (City != null && Stae != null ? ", " : "") + (Stae ?? "") +
                             ((City != null || Stae != null) && Post != null ? " " : "") + (Post ?? ""));
            appendValue(buf, Ctry);
            return buf.ToString();
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
