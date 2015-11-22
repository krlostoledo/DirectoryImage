using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Security;
using System.Collections;
using System.Threading;
using DirectoryImage;

namespace DirectoryImage
{
    partial class xmlImagenRed : dImagen
    {
        XmlDocument innerXML;
        XmlElement root;
        string _errores = "";
        string _sname, _fullpath;
        int _inum_files, _inum_folder;


        public xmlImagenRed() 
        {
            _fullpath = "Red";
            //_sname = System.IO.Path.GetFileName(path);
            innerXML = new XmlDocument();

            root = innerXML.CreateElement(Atributos.Folder);

            //DirectoryInfo dirInfo = new DirectoryInfo(path);
            //root.SetAttribute(Atributos.Name, dirInfo.Name);    //agrego el nombre
            //root.SetAttribute(Atributos.FullPath, path);        //agrego el camino

            /***** Llamado Recursivo ******/
            long size = RcsvoRed(root);

            if (_errores.Length != 0)
            {
                MessageBox.Show("Ocurrieron los siguientes errores.\nInformación: " + _errores, "Error.");
            }
            root.SetAttribute(Atributos.Size, size.ToString());//agrego el tamaño
            root.SetAttribute(Atributos.FolderNum, _inum_folder.ToString());
            root.SetAttribute(Atributos.FilesNum, _inum_files.ToString());
            innerXML.AppendChild(root);
        }

        /// <summary>
        /// Inserta en xmlparent todos los archivos y directorios de directory.
        /// </summary>
        /// <param name="xmlparent"></param>
        /// <param name="directory"></param>
        /// <returns>Tamaño en bytes de la carpeta directory</returns>
        long RcsvoRed(XmlElement xmlparent)
        {
            int threads_count = 44;
            List<string> lista = NetworkBrowser.getNetworkComputers();      //obtener listado de los servidores
            List<Hilo> hilos = new List<Hilo>();

            int count = lista.Count / threads_count;                        //en cuanto separaremos la lista
            if (count < 1)
                return 0;
           
            for (int k = 0; k < threads_count; k++)
            {

                hilos.Add(new Hilo(IO.RemoveList(lista, count)));
            }
            for (int k = 0; k < threads_count; k++)
            {
                hilos[k].DeepSearch();
            }
            return 0;
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
    }

    #region Hilos
    class Hilo
    {
        List<string> _server_list, result;
        public bool Finished = false;
        List<TreeNode> lista_TreeNodes = new List<TreeNode>();
        XmlElement parent;

        public Hilo(List<string> server_list)
        {
            if (server_list == null || server_list.Count == 0)
                throw new ArgumentNullException();
            this._server_list = server_list;
        }
        public void DeepSearch()
        {
            result = new List<string>(_server_list.Count);

            foreach (string server in _server_list)
            {
                List<string> list = NetworkBrowser.getShareFolders(server);
                if (list == null)
                    continue;
                XmlElement xmlTemp;
                lock (parent)
                {
                    xmlTemp = parent.OwnerDocument.CreateElement(Atributos.Folder);
                }
                
                xmlTemp.SetAttribute(Atributos.Name, server);      //Add name
                //xmlTemp.SetAttribute(Atributos.CreationTime, dir.CreationTime.ToString());
                long dirSize = 0;
                foreach (string carpeta_compartida in list)
                {
                    if (!carpeta_compartida.Contains("$"))
                    {
                        DirectoryInfo info = new DirectoryInfo(carpeta_compartida);
                        dirSize += xmlImagen.Rcsvo(xmlTemp, info);
                    }
                }
               
                xmlTemp.SetAttribute(Atributos.Size, dirSize.ToString());  //Add size
                //totalSize += dirSize;
                //_inum_folder++;

                //Add node to xmlparent
                lock (parent)
                {
                    parent.AppendChild(xmlTemp);
                }               
            }
            Finished = true;
        }        
        public bool IsFinished
        {
            get
            {
                return this.Finished;
            }
        }
        public List<TreeNode> List
        {
            get
            {
                if (Finished)
                    return lista_TreeNodes;
                else
                    return null;
            }
        }
    }
    #endregion

    #region NetworkBrowser CLASS

    public sealed class NetworkBrowser
    {
        #region Dll Imports


        [DllImport("Netapi32", CharSet = CharSet.Auto,
        SetLastError = true),
        SuppressUnmanagedCodeSecurityAttribute]
        public static extern int NetServerEnum(
            string ServerNane, // must be null
            int dwLevel,
            ref IntPtr pBuf,
            int dwPrefMaxLen,
            out int dwEntriesRead,
            out int dwTotalEntries,
            int dwServerType,
            string domain, // null for login domain
            out int dwResumeHandle
            );

        [DllImport("Netapi32", SetLastError = true),
        SuppressUnmanagedCodeSecurityAttribute]

        /// <summary>
        /// Netapi32.dll : The NetApiBufferFree function frees
        /// the memory that the NetApiBufferAllocate function allocates.
        /// Call NetApiBufferFree to free
        /// the memory that other network
        /// management functions return.
        /// </summary>
        public static extern int NetApiBufferFree(
            IntPtr pBuf);

        //create a _SERVER_INFO_100 STRUCTURE

        [StructLayout(LayoutKind.Sequential)]
        public struct _SERVER_INFO_100
        {
            internal int sv100_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string sv100_name;
        }

        #region Share_Info_502 struct
        [StructLayout(LayoutKind.Sequential)]
        public struct Share_Info_502
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string shi502_netname;

            internal int shi502_type;

            [MarshalAs(UnmanagedType.LPWStr)]
            internal string shi502_remark;

            internal int shi502_permissions;
            internal int shi502_max_uses;
            internal int shi502_current_uses;

            [MarshalAs(UnmanagedType.LPWStr)]
            internal string shi502_path;

            [MarshalAs(UnmanagedType.LPWStr)]
            internal string shi502_passwd;

            internal int shi502_reserved;
            [MarshalAs(UnmanagedType.LPStruct)]
            internal object shi502_security_descriptor;//dale null

        }
        #endregion

        #region SHARE_INFO_0 struct
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHARE_INFO_0
        {
            public string shi0_netname;
        }
        const uint MAX_PREFERRED_LENGTH = 0xFFFFFFFF;
        const int NERR_Success = 0;
        #endregion

        #region NetShareEnum Function
        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int NetShareEnum(
             StringBuilder ServerName,
             int level,
             ref IntPtr bufPtr,
             uint prefmaxlen,
             ref int entriesread,
             ref int totalentries,
             ref int resume_handle
             );
        #endregion

        #endregion

        #region Public Constructor
        /// <summary>
        /// Constructor, simply creates a new NetworkBrowser object
        /// </summary>
        public NetworkBrowser()
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Uses the DllImport : NetServerEnum
        /// with all its required parameters
        /// (see http://msdn.microsoft.com/library/default.asp?
        ///      url=/library/en-us/netmgmt/netmgmt/netserverenum.asp
        /// for full details or method signature) to
        /// retrieve a list of domain SV_TYPE_WORKSTATION
        /// and SV_TYPE_SERVER PC's
        /// </summary>
        /// <returns>Arraylist that represents all the SV_TYPE_WORKSTATION and SV_TYPE_SERVER
        /// PC's in the Domain</returns>
        public static List<string> getNetworkComputers()
        {
            //local fields

            var networkComputers = new List<string>();
            const int MAX_PREFERRED_LENGTH = -1;
            int SV_TYPE_WORKSTATION = 1;
            int SV_TYPE_SERVER = 2;
            IntPtr buffer = IntPtr.Zero;
            IntPtr tmpBuffer = IntPtr.Zero;
            int entriesRead = 0;
            int totalEntries = 0;
            int resHandle = 0;
            int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));

            try
            {
                //call the DllImport : NetServerEnum 
                //with all its required parameters
                //see http://msdn.microsoft.com/library/
                //default.asp?url=/library/en-us/netmgmt/netmgmt/netserverenum.asp
                //for full details of method signature
                int ret = NetServerEnum(null, 100, ref buffer,
                    MAX_PREFERRED_LENGTH,
                    out entriesRead,
                    out totalEntries, SV_TYPE_WORKSTATION |
                    SV_TYPE_SERVER, null, out 
                    resHandle);
                //if the returned with a NERR_Success 

                //(C++ term), =0 for C#

                if (ret == 0)
                {
                    //loop through all SV_TYPE_WORKSTATION 

                    //and SV_TYPE_SERVER PC's

                    for (int i = 0; i < totalEntries; i++)
                    {
                        //get pointer to, Pointer to the 

                        //buffer that received the data from

                        //the call to NetServerEnum. 

                        //Must ensure to use correct size of 

                        //STRUCTURE to ensure correct 

                        //location in memory is pointed to

                        tmpBuffer = new IntPtr((int)buffer +
                                   (i * sizeofINFO));
                        //Have now got a pointer to the list 

                        //of SV_TYPE_WORKSTATION and 

                        //SV_TYPE_SERVER PC's, which is unmanaged memory

                        //Needs to Marshal data from an 

                        //unmanaged block of memory to a 

                        //managed object, again using 

                        //STRUCTURE to ensure the correct data

                        //is marshalled 

                        _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)
                            Marshal.PtrToStructure(tmpBuffer,
                                    typeof(_SERVER_INFO_100));

                        //add the PC names to the ArrayList

                        networkComputers.Add(svrInfo.sv100_name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with acessing " +
                    "network computers in NetworkBrowser " +
                    "\r\n\r\n\r\n" + ex.Message,
                    "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                //The NetApiBufferFree function frees 

                //the memory that the 

                //NetApiBufferAllocate function allocates

                NetApiBufferFree(buffer);
            }
            //return entries found

            return networkComputers;

        }

        public static List<string> getShareFolders(string server_name)
        {
            int entriesread = 0;
            int totalentries = 0;
            int resume_handle = 0;
            int nStructSize = Marshal.SizeOf(typeof(SHARE_INFO_0));
            IntPtr bufPtr = IntPtr.Zero;
            StringBuilder server = new StringBuilder(server_name);

            int ret = NetShareEnum(server, 0, ref bufPtr, MAX_PREFERRED_LENGTH, ref entriesread, ref totalentries, ref resume_handle);
            if (ret == NERR_Success)
            {
                IntPtr currentPtr = bufPtr;
                var listado = new List<string>(entriesread);
                for (int i = 0; i < entriesread; i++)
                {
                    SHARE_INFO_0 shi0 = (SHARE_INFO_0)Marshal.PtrToStructure(currentPtr, typeof(SHARE_INFO_0));
                    listado.Add(shi0.shi0_netname);
                    currentPtr = new IntPtr(currentPtr.ToInt32() + nStructSize);
                }
                NetApiBufferFree(bufPtr);
                return listado;
            }
            return null;
        }
        #endregion
    }
    #endregion
}
