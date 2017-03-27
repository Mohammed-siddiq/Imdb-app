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
            else
            {
                movie.Poster = Movie.PosterPath;
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

        public dynamic GetMovieDetails(string MovieName)
        {
             NewMovie = new MovieViewModel();
           // NewMovieViewModel model = new NewMovieViewModel();
            string url = "https://api.themoviedb.org/3/search/movie?api_key=3fae25f1c2fc7c75b43ef7583be8adbf&language=en-US&query=" + MovieName;
            var RequestMovie = new HttpRequestMessage(HttpMethod.Get, url);
            var httpclient = new HttpClient();
            HttpResponseMessage ResponseMovie = null;
            try
            {
                 ResponseMovie = httpclient.SendAsync(RequestMovie).Result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(ResponseMovie.Content.ReadAsStringAsync().Result);
            var enumerableResults = (json["results"] as IEnumerable<dynamic>).ToList();
           
            if (enumerableResults == null || enumerableResults.Count==0)
            {
                return null;
            }
            var result = enumerableResults[0];
            string overview = result["overview"];
            string releasedate = result["release_date"];

            NewMovie.Poster = "http://image.tmdb.org/t/p/w185/" + result["poster_path"];
            NewMovie.Name = result["title"];
            NewMovie.Plot = overview;
            
            NewMovie.YearOfRealease = int.Parse(releasedate.Substring(0, 4));

            int id = result["id"];
            List<Person> movieactors = new List<Person>();
            url = "https://api.themoviedb.org/3/movie/" + id + "/credits?api_key=3fae25f1c2fc7c75b43ef7583be8adbf";
            RequestMovie = new HttpRequestMessage(HttpMethod.Get, url);
            ResponseMovie = httpclient.SendAsync(RequestMovie).Result;
            json = serializer.Deserialize<object>(ResponseMovie.Content.ReadAsStringAsync().Result);
            var Casts = (json["cast"] as IEnumerable<dynamic>).ToList();
            var Crew = (json["crew"] as IEnumerable<dynamic>).ToList();
            for (int i = 0,counter=0; i < Casts.Count; i++,counter++)
            {
                if (counter == 3)
                    break;
                Person p = new Person();
                var cast = Casts[i];
                string name = cast["name"];
                int cid = cast["id"];
                try
                {
                    url = "https://api.themoviedb.org/3/person/" + cid + "?api_key=3fae25f1c2fc7c75b43ef7583be8adbf&language=en-US";
                    RequestMovie = new HttpRequestMessage(HttpMethod.Get, url);
                    ResponseMovie = httpclient.SendAsync(RequestMovie).Result;
                    json = serializer.Deserialize<object>(ResponseMovie.Content.ReadAsStringAsync().Result);
                    p.Name = json["name"];
                    Debug.WriteLine("Name:\t", p.Name);
                    p.Bio = json["biography"];
                    Debug.WriteLine("Biography:\t", p.Bio);
                    if (json["birthday"] != null && json["birthday"].Length != 0)
                        p.Dob = new DateTime(int.Parse(json["birthday"].Substring(0, 4)), int.Parse(json["birthday"].Substring(5, 2)), int.Parse(json["birthday"].Substring(8, 2)));
                    p.Sex = (json["gender"] == 1) ? "Female" : "Male";

                
                }
                catch (Exception e)
                {
                    continue;
                }


                NewMovie.Actors.Add(p);


            }

            foreach (var c in Crew)
            {

                string name = c["name"];
                string role = c["job"];
                Debug.WriteLine(role + ":" + name);
                if (role.Equals("Producer"))
                {
                    Person p = new Person();
                    int cid = c["id"];
                    try
                    {
                        url = "https://api.themoviedb.org/3/person/" + cid + "?api_key=3fae25f1c2fc7c75b43ef7583be8adbf&language=en-US";
                        RequestMovie = new HttpRequestMessage(HttpMethod.Get, url);
                        ResponseMovie = httpclient.SendAsync(RequestMovie).Result;
                        json = serializer.Deserialize<object>(ResponseMovie.Content.ReadAsStringAsync().Result);
                        // string s = "As";

                        p.Name = json["name"];
                        if (json["biography"] != null)
                            p.Bio = json["biography"];
                        else
                            p.Bio = "Bio Not Available!";
                        p.Sex = (json["gender"] == 1) ? "Female" : "Male";
                        if (json["birthday"] != null && json["birthday"].Length != 0)
                            p.Dob = new DateTime(int.Parse(json["birthday"].Substring(0, 4)), int.Parse(json["birthday"].Substring(5, 2)), int.Parse(json["birthday"].Substring(8, 2)));

                    }
                    catch (Exception e)
                    {

                        Debug.WriteLine(e.Message);
                                   
                    }
                    finally
                    {
                        NewMovie.Producer = p;
                        

                    }
                    break;
                }
            }

            var jsonResult = serializer.Serialize(NewMovie);

            return jsonResult;



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