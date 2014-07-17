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
using System.Reflection;

namespace songpa
{
    public partial class Rush : Window
    {
        int nCurrentPage = 0;   // 현재 표시 중인 페이지 [6/9/2014 Mark]
        int nPageCount = 0; // 페이지 개수 [6/9/2014 Mark]
        BitmapImage imgRushTap1;
        BitmapImage imgRushTap2;
        BitmapImage imgRushTap3;
        BitmapImage imgRushTap4;
        BitmapImage imgRushTap5;
        BitmapImage imgRushTap6;

        public Rush()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            string filename1 = string.Format("{0}{1}", this.GetType().Namespace, ".res.rush_tag_01.gif");
            string filename2 = string.Format("{0}{1}", this.GetType().Namespace, ".res.rush_tag_02.gif");
            string filename3 = string.Format("{0}{1}", this.GetType().Namespace, ".res.rush_tag_03.gif");
            string filename4 = string.Format("{0}{1}", this.GetType().Namespace, ".res.rush_tag_04.gif");
            string filename5 = string.Format("{0}{1}", this.GetType().Namespace, ".res.rush_tag_05.gif");
            string filename6 = string.Format("{0}{1}", this.GetType().Namespace, ".res.rush_tag_06.gif");

            imgRushTap1 = MakeBitmap(executingAssembly, filename1);
            imgRushTap2 = MakeBitmap(executingAssembly, filename2);
            imgRushTap3 = MakeBitmap(executingAssembly, filename3);
            imgRushTap4 = MakeBitmap(executingAssembly, filename4);
            imgRushTap5 = MakeBitmap(executingAssembly, filename5);
            imgRushTap6 = MakeBitmap(executingAssembly, filename6);
            
            BoardRefresh();
                        
            StartTimer();
        }


        private BitmapImage MakeBitmap(Assembly executingAssembly, string filename)
        {
            BitmapImage item = new BitmapImage();
            item.BeginInit();
            item.StreamSource = executingAssembly.GetManifestResourceStream(filename);
            item.CacheOption = BitmapCacheOption.OnLoad;
            item.CreateOptions = BitmapCreateOptions.None;
            item.EndInit();
            item.Freeze();

            return item;
        }


        //public String strPath = @"\\192.168.0.14\all";
        //public String strPath = Connection.PATH;
        public String strPath = Connection.PATHlocal;


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
                    case "image2":
                        {
                            string image2= node.InnerText.Trim();
                            if (image2 == "-1")  // -1 이면 아무것도 선택 안된 것임. [7/8/2014 Mark]
                            {
                                img_Rush2.Source = null;
                                break;
                            }

                            switch (image2)
                            {
                                case "0":
                                    img_Rush2.Source = imgRushTap1;
                                    break;
                                case "1":
                                    img_Rush2.Source = imgRushTap2;
                                    break;
                                case "2":
                                    img_Rush2.Source = imgRushTap3;
                                    break;
                                case "3":
                                    img_Rush2.Source = imgRushTap4;
                                    break;
                                case "4":
                                    img_Rush2.Source = imgRushTap5;
                                    break;
                                case "5":
                                    img_Rush2.Source = imgRushTap6;
                                    break;
                            }
                        }
                        break;
                }
            }
            return true;
        }

        // 테스트용 좌우 키 이벤트 추가. [7/17/2014 Mark_laptap]
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                if (btn_Right.IsEnabled)
                {
                    nCurrentPage++;
                    BoardRefresh();
                }
                
            }
            else if (e.Key == Key.Left)
            {
                if (btn_Left.IsEnabled)
                {
                    nCurrentPage--;
                    BoardRefresh();
                }
            }
        }


    }
}
