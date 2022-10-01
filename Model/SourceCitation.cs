using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class SourceCitation : MediaContainer, IGedcomField
    {
        public enum DataTagContentsEnum { DATE, TEXT, COMBINED, SEPARATE };

        public string? Ref { get; set; }
        public string? Value { get; set; }
        public string? Page { get; set; }
        public string? Date { get; set; }
        public string? Text { get; set; }
        public string? Quay { get; set; }
        // yuck - some gedcom's don't use the data tag, some include write both text and date under the same tag, others use two data tags
        public DataTagContentsEnum? DataTagContents { get; set; } // set to null in default case (no data tag) so it isn't saved to json
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
