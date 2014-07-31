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

namespace songpa
{
    /// <summary>
    /// Connection.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Connection : Window
    {
        static public String PATH = "";
        static public String PATHlocal = Directory.GetCurrentDirectory();
        RegistryKey rkey;

        public Connection()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.CheckLisence() == false)
            {   // license expired [7/31/2014 Mark]
                MessageBox.Show("license expired.");
                this.Close();
                return;
            }

            cbo_connection_target.Items.Clear();
            cbo_connection_target.Items.Add("Rush ticket");
            cbo_connection_target.Items.Add("Concert & Festival");
            cbo_connection_target.Items.Add("History & culture");

            // 레지스트리 값 읽어오기 [6/5/2014 Mark]
            Registry.CurrentUser.CreateSubKey("SONGPA").CreateSubKey("connection");
            rkey = Registry.CurrentUser.OpenSubKey("SONGPA").OpenSubKey("connection", true);

            try
            {
                if (rkey.GetValue("IP") != null)    // 값이 있으면 [6/25/2014 Mark]
                {
                    txt_IP.Text = rkey.GetValue("IP").ToString();
                    txt_Account.Text = rkey.GetValue("ACCOUNT").ToString();
                    txt_PW.Password = rkey.GetValue("PASSWORD").ToString();
                    txt_Path.Text = rkey.GetValue("PATH").ToString();
                    cbo_connection_target.SelectedIndex = (int)rkey.GetValue("TARGET");

                    TryConnection();
                }   
            }
            catch
            {
                MessageBox.Show("접속 정보 레지스트리를 초기화해주세요.");
                return;
            }
         
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TryConnection();            
        }

        // 연결 시도... [7/25/2014 Mark]
        private void TryConnection()
        {
            //String strPath1 = @"\\192.168.0.14\all";
            PATH = "\\\\" + txt_IP.Text + "\\" + txt_Path.Text;


            SharedAPI api = new SharedAPI();
            int nRes = api.ConnectRemoteServer(PATH, txt_Account.Text, txt_PW.Password);

            String msg = "Connection error code : ";
            if (nRes == 0)
            {
                msg = "Connection sucessfull!";
            }
            //MessageBox.Show(msg + nRes);

            if (msg == "Connection sucessfull!")
            {
                RemoveExists();
                CopyFolder(PATH, PATHlocal);    // 접속한 pc로부터 컨텐츠들을 가져온다.

                // 접속 성공시 해당 값으로 접속정보를 레지스트리에 쓴다. [6/5/2014 Mark]
                rkey.SetValue("IP", txt_IP.Text.ToString());
                rkey.SetValue("ACCOUNT", txt_Account.Text.ToString());
                rkey.SetValue("PASSWORD", txt_PW.Password.ToString());
                rkey.SetValue("PATH", txt_Path.Text.ToString());
                rkey.SetValue("TARGET", cbo_connection_target.SelectedIndex);   // target도 저장 [7/25/2014 Mark]
    

                if (cbo_connection_target.SelectedIndex == 0)
                {
                    var newWindow = new Rush();
                    newWindow.Show();
                    this.Close();
                }
                else if (cbo_connection_target.SelectedIndex == 1)
                {
                    var newWindow = new Festival();
                    newWindow.Show();
                    this.Close();
                }
                else if (cbo_connection_target.SelectedIndex == 2)
                {
                    var newWindow = new History();
                    newWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("접속 정보가 잘못되었거나, 만들어지지 않은 페이지 입니다.");
                }
            }
            else // 실패일 경우에만 메시지 띄우자 자동 넘김을 위해서... [7/25/2014 Mark]
            {
                MessageBox.Show(msg + nRes);
            }
        }
        // 원본과, 목적지를 같이 대입  
        public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(destFolder, name);
                File.Copy(file, dest, true);
            }

            // foreach 안에서 재귀 함수를 통해서 폴더 복사 및 파일 복사 진행 완료  
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        public void RemoveExists()
        {

            // ...or with DirectoryInfo instance method.
            System.IO.DirectoryInfo di1 = new System.IO.DirectoryInfo(PATHlocal + @"\\rush_ticket_root");
            System.IO.DirectoryInfo di2 = new System.IO.DirectoryInfo(PATHlocal + @"\\festival_root");
            System.IO.DirectoryInfo di3 = new System.IO.DirectoryInfo(PATHlocal + @"\\history_root");
            // Delete this dir and all subdirs.
            try
            {
                if(di1.Exists)
                    di1.Delete(true);

                if (di2.Exists)
                    di2.Delete(true);
                
                if (di3.Exists)
                    di3.Delete(true);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("기존의 컨텐츠들을 삭제할 수 없었습니다.");
            }
        }

        private bool CheckLisence()
        {
            if (rkey.GetValue("LICENSE") == null)  // 라이선스 등록이 안되어 있음. [7/31/2014 Mark]
            {
                DateTime currDate = DateTime.Now;
                currDate = currDate.AddDays(20);
                rkey.SetValue("LICENSE", currDate);
                return true;
            }
            else       // 라이선스 등체크 [7/31/2014 Mark]
            {
                string LIC = rkey.GetValue("LICENSE").ToString();
                DateTime expiredDate = DateTime.Parse(LIC);
                //MessageBox.Show("lisence expired date : " + expiredDate.ToString());

                DateTime currDate = DateTime.Now;
                TimeSpan tDiff = expiredDate - currDate;

                //MessageBox.Show("remaining date : " + tDiff.TotalDays);

                if (tDiff.TotalDays < 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
