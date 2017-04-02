using Imdb.Models;
using Imdb.Repository.Interfaces;
using Imdb.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Imdb.Repository
{
    public class MovieRepository : IMovieRepository, IDisposable
    {
        private ImdbContext context;
        MovieViewModel NewMovie;
        public MovieRepository(ImdbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Movie> GetMovies()
        {
             return (context.Movies.Include(m => m.Actors.Select(p => p.Person)).Include(pr => pr.Producer.Person).ToList());
            //return context.Movies.ToList();
        }

       

        public Movie GetMovieByID(int? id)
        {
            return context.Movies.Include(a => a.Actors.Select(p => p.Person)).Include(p => p.Producer.Person).FirstOrDefault(x => x.Id == id);
        }

        public void InsertMovie(NewMovieViewModel Movie)
        {

            Movie movie = new Movie();
            List<Actor> actors = new List<Actor>();

            if (!Movie.AllActors)
            {
                
                for (int i = 0; i < Movie.ActorsId.Count; i++)
                {
                    Actor a = context.Actors.Find(Movie.ActorsId[i]);
                    actors.Add(a);
                }

            }
            else
            {
                foreach(Person p in NewMovie.Actors)
                {
                    Actor a;
                    if (!Exist(p))
                    {
                        a= new Actor();

                        a.Person = p;
                        actors.Add(a);

                    }
                    else
                    {
                        a = (from act in context.Actors
                             where act.Person.Name == p.Name && act.Person.Dob == p.Dob
                             select act).FirstOrDefault();
                        actors.Add(a);

                    }

                }
            }

            movie.Actors = actors;
            movie.Name = Movie.Name;
            movie.Plot = Movie.Plot;
            if (Movie.Poster != null && Movie.Poster.ContentLength > 0)
            {
                var uploadDir = "~/Uploads";
                //var imagePath = Path.Combine(Server.MapPath(uploadDir), Movie.Poster.FileName);
                var imageUrl = Path.Combine(uploadDir, Movie.Poster.FileName);
               // Movie.Poster.SaveAs(imagePath);
                movie.Poster = imageUrl;
            }
            else if(Movie.PosterPath.Length!=0)
            {
                movie.Poster = Movie.PosterPath;
            }
            else
            {
                movie.Poster = "N/A";
            }
            context.Movies.Add(movie);
            // context.Movies.Add(Movie);
        }

        private bool Exist(Person p)
        {
            var persons = context.Actors.Include(a => a.Person).ToList();
            foreach (Actor person in persons)
            {
                if (person.Person.Name.Equals(p.Name.ToLower()) && person.Person.Dob.Equals(p.Dob))
                    return true;
            }
            return false;

        }

        public void DeleteMovie(int MovieID)
        {
            Movie Movie = context.Movies.Find(MovieID);
            context.Movies.Remove(Movie);
        }

        public void UpdateMovie(NewMovieViewModel Movie)
        {
            List<Actor> actors = new List<Actor>();

            for (int i = 0; i < Movie.ActorsId.Count; i++)
            {
                Actor a = context.Actors.Find(Movie.ActorsId[i]);
                actors.Add(a);

            }
                Movie m = context.Movies.Find(Movie.Id);
            m.Name = Movie.Name.ToLower();
            m.Plot = Movie.Plot;
            m.Producer = context.Producers.Find(Movie.Producer);
            m.Actors = actors;

            }

      


        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}