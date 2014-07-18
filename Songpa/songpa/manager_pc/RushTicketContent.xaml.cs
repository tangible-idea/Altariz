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
    /// RushTicketContent.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RushTicketContent : Window
    {
        RegistryKey rkey;
        String pathRushRoot;
        String currentName;
        bool isNewBoard;

        String imgPath1 = "";

        // 현재 sub folder 명과 새로운 보드인지 판단. [6/8/2014 Mark]
        public RushTicketContent(String _currentName, bool _isNewBoard)
        {
            InitializeComponent();

            this.currentName = _currentName;
            this.isNewBoard = _isNewBoard;

            rkey = Registry.CurrentUser.OpenSubKey("SONGPA").OpenSubKey("root_info", true);

            if (rkey.GetValue("PATH") != null)
                pathRushRoot = rkey.GetValue("PATH").ToString() + "\\rush_ticket_root";

            if (isNewBoard) // 영문제목을 폴더명으로 쓰기 떄문에 수정시 변경 할 수 없다. [6/8/2014 Mark]
            {
                txt_EngTitle.IsEnabled = true;
            }
            else
            {
                txt_EngTitle.IsEnabled = false;
                XMLLoad(pathRushRoot + "\\" + currentName);
            }
        }

        private void btn_SelImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFileDlg = new OpenFileDialog();

            oFileDlg.Filter = "JPG Files(*.jpg)|*.jpg";
            if (oFileDlg.ShowDialog() == true)
            {
                imgPath1 = oFileDlg.FileName;       // path에 넣는다.
                img_Form1.Tag = oFileDlg.FileName;  // 태그에도 불러온 파일의 경로를 넣는다.

                BitmapImage bitmap = new BitmapImage(new Uri(imgPath1));
                img_Form1.Source = bitmap;
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

            currentName = txt_EngTitle.Text;
            DirectoryInfo di_SUB = new DirectoryInfo(pathRushRoot + "\\" + currentName);  //Create Directoryinfo value by sDirPath

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
                AddElement(textWriter, "title_eng", txt_EngTitle.Text);
                AddElement(textWriter, "title_kor", txt_KorTitle.Text);
                AddElement(textWriter, "pos_eng", txt_EngPosition.Text);
                AddElement(textWriter, "pos_kor", txt_KorPosition.Text);
                AddElement(textWriter, "period_eng", txt_EngPeriod.Text);
                AddElement(textWriter, "period_kor", txt_KorPeriod.Text);
                AddElement(textWriter, "time_eng", txt_EngTime.Text);
                AddElement(textWriter, "time_kor", txt_KorTime.Text);
                AddElement(textWriter, "price_eng", txt_EngPrice.Text);
                AddElement(textWriter, "price_kor", txt_KorPrice.Text);
                AddElement(textWriter, "contact", txt_Contact.Text);

                //if (img_Form1.Source == null)
                //    AddElement(textWriter, "image1", "");
                //else
                //{
                //    String pathAbsolute= img_Form1.Source.ToString();
                //    String pathRelative = pathAbsolute.Substring(pathAbsolute.LastIndexOf("/") + 1);
                //    AddElement(textWriter, "image1", pathRelative);
                //}

                if (img_Form1.Source == null)
                    AddElement(textWriter, "image1", "");
                else
                {
                    String pathAbsolute = "";
                    if (imgPath1 != "")
                    {
                        String sour = imgPath1;
                        String dest = pathRushRoot + "\\" + currentName;
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


                AddElement(textWriter, "image2", cbo_SelPosition.SelectedIndex.ToString());
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
            
            XmlNode root= XmlDoc.DocumentElement;
            XmlNodeList list = root.ChildNodes;

            foreach(XmlNode node in list)
            {
                switch (node.Name)
                {
                    case "title_eng":
                        txt_EngTitle.Text = node.InnerText;
                        break;
                    case "title_kor":
                        txt_KorTitle.Text = node.InnerText;
                        break;
                    case "pos_eng":
                        txt_EngPosition.Text = node.InnerText;
                        break;
                    case "pos_kor":
                        txt_KorPosition.Text = node.InnerText;
                        break;
                    case "period_eng":
                        txt_EngPeriod.Text = node.InnerText;
                        break;
                    case "period_kor":
                        txt_KorPeriod.Text = node.InnerText;
                        break;
                    case "time_eng":
                        txt_EngTime.Text = node.InnerText;
                        break;
                    case "time_kor":
                        txt_KorTime.Text = node.InnerText;
                        break;
                    case "price_eng":
                        txt_EngPrice.Text = node.InnerText;
                        break;
                    case "price_kor":
                        txt_KorPrice.Text = node.InnerText;
                        break;
                    case "contact":
                        txt_Contact.Text = node.InnerText;
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
                        //{
                        //    if (node.InnerText.Trim() == "")    // 경로가 없으면 저장 안함.
                        //        break;

                        //    String pathImage = pathToLoad + "\\" + node.InnerText;

                        //    if (!File.Exists(pathImage))
                        //    {
                        //        MessageBox.Show("이미지 경로가 잘못되어 불러올 수 없습니다.\n해당 파일이 삭제되었거나 옮겨진 것으로 판단됩니다.\n다른 파일로 변경해주세요.");
                        //        return false;
                        //    }

                        //    BitmapImage bitmap = new BitmapImage(new Uri(pathImage));
                            
                        //    if (bitmap != null)
                        //        img_Form1.Source = bitmap;
                        //}
                        break;
                    case "image2":
                        cbo_SelPosition.SelectedIndex = int.Parse(node.InnerText);  // 불러와서 콤보 박스 선택. [7/8/2014 Mark]
                        break;
                }
            }
            return true;
        }

        private void cbo_SelPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        // 이름을 만들어주는 함수 [7/1/2014 Mark]
        private String MakeNameString(String path, String prefixName)
        {
            int nCount = 0;
            while (true)
            {
                String pathFile = path + "\\" + prefixName + "_" + ++nCount + ".jpg";
                if (!File.Exists(pathFile))  // 해당 파일이 있으면
                {
                    return pathFile;
                }
            }

        }
    }
}
