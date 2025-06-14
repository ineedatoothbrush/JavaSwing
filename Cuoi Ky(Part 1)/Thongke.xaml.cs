using Cuoi_Ky_Part_1_.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Cuoi_Ky_Part_1_
{
    /// <summary>
    /// Interaction logic for Thongke.xaml
    /// </summary>
    public partial class Thongke : Window
    {
        public Thongke()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataContext db = new DataContext())
            {
                var stats = db.Employees.GroupBy(employee => employee.Department.Name)
                    .Select(emp => new
                    {
                        Name = emp.Key,
                        Count = emp.Count()
                    }).ToList();
                thongke.ItemsSource = stats;
            }
        }
    }
}
