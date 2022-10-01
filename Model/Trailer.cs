using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal class Trailer : ExtensionContainer
    {
        // TODO do we need this?  how many gedcoms attach stuff to the trailer?
        public override void Accept(FileStream fs, IVisitor visitor)
        {
            // ignore
        }
    }
}
