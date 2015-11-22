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
                foreach (string path in args)
                    OpenDocument(path);
            
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
            this.TabCtrl = new System.Windows.Forms.TabControl();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txbSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new Glass.GlassButton();
            this.lvSearch = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.panel = new System.Windows.Forms.Panel();
            this.circleProgress = new MRG.Controls.UI.LoadingCircle();
            this.glassButton1 = new Glass.GlassButton();
            this.btnMake = new Glass.GlassButton();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.searchThread1 = new System.ComponentModel.BackgroundWorker();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
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
            this.TabCtrl.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.cmsMenu.SuspendLayout();
            this.panel.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabCtrl
            // 
            this.TabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TabCtrl.Controls.Add(this.tabSearch);
            this.TabCtrl.HotTrack = true;
            this.TabCtrl.Location = new System.Drawing.Point(0, 57);
            this.TabCtrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.TabCtrl.Name = "TabCtrl";
            this.TabCtrl.SelectedIndex = 0;
            this.TabCtrl.Size = new System.Drawing.Size(601, 374);
            this.TabCtrl.TabIndex = 5;
            this.TabCtrl.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.TabCtrl_ControlAdded);
            this.TabCtrl.SelectedIndexChanged += new System.EventHandler(this.TabCtrl_SelectedIndexChanged);
            this.TabCtrl.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.TabCtrl_ControlRemoved);
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.label1);
            this.tabSearch.Controls.Add(this.txbSearch);
            this.tabSearch.Controls.Add(this.btnSearch);
            this.tabSearch.Controls.Add(this.lvSearch);
            this.tabSearch.Location = new System.Drawing.Point(4, 22);
            this.tabSearch.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Size = new System.Drawing.Size(593, 348);
            this.tabSearch.TabIndex = 4;
            this.tabSearch.Text = "Search";
            this.tabSearch.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "All or part of the file name:";
            // 
            // txbSearch
            // 
            this.txbSearch.Location = new System.Drawing.Point(9, 34);
            this.txbSearch.Name = "txbSearch";
            this.txbSearch.Size = new System.Drawing.Size(146, 20);
            this.txbSearch.TabIndex = 10;
            this.ToolTip.SetToolTip(this.txbSearch, "Ej: \".avi\" para localizar todos los .avi");
            this.txbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txbSearch_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSearch.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnSearch.InnerBorderColor = System.Drawing.Color.Red;
            this.btnSearch.Location = new System.Drawing.Point(10, 66);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(97, 22);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lvSearch
            // 
            this.lvSearch.AllowColumnReorder = true;
            this.lvSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvSearch.LargeImageList = this.imgList;
            this.lvSearch.Location = new System.Drawing.Point(172, 3);
            this.lvSearch.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.lvSearch.MultiSelect = false;
            this.lvSearch.Name = "lvSearch";
            this.lvSearch.Size = new System.Drawing.Size(419, 342);
            this.lvSearch.SmallImageList = this.imgList;
            this.lvSearch.TabIndex = 6;
            this.lvSearch.UseCompatibleStateImageBehavior = false;
            this.lvSearch.View = System.Windows.Forms.View.Details;
            this.lvSearch.DoubleClick += new System.EventHandler(this.lvSearch_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Full Path";
            this.columnHeader3.Width = 300;
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
            this.cmsMenu.Size = new System.Drawing.Size(138, 70);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(137, 22);
            this.tsmiDelete.Text = "Delete";
            this.tsmiDelete.Visible = false;
            // 
            // tsmiInsert
            // 
            this.tsmiInsert.Name = "tsmiInsert";
            this.tsmiInsert.Size = new System.Drawing.Size(137, 22);
            this.tsmiInsert.Text = "Add Image";
            this.tsmiInsert.Visible = false;
            // 
            // tsmiProperties
            // 
            this.tsmiProperties.Name = "tsmiProperties";
            this.tsmiProperties.Size = new System.Drawing.Size(137, 22);
            this.tsmiProperties.Text = "Properties";
            this.tsmiProperties.Visible = false;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.BackgroundImage = global::DirectoryImage.Properties.Resources.header;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.circleProgress);
            this.panel.Controls.Add(this.glassButton1);
            this.panel.Controls.Add(this.btnMake);
            this.panel.Location = new System.Drawing.Point(-2, 26);
            this.panel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(566, 29);
            this.panel.TabIndex = 6;
            // 
            // circleProgress
            // 
            this.circleProgress.Active = false;
            this.circleProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.circleProgress.BackColor = System.Drawing.Color.Transparent;
            this.circleProgress.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.circleProgress.Color = System.Drawing.Color.LightSteelBlue;
            this.circleProgress.Enabled = false;
            this.circleProgress.InnerCircleRadius = 5;
            this.circleProgress.Location = new System.Drawing.Point(528, 1);
            this.circleProgress.Name = "circleProgress";
            this.circleProgress.NumberSpoke = 12;
            this.circleProgress.OuterCircleRadius = 11;
            this.circleProgress.RotationSpeed = 50;
            this.circleProgress.Size = new System.Drawing.Size(30, 29);
            this.circleProgress.SpokeThickness = 2;
            this.circleProgress.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.circleProgress.TabIndex = 6;
            // 
            // glassButton1
            // 
            this.glassButton1.BackColor = System.Drawing.Color.RoyalBlue;
            this.glassButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.glassButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.glassButton1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.glassButton1.Image = global::DirectoryImage.Properties.Resources.Empty;
            this.glassButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.glassButton1.InnerBorderColor = System.Drawing.Color.Red;
            this.glassButton1.Location = new System.Drawing.Point(4, 1);
            this.glassButton1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.glassButton1.Name = "glassButton1";
            this.glassButton1.Size = new System.Drawing.Size(110, 25);
            this.glassButton1.TabIndex = 4;
            this.glassButton1.Text = "New Album";
            this.glassButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ToolTip.SetToolTip(this.glassButton1, "Create a new Album.");
            this.glassButton1.Click += new System.EventHandler(this.newAlbum_Click);
            // 
            // btnMake
            // 
            this.btnMake.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnMake.Enabled = false;
            this.btnMake.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMake.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnMake.Image = global::DirectoryImage.Properties.Resources.Full;
            this.btnMake.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMake.InnerBorderColor = System.Drawing.Color.Red;
            this.btnMake.Location = new System.Drawing.Point(120, 1);
            this.btnMake.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnMake.Name = "btnMake";
            this.btnMake.Size = new System.Drawing.Size(110, 25);
            this.btnMake.TabIndex = 0;
            this.btnMake.Text = "New Image";
            this.btnMake.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ToolTip.SetToolTip(this.btnMake, "Insert a new Image in the actual Album.");
            this.btnMake.Click += new System.EventHandler(this.btnMake_Click);
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
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::DirectoryImage.Properties.Resources.Do_Not;
            this.btnClose.Enabled = false;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Corbel", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(567, 26);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 29);
            this.btnClose.TabIndex = 12;
            this.ToolTip.SetToolTip(this.btnClose, "Cierra el Album Seleccionado");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.mnuCloseFolder_Click);
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
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DirectoryImage.Properties.Settings.Default, "_emailAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(465, 437);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(137, 15);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = global::DirectoryImage.Properties.Settings.Default._emailAddress;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
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
            this.mnuFile.Size = new System.Drawing.Size(35, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuNew
            // 
            this.mnuNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAlbum,
            this.mnuImage});
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(166, 22);
            this.mnuNew.Text = "New";
            // 
            // mnuAlbum
            // 
            this.mnuAlbum.Name = "mnuAlbum";
            this.mnuAlbum.Size = new System.Drawing.Size(169, 22);
            this.mnuAlbum.Text = "Album ...";
            this.mnuAlbum.Click += new System.EventHandler(this.newAlbum_Click);
            // 
            // mnuImage
            // 
            this.mnuImage.Enabled = false;
            this.mnuImage.Name = "mnuImage";
            this.mnuImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuImage.Size = new System.Drawing.Size(169, 22);
            this.mnuImage.Text = "Image ...";
            this.mnuImage.Click += new System.EventHandler(this.newImage_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuOpen.Size = new System.Drawing.Size(166, 22);
            this.mnuOpen.Text = "Open ...";
            this.mnuOpen.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Enabled = false;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSave.Size = new System.Drawing.Size(166, 22);
            this.mnuSave.Text = "Save";
            this.mnuSave.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Enabled = false;
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(166, 22);
            this.mnuSaveAs.Text = "Save as ...";
            this.mnuSaveAs.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // mnuCloseFolder
            // 
            this.mnuCloseFolder.Enabled = false;
            this.mnuCloseFolder.Name = "mnuCloseFolder";
            this.mnuCloseFolder.Size = new System.Drawing.Size(166, 22);
            this.mnuCloseFolder.Text = "Close Album";
            this.mnuCloseFolder.Click += new System.EventHandler(this.mnuCloseFolder_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(163, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(166, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(129, 22);
            this.mnuAbout.Text = "About ...";
            this.mnuAbout.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
            // 
            // frmPrincipal
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 456);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.mnuMain);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.TabCtrl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmPrincipal_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosing_1);
            this.TabCtrl.ResumeLayout(false);
            this.tabSearch.ResumeLayout(false);
            this.tabSearch.PerformLayout();
            this.cmsMenu.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabCtrl;
        private System.Windows.Forms.Panel panel;
        private Glass.GlassButton btnMake;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.ComponentModel.BackgroundWorker searchThread1;
        private Glass.GlassButton glassButton1;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiInsert;
        private System.Windows.Forms.ToolStripMenuItem tsmiProperties;
        private System.Windows.Forms.Label lblStatus;
        //private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private MRG.Controls.UI.LoadingCircle circleProgress;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TabPage tabSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbSearch;
        private Glass.GlassButton btnSearch;
        private System.Windows.Forms.ListView lvSearch;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
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
        private System.Windows.Forms.Button btnClose;
    }
}

