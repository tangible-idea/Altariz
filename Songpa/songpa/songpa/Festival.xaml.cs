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
using System.Reflection;

namespace songpa
{

    /// <summary>
    /// Festival.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Festival : Window
    {
        List<FestivalInfo> arrFestivalInfo = new List<FestivalInfo>();  // xml 모두 읽어서 정보 담아둘 곳 [7/1/2014 Mark]
        List<FestivalInfo> arrMusicalInfo = new List<FestivalInfo>();  // xml 모두 읽어서 정보 담아둘 곳 [7/1/2014 Mark]
        public String strPath = Connection.PATHlocal;   // 루트 경로 [7/1/2014 Mark]
        
        //int nFestivalCount = 0, nMusicalCount = 0;  // 각 항목 개수 [7/2/2014 Mark]
        int nFestivalCurrPage = 0, nMusicalCurrPage = 0;    // 각 탭의 현재 페이지 [7/2/2014 Mark]
        int nCurrentTap = 0;
        BitmapImage img_Tap1_off;
        BitmapImage img_Tap1_on;
        BitmapImage img_Tap2_off;
        BitmapImage img_Tap2_on;

        const int TAP_FESTIVAL=0;
        const int TAP_MUSICAL = 1;

        public Festival()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            string filename1 = string.Format("{0}{1}", this.GetType().Namespace, ".res.tap_festival_off.jpg");
            string filename2 = string.Format("{0}{1}", this.GetType().Namespace, ".res.tap_festival_on.jpg");
            string filename3 = string.Format("{0}{1}", this.GetType().Namespace, ".res.tap_musical_off.jpg");
            string filename4 = string.Format("{0}{1}", this.GetType().Namespace, ".res.tap_musical_on.jpg");

            img_Tap1_off= MakeBitmap(executingAssembly, filename1);
            img_Tap1_on= MakeBitmap(executingAssembly, filename2);
            img_Tap2_off= MakeBitmap(executingAssembly, filename3);
            img_Tap2_on= MakeBitmap(executingAssembly, filename4);

            image_tap1.Source = img_Tap1_on;
            image_tap2.Source = img_Tap2_off;

            BoardRefresh();
        }

        private BitmapImage MakeBitmap( Assembly executingAssembly, string filename)
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
        
        private void BoardRefresh()
        {
            arrFestivalInfo.Clear();
            arrMusicalInfo.Clear();
            //MessageBox.Show("refresh!");

            // rush ticket root folder [6/5/2014 Mark]
            DirectoryInfo DIR_festival_root = new DirectoryInfo(strPath + "\\festival_root\\");
            DirectoryInfo[] RushFestivalInfo = DIR_festival_root.GetDirectories("*.*");
            //int nPageCount = RushFestivalInfo.Length;

            // 탭 정리를 위해 전부 한꺼번에 가져오자 [7/1/2014 Mark]
            for (int i = 0; i < RushFestivalInfo.Length; ++i )
            {
                XMLLoad(RushFestivalInfo[i].FullName);
            }
            
            
            foreach (FestivalInfo FI in arrFestivalInfo)
            {
                
            }

            String path1="", path2="", path3="", path4="", path5="", path6="";
            try
            {
                if (nCurrentTap == TAP_FESTIVAL)
                {
                    int nFestivalMaxPage = (int)arrFestivalInfo.Count / 6;
                    if (nFestivalCurrPage + 1 > nFestivalMaxPage)
                        btn_Right.IsEnabled = false;
                    else
                        btn_Right.IsEnabled = true;

                    if (nFestivalCurrPage == 0)
                        btn_Left.IsEnabled = false;
                    else
                        btn_Left.IsEnabled = true;

                    int idxStart = nFestivalCurrPage * 6;
                    path1= arrFestivalInfo[idxStart + 0].imgThumb_path;
                    path2= arrFestivalInfo[idxStart + 1].imgThumb_path;
                    path3= arrFestivalInfo[idxStart + 2].imgThumb_path;
                    path4= arrFestivalInfo[idxStart + 3].imgThumb_path;
                    path5= arrFestivalInfo[idxStart + 4].imgThumb_path;
                    path6= arrFestivalInfo[idxStart + 5].imgThumb_path;
                }
                if (nCurrentTap == TAP_MUSICAL)
                {
                    int nMusicalMaxPage = (int)arrMusicalInfo.Count / 6;
                    if (nMusicalCurrPage + 1 > nMusicalMaxPage)
                        btn_Right.IsEnabled = false;
                    else
                        btn_Right.IsEnabled = true;

                    if (nMusicalCurrPage == 0)
                        btn_Left.IsEnabled = false;
                    else
                        btn_Left.IsEnabled = true;

                    int idxStart = nFestivalCurrPage * 6;
                    path1 = arrMusicalInfo[idxStart + 0].imgThumb_path;
                    path2 = arrMusicalInfo[idxStart + 1].imgThumb_path;
                    path3 = arrMusicalInfo[idxStart + 2].imgThumb_path;
                    path4 = arrMusicalInfo[idxStart + 3].imgThumb_path;
                    path5 = arrMusicalInfo[idxStart + 4].imgThumb_path;
                    path6 = arrMusicalInfo[idxStart + 5].imgThumb_path;
                }
            }
            catch
            {
            }


            try
            {
                if (path1 == "")
                    image1.Source = null;
                else
                    image1.Source = new BitmapImage(new Uri(path1));

                if (path2 == "")
                    image2.Source = null;
                else
                    image2.Source = new BitmapImage(new Uri(path2));

                if (path3 == "")
                    image3.Source = null;
                else
                    image3.Source = new BitmapImage(new Uri(path3));

                if (path4 == "")
                    image4.Source = null;
                else
                    image4.Source = new BitmapImage(new Uri(path4));

                if (path5 == "")
                    image5.Source = null;
                else
                    image5.Source = new BitmapImage(new Uri(path5));

                if (path6 == "")
                    image6.Source = null;
                else
                    image6.Source = new BitmapImage(new Uri(path6));
            }
            catch
            {

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

            if (info.txt1.Trim() != "") // 비어있는 정보가 아니면 하나씩 저장 [7/1/2014 Mark]
            {
                if (info.nCategory == TAP_FESTIVAL)
                    arrFestivalInfo.Add(info);
                else
                    arrMusicalInfo.Add(info);
            }
            return true;
        }

        private void onClickTap1(object sender, MouseButtonEventArgs e)
        {
            if (nCurrentTap == 0)
                return;
            
            image_tap1.Source = img_Tap1_on;
            image_tap2.Source = img_Tap2_off;
            nCurrentTap = 0;

            BoardRefresh();
        }

        private void onClickTap2(object sender, MouseButtonEventArgs e)
        {
            if (nCurrentTap == 1)
                return;

            image_tap1.Source = img_Tap1_off;
            image_tap2.Source = img_Tap2_on;
            nCurrentTap = 1;

            BoardRefresh();
        }

        private void btn_Left_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Right_Click(object sender, RoutedEventArgs e)
        {

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
