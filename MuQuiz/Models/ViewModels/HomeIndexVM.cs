using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models.ViewModels
{
    public class HomeIndexVM
    {
        // to-do: create attribute to check if game id exists
        [Required(ErrorMessage = "Please enter a game id.")]
        [Display(Name = "Game id")]
        public string GameId { get; set; }
    }
}
