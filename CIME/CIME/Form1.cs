using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Collections;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ReadExcelfile(@"P01.xlsx");

            if (arrCode.Count == 0)
            {
                MessageBox.Show("데이터 파일을 찾을 수 없거나, 잘못된 파일입니다.");
                Application.Exit();
            }
        }

        bool bCtrl = false;

        String word = "";
        String word_completed = "";

        List<String> arrCode = new List<String>();
        List<String> arrData = new List<String>();

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)   // 스페이스 || 엔터
            {
                //MessageBox.Show(word);
                word_completed += FindChineseWord();
                word = "";

                richTextBox1.Text = word_completed;
                e.KeyChar = Convert.ToChar(0);
                richTextBox1.Select(richTextBox1.Text.Length, 0);
            }
            else if (e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 27)    // 백스페이스
            {
                    e.KeyChar = Convert.ToChar(0);
                    richTextBox1.Text = word_completed;
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                    word = "";
            }
            else if (e.KeyChar >= 65 || e.KeyChar <= 90)
            {        
                if (!bCtrl)
                {
                word += e.KeyChar;
                e.KeyChar = Convert.ToChar(46);
                }
            }
            else
            {
                e.KeyChar = Convert.ToChar(0);
            }
        }


        private string FindChineseWord()
        {
            for (int i = 0; i < arrCode.Count; ++i )
            {
                if (word.ToUpper() == arrCode[i].ToUpper())   // 맞는 코드를 찾으면...
                {
                    return arrData[i];
                }
            }

            return "";

        }


        private void ReadExcelfile(String strFilePath)
        {
            try
            {
            // OLEDB를 이용한 엑셀 연결
            string szConn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + strFilePath + @";Extended Properties=Excel 12.0";

            OleDbConnection conn = new OleDbConnection(szConn);
            conn.Open();

            // 엑셀로부터 데이타 읽기
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", conn);
            OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            adpt.Fill(ds);

            
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if(dr[2] == DBNull.Value)
                        break;

                    String code = (String)dr[2];
                    String data = (String)dr[3];
                    if (code.Trim() == "")
                        continue;


                    arrCode.Add(code);
                    arrData.Add(data);

                    //string data = string.Format("CODE:{0}, DATA:{1}", dr[2], dr[3]);
                    //MessageBox.Show(data);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("err" + ex.ToString()); 
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                bCtrl = true;
            }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                bCtrl = false;
            }
        }

    }
}
