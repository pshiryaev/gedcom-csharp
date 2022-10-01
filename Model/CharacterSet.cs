using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class CharacterSet : ExtensionContainer, IGedcomField
    {
        public string? Value { get; set; }
        public string? Vers { get; set; }
        public override void Accept(FileStream fs, IVisitor visitor)
        {
            if (visitor.Visit(fs, this))
            {
                base.VisitContainedObjects(fs, visitor);
                visitor.EndVisit(this);
            }
        }

        public static string GetCorrectedCharsetName(string? generatorName, string? encoding, string? version)
        {
            // correct incorrectly-assigned encoding values
            if ("GeneWeb".Equals(generatorName) && "ASCII".Equals(encoding))
            {
                // GeneWeb ASCII -> Cp1252 (ANSI)
                encoding = "Cp1252";
            }
            else if ("Geni.com".Equals(generatorName) && "UNICODE".Equals(encoding))
            {
                // Geni.com UNICODE -> UTF-8
                encoding = "UTF-8";
            }
            else if ("Geni.com".Equals(generatorName) && "ANSEL".Equals(encoding))
            {
                // Geni.com ANSEL -> UTF-8
                encoding = "UTF-8";
            }
            else if ("GENJ".Equals(generatorName) && "UNICODE".Equals(encoding))
            {
                // GENJ UNICODE -> UTF-8
                encoding = "UTF-8";
            }

            // make encoding value java-friendly
            else if ("ASCII".Equals(encoding))
            { // ASCII followed by VERS MacOS Roman is MACINTOSH
                if ("MacOS Roman".Equals(version))
                {
                    encoding = "x-MacRoman";
                }
            }
            else if ("ATARIST_ASCII".Equals(encoding))
            {
                encoding = "ASCII";
            }
            else if ("MACROMAN".Equals(encoding) || "MACINTOSH".Equals(encoding))
            {
                encoding = "x-MacRoman";
            }
            else if ("ANSI".Equals(encoding) || "IBM WINDOWS".Equals(encoding))
            {
                encoding = "Cp1252";
            }
            else if ("WINDOWS-874".Equals(encoding))
            {
                encoding = "Cp874";
            }
            else if ("WINDOWS-1251".Equals(encoding))
            {
                encoding = "Cp1251";
            }
            else if ("WINDOWS-1254".Equals(encoding))
            {
                encoding = "Cp1254";
            }
            else if ("IBMPC".Equals(encoding) || "IBM DOS".Equals(encoding))
            {
                encoding = "Cp850";
            }
            else if ("UNICODE".Equals(encoding))
            {
                encoding = "UTF-16";
            }
            else if ("UTF-16BE".Equals(encoding))
            {
                encoding = "UnicodeBigUnmarked";
            }
            else if (encoding == null)
            {
                encoding = ""; // not found
            }

            return encoding;
        }

    }
}
