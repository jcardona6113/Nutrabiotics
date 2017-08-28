using NutraBiotics.Models;
using NutraBiotics.Services;
using NutraBiotics.ViewModels;
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
    public partial class SearchCashHeaderPage : ContentPage
    {
        DataService dataService;

        public SearchCashHeaderPage()
        {
            InitializeComponent();
            dataService = new DataService();

        }



        async Task SearchClickAsync(object sender, EventArgs args)
        {
            var searchCashHeaders = SearchCashHeaderViewModel.GetInstance();
            searchCashHeaders.Search();

            if(searchCashHeaders.CashHeaders != null && searchCashHeaders.CashHeaders.Count > 0)
            {
                var cashheaderlist = new CashHeadersListPage();
                await Navigation.PushModalAsync(cashheaderlist);
            }

        }



    }
}