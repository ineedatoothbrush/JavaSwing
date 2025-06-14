using BaiMau.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace BaiMau
{
    public partial class MainWindow : Window
    {
        private QlsachContext db = new QlsachContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        //hàm dùng để khi chạy chương trình thì datagrid đã được đổ sẵn dữ liệu theo yêu cầu của đầu bài (yêu cầu làm trong hàm ShowList())
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboTacGia.ItemsSource = db.TacGia.ToList();
            cboTacGia.DisplayMemberPath = "TenTacGia";
            cboTacGia.SelectedValuePath = "MaTg";
            //dgSach.ItemsSource = db.Saches.ToList();    => nếu đầu bài chỉ yêu cầu show danh sách ko có điều kiện gì thì dùng
            dgSach.ItemsSource = ShowList(); // =>  show 1 danh sách có điều kiện
        }

        //1. Hiển thị danh sách sách có số trang >=120, sắp xếp giảm dần theo số trang
        private List<Sach> ShowList()
        {
            var books = db.Saches
                //.Where(s => s.SoTrang >= 120)
                .OrderByDescending(s => s.SoTrang)
                .ToList();
            return books;

        }

        

        //Click 1 dòng trên DataGrid -> Hiển thị thông tin trên các textblock
        private void dgSach_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgSach.SelectedItem != null)
            {
                try
                {
                    Type t = dgSach.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();

                    txtMaSach.Text = p[0].GetValue(dgSach.SelectedItem).ToString();
                    txtTenSach.Text = p[1].GetValue(dgSach.SelectedItem).ToString();
                    txtNamXB.Text = p[2].GetValue(dgSach.SelectedItem).ToString();
                    txtSoTrang.Text = p[3].GetValue(dgSach.SelectedItem).ToString();
                    cboTacGia.SelectedValue = p[4].GetValue(dgSach.SelectedItem).ToString();
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
            if (string.IsNullOrWhiteSpace(txtMaSach.Text) ||
                string.IsNullOrWhiteSpace(txtTenSach.Text) ||
                string.IsNullOrWhiteSpace(txtNamXB.Text) ||
                string.IsNullOrWhiteSpace(txtSoTrang.Text) ||
                cboTacGia.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            // Kiểm tra Số trang và Năm xuất bản là số nguyên > 0
            if (!int.TryParse(txtNamXB.Text, out int namXuatBan) || namXuatBan <= 0)
            {
                MessageBox.Show("Năm xuất bản phải là số nguyên lớn hơn 0!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!int.TryParse(txtSoTrang.Text, out int soTrang) || soTrang <= 0)
            {
                MessageBox.Show("Số trang phải là số nguyên lớn hơn 0!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            
            return true;
        }

        // 3. Thêm
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {      
            try
            {
                if (Check())
                {
                    string maSach = txtMaSach.Text.Trim();
                    if (db.Saches.Any(s => s.MaSach == maSach))
                    {
                        MessageBox.Show("Mã đã tồn tại! Vui lòng nhập mã khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Sach newBook = new()
                    {
                        MaSach = txtMaSach.Text,
                        TenSach = txtTenSach.Text,
                        NamXuatBan = int.Parse(txtNamXB.Text),
                        SoTrang = int.Parse(txtSoTrang.Text),
                        MaTg = cboTacGia.SelectedValue.ToString()
                    };
                    db.Saches.Add(newBook);
                    db.SaveChanges();
                    //dgSach.ItemsSource = db.Saches.ToList();     => trường hợp muốn show danh sách gốc trong db
                    dgSach.ItemsSource = ShowList();   // show danh sách có điều kiện ở ý 1
                    MessageBox.Show("Thêm thành công!");
                }
            }          
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // 4. Sửa  => Lưu ý là ko sửa mã sách (nếu đề bài yêu cầu)
        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgSach.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Sach s = db.Saches.Find(txtMaSach.Text);
            try
            {
                if (Check())
                {
                    if (s.MaSach.ToString() != txtMaSach.Text)
                    {
                        MessageBox.Show("Không được thay đổi Mã sách!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                        s.TenSach = txtTenSach.Text;
                        s.MaTg = cboTacGia.SelectedValue.ToString();
                        s.NamXuatBan = int.Parse(txtNamXB.Text);
                        s.SoTrang = int.Parse(txtSoTrang.Text);
                        db.Saches.Update(s);
                        db.SaveChanges();
                        //dgSach.ItemsSource = db.Saches.ToList();
                        dgSach.ItemsSource = ShowList();
                        MessageBox.Show("Sửa thành công!");
                   
                    
                }
            }
            catch
            {
                MessageBox.Show("Không được thay đổi mã sách!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
            }

        //hàm dùng để khi xóa 1 hàng thì làm trống các textfield 
        private void Clear_Field()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtNamXB.Clear();
            txtSoTrang.Clear();
            cboTacGia.SelectedIndex = -1;
        }

        // 5. Xóa 1 item khi nhập vào textbox 1 mã của item
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            //nếu đầu bài yêu cầu chọn dòng để xóa

            //if (dgSach.SelectedItem == null)
            //{
            //    MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Sach s = db.Saches.Find(txtMaSach.Text);
                if (s != null)
                {
                    db.Saches.Remove(s);
                }
                try
                {
                    db.SaveChanges();
                    Clear_Field();
                    //dgSach.ItemsSource = db.Saches.ToList();
                    dgSach.ItemsSource = ShowList();

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