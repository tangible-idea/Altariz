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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageRefresh();

            string text = System.IO.File.ReadAllText(strPath + "\\text1.txt");
            MessageBox.Show(text);

            StartTimer();
        }


        public String strPath = @"\\192.168.0.14\all";


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //SharedAPI api = new SharedAPI();
            //int nRes= api.ConnectRemoteServer(strPath);

            //String str= nRes.ToString();
            //MessageBox.Show(str);


            ImageRefresh();


            //string text = System.IO.File.ReadAllText(@"\\192.168.0.14\all\");
        }

        private void ImageRefresh()
        {
            MessageBox.Show("refresh!");

            BitmapImage img = new BitmapImage(new Uri(strPath + "\\sample.jpg"));
            image1.Source = img;
        }


        private void StartTimer()
        {

          System.Windows.Threading.DispatcherTimer TimerClock = new System.Windows.Threading.DispatcherTimer();
          //TimerClock.Interval = new TimeSpan ( 0, 0, 0, 0, 200 ); // 200 milliseconds
          TimerClock.Interval= TimeSpan.FromSeconds(30);

          TimerClock.IsEnabled = true;

          TimerClock.Tick += new EventHandler(TimerClock_Tick);

        }



        void TimerClock_Tick(object sender, EventArgs e)
        {
            ImageRefresh();
        }


    }
}
