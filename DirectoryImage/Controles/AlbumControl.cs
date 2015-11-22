using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.IO;

namespace DirectoryImage
{
    public partial class AlbumControl : UserControl
    {        
        Label _lblStatus;
        frmPrincipal _parent;
        /// <summary>
        /// Album controlado por el AlbumControl.
        /// </summary>
        public dAlbum thisAlbum;
        Comparador _comparador;
        public bool _documentChange = false;
        public Label lblStatus {
            get { return this._lblStatus; }
            set { this._lblStatus = value; }
        }
        public bool ChangeIsDone
        {
            get
            {
                return this._documentChange;
            }
            set
            {
                _documentChange = value;                
            }
        }

        /// <summary>
        /// Nombre del album q tiene el Control
        /// </summary>
        public string aName { get { return thisAlbum.Name; } }
        public TreeNode Root
        {
            get
            {
                return this.treeView.Nodes[0];            
            }
        }

        #region  Constructor
        public AlbumControl(frmPrincipal form, dAlbum album)
        {
            _parent = form;
            thisAlbum = album;
            InitializeComponent();
            treeView.Nodes.Add(thisAlbum.Root);
            treeView.SelectedNode = treeView.Nodes[0];

            _comparador = new Comparador(listView);
            listView.ListViewItemSorter = _comparador;
            listView.Sorting = SortOrder.None;
        }
        #endregion

        #region TreeView Events
        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            xmlTreeNode node = e.Node as xmlTreeNode;
            node.Expand();
        }
        private void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Level != 1)
                e.CancelEdit = true;
        }
        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            (e.Node as xmlTreeNode).Name = e.Label;
            ChangeIsDone = true;
        }
        private void treeView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                TreeNode node = treeView.SelectedNode;
                if (node != null && node.Level == 1)
                    node.BeginEdit();
            }
        }
        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode nodeTMP in e.Node.Nodes)
            {
                (nodeTMP as xmlTreeNode).Collapse();

            }
        }
        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BackColor = Color.White;
                treeView.SelectedNode.ForeColor = Color.Black;
            }
        }
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            xmlTreeNode node = e.Node as xmlTreeNode;
            e.Node.BackColor = Color.FromArgb(51, 153, 255);
            e.Node.ForeColor = Color.White;
            node.Expand();
            node.FillListView(listView);
            //ShowStatus();//TODO: Show status en un control.

        }
        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView.SelectedNode = e.Node;

            if (e.Button == MouseButtons.Right)
            {
                tsmiDelete.Visible = false;
                tsmiInsert.Visible = false;
                tsmiProperties.Visible = false;
                //tsmiRename.Visible = false;

                if (e.Node.Level == 1)//es una imagen.
                {
                    tsmiDelete.Visible = true;
                    tsmiProperties.Visible = true;
                    //tsmiRename.Visible = true;
                }
                else if (e.Node.Level == 0)
                    tsmiInsert.Visible = true;
                cmsMenu.Show(treeView, e.X + 10, e.Y + 5);
            }
        }
        #endregion

        #region ListView Events
        void ExpandirFromListView()
        {
            ListViewItem lvi = listView.FocusedItem;   //tomo item marcado en listview

            xmlTreeNode nodeParent = treeView.SelectedNode as xmlTreeNode;  //tomo nodo marcado en treeview
            if (lvi == null || nodeParent == null)
            {   //si alguno no existe dbclick invalido
                SubirNivel();
                return;
            }
            if (lvi.Text == "..")
                SubirNivel();
            foreach (xmlTreeNode nodo in nodeParent.Nodes)
            {
                if (nodo.Text == lvi.Text)
                {//es el nodo de igual name y carpeta
                    nodo.Collapse();
                    nodo.FillListView(listView);
                    treeView.SelectedNode = nodo;
                    return;
                }
            }
            //Si no es ninguno de los anteriores
            nodeParent.Execute(lvi.Name);
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExpandirFromListView();
        }
        private void listView_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                ExpandirFromListView();
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int columna = e.Column;
            _comparador.SortColumn = columna;
            if (e.Column == _comparador.SortColumn)
            {
                // Switch the sorting order
                if (listView.Sorting == SortOrder.Ascending)
                    listView.Sorting = SortOrder.Descending;
                else
                    listView.Sorting = SortOrder.Ascending;
            }
            else
                listView.Sorting = SortOrder.Ascending;
            
        }
        #endregion

        #region contexmenu_Logic

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            int selected = treeView.SelectedNode.Index;
            if (thisAlbum.DeleteImagen(selected))
            {
                treeView.Nodes[0].Nodes.RemoveAt(selected);
                treeView.SelectedNode = treeView.Nodes[0];
                ChangeIsDone = true;
                //TODO: el show status falta
                //ShowStatus();
            }

        }
        private void tsmiInsert_Click(object sender, EventArgs e)
        {
            _parent.NewImage();
        }
        private void tsmiProperties_Click(object sender, EventArgs e)
        {
            int iselected = treeView.SelectedNode.Index;
            xmlImagen selected = thisAlbum[iselected] as xmlImagen;
            MessageBox.Show(string.Format("Imagen\n\nName: {0}\n#_Folders: {1}\n#_Files: {2}",
                selected.Name, selected.Folders, selected.Files), "Properties", MessageBoxButtons.OK);

        }
        #endregion

        void SubirNivel()
        {
            TreeNode node = treeView.SelectedNode;
            if (node != null && node.Parent != null)
                treeView.SelectedNode = node.Parent;
        }

        #region Mostrar de una Busqueda
        /// <summary>
        /// Dado el camino completo busca el album donde esta y obliga a mostrar el archivo.
        /// </summary>
        /// <param name="path">Camino del archivo o carpeta.</param>
        public void ShowItemFromSearch(string[] path)
        {
            Root.Expand();
            var result = ShowRecursivo(path, Root, 0);
            treeView.SelectedNode = result;
            ShowItemsInListView(result);
            string last_name = path[path.Length - 1];
            
            if (result.Text == last_name)       //era una carpeta y ya esta seleccionada
                return;

            foreach (ListViewItem item in listView.Items)
            {
                if (item.Text == last_name)
                {
                    item.BackColor = Color.FromArgb(51, 153, 255);
                    item.ForeColor = Color.White;
                    return;
                }
            }
        }
        
        
        TreeNode ShowRecursivo(string[] path, TreeNode node, int index) 
        {
            if (node.Text.Replace("\\", "") != path[index])
                return null;
            if(node.Text.Replace("\\", "") == path[path.Length-1])
                return node;
            index++;

            TreeNode tmp = null;
            foreach (TreeNode item in node.Nodes)
            {
                if (item.Text.Replace("\\","") == path[index])
                {
                    item.Expand();
                    tmp = ShowRecursivo(path, item, index);
                }
            }
            if (tmp != null)
                return tmp;
            return node;        
        }
        //TODO: Arreglar esta mierda de metodo
        /// <summary>
        ///  Busca la informacion de node en el xml y lo pone en el listview
        /// </summary>
        /// <param name="node"></param>
        void ShowItemsInListView(TreeNode node)
        {
            xmlTreeNode xml_node = (xmlTreeNode)node;
            xml_node.FillListView(listView);
            //string nodePath = node.FullPath;
            //string[] nodePath2 = nodePath.Split(new Char[] { '\\' });
            //XmlNode xmlNode = FindXmlNode(nodePath2);

            //listView.Clear();                      //revisar
            //listView.Items.Clear();
            //listView.BeginUpdate();

            //#region Insertar Folder ..
            //ListViewItem lv = new ListViewItem("..", 0);
            //listView.Items.Add(lv);
            //#endregion

            //foreach (XmlNode xmlnode in xmlNode.ChildNodes)
            //{
            //    lv = new ListViewItem(xmlnode.Attributes[Atributos.Name].Value, 0);
            //    if (xmlnode.Name == Atributos.Folder)
            //        lv.ImageIndex = 0;
            //    else
            //    {
            //        string key = Path.GetExtension(xmlnode.Attributes[Atributos.Name].Value);
            //        lv.ImageIndex = _parent.imgList.Images.IndexOfKey(key);
            //        if (lv.ImageIndex == -1)
            //            key = _parent.Extension(key);
            //        lv.ImageIndex = _parent.imgList.Images.IndexOfKey(key);
            //        if (lv.ImageIndex == -1)
            //            lv.ImageIndex = 2;
            //    }

            //    long? size = long.Parse(xmlnode.Attributes[Atributos.Size].Value);
            //    long rest = 0;
            //    #region sizeFormat
            //    if (size.HasValue)
            //    {
            //        size /= 1024;
            //        rest = size.Value % 1024;
            //        size /= 1024;

            //    }
            //    #endregion
            //    lv.SubItems.Add(((size > 0) ? size.ToString() + ',' : "") + rest.ToString() + " KB");
            //    lv.SubItems.Add(xmlnode.Attributes[Atributos.CreationTime].Value);

            //    listView.Items.Add(lv);
            //}
            //listView.EndUpdate();
        }

        #endregion        
    }
   
    public class Comparador : IComparer
    {
        ListView lv;
        public int SortColumn = 0;
        public Comparador(ListView lv)
        {
            this.lv = lv;
        }
                
        int IComparer.Compare(object x, object y)
        {
            ListViewItem x1 = (ListViewItem)x;
            ListViewItem x2 = (ListViewItem)y;

            if (x1.SubItems.Count-1 < SortColumn)
                return 1;
            else if (x2.SubItems.Count-1 < SortColumn)
                return -1;

            if (lv.Sorting == SortOrder.Descending)
            {
                ListViewItem tmp = x1;
                x1 = x2;
                x2 = tmp;
            }

            if (lv.View != View.Details)
            {
                return CaseInsensitiveComparer.Default.Compare(x1.Text, x2.Text);
            }
            if (x1.SubItems.Count < SortColumn || x2.SubItems.Count < SortColumn)
                return 0;
            ListViewItem.ListViewSubItem sub1 = x1.SubItems[SortColumn];
            ListViewItem.ListViewSubItem sub2 = x2.SubItems[SortColumn];

            switch (SortColumn)
            {
                case 0:
                    {
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    }
                case 1:
                    {
                        try
                        {
                            string numero1 = sub1.Text.Replace("KB", "");
                            numero1 = numero1.Replace(",", "");
                            double d1 = double.Parse(numero1);
                            string numero2 = sub2.Text.Replace("KB", "");
                            numero2 = numero2.Replace(",", "");
                            double d2 = double.Parse(numero2);
                            return (d1 >= d2)?0:1;
                        }
                        catch { return 1; }
                    }
                case 2:
                    {
                        try
                        {
                            string f1 = sub1.Text.Split(' ')[0];
                            string f2 = sub2.Text.Split(' ')[0];
                            return CaseInsensitiveComparer.Default.Compare(f1, f2);
                        }
                        catch { return 1; }
                    }
            }
            throw new NotImplementedException();
        }

    }
}
