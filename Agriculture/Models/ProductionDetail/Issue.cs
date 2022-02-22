﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.ProductionDetail
{
    public class Issue
    {
        public List<IssueDetail> issueDetails { get; set; } = new List<IssueDetail>();

    }

    public class IssueDetail
    {
        [Required(ErrorMessage = "Date Is Required.")]
        public DateTime Date { get; set; }
        public DateTime DateTime { get; set; }
        public Models.Common.IntegerNullString LoginDetail { get; set; } = new Common.IntegerNullString();
        public Models.Common.IntegerNullString MainArea { get; set; } = new Models.Common.IntegerNullString();
        public Models.Common.IntegerNullString SubArea { get; set; } = new Models.Common.IntegerNullString();
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "IssueQuantity Required.")]
        public float IssueQuantity { get; set; }

        public string Remark { get; set; }
        public Models.Common.IntegerNullString Product { get; set; } = new Models.Common.IntegerNullString();

    }
}
