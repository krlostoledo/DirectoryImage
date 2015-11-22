using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DirectoryImage
{
    public partial class AlbumControl : UserControl
    {
        public AlbumControl(frmPrincipal form, dAlbum album){
            _parent = form;
            thisAlbum = album;
            InitializeComponent();
            treeView.Nodes.Add(thisAlbum.Root) ;
            treeView.SelectedNode = treeView.Nodes[0] ;
        }
        Label _lblStatus;
        frmPrincipal _parent;
        public dAlbum thisAlbum;
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
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            xmlTreeNode node = e.Node as xmlTreeNode;
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
        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
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

        }
        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int i = e.Column;
            ListView.ListViewItemCollection list = listView.Items;

        }


        private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (treeView.SelectedNode.Level == 0)
            {
                //SelectedAlbum[e.Item].Name = e.Label;
                //FillTreeView();//TODO: Mejorar esto
            }
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
    }
}
