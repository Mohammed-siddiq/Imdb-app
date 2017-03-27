using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imdb.Models
{
    public class Actor
    {
        public int Id { get; set; }

        public Person Person { get; set; }

        public int PersonId { get; set; }

        public virtual List<Movie> Movies { get; set; }

    }
}