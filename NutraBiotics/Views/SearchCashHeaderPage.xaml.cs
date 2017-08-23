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
    }
}