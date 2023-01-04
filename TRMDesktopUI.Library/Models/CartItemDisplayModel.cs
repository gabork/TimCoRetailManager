using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        public ProductDisplayModel Product
        {
            get; set;
        }

        private int quantityInCart;

        public int QuantityInCart
        {
            get
            {
                return quantityInCart;
            }
            set
            {
                quantityInCart = value;
                CallPropertyChanged(nameof(QuantityInCart));
                CallPropertyChanged(nameof(DisplayText));
            }
        }

        public string DisplayText
        {
            get
            {
                return $"{Product.ProductName} ({QuantityInCart})";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
