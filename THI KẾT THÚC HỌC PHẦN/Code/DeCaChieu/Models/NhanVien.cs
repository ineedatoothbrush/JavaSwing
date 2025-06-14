using System;
using System.Collections.Generic;

namespace DeCaChieu.Models
{
    public partial class NhanVien
    {
        public string MaNhanVien { get; set; } = null!;
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string? MaPhongBan { get; set; }

        public virtual PhongBan? MaPhongBanNavigation { get; set; }
    }
}
