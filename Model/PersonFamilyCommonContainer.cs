using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    abstract class PersonFamilyCommonContainer : SourceCitationContainer
    {
        public List<EventFact> EventsFacts { get; set; } = new();
        public List<LdsOrdinance> LdsOrdinances { get; set; } = new();
        public List<string> Refns { get; set; } = new();
        public string? Rin { get; set; }
        public Change? Chan { get; set; }
        public string? Uid { get; set; }
        public string? UidTag { get; set; }

        public override void VisitContainedObjects(FileStream fs, IVisitor visitor)
        {
            foreach (EventFact eventFact in EventsFacts)
            {
                eventFact.Accept(fs, visitor);
            }
            foreach (LdsOrdinance ldsOrdinance in LdsOrdinances)
            {
                ldsOrdinance.Accept(fs, visitor);
            }
            if (Chan != null)
            {
                Chan.Accept(fs, visitor);
            }
            base.VisitContainedObjects(fs, visitor);
        }
    }
}
