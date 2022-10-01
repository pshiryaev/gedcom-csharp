using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Header : NoteContainer
    {
        public Generator? Sour { get; set; }
        public string? Dest { get; set; }
        public DateTime? Date  { get; set; }
        public string? SubmRef  { get; set; }
        public string? SubnRef  { get; set; }
        public Submission? Subn  { get; set; }
        public string? File  { get; set; }
        public string? Copr  { get; set; }
        public GedcomVersion? Gedc  { get; set; }
        public CharacterSet? Charset  { get; set; }
        public string? Lang  { get; set; }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                if (Sour != null)
                {
                    Sour.Accept(fs, visitor);
                }
                if (Date != null)
                {
                    Date.Accept(fs, visitor);
                }
                if (Subn != null)
                {
                    Subn.Accept(fs, visitor);
                }
                if (Gedc != null)
                {
                    Gedc.Accept(fs, visitor);
                }
                if (Charset != null)
                {
                    Charset.Accept(fs, visitor);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
