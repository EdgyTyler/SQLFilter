using System;
using System.Collections.Generic;

namespace SQLFilter.Models
{
    public partial class ProfanityFilter
    {
        public int Id { get; set; }
        public string BadWord { get; set; } = null!;
    }

}