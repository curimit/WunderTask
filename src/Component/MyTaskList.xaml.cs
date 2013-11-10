using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WunderTask.Component
{
    /// <summary>
    /// MyTaskList.xaml 的交互逻辑
    /// </summary>
    public partial class MyTaskList : UserControl, INotifyPropertyChanged
    {
        public MyTaskList()
        {
            InitializeComponent();
        }

        public MyTaskList(string Text)
        {
            this.Text = Text;
            InitializeComponent();
        }

        private string _text;

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                this._text = value;
                OnPropertyChanged("Text"); 
            }
        }

        #region Property Changed Event
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
