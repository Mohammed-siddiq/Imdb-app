using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Repository.Interfaces
{
    public interface IPersonRepository:IDisposable
    {

        IEnumerable<Person> GetPersons();
        Person GetPersonByID(int? PersonId);
        void InsertPerson(Person Person);
        void DeletePerson(int PersonID);
        void UpdatePerson(Person Person);
        void Save();

    }
}
