using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Media : NoteContainer
    {
        public string? Id  { get; set; }
        public string? Form  { get; set; }
        public string? Titl  { get; set; }
        public string? Blob  { get; set; }
        public Change? Chan  { get; set; }
        public string? File  { get; set; }
        public string? FileTag  { get; set; }
        public string? Prim  { get; set; }
        public string? Type  { get; set; }
        public string? Scbk  { get; set; } // Scrapbook
        public string? Sshow  { get; set; }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
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
