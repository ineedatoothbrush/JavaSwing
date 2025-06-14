using BaiMau.Models;
using Microsoft.EntityFrameworkCore;
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

namespace BaiMau
{
    public partial class DetailWindow : Window
    {
        public DetailWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (QlsachContext db = new QlsachContext())
                {
                    var query = from s in db.Saches
                                group s by s.MaTg into TGGR
                                select new
                                {
                                    MaTg = TGGR.Key,
                                    TongTien = TGGR.Sum(x => x.SoTrang * 80000)
                                };

                    var query2 = from s in query
                                 join t in db.TacGia on s.MaTg equals t.MaTg
                                 select new
                                 {
                                     t.MaTg,
                                     t.TenTacGia,
                                     s.TongTien
                                 };

                    dgThongKe.ItemsSource = query2.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }   
    }
}
