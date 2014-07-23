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
using System.IO;
using System.Xml;
using System.Collections.Specialized;


namespace manager_pc
{
    /// <summary>
    /// HistoryContent.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HistoryContent : Window
    {
        RegistryKey rkey;
        String pathHistoryRoot;
        String currentName;
        bool isNewBoard;

        String imgPath1 = "";
        String imgPath2 = "";
        String imgPath3 = "";
        String imgPathThumb = "";

        public HistoryContent(String _currentName, bool _isNewBoard)
        {
            InitializeComponent();

            this.currentName = _currentName;
            this.isNewBoard = _isNewBoard;

            rkey = Registry.CurrentUser.OpenSubKey("SONGPA").OpenSubKey("root_info", true);

            if (rkey.GetValue("PATH") != null)
                pathHistoryRoot = rkey.GetValue("PATH").ToString() + "\\history_root";

            if (isNewBoard) // 영문제목을 폴더명으로 쓰기 떄문에 수정시 변경 할 수 없다. [6/8/2014 Mark]
            {
                txt_1.IsEnabled = true;
            }
            else
            {
                txt_1.IsEnabled = false;
                XMLLoad(pathHistoryRoot + "\\" + currentName);
            }
        }

        private void btn_SelImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFileDlg = new OpenFileDialog();

            oFileDlg.Filter = "JPG Files(*.jpg)|*.jpg";
            if (oFileDlg.ShowDialog() == true)
            {
                imgPath1 = oFileDlg.FileName;
                img_Form1.Tag = oFileDlg.FileName;

                BitmapImage bitmap = new BitmapImage(new Uri(imgPath1));
                img_Form1.Source = bitmap;
            }
        }


        private void btn_Image2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFileDlg = new OpenFileDialog();

            oFileDlg.Filter = "JPG Files(*.jpg)|*.jpg";
            if (oFileDlg.ShowDialog() == true)
            {
                imgPath2 = oFileDlg.FileName;
                img_Form2.Tag = oFileDlg.FileName;

                BitmapImage bitmap = new BitmapImage(new Uri(imgPath2));
                img_Form2.Source = bitmap;
            }
        }

        private void btn_Image3_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFileDlg = new OpenFileDialog();

            oFileDlg.Filter = "JPG Files(*.jpg)|*.jpg";
            if (oFileDlg.ShowDialog() == true)
            {
                imgPath3 = oFileDlg.FileName;
                img_Form3.Tag = oFileDlg.FileName;

                BitmapImage bitmap = new BitmapImage(new Uri(imgPath3));
                img_Form3.Source = bitmap;
            }
        }

        private void btn_ThumbNail_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFileDlg = new OpenFileDialog();

            oFileDlg.Filter = "JPG Files(*.jpg)|*.jpg";
            if (oFileDlg.ShowDialog() == true)
            {
                imgPathThumb = oFileDlg.FileName;
                img_ThumbNail.Tag= oFileDlg.FileName;

                BitmapImage bitmap = new BitmapImage(new Uri(imgPathThumb));
                img_ThumbNail.Source = bitmap;
            }
        }


        // 이름을 만들어주는 함수 [7/1/2014 Mark]
        private String MakeNameString(String path, String prefixName)
        {
            int nCount= 0;
            while(true)
            {
                String pathFile = path + "\\" + prefixName + "_" + ++nCount + ".jpg";
                if (!File.Exists(pathFile))  // 해당 파일이 있으면
                {
                    return pathFile;
                }
            }
            
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            //if (isNewBoard) // new or modify? [6/8/2014 Mark]
            //{

            //}
            //else // modify [6/8/2014 Mark]
            //{
            //    Close();
            //}

            currentName = txt_1.Text;
            DirectoryInfo di_SUB = new DirectoryInfo(pathHistoryRoot + "\\" + currentName);  //Create Directoryinfo value by sDirPath

            if (di_SUB.Exists == false) // 해당 이름의 폴더가 존재하지 않으면... [6/8/2014 Mark]
            {
                di_SUB.Create();
                XMLCreate(di_SUB.FullName);
            }
            else
            {
                XMLCreate(di_SUB.FullName);
            }
            Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // 보기 좋게 줄 바꿈 해주는 함수.
        private String TextChanger(String str, int nWordPerLine)
        {
            str= str.Replace("\r", "");
            char[] delimiterChars= { '\n' };
            String[] words= str.Split(delimiterChars);

            String strRes = "";

            foreach (string s in words)
            {
                strRes += InsertCRLFinside(s, nWordPerLine);
            }
            return strRes;
        }

        // 각 위치에 삽입함. [7/24/2014 Mark]
        private String InsertCRLFinside(String strOriginal, int nMaxlen)
        {
            int nCount= strOriginal.Length / nMaxlen;   // 나누게 될 줄 개수 [7/24/2014 Mark]
            for (int i = 1; i <= nCount; ++i)
            {
                strOriginal= strOriginal.Insert(nMaxlen * i, "\r\n");
            }
            return strOriginal + "\r\n";
        }

        private void XMLCreate(String pathToSave)
        {
            // 생성할 XML 파일 경로와 이름, 인코딩 방식을 설정합니다.
            XmlTextWriter textWriter = new XmlTextWriter(pathToSave + "\\1.xml", Encoding.UTF8);

            // 들여쓰기 설정
            textWriter.Formatting = Formatting.Indented;

            // 문서에 쓰기를 시작합니다.
            textWriter.WriteStartDocument();

            // 루트 설정
            textWriter.WriteStartElement("root");
            {
                // 노드와 값 설정
                AddElement(textWriter, "tabs", cbo_SelPosition.SelectedIndex.ToString());

                AddElement(textWriter, "txt1", txt_1.Text);
                AddElement(textWriter, "txt2", txt_2.Text);
                AddElement(textWriter, "txt3", TextChanger(txt_3.Text, 32));
                AddElement(textWriter, "txt4", TextChanger(txt_4.Text, 45));

                if (img_Form1.Source == null)
                    AddElement(textWriter, "image1", "");
                else
                {
                    String pathAbsolute = "";
                    if (imgPath1 != "")
                    {
                        String sour = imgPath1;
                        String dest = pathHistoryRoot + "\\" + currentName;
                        dest = MakeNameString(dest, "image1");

                        File.Copy(sour, dest, true);
                        pathAbsolute = dest;
                    }
                    else    // 안바뀌었으면 그대로 가져감. [7/2/2014 Mark]
                    {
                        pathAbsolute = (String)img_Form1.Tag;
                    }
                    pathAbsolute = pathAbsolute.Replace("\\", "/");
                    String pathRelative = pathAbsolute.Substring(pathAbsolute.LastIndexOf("/") + 1);
                    AddElement(textWriter, "image1", pathRelative);
                }

                if (img_Form2.Source == null)
                    AddElement(textWriter, "image2", "");
                else
                {
                    String pathAbsolute = "";
                    if (imgPath2 != "")
                    {
                        String sour = imgPath2;
                        String dest = pathHistoryRoot + "\\" + currentName;
                        dest = MakeNameString(dest, "image2");

                        File.Copy(sour, dest, true);
                        pathAbsolute = dest;
                    }
                    else    // 안바뀌었으면 그대로 가져감. [7/2/2014 Mark]
                    {
                        pathAbsolute = (String)img_Form2.Tag;
                    }
                    pathAbsolute = pathAbsolute.Replace("\\", "/");
                    String pathRelative = pathAbsolute.Substring(pathAbsolute.LastIndexOf("/") + 1);
                    AddElement(textWriter, "image2", pathRelative);
                }

                if (img_Form3.Source == null)
                    AddElement(textWriter, "image3", "");
                else
                {
                    String pathAbsolute = "";
                    if (imgPath3 != "")
                    {
                        String sour = imgPath3;
                        String dest = pathHistoryRoot + "\\" + currentName;
                        dest = MakeNameString(dest, "image3");

                        File.Copy(sour, dest, true);
                        pathAbsolute = dest;
                    }
                    else    // 안바뀌었으면 그대로 가져감. [7/2/2014 Mark]
                    {
                        pathAbsolute = (String)img_Form3.Tag;
                    }
                    pathAbsolute = pathAbsolute.Replace("\\", "/");
                    String pathRelative = pathAbsolute.Substring(pathAbsolute.LastIndexOf("/") + 1);
                    AddElement(textWriter, "image3", pathRelative);
                }

                if (img_ThumbNail.Source == null)
                    AddElement(textWriter, "img_thumb", "");
                else
                {
                    String pathAbsolute = "";
                    if (imgPathThumb != "")
                    {
                        String sour = imgPathThumb;
                        String dest = pathHistoryRoot + "\\" + currentName;
                        dest = MakeNameString(dest, "imageThumb");

                        File.Copy(sour, dest, true);
                        pathAbsolute = dest;
                    }
                    else    // 안바뀌었으면 그대로 가져감. [7/2/2014 Mark]
                    {
                        pathAbsolute = (String)img_ThumbNail.Tag;
                    }

                    pathAbsolute= pathAbsolute.Replace("\\", "/");
                    String pathRelative = pathAbsolute.Substring(pathAbsolute.LastIndexOf("/") + 1);
                    AddElement(textWriter, "img_thumb", pathRelative);
                }
            }
            textWriter.WriteEndElement();

            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        private void AddElement(XmlTextWriter w, String key, String text)
        {
            w.WriteStartElement(key);
            w.WriteString(text);
            w.WriteEndElement();
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
                    case "tabs":
                        cbo_SelPosition.SelectedIndex = Int32.Parse(node.InnerText);
                        break;
                    case "txt1":
                        txt_1.Text = node.InnerText;
                        break;
                    case "txt2":
                        txt_2.Text = node.InnerText;
                        break;
                    case "txt3":
                        txt_3.Text = node.InnerText;
                        break;
                    case "txt4":
                        txt_4.Text = node.InnerText;
                        break;
                    case "image1":
                        {
                            if (node.InnerText.Trim() == "")
                                break;

                            String pathImage = pathToLoad + "\\" + node.InnerText;
                            BitmapImage bitmap = new BitmapImage(new Uri(pathImage));
                            if (bitmap != null)
                            {
                                img_Form1.Source = bitmap;
                                img_Form1.Tag = pathImage;
                            }
                        }
                        break;
                    case "image2":
                        {
                            if (node.InnerText.Trim() == "")
                                break;

                            String pathImage = pathToLoad + "\\" + node.InnerText;
                            BitmapImage bitmap = new BitmapImage(new Uri(pathImage));
                            if (bitmap != null)
                            {
                                img_Form2.Source = bitmap;
                                img_Form2.Tag = pathImage;
                            }
                        }
                        break;
                    case "image3":
                        {
                            if (node.InnerText.Trim() == "")
                                break;

                            String pathImage = pathToLoad + "\\" + node.InnerText;
                            BitmapImage bitmap = new BitmapImage(new Uri(pathImage));
                            if (bitmap != null)
                            {
                                img_Form3.Source = bitmap;
                                img_Form3.Tag = pathImage;
                            }
                        }
                        break;
                    case "img_thumb":
                        {
                            if (node.InnerText.Trim() == "")
                                break;

                            String pathImage = pathToLoad + "\\" + node.InnerText;
                            BitmapImage bitmap = new BitmapImage(new Uri(pathImage));
                            if (bitmap != null)
                            {
                                img_ThumbNail.Source = bitmap;
                                img_ThumbNail.Tag = pathImage;
                            }
                        }
                        break;
                }
            }
            return true;
        }

        private void txt_3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}
