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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace songpa
{
    /// <summary>
    /// Connection.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Connection : Window
    {
        static public String PATH = "";
        static public String PATHlocal = Directory.GetCurrentDirectory();
        RegistryKey rkey;

        public Connection()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbo_connection_target.Items.Clear();
            cbo_connection_target.Items.Add("Rush ticket");
            cbo_connection_target.Items.Add("Concert & Festival");
            cbo_connection_target.Items.Add("History & culture");

            // 레지스트리 값 읽어오기 [6/5/2014 Mark]
            Registry.CurrentUser.CreateSubKey("SONGPA").CreateSubKey("connection");
            rkey = Registry.CurrentUser.OpenSubKey("SONGPA").OpenSubKey("connection", true);

            if (rkey.GetValue("IP") != null)    // 값이 있으면 [6/25/2014 Mark]
            {
                txt_IP.Text = rkey.GetValue("IP").ToString();
                txt_Account.Text = rkey.GetValue("ACCOUNT").ToString();
                txt_PW.Password = rkey.GetValue("PASSWORD").ToString();
                txt_Path.Text = rkey.GetValue("PATH").ToString();
            }            


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //String strPath1 = @"\\192.168.0.14\all";
            PATH = "\\\\" + txt_IP.Text + "\\" + txt_Path.Text;


            SharedAPI api = new SharedAPI();
            int nRes = api.ConnectRemoteServer(PATH, txt_Account.Text, txt_PW.Password);

            String msg = "Connection failed.";
            if (nRes == 0)
            {
                msg = "Connection sucessfull!";
            }
            MessageBox.Show(msg);

            if (msg == "Connection sucessfull!")
            {
                CopyFolder(PATH, PATHlocal);

                // 접속 성공시 해당 값으로 접속정보를 레지스트리에 쓴다. [6/5/2014 Mark]
                rkey.SetValue("IP", txt_IP.Text.ToString());
                rkey.SetValue("ACCOUNT", txt_Account.Text.ToString());
                rkey.SetValue("PASSWORD", txt_PW.Password.ToString());
                rkey.SetValue("PATH", txt_Path.Text.ToString());

                if (cbo_connection_target.SelectedIndex == 0)
                {
                    var newWindow = new Rush();
                    newWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong connection target or in construction page.");
                }
                

                
            }

            
        }

        // 원본과, 목적지를 같이 대입  
        public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(destFolder, name);
                File.Copy(file, dest, true);
            }

            // foreach 안에서 재귀 함수를 통해서 폴더 복사 및 파일 복사 진행 완료  
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }  

    }
}
