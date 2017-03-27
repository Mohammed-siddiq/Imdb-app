using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Repository.Interfaces
{
    public interface IProducerRepository : IDisposable
    {
        IEnumerable<Producer> GetProducers();
        Producer GetProducerByID(int ProducerId);
        void InsertProducer(Producer Producer);
        void DeleteProducer(int ProducerID);
        void UpdateProducer(Producer Producer);
        void Save();


    }
}
