using RGR_Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;

namespace RGR_Xamarin.ViewModels
{
    internal class ActorsViewModel : BaseViewModel
    {
        private Actor _selectedActor = new Actor();
        public Actor SelectedActor
        {
            get
            {
                return _selectedActor;
            }
            set
            {
                _selectedActor = value;
                base.OnPropertyChanged("SelectedActor");
            }
        }

        public ObservableCollection<Actor> Actors { get; set; }
        public ObservableCollection<Country> Countries { get; set; }
        //public ObservableCollection<Movie> Movies { get; set; }
        public MultiSelectObservableCollection<Movie> Movies { get; }

        public ActorsViewModel()
        {
            Title = "Actors";

            Actors = new ObservableCollection<Actor>();
            Countries = new ObservableCollection<Country>(App.DataBase.GetCountriesAsync().Result);//!!!!!
            base.OnPropertyChanged("Countries");

            Movies = new MultiSelectObservableCollection<Movie>();
            #region
            //Movies = new ObservableCollection<Movie>(App.DataBase.GetMoviesAsync().Result);
            //Movie movie = new Movie()
            //{
            //    Name = "123",
            //    Description = "descr"
            //};
            //Movie movie1 = new Movie()
            //{
            //    Name = "312",
            //    Description = "rcsed"
            //};
            //Movies.Add(movie);
            //Movies.Add(movie1);
            #endregion

            LoadItemsCommand = new Command(async () => await GetAllActors());

            AddActorCommand = new Command(async () => await AddActor());

            UpdateActorCommand = new Command(async () => await UpdateActor());

            DeleteActorCommand = new Command(async () => await DeleteActor());
        }

        public ICommand AddActorCommand { get; }
        public ICommand UpdateActorCommand { get; }
        public ICommand DeleteActorCommand { get; }
        public ICommand LoadItemsCommand { get; }


        public async Task GetAllActors()
        {
            List<Actor> actors = await App.DataBase.GetActorsAsync();

            //Нужно изменить ссылку объекта страна, так как для привязки нужно, чтобы они ссылались на "Countries"
            List<Actor> newActors = actors.FindAll(actor => actor.Country != null);
            newActors.ForEach(actor => actor.Country = Countries.Single(country => country.Id == actor.Country.Id));
            //

            Actors.Clear();

            foreach (var actor in actors)
            {
                Actors.Add(actor);
            }

            IsBusy = false;
        }

        public async Task AddActor()
        {
            Country selectedCountry = SelectedActor.Country;

            if (!string.IsNullOrWhiteSpace(SelectedActor.Name))
            {
                SelectedActor.Id_Country = SelectedActor.Country.Id;
                await App.DataBase.SaveActorAsync(SelectedActor);

                IsBusy = true;

                SelectedActor = new Actor();
            }

            IsBusy = false;
        }

        public async Task UpdateActor()
        {
            if (checkConditionalActor())
            {
                SelectedActor.Id_Country = SelectedActor.Country.Id;
                await DataBaseOperation(App.DataBase.UpdateActorAsync(SelectedActor));

                IsBusy = true;

                SelectedActor = new Actor();
            }

            IsBusy = false;
        }

        public async Task DeleteActor()
        {
            if (checkConditionalActor())
            {
                await DataBaseOperation(App.DataBase.DeleteActorAsync(SelectedActor));

                IsBusy = true;

                SelectedActor = new Actor();
            }

            IsBusy = false;
        }

        private async Task DataBaseOperation(Task<int> dbAction)
        {
            int countUpdatedRows = await dbAction;

            if (countUpdatedRows == 0)
            {
                await App.Current.MainPage.DisplayAlert("Message", "Не была выбрана страна", "OK");
            }
        }

        private bool checkConditionalActor()
        {
            return SelectedActor.Id != 0 && !string.IsNullOrWhiteSpace(SelectedActor.Name);
        }

        public void OnAppearing()
        {
            Countries = new ObservableCollection<Country>(App.DataBase.GetCountriesAsync().Result);//!!!!!!!!!!!!!
            base.OnPropertyChanged("Countries");

            IsBusy = true;
        }
    }
}
