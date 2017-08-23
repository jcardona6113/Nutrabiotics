using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NutraBiotics.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace NutraBiotics.ViewModels
{
    public class InvoiceDetailViewModel : INotifyPropertyChanged
    {

        #region Attributes

        ObservableCollection<InvoiceDetail> _invoiceDetails;
        decimal _totallineas;

        #endregion

        #region Properties

        public ObservableCollection<InvoiceDetail> InvoiceDetails
        {
            set
            {
                if (_invoiceDetails != value)
                {
                    _invoiceDetails = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InvoiceDetails)));
                }
            }
            get
            {
                return _invoiceDetails;
            }
        }


        public decimal TotalLineas
        {
            set
            {
                if (_totallineas != value)
                {
                    _totallineas = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalLineas)));
                }
            }
            get
            {
                return _totallineas;
            }
        }

#endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor

        public InvoiceDetailViewModel(ObservableCollection<InvoiceDetail> detalle)
        {
            this._invoiceDetails = detalle;

            TotalLineas = InvoiceDetails.Sum(god => god.Value);
        }

        #endregion


        #region methods


        #endregion

    }
}

