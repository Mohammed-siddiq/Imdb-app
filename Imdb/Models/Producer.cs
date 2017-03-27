using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imdb.Models
{
    public class Producer
    {
        public int Id { get; set; }
        public Person Person { get; set; }
        public int PersonId { get; set; }
        public List<Movie> Movies { get; set; }
    }
}