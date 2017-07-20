using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using batch.Services.Database;
using batch.Services.Torrents;
using batch.Services.Tmdb;
using Microsoft.Extensions.Configuration;

namespace batch
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var sw = Stopwatch.StartNew();
            var next = Configuration["nextorrent_movies_url"];
            var torrent9 = Configuration["torrent9_movies_url"];
            var omg = Configuration["omgtorrent_movies_url"];
            var lien = Configuration["lientorrent_movies_url"];
            var cpasbien = Configuration["cpasbien_movies_url"];
            var leets = Configuration["1337x_movies_url"];

            var movieService = new MovieTorrentsService(next, torrent9, omg, lien, cpasbien, leets);
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
            var db = Configuration["db"];
            var connection = Configuration["connection_strings:play_db"];
            new DBService(db, connection).Insert(movies);
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