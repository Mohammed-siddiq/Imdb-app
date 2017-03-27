using Imdb.Models;
using Imdb.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Imdb.Repository
{
    public class ActorRepository : IActorRepository, IDisposable
    {
        private ImdbContext context;

        public ActorRepository(ImdbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Actor> GetActors()
        {
            return context.Actors.ToList();
        }

        public Actor GetActorByID(int id)
        {
            return context.Actors.Find(id);
        }

        public void InsertActor(Actor Actor)
        {
            context.Actors.Add(Actor);
        }

        public void DeleteActor(int ActorID)
        {
            Actor Actor = context.Actors.Find(ActorID);
            context.Actors.Remove(Actor);
        }

        public void UpdateActor(Actor Actor)
        {
            context.Entry(Actor).State = EntityState.Modified;
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