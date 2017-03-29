using Imdb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imdb.ViewModels
{
    public class NewMovieViewModel
    {


        public int Id { get; set; }
        public List<Actor> Actors { get; set; }

        public string PosterPath { get; set; }
        //[Required]
        public Person Person { get; set; }
        public List<Producer> Producers { get; set; }

        public Boolean AllActors { get; set; }
        [Required]
        public string Name { get; set; }

        //    public string Poster { get; set; }


        [DisplayName("Year Of Release")]
        [Required]
        [Range(1700,2030)]
        public int YearOfRelease { get; set; }

        public string Plot { get; set; }

        [Required]
        [DisplayName("Actors")]

        public List<int> ActorsId { get; set; }

        [Required]
        public int Producer { get; set; }


        [DataType(DataType.Upload)]
        public HttpPostedFileBase Poster { get; set; }
        public NewMovieViewModel()
        {
            Actors = new List<Actor>();
            Producers = new List<Producer>();
            ActorsId = new List<int>();
        }

    }
}