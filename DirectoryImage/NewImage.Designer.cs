namespace DirectoryImage
{
    partial class NewImage
    {
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewImage));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnFolderDialog = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCancel = new Glass.GlassButton();
            this.btnMake = new Glass.GlassButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(216, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // btnFolderDialog
            // 
            this.btnFolderDialog.Location = new System.Drawing.Point(234, 33);
            this.btnFolderDialog.Name = "btnFolderDialog";
            this.btnFolderDialog.Size = new System.Drawing.Size(25, 25);
            this.btnFolderDialog.TabIndex = 1;
            this.btnFolderDialog.Text = "...";
            this.btnFolderDialog.UseVisualStyleBackColor = true;
            this.btnFolderDialog.Click += new System.EventHandler(this.btnFolderDialog_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 71);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(283, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "No incluir Tumbs.db, archivos de tamaño 0 y *.ini, *.inf.";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // fbd
            // 
            this.fbd.ShowNewFolderButton = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.InnerBorderColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(148, 178);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnMake
            // 
            this.btnMake.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnMake.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMake.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMake.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnMake.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMake.InnerBorderColor = System.Drawing.Color.Red;
            this.btnMake.Location = new System.Drawing.Point(12, 178);
            this.btnMake.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnMake.Name = "btnMake";
            this.btnMake.Size = new System.Drawing.Size(80, 25);
            this.btnMake.TabIndex = 5;
            this.btnMake.Text = "Make";
            this.btnMake.Click += new System.EventHandler(this.btnMake_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DirectoryImage.Properties.Resources.About_beta;
            this.pictureBox1.Location = new System.Drawing.Point(301, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 191);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Folder to Scan:";
            // 
            // NewImage
            // 
            this.AcceptButton = this.btnMake;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(468, 215);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnMake);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnFolderDialog);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewImage";
            this.Load += new System.EventHandler(this.NewImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnFolderDialog;
        private System.Windows.Forms.CheckBox checkBox1;
        private Glass.GlassButton btnMake;
        private Glass.GlassButton btnCancel;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
    }
}