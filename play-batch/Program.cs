using System;
using System.Diagnostics;
using batch.Services.T411;

namespace batch
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var movies = MovieService.Instance.GetMovies();
            sw.Stop();
            Console.WriteLine($"movies: {movies.Count}");
            Console.WriteLine($"time elapsed: {sw.ElapsedMilliseconds}ms");
            Console.ReadKey();
        }
    }
}