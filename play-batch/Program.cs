using System;
using System.Diagnostics;
using System.IO;
using batch.Services.Database;
using batch.Services.Torrents;
using batch.Services.Tmdb;
using entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            var torrents = MovieTorrentsService.Instance.GetMovies();
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

            Console.WriteLine($"torents: {torrents.Count}");
            Console.WriteLine($"ids: {moviesIds.Count}");
            Console.WriteLine($"movies: {movies.Count}");
            Console.WriteLine($"time elapsed for torrents: {swTorrents}ms");
            Console.WriteLine($"time elapsed for movies id: {swMoviesIds}ms");
            Console.WriteLine($"time elapsed for movies: {swMovies}ms");
            Console.WriteLine($"time elapsed for db insertions: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"time elapsed: {swTorrents + swMoviesIds + swMovies + sw.ElapsedMilliseconds}ms");
            Console.ReadKey();
        }
    }
}