using RGR_Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.MultiSelectListView;

namespace RGR_Xamarin.ViewModels
{
    internal class MoviesViewModel : BaseViewModel
    {
        private Movie _selectedMovie = new Movie();
        public Movie SelectedMovie
        {
            get
            {
                return _selectedMovie;
            }
            set
            {
                _selectedMovie = value;
                base.OnPropertyChanged("SelectedMovie");
            }
        }

        public MultiSelectObservableCollection<Actor> Actors { get; set; }
        public ObservableCollection<Movie> Movies { get; set;}

        public MoviesViewModel()
        {
            Title = "Movies";

            Actors = new MultiSelectObservableCollection<Actor>(App.DataBase.GetActorsAsync().Result);
            Movies = new ObservableCollection<Movie>(App.DataBase.GetMoviesAsync().Result);

            LoadItemsCommand = new Command(async () => { await GetAllMovies(); await GetAllActors(); });

            AddMovieCommand = new Command(async () => await AddMovie());

            UpdateMovieCommand = new Command(async () => await UpdateMovie());

            DeleteMovieCommand = new Command(async () => await DeleteMovie());

            SetIsSelectedActorsCommand = new Command(async () => await SetIsSelectedActors());

            AddActorCommand = new Command( (selectedItem) => AddActorToSelectedMovie(selectedItem));
        }

        public ICommand CheckActorCommand { get; }
        public ICommand AddMovieCommand { get; }
        public ICommand UpdateMovieCommand { get; }
        public ICommand DeleteMovieCommand { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand AddActorCommand { get; }
        public ICommand SetIsSelectedActorsCommand { get; }

        public void AddActorToSelectedMovie(object selectedItem)
        {
            //if(!SelectedMovie.Actors.Contains(selectedItem))
            //{
            //    SelectedMovie.Actors.Add((Actor)selectedItem);
            //}
            //else
            //{
            //    SelectedMovie.Actors.Remove((Actor)selectedItem);
            //}
            if (!SelectedMovie.Actors.Exists(actor => actor.Id == ((Actor)selectedItem).Id))
            {
                SelectedMovie.Actors.Add((Actor)selectedItem);
            }
            else
            {
                SelectedMovie.Actors.RemoveAll(actor => actor.Id == ((Actor)selectedItem).Id);
            }
        }

        public Task SetIsSelectedActors()
        {
            return Task.Run(() =>
            {
                Actors.ForEach(actor => actor.IsSelected = false);
                for (int i = 0; i < Actors.Count; i++)
                {
                    SelectableItem<Actor> actor = Actors[i];

                    //bool check = Movies[i].Actors.Exists(oneActor => oneActor.Id == actor.Data.Id);
                    bool check = SelectedMovie.Actors.Exists(oneActor => oneActor.Id == actor.Data.Id);
                    if (check)
                    {
                        actor.IsSelected = true;
                    }
                }
            });
        }

        public async Task GetAllMovies()
        {
            List<Movie> movies = await App.DataBase.GetMoviesAsync();//В списках актеров не будет установлено Country, но будет Id_Country

            #region//Установка полноценной ссылки на актеров, возможно не надо 
            foreach (var movie in movies)
            {
                for (int i = 0; i < movie.Actors.Count; i++)
                {
                    for (int j = 0; j < Actors.Count; j++)
                    {
                        if(Actors[j].Data.Id == movie.Actors[i].Id)
                        {
                            movie.Actors[i] = Actors[j].Data;
                        }
                    }
                }
            }
            #endregion

            Movies.Clear();

            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }

            IsBusy = false;
        }

        public async Task GetAllActors()
        {
            List<Actor> actors = await App.DataBase.GetActorsAsync();

            Actors.Clear();

            foreach (var actor in actors)
            {
                Actors.Add(actor);
            }

            IsBusy = false;
        }

        public async Task AddMovie()
        {
            if (!string.IsNullOrWhiteSpace(SelectedMovie.Name))
            {
                await App.DataBase.SaveMoviesAsync(SelectedMovie);

                IsBusy = true;

                SelectedMovie = new Movie();
            }
            
            IsBusy = false;
        }

        public async Task UpdateMovie()
        {
            if (checkConditionalMovie())
            {
                await App.DataBase.UpdateMovieAsync(SelectedMovie);

                IsBusy = true;

                SelectedMovie = new Movie();
            }
            else
            {
                await DisplayNotEditMovie();
            }

            IsBusy = false;
        }

        public async Task DeleteMovie()
        {
            if (checkConditionalMovie())
            {
                await App.DataBase.DeleteMovieAsync(SelectedMovie);

                IsBusy = true;

                SelectedMovie = new Movie();
            } 
            else
            {
                await DisplayNotEditMovie();
            }

            IsBusy = false;
        }

        private async Task DataBaseOperation(Task dbAction)
        {
            //await dbAction();
            //int countUpdatedRows = await dbAction;

            //if (countUpdatedRows == 0)
            //{
            //    await App.Current.MainPage.DisplayAlert("Message", "Не была отредактирован фильм", "OK");
            //}
        }

        private async Task DisplayNotEditMovie()
        {
            await App.Current.MainPage.DisplayAlert("Message", "Не был выбран фильм", "OK");
        }

        private bool checkConditionalMovie()
        {
            return SelectedMovie.Id != 0 && !string.IsNullOrWhiteSpace(SelectedMovie.Name);
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
