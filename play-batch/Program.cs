using System;
using platch.Services.T411;

namespace platch
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