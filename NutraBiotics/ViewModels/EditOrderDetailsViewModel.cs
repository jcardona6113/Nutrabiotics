namespace NutraBiotics.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using NutraBiotics.Models;
    using NutraBiotics.Services;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    public class EditOrderDetailsViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        public bool edito;
        GridOrderDetail _gridorderdetail;
        #endregion

        #region Properties

        public GridOrderDetail GridOrderDetail
        {
            set
            {
                if (_gridorderdetail != value)
                {
                    _gridorderdetail = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GridOrderDetail)));
                }
            }
            get
            {
                return _gridorderdetail;
            }
        }

        public bool Edito
        {
            set
            {
                if (edito != value)
                {
                    edito = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edito)));
                }
            }
            get
            {
                return edito;
            }
        }


        #endregion

        #region Servicios
        NavigationService navigationService;
        DialogService dialogService;
        #endregion

        #region Constructor
        public EditOrderDetailsViewModel()
        {
   
            navigationService = new NavigationService();
            dialogService = new DialogService();
        }

        #endregion

        #region Methods

        public void RecibirDetalle(GridOrderDetail preEditDetail)
        {
            this.GridOrderDetail = preEditDetail;
        }

        #endregion

        #region Commands

        public ICommand SaveChangesCommand
        {
            get { return new RelayCommand(SaveChanges); }
        }

        async void SaveChanges()
        {
            edito = true;

            try
            {              
                var mainviewmodel = MainViewModel.GetInstance();
                mainviewmodel.NewOrder.PartEdited(_gridorderdetail);
                await navigationService.Back();
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        #endregion
    }
}
    

