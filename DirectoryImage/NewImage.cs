using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DirectoryImage
{
    public partial class NewImage : Form
    {
        public NewImage()
        {
            InitializeComponent();
        }

        private void btnFolderDialog_Click(object sender, EventArgs e)
        {
            if(fbd.ShowDialog() == DialogResult.OK)
                comboBox1.Text = fbd.SelectedPath;            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string Path {
            get { return comboBox1.Text; }
        }
        private void btnMake_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(comboBox1.Text)){
                this.DialogResult = DialogResult.OK;
                if (!Properties.Settings.Default._MostUsedPaths.Contains(comboBox1.Text))
                {
                    Properties.Settings.Default._MostUsedPaths.Add(comboBox1.Text);
                    Properties.Settings.Default.Save();
                }
                this.Close();
            }
            else{
                MessageBox.Show("Not valid path: " + comboBox1.Text+"\nYou need to define a folder to scan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void NewImage_Load(object sender, EventArgs e)
        {
            foreach (string path in Properties.Settings.Default._MostUsedPaths) {
                comboBox1.Items.Add(path);
            }
        }

        
    }
}