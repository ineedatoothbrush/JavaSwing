using DeThi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DeThi
{
    public partial class MainWindow : Window
    {
        private QLCongNhanContext db = new QLCongNhanContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbo.ItemsSource = db.Phongs.ToList();
            cbo.DisplayMemberPath = "TenPhong";
            cbo.SelectedValuePath = "MaPhong";
            //dgSach.ItemsSource = db.Saches.ToList();    => nếu đầu bài chỉ yêu cầu show danh sách ko có điều kiện gì thì dùng
            LoadData();
        }

        //hiển thị danh sách được sắp xếp Lương tăng dần ngay khi chạy chương trình
        private void LoadData()
        {
            var data = db.CongNhans
                .Include(cn => cn.MaPhongNavigation)
                .Select(cn => new
                {
                    cn.MaCongNhan,
                    cn.HoTen,
                    cn.NgayCong,
                    cn.PhuCap,
                    cn.MaPhong,
                    Luong = cn.NgayCong * 120000 + cn.PhuCap
                })
                .OrderBy(cn => cn.Luong)
                .ToList();
            datagrid.ItemsSource = data;
        }


        //Click 1 dòng trên DataGrid -> Hiển thị thông tin trên các textblock
        private void dgSach_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                try
                {
                    Type t = datagrid.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();

                    txtMaCongNhan.Text = p[0].GetValue(datagrid.SelectedItem).ToString();
                    txtHoTen.Text = p[1].GetValue(datagrid.SelectedItem).ToString();
                    txtNgayCong.Text = p[2].GetValue(datagrid.SelectedItem).ToString();
                    txtPhuCap.Text = p[3].GetValue(datagrid.SelectedItem).ToString();
                    cbo.SelectedValue = p[4].GetValue(datagrid.SelectedItem).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi chọn hàng: " + ex.Message, "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 2. Check Validation
        private bool Check()
        {
            // Kiểm tra dữ liệu không được bỏ trống
            if (string.IsNullOrWhiteSpace(txtMaCongNhan.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtNgayCong.Text) ||
                string.IsNullOrWhiteSpace(txtPhuCap.Text) ||
                cbo.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtNgayCong.Text, out int ngayCong) || ngayCong < 1 || ngayCong > 31)
            {
                MessageBox.Show("Ngày công phải là số nguyên và nằm trong khoảng từ 1 đến 31 !", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }


        // 3. Thêm
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
                {
                    if (db.CongNhans.Any(c => c.MaCongNhan == txtMaCongNhan.Text))
                    {
                        MessageBox.Show("Mã công nhân đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    CongNhan item = new()
                    {
                        MaCongNhan = txtMaCongNhan.Text,
                        HoTen = txtHoTen.Text,
                        NgayCong = int.Parse(txtNgayCong.Text),
                        PhuCap = int.Parse(txtPhuCap.Text),
                        MaPhong = cbo.SelectedValue.ToString()
                    };
                    db.CongNhans.Add(item);
                    db.SaveChanges();
                    //datagrid.ItemsSource = db.CongNhans.ToList();
                    LoadData();
                    MessageBox.Show("Thêm thành công!");
                }
            else
            {
                MessageBox.Show("Dữ liệu không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // 4. Sửa  => Lưu ý là ko sửa mã sách (nếu đề bài yêu cầu)
        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CongNhan s = db.CongNhans.Find(txtMaCongNhan.Text);
            try
            {
                if (Check())
                {
                    if (s.MaCongNhan.ToString() != txtMaCongNhan.Text)
                    {
                        MessageBox.Show("Không được thay đổi Mã công nhân!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    s.HoTen = txtHoTen.Text;
                    s.MaPhong = cbo.SelectedValue.ToString();
                    s.NgayCong = int.Parse(txtNgayCong.Text);
                    s.PhuCap = int.Parse(txtPhuCap.Text);
                    db.CongNhans.Update(s);
                    db.SaveChanges();
                    //datagrid.ItemsSource = db.CongNhans.ToList();
                    LoadData() ;
                    MessageBox.Show("Sửa thành công!");
                }
            }
            catch
            {
                MessageBox.Show("Không được thay đổi Mã công nhân!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        //hàm dùng để khi xóa 1 hàng thì làm trống các textfield 
        private void Clear_Field()
        {
            txtMaCongNhan.Clear();
            txtHoTen.Clear();
            txtNgayCong.Clear();
            txtPhuCap.Clear();
            cbo.SelectedIndex = -1;
        }

        // 5. Xóa  1 item khi nhập vào textbox 1 mã của item
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            //nếu đầu bài yêu cầu chọn dòng để xóa

            //if (datagrid.SelectedItem == null)
            //{
            //    MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                CongNhan s = db.CongNhans.Find(txtMaCongNhan.Text);
                if (s != null)
                {
                    db.CongNhans.Remove(s);
                }
                try
                {
                    db.SaveChanges();
                    Clear_Field();
                    //datagrid.ItemsSource = db.CongNhans.ToList();
                    LoadData();
                }
                catch
                {
                    MessageBox.Show("Có lỗi khi xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 6. Hiển thị thống kê tổng tiền sách theo tác giả
        private void btnTK_Click(object sender, RoutedEventArgs e)
        {
            DetailWindow tkWindow = new DetailWindow();
            tkWindow.ShowDialog();
        }

       
    }
}