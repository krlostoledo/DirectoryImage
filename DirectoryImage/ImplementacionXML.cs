using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;

using System.Windows.Forms;

namespace DirectoryImage {
    class xmlImagen :dImagen{
        dAlbum _parent;
        XmlDocument innerXML;
        XmlElement root;
        string _errores = "";
        string _sname, _fullpath;
        int _inum_files, _inum_folder ;
        public xmlImagen(dAlbum parent, string path):this(parent) {
            _fullpath = path;
            _sname = System.IO.Path.GetFileName(path);
            innerXML = new XmlDocument();
            
            root = innerXML.CreateElement(Atributos.Folder);
            
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            root.SetAttribute(Atributos.Name, dirInfo.Name);//agrego el nombre
            root.SetAttribute(Atributos.FullPath, path);    //agrego el camino
            
            long size = Rcsvo(root, dirInfo);
            if (_errores.Length != 0) {
                MessageBox.Show("Ocurrieron los siguientes errores.\nInformación: " + _errores, "Error.");
            }
            root.SetAttribute(Atributos.Size, size.ToString());//agrego el tamaño
            root.SetAttribute(Atributos.FolderNum, _inum_folder.ToString());
            root.SetAttribute(Atributos.FilesNum, _inum_files.ToString());
            innerXML.AppendChild(root);
            parent.InsertImagen(this);
        }
        public xmlImagen(dAlbum parent) {
            if (parent == null)
                throw new Exception();
            _parent = parent;
        }
        /// <summary>
        /// Inserta en xmlparent todos los archivos y directorios de directory.
        /// </summary>
        /// <param name="xmlparent"></param>
        /// <param name="directory"></param>
        /// <returns>Tamaño en bytes de la carpeta directory</returns>
        long Rcsvo(XmlElement xmlparent, DirectoryInfo directory) {
            long totalSize = 0;
            try {
                foreach (DirectoryInfo dir in directory.GetDirectories()) {
                    XmlElement xmlTemp = xmlparent.OwnerDocument.CreateElement(Atributos.Folder);
                    //xmlTemp.SetAttribute("P", dir.FullName);      //Add path
                    xmlTemp.SetAttribute(Atributos.Name, dir.Name);      //Add name
                    xmlTemp.SetAttribute(Atributos.CreationTime, dir.CreationTime.ToString());

                    long dirSize = Rcsvo(xmlTemp, dir);              //Rcsvo in xmlTemp

                    xmlTemp.SetAttribute(Atributos.Size, dirSize.ToString());  //Add size
                    totalSize += dirSize;
                    _inum_folder++;

                    //Add node to xmlparent
                    xmlparent.AppendChild(xmlTemp);
                }
                foreach (FileInfo file in directory.GetFiles()) {
                    XmlElement xmlTemp = xmlparent.OwnerDocument.CreateElement(Atributos.File);
                    //xmlTemp.SetAttribute("P", file.FullName);  //Add path
                    xmlTemp.SetAttribute(Atributos.Name, file.Name);      //Add name
                    xmlTemp.SetAttribute(Atributos.CreationTime, file.CreationTime.ToString());

                    long filesize = file.Length;
                    totalSize += filesize;
                    xmlTemp.SetAttribute(Atributos.Size, filesize.ToString());  //Add size
                    _inum_files++;

                    //Add node to xmlparent
                    xmlparent.AppendChild(xmlTemp);
                }
            }
            catch (UnauthorizedAccessException UAE) {
                _errores += "\n Error: "+UAE.Message+ ".";
                return totalSize;
            }
            catch (Exception ex) {
                MessageBox.Show("Un error no previsto ha ocurrido.\nInformación: "+ex.Message  ,"Error.");
                return totalSize;
            }
            return totalSize;
        }
        public override  void LoadLine(StreamReader reader) {
            try{
                innerXML = new XmlDocument();
                innerXML.InnerXml = reader.ReadLine();
                


                this.root = innerXML.ChildNodes[0] as XmlElement;
                string folder = root.Attributes[Atributos.FolderNum].Value;
                string files = root.Attributes[Atributos.FilesNum].Value;
                if(folder != null)
                    this._inum_folder = int.Parse(folder) ;
                if(files != null)
                    this._inum_files = int.Parse(files) ;
            }
            catch(NullReferenceException){
                MessageBox.Show("Algunos valores no pudieron obtenerse. Posiblemente el archivo fue creado por una versión anterior a la 0.31.\n", "Error al cargar archivo.");
            }
            catch(Exception ex){
                //this.txbResp.Text = "";
                MessageBox.Show("No fue posible leer el documento. Error: "+ex.Message, "Error al cargar archivo.");

            } 
        }
        public override void SaveLine(StreamWriter writer) {
            if (innerXML == null)
                return;
            //Elimino cualquier salto de línea y escribo
            string format = innerXML.InnerXml.Replace("\n", string.Empty);

            writer.Write(format);
            
        }
        public string SaveLineBETA()
        {
            if (innerXML == null)
                return string.Empty;
            //Elimino cualquier salto de línea y escribo
            string format = innerXML.InnerXml.Replace("\n", string.Empty);
            return format;
        }
        public override int Length {
            get { return int.Parse(root.Attributes[Atributos.Size].Value) ; }
        }
        public override string Name {
            get { return root.Attributes[Atributos.Name].Value ; }
            set {
                root.SetAttribute(Atributos.Name, value);
            }
        }
        public override string Path {
            get { return root.Attributes[Atributos.FullPath].Value; }
        }
        public override int Files {
            get { return this._inum_files;}
        }
        public override int Folders {
            get { return this._inum_folder; }
        }
        public override TreeNode Root {
            get { 
                xmlTreeNode tmp = new xmlTreeNode(root, Name, 22, 22);
                tmp.Expand();
                return tmp;
            }
        }

        public override void Search(Regex pattern, ListView lv) {
            //Voy a utilizar una pila en lugar de un método recursivo. Buscando eficiencia.
            //TODO: lograr mas eficiencia aun.
            Stack<XmlElement> pila = new Stack<XmlElement>(_inum_folder) ;
            pila.Push(this.root);                       //introduzco raiz en la pila.
            while (pila.Count > 0){                     //mientras hallan elementos en la pila.
                XmlElement element = pila.Pop() ;       //saco el elemento del tope
                string element_name = element.Attributes[Atributos.Name].Value ;
                if (pattern.IsMatch(element_name.ToLower()))
                {//INSERTAR EN EL LISTVIEW
                    int image_index = dAlbum.ImageIndex(System.IO.Path.GetExtension(element.Attributes[Atributos.Name].Value), lv.SmallImageList);
                    long? size = long.Parse(element.Attributes[Atributos.Size].Value);
                    string full_path = _parent.Name+"\\"+ FullPath(element) ;
                    ListViewItem lvi = new ListViewItem(new string[3] { element_name, dAlbum.SizeFormat(size), full_path }, image_index);
                    lv.Items.Add(lvi);
                }
                //Independientemente si coincide o no, voy a empilar todos sus hijos.
                foreach(XmlElement tmp_element in element.ChildNodes){
                    pila.Push(tmp_element);
                }
            }
            
        }

        public string FullPath(XmlElement element) {
            string path = "";
            while (element != null) {
                path = element.Attributes[Atributos.Name].Value + "\\" +path;
                element = element.ParentNode as XmlElement;
            }
            return path;
        }
    
    }
    public class xmlTreeNode :_TreeNode{
        XmlElement _element;
        /// <summary>
        /// Especifica si el TreeNode representa a un album
        /// </summary>
        bool IsRoot = false;
        public xmlTreeNode(string text, int imageindex, int selectedimageindex) : this(null, text, imageindex, selectedimageindex) { }
        public xmlTreeNode(XmlElement element)
            :
            base(element.Name, 0, 1) {
            this._element = element;

        }
        public xmlTreeNode(XmlElement element, string text, int imageindex, int selectedimageindex)
            :
            base(text, imageindex, selectedimageindex) 
        {
            if (element == null)//es root de una imagen.
                IsRoot = true;
            this._element = element;
        }
        public new string Name {
            get {
                if (IsRoot)
                    return base.Name;
                return _element.Attributes[Atributos.Name].Value; 
            }
            set {
                if (this.Level == 1)
                    _element.SetAttribute(Atributos.Name, value);
            
            }
        }

        public override void Expand() {
            if (this.Nodes.Count != 0) {
                foreach (xmlTreeNode treenode in this.Nodes) {
                    if (treenode.Nodes.Count != 0)
                        continue;
                    treenode.Expand();
                }
                return;
            }
            if (IsRoot) {
                return;
            }
            foreach (XmlElement element in _element.ChildNodes) {
                if (!element.HasChildNodes)//es un archivo
                    continue;
                TreeNode nodo = new xmlTreeNode(element);
                nodo.Text = element.Attributes[Atributos.Name].Value;
                foreach (XmlElement element2 in element.ChildNodes) {
                    TreeNode nodo2 = new xmlTreeNode(element2);
                    nodo2.Text = element.Attributes[Atributos.Name].Value;
                }
                Nodes.Add(nodo);
            }
        }
        public override void Collapse() {
            if (IsRoot) {
                return;
            }
            foreach (TreeNode node in Nodes) { 
                node.Nodes.Clear();
            }
            
        }
        public override void FillListView(ListView listView) {
            listView.Items.Clear();
            listView.BeginUpdate();

            if (IsRoot) {
                foreach (TreeNode node in this.Nodes) {
                    ListViewItem lv = new ListViewItem(node.Text, 22);
                    //lv.SubItems.Add(node.Attributes[Atributos.Size].Value);
                    listView.Items.Add(lv);
                }
            }
            else {
                #region Insertar Folder ..
                ListViewItem lv = new ListViewItem("..", 0);
                listView.Items.Add(lv);
                #endregion

                foreach (XmlNode xmlnode in _element.ChildNodes) {
                    lv = new ListViewItem(xmlnode.Attributes[Atributos.Name].Value, 0);
                    ImageList imgList = listView.SmallImageList;
                    string key = "";
                    if (xmlnode.Name != Atributos.Folder)
                        key = Path.GetExtension(xmlnode.Attributes[Atributos.Name].Value);
                    lv.ImageIndex = dAlbum.ImageIndex(key, imgList);
                    
                    long? size = long.Parse(xmlnode.Attributes[Atributos.Size].Value);
                    
                    lv.SubItems.Add(dAlbum.SizeFormat(size));
                    lv.SubItems.Add(xmlnode.Attributes[Atributos.CreationTime].Value);

                    listView.Items.Add(lv);
                }
            }
            listView.EndUpdate();
        }
        public override bool IsFolder {
            get { return _element.Name == "F"; }
        }
        /// <summary>
        /// Busca dado el nodo raíz, el camino hasta el final de full_path.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="full_path">Camino separado por \\, el primer valor coincide con el nombre del Album.</param>
        /// <returns></returns>
        public static TreeNode ShowNode(TreeNode root, string full_path) {
            string[] separeted_path = full_path.Split('\\');
            TreeNode tmp_node = root ;
            //k es igual a 1 pues separeted_path en 0 tiene el nombre del album.
            for(int k = 1; k < separeted_path.Length; k++){
                foreach (TreeNode node in tmp_node.Nodes) {
                    if (node.Text == separeted_path[k]) {
                        node.Expand();
                        tmp_node = node;
                        break;
                    }
                }
            }
            return tmp_node;
            
        }
    }
}
