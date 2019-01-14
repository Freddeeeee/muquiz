using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models.ViewModels
{
    public class AdminSongVM
    {
        [Required(ErrorMessage = "Enter song name")]
        [StringLength(64, ErrorMessage = "Must be between 1-64 characters", MinimumLength = 1)]
        [Display(Name = "Title")]
        public string SongName { get; set; }

        [Required(ErrorMessage = "Enter artist")]
        [StringLength(64, ErrorMessage = "Must be between 1-64 characters", MinimumLength = 1)]
        [Display(Name = "Artist")]
        public string Artist { get; set; }

        [Required(ErrorMessage = "Enter year released")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Enter a SpotifyID")]
        [StringLength(22, ErrorMessage = "ID must be 22 characters long", MinimumLength = 22)]
        [Display(Name = "Spotify Song-ID")]
        public string SpotifyId { get; set; }

        [Required(ErrorMessage = "Enter correct answer")]
        [StringLength(64, ErrorMessage = "Must be between 1-64 characters", MinimumLength = 1)]
        [Display(Name = "Correct answer")]
        public string CorrectAnswer { get; set; }

        [Required(ErrorMessage = "Enter incorrect answer #1")]
        [StringLength(64, ErrorMessage = "Must be between 1-64 characters", MinimumLength = 1)]
        [Display(Name = "Incorrect answer #1")]
        public string Answer1 { get; set; }

        [Required(ErrorMessage = "Enter incorrect answer #2")]
        [StringLength(64, ErrorMessage = "Must be between 1-64 characters", MinimumLength = 1)]
        [Display(Name = "Incorrect answer #2")]
        public string Answer2 { get; set; }

        [Required(ErrorMessage = "Enter incorrect answer #3")]
        [StringLength(64, ErrorMessage = "Must be between 1-64 characters", MinimumLength = 1)]
        [Display(Name = "Incorrect answer #3")]
        public string Answer3 { get; set; }

        [Required(ErrorMessage = "Select question type")]
        [Display(Name = "Question type")]
        public int QuestionType { get; set; }
    }
}
