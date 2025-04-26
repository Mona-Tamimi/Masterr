using System;
using System.Collections.Generic;

namespace master.Models;

public partial class QuestionOption
{
    public int OptionId { get; set; }

    public int? QuestionId { get; set; }

    public string? OptionText { get; set; }

    public virtual Question? Question { get; set; }
}
