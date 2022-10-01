using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    internal interface IVisitable
    {
        void Accept(FileStream fs, IVisitor visitor);
    }
}
