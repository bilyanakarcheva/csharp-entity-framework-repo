namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {

        //create ctors (for current context and dbcontext both) - DONE
        //context to inherit - DONE
        //dbset
        //connect to sql server - DONE
        //fluent api relation - Many to Many - DONE

        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Performer> Performers { get; set; }

        public DbSet<Producer> Producers { get; set; }

        public DbSet<Song> Songs { get; set;}

        public DbSet<SongPerformer> SongsPerformers { get; set;}

        public DbSet<Writer> Writers { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Song>(x =>
            {
                x.HasMany(x => x.SongPerformers)
                .WithOne(x => x.Song);
            });

            builder.Entity<Performer>(x =>
            {
                x.HasMany(x => x.PerformerSongs)
                .WithOne(x => x.Performer);
            });

            builder.Entity<SongPerformer>(x =>
            {
                x.HasKey(x => new { x.PerformerId, x.SongId});
            });

            builder.Entity<Writer>(x =>
            {
                x.HasMany(x => x.Songs)
                .WithOne(x => x.Writer);
            });

            builder.Entity<Album>(x =>
            {
                x.HasMany(x => x.Songs)
                .WithOne(x=> x.Album);
            });


            builder.Entity<Producer>(x =>
            {
                x.HasMany(x => x.Albums)
                .WithOne(x => x.Producer);
            });
        }
    }
}
