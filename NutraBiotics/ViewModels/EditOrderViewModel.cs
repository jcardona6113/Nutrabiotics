using NutraBiotics.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutraBiotics.ViewModels
{
    public class EditOrderViewModel : INotifyPropertyChanged
    {
        #region Attributes
        public ObservableCollection<GridOrderDetail> _orderDetails;
        public OrderHeader _orderHeader;
        #endregion


        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Properties

        public ObservableCollection<GridOrderDetail> OrderDetails
        {
            set
            {
                if (_orderDetails != value)
                {
                    _orderDetails = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderDetails)));
                }
            }
            get
            {
                return _orderDetails;
            }
        }


        public OrderHeader OrderHeader
        {
            set
            {
                if (_orderHeader != value)
                {
                    _orderHeader = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderHeader)));
                }
            }
            get
            {
                return _orderHeader;
            }
        }


        #endregion

        public EditOrderViewModel()
        {

        }

        #region Methods

        #endregion
    }
}
