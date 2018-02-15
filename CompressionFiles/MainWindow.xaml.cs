using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompressionFiles
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Obsługa przycisków
        private void ToCompressFile_Click(object sender, RoutedEventArgs e)
        {
            FileInfo file = ReadFile();
            if (file!=null)
                CompressFile(file);
        }

        private void ToDecompressFile_Click(object sender, RoutedEventArgs e)
        {
            FileInfo file = ReadFile();
            if (file != null)
                DecompressFile(file);
        }


        //metoda kompresji pliku do formatu GZ
        public static void CompressFile(FileInfo fileToCompress)
        {
            string message = "";
            // Otwieranie strumienia pliku
            using (FileStream inFile = fileToCompress.OpenRead())
            {
                // Kompresja pliku
                if ((File.GetAttributes(fileToCompress.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    // Tworzenie pliku z rozszerzeniem GZ
                    using (FileStream outFile =
                                File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream Compress = new GZipStream(outFile, CompressionMode.Compress))
                        {
                            // Kopiowanie pliku do strumienia kompresji
                            inFile.CopyTo(Compress);
                            message = "Kompresja pliku " + fileToCompress.Name + " została zakończona powodzeniem.";
                            
                            
                        }
                    }
                }else message = "Błąd pliku! Kompresja nie powiodła się.";
            }
            MessageBox.Show(message, "Zakończenie procesu");
        }

        //metoda wypakowywania pliku z formatu GZ
        public static void DecompressFile(FileInfo fileToDecompress)
        {
            string message="";
            // Otwieranie strumienia skompresowanego pliku
            using (FileStream FileToDecompress = fileToDecompress.OpenRead())
            {
                //Wypakowywanie pliku
                if ((File.GetAttributes(fileToDecompress.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fileToDecompress.Extension == ".gz")
                {
                    //Odtwarzanie nazwy pliku
                    string currentFileName = fileToDecompress.FullName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                    if (!File.Exists(newFileName))
                    {
                        FileStream DecompressdFile = File.Create(newFileName);


                        //Kopiowanie danych do strumienia
                        using (GZipStream Decompress = new GZipStream(FileToDecompress, CompressionMode.Decompress))
                        {
                            Decompress.CopyTo(DecompressdFile);
                            
                            message = "Dekompresja pliku " + fileToDecompress.FullName + " została zakończona powodzeniem.";
                        }

                    }
                    else message = "Plik już istnieje! Dekompresja pliku została anulowana.";
                }else  message = "Błąd pliku! Deompresja nie powiodła się.";
            }
            MessageBox.Show(message, "Zakończenie procesu");

        }

        //Wybór pliku
        public static FileInfo ReadFile()
        {
            string path = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                path = openFileDialog.FileName;
            if (path != "")
                return new FileInfo(path);
            else return null;
        }

    }
}
