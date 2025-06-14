using DeCaChieu.Models;
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

namespace DeCaChieu
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
                using (QLNSContext db = new QLNSContext())
                {
                    var danhSach = (from dc in db.NhanViens
                                    join pb in db.PhongBans on dc.MaPhongBan equals pb.MaPhongBan
                                    select new 
                                    {
                                        MaNhanVien = dc.MaNhanVien,
                                        TuNgay = dc.TuNgay.ToString("dd/MM/yyyy"),
                                        DenNgay = dc.DenNgay.ToString("dd/MM/yyyy"),
                                        TenPhongBan = pb.TenPhongBan
                                    }).ToList();

                    dgThongKe.ItemsSource = danhSach;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
