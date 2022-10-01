using gedcom5_csharp.Model;

namespace gedcom5_csharp.Parser
{
    public static class Extensions
    {
        public static int IndexOfNth(this string str, string value, uint nth = 0)
        {
            int offset = str.IndexOf(value);
            for (uint i = 1; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = str.IndexOf(value, offset + 1);
            }

            return offset;
        }
    }
    internal class ModelParser
    {
        public static string MORE_TAGS_EXTENSION_KEY = "more_tags";
        public enum Tag
        {
            ABBR, ADDR, ADR1, ADR2, ADR3, _AKA, ALIA, ANCI, ASSO, AUTH,
            BLOB,
            CALN, CAUS, CHAN, CHAR, CHIL, CITY, CONC, CONT, COPR, CORP, CTRY,
            DATA, DATE, DESC, DESI, DEST,
            EMAIL, _EMAIL, _EML,
            FAM, FAMC, FAMS, FAX, _FILE, FILE, FONE, FORM, _FREL,
            GED, GEDC, GIVN,
            HEAD, HUSB,
            INDI, _ITALIC,
            LANG,
            _MARRNM, _MAR, _MARNM, MEDI, _MREL,
            NAME, _NAME, NICK, NOTE, NPFX, NSFX,
            OBJE, ORDI,
            PAGE, _PAREN, PEDI, PHON, POST, PLAC, _PREF, _PRIM, _PRIMARY, PUBL,
            QUAY,
            REFN, RELA, REPO, RFN, RIN, ROMN,
            _SCBK, SOUR, SPFX, _SSHOW, STAE, STAT, SUBM, SUBN, SURN,
            TEMP, TEXT, TIME, TITL, TRLR, TYPE, _TYPE,
            UID, _UID, _URL,
            VERS,
            WIFE, _WEB, WWW, _WWW,

            // personal LDS ordinances
            BAPL, CONL, WAC, ENDL, SLGC,
            // family LDS ordinances
            SLGS
        }

        public Gedcom Parse(IEnumerable<string> lines)
        {
            Gedcom gedcom = new();
            Stack<object> objectStack = new();
            objectStack.Push(gedcom);
            var currentLevel = -1;

            foreach (string line in lines)
            {
                var gLine = ParseLine(line);

                AddGedComElement(gLine, objectStack, currentLevel);
                currentLevel = gLine.Level;
            }
            return gedcom;
        }

        internal GedcomLine ParseLine(string line)
        {
            var gLine = new GedcomLine();
            var lineArray = line.Split(' ');
            var elemCount = lineArray.Length;
            if (elemCount < 2)
                return gLine;
            if (!int.TryParse(lineArray[0], out int lvl))
                return gLine;

            gLine.Level = lvl;

            if (elemCount == 2)
            {
                gLine.Tag = lineArray[1];
                return gLine;
            }

            uint index = 1;
            if (lineArray[1].StartsWith("@"))
                gLine.Xref = lineArray[index++].Trim('@');

            gLine.Tag = lineArray[index++];
            if (elemCount > index)
                gLine.LineVal = line.Substring(line.IndexOfNth(" ", index) + 1).Trim('@');

            return gLine;
        }

        internal void AddGedComElement(GedcomLine gLine, Stack<object> objectStack, int currentLevel)
        {
            //var tmpLevel = currentLevel;
            while (gLine.Level < objectStack.Count - 1 /*currentLevel*/ )
            {
                objectStack.Pop();
                currentLevel--;
            }

            object? obj = null;
            var tos = objectStack.Peek();
            switch (gLine.Tag.ToUpperInvariant())
            {
                case "ABBR":
                    obj = HandleAbbr(tos, gLine);
                    break;
                case "ADDR":
                    obj = HandleAddr(tos, gLine);
                    break;
                case "ADR1":
                    obj = HandleAdr1(tos, gLine);
                    break;
                case "ADR2":
                    obj = HandleAdr2(tos, gLine);
                    break;
                case "ADR3":
                    obj = HandleAdr3(tos, gLine);
                    break;
                case "_AKA":
                    obj = HandleAka(tos, gLine);
                    break;
                case "ALIA":
                    obj = HandleAlia(tos, gLine);
                    break;
                case "ANCI":
                    obj = HandleAnci(tos, gLine);
                    break;
                case "ASSO":
                    obj = HandleAsso(tos, gLine);
                    break;
                case "AUTH":
                    obj = HandleAuth(tos, gLine);
                    break;
                case "BLOB":
                    obj = HandleBlob(tos, gLine);
                    break;
                case "CALN":
                    obj = HandleCaln(tos, gLine);
                    break;
                case "CAUS":
                    obj = HandleCaus(tos, gLine);
                    break;
                case "CHAN":
                    obj = HandleChan(tos, gLine);
                    break;
                case "CHAR":
                    obj = HandleChar(tos, gLine);
                    break;
                case "CHIL":
                    obj = HandleChil(tos, gLine);
                    break;
                case "CITY":
                    obj = HandleCity(tos, gLine);
                    break;
                case "CONC":
                    obj = HandleCont(tos, false, gLine);
                    break;
                case "CONT":
                    obj = HandleCont(tos, true, gLine);
                    break;
                case "COPR":
                    obj = HandleCopr(tos, gLine);
                    break;
                case "CORP":
                    obj = HandleCorp(tos, gLine);
                    break;
                case "CTRY":
                    obj = HandleCtry(tos, gLine);
                    break;
                case "DATA":
                    obj = HandleData(tos, gLine);
                    break;
                case "DATE":
                    obj = HandleDate(tos, gLine, objectStack);
                    break;
                case "DESC":
                    obj = HandleDesc(tos, gLine);
                    break;
                case "DESI":
                    obj = HandleDesi(tos, gLine);
                    break;
                case "DEST":
                    obj = HandleDest(tos, gLine);
                    break;
                case "EMAIL":
                case "_EMAIL":
                case "_EML":
                    obj = HandleEmail(tos, gLine);
                    break;
                case "FAM":
                    obj = HandleFam(tos, gLine);
                    break;
                case "FAMC":
                    obj = HandleFamc(tos, gLine);
                    break;
                case "FAMS":
                    obj = HandleFams(tos, gLine);
                    break;
                case "FAX":
                    obj = HandleFax(tos, gLine);
                    break;
                case "_FILE":
                case "FILE":
                    obj = HandleFile(tos, gLine);
                    break;
                case "FONE":
                    obj = HandleFone(tos, gLine);
                    break;
                case "FORM":
                    obj = HandleForm(tos, gLine);
                    break;
                case "_FREL":
                    obj = HandleFRel(tos, gLine);
                    break;
                case "GED":
                    //obj = HandleGed();
                    break;
                case "GEDC":
                    obj = HandleGedc(tos);
                    break;
                case "GIVN":
                    obj = HandleGivn(tos, gLine);
                    break;
                case "HEAD":
                    obj = HandleHead(tos);
                    break;
                case "HUSB":
                    obj = HandleHusb(tos, gLine);
                    break;
                case "INDI":
                    obj = HandleIndi(tos, gLine);
                    break;
                case "_ITALIC":
                    obj = HandleItalic(tos, gLine);
                    break;
                case "LANG":
                    obj = HandleLang(tos, gLine);
                    break;
                case "_MARRNM":
                case "_MARNM":
                case "_MAR":
                    obj = HandleMarrnm(tos, gLine);
                    break;
                case "MEDI":
                    obj = HandleMedi(tos, gLine);
                    break;
                case "_MREL":
                    obj = HandleMRel(tos, gLine);
                    break;
                case "NAME":
                case "_NAME":
                    obj = HandleName(tos, gLine);
                    break;
                case "NICK":
                    obj = HandleNick(tos, gLine);
                    break;
                case "NPFX":
                    obj = HandleNpfx(tos, gLine);
                    break;
                case "NSFX":
                    obj = HandleNsfx(tos, gLine);
                    break;
                case "NOTE":
                    obj = HandleNote(tos, gLine);
                    break;
                case "OBJE":
                    obj = HandleObje(tos, gLine);
                    break;
                case "ORDI":
                    obj = HandleOrdi(tos, gLine);
                    break;
                case "PAGE":
                    obj = HandlePage(tos, gLine);
                    break;
                case "_PAREN":
                    obj = HandleParen(tos, gLine);
                    break;
                case "PEDI":
                    obj = HandlePedi(tos, gLine);
                    break;
                case "PHON":
                    obj = HandlePhon(tos, gLine);
                    break;
                case "PLAC":
                    obj = HandlePlac(tos, gLine);
                    break;
                case "POST":
                    obj = HandlePost(tos, gLine);
                    break;
                case "_PREF":
                    obj = HandlePref(tos, gLine);
                    break;
                case "_PRIM":
                case "_PRIMARY":
                    obj = HandlePrim(tos, gLine);
                    break;
                case "PUBL":
                    obj = HandlePubl(tos, gLine);
                    break;
                case "QUAY":
                    obj = HandleQuay(tos, gLine);
                    break;
                case "REFN":
                    obj = HandleRefn(tos, gLine);
                    break;
                case "RELA":
                    obj = HandleRela(tos, gLine);
                    break;
                case "REPO":
                    obj = HandleRepo(tos, gLine);
                    break;
                case "RFN":
                    obj = HandleRfn(tos, gLine);
                    break;
                case "RIN":
                    obj = HandleRin(tos, gLine);
                    break;
                case "ROMN":
                    obj = HandleRomn(tos, gLine);
                    break;
                case "_SCBK":
                    obj = HandleScbk(tos, gLine);
                    break;
                case "SOUR":
                    obj = HandleSour(tos, gLine);
                    break;
                case "SPFX":
                    obj = HandleSpfx(tos, gLine);
                    break;
                case "_SSHOW":
                    obj = HandleSshow(tos, gLine);
                    break;
                case "STAE":
                    obj = HandleStae(tos, gLine);
                    break;
                case "STAT":
                    obj = HandleStat(tos, gLine);
                    break;
                case "SUBM":
                    obj = HandleSubm(tos, gLine);
                    break;
                case "SUBN":
                    obj = HandleSubn(tos, gLine);
                    break;
                case "SURN":
                    obj = HandleSurn(tos, gLine);
                    break;
                case "TEMP":
                    obj = HandleTemp(tos, gLine);
                    break;
                case "TEXT":
                    obj = HandleText(tos, gLine, objectStack);
                    break;
                case "TIME":
                    obj = HandleTime(tos, gLine);
                    break;
                case "TITL":
                    obj = HandleTitl(tos, gLine);
                    break;
                case "TRLR":
                    obj = HandleTrlr(tos, gLine);
                    break;
                case "TYPE":
                case "_TYPE":
                    obj = HandleType(tos, gLine);
                    break;
                case "VERS":
                    obj = HandleVers(tos, gLine);
                    break;
                case "_UID":
                case "UID":
                    obj = HandleUid(tos, gLine);
                    break;
                case "WIFE":
                    obj = HandleWife(tos, gLine);
                    break;
                case "WWW":
                case "_WWW":
                case "_WEB":
                case "_URL":
                    obj = HandleWww(tos, gLine);
                    break;
                // lds ordinance tags
                case "BAPL":
                case "CONL":
                case "WAC":
                case "ENDL":
                case "SLGC":
                    obj = HandleLdsOrdinance(tos, true, gLine);
                    break;
                case "SLGS":
                    obj = HandleLdsOrdinance(tos, false, gLine);
                    break;
                default:
                    obj = HandleEventFact(tos, gLine);
                    break;
            }

            if (/*gLine.Level >= currentLevel &&*/ obj != null)
                objectStack.Push(obj);

        }

        private object? HandleAbbr(object tos, GedcomLine gLine)
        {
            if (tos is Source source && source.Abbr == null)
                source.Abbr = gLine.LineVal;
            return null;
        }

        private object? HandleAddr(object tos, GedcomLine gLine)
        {
            Address address = new();
            address.Value = gLine.LineVal;

            if (tos is GeneratorCorporation corporation && corporation.Addr == null)
            {
                corporation.Addr = address;
                return address;
            }
            else if (tos is EventFact fact && fact.Addr == null)
            {
                fact.Addr = address;
                return address;
            }
            else if (tos is Person person && person.Addr == null)
            {
                person.Addr = address;
                return address;
            }
            else if (tos is Repository repository && repository.Addr == null)
            {
                repository.Addr = address;
                return address;
            }
            else if (tos is Submitter submitter && submitter.Addr == null)
            {
                submitter.Addr = address;
                return address;
            }
            
            return null;
        }

        private object? HandleAdr1(object tos, GedcomLine gLine)
        {
            if (tos is Address address && address.Adr1 == null)
                address.Adr1 = gLine.LineVal;
            return null;
        }

        private object? HandleAdr2(object tos, GedcomLine gLine)
        {
            if (tos is Address address && address.Adr2 == null)
                address.Adr2 = gLine.LineVal;
            return null;
        }

        private object? HandleAdr3(object tos, GedcomLine gLine)
        {
            if (tos is Address address && address.Adr3 == null) 
                address.Adr3 = gLine.LineVal;
            
            return null;
        }

        private object? HandleAka(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Aka == null) 
                name.Aka = gLine.LineVal;
            
            return null;
        }

        private object? HandleAlia(object tos, GedcomLine gLine)
        {
            // we don't Handle the standard ALIA @ref@ form, because nobody writes it that way
            // make it a name so it can have source citations
            if (tos is Person person && gLine.Xref == null) {
                Name name = new();
                name.Type ="ALIA";
                name.Value = gLine.LineVal;
                person.Names.Add(name);
                return name;
            }
            return null;
        }

        private object? HandleAnci(object tos, GedcomLine gLine)
        {
            if (tos is Person person && person.Anci == null && gLine.Xref != null) {
                person.Anci = gLine.Xref;
            }
            return null;
        }

        private object? HandleAsso(object tos, GedcomLine gLine)
        {
            if (tos is Person person) {
                Association association = new();
                if (gLine.Xref != null)
                {
                    association.Ref = gLine.LineVal;
                }
               person.Assos.Add(association);
                return association;
            }
            return null;
        }

        private object? HandleAuth(object tos, GedcomLine gLine)
        {
            if (tos is Source source && source.Auth == null) {
                source.Auth = gLine.LineVal;
            }
            return null;
        }

        private object? HandleBlob(object tos, GedcomLine gLine)
        {
            if (tos is Media media && media.Blob == null) {
                media.Blob = gLine.LineVal;
            }
            return null;
        }

        private object? HandleCaln(object tos, GedcomLine gLine)
        {
            if (tos is RepositoryRef @ref && @ref.Caln == null)
                @ref.Caln = gLine.LineVal;
            else if(tos is Source source && source.Caln == null) 
                source.Caln = gLine.LineVal;
            return null;
        }

        private object? HandleCaus(object tos, GedcomLine gLine)
        {
            if (tos is EventFact fact && fact.Caus == null)
                fact.Caus = gLine.LineVal;
            return null;
        }

        private object? HandleChan(object tos, GedcomLine gLine)
        {
            Change change = new();
            if (tos is PersonFamilyCommonContainer container && container.Chan == null) 
            {
                container.Chan = change;
                return change;
            }
            else if (tos is Media media && media.Chan == null)
            {
                media.Chan = change;
                return change;
            }
            else if (tos is Note note && note.Chan == null)
            {
                note.Chan = change;
                return change;
            }
            else if (tos is Source source && source.Chan == null)
            {
                source.Chan = change;
                return change;
            }
            else if (tos is Repository repository && repository.Chan == null)
            {
                repository.Chan = change;
                return change;
            }
            else if (tos is Submitter submitter && submitter.Chan == null)
            {
                submitter.Chan = change;
                return change;
            }
            
            return null;
        }

        private object? HandleChar(object tos, GedcomLine gLine)
        {
            if (tos is Header header && header.Charset == null) {
                CharacterSet charset = new();
                charset.Value = gLine.LineVal;
                header.Charset = charset;
                return charset;
            }
            return null;
        }

        private object? HandleChil(object tos, GedcomLine gLine)
        {
            if (tos is Family family) {
                ChildRef childRef = new();
                childRef.Ref = gLine.LineVal;
                family.ChildRefList.Add(childRef);
                return childRef;
            }
            return null;
        }


        private object? HandleCity(object tos, GedcomLine gLine)
        {
            if (tos is Address address && address.City == null) {
                address.City = gLine.LineVal;
            }
            return null;
        }

        private object? HandleCont(object tos, bool insertNewLine, GedcomLine gLine)
        {
            if(tos is IGedcomField field)
            {
                field.Value ??= "";

                if (insertNewLine)
                    field.Value += "\n";
                field.Value += gLine.LineVal;
            }

            return null;
        }

        private object? HandleCopr(object tos, GedcomLine gLine)
        {
            if (tos is Header header && header.Copr == null)
                header.Copr = gLine.LineVal;
            else if (tos is GeneratorData data && data.Copr == null)
                data.Copr = gLine.LineVal;

            return null;
        }

        private object? HandleCorp(object tos, GedcomLine gLine)
        {
            if (tos is Generator generator && generator.Corp == null)
            {
                GeneratorCorporation generatorCorporation = new();
                generatorCorporation.Value = gLine.LineVal;
                generator.Corp = generatorCorporation;
                return generatorCorporation;
            }
            return null;
        }

        private object? HandleCtry(object tos, GedcomLine gLine)
        {
            if (tos is Address address && address.Ctry == null)
                address.Ctry = gLine.LineVal;
            return null;
        }

        private object? HandleData(object tos, GedcomLine gLine)
        {
            if (tos is Generator generator && generator.Data == null) {
                GeneratorData generatorData = new();
                generatorData.Value = gLine.LineVal;
                generator.Data = generatorData;
                return generatorData;
            }
            else if (tos is SourceCitation citation) {
                // what a hack - if we come across a second data tag, set contents to separate
                if (citation.DataTagContents == SourceCitation.DataTagContentsEnum.DATE || citation.DataTagContents == SourceCitation.DataTagContentsEnum.TEXT)
                {
                    citation.DataTagContents = SourceCitation.DataTagContentsEnum.SEPARATE;
                }
                return tos; // move data attributes directly onto source citation
            }
            return null;
        }

        private object? HandleDate(object tos, GedcomLine gLine, Stack<object> objectStack)
        {
            if (tos is GeneratorData data && data.Date == null)
                data.Date = gLine.LineVal;
            else if (tos is Source source && source.Date == null)
                source.Date = gLine.LineVal;
            else if (tos is EventFact fact && fact.Date == null)
                fact.Date = gLine.LineVal;
            else if (tos is SourceCitation && ((SourceCitation)tos).Date == null)
                SetDataTagContents(tos, true, objectStack);
            else if ((tos is Header header1 && header1.Date == null) ||
                     (tos is Change change && change.Date == null))
            {
                Model.DateTime dateTime = new();
                dateTime.Value = gLine.LineVal;
                if (tos is Header header)
                {
                    header.Date = dateTime;
                }
                else
                {
                    ((Change)tos).Date = dateTime;
                }
                return dateTime;
            }
            return null;
        }

        private void SetDataTagContents(object tos, bool addDate, Stack<object> objectStack)
        {
            // what a hack - everyone uses the DATA tag differently; some people put only DATE under it, others only TEXT, others both
            // and still others put DATE and TEXT under two separate DATA tags
            // if the second-from-top-of-stack is also a source citation, then we're skipping a DATA tag and we need to set DataTagContents
            if (objectStack.Count > 2)
            {
                var firstObj = objectStack.Pop();
                if (objectStack.Peek() is SourceCitation)
                {
                    var dataTagContents = ((SourceCitation)tos).DataTagContents;
                    if (dataTagContents == null && addDate)
                    {
                        dataTagContents = SourceCitation.DataTagContentsEnum.DATE;
                    }
                    else if (dataTagContents == null && !addDate)
                    {
                        dataTagContents = SourceCitation.DataTagContentsEnum.TEXT;
                    }
                    else if (dataTagContents == SourceCitation.DataTagContentsEnum.TEXT && addDate ||
                             dataTagContents == SourceCitation.DataTagContentsEnum.DATE && !addDate)
                    {
                        dataTagContents = SourceCitation.DataTagContentsEnum.COMBINED;
                    }
               ((SourceCitation)tos).DataTagContents = dataTagContents;
                }
                objectStack.Push(firstObj);
            }
        }

        private object? HandleDesc(object tos, GedcomLine gLine)
        {
            if (tos is Submission submission && submission.Desc == null)
                submission.Desc = gLine.LineVal;
            return null;
        }

        private object? HandleDesi(object tos, GedcomLine gLine)
        {
            if (tos is Person person && person.Desi == null && gLine.Xref != null)
                person.Desi = gLine.Xref;
            return null;
        }

        private object? HandleDest(object tos, GedcomLine gLine)
        {
            if (tos is Header header && header.Dest == null) 
                header.Dest = gLine.LineVal;
            return null;
        }

        private object? HandleEmail(object tos, GedcomLine gLine)
        {
            if (tos is Submitter submitter && submitter.Email == null)
            {
                submitter.Email = gLine.LineVal;
                submitter.EmailTag = gLine.Tag;
            }
            else if (tos is GeneratorCorporation corporation && corporation.Email == null)
            {
                corporation.Email = gLine.LineVal;
                corporation.EmailTag = gLine.Tag;
            }
            else if (tos is EventFact fact && fact.Email == null)
            {
                fact.Email = gLine.LineVal;
                fact.EmailTag = gLine.Tag;
            }
            else if (tos is Person person && person.Email == null)
            {
                person.Email = gLine.LineVal;
                person.EmailTag = gLine.Tag;
            }
            else if (tos is Repository repository && repository.Email == null)
            {
                repository.Email = gLine.LineVal;
                repository.EmailTag = gLine.Tag;
            }

            return null;
        }

        private object? HandleEventFact(object tos, GedcomLine gLine)
        {
            var tagNameUpper = gLine.Tag.ToUpperInvariant();
            if ((tos is Person && EventFact.PERSONAL_EVENT_FACT_TAGS.Contains(tagNameUpper)) ||
                (tos is Family && EventFact.FAMILY_EVENT_FACT_TAGS.Contains(tagNameUpper))) {
                EventFact eventFact = new();
                eventFact.Tag = gLine.Tag;
                eventFact.Value = gLine.LineVal;
                ((PersonFamilyCommonContainer)tos).EventsFacts.Add(eventFact);
                return eventFact;
            }
            return null;
        }

        private object? HandleFam(object tos, GedcomLine gLine)
        {
            if (tos is Gedcom gedcom) {
                Family family = new();
                family.Id = gLine.Xref;
                gedcom.Families.Add(family);
                return family;
            }
            return null;
        }

        private object? HandleFamc(object tos, GedcomLine gLine)
        {
            if (tos is Person person) {
                ParentFamilyRef parentFamilyRef = new();
                parentFamilyRef.Ref = gLine.LineVal;
                person.Famc.Add(parentFamilyRef);
                return parentFamilyRef;
            }
            return null;
        }

        private object? HandleFams(object tos, GedcomLine gLine)
        {
            if (tos is Person person) {
                SpouseFamilyRef spouseFamilyRef = new();
                spouseFamilyRef.Ref = gLine.LineVal;
                person.Fams.Add(spouseFamilyRef);
                return spouseFamilyRef;
            }
            return null;
        }

        private object? HandleFax(object tos, GedcomLine gLine)
        {
            if (tos is GeneratorCorporation corporation && corporation.Fax == null)
                corporation.Fax = gLine.LineVal;
            else if (tos is Repository repository && repository.Fax == null)
                repository.Fax = gLine.LineVal;
            else if (tos is EventFact fact && fact.Fax == null)
                fact.Fax = gLine.LineVal;
            else if (tos is Person person && person.Fax == null)
                person.Fax = gLine.LineVal;
            else if (tos is Submitter submitter && submitter.Fax == null)
                submitter.Fax = gLine.LineVal;
            return null;
        }

        private object? HandleFile(object tos, GedcomLine gLine)
        {
            if (tos is Header header && header.File == null)
            {
                header.File = gLine.LineVal;
            }
            else if (tos is Media media && media.File == null)
            {
                media.File = gLine.LineVal;
                media.FileTag = gLine.Tag;
            }
            return null;
        }

        private object? HandleFone(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Fone == null)
                name.Fone = gLine.LineVal;
            return null;
        }

        private object? HandleForm(object tos, GedcomLine gLine)
        {
            if (tos is GedcomVersion version && version.Form == null)
                version.Form = gLine.LineVal;
            else if (tos is Media media && media.Form == null)
                media.Form = gLine.LineVal;
            return null;
        }

        private object? HandleFRel(object tos, GedcomLine gLine)
        {
            if (tos is ChildRef && ((ChildRef)tos).Frel == null) {
                ParentRelationship parentRelationship = new();
                ((ChildRef)tos).Frel = parentRelationship;
                return parentRelationship;
            }
            return null;
        }

        private object? HandleGedc(object tos)
        {
            if (tos is Header header && header.Gedc == null) {
                GedcomVersion gedcomVersion = new();
                header.Gedc = gedcomVersion;
                return gedcomVersion;
            }
            return null;
        }

        private object? HandleGivn(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Givn == null) {
                name.Givn = gLine.LineVal;
            }
            return null;
        }
        private object? HandleHead(object tos)
        {
            if (tos is Gedcom gedcom && gedcom.Head == null) {
                Header header = new();
                gedcom.Head = header;
                return header;
            }
            return null;
        }

        private object? HandleHusb(object tos, GedcomLine gLine)
        {
            if (tos is Family family) {
                SpouseRef spouseRef = new();
                spouseRef.Ref = gLine.LineVal;
                family.HusbandRefList.Add(spouseRef);
                return spouseRef;
            }
            return null;
        }

        private object? HandleIndi(object tos, GedcomLine gLine)
        {
            if (tos is Gedcom gedcom) {
                Person person = new();
                person.Id = gLine.Xref;
                gedcom.People.Add(person);
                return person;
            }
            return null;
        }

        private object? HandleItalic(object tos, GedcomLine gLine)
        {
            if (tos is Source source && source.Italic == null) {
                source.Italic = gLine.LineVal;
            }
            return null;
        }

        private object? HandleLang(object tos, GedcomLine gLine)
        {
            if (tos is Submitter submitter && submitter.Lang == null)
                submitter.Lang = gLine.LineVal;
            else if(tos is Header header && header.Lang == null) {
                header.Lang = gLine.LineVal;
            }
            return null;
        }

        private object? HandleLdsOrdinance(object tos, bool isPersonalOrdinance, GedcomLine gLine)
        {
            if ((tos is Person && isPersonalOrdinance) ||  (tos is Family && !isPersonalOrdinance)) {
                LdsOrdinance ldsOrd = new()
                {
                    Value = gLine.LineVal,
                    Tag = gLine.Tag.ToUpperInvariant()
                };
                ((PersonFamilyCommonContainer)tos).LdsOrdinances.Add(ldsOrd);
                return ldsOrd;
            }
            return null;
        }

        private object? HandleMarrnm(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Marrnm == null) {
                name.MarrnmTag = gLine.Tag;
                name.Marrnm = gLine.LineVal;
            }
            return null;
        }

        private object? HandleMedi(object tos, GedcomLine gLine)
        {
            if (tos is Source source && source.Medi == null)
            {
                source.Medi = gLine.LineVal;
            }
            // move medi tag out from under caln in a repository ref
            //TODO
            //else if (tos is FieldRef && ((FieldRef)tos).getTarget() is RepositoryRef &&
            //         ((FieldRef)tos).getFieldName().equals("CallNumber") &&
            //         ((RepositoryRef)((FieldRef)tos).getTarget()).getMediaType() == null) {
            //          ((RepositoryRef)((FieldRef)tos).getTarget()).setMediUnderCalnTag(true);
            //          return new FieldRef(((FieldRef)tos).getTarget(), "MediaType");
            //      }
            return null;
        }

        private object? HandleMRel(object tos, GedcomLine gLine)
        {
            if (tos is ChildRef cRef && cRef.Mrel == null) {
                ParentRelationship parentRelationship = new();
                parentRelationship.Value = gLine.LineVal;
                cRef.Mrel = parentRelationship;
                return parentRelationship;
            }
            return null;
        }

        private object? HandleName(object tos, GedcomLine gLine)
        {
            if (tos is Generator generator && generator.Name == null)
                generator.Name = gLine.LineVal;
            else if (tos is Repository repository && repository.Name == null)
                repository.Name = gLine.LineVal;
            else if (tos is Address address && address.Name == null)
                address.Name = gLine.LineVal;
            else if (tos is Submitter submitter && submitter.Name == null)
                submitter.Name = gLine.LineVal;
            else if (tos is Source source && source.Titl == null) // hack
                source.Titl = gLine.LineVal;
            else if (tos is Person person)
            {
                Name name = new();
                name.Value = gLine.LineVal;
                person.Names.Add(name);
                return name;
            }
            return null;
        }

        private object? HandleNick(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Nick == null) {
                name.Nick = gLine.LineVal;
            }
            return null;
        }

        private object? HandleNpfx(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Npfx == null) {
                name.Npfx = gLine.LineVal;
            }
            return null;
        }

        private object? HandleNsfx(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Nsfx == null) {
                name.Nsfx = gLine.LineVal;
            }
            return null;
        }

        private object? HandleNote(object tos, GedcomLine gLine)
        {
            if (tos is NoteContainer container) {
                if (gLine.Xref == null)
                {
                    Note note = new();
                    note.Value = gLine.LineVal;
                    container.Notes.Add(note);
                    return note;
                }
                else
                {
                    NoteRef noteRef = new();
                    noteRef.Ref = gLine.Xref;
                    container.NoteRefs.Add(noteRef);
                    return noteRef;
                }
            }
            else if (tos is Gedcom gedcom1) {
                Note note = new();
                if (gLine.LineVal != null)
                {
                    note.Id = gLine.LineVal;
                }
                if (gLine.Xref != null)
                {
                    // ref is invalid here, so store it as value - another geni-ism
                    note.Value = "@" + gLine.Xref + "@";
                }
                gedcom1.Notes.Add(note);
                return note;
            }
            return null;
        }

        private object? HandleObje(object tos, GedcomLine gLine)
        {
            if (tos is MediaContainer container)
            {
                if (gLine.Xref == null)
                {
                    Media media = new();
                    container.Media.Add(media);
                    return media;
                }
                else
                {
                    MediaRef mediaRef = new MediaRef();
                    mediaRef.Ref = gLine.Xref;
                    container.MediaRefs.Add(mediaRef);
                    return mediaRef;
                }
            }
            else if (tos is Gedcom gedcom1)
            {
                Media media = new();
                if (gLine.LineVal != null)
                {
                    media.Id = gLine.LineVal;
                }
               gedcom1.Media.Add(media);
                return media;
            }
            return null;
        }

        private object? HandleOrdi(object tos, GedcomLine gLine)
        {
            if (tos is Submission submission && submission.Ordi == null) {
                submission.Ordi = gLine.LineVal;
            }
            return null;
        }

        private object? HandlePage(object tos, GedcomLine gLine)
        {
            if (tos is SourceCitation citation && citation.Page == null) {
                citation.Page = gLine.LineVal;
            }
            return null;
        }

        private object? HandleParen(object tos, GedcomLine gLine)
        {
            if (tos is Source source && source.Paren == null) {
                source.Paren = gLine.LineVal;
            }
            return null;
        }

        private object? HandlePedi(object tos, GedcomLine gLine)
        {
            if (tos is ParentFamilyRef pref && pref.Pedi == null) {
                pref.Pedi = gLine.LineVal;
            }
            return null;
        }

        private object? HandlePhon(object tos, GedcomLine gLine)
        {
            if (tos is GeneratorCorporation corporation && corporation.Phon == null)
                corporation.Phon = gLine.LineVal;
            else if (tos is Repository repository && repository.Phon == null)
                repository.Phon = gLine.LineVal;
            else if (tos is EventFact fact && fact.Phon == null)
                fact.Phon = gLine.LineVal;
            else if (tos is Person person && person.Phon == null)
                person.Phon = gLine.LineVal;
            else if (tos is Submitter submitter && submitter.Phon == null)
                submitter.Phon = gLine.LineVal;
            return null;
        }

        private object? HandlePlac(object tos, GedcomLine gLine)
        {
            if (tos is EventFact fact && fact.Place == null) {
                fact.Place = gLine.LineVal;
            }
            return null;
        }

        private object? HandlePost(object tos, GedcomLine gLine)
        {
            if (tos is Address address && address.Post == null) {
                address.Post = gLine.LineVal;
            }
            return null;
        }

        private object? HandlePref(object tos, GedcomLine gLine)
        {
            if (tos is SpouseRef @ref && @ref.Pref == null) {
                @ref.Pref = gLine.LineVal;
            }
            return null;
        }

        private object? HandlePrim(object tos, GedcomLine gLine)
        {
            if (tos is Media media && media.Prim == null)
                media.Prim = gLine.LineVal;
            else if (tos is ParentFamilyRef @ref && @ref.Primary == null)
                @ref.Primary = gLine.LineVal;
            return null;
        }

        private object? HandlePubl(object tos, GedcomLine gLine)
        {
            if (tos is Source source && source.Publ  == null) {
                source.Publ = gLine.LineVal;
            }
            return null;
        }

        private object? HandleQuay(object tos, GedcomLine gLine)
        {
            if (tos is SourceCitation citation && citation.Quay == null) {
                citation.Quay = gLine.LineVal;
            }
            return null;
        }

        private object? HandleRefn(object tos, GedcomLine gLine)
        {
            if (tos is PersonFamilyCommonContainer container) {
                if (gLine.LineVal != null)
                    container.Refns.Add(gLine.LineVal);
            }
            else if (tos is Source source && source.Refn == null) {
                source.Refn = gLine.LineVal;
            }
            return null;
        }

        private object? HandleRela(object tos, GedcomLine gLine)
        {
            if (tos is Association && ((Association)tos).Rela == null) {
                ((Association)tos).Rela = gLine.LineVal;
            }
            return null;
        }

        private object? HandleRepo(object tos, GedcomLine gLine)
        {
            if (tos is Source source && source.Repo == null)
            {
                RepositoryRef repositoryRef = new();
                if (gLine.LineVal != null)
                {
                    repositoryRef.Ref = gLine.LineVal;
                }
                source.Repo = repositoryRef;
                return repositoryRef;
            }
            else if (tos is Gedcom gedcom1)
            {
                Repository repository = new();
                if (gLine.Xref != null)
                {
                    repository.Id = gLine.Xref;
                }
                gedcom1.Repositories.Add(repository);
                return repository;
            }
            return null;
        }

        private object? HandleRfn(object tos, GedcomLine gLine)
        {
            if (tos is Person && ((Person)tos).Rfn == null) {
                ((Person)tos).Rfn = gLine.LineVal;
            }
            return null;
        }

        private object? HandleRin(object tos, GedcomLine gLine)
        {
            if (tos is Submitter submitter && submitter.Rin == null)
                submitter.Rin = gLine.LineVal;
            else if (tos is Note note && note.Rin == null)
                note.Rin = gLine.LineVal;
            else if (tos is Repository repository && repository.Rin == null)
                repository.Rin = gLine.LineVal;
            else if (tos is EventFact fact && fact.Rin == null)
                fact.Rin = gLine.LineVal;
            else if (tos is Source source && source.Rin == null)
                source.Rin = gLine.LineVal;
            else if (tos is PersonFamilyCommonContainer container && container.Rin == null)
                container.Rin = gLine.LineVal;
            return null;
        }

        private object? HandleRomn(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Romn == null) {
                name.RomnTag = gLine.Tag;
                name.Romn = gLine.LineVal;
            }
            return null;
        }

        private object? HandleScbk(object tos, GedcomLine gLine)
        {
            if (tos is Media media && media.Scbk == null) {
                media.Scbk = gLine.LineVal;
            }
            return null;
        }
        private object? HandleStae(object tos, GedcomLine gLine)
        {
            if (tos is Address address && address.Stae == null) {
                address.Stae = gLine.LineVal;
            }
            return null;
        }

        private object? HandleStat(object tos, GedcomLine gLine)
        {
            if (tos is LdsOrdinance ordinance && ordinance.Stat == null) {
                ordinance.Stat = gLine.LineVal;
            }
            return null;
        }

        private object? HandleSubm(object tos, GedcomLine gLine)
        {
            if (tos is Header header && gLine.LineVal != null && header.SubmRef == null)
            {
                header.SubmRef = gLine.LineVal;
                //return new object(); // placeholder
            }
            else if (tos is Gedcom gedcom)
            {
                Submitter submitter = new();
                if (gLine.Xref != null)
                    submitter.Id = gLine.Xref;
                gedcom.Subms.Add(submitter);
                return submitter;
            }
            return null;
        }

        private object? HandleSubn(object tos, GedcomLine gLine)
        {
            if (tos is Header header && gLine.LineVal != null && header.SubnRef == null)
            {
                header.SubnRef = gLine.LineVal;
            }
            else if (tos is Header header1 && gLine.LineVal == null && header1.Subn == null)
            {
                Submission submission = new();
                header1.Subn = submission;
                return submission;
            }
            else if (tos is Gedcom gedcom1 && gedcom1.Subn == null)
            {
                Submission submission = new();
                if (gLine.LineVal != null)
                {
                    submission.Id = gLine.LineVal;
                }
                gedcom1.Subn = submission;
                return submission;
            }
            return null;
        }

        private object? HandleSurn(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Surn == null)
                name.Surn = gLine.LineVal;
            return null;
        }

        private object? HandleTemp(object tos, GedcomLine gLine)
        {
            if (tos is LdsOrdinance ordinance && ordinance.Temp == null) {
                ordinance.Temp = gLine.LineVal;
            }
            return null;
        }

        private object? HandleText(object tos, GedcomLine gLine, Stack<object> objectStack)
        {
            if (tos is SourceCitation citation)
            {
                SetDataTagContents(tos, false, objectStack);
                // standard says you should just have 1 TEXT tag, but geni uses multiple text tags in place of CONT tags here
                var text = citation.Text;
                if (text != null)
                {
                    citation.Text = text + "\n";
                }
                citation.Text = gLine.LineVal;
            }
            else if (tos is Source source)
            {
                var text = source.Text;
                if (text != null)
                {
                    source.Text = text + "\n";
                }
                source.Text = gLine.LineVal;
            }
            return null;
        }


        private object? HandleSour(object tos, GedcomLine gLine /*string id, string refs*/)
        {
            if (tos is Header header && header.Sour == null)
            {
                Generator generator = new();
                generator.Value = gLine.LineVal;
                header.Sour = generator;
                return generator;
            }
            else if (tos is SourceCitationContainer ||
                     tos is Note ||
                     tos is NoteRef
                  // ||  (tos is FieldRef &&
                  // ((FieldRef)tos).getTarget() is Note &&
                  // ((FieldRef)tos).getFieldName().equals("Value"))
                  )
            {
                SourceCitation sourceCitation = new();
                if (gLine.LineVal != null)
                {
                    sourceCitation.Ref = gLine.LineVal;
                }
                if (tos is SourceCitationContainer container)
                {
                    container.SourceCitations.Add(sourceCitation);
                }
                else if (tos is Note note)
                {
                    note.SourceCitations.Add(sourceCitation);
                }
                else if (tos is NoteRef noteRef)
                {
                    noteRef.SourceCitations.Add(sourceCitation);
                }
                // else
                // {
                //     // Reunion puts source citations under value: 0 NOTE 1 CONT ... 2 SOUR
                //     Note note = (Note)((FieldRef)tos).getTarget();
                //     note.addSourceCitation(sourceCitation);
                //     note.setSourceCitationsUnderValue(true);
                // }
                return sourceCitation;
            }
            else if (tos is Gedcom gedcom)
            {
                Source source = new();
                if (gLine.Xref != null)
                {
                    source.Id = gLine.Xref;
                }
                gedcom.Sources.Add(source);
                return source;
            }
            return null;
        }

        private object? HandleSpfx(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Spfx == null) {
                name.Spfx = gLine.LineVal;
            }
            return null;
        }

        private object? HandleSshow(object tos, GedcomLine gLine)
        {
            if (tos is Media media && media.Sshow == null) {
                media.Sshow = gLine.LineVal;
            }
            return null;
        }

        private object? HandleTime(object tos, GedcomLine gLine)
        {
            if (tos is Model.DateTime time && time.Time == null)
                time.Time = gLine.LineVal;
            return null;
        }

        private object? HandleTitl(object tos, GedcomLine gLine)
        {
            if (tos is Media media && media.Titl == null)
                media.Titl = gLine.LineVal;
            else if(tos is Source source && source.Titl == null)
                source.Titl = gLine.LineVal;
            return null;
        }

        private object? HandleTrlr(object tos, GedcomLine gLine)
        {
            if (tos is Gedcom) {
                // don't attach trailer to gedcom
                //return new Trailer();
                return null;
            }
            return null;
        }


        private object? HandleType(object tos, GedcomLine gLine)
        {
            if (tos is Name name && name.Type == null)
            {
                name.TypeTag = gLine.Tag;
                name.Type = gLine.LineVal;
            }
            else if (tos is Media media && media.Type == null)
            {
                media.Type = gLine.LineVal;
            }
            else if (tos is EventFact fact && fact.Type == null)
            {
                fact.Type = gLine.LineVal;
            }
            else if (tos is Association association && association.Type == null)
            {
                association.Type = gLine.LineVal;
            }
            else if (tos is Source source && source.Type == null)
            {
                source.TypeTag = gLine.Tag;
                source.Type = gLine.LineVal;
            }
            return null;
        }

        private object? HandleUid(object tos, GedcomLine gLine)
        {
            if (tos is PersonFamilyCommonContainer container && container.Uid == null)
            {
                container.UidTag = gLine.Tag;
                container.Uid = gLine.LineVal;
            }
            else if (tos is EventFact fact && fact.Uid == null)
            {
                fact.UidTag = gLine.Tag;
                fact.Uid = gLine.LineVal;
            }
            else if (tos is Source source && source.Uid == null) 
            {
                source.UidTag = gLine.Tag;
                source.Uid = gLine.LineVal;
            }
            return null;
        }

        private object? HandleVers(object tos, GedcomLine gLine)
        {
            if (tos is Generator generator && generator.Vers == null)
                generator.Vers = gLine.LineVal;
            else if (tos is GedcomVersion version && version.Value == null)
                version.Value = gLine.LineVal;
            else if (tos is CharacterSet set && set.Vers == null)
                set.Vers = gLine.LineVal;
            
            return null;
        }

        private object? HandleWife(object tos, GedcomLine gLine)
        {
            if (tos is Family family) {
                SpouseRef spouseRef = new();
                spouseRef.Ref = gLine.LineVal;
                family.WifeRefList.Add(spouseRef);
                return spouseRef;
            }
            return null;
        }

        private object? HandleWww(object tos, GedcomLine gLine)
        {
            if (tos is GeneratorCorporation corporation && corporation.Www == null)
            {
                corporation.Www = gLine.LineVal;
                corporation.WwwTag = gLine.Tag;
            }
            else if (tos is Repository repository && repository.Www == null)
            {
                repository.Www = gLine.LineVal;
                repository.WwwTag = gLine.Tag;
            }
            else if (tos is EventFact fact && fact.Www == null)
            {
                fact.Www = gLine.LineVal;
                fact.WwwTag = gLine.Tag;
            }
            else if (tos is Person person && person.Www == null)
            {
                person.Www = gLine.LineVal;
                person.WwwTag = gLine.Tag;
            }
            else if (tos is Submitter submitter && submitter.Www == null)
            {
                submitter.Www = gLine.LineVal;
                submitter.WwwTag = gLine.Tag;
            }
        
            return null;
        }


    }
}
