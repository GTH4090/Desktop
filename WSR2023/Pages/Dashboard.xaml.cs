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
using WSR2023.Models;
using System.Windows.Threading;

namespace WSR2023.Pages
{
    /// <summary>
    /// Логика взаимодействия для Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        DispatcherTimer Timer = new DispatcherTimer();
        public Dashboard()
        {
            InitializeComponent();
            File.WriteAllText("Memory.txt", "1");
            Timer.Interval = new TimeSpan(0, 0, 30);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {

            try
            {
                List<Employee> closedEmployee = Db.Employee.ToList().OrderByDescending(el => el.ClosedCount).Take(5).ToList();
                ClosedemployeeDataGrid.ItemsSource = closedEmployee;

                List<Employee> deadLinedEmployee = Db.Employee.ToList().OrderByDescending(el => el.DeadLinedCount).Take(5).ToList();
                DeadlinedemployeeDataGrid.ItemsSource = deadLinedEmployee;

                List<Models.Task> notClosedTasks = Db.Task.Where(el => el.StatusId == 1 || el.StatusId == 2).ToList();
                NotClosedLb.Content = $"Кол-во: {notClosedTasks.Count()}";
                notClosedTaskDataGrid.ItemsSource = notClosedTasks;

                List<Models.Task> deadlinedTasks = Db.Task.Where(el => el.Deadline < DateTime.Now).ToList();
                DeadlinedLb.Content = $"Кол-во: {deadlinedTasks.Count()}";
                deadlinedTaskDataGrid.ItemsSource = deadlinedTasks;
                if(deadlinedTasks.Count() > 2) 
                {
                    deadlinedSp.Background = Brushes.Red;
                }

                List<Models.Task> activeTasks = Db.Task.Where(el => (el.StartActualTime <= DateTime.Now || el.CreatedTime <= DateTime.Now) &&
                (el.FinishActualtime >= DateTime.Now || el.Deadline >= DateTime.Now) && el.StatusId == 2).ToList();
                ActiveLb.Content = $"Кол-во: {activeTasks.Count()}";
                activeTaskDataGrid.ItemsSource = activeTasks;
                if(activeTasks.Count() == 0 && DateTime.Now.TimeOfDay.Hours >= 9 &&
                    DateTime.Now.TimeOfDay.Hours <= 18)
                {
                    ActiveSp.Background = Brushes.Red;
                }

                DateTime weekEnd;
                int day = ((int)DateTime.Now.DayOfWeek);
                if (day != 0)
                {

                    weekEnd = DateTime.Now.AddDays(7 - day);
                }
                else
                {
                    weekEnd = DateTime.Now;
                }
                DateTime weekStart = weekEnd.AddDays(-6);

                List<Models.Task> notClosedPerWeekTasks = Db.Task.Where(el => el.StatusId == 1 || el.StatusId == 2 && (el.StartActualTime >= weekStart || el.CreatedTime >= weekStart
                || el.FinishActualtime <= weekEnd || el.Deadline <= weekEnd)).ToList();
                NotClosePreWeekLb.Content = $"Кол-во: {notClosedPerWeekTasks.Count()}";
                notClodesPreWeekTaskDataGrid.ItemsSource = notClosedPerWeekTasks;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(MainGrid.ActualWidth <= 1440)
            {
                Grid.SetRow(NotClosePerWeekSp, 1);
                Grid.SetColumn(NotClosePerWeekSp, 0);
                Grid.SetColumn(Top5BestSp, 1);
                Grid.SetColumn(Top5WorstSp, 2);
                MainGrid.ColumnDefinitions[3].Width = new GridLength(0);
            }
            
            if (MainGrid.ActualWidth > 1440)
            {
                Grid.SetRow(NotClosePerWeekSp, 0);
                Grid.SetColumn(NotClosePerWeekSp, 3);
                Grid.SetColumn(Top5BestSp, 0);
                Grid.SetColumn(Top5WorstSp, 1);
                MainGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Star);
            }
        }
    }
}
