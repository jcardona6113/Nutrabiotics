using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NutraBiotics.ViewModels;

namespace NutraBiotics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditShipToPage : ContentPage
    {
        public EditShipToPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var editShipTo = EditShipToViewModel.GetInstance();
         //   editShipTo.RefreshContacts();
        }
    }
}