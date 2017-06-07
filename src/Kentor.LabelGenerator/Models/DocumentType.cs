using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator.Models
{
    public enum DocumentType
    {
        [Display(Name = "2 kolumner")]
        A4_2Columns8Rows = 10,

        [Display(Name = "3 kolumner")]
        A4_3Columns8Rows = 20,
    }
}
