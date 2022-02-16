using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RGR_Xamarin.Models
{
    internal class Actor

    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDay { get; set; }

        [ForeignKey(typeof(Country))]
        public int Id_Country { get; set; }
        [OneToOne]
        public Country Country { get; set; }

        [ManyToMany(typeof(Movie_Actor))]
        public List<Movie> Movies { get; set; }

        public Actor(Actor actor)
        {
            Id = actor.Id;
            Name = actor.Name;
            Surname = actor.Surname;
            BirthDay = actor.BirthDay;
            Id_Country = actor.Id_Country;
            Country = actor.Country;
            Movies = actor.Movies;
        }

        public Actor()
        {

        }
    }
}
