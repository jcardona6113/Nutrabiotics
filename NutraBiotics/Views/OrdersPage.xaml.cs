﻿namespace NutraBiotics.Views
{
    using System;
	using System.Collections.Generic;
	using Xamarin.Forms;
    using ViewModels;

	public partial class OrdersPage : ContentPage
    {
    
        public OrdersPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var ordersViewModel = OrdersViewModel.GetInstance();
            ordersViewModel.RefreshOrders();
		}


        void RefreshList(object sender, EventArgs args)
        {
            var ordersViewModel = OrdersViewModel.GetInstance();
            if (ordersViewModel.OrderDate == null) { ordersViewModel.OrderDate = DateTime.Now; }
            ordersViewModel.RefreshOrders();
        }
    }
}
