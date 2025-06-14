using System;
using System.Collections.Generic;

namespace DeThi.Models
{
    public partial class Phong
    {
        public Phong()
        {
            CongNhans = new HashSet<CongNhan>();
        }

        public string MaPhong { get; set; } = null!;
        public string TenPhong { get; set; } = null!;

        public virtual ICollection<CongNhan> CongNhans { get; set; }
    }
}
