using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Repository.Interfaces
{
  public  interface IActorRepository:IDisposable
    {

        IEnumerable<Actor> GetActors();
        Actor GetActorByID(int ActorId);
        void InsertActor(Actor Actor);
        void DeleteActor(int ActorID);
        void UpdateActor(Actor Actor);
        void Save();

    }
}
