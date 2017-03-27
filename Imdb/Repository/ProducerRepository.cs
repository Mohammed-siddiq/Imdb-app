using Imdb.Models;
using Imdb.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Imdb.Repository
{
    public class ProducerRepository : IProducerRepository, IDisposable
    {
        private ImdbContext context;

        public ProducerRepository(ImdbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Producer> GetProducers()
        {
            return context.Producers.ToList();
        }

        public Producer GetProducerByID(int id)
        {
            return context.Producers.Find(id);
        }

        public void InsertProducer(Producer Producer)
        {
            context.Producers.Add(Producer);
        }

        public void DeleteProducer(int ProducerID)
        {
            Producer Producer = context.Producers.Find(ProducerID);
            context.Producers.Remove(Producer);
        }

        public void UpdateProducer(Producer Producer)
        {
            context.Entry(Producer).State = EntityState.Modified;
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