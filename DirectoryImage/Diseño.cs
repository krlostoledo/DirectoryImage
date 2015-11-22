using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace DirectoryImage {
    public class dAlbum {
        string _snombre, _sversion, _full_path;
        bool need_change = true;
        private List<dImagen> Listado;

        #region Constructores
        dAlbum(string Name, int iCount)
        {
            if (iCount > 0)
                Listado = new List<dImagen>(iCount);
            else
                Listado = new List<dImagen>(1);
            _snombre = Name;
            need_change = true;
        }
        dAlbum()
            : this("", 1)
        {

        } 
        #endregion

        #region Propiedades
        /// <summary>
        /// Determina si el album ha sido modificado y necesita guardarse.
        /// </summary>
        public bool Need_Change {
            get {
                return need_change;
            }
        }
        #endregion

        /// <summary>
        /// Crea un nuevo dAlbum.
        /// </summary>
        /// <returns>La nueva instancia o null.</returns>
        public static dAlbum Create() {
            NewAlbum al = new NewAlbum();
            if (al.ShowDialog() == DialogResult.Cancel)
                return null;
            return new dAlbum(al.newName, 2);
        }
        /// <summary>
        /// Obtener y modificar una imagen del Album en la posición especificada. 
        /// </summary>
        /// <param name="i">Indice de la imagen.</param>
        /// <returns></returns>
        public dImagen this[int i] {
            get {
                return Listado[i];
            }
            set {
                Listado[i] = value;
                need_change = true;
            }

        }
        
        #region TZip
        public void SaveAlbumToTzip(string path)
        {
            StreamWriter writer = new StreamWriter(path, false);
            int totalImagenes = Listado.Count;
            if (totalImagenes < 1)
            {
                //TODO:: Escribir solo la informacion del album
                return;
            }
            writer.Write("DirectoryImage. v" + Properties.Settings.Default._diVersion + "\n");
            writer.Write(this.Name + "\n");
            writer.Write(totalImagenes + "\n");
            foreach (dImagen image in Listado)
            {
                //TODO:: IMPORTANTE:: CADA IMAGEN SE ESCRIBE EN UNA SOLA LINEA, PUES SE
                //LEE EN UNA SOLA LINEA.
                image.SaveLine(writer);
                writer.Write("\n");
            }
            writer.Close();
            if (true)
                IO.Compact(path);
            need_change = false;
            _full_path = path;
        }
        public void SaveAlbumToTzipBETA(string path)
        {
            int totalImagenes = Listado.Count;
            if (totalImagenes < 1)
            {
                //TODO:: Escribir solo la informacion del album
                return;
            }
            string info = "DirectoryImage. v" + Properties.Settings.Default._diVersion + "\n";
            info += this.Name + "\n" ;
            info += totalImagenes + "\n";
            foreach (dImagen image in Listado)
            {
                //TODO:: IMPORTANTE:: CADA IMAGEN SE ESCRIBE EN UNA SOLA LINEA, PUES SE
                //LEE EN UNA SOLA LINEA.
                info += (image as xmlImagen).SaveLineBETA();
                info += "\n";
            }
            if (true)
                IO.CompactFromString( info, path);
            need_change = false;
            _full_path = path;
        }
        public static dAlbum LoadAlbumFromTzip(string path)
        {
            //FileStream filereader;
            StreamReader reader;
            dAlbum thisAlbum = new dAlbum();

            if (File.Exists(path)){
                reader = IO.Descompact(path);
            }
            //reader = new StreamReader(path);
            else//TODO:: Si no existe q hago.
                return null;
            thisAlbum._sversion = reader.ReadLine();
            thisAlbum._snombre = reader.ReadLine();
            int totalImages = int.Parse(reader.ReadLine());

            for (int k = 0; k < totalImages; k++)
            {
                xmlImagen img = new xmlImagen(thisAlbum);
                img.LoadLine(reader);
                thisAlbum.InsertImagen(img);
            }
            reader.Close();
            File.Delete(Application.ExecutablePath + "tmp");
            thisAlbum.need_change = false;          //el album recie creado no necesita guardarse
            thisAlbum._full_path = path;                      //esta es la direccion del album ahora
            return thisAlbum;
        } 
        #endregion
        
        #region TXml
        public void SaveAlbumToTxml(string path)
        {
            StreamWriter writer = new StreamWriter(path, false);
            int totalImagenes = Listado.Count;
            if (totalImagenes < 1)
            {
                //TODO:: Escribir solo la informacion del album
                return;
            }
            writer.Write("DirectoryImage. v" + Properties.Settings.Default._diVersion + "\n");
            writer.Write(this.Name + "\n");
            writer.Write(totalImagenes + "\n");
            foreach (dImagen image in Listado)
            {
                //TODO:: IMPORTANTE:: CADA IMAGEN SE ESCRIBE EN UNA SOLA LINEA, PUES SE
                //LEE EN UNA SOLA LINEA.
                image.SaveLine(writer);
                writer.Write("\n");
            }
            writer.Close();
            //if (true) //esto es para tzip
            //    IO.Compact(path);
            need_change = false;
            _full_path = path;
        }
        public static dAlbum LoadAlbumFromTxml(string path)
        {
            StreamReader reader;
            dAlbum thisAlbum = new dAlbum();

            if (File.Exists(path)){
                reader = new StreamReader(path);
            }
            else//TODO:: Si no existe q hago.
                return null;
            thisAlbum._sversion = reader.ReadLine();
            thisAlbum._snombre = reader.ReadLine();
            int totalImages = int.Parse(reader.ReadLine());

            for (int k = 0; k < totalImages; k++)
            {
                xmlImagen img = new xmlImagen(thisAlbum);
                img.LoadLine(reader);
                thisAlbum.InsertImagen(img);
            }
            reader.Close();
            thisAlbum.need_change = false;          //el album no necesita guardarse
            return thisAlbum;
            thisAlbum._full_path = path;
        } 
        #endregion
        
        
        /// <summary>
        /// Inserta una imagen en el listado del Album.
        /// </summary>
        /// <param name="image">Imagen a insertar.</param>
        /// <returns></returns>
        public bool InsertImagen(dImagen image) {
            this.Listado.Add(image);
            need_change = true;
            return true;
        }
        /// <summary>
        /// Elimina del listado una imagen.
        /// </summary>
        /// <param name="index">Indice de la imagen a eliminar.</param>
        /// <returns></returns>
        public bool DeleteImagen(int index) {
            Listado.RemoveAt(index);
            need_change = true;
            return true;
        }

        /// <summary>
        /// No implementado.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteImagen(string name) {
            //TODO: esto no está hecho.
            need_change = true;
            return false;

        }
        public decimal Version { get { return Properties.Settings.Default._appVersion; } }
        /// <summary>
        /// Nombre del Album.
        /// </summary>
        public string Name { 
            get { return _snombre; }
            set { this._snombre = value; }
        }
        /// <summary>
        /// Cantidad de Imagenes del Album.
        /// </summary>
        public int Count {
            get { return Listado.Count; }
        }
        public bool HasImages { get { return Listado.Count > 0; } }
        public IEnumerator<dImagen> GetEnumerator() {
            return Listado.GetEnumerator();
        }
        /// <summary>
        /// TreeNodo raíz del Album, con sus hijos introducidos(imágenes).
        /// </summary>
        public TreeNode Root{
            get {
                xmlTreeNode tmp_root = new xmlTreeNode(Name, 21, 21);
                foreach (dImagen image in Listado) {
                    tmp_root.Nodes.Add(image.Root) ;
                }
                return tmp_root;
            }
        
        }
        public void FillTreeNode(TreeNode nodo, ListView lv){
            if (nodo.Nodes.Count != 0)
                goto FillListView;
            string path = nodo.FullPath;
            string[] Path2 = path.Split(new Char[] { '\\' });
            //XmlNode xmlNode = FindXmlNode(Path2);
        FillListView:
            return;
        
        }
        /// <summary>
        /// Realiza una búsqueda del patrón en todas las imágenes del Album y llena el ListView con cada coincidencia.
        /// </summary>
        /// <param name="pattern">Patrón de Busqueda.</param>
        /// <param name="lv">ListView que se llenará con las coincidencias.</param>
        public void Search(Regex pattern, ListView lv) {
            foreach (dImagen image in Listado) {
                image.Search(pattern, lv);
            }
        }
        public static int ImageIndex(string key, ImageList image_list) {
            if (key == "" || image_list == null)
                return 0;   //Es un Folder
            //
            switch (key.ToLower()) {
                case ".htm": key = ".html"; break;
                case ".mpeg": key = ".avi"; break;
                case ".mpg": key = ".avi"; break;
                case ".png": key = ".gif"; break;
                case ".uga": key = ".gif"; break;
                case ".rm": key = ".avi"; break;
                case ".mov": key = ".avi"; break;
                case ".zip": key = ".rar"; break;
                case ".mht": key = ".html"; break;
                case ".ppsx": key = ".pptx"; break;
                case ".wmv": key = ".avi"; break;
                default: break;//Permanece inalterado
            }
            int index = image_list.Images.IndexOfKey(key);
            if (index == -1)
                index = 2;
            return index;
       }
        public static string SizeFormat(long? file_size) {
            if (file_size > 1024) {
                file_size /= 1024 ;
                string ssize = file_size.ToString() ;
                if (file_size >= 1000) {
                    return ssize.Insert(ssize.Length - 3, ",") + " KB";
                }
                else
                    return ssize + " KB";
            }
            else
                return "1 KB";
        }
        public string Full_Path {
            get {
                return _full_path;
            }
        }
    }
    public abstract class dImagen{
        public abstract int Length { get;}
        public abstract void SaveLine(StreamWriter writer);
        public abstract  void LoadLine(StreamReader reader);
        public abstract TreeNode Root { get;}
        public abstract string Name { get; set;}
        public abstract string Path { get;}
        public abstract int Folders { get;}
        public abstract int Files { get;}
        public abstract void Search(Regex pattern, ListView lv);
    }
    public abstract class _TreeNode :TreeNode {
        public _TreeNode(string text, int imageindex, int selectedimageindex):
            base(text, imageindex, selectedimageindex) { }
        public abstract void FillListView(ListView listView);
        public abstract new void Expand();
        public abstract new void Collapse();
        public abstract bool IsFolder { get;}        
    }
    
    
}
