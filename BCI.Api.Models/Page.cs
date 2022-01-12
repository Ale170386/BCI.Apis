﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Api.Models
{
    [Table("Page")]
    public class Page
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
