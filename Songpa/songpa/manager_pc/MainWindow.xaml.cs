using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace manager_pc
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckFolderStructure();
        }

        private void btn_PC1_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new RushTicketList();
            newWindow.Show();

            this.Close();
        }

        private void btn_SetFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_RootPath.Text = dialog.SelectedPath.ToString() + "\\server_root";

                DirectoryInfo di_ROOT = new DirectoryInfo(txt_RootPath.Text);  //Create Directoryinfo value by sDirPath

                if (di_ROOT.Exists == false)   //If New Folder not exits
                    di_ROOT.Create();             //create Folder

                DirectoryInfo di_SUB1 = new DirectoryInfo(txt_RootPath.Text+"\\rush_ticket_root");
                //DirectoryInfo di_SUB2 = new DirectoryInfo(txt_RootPath.Text+"\\sub2");

                if (di_SUB1.Exists == false)
                    di_SUB1.Create();       
                //if (di_SUB2.Exists == false)
                  //  di_SUB2.Create();       

                MessageBox.Show("Initialize successful!");
            }
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txt_RootPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFolderStructure();

        }

        private void CheckFolderStructure()
        {
            if (txt_RootPath.Text == "")
            {
                btn_PC1.IsEnabled = false;
                btn_PC2.IsEnabled = false;
                btn_PC3.IsEnabled = false;
            }
            else
            {
                btn_PC1.IsEnabled = true;
                btn_PC2.IsEnabled = true;
                btn_PC3.IsEnabled = true;
            }
        }
    }
}
