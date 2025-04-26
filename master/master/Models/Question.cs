using System;
using System.Collections.Generic;

namespace master.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int? CategoryId { get; set; }

    public string? QuestionText { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();
}
