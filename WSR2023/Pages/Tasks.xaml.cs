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
using static WSR2023.Classes.Helper;

namespace WSR2023.Pages
{
    /// <summary>
    /// Логика взаимодействия для Tasks.xaml
    /// </summary>
    public partial class Tasks : Page
    {
        public Tasks()
        {
            InitializeComponent();
            File.WriteAllText("Memory.txt", "2");
        }

        private void cbxLoad()
        {

            try
            {
                executiveEmployeeIdCbx.ItemsSource = Db.Employee.ToList();
                previousTaskIdCbx.ItemsSource = Db.Task.ToList();
                statusIdCbx.ItemsSource = Db.TaskStatus.ToList();
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
        private void loadData()
        {

            try
            {
                List<Models.Task> tasks = SelectedProject.Task.Where(el => el.StatusId != 3).ToList();
                tasks = tasks.OrderBy(el => el.SortNum).ToList();
                if(Searchtbx.Text != "")
                {
                    tasks = tasks.Where(el => el.FullTitle.Contains(Searchtbx.Text) && el.Description.Contains(Searchtbx.Text)).ToList();
                }
                taskDataGrid.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
            cbxLoad();
        }

        private void Searchtbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadData();
        }

        private void taskDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(taskDataGrid.SelectedItem != null)
            {
                var item = taskDataGrid.SelectedItem as Models.Task;
                Grid.SetColumnSpan(taskDataGrid, 1);
                DetailedInfo.Visibility = Visibility.Visible;
                grid1.DataContext = item;
            }
            
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Db.SaveChanges();
            Grid.SetColumnSpan(taskDataGrid, 2);
            DetailedInfo.Visibility = Visibility.Collapsed;
            grid1.DataContext = null;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetColumnSpan(taskDataGrid, 2);
            DetailedInfo.Visibility = Visibility.Collapsed;
            grid1.DataContext = null;
        }
    }
}
