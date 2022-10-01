using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Person : PersonFamilyCommonContainer
    {
        public string? Id { get; set; }
        public List<Name> Names { get; set; } = new();
        public List<ParentFamilyRef> Famc { get; set; } = new();
        public List<SpouseFamilyRef> Fams { get; set; } = new();
        //Associations
        public List<Association> Assos { get; set; } = new();
        //AncestorInterestSubmitterRef
        public string? Anci { get; set; } 
        //DescendantInterestSubmitterRef
        public string? Desi { get; set; }
        //RecordFileNumber
        public string? Rfn { get; set; }
        public Address? Addr { get; set; }
        public string? Phon { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? EmailTag { get; set; }
        public string? Www { get; set; }
        public string? WwwTag { get; set; }
        private List<Family> GetFamilies(Gedcom gedcom, List<ParentFamilyRef> familyRefs)
        {
            List<Family> families = new();
            foreach (var familyRef in familyRefs)
            {
                var family = familyRef.getFamily(gedcom);
                if (family != null)
                {
                    families.Add(family);
                }
            }
            return families;
        }
        /**
         * Convenience function to dereference parent family refs
         * @param gedcom Gedcom
         * @return list of parent families
         */
        public List<Family> getParentFamilies(Gedcom gedcom)
        {
            return GetFamilies(gedcom, Famc);
        }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                foreach (Name name in Names)
                {
                    name.Accept(fs, visitor);
                }
                foreach (ParentFamilyRef parentFamilyRef in Famc)
                {
                    parentFamilyRef.Accept(fs, visitor);
                }
                foreach (SpouseFamilyRef spouseFamilyRef in Fams)
                {
                    spouseFamilyRef.Accept(fs, visitor);
                }
                foreach (Association association in Assos)
                {
                    association.Accept(fs, visitor);
                }
                if (Addr != null)
                {
                    Addr.Accept(fs, visitor);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
