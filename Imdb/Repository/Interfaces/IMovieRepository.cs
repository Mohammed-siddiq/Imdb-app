using Imdb.Models;
using Imdb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Repository.Interfaces
{
   public interface IMovieRepository:IDisposable
    {
        IEnumerable<Movie> GetMovies();
        Movie GetMovieByID(int? MovieId);
        void InsertMovie(NewMovieViewModel Movie);
        void DeleteMovie(int MovieID);
        void UpdateMovie(NewMovieViewModel Movie);
        
        void Save();


    }
}
