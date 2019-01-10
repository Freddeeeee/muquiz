using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MuQuiz.Models.Entities
{
    public partial class MuquizContext : DbContext
    {
        public MuquizContext()
        {
        }

        public MuquizContext(DbContextOptions<MuquizContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GameSession> GameSession { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Song> Song { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<GameSession>(entity =>
            {
                entity.ToTable("GameSession", "dbm");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GameId)
                    .IsRequired()
                    .HasColumnName("GameID")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.HostConnectionId)
                    .HasColumnName("HostConnectionID")
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player", "dbm");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConnectionId)
                    .IsRequired()
                    .HasColumnName("ConnectionID")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.GameSessionId).HasColumnName("GameSessionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.HasOne(d => d.GameSession)
                    .WithMany(p => p.Player)
                    .HasForeignKey(d => d.GameSessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Player__GameSess__75A278F5");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question", "dbm");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Answer1)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Answer2)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Answer3)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.CorrectAnswer)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SongId).HasColumnName("SongID");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__SongID__656C112C");
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.ToTable("Song", "dbm");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Artist)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SongName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SpotifyId)
                    .IsRequired()
                    .HasColumnName("SpotifyID")
                    .HasMaxLength(32);
            });
        }
    }
}
