using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Imdb.Models;
using Imdb.ViewModels;
using System.IO;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Imdb.Repository.Interfaces;

namespace Imdb.Controllers
{
    public class MoviesController : Controller
    {
        private ImdbContext db = new ImdbContext();

        private IMovieRepository movieRepository;
        public MoviesController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }
        public MoviesController()
        {
        }

        // GET: Movies
        public ActionResult Index()
        {
            //return View(db.Movies.Include(m => m.Actors.Select(p => p.Person)).Include(pr => pr.Producer.Person).ToList());
            return View(movieRepository.GetMovies());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = movieRepository.GetMovieByID(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {

            var ViewModel = new NewMovieViewModel
            {
                Actors = db.Actors.Include(a => a.Person).ToList(),
                Producers = db.Producers.Include(a => a.Person).ToList()
            };
            return View(ViewModel);
        }

        // POST: Movies/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Id,Name,Plot,YearOfRelease,Poster")]
        public ActionResult Create(NewMovieViewModel m)
        {
            List<Actor> actors = new List<Actor>();
            m.Person = new Person();
            for (int i = 0; i < m.ActorsId.Count; i++)
            {
                Actor a = db.Actors.Find(m.ActorsId[i]);
                actors.Add(a);
            }
            Movie mov = new Movie();

            if (ModelState.IsValid)
            {

                mov.Actors = actors;
                mov.Name = m.Name;
                mov.Plot = m.Plot;
                mov.YearOfRelease = m.YearOfRelease;
              //  mov.Poster = m.Poster;
                mov.ProducerId = m.Producer;
                if (m.Poster != null && m.Poster.ContentLength > 0)
                {
                    var uploadDir = "~/Uploads";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), m.Poster.FileName);
                    var imageUrl = Path.Combine(uploadDir, m.Poster.FileName);
                    m.Poster.SaveAs(imagePath);
                    mov.Poster = imageUrl;
                }
                else if (m.PosterPath !=null)
                {
                    mov.Poster = m.PosterPath;
                }
                else
                {
                    mov.Poster = "N/A";
                }



                db.Movies.Add(mov);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(m);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //RedirectToAction("Index", "Movies");
            }
            Movie movie = movieRepository.GetMovieByID(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            List<int> actids = new List<int>();
            foreach(var actor in movie.Actors)
            {
                actids.Add(actor.Id);
            }
            var movieModel = new NewMovieViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Plot = movie.Plot,
                YearOfRelease = movie.YearOfRelease,
                Actors = db.Actors.Include(a => a.Person).ToList(),
                Producers = db.Producers.Include(a => a.Person).ToList(),
                
                Producer = movie.ProducerId,
                ActorsId = actids


            }; 

            return View(movieModel);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewMovieViewModel movie)
        {
            movie.Person = new Person();

            if (ModelState.IsValid)
            {
                movieRepository.UpdateMovie(movie);
                movieRepository.Save();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Movie movie = db.Movies.Find(id);
            Movie movie = movieRepository.GetMovieByID(id);
            
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            movieRepository.DeleteMovie(id);
            movieRepository.Save();
            return RedirectToAction("Index");
        }

        

     
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
