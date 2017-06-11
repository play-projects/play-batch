using entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace batch.Services.Database
{
    public class DBService
    {
        public static DBService Instance = new DBService();

        private readonly PlayContext context;

        private DBService()
        {
            context = new PlayContext();
        }

        public void Insert(List<Models.Movie> movies)
        {
            var t = movies.SelectMany(m => m.Torrents).ToList();
            InsertTorrents(t);

            var g = movies.SelectMany(m => m.Genres).ToList();
            InsertGenres(g);

            var c = movies.Select(m => m.Collection).ToList();
            InsertCollections(c);

            var torrents = context.Torrent.Select(to => to).ToList();
            var genres = context.Genre.Select(ge => ge).ToList();
            var collections = context.Collection.Select(co => co).ToList();

            foreach (var movie in movies)
            {
                var mov = new Movie
                {
                    CollectionId = collections.SingleOrDefault(co => co.TmdbId == movie.Collection?.Id)?.Id,
                    TraktId = movie.TraktId,
                    ImdbId = movie.ImdbId,
                    TmdbId = movie.TmdbId,
                    Title = movie.Title,
                    OriginalTitle = movie.OriginalTitle,
                    Year = movie.Year,
                    OriginalLanguage = movie.OriginalLanguage,
                    Overview = movie.Overview,
                    Tagline = movie.Tagline,
                    Popularity = (decimal)movie.Popularity,
                    ReleaseDate = movie.ReleaseDate,
                    VoteAverage = (decimal)movie.VoteAverage,
                    VoteCount = movie.VoteCount,
                    Runtime = movie.Runtime,
                    BackdropPath = movie.BackdropPath,
                    PosterPath = movie.PosterPath
                };
                context.Movie.Add(mov);
            }
            context.SaveChanges();

            foreach (var movie in movies)
            {
                var mov = context.Movie.Local.Where(m => m.ImdbId == movie.ImdbId).Single();
                foreach (var gen in movie.Genres)
                {
                    context.MovieGenre.Add(new MovieGenre
                    {
                        MovieId = mov.Id,
                        GenreId = genres.Where(ge => ge.TmdbId == gen.Id).Single().Id
                    });
                }
                    
                foreach (var tor in movie.Torrents)
                {
                    torrents.Where(to => to.T411Id == tor.Id).Single().MovieId = mov.Id;
                }
            }
            context.SaveChanges();
        }

        private void InsertCollections(List<Models.Collection> c)
        {
            var collections = c.GroupBy(co => co?.Id).Where(co => co.Key != null).Select(co => co.First());
            foreach (var collection in collections)
            {
                context.Collection.Add(new Collection
                {
                    TmdbId = collection.Id,
                    Name = collection.Name,
                    PosterPath = collection.PosterPath,
                    BackdropPath = collection.BackdropPath
                });
            }
            context.SaveChanges();
        }

        private void InsertGenres(List<Models.Genre> g)
        {
            var genres = g.GroupBy(ge => ge.Id).Where(ge => ge != null).Select(ge => ge.First());
            foreach (var genre in genres)
            {
                context.Genre.Add(new Genre
                {
                    TmdbId = genre.Id,
                    Name = genre.Name
                });
            }
            context.SaveChanges();
        }

        private void InsertTorrents(List<Models.Torrent> torrents)
        {
            InsertCategories();
            InsertLanguages();
            InsertQualities();

            var categories = context.Category.Select(c => new Category { Id = c.Id, Name = c.Name });
            var languages = context.Language.Select(l => new Language { Id = l.Id, Name = l.Name });
            var qualities = context.Quality.Select(q => new Quality { Id = q.Id, Name = q.Name });

            foreach (var torrent in torrents)
            {
                context.Add(new Torrent
                {
                    T411Id = torrent.Id,
                    Name = torrent.Name,
                    Slug = torrent.Slug,
                    Year = torrent.Year,
                    Size = (long)torrent.Size,
                    Seeders = torrent.Seeders,
                    Leechers = torrent.Leechers,
                    Completed = torrent.Completed,
                    CategoryId = categories.SingleOrDefault(c => c.Name.Equals(torrent.Category.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0,
                    LanguageId = languages.SingleOrDefault(l => l.Name.Equals(torrent.Language.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0,
                    QualityId = qualities.SingleOrDefault(q => q.Name.Equals(torrent.Quality.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0
                });
            }
            context.SaveChanges();
        }

        private void InsertCategories()
        {
            context.Category.Add(new Category { Name = Models.Category.Series.ToString().ToLower() });
            context.Category.Add(new Category { Name = Models.Category.Movie.ToString().ToLower() });
            context.SaveChanges();
        }

        private void InsertLanguages()
        {
            context.Language.Add(new Language { Name = Models.Language.VF.ToString().ToLower() });
            context.Language.Add(new Language { Name = Models.Language.VOSTFR.ToString().ToLower() });
            context.SaveChanges();
        }

        private void InsertQualities()
        {
            context.Quality.Add(new Quality { Name = Models.Quality.Low.ToString().ToLower() });
            context.Quality.Add(new Quality { Name = Models.Quality.Medium.ToString().ToLower() });
            context.Quality.Add(new Quality { Name = Models.Quality.High.ToString().ToLower() });
            context.Quality.Add(new Quality { Name = Models.Quality.VeryHigh.ToString().ToLower() });
            context.SaveChanges();
        }
    }
}
