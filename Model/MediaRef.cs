using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class MediaRef : ExtensionContainer
    {
        public string? Ref { get; set; }
        public Media? getMedia(Gedcom gedcom)
        {
            if (Ref == null)
                return null;
            return gedcom.getMedia(Ref);
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
