using Cuoi_Ky_Part_1_.Models;
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

namespace Cuoi_Ky_Part_1_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {    
        private DataContext db = new DataContext();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboDepartment.ItemsSource = db.Departments.ToList();
            cboDepartment.DisplayMemberPath = "Name";
            cboDepartment.SelectedValuePath = "Id";
            danhsach.ItemsSource = db.Employees.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = new Employee
            {
                Code = txtCode.Text,
                FullName = txtFullName.Text,
                DateOfBirth = DateOnly.FromDateTime(dpDOB.SelectedDate.Value),
                DepartmentId = cboDepartment.SelectedValue.ToString()
            };  
            db.Employees.Add(employee);
            try
            {
                db.SaveChanges();
                danhsach.ItemsSource = db.Employees.ToList(); 
                MessageBox.Show("Thêm nhân viên thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nhân viên: {ex.Message}");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Employee employee = db.Employees.Find(txtCode.Text);
            if (employee != null)
            {
                db.Remove(employee);
                try
                {
                    db.SaveChanges();
                    danhsach.ItemsSource = db.Employees.ToList();
                    MessageBox.Show("Xóa nhân viên thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật nhân viên: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Nhân viên không tồn tại.");
            }
            ClearControls();
        }
        private void ClearControls()
        {
            foreach (var control in data.Children)
            {
                if (control is TextBox textBox)
                {
                    textBox.Clear();
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = -1; // hoặc = 0 nếu bạn muốn mặc định chọn mục đầu tiên
                }
                else if (control is DatePicker datePicker)
                {
                    datePicker.SelectedDate = null;
                }
            }
        }

        private void danhsach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee employee = (Employee)danhsach.SelectedItem;
            if(employee != null)
            {
                txtCode.Text = employee.Code;
                txtFullName.Text = employee.FullName;
                dpDOB.SelectedDate = employee.DateOfBirth?.ToDateTime(new TimeOnly(0, 0));
                cboDepartment.SelectedValue = employee.DepartmentId;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Employee employee = db.Employees.Find(txtCode.Text);
            if (employee != null)
            {
                employee.FullName = txtFullName.Text;
                employee.DateOfBirth = DateOnly.FromDateTime(dpDOB.SelectedDate.Value);
                employee.DepartmentId = cboDepartment.SelectedValue.ToString();
                try
                {
                    db.SaveChanges();
                    danhsach.ItemsSource = db.Employees.ToList();
                    MessageBox.Show("Cập nhật nhân viên thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật nhân viên: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Nhân viên không tồn tại.");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new Thongke().ShowDialog();
        }
    }
}