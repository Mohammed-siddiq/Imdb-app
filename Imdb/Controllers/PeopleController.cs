using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Imdb.Models;
using Imdb.Repository.Interfaces;

namespace Imdb.Controllers
{
    public class PeopleController : Controller
    {
        private ImdbContext db = new ImdbContext();

        private IPersonRepository personRepository;
        public PeopleController(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }
        public PeopleController()
        {

        }


        // GET: People
        public ActionResult Index()
        {
            return View(db.People.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = personRepository.GetPersonByID(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Sex,Dob,Bio")] Person person)
        {
            if (ModelState.IsValid)
            {
                personRepository.InsertPerson(person);
                personRepository.Save();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = personRepository.GetPersonByID(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Sex,Dob,Bio")] Person person)
        {
            if (ModelState.IsValid)
            {
                personRepository.UpdatePerson(person);
                personRepository.Save();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = personRepository.GetPersonByID(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            personRepository.DeletePerson(id);
            personRepository.Save();
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
