using entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace batch.Services.Database
{
    public class DBService
    {
        private readonly PlayContext _ctx;
        public DBService(string db, string connection)
        {
            _ctx = new PlayContext(db, connection);
        }

        public void Insert(IEnumerable<Models.Movie> movies)
        {
            var t = movies.SelectMany(m => m.Torrents).ToList();
            InsertTorrents(t);
            InsertSources(t);

            var g = movies.SelectMany(m => m.Genres).ToList();
            InsertGenres(g);

            var c = movies.Select(m => m.Collection).ToList();
            InsertCollections(c);

            var torrents = _ctx.Torrent.Select(to => to).ToList();
            var genres = _ctx.Genre.Select(ge => ge).ToList();
            var collections = _ctx.Collection.Select(co => co).ToList();
            var sources = _ctx.Source.Select(so => so).ToList();

            foreach (var movie in movies)
            {
                var mov = new Movie
                {
                    CollectionId = collections.SingleOrDefault(co => co.TmdbId == movie.Collection?.Id)?.Id,
                    TraktId = movie.TraktId,
                    ImdbId = movie.ImdbId,
                    TmdbId = movie.TmdbId,
                    Title = movie.Title,
                    TitleSearch = movie.TitleSearch,
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
                    PosterPath = movie.PosterPath,
                    CreatedAt = DateTime.Now
                };
                _ctx.Movie.Add(mov);
            }
            _ctx.SaveChanges();

            foreach (var movie in movies)
            {
                var mov = _ctx.Movie.Local.Single(m => m.ImdbId == movie.ImdbId);
                foreach (var gen in movie.Genres)
                {
                    _ctx.MovieGenre.Add(new MovieGenre
                    {
                        MovieId = mov.Id,
                        GenreId = genres.Single(ge => ge.TmdbId == gen.Id).Id,
                    });
                }
                    
                foreach (var tor in movie.Torrents)
                {
                    var torr = torrents.Single(to => to.Guid == tor.Guid);
                    torr.MovieId = mov.Id;
                    torr.SourceId = sources.Single(so => so.Name == ((Models.Source)torr.SourceId).ToString()).Id;
                }
            }
            _ctx.SaveChanges();
        }

        private void InsertCollections(IEnumerable<Models.Collection> c)
        {
            var collections = c.GroupBy(co => co?.Id).Where(co => co.Key != null).Select(co => co.First());
            foreach (var collection in collections)
            {
                _ctx.Collection.Add(new Collection
                {
                    TmdbId = collection.Id,
                    Name = collection.Name,
                    PosterPath = collection.PosterPath,
                    BackdropPath = collection.BackdropPath,
                    CreatedAt = DateTime.Now
                });
            }
            _ctx.SaveChanges();
        }

        private void InsertGenres(IEnumerable<Models.Genre> g)
        {
            var genres = g.GroupBy(ge => ge.Id).Where(ge => ge != null).Select(ge => ge.First());
            foreach (var genre in genres)
            {
                _ctx.Genre.Add(new Genre
                {
                    TmdbId = genre.Id,
                    Name = genre.Name,
                    CreatedAt = DateTime.Now
                });
            }
            _ctx.SaveChanges();
        }

        private void InsertSources(IEnumerable<Models.Torrent> t)
        {
            var sources = t.GroupBy(s => s.Source).Where(s => s != null).Select(s => s.Key);
            foreach (var source in sources)
            {
                _ctx.Source.Add(new Source
                {
                    Name = source.ToString(),
                    CreatedAt = DateTime.Now
                });
            }
            _ctx.SaveChanges();
        }

        private void InsertTorrents(IEnumerable<Models.Torrent> torrents)
        {
            InsertCategories();
            InsertLanguages();
            InsertQualities();

            var categories = _ctx.Category.Select(c => new Category { Id = c.Id, Name = c.Name });
            var languages = _ctx.Language.Select(l => new Language { Id = l.Id, Name = l.Name });
            var qualities = _ctx.Quality.Select(q => new Quality { Id = q.Id, Name = q.Name });

            foreach (var torrent in torrents)
            {
                _ctx.Add(new Torrent
                {
                    Guid = torrent.Guid,
                    Name = torrent.Name,
                    Slug = torrent.Slug,
                    Year = torrent.Year,
                    Size = (long)torrent.Size,
                    Seeders = torrent.Seeders,
                    Leechers = torrent.Leechers,
                    Completed = torrent.Completed,
                    SourceId = (int)torrent.Source,
                    CategoryId = categories.SingleOrDefault(c => c.Name.Equals(torrent.Category.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0,
                    LanguageId = languages.SingleOrDefault(l => l.Name.Equals(torrent.Language.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0,
                    QualityId = qualities.SingleOrDefault(q => q.Name.Equals(torrent.Quality.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Id ?? 0,
                    CreatedAt = DateTime.Now
                });
            }
            _ctx.SaveChanges();
        }

        private void InsertCategories()
        {
            _ctx.Category.Add(new Category { Name = Models.Category.Series.ToString().ToLower(), CreatedAt = DateTime.Now });
            _ctx.Category.Add(new Category { Name = Models.Category.Movie.ToString().ToLower(), CreatedAt = DateTime.Now });
            _ctx.SaveChanges();
        }

        private void InsertLanguages()
        {
            _ctx.Language.Add(new Language { Name = Models.Language.VF.ToString().ToLower(), CreatedAt = DateTime.Now });
            _ctx.Language.Add(new Language { Name = Models.Language.VOSTFR.ToString().ToLower(), CreatedAt = DateTime.Now });
            _ctx.SaveChanges();
        }

        private void InsertQualities()
        {
            _ctx.Quality.Add(new Quality { Name = Models.Quality.Low.ToString().ToLower(), CreatedAt = DateTime.Now });
            _ctx.Quality.Add(new Quality { Name = Models.Quality.Medium.ToString().ToLower(), CreatedAt = DateTime.Now });
            _ctx.Quality.Add(new Quality { Name = Models.Quality.High.ToString().ToLower(), CreatedAt = DateTime.Now });
            _ctx.Quality.Add(new Quality { Name = Models.Quality.VeryHigh.ToString().ToLower(), CreatedAt = DateTime.Now });
            _ctx.SaveChanges();
        }
    }
}
