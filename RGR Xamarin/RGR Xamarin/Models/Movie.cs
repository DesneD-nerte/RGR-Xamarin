using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RGR_Xamarin.Models
{
    internal class Movie
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? Release { get; set; }
        
        [ManyToMany(typeof(Movie_Actor))]
        public List<Actor> Actors { get; set; }

        public Movie()
        {
            Actors = new List<Actor>();
        }

    }
}
