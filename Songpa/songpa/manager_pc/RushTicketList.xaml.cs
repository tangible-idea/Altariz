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

namespace manager_pc
{
    /// <summary>
    /// RushTicketList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RushTicketList : Window
    {
        public RushTicketList()
        {
            InitializeComponent();

            List<TodoItem> items = new List<TodoItem>();

            for (int i = 0; i < 100; ++i )
            {
                items.Add(new TodoItem() { Title = "스팀펑크아트", Number= i });
            }
            

            lbTodoList.ItemsSource = items;

        }

        private void btn_selPC_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.Show();

            this.Close();
        }

        private void btn_newBoard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnBtnClick_board_modify(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            MessageBox.Show("modify : "+ btn.Tag);
        }
        private void OnBtnClick_board_delete(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            MessageBox.Show("delete : " + btn.Tag);
        }
    }

    public class TodoItem
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public String ButtonLabel { get; set; }
    }
}
