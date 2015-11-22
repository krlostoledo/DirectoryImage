using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace DirectoryImage.Tools
{
    /// <summary>
    /// Clase Singleton Encargada de hacer busquedas de un patrón en una coleccion de albums.
    /// </summary>
    class Buscador
    {
        /// <summary>
        /// Patron a Buscar.
        /// </summary>
        Regex pattern;
        /// <summary>
        /// Lista de albums donde buscar el patrón.
        /// </summary>
        List<dAlbum> Album;
        /// <summary>
        /// Lista de elementos útil para la búsqueda.
        /// </summary>
        List<AlbumName_ListResult> list = new List<AlbumName_ListResult>(10);

        public bool IsFinished { get; private set; }

        protected Buscador(List<dAlbum> Album, Regex pattern)
        {
            this.pattern = pattern;
            if(Album != null)
                this.Album = Album;
            IsFinished = false;
        }
        /// <summary>
        /// Función que realiza toda la operación.
        /// </summary>
        public void Find()
        {
            IsFinished = false;
            list.Clear();
            foreach (dAlbum album in Album)
            {
                foreach (dImagen image in album.Listado)
                {
                    list.Insert(0, new AlbumName_ListResult() { AlbumName = album.Name, Resultados = image.Search(pattern) });
                } 
            }          

            IsFinished = true;
        }

        /// <summary>
        /// Función para llenar un ListView con los resultados de la búsqueda.
        /// </summary>
        /// <param name="lv"></param>
        public void Fill(ListView lv) {
            if (!IsFinished)
                return;
            foreach (AlbumName_ListResult item in list)
            {
                foreach (XmlElement element in item.Resultados)
                {
                    //INSERTAR EN EL LISTVIEW
                    //int image_index = dAlbum.ImageIndex(System.IO.Path.GetExtension(element.Attributes[Atributos.Name].Value),
                    //    lv.SmallImageList);
                    long? size = long.Parse(element.Attributes[Atributos.Size].Value);
                    string full_path = item.AlbumName + "\\" + FullPath(element);

                    ListViewItem lvi = new ListViewItem(new string[3] { element.Attributes[Atributos.Name].Value, 
                        dAlbum.SizeFormat(size), full_path },
                        0/*image_index*/);

                    if (element.Name != Atributos.Folder)
                    {
                        //cojo la extension
                        string key = Path.GetExtension(element.Attributes[Atributos.Name].Value);
                        //en dependencia de la extension y las existentes en imgList retorna su index
                        lvi.ImageIndex = dAlbum.ImageIndex(key, lv.SmallImageList);
                    }
                    lv.Items.Add(lvi);
                }
            }            
        }
        public string FullPath(XmlElement element)
        {
            string path = "";
            while (element != null)
            {
                if (element.ParentNode == null)
                {
                    path = element.Attributes[Atributos.Name].Value + path;
                    return path;
                }
                path = element.Attributes[Atributos.Name].Value + "\\" + path;
                element = element.ParentNode as XmlElement;
            }

            return path;
        }

        #region Singleton Region
        private static Buscador myBuscador = null;
        /// <summary>
        /// Si el buscador está trabajando retorna null.
        /// </summary>
        /// <param name="Album"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Buscador GetBuscador(dAlbum Album, Regex pattern) {
           return GetBuscador(new List<dAlbum>(){ Album}, pattern);
        }
        public static Buscador GetBuscador(List<dAlbum> Album, Regex pattern)
        {
            if (Album == null || Album.Count == 0)
                return null;

            if (myBuscador == null)
            {
                myBuscador = new Buscador( Album, pattern);
            }
            else
            {
                if (!myBuscador.IsFinished)
                    return null;
                myBuscador.Album = Album;
                myBuscador.pattern = pattern;
            }
            return myBuscador;
        }
        public static Buscador GetBuscador()
        {
            return myBuscador;           
        }
        public static bool isBusy {
            get {
                return myBuscador != null && !myBuscador.IsFinished;
            }
        }
        #endregion

        private struct AlbumName_ListResult {
            public string AlbumName;
            public List<XmlElement> Resultados;
        }
    }
}
