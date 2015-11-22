using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace DirectoryImage {    
    public abstract class _TreeNode :TreeNode {
        public _TreeNode(string text, int imageindex, int selectedimageindex):
            base(text, imageindex, selectedimageindex) { }
        public abstract void FillListView(ListView listView);
        public abstract new void Expand();
        public abstract new void Collapse();
        public abstract bool IsFolder { get;}        
    }
}
