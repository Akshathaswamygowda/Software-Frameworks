using CECS475Assignment5.Model;
using CECS475Assignment5.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CECS475Assignment5.ViewModel
{
    /// <summary>
    /// AddView for Adding Customer into Database
    /// </summary>
    class AddViewModel : ViewModelBase
    {
        private string customerId;
        private string name;
        private string address;
        private string city;
        private string zipcode;
        private Customer customer;
        private ObservableCollection<State> statelist = new ObservableCollection<State>();
        private State selectedstate;
        
        /// <summary>
        /// Addview Constructor
        /// </summary>
        public AddViewModel()
        {
            MainViewModel mv = new MainViewModel();
            LoadComboxMethod();
            SaveCustomer = new RelayCommand<Window>(AcceptCustomer);
            ExitWindow = new RelayCommand<Window>(CloseWindow);
        }
        /// <summary>
        /// Method for loading DropBox
        /// </summary>
        public void LoadComboxMethod()
        {
            try
            {
                // Query for retrieving the States from Database
                var states = (from state in MMABooksEntity.mmaBooks.States
                              orderby state.StateName
                              select state).ToList();
                Statelist = new ObservableCollection<CECS475Assignment5.State>(states);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
        }

        /// <summary>
        /// Method for Closing Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(Window obj)
        {

            if (obj != null)
            {
                obj.Close();
            }
        }

        /// <summary>
        /// Method for Adding Operation of the Customer
        /// </summary>
        /// <param name="obj">Window Parameter</param>
        private void AcceptCustomer(Window obj)
        {
            if (Name != null && City != null && Zipcode != null)
            {

                customer = new Customer();
                this.PutCustomerData(customer);
                MMABooksEntity.mmaBooks.Customers.Add(customer);
                MMABooksEntity.mmaBooks.SaveChanges();
                MessageBox.Show("Customer Added Successfully","Add Customer",MessageBoxButton.OK,MessageBoxImage.Information);
                obj.Close();
                Messenger.Default.Send(new NotificationMessage("Customer Added!"));
                Messenger.Default.Send(customer, "add");
            }
            else
            {
                MessageBox.Show("Data Fields cannot be empty");
            }
        }

        /// <summary>
        /// Method for Adding input into customer object
        /// </summary>
        /// <param name="customer">for storing input values</param>
        public void PutCustomerData(Customer customer)
        {
            customer.Name = Name;
            customer.Address = Address;
            customer.City = City;
            customer.State = Selectedstate.StateCode;
            customer.ZipCode = Zipcode;

        }

        /// <summary>
        /// ICommand for Accept Operation
        /// </summary>
        public ICommand SaveCustomer { get; private set; }

        /// <summary>
        /// ICommand for exit window
        /// </summary>
        public ICommand ExitWindow { get; private set; }

        /// <summary>
        /// Collection List Property for State
        /// </summary>
        public ObservableCollection<State> Statelist
        {
            get
            {
                return statelist;
            }
            set
            {
                statelist = value;
                RaisePropertyChanged("Statelist");
            }
        }
        /// <summary>
        /// Customer Property
        /// </summary>
        public Customer Customer
        {
            get
            {
                return customer;
            }
            set
            {
                customer = value;
                RaisePropertyChanged("Customer");
            }
        }

        /// <summary>
        /// Property for StateName 
        /// </summary>
        public State Selectedstate
        {
            get
            {
                return selectedstate;
            }
            set
            {
                selectedstate = value;
                RaisePropertyChanged("Selectedstate");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerId
        {
            get
            {
                return customerId;
            }
            set
            {
                try
                {
                    string input = value.ToString();
                    bool rt = Regex.IsMatch(input, "[0-9]+$");
                    if(input.Length==0)
                    {
                        throw new NullReferenceException("Invalid Entry");
                    }
                    if(!rt)
                    {
                        throw new Exception("Enter Valid Customer Id");
                    }
                    customerId = value;
                    RaisePropertyChanged("CustomerId");

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Customer ID", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        /// <summary>
        /// Property for Name
        /// </summary>
        public string Name
        {

            get
            {
                return name;
            }
            set
            {
                try
                {
                    string input = value.ToString();
                    bool rt = Regex.IsMatch(input, "[a-zA-Z]+$");
                    if (value.Length == 0)
                    {
                        throw new NullReferenceException("Invalid Entry");
                    }
                    if (!rt)
                    {
                        throw new Exception("Enter Valid Name");
                    }
                    name = value;
                    RaisePropertyChanged("Name");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        /// <summary>
        /// Property for Address
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
        /// Property for City
        /// </summary>
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                try
                {
                    string input = value.ToString();
                    //Validation
                    bool rt = Regex.IsMatch(input, "[a-zA-Z]+$");
                    if (value.Length == 0)
                    {
                        throw new NullReferenceException("Invalid Entry");
                    }
                    if (!rt)
                    {
                        throw new Exception("Enter Valid City Name");
                    }
                    city = value;
                    RaisePropertyChanged("City");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid City Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Property for Zipcode
        /// </summary>
        public string Zipcode
        {
            get
            {
                return zipcode;
            }
            set
            {
                try
                {
                    string input = value.ToString();
                    //Validation
                    bool rt = Regex.IsMatch(input, "[0-9]+$");
                    if (value.Length == 0)
                    {
                        throw new NullReferenceException("Invalid Entry");
                    }
                    if (!rt)
                    {
                        throw new Exception("Enter Valid Zipcode");
                    }
                    zipcode = value;
                    RaisePropertyChanged("Zipcode");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Zipcode", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    }
}
