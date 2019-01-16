using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models.ViewModels
{
    public class HostSetupVM
    {
        [Required(ErrorMessage = "You must select the number of songs per game.")]
        [Range(3, 10, ErrorMessage = "The number of songs must be between {0} and {1}.")]
        public int NumberOfSongs { get; set; }
    }
}
