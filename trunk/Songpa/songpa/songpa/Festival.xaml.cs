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
using System.Xml;
using System.IO;
using System.Collections;

namespace songpa
{

    /// <summary>
    /// Festival.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Festival : Window
    {
        List<FestivalInfo> arrInfo = new List<FestivalInfo>();  // xml 모두 읽어서 정보 담아둘 곳 [7/1/2014 Mark]
        public String strPath = Connection.PATHlocal;   // 루트 경로 [7/1/2014 Mark]
        
        int nFestivalCount = 0, nMusicalCount = 0;
        int nCurrentTap = 0;

        public Festival()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BoardRefresh();
        }

        private void BoardRefresh()
        {
            //MessageBox.Show("refresh!");

            // rush ticket root folder [6/5/2014 Mark]
            DirectoryInfo DIR_festival_root = new DirectoryInfo(strPath + "\\festival_root\\");
            DirectoryInfo[] RushFestivalInfo = DIR_festival_root.GetDirectories("*.*");
            //int nPageCount = RushFestivalInfo.Length;

            //if (nCurrentPage + 1 >= nPageCount)
            //    btn_Right.IsEnabled = false;
            //else
            //    btn_Right.IsEnabled = true;

            //if (nCurrentPage == 0)
            //    btn_Left.IsEnabled = false;
            //else
            //    btn_Left.IsEnabled = true;

            // 탭 정리를 위해 전부 한꺼번에 가져오자 [7/1/2014 Mark]
            for (int i = 0; i < RushFestivalInfo.Length; ++i )
            {
                XMLLoad(RushFestivalInfo[i].FullName);
            }
            
            
            foreach (FestivalInfo FI in arrInfo)
            {
                if (FI.nCategory == 0)
                    ++nFestivalCount;
                else if (FI.nCategory == 1)
                    ++nMusicalCount;
            }

        }


        private bool XMLLoad(String pathToLoad)
        {
            XmlDocument XmlDoc = new XmlDocument();
            FestivalInfo info = new FestivalInfo();

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
                    case "tabs":
                        info.nCategory = Int32.Parse(node.InnerText);
                        //cbo_SelPosition.SelectedIndex = Int32.Parse(node.InnerText);
                        break;
                    case "txt1":
                        info.txt1 = node.InnerText;
                        break;
                    case "txt2":
                        info.txt2 = node.InnerText;
                        break;
                    case "txt3":
                        info.txt3 = node.InnerText;
                        break;
                    case "txt4":
                        info.txt4 = node.InnerText;
                        break;
                    case "image1":
                        {
                            if (node.InnerText.Trim() == "")
                                break;

                            String pathImage = pathToLoad + "\\" + node.InnerText;
                            info.img1_path = pathImage;
                        }
                        //    BitmapImage bitmap = new BitmapImage(new Uri(pathImage));
                        //    if (bitmap != null)
                        //        img_Form1.Source = bitmap;
                        //}
                        break;
                    case "image2":
                        {
                            if (node.InnerText.Trim() == "")
                                break;

                            String pathImage = pathToLoad + "\\" + node.InnerText;
                            info.img2_path = pathImage;
                        }
                        break;
                    case "image3":
                        {
                            if (node.InnerText.Trim() == "")
                                break;

                            String pathImage = pathToLoad + "\\" + node.InnerText;
                            info.img3_path = pathImage;
                        }
                        break;
                    case "img_thumb":
                        {
                            if (node.InnerText.Trim() == "")
                                break;

                            String pathImage = pathToLoad + "\\" + node.InnerText;
                            info.imgThumb_path = pathImage;
                        }
                        break;
                }
            }

            if (info.txt1.Trim() != "")
                arrInfo.Add(info);  // 비어있는 정보가 아니면 하나씩 저장 [7/1/2014 Mark]

            return true;
        }

        private void onClickTap1(object sender, MouseButtonEventArgs e)
        {
            if (nCurrentTap == 0)
                return;

        }

        private void onClickTap2(object sender, MouseButtonEventArgs e)
        {
            if (nCurrentTap == 1)
                return;
        }


    }

    class FestivalInfo
    {
        public int nCategory;  // festival or musical

        public String img1_path;
        public String img2_path;
        public String img3_path;
        public String imgThumb_path;

        public String txt1;
        public String txt2;
        public String txt3;
        public String txt4;
    };
}
