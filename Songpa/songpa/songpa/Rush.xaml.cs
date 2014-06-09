﻿using System;
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

            //string text = System.IO.File.ReadAllText(strPath + "\\contents.txt");
            //MessageBox.Show(text);

            StartTimer();
        }


        //public String strPath = @"\\192.168.0.14\all";
        public String strPath = Connection.PATH;


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
            //MessageBox.Show("refresh!");

            // rush ticket root folder [6/5/2014 Mark]
            System.IO.DirectoryInfo DIR_rush_root = new System.IO.DirectoryInfo(strPath + "\\rush_ticket_root\\");

            System.IO.DirectoryInfo[] RushRootInfo = DIR_rush_root.GetDirectories("*.*");
            foreach (System.IO.DirectoryInfo d in RushRootInfo)
            {
                //MessageBox.Show("rush_root : " + d.FullName);
                System.IO.DirectoryInfo DIR_rush_image1 = new System.IO.DirectoryInfo(d.FullName+"\\image1");
                System.IO.FileInfo[] FILE_image1= DIR_rush_image1.GetFiles();
                BitmapImage img1 = new BitmapImage(new Uri(FILE_image1[0].FullName));
                img_Rush1.Source = img1;

                System.IO.DirectoryInfo DIR_rush_image2 = new System.IO.DirectoryInfo(d.FullName + "\\image2");
                System.IO.FileInfo[] FILE_image2 = DIR_rush_image2.GetFiles();
                BitmapImage img2 = new BitmapImage(new Uri(FILE_image2[0].FullName));
                img_Rush2.Source = img2;

                string text = System.IO.File.ReadAllText(d.FullName + "\\contents\\1.txt");
                txt_Title.Content = text;
            }

            

            //BitmapImage img = new BitmapImage(new Uri(strPath + "\\rush_ticket_root\\image1\\1.jpg"));
            //image1.Source = img;
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

        private void btn_Left_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Right_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}