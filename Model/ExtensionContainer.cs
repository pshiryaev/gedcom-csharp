using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    abstract class ExtensionContainer : IVisitable
    {
        public Dictionary<string, object> ExtensionList { get; set; } = new Dictionary<string, object>(); 

        public virtual void VisitContainedObjects(FileStream fs, IVisitor visitor)
        {
            foreach(var entry in ExtensionList)
            {
                visitor.Visit(fs, entry.Key, entry.Value);
            }
        }

        public abstract void Accept(FileStream fs, IVisitor visitor);
    }
}
