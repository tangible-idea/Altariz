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

        // 현재 sub folder 명과 새로운 보드인지 판단. [6/8/2014 Mark]
        public RushTicketContent(String _currentName, bool _isNewBoard)
        {
            InitializeComponent();

            this.currentName = _currentName;
            this.isNewBoard = _isNewBoard;

            if (isNewBoard) // 영문제목을 폴더명으로 쓰기 떄문에 수정시 변경 할 수 없다. [6/8/2014 Mark]
                txt_EngTitle.IsEnabled = true;
            else
                txt_EngTitle.IsEnabled = false;

            rkey = Registry.CurrentUser.OpenSubKey("SONGPA").OpenSubKey("root_info", true);

            if (rkey.GetValue("PATH") != null)
                pathRushRoot = rkey.GetValue("PATH").ToString() + "\\rush_ticket_root";
        }

        private void btn_SelImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (isNewBoard) // new or modify? [6/8/2014 Mark]
            {
                currentName = txt_EngTitle.Text;
                DirectoryInfo di_SUB = new DirectoryInfo(pathRushRoot + "\\" + currentName);  //Create Directoryinfo value by sDirPath

                if (di_SUB.Exists == false) // 해당 이름의 폴더가 존재하지 않으면... [6/8/2014 Mark]
                {
                    di_SUB.Create();
                    XMLCreate(di_SUB.FullName);
                }
                Close();
            }
            else // modify [6/8/2014 Mark]
            {
                Close();
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void XMLCreate(String pathToSave)
        {
            // 문서를 만들고 지정된 값의 노드를 만든다..
            XmlDocument NewXmlDoc = new XmlDocument();
            //NewXmlDoc.AppendChild(NewXmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            // 최상위 노드를 만든다.
            XmlNode root = NewXmlDoc.CreateElement("", "Root", "");
            NewXmlDoc.AppendChild(root);

            // 지정된 XML문서로 만들고 저장한다.
            NewXmlDoc.Save(pathToSave + "\\1.xml");
        }
    }
}
