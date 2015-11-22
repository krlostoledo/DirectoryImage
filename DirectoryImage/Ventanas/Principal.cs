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
using DirectoryImage.Ventanas;
//F-Folders, FI-File, N-Name, CT-CreationTime, S-Size
namespace DirectoryImage
{
    public partial class frmPrincipal: Form
    {      
        #region Variables Globales
        List<AlbumControl> album_ctrl_list = new List<AlbumControl>(10);
            //bool _documentChange = false;
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
            public dAlbum SelectedAlbum 
            {                
                get 
                {
                    //TODO: esta propiedad se ejecuta muchas veces. poner breakpoint
                    int index = TabCtrl.SelectedIndex;          //indice del tab seleccionado
                    if (index < 0 || index >= TabCtrl.TabPages.Count)
                        return null;
                    return SelectedAlbumControl.thisAlbum;
                }
            }

            public AlbumControl SelectedAlbumControl {
                get {
                    if(TabCtrl.SelectedIndex >= 0 && TabCtrl.SelectedIndex < album_ctrl_list.Count)
                        return album_ctrl_list[TabCtrl.SelectedIndex];
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
        void OpenDocument(string[] path)
        {
            if (!bckOpen.IsBusy)
                bckOpen.RunWorkerAsync(path);
            else
            {
                return;
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
            dImagen img;
            if (cont.full_path == "RED")
                img = new xmlImagen();
            else
                img = new xmlImagen(cont.full_path);
            cont.SelectedAlbum.InsertImagen(img);
        }
        private void searchThread1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            //FillTreeView();
            TreeNode node = SelectedAlbum[SelectedAlbum.Count - 1].Root;
            SelectedAlbumControl.treeView.Nodes[0].Nodes.Add(node);
            if (!SelectedAlbumControl.treeView.Nodes[0].IsExpanded)
                SelectedAlbumControl.treeView.Nodes[0].Expand();
            TabCtrl.Enabled = true;
            try
            {
                SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
                simpleSound.Play();
            }
            catch { }
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
              
        
        private void frmPrincipal_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
                SubirNivel();
        }
        #endregion

       
        #region DragDrop
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] links = e.Data.GetData(DataFormats.FileDrop) as string[];
            OpenDocument(links);                        
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        }
        #endregion

        public string Extension(string extension)
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

        /// <summary>
        /// Dado el camino completo busca el album donde esta y obliga a mostrar el archivo.
        /// </summary>
        /// <param name="path">Camino del archivo o carpeta.</param>
        public void ShowItemFromSearch(string[] path)
        {
            foreach (AlbumControl item in album_ctrl_list)
            {
                if (item.aName == path[0])
                {
                    item.ShowItemFromSearch(path);
                    break;
                }
            }
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
        bool InsertarNewAlbum(dAlbum new_album){
            TabCtrl.TabPages.Add(new_album.Name, new_album.Name);
            int index_last = TabCtrl.TabPages.Count - 1;

            AlbumControl alControl = new AlbumControl(this, new_album);
            alControl.treeView.ImageList = this.imgList;
            //alControl.listView.LargeImageList = this.imgList;
            alControl.listView.SmallImageList = this.imgList;
            alControl.Parent = TabCtrl.TabPages[index_last];
            alControl.Dock = DockStyle.Fill;
            album_ctrl_list.Add(alControl);
            TabCtrl.SelectedIndex = index_last;
            
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
                TabCtrl.Enabled = false;
                searchThread1.RunWorkerAsync( new Container(SelectedAlbum, img.Path));
                ChangeIsDone = true;
            }
        }

        /// <summary>
        /// Abre un folder del disco duro
        /// </summary>
        void OpenFolder() {
            ofd.InitialDirectory = Properties.Settings.Default._workPath;
            //if (!CloseDocument())
            //    return;
            if (DialogResult.OK != ofd.ShowDialog())
                return;
            OpenDocument(new string[]{ofd.FileName});
            
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
                catch {
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
                if (dr == DialogResult.Yes)
                {                           //quiere guardar los cambios
                    saveAsToolStripMenuItem_Click(null, EventArgs.Empty);
                    if (SelectedAlbum.Need_Change)                      //no se guardó                 
                        return false;
                }
                else if (dr == DialogResult.Cancel)                     //no quiere cerrar el album
                    return false;                
            }
            
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
            for (int k = 0; k < TabCtrl.TabPages.Count; k++)  //desde el primero hasta el último.
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


        private void frmPrincipal_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if(!CloseApplication())
                e.Cancel = true;
        }  

        #region TabControl Events
        private void TabCtrl_ControlAdded(object sender, ControlEventArgs e)
        {
            mnuSave.Enabled = true;
            mnuSaveAs.Enabled = true;
        }
        private void TabCtrl_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (TabCtrl.TabCount == 1) {        //Si nos quedamos en solo un tab.
                mnuSave.Enabled = false;
                mnuSaveAs.Enabled = false;
                
            }
        }
        private void TabCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabCtrl.SelectedIndex < album_ctrl_list.Count || TabCtrl.SelectedIndex == 0)
            {
               
            }            
        }   
        #endregion

       

        #region bckworker Open
        private void bckOpen_DoWork(object sender, DoWorkEventArgs e)
        {
           Tools.Abridor.Clear();
            foreach (string path in (string[])e.Argument)
            {
                Tools.Abridor abridor = Tools.Abridor.GetAbridor(path);
            }
        }

        private void bckOpen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var abri = Tools.Abridor.GetAbridor();
            foreach (dAlbum album in abri.GetAlbum)
            {
                InsertarNewAlbum(album);
            }            
            ChangePath = abri.GetAlbum[0].Full_Path;
        } 
        #endregion
       
        void SubirNivel()
        {
            TreeNode node = SelectedAlbumControl.treeView.SelectedNode;
            if (node != null && node.Parent != null)
                SelectedAlbumControl.treeView.SelectedNode = node.Parent;
        }

        

        private void lblEmail_Click(object sender, EventArgs e)
        {
            //--> Esta es la linea para que te abra un correo hacia la direccion que especifiques
            System.Diagnostics.Process.Start(string.Format("mailto:{0}", lblEmail.Text));
        }

        private void btnSearchShow_Click(object sender, EventArgs e)
        {
            Buscar b = new Buscar(this, album_ctrl_list);
            b.Show();
        }                   
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