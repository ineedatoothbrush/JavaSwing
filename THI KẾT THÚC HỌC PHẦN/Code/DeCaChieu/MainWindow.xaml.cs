using DeCaChieu.Models;
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

namespace DeCaChieu
{
    public partial class MainWindow : Window
    {
        private QLNSContext db = new QLNSContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbo.ItemsSource = db.PhongBans.ToList();
            cbo.DisplayMemberPath = "TenPhongBan";
            cbo.SelectedValuePath = "MaPhongBan";
            //dgSach.ItemsSource = db.Saches.ToList();    => nếu đầu bài chỉ yêu cầu show danh sách ko có điều kiện gì thì dùng
            datagrid.ItemsSource = ShowList();
        }


        private List<NhanVien> ShowList()
        {
            var books = db.NhanViens
                //.Where(s => s.SoTrang >= 120)
                .OrderBy(s => s.MaNhanVien)
                .ToList();
            return books;

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
                    txtMaNhanVien.Text = p[0].GetValue(datagrid.SelectedItem).ToString();
                    dtpTuNgay.SelectedDate = p[1].GetValue(datagrid.SelectedItem) as DateTime?;
                    dtpDenNgay.SelectedDate = p[2].GetValue(datagrid.SelectedItem) as DateTime?;
                    cbo.SelectedValue = p[3].GetValue(datagrid.SelectedItem).ToString();
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
            if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text) ||
                dtpTuNgay.SelectedDate == null ||
                dtpDenNgay.SelectedDate == null ||
                cbo.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //DateTime? dateFrom = dtpDateFrom.SelectedDate;
            //DateTime? dateTo = dtpDateTo.SelectedDate;
            //if (dateFrom.Value > dateTo.Value)
            //{
            //    MessageBox.Show("'Đến ngày' phải lớn hơn hoặc bằng 'Từ ngày'!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            return true;
        }


        // 3. Thêm
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
            {
                // Kiểm tra trùng Mã NV & Từ ngày
                bool exists = db.NhanViens.Any(s => s.MaNhanVien == txtMaNhanVien.Text.Trim() && s.TuNgay == dtpTuNgay.SelectedDate.Value);
                if (exists)
                {
                    MessageBox.Show("Thông tin đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                NhanVien item = new()
                {
                    MaNhanVien = txtMaNhanVien.Text,
                    TuNgay = dtpTuNgay.SelectedDate.Value,
                    DenNgay = dtpDenNgay.SelectedDate.Value,
                    MaPhongBan = cbo.SelectedValue.ToString()
                };
                db.NhanViens.Add(item);
                db.SaveChanges();
                //datagrid.ItemsSource = db.CongNhans.ToList();
                datagrid.ItemsSource = ShowList();
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

            NhanVien s = db.NhanViens.Find(txtMaNhanVien.Text);
            try
            {
                if (Check())
                {
                    if (s.MaNhanVien.ToString() != txtMaNhanVien.Text)
                    {
                        MessageBox.Show("Không được thay đổi Mã công nhân!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    s.TuNgay = dtpTuNgay.SelectedDate.Value;
                    s.DenNgay = dtpDenNgay.SelectedDate.Value;
                    s.MaPhongBan = cbo.SelectedValue.ToString();
                    db.NhanViens.Update(s);
                    db.SaveChanges();
                    //datagrid.ItemsSource = db.CongNhans.ToList();
                    datagrid.ItemsSource = ShowList();
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
            txtMaNhanVien.Clear();
            dtpTuNgay.SelectedDate = null;
            dtpDenNgay.SelectedDate= null;
            cbo.SelectedIndex = -1;
        }

        // 5. Xóa 1 item khi nhập vào textbox 1 mã của item
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
                NhanVien s = db.NhanViens.Find(txtMaNhanVien.Text);
                if (s != null)
                {
                    db.NhanViens.Remove(s);
                }
                try
                {
                    db.SaveChanges();
                    Clear_Field();
                    //datagrid.ItemsSource = db.CongNhans.ToList();
                    datagrid.ItemsSource = ShowList();
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