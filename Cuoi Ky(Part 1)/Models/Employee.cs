using System;
using System.Collections.Generic;

namespace Cuoi_Ky_Part_1_.Models;

public partial class Employee
{
    public string Code { get; set; } = null!;

    public string? FullName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }
}
