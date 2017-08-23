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
    public partial class NewContactPage : ContentPage
    {
        public NewContactPage()
        {
            InitializeComponent();
        }


        async void OnPersonContacIDcompleted(object sender, EventArgs e)
        {
            var newcontactviewmodel = NewContactViewModel.GetInstance();
            await newcontactviewmodel.PersonContactIdCompleted();
        }

    }
}