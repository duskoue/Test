using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApiDocument
{
    /// <summary>
    /// Interaction logic for frmPregled.xaml
    /// </summary>
    public partial class frmPregled : Window
    {
        MainWindow mainW;
        public frmPregled()
        {
            InitializeComponent();
           
            this.KeyDown += new System.Windows.Input.KeyEventHandler(FrmPregled_KeyDown);
            mainW = System.Windows.Application.Current.MainWindow as MainWindow;
        }

        private void FrmPregled_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Title = mainW.txtnazivfunkcija.Text;

            //string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //string folderName = System.IO.Path.Combine(projectPath, "KompletnaDokumentacija");
            //DirectoryInfo d = new DirectoryInfo(folderName);
            //FileInfo[] Files = d.GetFiles("*.pdf"); 

            //foreach (FileInfo file in Files)
            //{


            //    lbpregled1.Items.Add(file.Name);
            //}

            lbpregled1.SelectedItemChanged += lbpregled1_SelectedItemChanged;

        }
        private static TreeViewItem CreateDirectory(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeViewItem { Header = directoryInfo.Name };
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Items.Add(CreateDirectory(directory));

            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Items.Add(new TreeViewItem { Header = file.Name });

            return directoryNode;

        }
        private void ListDirectory(System.Windows.Controls.TreeView treeView, string path)
        {
            treeView.Items.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
           treeView.Items.Add(CreateDirectory(rootDirectoryInfo));
        }

        private void Dir1_Load(string path, System.Windows.Controls.TreeView Tree)
        {

            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] directories =
            di.GetDirectories("*", SearchOption.AllDirectories);
            TreeViewItem rootDir = null;
            rootDir = new TreeViewItem();
            rootDir.Header = di.Name;
            foreach (DirectoryInfo dir in directories)
            {
                TreeViewItem treeItem = null;
                treeItem = new TreeViewItem();
                treeItem.Header = dir;

                foreach (var fi in dir.GetFiles())

                    treeItem.Items.Add(new TreeViewItem() { Header = fi });
                rootDir.Items.Add(treeItem);

            }

            Tree.Items.Add(rootDir);
        }

        private void Lbpregled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string file = lbpregled1.SelectedItem.ToString();
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string folderName = System.IO.Path.Combine(projectPath, "KompletnaDokumentacija");
            string fn = System.IO.Path.Combine(folderName, file);
            System.Diagnostics.Process.Start(fn);
            this.Close();
        }
      //  string path1 = @"C:\Users\Dusko Marinkovic\Desktop\KompletnaDokumentacija";
        private void lbpregled1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            string file = lbpregled1.SelectedItem.ToString();
            string path1 = @"C:\Users\Dusko Marinkovic\Desktop\KompletnaDokumentacija";
            //string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            //string folderName = System.IO.Path.Combine(projectPath, "KompletnaDokumentacija");
            //string fn = System.IO.Path.Combine(path1, file);
            //string fn = @"C:\Users\Dusko Marinkovic\Desktop\KompletnaDokumentacija";
            System.Diagnostics.Process.Start(path1);
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string path1 = @"C:\Users\Dusko Marinkovic\Desktop\KompletnaDokumentacija";

           ListDirectory(lbpregled1, path1);
            
        }
    }
}
