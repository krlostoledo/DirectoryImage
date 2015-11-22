using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DirectoryImage.Tools
{
    class Abridor
    {
        List<dAlbum> myAlbum = new List<dAlbum>();
        bool isFinished = false;
        protected Abridor(string path)
        {
            Abrir(path);
        }

        private void Abrir(string path)
        {
            try
            {
                string ext = Path.GetExtension(path).ToLower();
                if (ext == ".tzip")
                    myAlbum.Add(dAlbum.LoadAlbumFromTzip(path));
                else if (ext == ".txml")
                    myAlbum.Add(dAlbum.LoadAlbumFromTxml(path));
            }
            catch
            {
                MessageBox.Show("No fue posible abrir el documento: " + path, "Error al cargar archivo.");
            }
            isFinished = true;
        }
        public bool IsFinished{
            get
            {
                return isFinished;
            }
        }
        public List<dAlbum> GetAlbum {
            get { return this.myAlbum;}
        }

        #region Singleton Region
        private static Abridor myAbridor = null;
        /// <summary>
        /// Si el abridor está trabajando retorna null.
        /// </summary>
        /// <param name="Album"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Abridor GetAbridor(string path)
        {
            if (myAbridor == null)
            {
                myAbridor = new Abridor(path);
                return myAbridor;
            }
            else if (myAbridor.IsFinished)
                myAbridor.Abrir(path);
            return null;
        }
        public static Abridor GetAbridor()
        {
            return myAbridor;
        }
        public static bool isBusy
        {
            get
            {
                return myAbridor != null && !myAbridor.IsFinished;
            }
        }
        #endregion

        internal static void Clear()
        {
            if (myAbridor != null)
                myAbridor.myAlbum.Clear();
        }
    }
}
