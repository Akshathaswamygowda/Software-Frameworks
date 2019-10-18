
using CECS475Assignment5.ViewModel;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CECS475Assignment5.Model
{
    /// <summary>
    /// CustomerModel Class
    /// </summary>
    public class CustomerModel : ObservableObject
    {
        
        private string customerId;
        private string name;
        private string address;
        private string city;
        private string state;
        private string zipcode;
        /// <summary>
        /// CustomerModel Constructor
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zipcode"></param>
        public CustomerModel(string customerId, string name, string address, string city, string state, string zipcode)
        {
            this.CustomerId = customerId;
            this.name = name;
            this.address = address;
            this.city = city;
            this.state = state;
            this.zipcode = zipcode;
        }

       /// <summary>
       /// Customer ID property
       /// </summary>
        public string CustomerId
        {
            get
            {
                return customerId;
            }
            set
            {
                customerId = value;
                RaisePropertyChanged("CustomerId");
            }
        }
        /// <summary>
        /// Name of Customer
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        /// <summary>
        /// Adress of the Customer
        /// </summary>
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                RaisePropertyChanged("Address");
            }
        }
        /// <summary>
        /// City Name
        /// </summary>
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
                RaisePropertyChanged("City");
            }
        }
        /// <summary>
        /// State Name
        /// </summary>
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }

        /// <summary>
        /// Zipcode Number
        /// </summary>
        public string Zipcode
        {
            get
            {
                return zipcode;
            }

            set
            {
                zipcode = value;
                RaisePropertyChanged("Zipcode");
            }
        }

    }
    
}
