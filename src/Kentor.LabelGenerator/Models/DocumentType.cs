using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator.Models
{
    public enum DocumentType
    {
        [Display(Name = "Adressetiketter 70 x 36 mm")]
        A4_70x36_L7181 = 5,

        [Display(Name = "Adressetiketter 70 x 37 mm")]
        A4_70x37_L7180 = 10,

        [Display(Name = "Adressetiketter 99,1 x 33,9 mm")]
        A4_99x33_L7162 = 15,

        [Display(Name = "Adressetiketter 105 x 37 mm")]
        A4_105x37_L7182 = 20,
    }
}
