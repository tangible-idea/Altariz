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

namespace songpa
{
    /// <summary>
    /// Connection.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Connection : Window
    {
        public Connection()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //String strPath1 = @"\\192.168.0.14\all";
            String strPath = "\\\\" + txt_IP.Text + "\\" + txt_Path.Text;


            SharedAPI api = new SharedAPI();
            int nRes = api.ConnectRemoteServer(strPath, txt_Account.Text, txt_PW.Password);

            String msg = "Connection failed.";
            if (nRes == 0)
            {
                msg = "Connection sucessfull!";
            }
            MessageBox.Show(msg);

            if (msg == "Connection sucessfull!")
            {
                var newWindow = new MainWindow();
                newWindow.Show();
            }

            
        }
    }
}
