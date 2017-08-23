﻿namespace NutraBiotics.Services
{
    using System.Threading.Tasks;
    using Views;
    using Xamarin.Forms;

	public class NavigationService
    {
        public void SetMainPage(string pageName)
        {
            switch (pageName)
            {
				case "MasterPage":
                    Application.Current.MainPage = new MasterPage();
					break;
				case "LoginPage":
					Application.Current.MainPage = new LoginPage();
					break;
			}
        }

        public async Task Navigate(string pageName)
        {
            App.Master.IsPresented = false;

            switch (pageName)
            {
				case "DownloadPage":
					await App.Navigator.PushAsync(new DownloadPage());
					break;
				case "NewOrderTab":
					await App.Navigator.PushAsync(new NewOrderTab());
					break;
				case "SearchCustomerPage":
					await App.Navigator.PushAsync(new SearchCustomerPage());
					break;
				case "SearchShipToPage":
					await App.Navigator.PushAsync(new SearchShipToPage());
					break;
				case "SearchContactPage":
					await App.Navigator.PushAsync(new SearchContactPage());
					break;

                case "SearchPriceListPartPage":
                    await App.Navigator.PushAsync(new SearchPriceListPartPage());
                    break;

                case "CustomersPage":
                    await App.Navigator.PushAsync(new CustomersPage());
                    break;

                case "EditCustomerPage":
                    await App.Navigator.PushAsync(new EditCustomerPage());
                    break;

                case "EditShipToPage":
                    await App.Navigator.PushAsync(new EditShipToPage());
                    break;

                case "EditContactPage":
                    await App.Navigator.PushAsync(new EditContactPage());
                    break;

                case "NewShipToPage":
                    await App.Navigator.PushAsync(new NewShipToPage());
                    break;

                case "NewContactPage":
                    await App.Navigator.PushAsync(new NewContactPage());
                    break;

                case "EditOrderDetailsPage":
                    await App.Navigator.PushAsync(new EditOrderDetailsPage());
                    break;

                case "TabPursePage":
                    await App.Navigator.PushAsync(new TabPursePage());
                    break;

                case "EditOrderPage":
                    await App.Navigator.PushAsync(new EditOrderPage());
                    break;

                case "PersonContactsPage":
                    await App.Navigator.PushAsync(new PersonContactsPage());
                    break;

                case "NewPersonContactPage":
                    await App.Navigator.PushAsync(new NewPersonContactPage());
                    break;

                case "UploadPage":
                    await App.Navigator.PushAsync(new UploadPage());
                    break;


                case "SearchCalendarPage":
                    await App.Navigator.PushAsync(new SearchCalendarPage());
                    break;

                case "InvoiceDetailPage":
                    await App.Navigator.PushAsync(new InvoiceDetailPage());
                    break;
            }
    }

        public async Task Back()
        {
            await App.Navigator.PopAsync();
        }
    }
}
