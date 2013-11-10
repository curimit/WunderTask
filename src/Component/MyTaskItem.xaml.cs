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
    /// MyTaskItem.xaml 的交互逻辑
    /// </summary>
    public partial class MyTaskItem : UserControl, INotifyPropertyChanged
    {
        public MyTaskItem()
        {
            InitializeComponent();
        }

        public MyTaskItem(string Text)
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

        private bool _isChecked;

        public bool IsChecked
        {

            get
            {
                return _isChecked;
            }

            set
            {
                this._isChecked = value;
                OnPropertyChanged("IsChecked");
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
