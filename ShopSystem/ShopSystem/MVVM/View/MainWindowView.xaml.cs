using ShopSystem.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShopSystem.MVVM.View
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public MainWindowView()
        {
            InitializeComponent();
            Timer.Tick += new EventHandler(TimerClick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new MainWindowVM();
            viewModel.RequestClose += () => Close();
            DataContext = viewModel;
        }

        private void TimerClick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            ClockLabel.Content = $"{dateTime.Hour.ToString("D2")}:{dateTime.Minute.ToString("D2")}:{dateTime.Second.ToString("D2")} {dateTime.DayOfWeek.ToString()} " +
                $"{dateTime.Day.ToString("D2")}.{dateTime.Month.ToString("D2")}.{dateTime.Year.ToString("D4")}";
        }
    }
}
