using System;
using System.Diagnostics;
using System.Linq;
using batch.Services;
using batch.Services.Database;
using batch.Services.Torrents;
using batch.Services.Tmdb;

namespace batch
{
    class Program
    {
        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var movieService = new MovieTorrentsService();
            var torrents = movieService.GetMovies();
            sw.Stop();
            var swTorrents = sw.ElapsedMilliseconds;

            sw.Restart();
            var moviesIds = MovieExtractorsService.Instance.GetMoviesIds(torrents);
            sw.Stop();
            var swMoviesIds = sw.ElapsedMilliseconds;

            sw.Restart();
            var movies = MovieExtractorsService.Instance.GetMovies(moviesIds);
            sw.Stop();
            var swMovies = sw.ElapsedMilliseconds;

            sw.Restart();
            new DBService().Insert(movies);
            sw.Stop();

            Console.WriteLine($"torrents: {torrents.Count()}");
            Console.WriteLine($"ids: {moviesIds.Count()}");
            Console.WriteLine($"movies: {movies.Count()}");
            Console.WriteLine($"time elapsed for torrents: {swTorrents}ms");
            Console.WriteLine($"time elapsed for movies id: {swMoviesIds}ms");
            Console.WriteLine($"time elapsed for movies: {swMovies}ms");
            Console.WriteLine($"time elapsed for db insertions: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"time elapsed: {swTorrents + swMoviesIds + swMovies + sw.ElapsedMilliseconds}ms");
            Console.ReadKey();
        }
    }
}