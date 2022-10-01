using gedcom5_csharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace gedcom5_csharp.Model
{
    internal class Gedcom : ExtensionContainer
    {
        public Header? Head { get; set; }
        public List<Submitter> Subms { get; set; } = new();
        public Submission? Subn { get; set; }
        public List<Person> People { get; set; } = new();
        public List<Family> Families { get; set; } = new();
        public List<Media> Media { get; set; } = new();
        public List<Note> Notes { get; set; } = new();
        public List<Source> Sources { get; set; } = new();
        public List<Repository> Repositories { get; set; } = new();

        [XmlIgnore]
        private Dictionary<string, Person>? PersonIndex;
        [XmlIgnore]
        private Dictionary<string, Family>? FamilyIndex;
        [XmlIgnore]
        private Dictionary<string, Media>? MediaIndex;
        [XmlIgnore]
        private Dictionary<string, Note>? NoteIndex;
        [XmlIgnore]
        private Dictionary<string, Source>? SourceIndex;
        [XmlIgnore]
        private Dictionary<string, Repository>? RepositoryIndex;
        [XmlIgnore]
        private Dictionary<string, Submitter>? SubmitterIndex;

        /**
         * Use this function in place of Header.getSubmission
         *
         * @return Submission top-level record or from header
         */
        public Submission? GetSubmission()
        {
            if (Subn != null)
            {
                return Subn;
            }
            else if (Head != null)
            {
                return Head.Subn;
            }
            return null;
        }

        public Family? GetFamily(string id)
        {
            return FamilyIndex != null ? FamilyIndex[id] : null;
        }
        public Person? GetPerson(string id)
        {
            return PersonIndex != null ? PersonIndex[id] : null;
        }

        public Media? getMedia(string id)
        {
            return MediaIndex != null ? MediaIndex[id] : null;
        }
        public Note? GetNote(string id)
        {
            return NoteIndex != null ? NoteIndex[id] : null;
        }

        public void UpdateReferences()
        {
            foreach (Person person in People)
            {
                foreach (SpouseFamilyRef spouseRef in person.Fams)
                {
                    if (spouseRef.Ref == null)
                        continue;

                    var family = GetFamily(spouseRef.Ref);
                    if (family == null)
                        continue;

                    bool spouseRefInHusbands = family.HusbandRefList.Any(hs => hs.Ref!= null && hs.Ref.Equals(person.Id));
                    bool spouseRefInWives = family.WifeRefList.Any(w => w.Ref != null && w.Ref.Equals(person.Id));
                    if (!spouseRefInHusbands && !spouseRefInWives)
                    {
                        var fact = person.EventsFacts.FirstOrDefault(evnt => evnt.Tag != null && evnt.Tag.Equals("SEX"));

                        var sRef = new SpouseRef();
                        sRef.Ref = person.Id;

                        if (fact != null && fact.Value != null && fact.Value.Equals("F"))
                            family.WifeRefList.Add(sRef);
                        else
                            family.HusbandRefList.Add(sRef);
                    }
                }

                foreach (ParentFamilyRef parentRef in person.Famc)
                {
                    if (parentRef.Ref == null)
                        continue;

                    Family? family = GetFamily(parentRef.Ref);
                    if (family == null)
                        continue;

                    bool containsReference = family.ChildRefList.Any(ch => ch.Ref != null && ch.Ref.Equals(person.Id));
                    if (!containsReference)
                    {
                        ChildRef cRef = new();
                        cRef.Ref = person.Id;
                        family.ChildRefList.Add(cRef);
                    }
                }
            }

            foreach (Family family in Families)
            {
                if (family.Id == null)
                    continue;

                foreach (SpouseRef sRef in family.HusbandRefList)
                {
                    if (sRef.Ref == null)
                        continue;

                    var person = GetPerson(sRef.Ref);
                    if (person == null)
                        continue;

                    bool containsRef = person.Fams.Any(spouseFamilyRef => spouseFamilyRef.Ref != null && spouseFamilyRef.Ref.Equals(family.Id));
                    if (!containsRef)
                    {
                        var spouseFamilyRef = new SpouseFamilyRef();
                        spouseFamilyRef.Ref = family.Id;
                        person.Fams.Add(spouseFamilyRef);
                    }
                }
                foreach (SpouseRef sRef in family.WifeRefList)
                {
                    if (sRef.Ref == null)
                        continue;

                    var person = GetPerson(sRef.Ref);
                    if (person == null)
                        continue;

                    bool containsRef = person.Fams.Any(spouseFamilyRef => spouseFamilyRef.Ref != null && spouseFamilyRef.Ref.Equals(family.Id));
                    if (!containsRef)
                    {
                        var spouseFamilyRef = new SpouseFamilyRef();
                        spouseFamilyRef.Ref = family.Id;
                        person.Fams.Add(spouseFamilyRef);
                    }
                }

                foreach (ChildRef cRef in family.ChildRefList)
                {
                    if (cRef.Ref == null)
                        continue;

                    var person = GetPerson(cRef.Ref);
                    if (person == null)
                        continue;

                    bool containsRef = person.Famc.Any(parentFamilyRef => parentFamilyRef.Ref != null && parentFamilyRef.Ref.Equals(family.Id));
                    if (!containsRef)
                    {
                        var spouseFamilyRef = new ParentFamilyRef();
                        spouseFamilyRef.Ref = family.Id;
                        person.Famc.Add(spouseFamilyRef);
                    }
                }
            }
        }

        public void CreateIndexes()
        {
            PersonIndex = new Dictionary<string, Person>();
            foreach (Person person in People)
            {
                if (person.Id == null)
                    continue;

                PersonIndex.Add(person.Id, person);
            }
            FamilyIndex = new Dictionary<string, Family>();
            foreach (Family family in Families)
            {
                if (family.Id == null)
                    continue;

                FamilyIndex.Add(family.Id, family);
            }
            MediaIndex = new Dictionary<string, Media>();
            foreach (Media m in Media)
            {
                if (m.Id == null)
                    continue;

                MediaIndex.Add(m.Id, m);
            }
            NoteIndex = new Dictionary<string, Note>();
            foreach (Note note in Notes)
            {
                if (note.Id == null)
                    continue;
                NoteIndex.Add(note.Id, note);
            }
            SourceIndex = new Dictionary<string, Source>();
            foreach (Source source in Sources)
            {
                if (source.Id == null)
                    continue;

                SourceIndex.Add(source.Id, source);
            }
            RepositoryIndex = new Dictionary<string, Repository>();
            foreach (Repository repository in Repositories)
            {
                if (repository.Id == null)
                    continue;
                RepositoryIndex.Add(repository.Id, repository);
            }

            SubmitterIndex = new Dictionary<string, Submitter>();
            foreach (Submitter submitter in Subms)
            {
                if (submitter.Id == null)
                    continue;
                SubmitterIndex.Add(submitter.Id, submitter);
            }
        }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                if (Head != null)
                {
                    Head.Accept(fs, visitor);
                }
                foreach (Submitter submitter in Subms)
                {
                    submitter.Accept(fs, visitor);
                }
                if (Subn != null)
                {
                    Subn.Accept(fs, visitor);
                }
                foreach (Person person in People)
                {
                    person.Accept(fs, visitor);
                }
                foreach (Family family in Families)
                {
                    family.Accept(fs, visitor);
                }
                foreach (Media media in Media)
                {
                    media.Accept(fs, visitor);
                }
                foreach (Note note in Notes)
                {
                    note.Accept(fs, visitor);
                }
                foreach (Source source in Sources)
                {
                    source.Accept(fs, visitor);
                }
                foreach (Repository repository in Repositories)
                {
                    repository.Accept(fs, visitor);
                }
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}
