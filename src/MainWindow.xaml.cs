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
using WunderTask.Component;

namespace WunderTask
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string WindowTitle { get; set; }

        private GTasksAPI api = new GTasksAPI();

        public MainWindow()
        {
            InitializeComponent();
            WindowTitle = "WunderTask1"; 
            OnPropertyChanged("WindowTitle");
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private async void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await api.Synchronize();
            WindowTitle = "WunderTask2";
            OnPropertyChanged("WindowTitle");

            this.TaskListBox.Items.Clear();
            foreach (var tasklist in api.GetTaskLists())
            {
                this.TaskListBox.Items.Add(new MyTaskList(tasklist.Title));
            }
        }

        private async void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tasklist_title = ((MyTaskList)this.TaskListBox.SelectedItem).Text;
            var tasks = await api.GetTasksByTitle(tasklist_title);
            if (tasks == null)
            {
                MessageBox.Show("No such list.");
                return;
            }

            this.TasksBox.Items.Clear();
            foreach (var task in tasks.Items)
            {
                MyTaskItem item = new MyTaskItem();
                item.Text = task.Title;
                item.IsChecked = task.Status == "completed";
                this.TasksBox.Items.Add(item);
            }
        }
    }

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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

    [ValueConversion(typeof(string), typeof(string))]
    public class TitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string title = (string)value;
            return "[" + title + "]";
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string title = (string)value;
            return title.Substring(1, title.Length - 2);
        }
    }
}
