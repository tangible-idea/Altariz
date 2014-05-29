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

namespace songpa
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public String strPath = @"\\192.168.0.14\all";


        private void button1_Click(object sender, RoutedEventArgs e)
        {


            //SharedAPI api = new SharedAPI();
            //int nRes= api.ConnectRemoteServer(strPath);

            //String str= nRes.ToString();
            //MessageBox.Show(str);

            
            BitmapImage img = new BitmapImage(new Uri(strPath+"\\sample.jpg"));
            image1.Source = img;

            //string text = System.IO.File.ReadAllText(@"\\192.168.0.14\all\");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage img = new BitmapImage(new Uri(strPath + "\\sample.jpg"));
            image1.Source = img;

            string text = System.IO.File.ReadAllText(strPath + "\\text1.txt");
            MessageBox.Show(text);
        }
    }
}
