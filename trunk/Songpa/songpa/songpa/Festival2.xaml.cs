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

namespace songpa
{
    /// <summary>
    /// Festival2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Festival2 : Window
    {
        public Festival2()
        {
            InitializeComponent();

            int nWidth = 1920;
            int nHeight = 1080;

            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 0;
            this.Top = -1 * nHeight;
            //this.Width = SystemParameters.VirtualScreenWidth;
            //this.Height = SystemParameters.VirtualScreenHeight;
            this.Width = nWidth;
            this.Height = nHeight;

            my_windows.Width = nWidth;
            my_windows.Height = nHeight;
            //this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;
            this.Show();
        }
    }
}
