using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DirectoryImage {
    public partial class NewAlbum :Form {
        public NewAlbum() {
            InitializeComponent();
        }
        public string newName = "";
        
        private void btnAceptar_Click(object sender, EventArgs e) {
            newName = tbNombre.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}