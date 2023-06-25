using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static WSR2023.Classes.Helper;

namespace WSR2023.UpWindows
{
    /// <summary>
    /// Логика взаимодействия для GantWin.xaml
    /// </summary>
    public partial class GantWin : Window
    {
        int interval = 0;
        DateTime StartDate = DateTime.Now;
        DateTime EndDate = DateTime.Now;
        public GantWin()
        {
            InitializeComponent();


        }

        private void gantLoad()
        {
            if (GantGrid == null)
            {
                return;
            }
            GantGrid.RowDefinitions.Clear();
            GantGrid.ColumnDefinitions.Clear();
            GantGrid.Children.Clear();

            for (int i = 0; i <= SelectedProject.Task.Count(); i++)
            {
                GantGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i <= (EndDate - StartDate).Days; i++)
            {
                GantGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SizeSl.Value) });
                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(3);
                Grid.SetColumn(border, i);
                Grid.SetRowSpan(border, GantGrid.RowDefinitions.Count());
                Grid.SetRow(border, 0);
                if (StartDate.AddDays(i).DayOfWeek == DayOfWeek.Saturday || StartDate.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                {
                    border.Background = Brushes.Red;
                }
                if (StartDate.AddDays(i).Date == DateTime.Now.Date)
                {
                    border.Background = Brushes.Blue;
                }
                GantGrid.Children.Add(border);
                Label dateLb = new Label();
                Grid.SetColumn(dateLb, i);
                Grid.SetRow(dateLb, GantGrid.RowDefinitions.Count() - 1);
                dateLb.Content = StartDate.AddDays(i).ToString("dd MMMM");
                GantGrid.Children.Add(dateLb);
                

            }
            var tasks = SelectedProject.Task.ToList();
            for (int i = 0; i < tasks.Count(); i++)
            {

                try
                {
                    Label TaskLb = new Label();
                    TaskLb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TaskLb.VerticalAlignment = VerticalAlignment.Stretch;
                    TaskLb.Content = tasks[i].FullTitle;
                    
                    TaskLb.Background = Brushes.Wheat;
                    TaskLb.Width = double.NaN;
                    TaskLb.Height = double.NaN;
                    Grid.SetRow(TaskLb, i);
                    DateTime start;
                    if (tasks[i].StartActualTime != null)
                    {
                        

                        start = tasks[i].StartActualTime.Value;
                    }
                    else
                    {
                        
                        start = tasks[i].CreatedTime.Value;

                    }
                    Grid.SetColumn(TaskLb, (start.Date - StartDate.Date).Days);

                    Nullable<DateTime> Finish;
                    if (tasks[i].FinishActualtime != null)
                    {
                        Grid.SetColumnSpan(TaskLb, (tasks[i].FinishActualtime.Value.Date - start.Date).Days +1);
                        Finish = (tasks[i].FinishActualtime.Value);
                    }
                    else
                    {
                        Grid.SetColumnSpan(TaskLb, (EndDate.Date - start.Date).Days + 1);
                        Finish = null;
                    }
                    if(start.Date < StartDate ||  Finish == null || Finish > EndDate) 
                    {
                        TaskLb.Content = "(обрезанно) " + tasks[i].FullTitle;
                    }
                    TaskLb.Tag = tasks[i];
                    TaskLb.MouseDown += TaskLb_MouseDown;


                    GantGrid.Children.Add(TaskLb);
                }
                catch (Exception)
                {


                }




            }

        }

        private void TaskLb_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TaskTp.Visibility = Visibility.Visible;
            TaskTp.DataContext = (sender as Label).Tag;
            TaskTp.IsOpen = true;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void IntervalCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            interval = IntervalCbx.SelectedIndex;
            if (interval == 0 || interval == 1)
            {
                int day = ((int)EndDate.DayOfWeek);
                if (day != 0)
                {
                    EndDate = EndDate.AddDays(7 - day);
                }
                if (interval == 0)
                {
                    StartDate = EndDate.AddDays(-6);
                }
                else
                {
                    StartDate = EndDate.AddDays(-13);
                }
            }
            if (interval == 2)
            {
                StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
            }
            if (interval == 3)
            {
                StartDate = new DateTime(StartDate.Year, 1, 1);
                EndDate = StartDate.AddYears(1).AddDays(-1);
            }

            EndDateLb.Content = EndDate.Date.ToString("dd MMMM yyyy");
            StartDateLb.Content = StartDate.Date.ToString("dd MMMM yyyy");

            gantLoad();

        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            interval = IntervalCbx.SelectedIndex;
            if (interval == 0)
            {
                StartDate = StartDate.AddDays(-7);
                EndDate = EndDate.AddDays(-7);
            }
            if (interval == 1)
            {
                StartDate = StartDate.AddDays(-14);
                EndDate = EndDate.AddDays(-14);
            }
            if (interval == 2)
            {
                StartDate = StartDate.AddMonths(-1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
            }
            if (interval == 3)
            {
                StartDate = StartDate.AddYears(-1);
                EndDate = StartDate.AddYears(1).AddDays(-1);
            }


            EndDateLb.Content = EndDate.Date.ToString("dd MMMM yyyy");
            StartDateLb.Content = StartDate.Date.ToString("dd MMMM yyyy");
            gantLoad();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            interval = IntervalCbx.SelectedIndex;
            if (interval == 0)
            {
                StartDate = StartDate.AddDays(7);
                EndDate = EndDate.AddDays(7);
            }
            if (interval == 1)
            {
                StartDate = StartDate.AddDays(14);
                EndDate = EndDate.AddDays(14);
            }
            if (interval == 2)
            {
                StartDate = StartDate.AddMonths(1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
            }
            if (interval == 3)
            {
                StartDate = StartDate.AddYears(1);
                EndDate = StartDate.AddYears(1).AddDays(-1);
            }


            EndDateLb.Content = EndDate.Date.ToString("dd MMMM yyyy");
            StartDateLb.Content = StartDate.Date.ToString("dd MMMM yyyy");
            gantLoad();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntervalCbx.SelectedIndex = 0;
            gantLoad();
        }

        private void SizeSl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            gantLoad();
        }

        private void ClBtn_Click(object sender, RoutedEventArgs e)
        {
            TaskTp.Visibility = Visibility.Collapsed;
            TaskTp.DataContext = null;
            TaskTp.IsOpen = false;
        }
    }
}
