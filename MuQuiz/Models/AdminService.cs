using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuQuiz.Models.Entities;
using MuQuiz.Models.ViewModels;

namespace MuQuiz.Models
{
    public class AdminService
    {
        private readonly MuquizContext muquizContext;

        public AdminService(MuquizContext muquizContext)
        {
            this.muquizContext = muquizContext;
        }

        internal async Task AddSong(AdminSongVM vm)
        {
            await muquizContext.Song.AddAsync(new Song
            {
                SpotifyId = vm.SpotifyId,
                Artist = vm.Artist,
                SongName = vm.SongName,
                Year = vm.Year
            });
            await muquizContext.SaveChangesAsync();

            var newSong = await muquizContext.Song.SingleAsync(s => s.SpotifyId == vm.SpotifyId);
            await muquizContext.Question.AddAsync(new Question
            {
                CorrectAnswer = vm.CorrectAnswer,
                Answer1 = vm.Answer1,
                Answer2 = vm.Answer2,
                Answer3 = vm.Answer3,
                QuestionType = vm.QuestionType,
                SongId = newSong.Id
            });
            await muquizContext.SaveChangesAsync();
        }

        internal SongItem[] GetAllSongs()
        {
            return muquizContext.Song.Select(s => new SongItem
            {
                Id = s.Id,
                SongName = s.SongName,
                Artist = s.Artist
            }).ToArray();
        }
    }

    public class SongItem
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public string Artist { get; set; }
    }
}