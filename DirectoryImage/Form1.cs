using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using DirectoryImage;
using DirectoryImage.Properties;
using Glass;
using System.Net.Mail;
using System.Reflection;
using System.Collections.Generic;
//F-Folders, FI-File, N-Name, CT-CreationTime, S-Size
namespace DirectoryImage
{
    public partial class frmPrincipal: Form
    {      
        #region Variables Globales
        List<AlbumControl> album_ctrl_list = new List<AlbumControl>(10);
            bool _documentChange = false;
            string _albumPath = "";
            
        #endregion

        #region PropiedadesGlobales
            public string ChangePath {
                set {
                    _albumPath = value;
                    if (!File.Exists(_albumPath))
                        mnuSave.Enabled = false;
                    else
                        mnuSave.Enabled = true;
                }
                get {
                    return _albumPath;
                }
            }
            public bool ChangeIsDone {
                get {
                    return SelectedAlbumControl.ChangeIsDone;
                }
                set {
                    if (value) {
                        mnuSave.Enabled = true;
                        mnuSaveAs.Enabled = true;
                        SelectedAlbumControl.ChangeIsDone = true;
                    }
                    else {
                        //mnuSave.Enabled = false;
                        SelectedAlbumControl.ChangeIsDone = false;
                    }              
                }
            }
            /// <summary>
            /// Retorna el Album seleccionado, null si no hay ninguno.
            /// </summary>
            public dAlbum SelectedAlbum {
                //set {
                //    if (value == null) {
                //        mnuSave.Enabled = false;        //no se puede salvar
                //        mnuSaveAs.Enabled = false;
                //        mnuCloseFolder.Enabled = false; //no se puede cerrar
                //        this._album = null;             //no hay album
                //        treeView.Nodes.Clear();         //todo lo limpio
                //        listView.Items.Clear();         
                //        mnuImage.Enabled = false;       //No puedo insertar nuevas im'agenes
                //        btnMake.Enabled = false;
                //        return;
                //    }
                //    mnuSave.Enabled = true;             //puedo guardarlo
                //    mnuSaveAs.Enabled = true;           
                //    mnuCloseFolder.Enabled = true;      //puedo cerrarlo
                //    this._album = value;
                //    mnuImage.Enabled = true;
                //    btnMake.Enabled = true;
                //}
                get {
                    int index = TabCtrl.SelectedIndex;          //indice del tab seleccionado
                    if (index < 0 || index >= TabCtrl.TabPages.Count - 1)    //no puede ser el último
                        return null;
                    return SelectedAlbumControl.thisAlbum;
                }
            }

            int selected_index = -1;
            public AlbumControl SelectedAlbumControl {
                get {
                    if(selected_index >= 0 && selected_index < album_ctrl_list.Count)
                        return album_ctrl_list[selected_index];
                    else
                        return null;               
                }
            }
            
        #endregion

        #region Metodos para cargar imagenes
        /// <summary>
        /// Carga un Album. 
        /// </summary>
        /// <param name="path"> Camino absoluto del archivo a abrir.</param>
        void OpenDocument(string path)
        {
            try{
                string ext = Path.GetExtension(path).ToLower();
                if (ext == ".tzip")                    
                    InsertarNewAlbum( dAlbum.LoadAlbumFromTzip(path));
                else if (ext == ".txml")
                    InsertarNewAlbum( dAlbum.LoadAlbumFromTxml(path));
                
                if (SelectedAlbum == null)
                    return;
                ActualizeDocument();
                ChangePath = path;
            }
            catch{
                MessageBox.Show("No fue posible abrir el documento: " + path, "Error al cargar archivo.");
            }
        }
            #endregion

        #region Metodos TreeView
        /// <summary>
        /// Actualiza el TreeView a partir del innerXML
        /// </summary>
        void FillTreeView()
        {
            if (SelectedAlbum == null || !SelectedAlbum.HasImages)
                return;
            SelectedAlbumControl.treeView.Nodes.Clear();
            SelectedAlbumControl.treeView.Nodes.Add(SelectedAlbum.Root);
            SelectedAlbumControl.treeView.SelectedNode = SelectedAlbumControl.treeView.Nodes[0];
        }
        void LoadTreeView(XmlNode xmlParent, TreeNode node)
        {
            if (!xmlParent.HasChildNodes)
                return;
            SelectedAlbumControl.treeView.BeginUpdate();
            foreach (XmlNode xmlnode in xmlParent.ChildNodes)
            {
                if (xmlnode.Name == Atributos.Folder)
                {
                    TreeNode tmpNode = new TreeNode(xmlnode.Attributes[Atributos.Name].Value, 0, 1);
                    LoadTreeView(xmlnode, tmpNode);
                    node.Nodes.Add(tmpNode);
                }
                else if (xmlnode.Name == Atributos.File)
                {

                }
            }
            SelectedAlbumControl.treeView.EndUpdate();

        }
        #endregion

        #region BackgroundWorker
        void searchThread1_DoWork(object sender, DoWorkEventArgs e)
        {
            Container cont = (Container)e.Argument;
            xmlImagen img = new xmlImagen(cont.SelectedAlbum, cont.full_path);
            //if(SelectedAlbumControl.thisAlbum != null)
            //    SelectedAlbumControl.thisAlbum.InsertImagen(img);
        }
        private void searchThread1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
                simpleSound.Play();
            }
            catch { }
            //FillTreeView();
            TreeNode node = SelectedAlbum[SelectedAlbum.Count - 1].Root;
            SelectedAlbumControl.treeView.Nodes[0].Nodes.Add(node);
            if (!SelectedAlbumControl.treeView.Nodes[0].IsExpanded)
                SelectedAlbumControl.treeView.Nodes[0].Expand();
            DeactivateProgress();
            TabCtrl.Enabled = true;
        }
        #endregion

        /// <summary>
        /// Se requiere haber seleccionado en fbd el path
        /// </summary>
        void ActualizeDocument()
        {
            //txbResp.Text = XmlFormat(innerXML.InnerXml);
            FillTreeView();
            //treeView.SelectedNode = treeView.Nodes[0]; //para llenar el list automat.

        }

        #region MainMenu
        private void newAlbum_Click(object sender, EventArgs e) {
            NewFolder();
        }
        private void newImage_Click(object sender, EventArgs e) {
            NewImage();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFolder();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFolder();
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFolderAs();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            if (CloseApplication())
                Close();
        }
        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();            
        }
        #endregion

        #region Eventos
        private void btnMake_Click(object sender, EventArgs e)
        {
            NewImage();
        }
        
        private void lbxSearch_MouseDoubleClick(object sender, MouseEventArgs e) {
            //ShowNode(lbxSearch.SelectedItem.ToString()) ;
        }
        private void mnuCloseFolder_Click(object sender, EventArgs e)
        {
            CloseDocument();
        }
        #region TreeView
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
                TreeNode node = SelectedAlbumControl.treeView.SelectedNode;
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
            node.FillListView(SelectedAlbumControl.listView);
            ShowStatus();
        }
        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SelectedAlbumControl.treeView.SelectedNode = e.Node;

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
                cmsMenu.Show(SelectedAlbumControl.treeView, e.X + 10, e.Y + 5);
            }
        }       
        #endregion
        
        
        /// <summary>
        /// Enviar Correo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            //--> Esta es la linea para que te abra un correo hacia la direccion que especifiques
            System.Diagnostics.Process.Start(string.Format("mailto:{0}",linkLabel1.Text));
        }
        private void frmPrincipal_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
                SubirNivel();
        }
        #endregion

        #region SearchMethods
        
        
        /// <summary>
        /// Encuentra nodo dado un camino completo
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        XmlNode FindXmlNode(string[] path)
        {
            //XmlNode xmlResp = Album.ChildNodes[0];
            ////if(xmlResp.Attributes["Name"].Value != path[0]) //hizo mal la busqueda
            ////    throw new Exception() ;
            //for(int k = 0; k < path.Length; k++)
            //{

            //    foreach(XmlNode xmlnode in xmlResp.ChildNodes)
            //    {
            //        if(xmlnode.Name == Atributos.Folder)
            //        {
            //            if(xmlnode.Attributes[Atributos.Name].Value == path[k])
            //            { //encontre
            //                xmlResp = xmlnode;
            //                break;
            //            }
            //        }
            //    }
            //}
            //return xmlResp;
            return null;
        }
        #endregion

        #region DragDrop
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] links = e.Data.GetData(DataFormats.FileDrop) as string[];
            foreach (string path in links)
            {
                OpenDocument(path);
            }            
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        }
        #endregion

        string Extension(string extension)
        {
            switch(extension.ToLower())
            {
                case ".htm": return ".html";
                case ".mpeg": return ".avi";
                case ".mpg": return ".avi";
                case ".png": return ".gif";
                case ".uga": return ".gif";
                case ".rm": return ".avi";
                case ".mov": return ".avi";
                case ".zip": return ".rar";
                case ".mht": return ".html";
                case ".ppsx": return ".pptx";
                case ".wmv": return ".avi" ; 
                default: return "";
            }
        }
        
        #region listViewMenuLogic
        private void listviewMenu_Opening(object sender, CancelEventArgs e)
        {
            
        }
        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //detailsToolStripMenuItem.Checked = true;
            //listToolStripMenuItem.Checked = false;
            //tilesToolStripMenuItem.Checked = false;
            //listView.View = View.Details;
        }
        private void tilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //detailsToolStripMenuItem.Checked = false;
            //listToolStripMenuItem.Checked = false;
            //tilesToolStripMenuItem.Checked = true;
            //listView.View = View.Tile;
        }
        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //detailsToolStripMenuItem.Checked = false;
            //listToolStripMenuItem.Checked = true;
            //tilesToolStripMenuItem.Checked = false;
            //listView.View = View.List;
        }

        #endregion

        //TODO: Arreglar esta mierda de metodo
        /// <summary>
        /// Busca la informacion de node en el xml y lo pone en el listview
        /// </summary>
        /// <param name="node"></param>
        void ShowItemsInListView(TreeNode node)
        {
            string nodePath = node.FullPath;
            string[] nodePath2 = nodePath.Split(new Char[] { '\\' });
            XmlNode xmlNode = FindXmlNode(nodePath2);

            //listView.Clear() ;                      //revisar
            SelectedAlbumControl.listView.Items.Clear();
            SelectedAlbumControl.listView.BeginUpdate();

            #region Insertar Folder ..
            ListViewItem lv = new ListViewItem("..", 0);
            SelectedAlbumControl.listView.Items.Add(lv);
            #endregion

            foreach (XmlNode xmlnode in xmlNode.ChildNodes)
            {
                lv = new ListViewItem(xmlnode.Attributes[Atributos.Name].Value, 0);
                if (xmlnode.Name == Atributos.Folder)
                    lv.ImageIndex = 0;
                else
                {
                    string key = Path.GetExtension(xmlnode.Attributes[Atributos.Name].Value);
                    lv.ImageIndex = imgList.Images.IndexOfKey(key);
                    if (lv.ImageIndex == -1)
                        key = this.Extension(key);
                    lv.ImageIndex = imgList.Images.IndexOfKey(key);
                    if (lv.ImageIndex == -1)
                        lv.ImageIndex = 2;
                }

                long? size = long.Parse(xmlnode.Attributes[Atributos.Size].Value);
                long rest = 0;
                #region sizeFormat
                if (size.HasValue)
                {
                    size /= 1024;
                    rest = size.Value % 1024;
                    size /= 1024;

                }
                #endregion
                lv.SubItems.Add(((size > 0) ? size.ToString() + ',' : "") + rest.ToString() + " KB");
                lv.SubItems.Add(xmlnode.Attributes[Atributos.CreationTime].Value);

                SelectedAlbumControl.listView.Items.Add(lv);
            }
            SelectedAlbumControl.listView.EndUpdate();
        }
        void ShowTreeNode(TreeNode node) {

        }        
        private void txbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                btnSearch_Click(null, EventArgs.Empty ) ;

        }
        void ShowNode(string fullpath){
            string[] path = fullpath.Split('\\') ;
            TreeNode treeNodo = SelectedAlbumControl.treeView.TopNode;
            treeNodo.Expand();
            for(int k = 1; k < path.Length; k++){
                foreach(TreeNode nodo in treeNodo.Nodes){
                    if(nodo.Text == path[k]){
                        treeNodo = nodo ;
                        treeNodo.Expand();
                        break ;
                    }
                }
            }
            ShowItemsInListView(treeNodo) ;
            SelectedAlbumControl.treeView.SelectedNode = treeNodo;
            //listView.selected
            //ADD una forma de seleccionar el item seleccionado
            //TabCtrl.SelectedIndex = 0 ;
           
        }        
                  
        
        #region ACCIONES
        bool NewFolder(){
            if (searchThread1.IsBusy)
                return false;

            //if (ChangeIsDone == true) {
            //    DialogResult result = MessageBox.Show("El Album ha sido modificado. Desea guardar los cambios", "Album modificado", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) ;
            //    if (DialogResult.Yes == result && !SaveFolderAs())//quiere salvar pero no se salvo
            //        return false;
            //    else if (DialogResult.Cancel == result)
            //        return false;
            //}
            dAlbum album = dAlbum.Create();
            if (album == null)
                return false;           
            
            return InsertarNewAlbum(album) ;
        }
        /// <summary>
        /// Inserta un dAlbum en un nuevo TabPage y actualiza SelectedAlbum y album_ctrl_list.
        /// </summary>
        /// <param name="new_album">Album a insertar.</param>
        /// <returns></returns>
        bool InsertarNewAlbum(dAlbum new_album) {
            int index = TabCtrl.TabPages.Count - 1;
            TabCtrl.TabPages.Insert(index,
                new_album.Name, new_album.Name);

            AlbumControl alControl = new AlbumControl(this, new_album);
            alControl.treeView.ImageList = this.imgList;
            //alControl.listView.LargeImageList = this.imgList;
            alControl.listView.SmallImageList = this.imgList;
            alControl.Parent = TabCtrl.TabPages[index];
            alControl.Dock = DockStyle.Fill;
            album_ctrl_list.Add(alControl);
            TabCtrl.SelectedIndex = index;
            

            ShowStatus();

            btnMake.Enabled = true;
            mnuImage.Enabled = true;
            mnuCloseFolder.Enabled = true;
            return true;
        }
        /// <summary>
        /// Controla toda la lógica de creación de una nueva imagen.
        /// </summary>
        public void NewImage() {
            if (searchThread1.IsBusy)
                return;
            if (SelectedAlbum == null) {
                if (DialogResult.Yes == MessageBox.Show("You need an album where to add the new image.\nDo you want to make an album now?", "Album not found.", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    if (!NewFolder())                   
                        return;
                    else{
                        goto continueimage;
                    }
                else
                    return;
            }
        continueimage:
            DirectoryImage.NewImage img = new NewImage();
            DialogResult dialog = img.ShowDialog();
            if (dialog == DialogResult.OK) {
                ActivateProgress();
                TabCtrl.Enabled = false;
                searchThread1.RunWorkerAsync( new Container(SelectedAlbum,img.Path));
                ChangeIsDone = true;
            }
        }
        void OpenFolder() {
            ofd.InitialDirectory = Properties.Settings.Default._workPath;
            //if (!CloseDocument())
            //    return;
            if (DialogResult.OK != ofd.ShowDialog())
                return;
            OpenDocument(ofd.FileName);
            string newp = Path.GetDirectoryName(ofd.FileName);
            Properties.Settings.Default._workPath = newp;
        }
        bool SaveFolder() {
            if (ChangeIsDone) {
                if (SelectedAlbum == null)
                    return false;
                string full_path = SelectedAlbum.Full_Path;
                if (Path.GetExtension(full_path).ToLower() == ".tzip")
                    SelectedAlbum.SaveAlbumToTzip(full_path);
                else if (Path.GetExtension(full_path).ToLower() == ".txml")
                    SelectedAlbum.SaveAlbumToTxml(full_path);
                
                ChangeIsDone = false;
            }
            return true;
        }
        bool SaveFolderAs(){
            if (SelectedAlbum == null)
                return true;
            sfd.FileName = SelectedAlbum.Name;
            sfd.InitialDirectory = Properties.Settings.Default._workPath;
            if (DialogResult.OK == sfd.ShowDialog()) {
                try {
                    string ext = Path.GetExtension(sfd.FileName).ToLower() ;
                    if (ext == ".tzip")
                        SelectedAlbum.SaveAlbumToTzip(sfd.FileName);
                    else if (ext == ".txml")
                        SelectedAlbum.SaveAlbumToTxml(sfd.FileName);
                    string newp = Path.GetDirectoryName(sfd.FileName);
                    Properties.Settings.Default._workPath = newp;
                }
                catch(Exception ex) {
                    MessageBox.Show("No se pudo guardar correctamente");
                    return false;
                }
                ChangePath = sfd.FileName;
                ChangeIsDone = false;    //importante hacer esto
                return true;
            }
            return false;
        }       
        /// <summary>
        /// Cierra el Tab seleccionado y su correspondiente album.
        /// </summary>
        /// <returns></returns>
        bool CloseDocument(){
            if (SelectedAlbum != null && SelectedAlbum.Need_Change) { //Ask for save it
                DialogResult dr = MessageBox.Show("Do you want to save the changes in this document?",
                    "DirectoryImage.",
                    MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes) {                           //quiere guardar los cambios
                    saveAsToolStripMenuItem_Click(null, EventArgs.Empty);
                    if (SelectedAlbum.Need_Change)                      //no se guardó                 
                        return false;
                }
                else if (dr == DialogResult.Cancel)                     //no quiere cerrar el album
                    return false;
            }
            //ChangeIsDone = false;
            if (TabCtrl.TabCount == 1)
                return false;
            int index = TabCtrl.SelectedIndex;
            
            TabCtrl.TabPages.RemoveAt(index);
            this.album_ctrl_list.RemoveAt(index);

            //Actualizar el nombre de la ventana
            this.Text = "DirectoryImage v" + Properties.Settings.Default._diVersion;
            ShowStatus();
            return true;   //en caso de documento vacio o no importante
        }
        /// <summary>
        /// Determina si puede ser cerrada la aplicación,
        /// </summary>
        /// <returns></returns>
        bool CloseApplication() {
            for (int k = 0; k < TabCtrl.TabPages.Count - 1; k++)  //desde el primero hasta el penúltimo.
            {
                TabCtrl.SelectedTab = TabCtrl.TabPages[k];
                if (!CloseDocument())
                {
                    return false;
                }
            }
            Properties.Settings.Default.Save();
            return true;
        }
        void ShowStatus(){
            if (SelectedAlbumControl == null || SelectedAlbum == null) {                
                this.Text = "DirectoryImage v" + 
                    Assembly.GetExecutingAssembly().GetName().Version.ToString();        
                lblStatus.Text = "Escriba por cualquier sugerencia.";
                return;            
            }
            TreeNode selected = SelectedAlbumControl.treeView.SelectedNode;
            
            if (selected.Level == 0) { //es la raiz
                lblStatus.Text = SelectedAlbum.Name + ": " + SelectedAlbum.Count.ToString() + " images.";
            }
            else if (selected.Level == 1) { //es una imagen
                xmlImagen tmp_xmlImagen = SelectedAlbum[selected.Index] as xmlImagen;
                lblStatus.Text = string.Format("{0}: {1} folders, {2} files.", tmp_xmlImagen.Name, tmp_xmlImagen.Folders, tmp_xmlImagen.Files);
            }
            else {
                lblStatus.Text = "Escriba por cualquier sugerencia.";
            }
            
            this.Text = SelectedAlbum.Name ;
            this.Text = "DirectoryImage v" + 
                Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
        }
        #endregion

        #region AccionesPacotilla
        /// <summary>
        /// Activa el circleProgress en señal de un proceso iniciado.
        /// </summary>
        /// <returns></returns>
        void ActivateProgress() {
            circleProgress.Color = System.Drawing.Color.Blue;
            circleProgress.Active = true;
        }
        /// <summary>
        /// Desactiva el circleProgress en señal de un proceso terminado.
        /// </summary>
        /// <returns></returns>
        void DeactivateProgress()
        {
            circleProgress.Color = System.Drawing.Color.LightSteelBlue;
            circleProgress.Active = false;
        }
        
        void SubirNivel() {
            TreeNode node = SelectedAlbumControl.treeView.SelectedNode;
            if(node != null && node.Parent != null)
                SelectedAlbumControl.treeView.SelectedNode = node.Parent;        
        }
        #endregion

        private void lvSearch_DoubleClick(object sender, EventArgs e) {
            ListViewItem item = lvSearch.SelectedItems[0];
            string fullpath = item.SubItems[2].Text;
            int index = fullpath.IndexOf('\\');
            string path = fullpath.Substring(0, index);
            
            TreeNode node = xmlTreeNode.ShowNode(SelectedAlbumControl.treeView.Nodes[0], fullpath);
            if (node != null) {
                //foreach (TabPage tab in TabCtrl.TabPages)
                //{
                //    string value = tab.Name;
                //    value= tab.Text;
                //}
                TabPage tabPage = TabCtrl.TabPages[path];
                if (tabPage == null)
                    return;
                TabCtrl.SelectedTab = tabPage;

                (node as xmlTreeNode).FillListView(SelectedAlbumControl.listView);
                SelectedAlbumControl.treeView.SelectedNode = node;
                foreach (ListViewItem lvi in SelectedAlbumControl.listView.Items)
                {
                    if (lvi.Text.ToLower() == item.Text.ToLower()) {
                        lvi.Selected = true;
                        break;
                    }
                }
            }
        }

        private void frmPrincipal_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if(!CloseApplication())
                e.Cancel = true;
        }        
        private void frmPrincipal_Load(object sender, EventArgs e) {
            Text = "DirectoryImage v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string path = txbSearch.Text.ToLower();
            if (SelectedAlbumControl.thisAlbum == null || path == "")
                return;

            Regex pattern;
            try { pattern = new Regex(path); }
            catch
            {
                MessageBox.Show("Patrón de búsqueda no válido.");
                return;
            }
            //Si no tiene imagenes no hay busqueda.
            if (SelectedAlbumControl.thisAlbum.HasImages)
            {
                lvSearch.Items.Clear();
                lvSearch.BeginUpdate();
                SelectedAlbumControl.thisAlbum.Search(pattern, lvSearch);
                lvSearch.EndUpdate();
            }
        }

        #region TabControl Events
        private void TabCtrl_ControlAdded(object sender, ControlEventArgs e)
        {
            btnClose.Enabled = true;
            mnuSave.Enabled = true;
            mnuSaveAs.Enabled = true;
        }
        private void TabCtrl_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (TabCtrl.TabCount == 1) {        //Si nos quedamos en solo un tab.
                mnuSave.Enabled = false;
                mnuSaveAs.Enabled = false;
                if (btnClose.Enabled)
                    btnClose.Enabled = false;
            }
        }
        private void TabCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabCtrl.SelectedIndex < album_ctrl_list.Count || TabCtrl.SelectedIndex == 0)
            {
                selected_index = TabCtrl.SelectedIndex;
                if(TabCtrl.TabCount > 1)
                    btnClose.Enabled = true;
                else
                    btnClose.Enabled = false;
            }
            else if (TabCtrl.SelectedIndex == TabCtrl.TabPages.Count - 1)    //es el ultimo
            {
                btnClose.Enabled = false;
                txbSearch.Focus();
            }
            //if(TabCtrl.SelectedIndex == 1)
            //    txbSearch.Focus() ;

        }   
        #endregion

        
                
    }

    #region Atributos
    public static class Atributos
    {
        private static byte version = 1;
        public static string Version
        {
            get{
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }            
        }
        public static void ReadVersion(string line)
        {
            //line = line.Remove.Replace(' ','') ;
            string[] firstLine = line.Split('.');
            version = byte.Parse(firstLine[3]);

        }
        public static string WriteVersion()
        {
            return "DirectoryImage format.Version=.1.Author=.Krlo";
        }
        #region Atributos
        public static string Name
        {
            get
            {
                switch(version)
                {
                    case 0: return "Name";
                    case 1: return "N";
                    default: return "";
                }
            }
        }
        public static string Folder
        {
            get
            {
                switch(version)
                {
                    case 0: return "Folder";
                    case 1: return "F";
                    default: return "";
                }
            }
        }
        public static string File
        {
            get
            {
                switch(version)
                {
                    case 0: return "File";
                    case 1: return "Fi";
                    default: return "";
                }
            }
        }
        public static string Size
        {
            get
            {
                switch(version)
                {
                    case 0: return "Size";
                    case 1: return "S";
                    default: return "";
                }
            }
        }
        public static string CreationTime
        {
            get
            {
                switch(version)
                {
                    case 0: return "CreationTime";
                    case 1: return "CT";
                    default: return "";
                }
            }
        }
        public static string FullPath
        {
            get
            {
                switch(version)
                {
                    case 0: return "FullPath";
                    case 1: return "FP";
                    default: return "";
                }
            }
        }
        public static string FolderNum {
            get { return "c"; }
        }
        public static string FilesNum {
            get { return "a"; }
        }
        #endregion
    }
    #endregion
    internal class Container {
        public string full_path;
        public dAlbum SelectedAlbum;
        public Container(dAlbum SelectedAlbum, string fullpath){
            full_path = fullpath;
            this.SelectedAlbum = SelectedAlbum;
        }
    }
}