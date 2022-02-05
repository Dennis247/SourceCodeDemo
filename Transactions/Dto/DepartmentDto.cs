﻿using System.ComponentModel.DataAnnotations;

namespace Transactions.Dto
{
    public class DepartmentDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
