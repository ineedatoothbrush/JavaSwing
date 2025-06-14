using System;
using System.Collections.Generic;

namespace BaiMau.Models;

public partial class Sach
{
    public string MaSach { get; set; } 

    public string TenSach { get; set; } 

    public int? NamXuatBan { get; set; }

    public int? SoTrang { get; set; }

    public string MaTg { get; set; }

    public virtual TacGia? TacGia { get; set; }

    public decimal? ThanhTien => SoTrang * 80000;
}
