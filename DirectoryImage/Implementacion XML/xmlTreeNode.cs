using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace DirectoryImage {    
    public class xmlTreeNode :_TreeNode{
        XmlElement _element;
        /// <summary>
        /// Especifica si el TreeNode representa a un album
        /// </summary>
        bool IsRoot = false;
        public xmlTreeNode(string text, int imageindex, int selectedimageindex) : this(null, text, imageindex, selectedimageindex) { }
        public xmlTreeNode(XmlElement element)
            :
            base(element.Name, 0, 1) {
            this._element = element;

        }
        public xmlTreeNode(XmlElement element, string text, int imageindex, int selectedimageindex)
            :
            base(text, imageindex, selectedimageindex) 
        {
            if (element == null)//es root de una imagen.
                IsRoot = true;
            this._element = element;
        }
        public new string Name {
            get {
                if (IsRoot)
                    return base.Name;
                return _element.Attributes[Atributos.Name].Value; 
            }
            set {
                if (this.Level == 1)
                    _element.SetAttribute(Atributos.Name, value);
            
            }
        }

        public override void Expand() {
            if (this.Nodes.Count != 0) {
                foreach (xmlTreeNode treenode in this.Nodes) {
                    if (treenode.Nodes.Count != 0)
                        continue;
                    treenode.Expand();
                }
                return;
            }
            if (IsRoot) {
                return;
            }
            foreach (XmlElement element in _element.ChildNodes) {
                if (!element.HasChildNodes)//es un archivo
                    continue;
                TreeNode nodo = new xmlTreeNode(element);
                nodo.Text = element.Attributes[Atributos.Name].Value;
                foreach (XmlElement element2 in element.ChildNodes) {
                    TreeNode nodo2 = new xmlTreeNode(element2);
                    nodo2.Text = element.Attributes[Atributos.Name].Value;
                }
                Nodes.Add(nodo);
            }
        }

        public override void Collapse() {
            if (IsRoot) {
                return;
            }
            foreach (TreeNode node in Nodes) { 
                node.Nodes.Clear();
            }
            
        }
        public override void FillListView(ListView listView) {
            listView.Items.Clear();
            listView.BeginUpdate();

            if (IsRoot) {
                foreach (TreeNode node in this.Nodes) {
                    ListViewItem lv = new ListViewItem(node.Text, 22);
                    //lv.SubItems.Add(node.Attributes[Atributos.Size].Value);
                    listView.Items.Add(lv);
                }
            }
            else {
                #region Insertar Folder ..
                ListViewItem lv = new ListViewItem("..", 0);
                listView.Items.Add(lv);
                #endregion

                foreach (XmlNode xmlnode in _element.ChildNodes) {
                    lv = new ListViewItem(xmlnode.Attributes[Atributos.Name].Value, 0);
                    ImageList imgList = listView.SmallImageList;
                    string key = "";
                    if (xmlnode.Name != Atributos.Folder)
                        key = Path.GetExtension(xmlnode.Attributes[Atributos.Name].Value);
                    lv.ImageIndex = dAlbum.ImageIndex(key, imgList);
                    
                    long? size = long.Parse(xmlnode.Attributes[Atributos.Size].Value);
                    
                    lv.SubItems.Add(dAlbum.SizeFormat(size));
                    lv.SubItems.Add(xmlnode.Attributes[Atributos.CreationTime].Value);

                    listView.Items.Add(lv);
                }
            }
            listView.EndUpdate();
        }
        public override bool IsFolder {
            get { return _element.Name == "F"; }
        }
        /// <summary>
        /// Busca dado el nodo raíz, el camino hasta el final de full_path.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="full_path">Camino separado por \\, el primer valor coincide con el nombre del Album.</param>
        /// <returns></returns>
        public static string ShowNode(TreeNode root, string full_path) {
            string[] separeted_path = full_path.Split(new string[]{"\\"}, StringSplitOptions.RemoveEmptyEntries);
            TreeNode tmp_node = root ;
            tmp_node.Expand();
            //k es igual a 1 pues separeted_path en 0 tiene el nombre del album.
            string tmp;
            for(int k = 1; k < separeted_path.Length; k++){
                if (separeted_path[k] == "")
                    continue;
                foreach (TreeNode node in tmp_node.Nodes) {
                    tmp = node.Text.Replace("\\", "");
                    if (tmp == separeted_path[k])
                    {
                        node.Expand();                        
                        tmp_node = node;
                        break;
                    }
                }
            }
            tmp_node.TreeView.SelectedNode = tmp_node;
            string last_file = separeted_path[separeted_path.Length - 1];
            if (tmp_node.Text == last_file)     //es una carpeta
                return "";
            else                                //es un archivo           
                return last_file;            
        }
        public void Execute(string name){
            //TODO: ejecutar un archivo...
            
            return;
        }
    }
}
