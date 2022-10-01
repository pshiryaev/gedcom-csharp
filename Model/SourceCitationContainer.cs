using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    abstract class SourceCitationContainer : MediaContainer
    {
        public List<SourceCitation> SourceCitations { get; set; } = new();
        public override void VisitContainedObjects(FileStream fs, IVisitor visitor)
        {
            foreach (SourceCitation sourceCitation in SourceCitations)
            {
                sourceCitation.Accept(fs, visitor);
            }
            base.VisitContainedObjects(fs, visitor);
        }

    }
}
