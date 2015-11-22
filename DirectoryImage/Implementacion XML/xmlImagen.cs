using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DirectoryImage
{
    partial class xmlImagen : dImagen
    {
        XmlDocument innerXML;
        XmlElement root;
        string _errores = "";
        string _sname, _fullpath;
        int _inum_files, _inum_folder;

        /// <summary>
        /// Constructor de una dImagen nueva.
        /// </summary>
        /// <param name="parent">Album al que pertenecerá.</param>
        /// <param name="path">Camino a la carpeta a la cual hacerle la dImagen.</param>
        public xmlImagen(string path)
        {
            _fullpath = path;
            _sname = System.IO.Path.GetFileName(path);
            innerXML = new XmlDocument();

            root = innerXML.CreateElement(Atributos.Folder);

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            root.SetAttribute(Atributos.Name, dirInfo.Name);    //agrego el nombre
            root.SetAttribute(Atributos.FullPath, path);        //agrego el camino

            /***** Llamado Recursivo ******/
            long size = Rcsvo(root, dirInfo);

            if (_errores.Length != 0)
            {
                MessageBox.Show("Ocurrieron los siguientes errores.\nInformación: " + _errores, "Error.");
            }
            root.SetAttribute(Atributos.Size, size.ToString());//agrego el tamaño
            root.SetAttribute(Atributos.FolderNum, _inum_folder.ToString());
            root.SetAttribute(Atributos.FilesNum, _inum_files.ToString());
            innerXML.AppendChild(root);
        }

        public xmlImagen() { }

        /// <summary>
        /// Inserta en xmlparent todos los archivos y directorios de directory.
        /// </summary>
        /// <param name="xmlparent"></param>
        /// <param name="directory"></param>
        /// <returns>Tamaño en bytes de la carpeta directory</returns>
        public static long Rcsvo(XmlElement xmlparent, DirectoryInfo directory)
        {
            long totalSize = 0;
            try
            {
                foreach (DirectoryInfo dir in directory.GetDirectories())
                {
                    XmlElement xmlTemp = xmlparent.OwnerDocument.CreateElement(Atributos.Folder);
                    //xmlTemp.SetAttribute("P", dir.FullName);      //Add path
                    xmlTemp.SetAttribute(Atributos.Name, dir.Name);      //Add name
                    xmlTemp.SetAttribute(Atributos.CreationTime, dir.CreationTime.ToString());

                    long dirSize = Rcsvo(xmlTemp, dir);              //Rcsvo in xmlTemp

                    xmlTemp.SetAttribute(Atributos.Size, dirSize.ToString());  //Add size
                    totalSize += dirSize;
                    //_inum_folder++;

                    //Add node to xmlparent
                    xmlparent.AppendChild(xmlTemp);
                }
                foreach (FileInfo file in directory.GetFiles())
                {
                    XmlElement xmlTemp = xmlparent.OwnerDocument.CreateElement(Atributos.File);
                    //xmlTemp.SetAttribute("P", file.FullName);  //Add path
                    xmlTemp.SetAttribute(Atributos.Name, file.Name);      //Add name
                    xmlTemp.SetAttribute(Atributos.CreationTime, file.CreationTime.ToString());

                    long filesize = file.Length;
                    totalSize += filesize;
                    xmlTemp.SetAttribute(Atributos.Size, filesize.ToString());  //Add size
                    //_inum_files++;

                    //Add node to xmlparent
                    xmlparent.AppendChild(xmlTemp);
                }
            }
            catch (UnauthorizedAccessException)
            {
                //_errores += "\n Error: " + UAE.Message + ".";
                return totalSize;
            }
            catch (Exception)
            {
                //_errores += "\nError no previsto: " + ex.Message;
                return totalSize;
            }
            return totalSize;
        }

        public override void LoadLine(StreamReader reader)
        {
            try
            {
                innerXML = new XmlDocument();
                innerXML.InnerXml = reader.ReadLine();

                this.root = innerXML.ChildNodes[0] as XmlElement;
                string folder = root.Attributes[Atributos.FolderNum].Value;
                string files = root.Attributes[Atributos.FilesNum].Value;
                if (folder != null)
                    this._inum_folder = int.Parse(folder);
                if (files != null)
                    this._inum_files = int.Parse(files);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Algunos valores no pudieron obtenerse. Posiblemente el archivo fue creado por una versión anterior a la 0.31.\n", "Error al cargar archivo.");
            }
            catch (Exception ex)
            {
                //this.txbResp.Text = "";
                MessageBox.Show("No fue posible leer el documento. Error: " + ex.Message, "Error al cargar archivo.");

            }
        }
        public override void SaveLine(StreamWriter writer)
        {
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
        public override int Length
        {
            get { return int.Parse(root.Attributes[Atributos.Size].Value); }
        }
        public override string Name
        {
            get { return root.Attributes[Atributos.Name].Value; }
            set
            {
                root.SetAttribute(Atributos.Name, value);
            }
        }
        public override string Path
        {
            get { return root.Attributes[Atributos.FullPath].Value; }
        }
        public override int Files
        {
            get { return this._inum_files; }
        }
        public override int Folders
        {
            get { return this._inum_folder; }
        }
        public override TreeNode Root
        {
            get
            {
                xmlTreeNode tmp = new xmlTreeNode(root, Name, 22, 22);
                tmp.Expand();
                return tmp;
            }
        }
        public override List<XmlElement> Search(Regex pattern)
        {
            //Voy a utilizar una pila en lugar de un método recursivo. Buscando eficiencia.
            //TODO: lograr mas eficiencia aun.
            List<XmlElement> list = new List<XmlElement>(50);

            Stack<XmlElement> pila = new Stack<XmlElement>(_inum_folder);
            pila.Push(this.root);                       //introduzco raiz en la pila.

            while (pila.Count > 0)
            {                     //mientras hallan elementos en la pila.
                XmlElement element = pila.Pop();       //saco el elemento del tope
                string element_name = element.Attributes[Atributos.Name].Value;
                if (pattern.IsMatch(element_name.ToLower()))
                {
                    list.Add(element);
                }
                //Independientemente si coincide o no, voy a empilar todos sus hijos.
                foreach (XmlElement tmp_element in element.ChildNodes)
                {
                    pila.Push(tmp_element);
                }
            }
            return list;
        }

        public string FullPath(XmlElement element)
        {
            string path = "";
            while (element != null)
            {
                path = element.Attributes[Atributos.Name].Value + "\\" + path;
                element = element.ParentNode as XmlElement;
            }
            return path;
        }

    }
}
