using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace DirectoryImage
{
    public abstract class dImagen
    {
        public abstract int Length { get; }
        public abstract void SaveLine(StreamWriter writer);
        public abstract void LoadLine(StreamReader reader);
        public abstract TreeNode Root { get; }
        public abstract string Name { get; set; }
        public abstract string Path { get; }
        public abstract int Folders { get; }
        public abstract int Files { get; }
        public abstract List<XmlElement> Search(Regex pattern);
    }
}
