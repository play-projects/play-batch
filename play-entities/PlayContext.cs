using Microsoft.EntityFrameworkCore;

namespace entities.Models
{
    public partial class PlayContext : DbContext
    {
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Character> Character { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<MovieGenre> MovieGenre { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Quality> Quality { get; set; }
        public virtual DbSet<Torrent> Torrent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=ARTHUR-PC;Database=PLAY;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("CATEGORY");

                entity.Property(e => e.Id)
                    .HasColumnName("CATEGORY_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("CATEGORY_NAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("CATEGORY_CREATED_AT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.MovieId })
                    .HasName("PK_CHARACTER");

                entity.ToTable("CHARACTER");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

                entity.Property(e => e.MovieId).HasColumnName("MOVIE_ID");

                entity.Property(e => e.Name)
                    .HasColumnName("CHARACTER_NAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Order).HasColumnName("CHARACTER_ORDER");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("CHARACTER_CREATED_AT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("COLLECTION");

                entity.Property(e => e.Id)
                    .HasColumnName("COLLECTION_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.BackdropPath)
                    .HasColumnName("COLLECTION_BACKDROP_PATH")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Name)
                    .HasColumnName("COLLECTION_NAME")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.PosterPath)
                    .HasColumnName("COLLECTION_POSTER_PATH")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.TmdbId).HasColumnName("COLLECTION_TMDB_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("COLLECTION_CREATED_AT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("GENRE");

                entity.Property(e => e.Id)
                    .HasColumnName("GENRE_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("GENRE_NAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.TmdbId).HasColumnName("GENRE_TMDB_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("GENRE_CREATED_AT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("LANGUAGE");

                entity.Property(e => e.Id)
                    .HasColumnName("LANGUAGE_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("LANGUAGE_NAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("LANGUAGE_CREATED_AT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("MOVIE");

                entity.Property(e => e.Id)
                    .HasColumnName("MOVIE_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CollectionId).HasColumnName("COLLECTION_ID");

                entity.Property(e => e.BackdropPath)
                    .HasColumnName("MOVIE_BACKDROP_PATH")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.ImdbId)
                    .HasColumnName("MOVIE_IMDB_ID")
                    .HasColumnType("varchar(25)");

                entity.Property(e => e.OriginalLanguage)
                    .HasColumnName("MOVIE_ORIGINAL_LANGUAGE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OriginalTitle)
                    .HasColumnName("MOVIE_ORIGINAL_TITLE")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Overview)
                    .HasColumnName("MOVIE_OVERVIEW")
                    .HasColumnType("varchar(2500)");

                entity.Property(e => e.Popularity)
                    .HasColumnName("MOVIE_POPULARITY")
                    .HasColumnType("decimal");

                entity.Property(e => e.PosterPath)
                    .HasColumnName("MOVIE_POSTER_PATH")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("MOVIE_RELEASE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Runtime).HasColumnName("MOVIE_RUNTIME");

                entity.Property(e => e.Tagline)
                    .HasColumnName("MOVIE_TAGLINE")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.Title)
                    .HasColumnName("MOVIE_TITLE")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.TmdbId).HasColumnName("MOVIE_TMDB_ID");

                entity.Property(e => e.TraktId).HasColumnName("MOVIE_TRAKT_ID");

                entity.Property(e => e.VoteAverage)
                    .HasColumnName("MOVIE_VOTE_AVERAGE")
                    .HasColumnType("decimal");

                entity.Property(e => e.VoteCount).HasColumnName("MOVIE_VOTE_COUNT");

                entity.Property(e => e.Year).HasColumnName("MOVIE_YEAR");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("MOVIE_CREATED_AT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.HasKey(e => new { e.GenreId, e.MovieId })
                    .HasName("PK_MOVIE_GENRE");

                entity.ToTable("MOVIE_GENRE");

                entity.Property(e => e.GenreId).HasColumnName("GENRE_ID");

                entity.Property(e => e.MovieId).HasColumnName("MOVIE_ID");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("PERSON");

                entity.Property(e => e.Id)
                    .HasColumnName("PERSON_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Firstname)
                    .HasColumnName("PERSON_FIRSTNAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Lastname)
                    .HasColumnName("PERSON_LASTNAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.ProfilePath)
                    .HasColumnName("PERSON_PROFILE_PATH")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("PERSON_CREATED_AT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Quality>(entity =>
            {
                entity.ToTable("QUALITY");

                entity.Property(e => e.Id)
                    .HasColumnName("QUALITY_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("QUALITY_NAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("QUALITY_CREATED_AT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Torrent>(entity =>
            {
                entity.ToTable("TORRENT");

                entity.Property(e => e.Id)
                    .HasColumnName("TORRENT_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID");

                entity.Property(e => e.LanguageId).HasColumnName("LANGUAGE_ID");

                entity.Property(e => e.T411Id).HasColumnName("TORRENT_T411_ID");

                entity.Property(e => e.MovieId).HasColumnName("MOVIE_ID");

                entity.Property(e => e.QualityId).HasColumnName("QUALITY_ID");

                entity.Property(e => e.Completed).HasColumnName("TORRENT_COMPLETED");

                entity.Property(e => e.Leechers).HasColumnName("TORRENT_LEECHERS");

                entity.Property(e => e.Name)
                    .HasColumnName("TORRENT_NAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Seeders).HasColumnName("TORRENT_SEEDERS");

                entity.Property(e => e.Size).HasColumnName("TORRENT_SIZE");

                entity.Property(e => e.Slug)
                    .HasColumnName("TORRENT_SLUG")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Year).HasColumnName("TORRENT_YEAR");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("TORRENT_CREATED_AT")
                    .HasColumnType("datetime");
            });
        }
    }
}