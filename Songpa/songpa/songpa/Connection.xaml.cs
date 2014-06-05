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

namespace songpa
{
    /// <summary>
    /// Connection.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Connection : Window
    {
        static public String PATH = "";
        RegistryKey rkey;

        public Connection()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Registry.CurrentUser.CreateSubKey("SONGPA").CreateSubKey("connection");
            rkey = Registry.CurrentUser.OpenSubKey("SONGPA").OpenSubKey("connection", true);

            txt_IP.Text = rkey.GetValue("IP").ToString();
            txt_Account.Text = rkey.GetValue("ACCOUNT").ToString();
            txt_PW.Password = rkey.GetValue("PASSWORD").ToString();
            txt_Path.Text = rkey.GetValue("PATH").ToString();


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
                rkey.SetValue("IP", txt_IP.Text.ToString());
                rkey.SetValue("ACCOUNT", txt_Account.Text.ToString());
                rkey.SetValue("PASSWORD", txt_PW.Password.ToString());
                rkey.SetValue("PATH", txt_Path.Text.ToString());

                var newWindow = new MainWindow();
                newWindow.Show();
            }

            
        }

    }
}
