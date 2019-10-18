using CECS475Assignment5.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CECS475Assignment5.ViewModel
{
    /// <summary>
    /// UpdateView Model for Update Operation
    /// </summary>
    class UpdateViewModel : ViewModelBase
    {
        private string customerId;
        private string name;
        private string address;
        private string state;
        private string city;
        private string zipcode;
        public Customer selectedcustomer;
        CustomerModel customermodel;
        private ObservableCollection<State> statelist = new ObservableCollection<State>();
        private State selectedstate;

        /// <summary>
        /// Intializes the new Instance of UpdateViewModel
        /// </summary>
        /// <param name="selectedcustomer"></param>
        public UpdateViewModel(Customer selectedcustomer)
        {
            customermodel = new CustomerModel(CustomerId, Name, Address, City, State, Zipcode);
            LoadComboxMethod();
            this.selectedcustomer = selectedcustomer;
            UpdateCustomer = new RelayCommand<Window>(DoUpdate);
            ExitWindow = new RelayCommand<Window>(CloseWindow); 
            this.DisplayCustomer();
        }

        /// <summary>
        /// To Close Window
        /// </summary>
        /// <param name="obj">window parameter</param>

        private void CloseWindow(Window obj)
        {
            if (obj != null)
            {
                obj.Close();
            }
        }
        /// <summary>
        /// To Update the Existing Record
        /// </summary>
        /// <param name="obj">Window parameter</param>
        private void DoUpdate(Window obj)
        {
            //Query to check if the Customer Present in the Database
            Customer customer = MMABooksEntity.mmaBooks.Customers.SingleOrDefault
                (b => b.CustomerID == selectedcustomer.CustomerID);
             try
                {
                if (customer != null)
                {
                    this.PutCustomerData(customer);
                    MMABooksEntity.mmaBooks.SaveChanges();
                    obj.Close();
                    Messenger.Default.Send(new NotificationMessage("Customer Updated!"));
                    Messenger.Default.Send(customer, "edit");
                }
                else
                {
                    throw new NullReferenceException("No Record Found to Update");
                }
            }
            catch(NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "No Record Found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                if (MMABooksEntity.mmaBooks.Entry(customer).State == EntityState.Detached)
                {
                    
                    Window window = new Window();
                    MessageBox.Show("Another user has deleted " + "that customer.", "Concurrency Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    
                }
                else
                {
                    MainViewModel mv = new MainViewModel();
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    Window window = new Window();
                    MessageBox.Show("Another user has updated " + "that customer.", "Concurrency Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    obj.Close();
                    Messenger.Default.Send(customer, "edit");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
        }

        /// <summary>
        /// Collection Of State
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
        /// Selected State Name Property
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
        /// Method to Load Combobox
        /// </summary>
        public void LoadComboxMethod()
        {
            try
            {
                //Query to display State in Update Window From Database
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
        /// To save Back Data into Database
        /// </summary>
        /// <param name="customer"></param>
        public void PutCustomerData(Customer customer)
        {
            customer.Name = Name;
            customer.Address = Address;
            customer.City = City;
            customer.State = Selectedstate.StateCode;
            customer.ZipCode = Zipcode;
        }
        /// <summary>
        /// To Display in the Main Window
        /// </summary>
        public void DisplayCustomer()
        {
            Name = selectedcustomer.Name;
            Address = selectedcustomer.Address;
            City = selectedcustomer.City;
            Selectedstate = (from state in MMABooksEntity.mmaBooks.States
                             where selectedcustomer.State == state.StateCode
                             select state).SingleOrDefault();
            Zipcode = selectedcustomer.ZipCode;

        }
        public ICommand UpdateCustomer { get; private set; }
        public ICommand ExitWindow { get; private set; }

        /// <summary>
        /// Cutomer Id Property
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
        /// Customer Name Property
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                string input = value.ToString();
                bool rt = Regex.IsMatch(input, "[a-zA-Z]+$");
                try
                {
                    if(input.Length==0)
                    {
                        throw new NullReferenceException("Name Cannot Be Empty");
                    }
                    if(!rt)
                    {
                        throw new Exception("Enter Valid Name");
                    }
                    name = value;
                    RaisePropertyChanged("Name");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        /// <summary>
        /// Customer Address Property
        /// </summary>
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                string input = value.ToString();
                bool rt = Regex.IsMatch(input, "[a-zA-Z0-9]+$");
                try
                {
                    if(input.Length==0)
                    {
                        throw new NullReferenceException("Address Cannot be Empty");
                    }
                    if(!rt)
                    {
                        throw new Exception("Enter Valid Address");
                    }
                    address = value;
                    RaisePropertyChanged("Address");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Address", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        /// <summary>
        /// Customer State Property
        /// </summary>
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                string input = value.ToString();
                bool rt = Regex.IsMatch(input, "[a-zA-Z]+$");
                try
                {
                    if (input.Length == 0)
                    {
                        throw new NullReferenceException("State Cannot be Empty");
                    }
                    if (!rt)
                    {
                        throw new Exception("Enter Valid State");
                    }
                    state = value;
                    RaisePropertyChanged("State");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid State", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                    bool rt = Regex.IsMatch(input, "[a-zA-Z]+$");
                    if (input.Length == 0)
                    {
                        throw new NullReferenceException("City Cannot be Empty");
                    }
                    if (!rt)
                    {
                        throw new Exception("Enter Valid City");
                    }
                    city = value;
                    RaisePropertyChanged("City");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid City Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Zipcode Property
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
                    //bool rt = Regex.IsMatch(input, "[0-9]+$");
                    if (input.Length == 0)
                    {
                        throw new NullReferenceException("Zipcode Cannot be Empty");
                    }
                    //if (!rt)
                    //{
                    //    throw new Exception("Enter Valid Zipcode");
                    //}
                    zipcode = value;
                    RaisePropertyChanged("Zipcode");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Zipcode", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
