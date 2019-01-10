using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models.ViewModels
{
    public class HomeIndexVM
    {
        [Required(ErrorMessage = "Please enter a game id.")]
        [Display(Name = "Game id")]
        [StringLength(4, ErrorMessage = "The code should contain exactly 4 letters.", MinimumLength =4 )]
        public string GameId { get; set; }
    }
}
