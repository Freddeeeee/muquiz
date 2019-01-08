using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models.ViewModels
{
    public class PlayerIndexVM
    {
        [Required(ErrorMessage = "Please enter a name.")]
        [Display(Name="Enter your name")]
        public string Name { get; set; }
    }
}
