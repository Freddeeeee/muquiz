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
        private readonly MuquizContext context;

        public AdminService(MuquizContext context)
        {
            this.context = context;
        }

        internal async Task AddSong(AdminAddEditSongVM vm)
        {
            await AddToSongTable(vm);
            await AddToQuestionTable(vm);
        }

        private async Task AddToSongTable(AdminAddEditSongVM vm)
        {
            await context.Song.AddAsync(new Song
            {
                SpotifyId = vm.SpotifyId,
                Artist = vm.Artist,
                SongName = vm.SongName,
                Year = vm.Year
            });

            await context.SaveChangesAsync();
        }

        private async Task AddToQuestionTable(AdminAddEditSongVM vm)
        {
            var newSong = await context.Song.SingleAsync(s => s.SpotifyId == vm.SpotifyId);
            await context.Question.AddAsync(new Question
            {
                CorrectAnswer = $"{vm.Artist} - {vm.SongName}",
                Answer1 = vm.Answer1,
                Answer2 = vm.Answer2,
                Answer3 = vm.Answer3,
                QuestionType = 1, // remove hard-coding if alternatives added
                SongId = newSong.Id
            });

            await context.SaveChangesAsync();
        }

        internal async Task DeleteSong(int id)
        {
            context.Question.RemoveRange(context.Question.Where(q => q.SongId == id));
            context.Song.Remove(context.Song.Single(s => s.Id == id));
            await context.SaveChangesAsync();
        }

        internal async Task UpdateSong(AdminAddEditSongVM vm)
        {
            var song = await context.Song.SingleAsync(s => s.Id == vm.SongId);
            var question = await context.Question.SingleAsync(q => q.SongId == vm.SongId);

            await UpdateInSongTable(song, vm);
            await UpdateInQuestionTable(question, vm);
        }

        private async Task UpdateInSongTable(Song song, AdminAddEditSongVM vm)
        {
            song.SpotifyId = vm.SpotifyId;
            song.Artist = vm.Artist;
            song.SongName = vm.SongName;
            song.Year = vm.Year;
            await context.SaveChangesAsync();
        }

        private async Task UpdateInQuestionTable(Question question, AdminAddEditSongVM vm)
        {
            question.CorrectAnswer = $"{vm.Artist} - {vm.SongName}";
            question.Answer1 = vm.Answer1;
            question.Answer2 = vm.Answer2;
            question.Answer3 = vm.Answer3;

            await context.SaveChangesAsync();
        }

        internal async Task<AdminShowSongVM[]> GetAllSongs()
        {
            return await context.Song.Select(s => new AdminShowSongVM
            {
                Id = s.Id,
                SongName = s.SongName,
                Artist = s.Artist
            }).ToArrayAsync();
        }

        internal async Task<AdminAddEditSongVM> GetSongForUpdate(int id)
        {
            var song = await context.Song.SingleAsync(s => s.Id == id);
            var question = await context.Question.SingleAsync(q => q.SongId == id);
            return new AdminAddEditSongVM
            {
                Artist = song.Artist,
                SongName = song.SongName,
                Year = song.Year,
                SpotifyId = song.SpotifyId,
                Answer1 = question.Answer1,
                Answer2 = question.Answer2,
                Answer3 = question.Answer3,
                SongId = id
            };
        }
    }
}