using RGR_Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RGR_Xamarin.ViewModels
{
    internal class CountriesViewModel : BaseViewModel
    {

        private Country _selectedCountry = new Country();
        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                base.OnPropertyChanged("SelectedCountry");
            }
        }
        public ObservableCollection<Country> Countries { get; set; }

        public CountriesViewModel()
        {
            Title = "Country";

            Countries = new ObservableCollection<Country>();

            LoadItemsCommand = new Command(async () => await GetAllCountries());

            AddCountryCommand = new Command(async () => await AddCountry());

            UpdateCountryCommand = new Command(async () => await UpdateCountry());

            DeleteCountryCommand = new Command(async () => await DeleteCountry());
        }

        public ICommand AddCountryCommand { get; }
        public ICommand UpdateCountryCommand { get; }
        public ICommand DeleteCountryCommand { get; }
        public ICommand LoadItemsCommand { get; }


        public async Task GetAllCountries()
        {
            List<Country> countries = await App.DataBase.GetCountriesAsync();

            Countries.Clear();

            foreach (var country in countries)
            {
                Countries.Add(country);
            }

            IsBusy = false;
        }

        public async Task AddCountry()
        {
            if (!string.IsNullOrWhiteSpace(SelectedCountry.Name))
            {
                await App.DataBase.SaveCountriesAsync(SelectedCountry);

                IsBusy = true;

                SelectedCountry = new Country();
            }

            IsBusy = false;
        }

        public async Task UpdateCountry()
        {
            if (!string.IsNullOrWhiteSpace(SelectedCountry.Name))
            {
                await DataBaseOperation(App.DataBase.UpdateCountryAsync(SelectedCountry));

                IsBusy = true;

                SelectedCountry = new Country();
            }

            IsBusy = false;
        }

        public async Task DeleteCountry()
        {
            if (!string.IsNullOrWhiteSpace(SelectedCountry.Name))
            {
                await DataBaseOperation(App.DataBase.DeleteCountryAsync(SelectedCountry));

                IsBusy = true;

                SelectedCountry = new Country();
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

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
