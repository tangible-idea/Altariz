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
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;

namespace manager_pc
{
    /// <summary>
    /// HistoryList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HistoryList : Window
    {
        RegistryKey rkey;
        String pathRoot;
        String pathHistoryRoot;

        public HistoryList()
        {
            InitializeComponent();

            rkey = Registry.CurrentUser.OpenSubKey("SONGPA").OpenSubKey("root_info", true);

            if (rkey.GetValue("PATH") != null)
            {
                pathRoot = rkey.GetValue("PATH").ToString();
                pathHistoryRoot = pathRoot + "\\history_root";
            }

            RefreshFolderList();
        }


        /// <summary>
        /// 폴더 리스트를 다시 가져온다.
        /// </summary>
        private void RefreshFolderList()
        {
            DirectoryInfo di_HISTORY_ROOT = new DirectoryInfo(pathHistoryRoot);
            DirectoryInfo[] arrInfo = di_HISTORY_ROOT.GetDirectories();

            //if (arrInfo.Length == 0)
            //    return;ㅁ

            List<ContentItem> items = new List<ContentItem>();
            for (int i = 0; i < arrInfo.Length; ++i)
            {
                items.Add(new ContentItem() { Title = arrInfo[i].Name, Number = i });
            }
            listSubProjects.ItemsSource = items;
        }

        private void btn_selPC_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.Show();

            this.Close();
        }

        private void btn_newBoard_Click(object sender, RoutedEventArgs e)
        {
            var historyContentWindow = new HistoryContent("", true);
            historyContentWindow.Owner = this;

            historyContentWindow.ShowDialog();
            RefreshFolderList();
            //if (rushContentWindow.ShowDialog() == false)
            //{
            //    RefreshFolderList();
            //}
        }

        private void OnBtnClick_board_modify(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            //MessageBox.Show("modify : "+ btn.Tag);

            int currIdx = Int32.Parse(btn.Tag.ToString());
            ContentItem currItem = (ContentItem)listSubProjects.Items[currIdx];

            var historyContentWindow = new HistoryContent(currItem.Title, false);
            historyContentWindow.Owner = this;
            if (historyContentWindow.ShowDialog() == false)
            {
                //MessageBox.Show("modify");
                //RefreshFolderList();
            }
        }
        private void OnBtnClick_board_delete(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            //MessageBox.Show("delete : " + btn.Tag);
            int currIdx = Int32.Parse(btn.Tag.ToString());
            ContentItem currItem = (ContentItem)listSubProjects.Items[currIdx];
            MessageBoxResult res = MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)    // 해당 항목을 지우고 폴더들 리프레쉬 [6/8/2014 Mark]
            {
                //listSubProjects.Items.RemoveAt(currIdx);
                Directory.Delete(pathHistoryRoot + "\\" + currItem.Title, true);
                System.Threading.Thread.Sleep(350);
                RefreshFolderList();
            }
        }


    }

}
