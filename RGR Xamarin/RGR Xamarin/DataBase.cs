using RGR_Xamarin.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace RGR_Xamarin
{
    internal class DataBase
    {
        private readonly SQLiteAsyncConnection _database;
        public DataBase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            _database.CreateTableAsync<Actor>();
            _database.CreateTableAsync<Country>();
            _database.CreateTableAsync<Movie>();
            _database.CreateTableAsync<Movie_Actor>();
        }

        //Actors
        public Task<List<Actor>> GetActorsAsync()
        {
            var actors = _database.GetAllWithChildrenAsync<Actor>();

            return actors;
        }
        public Task<int> SaveActorAsync(Actor actor)
        {
            return _database.InsertAsync(actor);
        }
        public Task<int> UpdateActorAsync(Actor actorOnChanged)
        {
            return _database.UpdateAsync(actorOnChanged);
        }
        public Task<int> DeleteActorAsync(Actor actorOnChanged)
        {
            return _database.DeleteAsync(actorOnChanged);
        }

        //Countries
        public Task<List<Country>> GetCountriesAsync()
        {
            return _database.Table<Country>().ToListAsync();
        }

        public Task<int> SaveCountriesAsync(Country country)
        {
            return _database.InsertAsync(country);
        }
        public Task<int> UpdateCountryAsync(Country countryOnChanged)
        {
            return _database.UpdateAsync(countryOnChanged);
        }
        public Task<int> DeleteCountryAsync(Country countryOnChanged)
        {
            return _database.DeleteAsync(countryOnChanged);
        }

        //Movies
        public Task<List<Movie>> GetMoviesAsync()
        {
            //return _database.Table<Movie>().ToListAsync();
            return _database.GetAllWithChildrenAsync<Movie>();
        }
        public Task SaveMoviesAsync(Movie movie)
        {
            return _database.InsertWithChildrenAsync(movie);
        }
        public Task UpdateMovieAsync(Movie movieOnChanged)
        {
            return _database.UpdateWithChildrenAsync(movieOnChanged);
        }
        public Task DeleteMovieAsync(Movie movieOnChanged)
        {
            return _database.DeleteAsync(movieOnChanged);
        }

        /// <summary>
        /// actor's data for filtering
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        internal async Task<List<Actor>> FilterActorsAsync(Actor actor)
        {
            string query = "SELECT * FROM Actor WHERE Name = ? OR Surname = ? OR BirthDay = ? OR Id_Country = ?";

            List<Actor> filteredActors = await _database.QueryAsync<Actor>(query, actor.Name, actor.Surname, actor.BirthDay, actor.Id_Country);

            List<Actor> actorsWithCountries = new List<Actor>();
            for (int i = 0; i < filteredActors.Count; i++)
            {
                Actor actor1 = await _database.GetWithChildrenAsync<Actor>(filteredActors[i].Id);
                actorsWithCountries.Add(actor1);
            }

            return actorsWithCountries;
        }

        internal async Task<List<Actor>> FilterActorsWithLinqAsync(Actor actor)
        {
            List<Actor> filteredActors = await _database.Table<Actor>().Where(a =>
                a.Name == actor.Name ||
                a.Surname == actor.Surname ||
                a.BirthDay == actor.BirthDay ||
                a.Id_Country == actor.Id_Country
            ).ToListAsync();

            List<Actor> actorsWithCountries = new List<Actor>();
            for (int i = 0; i < filteredActors.Count; i++)
            {
                Actor actor1 = await _database.GetWithChildrenAsync<Actor>(filteredActors[i].Id);
                actorsWithCountries.Add(actor1);
            }

            return actorsWithCountries;
        }
    }
}
