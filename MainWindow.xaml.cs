using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Inventory.View;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new LoginWindow());
        }
        public void NavigateToHomePage()
        {
            mainFrame.Navigate(new HomePage());
        }
        public void NavigateToLoginPage()
        {
            mainFrame.Navigate(new LoginWindow());
        }
        public void NavigateToManageItems()
        {
            mainFrame.Navigate(new ManageItems());
        }
        public void NavigateToContactsPage(string contactType)
        {
            Contact supplierPage = new Contact(contactType);
            ((MainWindow)Application.Current.MainWindow).mainFrame.NavigationService.Navigate(supplierPage);
        }
        public void NavigateToCustomerPage()
        {
            mainFrame.Navigate(new Customer());
        }
        public void NavigateToCatagoryPage()
        {
            mainFrame.Navigate(new Catagory());
        }
        public void NavigateToReport()
        {
            mainFrame.Navigate(new ReportPage());
        }
        public void NavigateToSales()
        {
            mainFrame.Navigate(new Sales());
        }
        public void NavigateToSalesHistory()
        {
            mainFrame.Navigate(new SalesDetails());
        }
    }    
}
