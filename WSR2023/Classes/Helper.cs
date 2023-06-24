using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WSR2023.Models;

namespace WSR2023.Classes
{
    internal class Helper
    {
        public static WSR2023Entities1 Db = new WSR2023Entities1();

        public static Project SelectedProject = null;

        public static void Error(string message = "Ошибка подключения к БД")
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void Info(string message = "Ошибка подключения к БД")
        {
            MessageBox.Show(message, "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
        }


    }
}
