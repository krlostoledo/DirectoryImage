using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DirectoryImage.Ventanas
{
    public partial class Buscar : Form
    {
        List<AlbumControl> album_list;
        frmPrincipal principal;
        Comparador _comparador;

        public Buscar(frmPrincipal principal, List<AlbumControl> album_list)
        {
            InitializeComponent();
            this.album_list = album_list;   //TODO: pasando el principal no nocesite la lista de album
            this.principal = principal;

            _comparador = new Comparador(lvSearch);
            lvSearch.ListViewItemSorter = _comparador;
            lvSearch.Sorting = SortOrder.None;
        }

       
        #region bckworker Busqueda
        private void bckBusqueda_DoWork(object sender, DoWorkEventArgs e)
        {
            string patron = txbSearch.Text.ToLower();
            if (album_list.Count == 0 || patron == "")
                return;

            //Lock();
            Regex pattern;
            try { pattern = new Regex(patron); }
            catch
            {
                MessageBox.Show("Patrón de búsqueda no válido.");
                return;
            }

            List<dAlbum> lista_album = new List<dAlbum>();
            foreach (AlbumControl album in album_list)
            {
                lista_album.Add(album.thisAlbum);
            }

            var busc = Tools.Buscador.GetBuscador(lista_album, pattern);

            if (busc != null)
                busc.Find();
        }

        private void bckBusqueda_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Tools.Buscador.isBusy)
                return;
            lvSearch.Items.Clear();
            lvSearch.BeginUpdate();
            var buscador = Tools.Buscador.GetBuscador();
            if (buscador != null)
                buscador.Fill(lvSearch);
            lvSearch.EndUpdate();
            //Unlock();
        }
        #endregion

        private void Buscar_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            bckBusqueda.RunWorkerAsync();
        }

        private void txbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch_Click(sender, EventArgs.Empty);
        }

        private void lvSearch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || lvSearch.SelectedIndices.Count < 1)
                return;
            string full_path = lvSearch.SelectedItems[0].SubItems[2].Text;
            string[] result = full_path.Split(new string[]{"\\"}, StringSplitOptions.RemoveEmptyEntries);
            principal.ShowItemFromSearch(result);
        }

        private void Buscar_Load(object sender, EventArgs e)
        {
            txbSearch.Focus();
        }

        private void lvSearch_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int columna = e.Column;
            _comparador.SortColumn = columna;
            if (e.Column == _comparador.SortColumn)
            {
                // Switch the sorting order
                if (lvSearch.Sorting == SortOrder.Ascending)
                    lvSearch.Sorting = SortOrder.Descending;
                else
                    lvSearch.Sorting = SortOrder.Ascending;
            }
            else
                lvSearch.Sorting = SortOrder.Ascending;
        }        
    }
}
