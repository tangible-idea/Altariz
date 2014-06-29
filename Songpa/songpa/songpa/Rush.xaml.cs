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
using System.Xml;

namespace songpa
{
    public partial class MainWindow : Window
    {
        int nCurrentPage = 0;   // 현재 표시 중인 페이지 [6/9/2014 Mark]
        int nPageCount = 0; // 페이지 개수 [6/9/2014 Mark]

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BoardRefresh();
                        
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


            BoardRefresh();


            //string text = System.IO.File.ReadAllText(@"\\192.168.0.14\all\");
        }

        private void BoardRefresh()
        {
            //MessageBox.Show("refresh!");

            // rush ticket root folder [6/5/2014 Mark]
            DirectoryInfo DIR_rush_root = new DirectoryInfo(strPath + "\\rush_ticket_root\\");
            DirectoryInfo[] RushRootInfo = DIR_rush_root.GetDirectories("*.*");
            nPageCount = RushRootInfo.Length;

            if (nCurrentPage + 1 >= nPageCount)
                btn_Right.IsEnabled = false;
            else
                btn_Right.IsEnabled = true;

            if (nCurrentPage == 0)
                btn_Left.IsEnabled = false;
            else
                btn_Left.IsEnabled = true;

            XMLLoad(RushRootInfo[nCurrentPage].FullName);

            
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
            BoardRefresh();
        }

        private void btn_Left_Click(object sender, RoutedEventArgs e)
        {
            nCurrentPage--;
            BoardRefresh();
        }

        private void btn_Right_Click(object sender, RoutedEventArgs e)
        {
            nCurrentPage++;
            BoardRefresh();
        }



        private bool XMLLoad(String pathToLoad)
        {
            XmlDocument XmlDoc = new XmlDocument();

            try
            {
                XmlDoc.Load(pathToLoad + "\\1.xml");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("저장된 파일의 형식이 올바르지 않습니다.\n정보를 정상적으로 불러오지 못했습니다.");
                return false;
            }

            XmlNode root = XmlDoc.DocumentElement;
            XmlNodeList list = root.ChildNodes;

            foreach (XmlNode node in list)
            {
                switch (node.Name)
                {
                    case "title_eng":
                        txt_EngTitle.Content = node.InnerText;
                        break;
                    case "title_kor":
                        txt_KorTitle.Content = node.InnerText;
                        break;
                    case "pos_eng":
                        txt_EngPlace.Content = node.InnerText;
                        break;
                    case "pos_kor":
                        txt_KorPlace.Content = node.InnerText;
                        break;
                    case "period_eng":
                        txt_EngPeriod.Content = node.InnerText;
                        break;
                    case "period_kor":
                        txt_KorPeriod.Content = node.InnerText;
                        break;
                    case "time_eng":
                        txt_EngTime.Content = node.InnerText;
                        break;
                    case "time_kor":
                        txt_KorTime.Content = node.InnerText;
                        break;
                    case "price_eng":
                        txt_EngPrices.Content = node.InnerText;
                        break;
                    case "price_kor":
                        txt_KorPrices.Content = node.InnerText;
                        break;
                    case "contact":
                        txt_Contact.Content = node.InnerText;
                        break;
                    case "image1":
                        {
                            if (node.InnerText.Trim() == "")
                            {
                                img_Rush1.Source = null;
                                break;
                            }

                            String pathImage = pathToLoad +"\\"+node.InnerText;

                            if (!File.Exists(pathImage))
                            {
                                MessageBox.Show("이미지 경로가 잘못되어 불러올 수 없습니다.");
                                return false;
                            }

                            BitmapImage bitmap = new BitmapImage(new Uri(pathImage));
                            if (bitmap != null)
                                img_Rush1.Source = bitmap;
                        }
                        break;
                }
            }
            return true;
        }


    }
}
