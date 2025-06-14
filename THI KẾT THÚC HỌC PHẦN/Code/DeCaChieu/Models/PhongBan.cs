using System;
using System.Collections.Generic;

namespace DeCaChieu.Models
{
    public partial class PhongBan
    {
        public PhongBan()
        {
            NhanViens = new HashSet<NhanVien>();
        }

        public string MaPhongBan { get; set; } = null!;
        public string TenPhongBan { get; set; } = null!;

        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
