using System;
using System.Collections.Generic;

namespace BaiMau.Models;

public partial class TacGia
{
    public string MaTg { get; set; } = null!;

    public string TenTacGia { get; set; } = null!;

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
