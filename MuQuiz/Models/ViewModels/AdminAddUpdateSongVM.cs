using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models.ViewModels
{
    public class AdminAddEditSongVM
    {
        [Required(ErrorMessage = "Enter song title.")]
        [StringLength(64, ErrorMessage = "Song title must be between 1-64 characters", MinimumLength = 1)]
        public string SongName { get; set; }

        [Required(ErrorMessage = "Enter artist.")]
        [StringLength(64, ErrorMessage = "Artist must be between 1-64 characters", MinimumLength = 1)]
        public string Artist { get; set; }

        [Required(ErrorMessage = "Enter year released.")]
        [Range(1500, 2100, ErrorMessage ="Please double-check the year entered.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Enter a Spotify ID.")]
        [StringLength(22, ErrorMessage = "Spotify ID must be 22 characters long.", MinimumLength = 22)]
        public string SpotifyId { get; set; }

        [Required(ErrorMessage = "Enter alternative answer #1.")]
        [StringLength(64, ErrorMessage = "All alternative answers must be between 1-64 characters.", MinimumLength = 1)]
        public string Answer1 { get; set; }

        [Required(ErrorMessage = "Enter alternative answer #2.")]
        [StringLength(64, ErrorMessage = "All alternative answers must be between 1-64 characters.", MinimumLength = 1)]
        public string Answer2 { get; set; }

        [Required(ErrorMessage = "Enter alternative answer #3.")]
        [StringLength(64, ErrorMessage = "All alternative answers must be between 1-64 characters.", MinimumLength = 1)]
        public string Answer3 { get; set; }

        public int SongId { get; set; }

        //[Required(ErrorMessage = "Select question type")]
        //[Display(Name = "Question type")]
        //public int QuestionType { get; set; }
    }
}
