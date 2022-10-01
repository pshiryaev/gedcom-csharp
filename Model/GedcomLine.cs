using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class GedcomLine
    {
        public int Level { get; set; } = -1;
        public string Tag { get; set; } = string.Empty;
        public string? Xref { get; set; }
        public string? LineVal { get; set; }

    }
}
