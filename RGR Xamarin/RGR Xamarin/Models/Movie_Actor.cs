using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RGR_Xamarin.Models
{
    internal class Movie_Actor
    {
        [ForeignKey(typeof(Movie))]
        public int Id_Movie { get; set; }

        [ForeignKey(typeof(Actor))]
        public int Id_Actor { get; set; }
    }
}
