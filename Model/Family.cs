using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Family : PersonFamilyCommonContainer
    {
        public string? Id { get; set; }
        public List<SpouseRef> HusbandRefList { get; set; } = new();
        public List<SpouseRef> WifeRefList { get; set; } = new();
        public List<ChildRef> ChildRefList { get; set; } = new();

        public List<Person> GetFamilyMembers(Gedcom gedcom, List<SpouseRef> memberRefList, bool preferredFirst)
        {
            List<Person> members = new();
            foreach  (SpouseRef memberRef in memberRefList)
            {
                var member = memberRef.GetPerson(gedcom);
                if (member != null)
                {
                    if (preferredFirst && "Y".Equals(memberRef.Pref))
                    {
                        members.Insert(0, member);
                    }
                    else
                    {
                        members.Add(member);
                    }
                }
            }
            return members;
        }

        public List<Person> GetFamilyMembers(Gedcom gedcom, List<ChildRef> memberRefList, bool preferredFirst)
        {
            List<Person> members = new();
            foreach (ChildRef memberRef in memberRefList)
            {
                var member = memberRef.GetPerson(gedcom);
                if (member != null)
                {
                    if (preferredFirst && "Y".Equals(memberRef.Pref))
                    {
                        members.Insert(0, member);
                    }
                    else
                    {
                        members.Add(member);
                    }
                }
            }
            return members;
        }

        /**
         * Convenience function to dereference husband refs
         * Return preferred in first position
         * @param gedcom Gedcom
         * @return list of husbands, generally just one unless there are several alternatives with one preferred
         */
        public List<Person> GetHusbands(Gedcom gedcom)
        {
            return GetFamilyMembers(gedcom, HusbandRefList, true);
        }
        /**
         * Convenience function to dereference wife refs
         * Return preferred in first position
         * @param gedcom Gedcom
         * @return list of wives, generally just one unless there are several alternatives with one preferred
         */
        public List<Person> GetWives(Gedcom gedcom)
        {
            return GetFamilyMembers(gedcom, WifeRefList, true);
        }

        /**
         * Convenience function to dereference child refs
         * @param gedcom Gedcom
         * @return list of children
         */
        public List<Person> GetChildren(Gedcom gedcom)
        {
            return GetFamilyMembers(gedcom, ChildRefList, false);
        }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                foreach (var husband in HusbandRefList)
                {
                    husband.Accept(fs, visitor, true);
                }
                foreach (var wife in WifeRefList)
                {
                    wife.Accept(fs, visitor, false);
                }
                foreach (var childRef in ChildRefList)
                {
                    childRef.Accept(fs, visitor);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }

    }
}
