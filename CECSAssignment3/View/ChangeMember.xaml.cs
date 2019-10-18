using CECSAssignment3.Model;
using CECSAssignment3.ViewModel;
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

namespace CECSAssignment3.View
{
    /// <summary>
    /// Interaction logic for ChangeMember.xaml
    /// </summary>
    public partial class ChangeMember : Window, ICloseable
    {
        Window window = new Window();
        public ChangeMember()
        {
            InitializeComponent();
            this.DataContext = new UpdateViewModel();
        }

        public ChangeMember(Member m)
        {
           
            //m = new Member(m.Firstname, m.Lastname, m.Email);
            InitializeComponent();
            this.DataContext = new UpdateViewModel();
            var viewmodel = (UpdateViewModel)DataContext;
            viewmodel.SetProperties(m);
        }
        void Close()
        {
            window.Close();
        }
    }
}
