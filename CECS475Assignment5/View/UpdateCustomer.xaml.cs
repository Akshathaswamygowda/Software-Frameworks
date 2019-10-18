using CECS475Assignment5.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CECS475Assignment5.View
{
    /// <summary>
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Window
    {
        public UpdateCustomer()
        {
            InitializeComponent();
        }
        /// <summary>
        /// UpdateCustomer Consrtructor
        /// </summary>
        /// <param name="selectedcustomer">Passing Selected Customer object</param>
        public UpdateCustomer(Customer selectedcustomer)
        {
            InitializeComponent();
            UpdateViewModel vm = new UpdateViewModel(selectedcustomer);
            this.DataContext = vm;
        }
    }
}
