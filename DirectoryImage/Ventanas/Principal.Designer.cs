namespace DirectoryImage
{
    partial class frmPrincipal
    {
        
        public frmPrincipal():this(null){}
        
        public frmPrincipal(string[] args)
        {
            InitializeComponent();
            this.Text = "DirectoryImage v" + Atributos.Version;
            if(args != null)
                OpenDocument(args);            
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.searchThread1 = new System.ComponentModel.BackgroundWorker();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCloseFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.bckBusqueda = new System.ComponentModel.BackgroundWorker();
            this.bckOpen = new System.ComponentModel.BackgroundWorker();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnMake = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSearchShow = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblEmail = new System.Windows.Forms.ToolStripStatusLabel();
            this.TabCtrl = new System.Windows.Forms.TabControl();
            this.cmsMenu.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "close.bmp");
            this.imgList.Images.SetKeyName(1, "open.bmp");
            this.imgList.Images.SetKeyName(2, "file.bmp");
            this.imgList.Images.SetKeyName(3, ".xls");
            this.imgList.Images.SetKeyName(4, ".avi");
            this.imgList.Images.SetKeyName(5, ".doc");
            this.imgList.Images.SetKeyName(6, ".exe");
            this.imgList.Images.SetKeyName(7, ".pdf");
            this.imgList.Images.SetKeyName(8, ".pps");
            this.imgList.Images.SetKeyName(9, ".rar");
            this.imgList.Images.SetKeyName(10, ".gif");
            this.imgList.Images.SetKeyName(11, ".jpg");
            this.imgList.Images.SetKeyName(12, ".txt");
            this.imgList.Images.SetKeyName(13, ".html");
            this.imgList.Images.SetKeyName(14, ".mp3");
            this.imgList.Images.SetKeyName(15, ".bmp");
            this.imgList.Images.SetKeyName(16, ".dll");
            this.imgList.Images.SetKeyName(17, ".swf");
            this.imgList.Images.SetKeyName(18, ".pptx");
            this.imgList.Images.SetKeyName(19, ".docx");
            this.imgList.Images.SetKeyName(20, ".bmp");
            this.imgList.Images.SetKeyName(21, "Empty.ico");
            this.imgList.Images.SetKeyName(22, "Full.ico");
            this.imgList.Images.SetKeyName(23, ".ini");
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDelete,
            this.tsmiInsert,
            this.tsmiProperties});
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.Size = new System.Drawing.Size(133, 70);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(132, 22);
            this.tsmiDelete.Text = "Delete";
            this.tsmiDelete.Visible = false;
            // 
            // tsmiInsert
            // 
            this.tsmiInsert.Name = "tsmiInsert";
            this.tsmiInsert.Size = new System.Drawing.Size(132, 22);
            this.tsmiInsert.Text = "Add Image";
            this.tsmiInsert.Visible = false;
            // 
            // tsmiProperties
            // 
            this.tsmiProperties.Name = "tsmiProperties";
            this.tsmiProperties.Size = new System.Drawing.Size(132, 22);
            this.tsmiProperties.Text = "Properties";
            this.tsmiProperties.Visible = false;
            // 
            // fbd
            // 
            this.fbd.ShowNewFolderButton = false;
            // 
            // searchThread1
            // 
            this.searchThread1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.searchThread1_DoWork);
            this.searchThread1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.searchThread1_RunWorkerCompleted);
            // 
            // ToolTip
            // 
            this.ToolTip.AutoPopDelay = 5000;
            this.ToolTip.InitialDelay = 200;
            this.ToolTip.ReshowDelay = 100;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(4, 441);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 8;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.helpToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(602, 24);
            this.mnuMain.TabIndex = 10;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSave,
            this.mnuSaveAs,
            this.mnuCloseFolder,
            this.toolStripMenuItem2,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuNew
            // 
            this.mnuNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAlbum,
            this.mnuImage});
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(158, 22);
            this.mnuNew.Text = "New";
            // 
            // mnuAlbum
            // 
            this.mnuAlbum.Name = "mnuAlbum";
            this.mnuAlbum.Size = new System.Drawing.Size(162, 22);
            this.mnuAlbum.Text = "Album ...";
            this.mnuAlbum.Click += new System.EventHandler(this.newAlbum_Click);
            // 
            // mnuImage
            // 
            this.mnuImage.Enabled = false;
            this.mnuImage.Name = "mnuImage";
            this.mnuImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuImage.Size = new System.Drawing.Size(162, 22);
            this.mnuImage.Text = "Image ...";
            this.mnuImage.Click += new System.EventHandler(this.newImage_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuOpen.Size = new System.Drawing.Size(158, 22);
            this.mnuOpen.Text = "Open ...";
            this.mnuOpen.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Enabled = false;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSave.Size = new System.Drawing.Size(158, 22);
            this.mnuSave.Text = "Save";
            this.mnuSave.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Enabled = false;
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(158, 22);
            this.mnuSaveAs.Text = "Save as ...";
            this.mnuSaveAs.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // mnuCloseFolder
            // 
            this.mnuCloseFolder.Enabled = false;
            this.mnuCloseFolder.Name = "mnuCloseFolder";
            this.mnuCloseFolder.Size = new System.Drawing.Size(158, 22);
            this.mnuCloseFolder.Text = "Close Album";
            this.mnuCloseFolder.Click += new System.EventHandler(this.mnuCloseFolder_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(155, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(158, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(119, 22);
            this.mnuAbout.Text = "About ...";
            this.mnuAbout.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
            // 
            // bckOpen
            // 
            this.bckOpen.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckOpen_DoWork);
            this.bckOpen.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bckOpen_RunWorkerCompleted);
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "tzip";
            this.sfd.Filter = "ToleZip files|*.tzip|ToleXML files|*.txml";
            this.sfd.InitialDirectory = global::DirectoryImage.Properties.Settings.Default._workPath;
            // 
            // ofd
            // 
            this.ofd.AddExtension = false;
            this.ofd.DefaultExt = "txml";
            this.ofd.Filter = "ToleZip files|*.tzip|ToleXML files|*.txml";
            this.ofd.InitialDirectory = global::DirectoryImage.Properties.Settings.Default._workPath;
            this.ofd.RestoreDirectory = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMake,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.btnSearchShow});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(602, 25);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnMake
            // 
            this.btnMake.Image = global::DirectoryImage.Properties.Resources.Empty;
            this.btnMake.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMake.Name = "btnMake";
            this.btnMake.Size = new System.Drawing.Size(90, 22);
            this.btnMake.Text = "New Album";
            this.btnMake.Click += new System.EventHandler(this.newAlbum_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::DirectoryImage.Properties.Resources.Full;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(87, 22);
            this.toolStripButton2.Text = "New Image";
            this.toolStripButton2.Click += new System.EventHandler(this.newImage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSearchShow
            // 
            this.btnSearchShow.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchShow.Image")));
            this.btnSearchShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearchShow.Name = "btnSearchShow";
            this.btnSearchShow.Size = new System.Drawing.Size(62, 22);
            this.btnSearchShow.Text = "Search";
            this.btnSearchShow.Click += new System.EventHandler(this.btnSearchShow_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblEmail});
            this.statusStrip1.Location = new System.Drawing.Point(0, 434);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(602, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblEmail
            // 
            this.lblEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEmail.IsLink = true;
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(134, 17);
            this.lblEmail.Text = "krlostoledo@gmail.com";
            this.lblEmail.ToolTipText = "Programmer e-mail";
            this.lblEmail.Click += new System.EventHandler(this.lblEmail_Click);
            // 
            // TabCtrl
            // 
            this.TabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TabCtrl.HotTrack = true;
            this.TabCtrl.Location = new System.Drawing.Point(0, 52);
            this.TabCtrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.TabCtrl.Name = "TabCtrl";
            this.TabCtrl.SelectedIndex = 0;
            this.TabCtrl.Size = new System.Drawing.Size(602, 379);
            this.TabCtrl.TabIndex = 6;
            // 
            // frmPrincipal
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 456);
            this.Controls.Add(this.TabCtrl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.mnuMain);
            this.Controls.Add(this.lblStatus);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DirectoryImage.Properties.Settings.Default, "_Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = global::DirectoryImage.Properties.Settings.Default._Name;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmPrincipal_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosing_1);
            this.cmsMenu.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog sfd;
        public System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.ComponentModel.BackgroundWorker searchThread1;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiInsert;
        private System.Windows.Forms.ToolStripMenuItem tsmiProperties;
        private System.Windows.Forms.Label lblStatus;
        //private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuAlbum;
        private System.Windows.Forms.ToolStripMenuItem mnuImage;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.ComponentModel.BackgroundWorker bckBusqueda;
        private System.ComponentModel.BackgroundWorker bckOpen;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnMake;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblEmail;
        private System.Windows.Forms.TabControl TabCtrl;
        private System.Windows.Forms.ToolStripButton btnSearchShow;
    }
}

