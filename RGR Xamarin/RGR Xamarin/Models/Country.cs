using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RGR_Xamarin.Models
{
    internal class Country
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
