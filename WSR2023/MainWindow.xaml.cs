using System;
using System.Collections.Generic;
using System.IO;
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
using WSR2023.Models;
using WSR2023.Pages;
using static WSR2023.Classes.Helper;

namespace WSR2023
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void loadData()
        {

            try
            {
                ProjectsLv.ItemsSource = Db.Project.ToList();
                ProjectsLv.SelectedIndex = 0;
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                VersionLbl.Content = version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString() + version.Revision.ToString();
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            TitleLb.Content = (e.Content as Page).Title;
            if (MainFrame.CanGoBack)
            {
                BackBtn.Visibility = Visibility.Visible;
            }
            else
            {
                BackBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void DashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Dashboard());
        }

        private void TaskBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Tasks());

        }

        private void GantBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Gant());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
            if(File.Exists("Memory.txt"))
            {
                int num = Convert.ToInt32(File.ReadAllText("Memory.txt"));
                if(num == 1)
                {
                    MainFrame.Navigate(new Dashboard());
                }
                if(num == 2)
                {
                    MainFrame.Navigate(new Tasks());
                }
                if (num == 3)
                {
                    MainFrame.Navigate(new Gant());
                }
            }
        }

        private void ProjectsLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProject = ProjectsLv.SelectedItem as Project;
            if (File.Exists("Memory.txt"))
            {
                int num = Convert.ToInt32(File.ReadAllText("Memory.txt"));
                if (num == 1)
                {
                    MainFrame.Navigate(new Dashboard());
                }
                if (num == 2)
                {
                    MainFrame.Navigate(new Tasks());
                }
                if (num == 3)
                {
                    MainFrame.Navigate(new Gant());
                }
            }
        }
    }
}
