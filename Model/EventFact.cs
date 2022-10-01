using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class EventFact : SourceCitationContainer, IGedcomField
    {
        public string? Value { get; set; }
        public string? Tag { get; set; }
        public string? Type { get; set; }
        public string? Date { get; set; }
        public string? Place { get; set; }
        public Address? Addr { get; set; }
        public string? Phon { get; set; }
        public string? Fax { get; set; }
        public string? Rin { get; set; }
        public string? Caus { get; set; }
        public string? Uid { get; set; }
        public string? UidTag { get; set; }
        public string? Email { get; set; }
        public string? EmailTag { get; set; }
        public string? Www { get; set; }
        public string? WwwTag { get; set; }

        public static List<string> PERSONAL_EVENT_FACT_TAGS = new()
        {
            "ADOP", "ADOPTION", "ADULT_CHRISTNG", "AFN", "ARRI", "ARVL", "ARRIVAL", "_ATTR",
            "BAP", "BAPM", "BAPT", "BAPTISM", "BARM", "BAR_MITZVAH", "BASM", "BAS_MITZVAH", "BATM", "BAT_MITZVAH", "BIRT", "BIRTH", "BLES", "BLESS", "BLESSING", "BLSL", "BURI", "BURIAL",
            "CAST", "CASTE", "CAUS", "CENS", "CENSUS", "CHILDREN_COUNT", "CHR", "CHRA", "CHRISTENING", "CIRC", "CITN", "_COLOR", "CONF", "CONFIRMATION", "CREM", "CREMATION",
            "_DCAUSE", "DEAT", "DEATH", "_DEATH_OF_SPOUSE", "DEED", "_DEG", "_DEGREE", "DEPA", "DPRT", "DSCR", "DWEL",
            "EDUC", "EDUCATION", "_ELEC", "EMAIL", "EMIG", "EMIGRATION", "EMPL", "_EMPLOY", "ENGA", "ENLIST", "EVEN", "EVENT", "_EXCM", "EXCO", "EYES",
            "FACT", "FCOM", "FIRST_COMMUNION", "_FNRL", "_FUN",
            "_FA1", "_FA2", "_FA3", "_FA4", "_FA5", "_FA6", "_FA7", "_FA8", "_FA9", "_FA10", "_FA11", "_FA12", "_FA13",
            "GRAD", "GRADUATION",
            "HAIR", "HEIG", "_HEIG", "_HEIGHT",
            "IDNO", "IDENT_NUMBER", "_INTE",
            "ILL", "ILLN", "IMMI", "IMMIGRATION",
            "LVG", "LVNG",
            "MARR", "MARRIAGE_COUNT", "_MDCL", "_MEDICAL", "MIL", "_MIL", "MILA", "MILD", "MILI", "_MILI", "MILT", "_MILT", "_MILTID", "MISE", "_MISE", "_MILITARY_SERVICE", "MISN ", "_MISN", "MOVE",
            "_NAMS", "NATI", "NATIONALITY", "NATU", "NATURALIZATION", "NCHI", "NMR",
            "OCCU", "OCCUPATION", "ORDI", "ORDL", "ORDN", "ORDINATION",
            "PHON", "PHY_DESCRIPTION", "PROB", "PROBATE", "PROP", "PROPERTY",
            "RACE", "RELI", "RELIGION", "RESI", "RESIR", "RESIDENCE", "RETI", "RETIREMENT",
            "SEX", "SOC_SEC_NUMBER", "SSN", "STIL", "STLB",
            "TITL", "TITLE",
            "WEIG", "_WEIG", "_WEIGHT",
            "WILL"
        };

        public static List<string> FAMILY_EVENT_FACT_TAGS = new(){
            "ANUL", "ANNULMENT",
            "CENS", "CLAW",
            "_DEATH_OF_SPOUSE", "DIV", "DIVF", "DIVORCE", "_DIV",
            "EMIG", "ENGA", "EVEN", "EVENT",
            "IMMI",
            "MARB", "MARC", "MARL", "MARR", "MARRIAGE", "MARS", "_MBON",
            "NCHI",
            "RESI",
            "SEPA", "_SEPR", "_SEPARATED"
        };


        public static Dictionary<string, string> DISPLAY_TYPE = new() {
            { "ADOP", "adop" },
            {"ADOPTION", "adop"},
            {"AFN", "afn"},
            {"ANUL", "anul"},
            {"ANNULMENT", "anul"},
            {"ARRIVAL", "arvl"},
            {"ARRI", "arvl"},
            {"ARVL", "arvl"},
            {"_ATTR", "attr"},
            {"BAP", "bapm"},
            {"BAPM", "bapm"},
            {"BAPT", "bapm"},
            {"BAPTISM", "bapm"},
            {"BARM", "barm"},
            {"BAR_MITZVAH", "barm"},
            {"BATM", "batm"},
            {"BIRT", "birt"},
            {"BIRTH", "birt"},
            {"BLES", "bles"},
            {"BURI", "buri"},
            {"BURIAL", "buri"},
            {"CAST", "cast"},
            {"CAUS", "caus"},
            {"CAUSE", "caus"},
            {"CENS", "cens"},
            {"CHR", "chr"},
            {"CHRISTENING", "chr"},
            {"CLAW", "claw"},
            {"_COLOR", "color"},
            {"CONF", "conf"},
            {"CREM", "crem"},
            {"_DCAUSE", "caus"},
            {"DEAT", "deat"},
            {"DEATH", "deat"},
            {"_DEATH_OF_SPOUSE", "death_of_spouse"},
            {"DEED", "deed"},
            {"_DEG", "deg"},
            {"_DEGREE", "deg"},
            {"DEPA", "dprt"},
            {"DPRT", "dprt"},
            {"DIV", "div"},
            {"DIVF", "divf"},
            {"DIVORCE", "div"},
            {"_DIV", "div"},
            {"DSCR", "dscr"},
            {"EDUC", "educ"},
            {"EDUCATION", "educ"},
            {"_ELEC", "elec"},
            {"EMAIL", "email"},
            {"EMIG", "emig"},
            {"EMIGRATION", "emig"},
            {"EMPL", "empl"},
            {"_EMPLOY", "empl"},
            {"ENGA", "enga"},
            {"ENLIST", "milt"},
            {"EVEN", "even"},
            {"EVENT", "even"},
            {"_EXCM", "excm"},
            {"EYES", "eyes"},
            {"FCOM", "fcom"},
            {"_FNRL", "fnrl"},
            {"_FUN", "fnrl"},
            {"GRAD", "grad"},
            {"GRADUATION", "grad"},
            {"HAIR", "hair"},
            {"HEIG", "heig"},
            {"_HEIG", "heig"},
            {"_HEIGHT", "heig"},
            {"ILL", "ill"},
            {"IMMI", "immi"},
            {"IMMIGRATION", "immi"},
            {"MARB", "marb"},
            {"MARC", "marc"},
            {"MARL", "marl"},
            {"MARR", "marr"},
            {"MARRIAGE", "marr"},
            {"MARS", "mars"},
            {"_MBON", "marb"},
            {"_MDCL", "mdcl"},
            {"_MEDICAL", "mdcl"},
            {"MIL", "milt"},
            {"_MIL", "milt"},
            {"MILI", "milt"},
            {"_MILI", "milt"},
            {"_MILT", "milt"},
            {"_MILTID", "milt"},
            {"_MILITARY_SERVICE", "milt"},
            {"MISE", "milt"},
            {"_MISN", "misn"},
            {"_NAMS", "nams"},
            {"NATI", "nati"},
            {"NATU", "natu"},
            {"NATURALIZATION", "natu"},
            {"NCHI", "nchi"},
            {"OCCU", "occu"},
            {"OCCUPATION", "occu"},
            {"ORDI", "ordn"},
            {"ORDN", "ordn"},
            {"PHON", "phon"},
            {"PROB", "prob"},
            {"PROP", "prop"},
            {"RELI", "reli"},
            {"RELIGION", "reli"},
            {"RESI", "resi"},
            {"RESIDENCE", "resi"},
            {"RETI", "reti"},
            {"SEPA", "sepa"},
            {"_SEPARATED", "sepa"},
            {"_SEPR", "sepa"},
            {"SEX", "sex"},
            {"SSN", "ssn"},
            {"SOC_SEC_NUMBER", "ssn"},
            {"TITL", "titl"},
            {"TITLE", "titl"},
            {"_WEIG", "weig"},
            {"_WEIGHT", "weig"},
            {"WILL", "will"},
        };

        //public string getDisplayType()
        //{
        //    ResourceBundle resourceBundle = ResourceBundle.getBundle("EventFact", Locale.getDefault());
        //    if (tag != null)
        //    {
        //        String key = DISPLAY_TYPE.get(tag.toUpperCase());
        //        if (key != null)
        //        {
        //            return resourceBundle.getString(key);
        //        }
        //    }
        //    return resourceBundle.getString("other"},
        //}
        public override void VisitContainedObjects(FileStream fs, IVisitor visitor)
        {
            if (Addr != null)
            {
                Addr.Accept(fs, visitor);
            }
            base.VisitContainedObjects(fs, visitor);
        }

        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                this.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }
    }
}

