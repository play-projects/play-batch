using System;
using System.Diagnostics;
using System.Linq;
using batch.Models;
using batch.Services.Torrents;
using batch.Services.Tmdb;

namespace batch
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            //var torrents = MovieTorrentsService.Instance.GetMovies();
            //var movies = MovieExtractorsService.Instance.GetMoviesIds(torrents);
            sw.Stop();

            var movie = MovieExtractorsService.Instance.GetMovie(new Movie {TmdbId = 263115});

            //Console.WriteLine($"torrents: {torrents.Count}");
            //Console.WriteLine($"movies: {movies.Count}");

            Console.WriteLine($"time elapsed: {sw.ElapsedMilliseconds}ms");
            Console.ReadKey();
        }
    }
}