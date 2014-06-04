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

            DataContext = new List<ScreenRequest>
                              {
                                  new ScreenRequest() {ActionDescription = "Click Me!"},
                                  new ScreenRequest() {ActionDescription = "Click Me Too!"},
                                  new ScreenRequest() {ActionDescription = "Click Me Again!!"},
                              };
        }
    }


    //Dead-simple implementation of ICommand
    //Serves as an abstraction of Actions performed by the user via interaction with the UI (for instance, Button Click)
    public class Command : ICommand
    {
        public Action Action { get; set; }

        public void Execute(object parameter)
        {
            if (Action != null)
                Action();
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;

        public Command(Action action)
        {
            Action = action;
        }
    }

    public class ScreenRequest : INotifyPropertyChanged
    {
        public Command SomeAction { get; set; }

        private string _actionDescription;
        public string ActionDescription
        {
            get { return _actionDescription; }
            set
            {
                _actionDescription = value;
                NotifyPropertyChanged("ActionDescription");
            }
        }

        private string _details;
        public string Details
        {
            get { return _details; }
            set
            {
                _details = value;
                NotifyPropertyChanged("Details");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ScreenRequest()
        {
            SomeAction = new Command(ExecuteSomeAction) { IsEnabled = true };
        }

        //public SomeProperty YourProperty { get; set; }

        private void ExecuteSomeAction()
        {
            //Place your custom logic here based on YourProperty
            ActionDescription = "Clicked!!";
            Details = "Some Details";
        }
    }
}
