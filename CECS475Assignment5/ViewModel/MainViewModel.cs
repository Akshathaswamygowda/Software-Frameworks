using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;
using CECS475Assignment5.View;
using System.Windows;
using CECS475Assignment5.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CECS475Assignment5.ViewModel
{
    /// <summary>
    /// MainViewModel Class
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        
        private ObservableCollection<CustomerModel> customer;
        private CustomerModel customers;
        private string customerId;
        private string name;
        private string address;
        private string city;
        private string state;
        private string zipcode;
        private Customer selectedcustomer;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            customers = new CustomerModel(CustomerId, Name, Address, State, City, Zipcode);
            AddCustomer = new RelayCommand(AddMethod);
            UpdateCommand = new RelayCommand(UpdateMethod);
            DeleteCommand = new RelayCommand(Deletemethod);
            ExitWindow = new RelayCommand<Window>(ExitMethod);
            GetCustomer = new RelayCommand(GetCustomerMethod);
            
            Messenger.Default.Register<Customer>(this, "add", (customer) =>
            {

                try
                {
                    selectedcustomer = customer;
                    CustomerId = selectedcustomer.CustomerID.ToString();
                    this.DisplayCustomer();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                }

            });

            //Messenger to display updated customer back to Main Window
            Messenger.Default.Register<Customer>(this, "edit", (customer) =>
            {
                try
                {
                    selectedcustomer = customer;
                    selectedcustomer.State1 = (from state in MMABooksEntity.mmaBooks.States
                                               where selectedcustomer.State == state.StateCode
                                               select state).SingleOrDefault();
                    this.DisplayCustomer();
                }
                //Update Concurrecny Handling
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                    if (MMABooksEntity.mmaBooks.Entry(customer).State == EntityState.Detached)
                    {
                        MessageBox.Show("Another user has deleted " + "that customer.", "Concurrency Error");
                        ClearControls();
                    }
                    else
                    {
                        ex.Entries.Single().Reload();
                        MessageBox.Show("Another user has updated " + "that customer.", "Concurrency Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                }

            });


        }
        /// <summary>
        /// Method to Exit
        /// </summary>
        /// <param name="obj"></param>
        private void ExitMethod(Window obj)
        {
            if (obj != null)
            {
                obj.Close();
            }
        }
        /// <summary>
        /// Method to Delete the customer from Database
        /// </summary>
        private void Deletemethod()
        {
            try
            {
                if (selectedcustomer == null)
                {
                    throw new NullReferenceException("There is No Such Record To Delete");
                }
                else
                {
                    MMABooksEntity.mmaBooks.Customers.Remove(selectedcustomer);
                    MMABooksEntity.mmaBooks.SaveChanges();
                    Messenger.Default.Send(new NotificationMessage("Customer Removed"));
                    this.ClearControls();
                }
            }
            catch(NullReferenceException ex)
            {
                ClearControls();
                MessageBox.Show(ex.Message, "No Record Found", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Concurrency handling Exception for Delete Operation
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                if (MMABooksEntity.mmaBooks.Entry(selectedcustomer).State == EntityState.Detached)
                {
                    MessageBox.Show("Another user has deleted " + "that customer.", "Concurrency Error");
                    CustomerId = "";
                    ClearControls();
                }
                else if (MMABooksEntity.mmaBooks.Entry(selectedcustomer).State == EntityState.Modified)
                { 
                    MessageBox.Show("Another user has updated " + "that customer.", "Concurrency Error");
                    DisplayCustomer();
                }
            }

            catch (DbUpdateException ex)
            {
                MessageBox.Show(ex.Message, "Update Exception");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }

        }
        /// <summary>
        /// Method for Clearing TextBox
        /// </summary>
        private void ClearControls()
        {
            CustomerId = "";
            Name = "";
            Address = "";
            City = "";
            Zipcode = "";
            State = "";
        }
        /// <summary>
        /// Update Customer Operation
        /// </summary>
        private void UpdateMethod()
        {
            if (null != selectedcustomer)
            {
                //Displays the Update Window
                UpdateCustomer update = new UpdateCustomer(selectedcustomer);
                update.Show();
            }
            else
            {
                MessageBox.Show("Please search for a customer before you" + "attempt to delete.", "No selected customer");
            }
        }
        /// <summary>
        /// GetCustomer Method
        /// </summary>
        private void GetCustomerMethod()
        {
            GetCustomerDetils(CustomerId);
        }
        /// <summary>
        /// Display Customer based on the entered Customer ID
        /// </summary>
        /// <param name="customerID">For searching in Database</param>
        public void GetCustomerDetils(string customerID)
        {
            try
            {
                int id = Convert.ToInt32(customerID);
                //Query for searching customer based on the entered customer ID
                selectedcustomer = (from customer in MMABooksEntity.mmaBooks.Customers
                                    where customer.CustomerID == id
                                    select customer).SingleOrDefault();

                //Checking if record id present in the Database
                if (selectedcustomer == null)
                {
                    MessageBox.Show("No customer found with this ID. " +
                        "Please try again.", "Customer Not Found");
                }
                else
                {

                    //Checking if State is loaded in the Textbox
                    if (!MMABooksEntity.mmaBooks.Entry(selectedcustomer).Reference("State1").IsLoaded)
                        MMABooksEntity.mmaBooks.Entry(
                       selectedcustomer).Reference("State1").Load();
                    this.DisplayCustomer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
        }
        /// <summary>
        /// To Display Add Window
        /// </summary>
        private void AddMethod()
        {
            AddCustomer add = new AddCustomer();
            add.Show();

        }
        /// <summary>
        /// Customer ID Property
        /// </summary>
        [Key]
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
        [ConcurrencyCheck]
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
        /// Customer Address Property
        /// </summary>
        [ConcurrencyCheck]
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
        /// Customer State Property
        /// </summary>
        [ConcurrencyCheck]
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
        /// Customer City Property
        /// </summary>
        [ConcurrencyCheck]
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
        /// Customer Zipcode Property
        /// </summary>
        [ConcurrencyCheck]
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
        public ICommand ExitWindow { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand AddCustomer { get; private set; }

        public ICommand UpdateCommand { get; private set; }

        public ICommand GetCustomer { get; private set; }

        /// <summary>
        /// Display Customer in the Main Window
        /// </summary>
        public void DisplayCustomer()
        {
            Name = selectedcustomer.Name;
            Address = selectedcustomer.Address;
            City = selectedcustomer.City;
            State = selectedcustomer.State1.StateName;
            Zipcode = selectedcustomer.ZipCode;
        }
        /// <summary>
        /// For Storing Data into Database
        /// </summary>
        /// <param name="customer"></param>
        public void PutCustomerData(Customer customer)
        {
            customer.Name = Name;
            customer.Address = Address;
            customer.City = City;
            customer.State = State;
            customer.ZipCode = Zipcode;
        }

    }
}