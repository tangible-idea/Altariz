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
    /// History.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class History : Window
    {
        List<HistoryInfo> arrHistoryInfo = new List<HistoryInfo>();  // xml 모두 읽어서 정보 담아둘 곳 [7/1/2014 Mark]
        List<HistoryInfo> arrCultureInfo = new List<HistoryInfo>();  // xml 모두 읽어서 정보 담아둘 곳 [7/1/2014 Mark]
        public String strPath = Connection.PATHlocal;   // 루트 경로 [7/1/2014 Mark]
        
        int nHistoryCurrPage = 0, nCultureCurrPage = 0;    // 각 탭의 현재 페이지 [7/2/2014 Mark]
        int nCurrentTap = 0;    // 현재 선택한 탭. [7/1/2014 Mark]
        int nCurrSel = 0;       // 현재 선택한 이미지 [7/2/2014 Mark]
        BitmapImage img_Tap1_off;  
        BitmapImage img_Tap1_on;
        BitmapImage img_Tap2_off;
        BitmapImage img_Tap2_on;        // 이미지들 [7/1/2014 Mark]

        const int TAP_HISTORY= 0;      // enumulation [7/1/2014 Mark]
        const int TAP_CULTURE = 1;
        const int ICON_COUNT_EACH_PAGE = 6;

        Festival2 topWindow;    // 상단 윈도우 [7/2/2014 Mark]

        public History()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            topWindow = new Festival2();    // 상단 윈도우 출력 [7/2/2014 Mark]
            topWindow.Show();

            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            string filename1 = string.Format("{0}{1}", this.GetType().Namespace, ".res.tap_remains2.jpg");
            string filename2 = string.Format("{0}{1}", this.GetType().Namespace, ".res.tap_remains1.jpg");
            string filename3 = string.Format("{0}{1}", this.GetType().Namespace, ".res.tap_relic2.jpg");
            string filename4 = string.Format("{0}{1}", this.GetType().Namespace, ".res.tap_relic1.jpg");

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



        // 각 위치에 삽입함. [7/24/2014 Mark]
        private String InsertCRLFinside(String strOriginal)
        {
            char[] split = { ' ' };
            String[] words = strOriginal.Split(split);
            String strRes = "";

            for (int i = 0; i < words.Length; ++i)
            {
                if (i == words.Length - 1)
                {
                    strRes += words[i];
                }
                else
                {
                    strRes += words[i];
                    strRes += "\r\n";
                }
            }

            return strRes;
        }


        private void BoardRefresh()
        {
            arrHistoryInfo.Clear();
            arrCultureInfo.Clear();
            //MessageBox.Show("refresh!");

            try
            {
                // rush ticket root folder [6/5/2014 Mark]
                DirectoryInfo DIR_history_root = new DirectoryInfo(strPath + "\\history_root\\");
                DirectoryInfo[] DirInfoArr_HistoryInfo = DIR_history_root.GetDirectories("*.*");
                //int nPageCount = RushFestivalInfo.Length;

                // 탭 정리를 위해 전부 한꺼번에 가져오자 [7/1/2014 Mark]
                for (int i = 0; i < DirInfoArr_HistoryInfo.Length; ++i)
                {
                    XMLLoad(DirInfoArr_HistoryInfo[i].FullName);
                }
            }
            catch
            {
                MessageBox.Show("데이터를 찾을 수 없습니다. 서버 PC에서 폴더 세팅을 확인해주세요.");
                this.Close();
                return;
            }

            {   // 전부 초기화 [7/27/2014 Mark_laptap]
                TB1.Text = TB2.Text = TB3.Text = TB4.Text = TB5.Text = TB6.Text = "";
                image1.Source = image2.Source = image3.Source = image4.Source = image5.Source = image6.Source = null;
                topWindow.image1.Source = topWindow.image2.Source = topWindow.image3.Source = null;

                topWindow.txt1.Content = "";
                topWindow.txt2.Content = "";
                TextRange textRange3 = new TextRange(topWindow.txt_RT_3.Document.ContentStart, topWindow.txt_RT_3.Document.ContentEnd);
                textRange3.Text = "";

                TextRange textRange4 = new TextRange(topWindow.txt_RT_4.Document.ContentStart, topWindow.txt_RT_4.Document.ContentEnd);
                textRange4.Text = "";
            }

            String path1="", path2="", path3="", path4="", path5="", path6="";
            try
            {   //   image path를 데이터로 부터 가져오는 부분.
                if (nCurrentTap == TAP_HISTORY)
                {
                    int nHistoryMaxPage = (int)arrHistoryInfo.Count / 6;
                    if (nHistoryCurrPage + 1 > nHistoryMaxPage)
                        btn_Right.IsEnabled = false;
                    else
                        btn_Right.IsEnabled = true;

                    if (nHistoryCurrPage == 0)
                        btn_Left.IsEnabled = false;
                    else
                        btn_Left.IsEnabled = true;

                    int idxStart = nHistoryCurrPage * 6;
                    path1= arrHistoryInfo[idxStart + 0].imgThumb_path;
                    path2= arrHistoryInfo[idxStart + 1].imgThumb_path;
                    path3= arrHistoryInfo[idxStart + 2].imgThumb_path;
                    path4= arrHistoryInfo[idxStart + 3].imgThumb_path;
                    path5= arrHistoryInfo[idxStart + 4].imgThumb_path;
                    path6= arrHistoryInfo[idxStart + 5].imgThumb_path;
                }
                if (nCurrentTap == TAP_CULTURE)
                {
                    int nCultureMaxPage = (int)arrCultureInfo.Count / 6;
                    if (nCultureCurrPage + 1 > nCultureMaxPage)
                        btn_Right.IsEnabled = false;
                    else
                        btn_Right.IsEnabled = true;

                    if (nCultureCurrPage == 0)
                        btn_Left.IsEnabled = false;
                    else
                        btn_Left.IsEnabled = true;

                    int idxStart = nCultureCurrPage * 6;
                    path1 = arrCultureInfo[idxStart + 0].imgThumb_path;
                    path2 = arrCultureInfo[idxStart + 1].imgThumb_path;
                    path3 = arrCultureInfo[idxStart + 2].imgThumb_path;
                    path4 = arrCultureInfo[idxStart + 3].imgThumb_path;
                    path5 = arrCultureInfo[idxStart + 4].imgThumb_path;
                    path6 = arrCultureInfo[idxStart + 5].imgThumb_path;
                }
            }
            catch
            {
            }

            try
            {   // 가져온 path를 이미지 컨트롤에 입력하는 부분.
                if ((path1 == null) || (path1 == ""))
                    image1.Source = null;
                else
                    image1.Source = new BitmapImage(new Uri(path1));

                if ((path2 == null) || (path2 == ""))
                    image2.Source = null;
                else
                    image2.Source = new BitmapImage(new Uri(path2));

                if ((path3 == null) || (path3 == ""))
                    image3.Source = null;
                else
                    image3.Source = new BitmapImage(new Uri(path3));

                if ((path4 == null) || (path4 == ""))
                    image4.Source = null;
                else
                    image4.Source = new BitmapImage(new Uri(path4));

                if ((path5 == null) || (path5 == ""))
                    image5.Source = null;
                else
                    image5.Source = new BitmapImage(new Uri(path5));

                if ((path6 == null) || (path6 == ""))
                    image6.Source = null;
                else
                    image6.Source = new BitmapImage(new Uri(path6));
            }
            catch
            {

            }

            try
            {   // 썸네일 타이틀 텍스트를 입력하는 부분.
                TB1.Text = TB2.Text = TB3.Text = TB4.Text = TB5.Text = TB6.Text = "";
                if (nCurrentTap == TAP_HISTORY)
                {
                    int idxStart = nHistoryCurrPage * 6;
                    TB1.Text = InsertCRLFinside(arrHistoryInfo[idxStart + 0].txt1);
                    TB2.Text = InsertCRLFinside(arrHistoryInfo[idxStart + 1].txt1);
                    TB3.Text = InsertCRLFinside(arrHistoryInfo[idxStart + 2].txt1);
                    TB4.Text = InsertCRLFinside(arrHistoryInfo[idxStart + 3].txt1);
                    TB5.Text = InsertCRLFinside(arrHistoryInfo[idxStart + 4].txt1);
                    TB6.Text = InsertCRLFinside(arrHistoryInfo[idxStart + 5].txt1);
                }
                if (nCurrentTap == TAP_CULTURE)
                {
                    int idxStart = nCultureCurrPage * 6;
                    TB1.Text = InsertCRLFinside(arrCultureInfo[idxStart + 0].txt1);
                    TB2.Text = InsertCRLFinside(arrCultureInfo[idxStart + 1].txt1);
                    TB3.Text = InsertCRLFinside(arrCultureInfo[idxStart + 2].txt1);
                    TB4.Text = InsertCRLFinside(arrCultureInfo[idxStart + 3].txt1);
                    TB5.Text = InsertCRLFinside(arrCultureInfo[idxStart + 4].txt1);
                    TB6.Text = InsertCRLFinside(arrCultureInfo[idxStart + 5].txt1);
                }
            }
            catch
            {
            }


            /// 여기서 부터 다른 페이지 (History2)
            string pathImg1 = "", pathImg2 = "", pathImg3 = "";
            try
            {
                if (nCurrentTap == TAP_HISTORY)
                {   // 선택한 것에 맞는 화면 띄움. [7/2/2014 Mark]
                    topWindow.txt1.Content = arrHistoryInfo[nCurrSel].txt1;
                    topWindow.txt2.Content = arrHistoryInfo[nCurrSel].txt2;

                    TextRange textRange3 = new TextRange(topWindow.txt_RT_3.Document.ContentStart, topWindow.txt_RT_3.Document.ContentEnd);
                    textRange3.Text= arrHistoryInfo[nCurrSel].txt3;

                    TextRange textRange4 = new TextRange(topWindow.txt_RT_4.Document.ContentStart, topWindow.txt_RT_4.Document.ContentEnd);
                    textRange4.Text = arrHistoryInfo[nCurrSel].txt4;

                    pathImg1= arrHistoryInfo[nCurrSel].img1_path;
                    pathImg2= arrHistoryInfo[nCurrSel].img2_path;
                    pathImg3= arrHistoryInfo[nCurrSel].img3_path;
                }
                else
                {
                    topWindow.txt1.Content = arrCultureInfo[nCurrSel].txt1;
                    topWindow.txt2.Content = arrCultureInfo[nCurrSel].txt2;

                    TextRange textRange3 = new TextRange(topWindow.txt_RT_3.Document.ContentStart, topWindow.txt_RT_3.Document.ContentEnd);
                    textRange3.Text = arrCultureInfo[nCurrSel].txt3;

                    TextRange textRange4 = new TextRange(topWindow.txt_RT_4.Document.ContentStart, topWindow.txt_RT_4.Document.ContentEnd);
                    textRange4.Text = arrCultureInfo[nCurrSel].txt4;

                    pathImg1 = arrCultureInfo[nCurrSel].img1_path;
                    pathImg2 = arrCultureInfo[nCurrSel].img2_path;
                    pathImg3 = arrCultureInfo[nCurrSel].img3_path;
                }
            }
            catch (System.Exception ex)
            {
            	
            }

            if(pathImg1==null || pathImg1=="")
                topWindow.image1.Source= null;
            else
                topWindow.image1.Source = new BitmapImage(new Uri(pathImg1));

            if (pathImg2 == null || pathImg2 == "")
                topWindow.image2.Source= null;
            else
                topWindow.image2.Source = new BitmapImage(new Uri(pathImg2));

            if (pathImg3 == null || pathImg3 == "")
                topWindow.image3.Source= null;
            else
                topWindow.image3.Source = new BitmapImage(new Uri(pathImg3));


        }


        private bool XMLLoad(String pathToLoad)
        {


            XmlDocument XmlDoc = new XmlDocument();
            HistoryInfo info = new HistoryInfo();

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
                if (info.nCategory == TAP_HISTORY)
                    arrHistoryInfo.Add(info);
                else
                    arrCultureInfo.Add(info);
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
            nHistoryCurrPage = 0;
            nCultureCurrPage = 0;
            Position0();  // 탭이 변경될 때, 선택이 초기화됨. [7/21/2014 Mark_laptap]

            BoardRefresh();
        }

        private void onClickTap2(object sender, MouseButtonEventArgs e)
        {
            if (nCurrentTap == 1)
                return;

            image_tap1.Source = img_Tap1_off;
            image_tap2.Source = img_Tap2_on;
            nHistoryCurrPage = 0;
            nCultureCurrPage = 0;
            nCurrentTap = 1;
            Position0();  // 탭이 변경될 때, 선택이 초기화됨. [7/21/2014 Mark_laptap]

            BoardRefresh();
        }

        private void btn_Left_Click(object sender, RoutedEventArgs e)
        {
            if (nCurrentTap == TAP_HISTORY)
                --nHistoryCurrPage;
            else
                --nCultureCurrPage;

            BoardRefresh();
        }

        private void btn_Right_Click(object sender, RoutedEventArgs e)
        {
            if (nCurrentTap == TAP_HISTORY)
                ++nHistoryCurrPage;
            else
                ++nCultureCurrPage;

            BoardRefresh();
        }


        private void onClickImage1(object sender, MouseButtonEventArgs e)
        {
            Position0();

            BoardRefresh();
        }

        private void Position0()
        {
            img_selected.Margin = new Thickness(140, 220, 0, 0);
            rct_mask.Margin = new Thickness(153, 232, 0, 0);
            if (nCurrentTap == TAP_HISTORY)
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 0;
            }
            else
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 0;
            }
        }


        private void onClickImage2(object sender, MouseButtonEventArgs e)
        {
            img_selected.Margin = new Thickness(475, 220, 0, 0);
            rct_mask.Margin = new Thickness(490, 232, 0, 0);
            if (nCurrentTap == TAP_HISTORY)
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 1;
            }
            else
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 1;
            }
            BoardRefresh();
        }

        private void onClickImage3(object sender, MouseButtonEventArgs e)
        {
            img_selected.Margin = new Thickness(815, 220, 0, 0);
            rct_mask.Margin = new Thickness(827, 232, 0, 0);
            if (nCurrentTap == TAP_HISTORY)
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 2;
            }
            else
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 2;
            }
            BoardRefresh();
        }
        private void onClickImage4(object sender, MouseButtonEventArgs e)
        {
            img_selected.Margin = new Thickness(140, 540, 0, 0);
            rct_mask.Margin = new Thickness(153, 552, 0, 0);
            if (nCurrentTap == TAP_HISTORY)
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 3;
            }
            else
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 3;
            }
            BoardRefresh();
        }
        private void onClickImage5(object sender, MouseButtonEventArgs e)
        {
            img_selected.Margin = new Thickness(475, 540, 0, 0);
            rct_mask.Margin = new Thickness(490, 552, 0, 0);
            if (nCurrentTap == TAP_HISTORY)
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 4;
            }
            else
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 4;
            }
            BoardRefresh();
        }

        private void onClickImage6(object sender, MouseButtonEventArgs e)
        {
            img_selected.Margin = new Thickness(815, 540, 0, 0);
            rct_mask.Margin = new Thickness(827, 552, 0, 0);
            if (nCurrentTap == TAP_HISTORY)
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 5;
            }
            else
            {
                nCurrSel = (nHistoryCurrPage * ICON_COUNT_EACH_PAGE) + 5;
            }
            BoardRefresh();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            topWindow.Close();
        }


    }

    class HistoryInfo
    {
        public int nCategory;  // history or culture

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
