using GalaSoft.MvvmLight.Command;
using NutraBiotics.Models;
using NutraBiotics.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NutraBiotics.ViewModels
{
    public class EditContactViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Atribbutes
        Contact _contact;
        bool isEnabled;
        bool isRunning;
        string result;
        #endregion

        #region Services
        DataService dataService;
        DialogService dialogService;
        NavigationService navigationservice;
        #endregion

        #region Properties

        public string Result
        {
            set
            {
                if (result != value)
                {
                    result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
            get
            {
                return result;
            }
        }


        public Contact Contact
        {
            set
            {
                if (_contact != value)
                {
                    _contact = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Contact)));
                }
            }
            get
            {
                return _contact;
            }
        }


        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
            get
            {
                return isEnabled;
            }
        }

        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
            get
            {
                return isRunning;
            }
        }

        #endregion

        #region constructor
        public EditContactViewModel(Contact contact)
        {
            this._contact = contact;
        }
        #endregion

        public ICommand SaveContactCommand
        {
            get { return new RelayCommand(SaveContact); }
        }
        async void SaveContact()
        {
            IsRunning = true;
            try
            {

                if (Contact.Name == string.Empty)
                {
                    await dialogService.ShowMessage(
                        "Error",
                        "Debes ingresar nombre de Contacto.");
                    return;
                }


                if (Contact.PhoneNum == string.Empty)
                {
                    await dialogService.ShowMessage(
                        "Error",
                        "Debes ingresar celular.");
                    return;
                }

                dataService.Insert<Contact>(Contact);
                IsRunning = false;
                Result = "Se actualizo el registro con exito.";


                IsRunning = false;

            }
            catch (Exception ex)
            {
                result = ex.Message;
                IsRunning = false;
            }
        }
    }
}
