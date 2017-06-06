﻿using System;
using System.Diagnostics;
using batch.Services.Torrents;
using batch.Services.Tmdb;

namespace batch
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var torrents = MovieTorrentsService.Instance.GetMovies();
            var moviesIds = MovieExtractorsService.Instance.GetMoviesIds(torrents);
            var movies = MovieExtractorsService.Instance.GetMovies(moviesIds);
            sw.Stop();
                       
            Console.WriteLine($"time elapsed: {sw.ElapsedMilliseconds}ms");
            Console.ReadKey();
        }
    }
}