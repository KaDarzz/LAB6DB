using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Lab6.Models;

public partial class ServiceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<Services> Services { get; set; } = new List<Services>();
}
