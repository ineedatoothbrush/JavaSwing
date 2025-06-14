using System;
using System.Collections.Generic;

namespace Cuoi_Ky_Part_1_.Models;

public partial class Department
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
