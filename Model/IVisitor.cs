using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal interface IVisitor
    {
        public bool Visit(FileStream fs, Address address) { return true; }
        public bool Visit(FileStream fs, Association association) { return true; }
        public bool Visit(FileStream fs, Change change) { return true; }
        public bool Visit(FileStream fs, CharacterSet characterSet) { return true; }
        public bool Visit(FileStream fs, ChildRef childRef) { return true; }
        public bool Visit(FileStream fs, DateTime dateTime) { return true; }
        public bool Visit(FileStream fs, EventFact eventFact) { return true; }
        public bool Visit(FileStream fs, string extensionKey, object extension) { return true; }
        public bool Visit(FileStream fs, Family family) { return true; }
        public bool Visit(FileStream fs, Gedcom gedcom) { return true; }
        public bool Visit(FileStream fs, GedcomVersion gedcomVersion) { return true; }
        public bool Visit(FileStream fs, Generator generator) { return true; }
        public bool Visit(FileStream fs, GeneratorCorporation generatorCorporation) { return true; }
        public bool Visit(FileStream fs, GeneratorData generatorData) { return true; }
        public bool Visit(FileStream fs, Header header) { return true; }
        public bool Visit(FileStream fs, LdsOrdinance ldsOrdinance) { return true; }
        public bool Visit(FileStream fs, Media media) { return true; }
        public bool Visit(FileStream fs, MediaRef mediaRef) { return true; }
        public bool Visit(FileStream fs, Name name) { return true; }
        public bool Visit(FileStream fs, Note note) { return true; }
        public bool Visit(FileStream fs, NoteRef noteRef) { return true; }
        public bool Visit(FileStream fs, ParentFamilyRef parentFamilyRef) { return true; }
        public bool Visit(FileStream fs, ParentRelationship parentRelationship, bool isFather) { return true; }
        public bool Visit(FileStream fs, Person person) { return true; }
        public bool Visit(FileStream fs, Repository repository) { return true; }
        public bool Visit(FileStream fs, RepositoryRef repositoryRef) { return true; }
        public bool Visit(FileStream fs, Source source) { return true; }
        public bool Visit(FileStream fs, SourceCitation sourceCitation) { return true; }
        public bool Visit(FileStream fs, SpouseRef spouseRef, bool isHusband) { return true; }
        public bool Visit(FileStream fs, SpouseFamilyRef spouseFamilyRef) { return true; }
        public bool Visit(FileStream fs, Submission submission) { return true; }
        public bool Visit(FileStream fs, Submitter submitter) { return true; }
        public void EndVisit(ExtensionContainer obj) { }
    }
}
