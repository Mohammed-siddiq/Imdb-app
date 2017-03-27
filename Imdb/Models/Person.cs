﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imdb.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string  Sex { get; set; }
        public DateTime Dob { get; set; }
        public string Bio { get; set; }
    }
}