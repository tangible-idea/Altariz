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
using Microsoft.Win32;
using System.Management;
using System.Net;

namespace manager_pc
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        RegistryKey rkey;

        public MainWindow()
        {
            InitializeComponent();

            // 레지스트리 값 읽어오기 [6/5/2014 Mark]
            Registry.CurrentUser.CreateSubKey("SONGPA").CreateSubKey("root_info");
            rkey = Registry.CurrentUser.OpenSubKey("SONGPA").OpenSubKey("root_info", true);

            if (rkey.GetValue("PATH") != null)
            {
                txt_RootPath.Text = rkey.GetValue("PATH").ToString();
            }
            CheckFolderStructure(); // 각 폴더 구조 만들기

            Title = getMyIP();  // window title을 ip로 변경. [6/10/2014 Mark]
        }

        private void btn_PC1_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new RushTicketList();
            newWindow.Show();

            this.Close();
        }

        private void btn_PC2_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new FestivalList();
            newWindow.Show();

            this.Close();
        }


        private void btn_PC3_Click_1(object sender, RoutedEventArgs e)
        {
            var newWindow = new HistoryList();
            newWindow.Show();

            this.Close();
        }

        private void btn_PC3_Click(object sender, RoutedEventArgs e)
        {
            //var newWindow = new RushTicketList();
            //newWindow.Show();

            //this.Close();
        }


        private void btn_SetFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_RootPath.Text = dialog.SelectedPath.ToString() + "\\server_root";

                DirectoryInfo di_ROOT = new DirectoryInfo(txt_RootPath.Text);  //Create Directoryinfo value by sDirPath

                ShareFolderPermission(di_ROOT.FullName);    // 공유 폴더로 만든다. [6/9/2014 Mark]

                if (di_ROOT.Exists == false)   //If New Folder not exits
                    di_ROOT.Create();             //create Folder
                CreateFolderStructure();    // 각 서브 폴더 구조 만들기 [7/18/2014 Mark_laptap]


                rkey.SetValue("PATH", txt_RootPath.Text.ToString());
                MessageBox.Show("Initialize successful!");
            }
        }


        private void ShareFolderPermission(String pathFolder)
        {
            String folderName = pathFolder.Substring(pathFolder.LastIndexOf("\\") + 1);
            try
            {
                ManagementClass managementClass = new ManagementClass("Win32_Share");
                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");
                ManagementBaseObject outParams;
                inParams["Description"] = "";
                inParams["Name"] = folderName;
                inParams["Path"] = pathFolder;
                inParams["Type"] = 0x0;

                outParams = managementClass.InvokeMethod("Create", inParams, null);

                if ((uint)(outParams.Properties["ReturnValue"].Value) != 0)
                    MessageBox.Show("Folder might be already in share or unable to share the directory");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        // 각 서브 폴더 구조 만들기
        private void CreateFolderStructure()
        {

            DirectoryInfo di_SUB1 = new DirectoryInfo(txt_RootPath.Text + "\\rush_ticket_root");
            DirectoryInfo di_SUB2 = new DirectoryInfo(txt_RootPath.Text + "\\festival_root");
            DirectoryInfo di_SUB3 = new DirectoryInfo(txt_RootPath.Text + "\\history_root");

            if (di_SUB1.Exists == false)
                di_SUB1.Create();
            if (di_SUB2.Exists == false)
                di_SUB2.Create();
            if (di_SUB3.Exists == false)
                di_SUB3.Create();
        }

        // 각 폴더 체크하기
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

        public string getMyIP()
        {
            IPHostEntry host = Dns.GetHostByName(Dns.GetHostName());
            string myip = host.AddressList[0].ToString();
            return myip;
        }

    }
}
