using System;
using System.Collections.Generic;

using Xamarin.Forms;
using NutraBiotics.ViewModels;
using NutraBiotics.Models;
using System.Collections.ObjectModel;
using NutraBiotics.Services;

namespace NutraBiotics.Views
{
    public partial class NewOrderDetailPage : ContentPage
    {
        GridOrderDetail item;
        NavigationService navigationService;

        public NewOrderDetailPage()
        {
            InitializeComponent();
            navigationService = new NavigationService();
        }

         private async void lvDetails_ItemSelected(Object sender, SelectedItemChangedEventArgs e)
        {
            //envio modelo de detalle seleccionado a la pagina de edicion
            //item = (GridOrderDetail)e.SelectedItem;
            //var mainViewModel = MainViewModel.GetInstance();
            //mainViewModel.EditOrderDetails = new EditOrderDetailsViewModel();
            //mainViewModel.EditOrderDetails.RecibirDetalle(item);
            //await navigationService.Navigate("EditOrderDetailsPage");
        }

        async void EditProductCommand(object sender, EventArgs args)
        {
            //Instancio el mainviewmodel de la pagina edicion de detalle, y le envio los valores
            // de la grid seleccionada
            //instancio la pagina de edicion de detalle
            //ejecuto el push modal de la pagina
            //var editdetailVM = new EditOrderDetailsViewModel();
            //editdetailVM.RecibirDetalle(item);
            //var editdetailPage = new EditOrderDetailsPage();
            //await Navigation.PushModalAsync(editdetailPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var mainviewmodel = MainViewModel.GetInstance();
            if (mainviewmodel.EditOrderDetails != null)
            {

                if (mainviewmodel.EditOrderDetails.edito == false)
                {
                    mainviewmodel.NewOrder.RestoreDetail();
                }
            }
          
        }
    }
}
