using System;
using System.Collections.Generic;

namespace master.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
