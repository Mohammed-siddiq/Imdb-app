using Imdb.Models;
using Imdb.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Imdb.Repository
{
    public class PersonRepository : IPersonRepository, IDisposable
    {
        private ImdbContext context;

        public PersonRepository(ImdbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Person> GetPersons()
        {
            return context.People.ToList();
        }

        public Person GetPersonByID(int? id)
        {
            return context.People.Find(id);
        }

        public void InsertPerson(Person Person)
        {
            context.People.Add(Person);
        }

        public void DeletePerson(int PersonID)
        {
            Person Person = context.People.Find(PersonID);
            context.People.Remove(Person);

        }

        public void UpdatePerson(Person Person)
        {
            Person p = GetPersonByID(Person.Id);
            p.Name = Person.Name;
            p.Sex = Person.Sex;
            p.Dob = Person.Dob;
            p.Bio = Person.Bio;
            Save();
          //  context.Entry(Person).State = EntityState.Modified;
           
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