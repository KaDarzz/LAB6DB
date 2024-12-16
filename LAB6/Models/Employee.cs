using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Lab6.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? Position { get; set; }

    public string? Phone { get; set; }

    [JsonIgnore]
    public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; } = new List<EmployeeSchedule>();

    [JsonIgnore]
    public virtual ICollection<PerformedService> PerformedServices { get; set; } = new List<PerformedService>();
}
