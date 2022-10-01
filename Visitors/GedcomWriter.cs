using gedcom5_csharp.Model;
using gedcom5_csharp.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static gedcom5_csharp.Parser.ModelParser;

namespace gedcom5_csharp.Visitors
{
    internal class GedcomWriter : IVisitor
    {
        public void Write(Gedcom gedcom, string path)
        {
            var charset = GetCharsetName(gedcom);
            eol = (charset.Equals("x-MacRoman") ? "\r" : "\r\n");

            using (var fs = File.Create(path))
            {
                gedcom.Accept(fs, this);

                AddLine(fs, "0 TRLR" + eol);
                fs.Flush();
            }


            //stack = new Stack<object>();
            //nestedException = null;

            //StreamWriter writer;
            //if ("ANSEL".Equals(charset)) {
            //     writer = new AnselOutputStreamWriter(oStream);
            //}
            //else {
            //     writer = new OutputStreamWriter(oStream, charset);
            //    //}
            //this.writer = new BufferedWriter(writer);
            //gedcom.Accept(this);
            //this.writer.Write(fs, "0 TRLR" + eol);
            //this.writer.flush();
            //if (nestedException != null)
            //{
            //    throw nestedException;
            //}
        }


        //private Writer writer = null;
        private string eol = "";
        private Stack<object> stack = new();


        private static void AddLine(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        private string GetCharsetName(Gedcom gedcom)
        {
            Header? header = gedcom.Head;
            var generator = (header != null && header.Sour != null ? header.Sour.Value : null);
            var charset = (header != null && header.Charset != null ? header.Charset.Value : null);
            var version = (header != null && header.Charset != null ? header.Charset.Vers : null);
            charset = CharacterSet.GetCorrectedCharsetName(generator, charset, version);
            if (charset.Length == 0)
            {
                charset = "UTF-8"; // default
            }
            return charset;
        }



        private void Write(FileStream fs, string? tag, string? id, string? xRef, string? value, bool forceValueOnSeparateLine)
        {
            try
            {
                int level = stack.Count;
                AddLine(fs, level + " ");
                if (id != null && id.Length > 0)
                {
                    AddLine(fs, "@" + id + "@ ");
                }
                if (tag != null)
                    AddLine(fs, tag);
                if (xRef != null && xRef.Length > 0)
                {
                    AddLine(fs, " @" + xRef + "@");
                }
                if (value != null && value.Length > 0)
                {
                    if (forceValueOnSeparateLine && !value.StartsWith("\n"))
                    {
                        AddLine(fs, eol + (level + 1) + " CONC ");
                    }
                    else
                    {
                        AddLine(fs, " ");
                    }
                    //StringBuilder buf = new(value);
                    int cnt = 0;
                    bool lastLine = false;
                    while (!lastLine)
                    {
                        int nlPos = value.IndexOf("\n");
                        string line;
                        if (nlPos >= 0)
                        {
                            line = value[..nlPos]; // .Substring(0, nlPos);
                            value = value.Remove(0, nlPos + 1);
                        }
                        else
                        {
                            line = value;
                            lastLine = true;
                        }
                        if (cnt > 0)
                        {
                            AddLine(fs, eol + (level + 1).ToString() + " CONT ");
                        }
                        while (line.Length > 200)
                        {
                            AddLine(fs, line[..200]);
                            line = line[200..];
                            AddLine(fs, eol + (level + 1) + " CONC ");
                        }
                        AddLine(fs, line);
                        cnt++;
                    }
                }
                AddLine(fs, eol);
            }
            catch (IOException)
            {
                //nestedException = e;
            }
        }
        private void Write(FileStream fs, string? tag, string? id, string? xRef, string? value)
        {
            Write(fs, tag, id, xRef, value, false);
        }

        private void Write(FileStream fs, string? tag)
        {
            Write(fs, tag, null, null, null);
        }

        private void Write(FileStream fs, string? tag, string? value)
        {
            Write(fs, tag, null, null, value);
        }

        private void WriteFieldExtensions(FileStream fs, string? fieldName, ExtensionContainer ec)
        {
            if (!ec.ExtensionList.ContainsKey(ModelParser.MORE_TAGS_EXTENSION_KEY))
                return;

            List<GedcomLine> moreTags = (List<GedcomLine>)ec.ExtensionList[ModelParser.MORE_TAGS_EXTENSION_KEY];
            if (moreTags != null)
            {
                //foreach (GedcomLine tag in moreTags)
                //{
                //    // TODO
                //    //if (fieldName.Equals(tag.getParentTagName()))
                //    //{
                //    //    stack.Push(new object()); // placeholder
                //    //    WriteGedcomTag(fs, tag);
                //    //    stack.Pop();
                //    //}
                //}
            }
        }

        private void WriteGedcomTag(FileStream fs, GedcomLine tag)
        {
            Write(fs, tag.Tag, tag.Tag, tag.Xref, tag.LineVal);
            stack.Push(tag);
            //foreach (GedcomLine child in tag.getChildren())
            //{
            //    WriteGedcomTag(fs, child);
            //}
            stack.Pop();
        }

        private void WriteString(FileStream fs, string? tag, ExtensionContainer ec, string? value)
        {
            if (value != null && value.Length > 0)
            {
                Write(fs, tag, value);
                WriteFieldExtensions(fs, tag, ec);
            }
        }
        private void WriteRef(FileStream fs, string? tag, ExtensionContainer ec, string? xRef)
        {
            if (xRef != null && xRef.Length > 0)
            {
                Write(fs, tag, null, xRef, null);
                WriteFieldExtensions(fs, tag, ec);
            }
        }

        private void WriteSpouseRefStrings(FileStream fs, SpouseRef spouseRef)
        {
            WriteString(fs, "_PREF", spouseRef, spouseRef.Pref);
        }

        public bool Visit(FileStream fs, Address address)
        {
            Write(fs, "ADDR", address.Value);
            stack.Push(address);
            WriteString(fs, "ADR1", address, address.Adr1);
            WriteString(fs, "ADR2", address, address.Adr2);
            WriteString(fs, "ADR3", address, address.Adr3);
            WriteString(fs, "CITY", address, address.City);
            WriteString(fs, "STAE", address, address.Stae);
            WriteString(fs, "POST", address, address.Post);
            WriteString(fs, "CTRY", address, address.Ctry);
            WriteString(fs, "_NAME", address, address.Name);
            return true;
        }
        public bool Visit(FileStream fs, Association association)
        {
            Write(fs, "ASSO", null, association.Ref, null);
            stack.Push(association);
            WriteString(fs, "TYPE", association, association.Type);
            WriteString(fs, "RELA", association, association.Rela);
            return true;
        }

        public bool Visit(FileStream fs, Change change)
        {
            Write(fs, "CHAN");
            stack.Push(change);
            return true;
        }

        public bool Visit(FileStream fs, CharacterSet characterSet)
        {
            Write(fs, "CHAR", characterSet.Value);
            stack.Push(characterSet);
            WriteString(fs, "VERS", characterSet, characterSet.Vers);
            return true;
        }

        public bool Visit(FileStream fs, ChildRef childRef)
        {
            Write(fs, "CHIL", null, childRef.Ref, null);
            stack.Push(childRef);
            WriteSpouseRefStrings(fs, childRef);
            return true;
        }

        public bool Visit(FileStream fs, Model.DateTime dateTime)
        {
            Write(fs, "DATE", dateTime.Value);
            stack.Push(dateTime);
            WriteString(fs, "TIME", dateTime, dateTime.Time);
            return true;
        }

        private void WriteEventFactStrings(FileStream fs, EventFact eventFact)
        {
            WriteString(fs, "TYPE", eventFact, eventFact.Type);
            WriteString(fs, "DATE", eventFact, eventFact.Date);
            WriteString(fs, "PLAC", eventFact, eventFact.Place);
            WriteString(fs, "CAUS", eventFact, eventFact.Caus);
            WriteString(fs, "RIN", eventFact, eventFact.Rin);

            WriteString(fs, "PHON", eventFact, eventFact.Phon);
            WriteString(fs, "FAX", eventFact, eventFact.Fax);
            WriteString(fs, eventFact.EmailTag, eventFact, eventFact.Email);
            WriteString(fs, eventFact.WwwTag, eventFact, eventFact.Www);

            WriteString(fs, eventFact.UidTag, eventFact, eventFact.Uid);
        }


        public bool Visit(FileStream fs, EventFact eventFact)
        {
            Write(fs, eventFact.Tag, eventFact.Value);
            stack.Push(eventFact);
            WriteEventFactStrings(fs, eventFact);
            return true;
        }



        public bool Visit(FileStream fs, string extensionKey, object extension)
        {
            if (ModelParser.MORE_TAGS_EXTENSION_KEY.Equals(extensionKey))
            {
                //TODO
                //List<GedcomTag> moreTags = (List<GedcomTag>)extension;
                // for (GedcomTag tag : moreTags)
                // {
                //     if (tag.getParentTagName() == null)
                //     { // if field name is not null, the extension should have been written already
                //         writeGedcomTag(tag);
                //     }
                // }
            }
            return true;
        }

        private void WritePersonFamilyCommonContainerStrings(FileStream fs, PersonFamilyCommonContainer pf)
        {
            foreach (string refn in pf.Refns)
            {
                // it's a problem if there's multiple refns with sub-tags
                WriteString(fs, "REFN", pf, refn);
            }
            WriteString(fs, "RIN", pf, pf.Rin);
            WriteString(fs, pf.UidTag, pf, pf.Uid);
        }


        public bool Visit(FileStream fs, Family family)
        {
            Write(fs, "FAM", family.Id, null, null);
            stack.Push(family);
            WritePersonFamilyCommonContainerStrings(fs, family);
            return true;
        }


        public bool Visit(FileStream fs, Gedcom gedcom)
        {
            return true;
        }


        public bool Visit(FileStream fs, GedcomVersion gedcomVersion)
        {
            Write(fs, "GEDC");
            stack.Push(gedcomVersion);
            WriteString(fs, "VERS", gedcomVersion, gedcomVersion.Value);
            WriteString(fs, "FORM", gedcomVersion, gedcomVersion.Form);
            return true;
        }


        public bool Visit(FileStream fs, Generator generator)
        {
            Write(fs, "SOUR", generator.Value);
            stack.Push(generator);
            WriteString(fs, "NAME", generator, generator.Name);
            WriteString(fs, "VERS", generator, generator.Vers);
            return true;
        }


        public bool Visit(FileStream fs, GeneratorCorporation generatorCorporation)
        {
            Write(fs, "CORP", generatorCorporation.Value);
            stack.Push(generatorCorporation);
            WriteString(fs, "PHON", generatorCorporation, generatorCorporation.Phon);
            WriteString(fs, "FAX", generatorCorporation, generatorCorporation.Fax);
            WriteString(fs, generatorCorporation.EmailTag, generatorCorporation, generatorCorporation.Email);
            WriteString(fs, generatorCorporation.WwwTag, generatorCorporation, generatorCorporation.Www);
            return true;
        }


        public bool Visit(FileStream fs, GeneratorData generatorData)
        {
            Write(fs, "DATA", generatorData.Value);
            stack.Push(generatorData);
            WriteString(fs, "DATE", generatorData, generatorData.Date);
            WriteString(fs, "COPR", generatorData, generatorData.Copr);
            return true;
        }


        public bool Visit(FileStream fs, Header header)
        {
            Write(fs, "HEAD");
            stack.Push(header);
            WriteString(fs, "DEST", header, header.Dest);
            WriteString(fs, "FILE", header, header.File);
            WriteString(fs, "COPR", header, header.Copr);
            WriteString(fs, "LANG", header, header.Lang);
            WriteRef(fs, "SUBM", header, header.SubmRef);
            WriteRef(fs, "SUBN", header, header.SubnRef);
            return true;
        }


        public bool Visit(FileStream fs, LdsOrdinance ldsOrdinance)
        {
            Write(fs, ldsOrdinance.Tag, ldsOrdinance.Value);
            stack.Push(ldsOrdinance);
            WriteEventFactStrings(fs, ldsOrdinance);
            WriteString(fs, "STAT", ldsOrdinance, ldsOrdinance.Stat);
            WriteString(fs, "TEMP", ldsOrdinance, ldsOrdinance.Temp);
            return true;
        }


        public bool Visit(FileStream fs, Media media)
        {
            Write(fs, "OBJE", media.Id, null, null);
            stack.Push(media);
            WriteString(fs, "FORM", media, media.Form);
            WriteString(fs, "TITL", media, media.Titl);
            WriteString(fs, "BLOB", media, media.Blob);
            WriteString(fs, media.FileTag, media, media.File);
            WriteString(fs, "_PRIM", media, media.Prim);
            WriteString(fs, "_TYPE", media, media.Type);
            WriteString(fs, "_SCBK", media, media.Scbk);
            WriteString(fs, "_SSHOW", media, media.Sshow);
            return true;
        }


        public bool Visit(FileStream fs, MediaRef mediaRef)
        {
            Write(fs, "OBJE", null, mediaRef.Ref, null);
            stack.Push(mediaRef);
            return true;
        }


        public bool Visit(FileStream fs, Name name)
        {
            // handle ALIA name by recording it with that tag
            string tag;
            string? type = name.Type;
            if ("ALIA".Equals(type))
            {
                tag = type;
                type = null;
            }
            else
            {
                tag = "NAME";
            }
            Write(fs, tag, name.Value);
            stack.Push(name);
            WriteString(fs, "GIVN", name, name.Givn);
            WriteString(fs, "SURN", name, name.Surn);
            WriteString(fs, "NPFX", name, name.Npfx);
            WriteString(fs, "NSFX", name, name.Nsfx);
            WriteString(fs, "SPFX", name, name.Spfx);
            WriteString(fs, "NICK", name, name.Nick);
            WriteString(fs, name.TypeTag, name, type);
            WriteString(fs, name.AkaTag, name, name.Aka);
            WriteString(fs, name.MarrnmTag, name, name.Marrnm);
            return true;
        }


        public bool Visit(FileStream fs, Note note)
        {
            bool visitChildren = false;
            if (note.SourceCitationsUnderValue && note.SourceCitations.Count > 0 &&
                note.Value != null && note.Value.Length > 0)
            {
                // yuck: handle Reunion broken citations: 0 NOTE 1 CONT ... 2 SOUR; also Easytree: 0 NOTE 1 CONC ... 2 SOUR
                Write(fs, "NOTE", note.Id, null, note.Value, true);
                stack.Push(note);
                stack.Push(new object()); // increment level to 2
                foreach (SourceCitation sc in note.SourceCitations)
                {
                    sc.Accept(fs, this);
                }
                stack.Pop();
                visitChildren = true;
            }
            else
            {
                Write(fs, "NOTE", note.Id, null, note.Value);
                stack.Push(note);
            }

            // write note strings
            WriteString(fs, "RIN", note, note.Rin);

            if (visitChildren)
            {
                // if we return false below we need to visit the rest of the children and pop the stack here
                note.VisitContainedObjects(fs, this, false);
                stack.Pop();
            }
            return !visitChildren;
        }


        public bool Visit(FileStream fs, NoteRef noteRef)
        {
            Write(fs, "NOTE", null, noteRef.Ref, null);
            stack.Push(noteRef);
            return true;
        }


        public bool Visit(FileStream fs, ParentFamilyRef parentFamilyRef)
        {
            Write(fs, "FAMC", null, parentFamilyRef.Ref, null);
            stack.Push(parentFamilyRef);
            WriteSpouseFamilyRefStrings(fs, parentFamilyRef);
            WriteString(fs, "PEDI", parentFamilyRef, parentFamilyRef.Pedi);
            WriteString(fs, "_PRIMARY", parentFamilyRef, parentFamilyRef.Primary);
            return true;
        }


        public bool Visit(FileStream fs, ParentRelationship parentRelationship, bool isFather)
        {
            Write(fs, isFather ? "_FREL" : "_MREL", parentRelationship.Value);
            stack.Push(parentRelationship);
            return true;
        }


        public bool Visit(FileStream fs, Person person)
        {
            Write(fs, "INDI", person.Id, null, null);
            stack.Push(person);
            WriteRef(fs, "ANCI", person, person.Anci);
            WriteRef(fs, "DESI", person, person.Desi);
            WriteString(fs, "RFN", person, person.Rfn);
            WriteString(fs, "PHON", person, person.Phon);
            WriteString(fs, "FAX", person, person.Fax);
            WriteString(fs, person.EmailTag, person, person.Email);
            WriteString(fs, person.WwwTag, person, person.Www);
            WritePersonFamilyCommonContainerStrings(fs, person);
            return true;
        }


        public bool Visit(FileStream fs, Repository repository)
        {
            Write(fs, "REPO", repository.Id, null, repository.Value);
            stack.Push(repository);
            WriteString(fs, "NAME", repository, repository.Name);
            WriteString(fs, "PHON", repository, repository.Phon);
            WriteString(fs, "FAX", repository, repository.Fax);
            WriteString(fs, "RIN", repository, repository.Rin);
            WriteString(fs, repository.EmailTag, repository, repository.Email);
            WriteString(fs, repository.WwwTag, repository, repository.Www);
            return true;
        }


        public bool Visit(FileStream fs, RepositoryRef repositoryRef)
        {
            Write(fs, "REPO", null, repositoryRef.Ref, repositoryRef.Value);
            stack.Push(repositoryRef);
            if (repositoryRef.IsMediUnderCalnTag || (repositoryRef.Caln != null && repositoryRef.Caln.Length > 0))
            {
                Write(fs, "CALN", repositoryRef.Caln);
            }
            if (repositoryRef.IsMediUnderCalnTag)
            {
                stack.Push(new object()); // placeholder
            }
            WriteString(fs, "MEDI", repositoryRef, repositoryRef.Medi);
            if (repositoryRef.IsMediUnderCalnTag)
            {
                stack.Pop();
            }

            return true;
        }


        public bool Visit(FileStream fs, Source source)
        {
            Write(fs, "SOUR", source.Id, null, null);
            stack.Push(source);
            WriteString(fs, "AUTH", source, source.Auth);
            WriteString(fs, "TITL", source, source.Titl);
            WriteString(fs, "ABBR", source, source.Abbr);
            WriteString(fs, "PUBL", source, source.Publ);
            WriteString(fs, "TEXT", source, source.Text);
            WriteString(fs, "REFN", source, source.Refn);
            WriteString(fs, "RIN", source, source.Rin);
            WriteString(fs, "MEDI", source, source.Medi);
            WriteString(fs, "CALN", source, source.Caln);
            WriteString(fs, source.TypeTag, source, source.Type);
            WriteString(fs, source.UidTag, source, source.Uid);
            WriteString(fs, "_PAREN", source, source.Paren);
            WriteString(fs, "_ITALIC", source, source.Italic);
            WriteString(fs, "DATE", source, source.Date);
            return true;
        }

        private void WriteUnderData(FileStream fs, string tag, SourceCitation sourceCitation, string? value)
        {
            if (value != null && value.Length > 0)
            {
                Write(fs, "DATA");
                stack.Push(new object()); // placeholder
                WriteString(fs, tag, sourceCitation, value);
                stack.Pop();
            }
        }

        public bool Visit(FileStream fs, SourceCitation sourceCitation)
        {
            Write(fs, "SOUR", null, sourceCitation.Ref, sourceCitation.Value);
            stack.Push(sourceCitation);
            WriteString(fs, "PAGE", sourceCitation, sourceCitation.Page);
            WriteString(fs, "QUAY", sourceCitation, sourceCitation.Quay);
            if (sourceCitation.DataTagContents == SourceCitation.DataTagContentsEnum.COMBINED &&
                (sourceCitation.Date != null && sourceCitation.Date.Length > 0 ||
                 sourceCitation.Text != null && sourceCitation.Text.Length > 0))
            {
                Write(fs, "DATA");
                stack.Push(new object()); // placeholder
                WriteString(fs, "DATE", sourceCitation, sourceCitation.Date);
                WriteString(fs, "TEXT", sourceCitation, sourceCitation.Text);
                stack.Pop();
            }
            else if (sourceCitation.DataTagContents == SourceCitation.DataTagContentsEnum.DATE)
            {
                WriteUnderData(fs, "DATE", sourceCitation, sourceCitation.Date);
                WriteString(fs, "TEXT", sourceCitation, sourceCitation.Text);
            }
            else if (sourceCitation.DataTagContents == SourceCitation.DataTagContentsEnum.TEXT)
            {
                WriteUnderData(fs, "TEXT", sourceCitation, sourceCitation.Text);
                WriteString(fs, "DATE", sourceCitation, sourceCitation.Date);
            }
            else if (sourceCitation.DataTagContents == SourceCitation.DataTagContentsEnum.SEPARATE)
            {
                WriteUnderData(fs, "DATE", sourceCitation, sourceCitation.Date);
                WriteUnderData(fs, "TEXT", sourceCitation, sourceCitation.Text);
            }
            else if (sourceCitation.DataTagContents == null)
            {
                WriteString(fs, "DATE", sourceCitation, sourceCitation.Date);
                WriteString(fs, "TEXT", sourceCitation, sourceCitation.Text);
            }
            return true;
        }


        public bool Visit(FileStream fs, SpouseRef spouseRef, bool isHusband)
        {
            Write(fs, isHusband ? "HUSB" : "WIFE", null, spouseRef.Ref, null);
            stack.Push(spouseRef);
            WriteSpouseRefStrings(fs, spouseRef);
            return true;
        }

        private void WriteSpouseFamilyRefStrings(FileStream fs, SpouseFamilyRef spouseFamilyRef)
        {
            // nothing to write
        }


        public bool Visit(FileStream fs, SpouseFamilyRef spouseFamilyRef)
        {
            Write(fs, "FAMS", null, spouseFamilyRef.Ref, null);
            stack.Push(spouseFamilyRef);
            WriteSpouseFamilyRefStrings(fs, spouseFamilyRef);
            return true;
        }


        public bool Visit(FileStream fs, Submission submission)
        {
            Write(fs, "SUBN", submission.Id, null, null);
            stack.Push(submission);
            WriteString(fs, "DESC", submission, submission.Desc);
            WriteString(fs, "ORDI", submission, submission.Ordi);
            return true;
        }


        public bool Visit(FileStream fs, Submitter submitter)
        {
            Write(fs, "SUBM", submitter.Id, null, submitter.Value);
            stack.Push(submitter);
            WriteString(fs, "PHON", submitter, submitter.Phon);
            WriteString(fs, "FAX", submitter, submitter.Fax);
            WriteString(fs, "NAME", submitter, submitter.Name);
            WriteString(fs, "RIN", submitter, submitter.Rin);
            WriteString(fs, "LANG", submitter, submitter.Lang);
            WriteString(fs, submitter.WwwTag, submitter, submitter.Www);
            WriteString(fs, submitter.EmailTag, submitter, submitter.Email);
            return true;
        }


        public void EndVisit(ExtensionContainer obj)
        {
            if (obj is not Gedcom)
            {
                stack.Pop();
            }
        }

    }
}
