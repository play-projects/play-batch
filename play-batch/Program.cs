using System;
using batch.Services.T411;

namespace batch
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = MovieService.Instance.GetMovies();
            Console.ReadKey();
        }
    }
}