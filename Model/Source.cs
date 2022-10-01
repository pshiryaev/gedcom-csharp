using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Source : MediaContainer
    {
        public string? Id { get; set; }
        public string? Auth { get; set; }
        public string? Titl { get; set; }
        public string? Abbr { get; set; }
        public string? Publ { get; set; }
        public string? Text { get; set; }
        public RepositoryRef? Repo { get; set; }
        public string? Refn { get; set; }
        public string? Rin { get; set; }
        public Change? Chan { get; set; }
        public string? Medi { get; set; }
        public string? Caln { get; set; }
        public string? Type { get; set; }
        public string? TypeTag { get; set; }
        public string? Uid { get; set; }
        public string? UidTag { get; set; }
        public string? Paren { get; set; }
        public string? Italic { get; set; }
        public string? Date { get; set; }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                if (Repo != null)
                {
                    Repo.Accept(fs, visitor);
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
