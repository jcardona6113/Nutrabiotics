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
    public partial class SearchInvoicesPage : ContentPage
    {
        public SearchInvoicesPage()
        {
            InitializeComponent();
        }

        //async Task SearchClickAsync(object sender, EventArgs args)
        //{
        //    var SearchInvoices = SearchInvoicesViewModel.GetInstance();
        //    SearchInvoices.Search();

        //    var invoicelist = new InvoicesListPage();
        //    await Navigation.PushModalAsync(invoicelist);
        //}

    }
}