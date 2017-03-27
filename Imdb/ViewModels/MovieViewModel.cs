using Imdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imdb.ViewModels
{
    public class MovieViewModel
    {
        public string Name { get; set; }

        public string Poster { get; set; }
        public string Plot { get; set; }

        public int YearOfRealease { get; set; }

        public List<Person> Actors { get; set; }

        public Person Producer { get; set; }

        public MovieViewModel()
        {
            Actors = new List<Person>();
        }
    }
}