using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imdb.Models
{
        public class Movie
        {

            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Plot { get; set; }
            
            public int YearOfRelease { get; set; }

            public string Poster { get; set; }
            
            
            public virtual List<Actor> Actors { get; set; }

            public int ProducerId { get; set; }

            public virtual Producer Producer { get; set; }
    }
}