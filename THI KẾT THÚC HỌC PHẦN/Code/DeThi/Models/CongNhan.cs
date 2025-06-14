using System;
using System.Collections.Generic;

namespace DeThi.Models
{
    public partial class CongNhan
    {
        public string MaCongNhan { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public int NgayCong { get; set; }
        public int PhuCap { get; set; }
        public string? MaPhong { get; set; }

        public decimal? Luong => NgayCong*120000 + PhuCap;
        public virtual Phong? MaPhongNavigation { get; set; }


        

    }
}
