using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using DirectoryImage;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;

namespace DirectoryImage
{
    class IO
    {
        public static bool Compact(string file) {
            try
            {
                FileStream infile = new FileStream(file, FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite);
                byte[] buffer = new byte[infile.Length];
                
                int count = infile.Read(buffer, 0, buffer.Length);
                if (count != buffer.Length){
                    infile.Close();
                    MessageBox.Show("Test Failed: Unable to read data from file");
                    return false;
                }
                infile.Close();     
                
                FileStream outfile = new FileStream(file,FileMode.Create);
                GZipStream gZip = new GZipStream(outfile, CompressionMode.Compress, true);
                gZip.Write(buffer, 0, buffer.Length);
                gZip.Close();
                outfile.Close();

                StreamWriter writer = new StreamWriter(file, true);
                writer.WriteLine("Original Size:") ;
                writer.WriteLine(count) ;
                writer.Close();
                
                
            }
            catch (InvalidDataException){
                MessageBox.Show("Error: The file being read contains invalid data.");
                return false;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Error:The file specified was not found.");
                return false;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error: path is a zero-length string, contains only white space, or contains one or more invalid characters");
                return false;
            }
            catch (PathTooLongException)
            {
                MessageBox.Show("Error: The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.");
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Error: The specified path is invalid, such as being on an unmapped drive.");
                return false;
            }
            catch (IOException)
            {
                MessageBox.Show("Error: An I/O error occurred while opening the file.");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Error: path specified a file that is read-only, the path is a directory, or caller does not have the required permissions.");
                return false;
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Error: You must provide parameters for MyGZIP.");
                return false;
            }
            return true ;
        }
        public static bool CompactFromString(string data, string path) {
            try
            {
                
                byte[] new_data = Encoding.ASCII.GetBytes(data);
                
                int lengt = data.Length;
                byte[] buffer = new byte[lengt];

                //buffer = (byte[])c.ConvertTo(data, arr.GetType());
                MemoryStream reader = new MemoryStream(new_data);

                GZipStream gZip = new GZipStream(reader, CompressionMode.Compress, true);
                gZip.Write(buffer, 0, new_data.Length);
                gZip.Close();

                StreamWriter writer = new StreamWriter(path, true);
                writer.Write(buffer);
                writer.WriteLine("Original Size:");
                writer.WriteLine(lengt);
                writer.Close();

                return true;
            }
            
            catch (InvalidDataException){
            MessageBox.Show("Error: The file being read contains invalid data.");
            return false;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Error:The file specified was not found.");
                return false;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error: path is a zero-length string, contains only white space, or contains one or more invalid characters");
                return false;
            }
            catch (PathTooLongException)
            {
                MessageBox.Show("Error: The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.");
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Error: The specified path is invalid, such as being on an unmapped drive.");
                return false;
            }
            catch (IOException)
            {
                MessageBox.Show("Error: An I/O error occurred while opening the file.");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Error: path specified a file that is read-only, the path is a directory, or caller does not have the required permissions.");
                return false;
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Error: You must provide parameters for MyGZIP.");
                return false;
            }
        
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns>El StreamReader debe ser borrado.</returns>
        public static StreamReader Descompact(string path) {
            try{
                StreamReader reader = new StreamReader(path);
                string last_line = "1000";
                while (!reader.EndOfStream)
                    last_line = reader.ReadLine();

                int size = int.Parse(last_line);
                reader.Close();

                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                GZipStream zipStream = new GZipStream(file, CompressionMode.Decompress);

                byte[] array = new byte[size];
                zipStream.Read(array, 0, size);
                zipStream.Close();
                                
                FileStream save = new FileStream(Application.ExecutablePath + "tmp", FileMode.Create, FileAccess.Write);
                save.Write(array, 0, array.Length);
                save.Close();
                
                zipStream.Close();
                return new StreamReader(Application.ExecutablePath + "tmp");
            }
            catch{
                MessageBox.Show("Error.");
                return null;
            }
            
         }


        /// <summary>
        /// Extrae una lista de count elementos.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> RemoveList<T>(List<T> list, int count)
        {
            if (count < 1)
                return null;
            List<T> value = new List<T>(count);
            for (int k = 0; k < count; k++)
            {
                T tmp = IO.Pop<T>(list);
                if (tmp == null)
                    break;
                value.Add(tmp);
            }
            return value;
        }
        /// <summary>
        /// Retorna el último elemento de una lista y lo elimina de esta.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>Ultimo elemento o default si esta vacía.</returns>
        public static T Pop<T>(List<T> list)
        {
            int index = list.Count - 1;
            if (index < 0)
                return default(T);
            T value = list[index];
            list.RemoveAt(index);
            return value;
        }
    }
}
