using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gedcom5_csharp.Model
{
    abstract class MediaContainer : NoteContainer
    {
        public List<MediaRef> MediaRefs { get; set; } = new();
        public List<Media> Media { get; set; } = new();
        public void AddMedia(Media mediaObject)
        {
            Media ??= new ();
            Media.Add(mediaObject);
        }

        public override void VisitContainedObjects(FileStream fs, IVisitor visitor)
        {
            foreach (MediaRef mediaRef in MediaRefs)
            {
                mediaRef.Accept(fs, visitor);
            }
            foreach (Media m in Media)
            {
                m.Accept(fs, visitor);
            }
            base.VisitContainedObjects(fs, visitor);
        }

    }
}
