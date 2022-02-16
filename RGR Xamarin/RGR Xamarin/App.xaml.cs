using RGR_Xamarin.Services;
using RGR_Xamarin.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RGR_Xamarin
{
    public partial class App : Application
    {
        private static DataBase _dataBase;
        internal static DataBase DataBase
        {
            get
            {
                return _dataBase ?? (_dataBase = new DataBase(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "movies.db3")
                    ));
                //return _dataBase ?? (_dataBase = new DataBase(@"C:/Users/ersho/source/repos/XamarinSQLLiteCRUDQR/XamarinSQLLiteCRUDQR/XamarinSQLLiteCRUDQR/movies.db"));
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
