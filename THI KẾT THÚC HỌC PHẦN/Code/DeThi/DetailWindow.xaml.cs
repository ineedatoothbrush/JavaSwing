﻿using DeThi.Models;
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

namespace DeThi
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
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
                using (QLCongNhanContext db = new QLCongNhanContext())
                {
                    var query = db.CongNhans
                     .GroupBy(cn => new { cn.MaPhong, cn.MaPhongNavigation.TenPhong })
                     .Select(g => new
                     {
                         MaPhong = g.Key.MaPhong,
                         TenPhong = g.Key.TenPhong,
                         TongLuong = g.Sum(cn => cn.NgayCong * 120000 + cn.PhuCap)
                     })
                     .OrderBy(x => x.MaPhong)
                     .ToList();

                    dgThongKe.ItemsSource = query;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
