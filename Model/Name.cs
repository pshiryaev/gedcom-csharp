using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Name : SourceCitationContainer, IGedcomField
    {
        public string? Value { get; set; }
        public string? Givn { get; set; }
        public string? Surn { get; set; }
        public string? Npfx { get; set; } // Prefix
        public string? Nsfx { get; set; } // Suffix
        public string? Spfx { get; set; } // SurnamePrefix
        public string? Nick { get; set; } // Nickname
        public string? Fone { get; set; }
        public string? Romn { get; set; }
        public string? Type { get; set; }
        public string? TypeTag { get; set; }
        public string? Aka { get; set; }
        public string? AkaTag { get; set; }
        public string? FoneTag { get; set; }
        public string? RomnTag { get; set; }
        public string? Marrnm { get; set; }
        public string? MarrnmTag { get; set; }

        private void AppendValue(StringBuilder buf, string? value)
        {
            if (value != null)
            {
                if (buf.Length > 0)
                {
                    buf.Append(' ');
                }
                buf.Append(value);
            }
        }

        public string GetDisplayValue()
        {
            if (Value != null)
            {
                return Value;
            }
            else
            {
                var buf = new StringBuilder();
                AppendValue(buf, Npfx);
                AppendValue(buf, Givn);
                AppendValue(buf, Spfx);
                AppendValue(buf, Surn);
                AppendValue(buf, Nsfx);
                return buf.ToString();
            }
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
