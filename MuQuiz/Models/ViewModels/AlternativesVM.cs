using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models.ViewModels
{
    public class AlternativesVM
    {
        public string Alternative { get; set; }
        public int IsRightAlt { get; set; } = 0;
    }
}
