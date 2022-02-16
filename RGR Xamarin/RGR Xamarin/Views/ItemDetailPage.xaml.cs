using RGR_Xamarin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace RGR_Xamarin.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}